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
	public class UIActions : LongLivingMarshalByRefObject, NUnit.Core.EventListener, UIEvents
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
//		private TestDomain testDomain = null;

		/// <summary>
		/// Our current test project, if we have one.
		/// </summary>
		private NUnitProject testProject = null;

		/// <summary>
		/// Set to true to indicate the user loaded
		/// an assembly rather than a project. 
		/// </summary>
		private bool isAssembly = false;

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

		public UITestNode LoadedTest
		{
			get { return loadedTest; }
		}

		public string TestFileName
		{
			get { return testFileName; }
		}

		public bool IsTestRunning
		{
			get { return runningTest != null; }
		}

		public bool IsProject
		{
			get { return !isAssembly; }
		}

		public bool IsAssembly
		{
			get { return isAssembly; }
		}

		public NUnitProject TestProject
		{
			get { return testProject; }
		}

		public TestDomain TestDomain
		{
			get { return TestProject.TestDomain; }
			set { TestProject.TestDomain = value; }
		}

		public string ActiveConfig
		{
			get { return IsProject ? testProject.ActiveConfig.Name : null; }
			set
			{
				if ( IsProject && testProject.ActiveConfig.Name != value )
				{
					testProject.SetActiveConfig( value );
					testProject.Save();
					LoadTest( TestFileName );
				}
			}
		}

		public string ProjectPath
		{
			get { return testProject.ProjectPath; }
		}

		public TestResult LastResult
		{
			get { return lastResult; }
		}

		public bool IsReloadPending
		{
			get { return reloadPending; }
		}

		#endregion

		#region EventListener Handlers

		/// <summary>
		/// Trigger event when each test starts
		/// </summary>
		/// <param name="testCase">TestCase that is starting</param>
		public void TestStarted(NUnit.Core.TestCase testCase)
		{
			if ( TestStartingEvent != null )
				TestStartingEvent( this, new TestEventArgs( TestAction.TestStarting, testCase ) );
		}

		/// <summary>
		/// Trigger event when each test finishes
		/// </summary>
		/// <param name="result">Result of the case that finished</param>
		public void TestFinished(TestCaseResult result)
		{
			if ( TestFinishedEvent != null )
				TestFinishedEvent( this, new TestEventArgs( TestAction.TestFinished, result ) );
		}

		/// <summary>
		/// Trigger event when each suite starts
		/// </summary>
		/// <param name="suite">Suite that is starting</param>
		public void SuiteStarted(TestSuite suite)
		{
			if ( SuiteStartingEvent != null )
				SuiteStartingEvent( this, new TestEventArgs( TestAction.SuiteStarting, suite ) );
		}

		/// <summary>
		/// Trigger event when each suite finishes
		/// </summary>
		/// <param name="result">Result of the suite that finished</param>
		public void SuiteFinished(TestSuiteResult result)
		{
			if ( SuiteFinishedEvent != null )
				SuiteFinishedEvent( this, new TestEventArgs( TestAction.SuiteFinished, result ) );
		}

		#endregion

		#region Methods

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
			if ( RunStartingEvent != null )
				RunStartingEvent( this, new TestEventArgs( TestAction.RunStarting, runningTest ) );

			try
			{
				TestDomain.TestName = runningTest.FullName;
				lastResult = TestDomain.Run(this);
				
				if ( RunFinishedEvent != null )
					RunFinishedEvent( this, new TestEventArgs( TestAction.RunFinished, lastResult ) );
			}
			catch( Exception exception )
			{
				if ( RunFinishedEvent != null )
					RunFinishedEvent( this, new TestEventArgs( TestAction.RunFinished, exception ) );
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

		public void LoadTest( string newFileName )
		{
			try
			{
				if ( LoadStartingEvent != null )
					LoadStartingEvent( this, new TestLoadEventArgs( TestLoadAction.LoadStarting, newFileName ) );

				// See if there is a test project
				NUnitProject newProject = GetTestProject( newFileName );			

				// If loading same one, unload first
				if ( IsTestLoaded && TestFileName == newFileName )
					UnloadTest();

				// Make sure it all works before switching old one out
				TestDomain newDomain = new TestDomain(stdOutWriter, stdErrWriter);

				UITestNode newTest = this.IsAssembly
					? newDomain.Load( newFileName )
					: newDomain.Load( newFileName, newProject.ActiveConfig.Assemblies );
				
				// If we didn't unload earlier, do it now
				if  ( IsTestLoaded ) 
					UnloadTest();

				// We're cool, so swap in the new info
				testProject = newProject;
				testProject.TestDomain = newDomain;
				testFileName = this.IsAssembly ? newFileName : newProject.ProjectPath;
				loadedTest = newTest;
				lastResult = null;
				reloadPending = false;
				
				// Cancel any pending reload
				reloadPending = false;

				// ToDo - figure out how to handle
				SetWorkingDirectory( this.IsAssembly
					? newFileName
					: newProject.ActiveConfig.Assemblies[0] );

				if ( LoadCompleteEvent != null )
					LoadCompleteEvent( this, new TestLoadEventArgs( TestLoadAction.LoadComplete, TestFileName, LoadedTest ) );

				if ( UserSettings.Options.ReloadOnChange )
					InstallWatcher( testFileName );
			}
			catch( Exception exception )
			{
				if ( LoadFailedEvent != null )
					LoadFailedEvent( this, new TestLoadEventArgs( TestLoadAction.LoadFailed, newFileName, exception ) );
			}
		}

		/// <summary>
		/// Unload the current test suite and fire the Unloaded event
		/// </summary>
		public void UnloadTest( )
		{
			if( IsTestLoaded )
			{
				if ( UnloadStartingEvent != null )
					UnloadStartingEvent( this, new TestLoadEventArgs( TestLoadAction.UnloadStarting, TestFileName, LoadedTest ) );

				// Hold the name for notifications after unload
				string fileName = TestFileName;

				TestDomain.Unload();

				TestDomain = null;
				testFileName = null;
				//testProject = null;
				loadedTest = null;
				lastResult = null;
				reloadPending = false;

				RemoveWatcher();

				if ( UnloadCompleteEvent != null )
					UnloadCompleteEvent( this, new TestLoadEventArgs( TestLoadAction.UnloadComplete, fileName, LoadedTest ) );
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
					if ( ReloadStartingEvent != null )
						ReloadStartingEvent( this, new TestLoadEventArgs( TestLoadAction.ReloadStarting, TestFileName, loadedTest ) );

					// Don't unload the old domain till after the event
					// handlers get a chance to compare the trees.
					TestDomain newDomain = new TestDomain(stdOutWriter, stdErrWriter);
					UITestNode newTest = newDomain.Load( testFileName );

					bool notifyClient = !UIHelper.CompareTree( LoadedTest, newTest );

					TestDomain.Unload();

					TestDomain = newDomain;
					loadedTest = newTest;
					reloadPending = false;

					if ( notifyClient && ReloadCompleteEvent != null )
						ReloadCompleteEvent( this, new TestLoadEventArgs( TestLoadAction.ReloadComplete, TestFileName, newTest ) );
				
				}
				catch( Exception exception )
				{
					if ( ReloadFailedEvent != null )
						ReloadFailedEvent( this, new TestLoadEventArgs( TestLoadAction.LoadFailed, testFileName, exception ) );
				}
		}

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
		private void InstallWatcher(string assemblyFileName)
		{
			if(watcher!=null) watcher.Stop();

			FileInfo info = new FileInfo(assemblyFileName);
			watcher = new AssemblyWatcher(1000, info);
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

		/// <summary>
		/// Helper returns a test project if there is one,
		/// creates one for VS files, returns null otherwise.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private NUnitProject GetTestProject( string path )
		{
			NUnitProject project = null;
			this.isAssembly = false; // Assume a project

			if ( NUnitProject.IsProjectFile( path ) )
				project = new NUnitProject( path );
			else
			{
				string projectPath = NUnitProject.ProjectPathFromFile( path );
				// ToDo: Warn in this case?
				if ( File.Exists( projectPath ) )
					project = new NUnitProject( projectPath );
				else if ( VSProject.IsProjectFile( path ) )
				{
					project = NUnitProject.FromVSProject( path );
					project.Save( projectPath );
				}
				else if ( VSProject.IsSolutionFile( path ) )
				{
					project = NUnitProject.FromVSSolution( path );
					project.Save( projectPath );
				}
				else
				{
					project = NUnitProject.FromAssembly( path );
					this.isAssembly = true;
					// No automatic save for assemblies
				}
			}

			return project;
		}

		#endregion
	}
}
