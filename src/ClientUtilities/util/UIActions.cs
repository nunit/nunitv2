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
		/// Loads an assembly and executes tests.
		/// </summary>
		private TestDomain testDomain = null;

		/// <summary>
		/// The currently loaded test, returned by the testrunner
		/// </summary>
		private UITestNode currentTest = null;

		/// <summary>
		/// The test that is running
		/// </summary>
		private UITestNode runningTest = null;

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

		public bool IsAssemblyLoaded
		{
			get { return testDomain != null; }
		}

		public string LoadedAssembly
		{
			get { return testDomain == null ? null : testDomain.AssemblyName; }
		}

		public bool IsTestRunning
		{
			get { return runningTest != null; }
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
				testDomain.TestName = runningTest.FullName;
				TestResult result = testDomain.Run(this);
				
				if ( RunFinishedEvent != null )
					RunFinishedEvent( this, new TestEventArgs( TestAction.RunFinished, result ) );
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

		/// <summary>
		/// Load an assembly, firing the SuiteLoaded event.
		/// </summary>
		/// <param name="assemblyFileName">Assembly to be loaded</param>
		public void LoadAssembly( string assemblyFileName )
		{
			try
			{
				if ( LoadStartingEvent != null )
					LoadStartingEvent( this, new TestLoadEventArgs( TestLoadAction.LoadStarting, assemblyFileName ) );

				// Make sure it all works before switching old one out
				NUnit.Framework.TestDomain newDomain = new NUnit.Framework.TestDomain(stdOutWriter, stdErrWriter);
				Test newTest = newDomain.Load(assemblyFileName);
				
				if  ( IsAssemblyLoaded ) UnloadAssembly();

				testDomain = newDomain;
				currentTest = newTest;
				reloadPending = false;

				SetWorkingDirectory(assemblyFileName);

				if ( LoadCompleteEvent != null )
					LoadCompleteEvent( this, new TestLoadEventArgs( TestLoadAction.LoadComplete, assemblyFileName, currentTest ) );

				InstallWatcher( assemblyFileName );
			}
			catch( Exception exception )
			{
				if ( LoadFailedEvent != null )
					LoadFailedEvent( this, new TestLoadEventArgs( TestLoadAction.LoadFailed, assemblyFileName, exception ) );
			}
		}

		/// <summary>
		/// Unload the current assembly and fire the SuiteUnloaded event
		/// </summary>
		public void UnloadAssembly( )
		{
			if(testDomain != null)
			{
				string assemblyName = LoadedAssembly;

				if ( UnloadStartingEvent != null )
					UnloadStartingEvent( this, new TestLoadEventArgs( TestLoadAction.UnloadStarting, assemblyName, currentTest ) );

				testDomain.Unload();
				testDomain = null;
				reloadPending = false;

				RemoveWatcher();

				if ( UnloadCompleteEvent != null )
					UnloadCompleteEvent( this, new TestLoadEventArgs( TestLoadAction.UnloadComplete, assemblyName, currentTest ) );
			}
		}

		/// <summary>
		/// Reload the current assembly on command
		/// </summary>
		public void ReloadAssembly()
		{
			OnAssemblyChanged( testDomain.AssemblyName );
		}

		/// <summary>
		/// Handle watcher event that signals when the loaded assembly
		/// file has changed. Make sure it's a real change before
		/// firing the SuiteChangedEvent. Since this all happens
		/// asynchronously, we use an event to let ui components
		/// know that the failure happened.
		/// </summary>
		/// <param name="assemblyFileName">Assembly file that changed</param>
		public void OnAssemblyChanged( string assemblyFileName )
		{
			if ( IsTestRunning )
				reloadPending = true;
			else 
				try
				{
					if ( ReloadStartingEvent != null )
						ReloadStartingEvent( this, new TestLoadEventArgs( TestLoadAction.ReloadStarting, LoadedAssembly, currentTest ) );

					// Don't unload the old domain till after the event
					// handlers get a chance to compare the trees.
					TestDomain newDomain = new TestDomain(stdOutWriter, stdErrWriter);
					UITestNode newTest = newDomain.Load(assemblyFileName);

					bool notifyClient = !UIHelper.CompareTree( currentTest, newTest );

					testDomain.Unload();

					testDomain = newDomain;
					currentTest = newTest;
					reloadPending = false;

					if ( notifyClient && ReloadCompleteEvent != null )
						ReloadCompleteEvent( this, new TestLoadEventArgs( TestLoadAction.ReloadComplete, LoadedAssembly, newTest ) );
				
				}
				catch( Exception exception )
				{
					if ( ReloadFailedEvent != null )
						ReloadFailedEvent( this, new TestLoadEventArgs( TestLoadAction.LoadFailed, assemblyFileName, exception ) );
				}
		}

		private static void SetWorkingDirectory(string assemblyFileName)
		{
			FileInfo info = new FileInfo(assemblyFileName);
			Directory.SetCurrentDirectory(info.DirectoryName);
		}
		
		/// <summary>
		/// Install our watcher object so as to get notifications
		/// about changes to an assembly.
		/// </summary>
		/// <param name="assemblyFileName">Full path of the assembly to watch</param>
		private void InstallWatcher(string assemblyFileName)
		{
			if(watcher!=null) watcher.Stop();

			FileInfo info = new FileInfo(assemblyFileName);
			watcher = new AssemblyWatcher(1000, info);
			watcher.AssemblyChangedEvent += new AssemblyWatcher.AssemblyChangedHandler( OnAssemblyChanged );
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
	}
}

