#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.UiKit
{
	using System;
	using System.IO;
	using System.Threading;
	using NUnit.Core;
	using NUnit.Util;
	using NUnit.Framework;


	/// <summary>
	/// UIActions handles interactions between a test runner and a client
	/// program - typically the user interface. It implemements the
	/// EventListener interface which is used by the test runner and the
	/// UIEvents interface in order to provide the information to any 
	/// component that is interested. See comments in the definition
	/// of UIEvents for more detailed information.
	/// 
	/// UIActions also provides a set of methods which allow the elements
	/// in the UI to control loading, unloading and running of tests.
	/// </summary>
	public class UIActions : LongLivingMarshalByRefObject, NUnit.Core.EventListener, ITestLoader
	{
		#region Instance Variables

		/// <summary>
		/// StdOut stream for use by the TestRunner
		/// </summary>
		private TextWriter stdOutWriter;

		/// <summary>
		/// StdErr stream for use by the TestRunner
		/// </summary>
		private TextWriter stdErrWriter;

		/// <summary>
		/// Loads and executes tests. Non-null when
		/// we have loaded a test.
		/// </summary>
		private TestDomain testDomain = null;

		/// <summary>
		/// Our current test project, if we have one.
		/// </summary>
		private NUnitProject testProject = null;

		/// <summary>
		///  The file name of the currently loaded test.
		///  This will be a project name if we loaded
		///  a project, or an assembly name if not.
		///  In the first case, testProject will be
		///  non-null, in the second, null.
		/// </summary>
		private string testFileName;

		/// <summary>
		/// The currently loaded test, returned by the testrunner
		/// </summary>
		private UITestNode loadedTest = null;

		/// <summary>
		/// The test that is running
		/// </summary>
		private UITestNode runningTest = null;

		/// <summary>
		/// Result of the last test run
		/// </summary>
		private TestResult lastResult = null;

		/// <summary>
		/// The thread that is running a test
		/// </summary>
		private Thread runningThread = null;

		/// <summary>
		/// Watcher fires when the assembly changes
		/// </summary>
		private AssemblyWatcher watcher;

		/// <summary>
		/// Assembly changed during a test and
		/// needs to be reloaded later
		/// </summary>
		private bool reloadPending = false;

		#endregion

		#region Events

		public event TestLoadEventHandler LoadStartingEvent;	
		public event TestLoadEventHandler LoadCompleteEvent;	
		public event TestLoadEventHandler LoadFailedEvent;

		public event TestLoadEventHandler ReloadStartingEvent;
		public event TestLoadEventHandler ReloadCompleteEvent;
		public event TestLoadEventHandler ReloadFailedEvent;

		public event TestLoadEventHandler UnloadStartingEvent;
		public event TestLoadEventHandler UnloadCompleteEvent;
		public event TestLoadEventHandler UnloadFailedEvent;

		public event TestEventHandler RunStartingEvent;	
		public event TestEventHandler RunFinishedEvent;
		
		public event TestEventHandler SuiteStartingEvent;
		public event TestEventHandler SuiteFinishedEvent;

		public event TestEventHandler TestStartingEvent;
		public event TestEventHandler TestFinishedEvent;

		#endregion

		#region Constructor

		public UIActions(TextWriter stdOutWriter, TextWriter stdErrWriter)
		{
			this.stdOutWriter = stdOutWriter;
			this.stdErrWriter = stdErrWriter;
		}

		#endregion

		#region Properties

		public bool IsTestLoaded
		{
			get { return loadedTest != null; }
		}

		public bool IsReloadPending
		{
			get { return reloadPending; }
		}

		public bool IsTestRunning
		{
			get { return runningTest != null; }
		}

		public NUnitProject TestProject
		{
			get { return testProject; }
		}

		public string ActiveConfig
		{
			get { return TestProject.ActiveConfig.Name; }
			set
			{
				if ( TestProject.ActiveConfig.Name != value )
				{
					TestProject.SetActiveConfig( value );
					TestProject.Save();
					LoadTest( testFileName );
				}
			}
		}

		public TestResult LastResult
		{
			get { return lastResult; }
		}

		#endregion

		#region EventListener Handlers

		/// <summary>
		/// Trigger event when each test starts
		/// </summary>
		/// <param name="testCase">TestCase that is starting</param>
		public void TestStarted(NUnit.Core.TestCase testCase)
		{
			FireTestStartingEvent( testCase );
		}

		/// <summary>
		/// Trigger event when each test finishes
		/// </summary>
		/// <param name="result">Result of the case that finished</param>
		public void TestFinished(TestCaseResult result)
		{
			FireTestFinishedEvent( result );
		}

		/// <summary>
		/// Trigger event when each suite starts
		/// </summary>
		/// <param name="suite">Suite that is starting</param>
		public void SuiteStarted(TestSuite suite)
		{
			FireSuiteStartingEvent( suite );
		}

		/// <summary>
		/// Trigger event when each suite finishes
		/// </summary>
		/// <param name="result">Result of the suite that finished</param>
		public void SuiteFinished(TestSuiteResult result)
		{
			FireSuiteFinishedEvent( result );
		}

		#endregion

		#region Methods for Running Tests

		/// <summary>
		/// Run a testcase or testsuite from the currrent tree
		/// firing the RunStarting and RunFinished events.
		/// Silently ignore the call if a test is running
		/// to allow for latency in the UI.
		/// </summary>
		/// <param name="test">Test to be run</param>
		public void RunTestSuite( UITestNode testInfo )
		{
			if ( !IsTestRunning )
			{
				runningTest = testInfo;
				runningThread = new Thread( new ThreadStart( this.TestRunThreadProc ) );
				runningThread.Start();
			}
		}

		/// <summary>
		/// The thread proc for our actual test run
		/// </summary>
		private void TestRunThreadProc()
		{
			FireRunStartingEvent( runningTest );

			try
			{
				testDomain.TestName = runningTest.FullName;
				lastResult = testDomain.Run(this);
				
				FireRunFinishedEvent( lastResult );
			}
			catch( Exception exception )
			{
				FireRunFinishedEvent( exception );
			}
			finally
			{
				runningTest = null;
				runningThread = null;
			}
		}

		/// <summary>
		/// Cancel the currently running test.
		/// Fail silently if there is none to
		/// allow for latency in the UI.
		/// </summary>
		public void CancelTestRun()
		{
			if ( IsTestRunning )
			{
				runningThread.Abort();
				runningThread.Join();
			}
		}

		#endregion

		#region Methods for Loading and Unloading Tests

		public void LoadTest( string newFileName )
		{
			try
			{
				FireLoadStartingEvent( newFileName );

				// See if there is a test project
				NUnitProject newProject = NUnitProject.MakeProject( newFileName );			

				// If loading same one, unload first
				if ( IsTestLoaded && testFileName == newFileName )
					UnloadTest();

				// Make sure it all works before switching old one out
				TestDomain newDomain = new TestDomain(stdOutWriter, stdErrWriter);

				UITestNode newTest = newProject.IsWrapper
					? newDomain.Load( newFileName )
					: newDomain.Load( newFileName, newProject.ActiveConfig.Assemblies );
				
				// If we didn't unload earlier, do it now
				if  ( IsTestLoaded ) 
					UnloadTest();

				// We're cool, so swap in the new info
				testProject = newProject;
				testDomain = newDomain;
				testFileName = testProject.IsWrapper 
					? newFileName 
					: newProject.ProjectPath;
				loadedTest = newTest;
				lastResult = null;
				reloadPending = false;
				
				// Cancel any pending reload
				reloadPending = false;

				// TODO: Figure out how to handle relative paths in tests
				SetWorkingDirectory( TestProject.IsWrapper
					? newFileName
					: newProject.ActiveConfig.Assemblies[0] );

				FireLoadCompleteEvent( testFileName, this.loadedTest );

				if ( UserSettings.Options.ReloadOnChange )
					InstallWatcher( );
			}
			catch( Exception exception )
			{
				FireLoadFailedEvent( newFileName, exception );
			}
		}

		/// <summary>
		/// Unload the current test suite and fire the Unloaded event
		/// </summary>
		public void UnloadTest( )
		{
			if( IsTestLoaded )
			{
				// Hold the name for notifications after unload
				string fileName = testFileName;

				try
				{
					FireUnloadStartingEvent( testFileName, this.loadedTest );

					RemoveWatcher();

					testDomain.Unload();

					testDomain = null;
					testFileName = null;
					//testProject = null;
					loadedTest = null;
					lastResult = null;
					reloadPending = false;

					FireUnloadCompleteEvent( fileName, this.loadedTest );
				}
				catch( Exception exception )
				{
					FireUnloadFailedEvent( fileName, exception );
				}
			}
		}

		/// <summary>
		/// Reload the current test on command
		/// </summary>
		public void ReloadTest()
		{
			OnTestChanged( testFileName );
		}

		/// <summary>
		/// Handle watcher event that signals when the loaded assembly
		/// file has changed. Make sure it's a real change before
		/// firing the SuiteChangedEvent. Since this all happens
		/// asynchronously, we use an event to let ui components
		/// know that the failure happened.
		/// </summary>
		/// <param name="assemblyFileName">Assembly file that changed</param>
		public void OnTestChanged( string testFileName )
		{
			if ( IsTestRunning )
				reloadPending = true;
			else 
				try
				{
					FireReloadStartingEvent( testFileName, this.loadedTest );

					// Don't unload the old domain till after the event
					// handlers get a chance to compare the trees.
					TestDomain newDomain = new TestDomain(stdOutWriter, stdErrWriter);
					UITestNode newTest = newDomain.Load( testFileName );

					bool notifyClient = !UIHelper.CompareTree( this.loadedTest, newTest );

					testDomain.Unload();

					testDomain = newDomain;
					loadedTest = newTest;
					reloadPending = false;

					if ( notifyClient )
						FireReloadCompleteEvent( testFileName, newTest );
				
				}
				catch( Exception exception )
				{
					FireReloadFailedEvent( testFileName, exception );
				}
		}

		#endregion

		#region Helper Methods

		private static void SetWorkingDirectory(string testFileName)
		{
			FileInfo info = new FileInfo(testFileName);
			Directory.SetCurrentDirectory(info.DirectoryName);
		}
		
		/// <summary>
		/// Install our watcher object so as to get notifications
		/// about changes to a test.
		/// </summary>
		/// <param name="assemblyFileName">Full path of the assembly to watch</param>
		private void InstallWatcher()
		{
			if(watcher!=null) watcher.Stop();

			watcher = new AssemblyWatcher( 1000, TestProject.ActiveConfig.Assemblies );
			watcher.AssemblyChangedEvent += new AssemblyWatcher.AssemblyChangedHandler( OnTestChanged );
			watcher.Start();
		}

		/// <summary>
		/// Stop and remove our current watcher object.
		/// </summary>
		private void RemoveWatcher()
		{
			if ( watcher != null )
			{
				watcher.Stop();
				watcher = null;
			}
		}

		#endregion

		#region Methods for Firing Events
		
		/// <summary>
		/// Generic routines called by other firing routines
		/// </summary>
		private void FireEvent( 
			TestLoadEventHandler handler, TestLoadEventArgs e )
		{
			if ( handler != null )
				handler( this, e );
		}

		private void FireEvent( 
			TestEventHandler handler, TestEventArgs e )
		{
			if ( handler != null )
				handler( this, e );
		}

		private void FireLoadStartingEvent( string fileName )
		{
			FireEvent( 
				LoadStartingEvent,
				new TestLoadEventArgs( TestLoadAction.LoadStarting, fileName ) );
		}

		private void FireLoadCompleteEvent( string fileName, UITestNode test )
		{
			FireEvent( 
				LoadCompleteEvent,
				new TestLoadEventArgs( TestLoadAction.LoadComplete, fileName, test ) );

		}

		private void FireLoadFailedEvent( string fileName, Exception exception )
		{
			FireEvent(
				LoadFailedEvent,
				new TestLoadEventArgs( TestLoadAction.LoadFailed, fileName, exception ) );
		}

		private void FireUnloadStartingEvent( string fileName, UITestNode test )
		{
			FireEvent(
				UnloadStartingEvent,
				new TestLoadEventArgs( TestLoadAction.UnloadStarting, fileName, test ) );
		}

		private void FireUnloadCompleteEvent( string fileName, UITestNode test )
		{
			FireEvent(
				UnloadCompleteEvent,
				new TestLoadEventArgs( TestLoadAction.UnloadComplete, fileName, test ) );
		}

		private void FireUnloadFailedEvent( string fileName, Exception exception )
		{
			FireEvent(
				UnloadFailedEvent, 
				new TestLoadEventArgs( TestLoadAction.UnloadFailed, fileName, exception ) );
		}

		private void FireReloadStartingEvent( string fileName, UITestNode test )
		{
			FireEvent(
				ReloadStartingEvent,
				new TestLoadEventArgs( TestLoadAction.ReloadStarting, fileName, test ) );
		}

		private void FireReloadCompleteEvent( string fileName, UITestNode test )
		{
			FireEvent(
				ReloadCompleteEvent,
				new TestLoadEventArgs( TestLoadAction.ReloadComplete, fileName, test ) );
		}

		private void FireReloadFailedEvent( string fileName, Exception exception )
		{
			FireEvent(
				ReloadFailedEvent, 
				new TestLoadEventArgs( TestLoadAction.ReloadFailed, fileName, exception ) );
		}

		private void FireRunStartingEvent( UITestNode test )
		{
			FireEvent(
				RunStartingEvent,
				new TestEventArgs( TestAction.RunStarting, test ) );
		}

		private void FireRunFinishedEvent( TestResult result )
		{	
			FireEvent(
				RunFinishedEvent,
				new TestEventArgs( TestAction.RunFinished, result ) );
		}

		private void FireRunFinishedEvent( Exception exception )
		{
			FireEvent(
				RunFinishedEvent,
				new TestEventArgs( TestAction.RunFinished, exception ) );
		}

		private void FireTestStartingEvent( UITestNode test )
		{
			FireEvent(
				TestStartingEvent,
				new TestEventArgs( TestAction.TestStarting, test ) );
		}

		private void FireTestFinishedEvent( TestResult result )
		{	
			FireEvent(
				TestFinishedEvent,
				new TestEventArgs( TestAction.TestFinished, result ) );
		}

		private void FireSuiteStartingEvent( UITestNode test )
		{
			FireEvent(
				SuiteStartingEvent,
				new TestEventArgs( TestAction.SuiteStarting, test ) );
		}

		private void FireSuiteFinishedEvent( TestResult result )
		{	
			FireEvent(
				SuiteFinishedEvent,
				new TestEventArgs( TestAction.SuiteFinished, result ) );
		}

		#endregion
	}
}
