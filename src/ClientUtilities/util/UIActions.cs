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
	public class UIActions : MarshalByRefObject, NUnit.Core.EventListener, UIEvents
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
		/// Watcher fires when the assembly changes
		/// </summary>
		private AssemblyWatcher watcher;

		#endregion

		#region Events

		public event TestSuiteLoadedHandler TestSuiteLoadedEvent;
		
		public event TestSuiteChangedHandler TestSuiteChangedEvent;
		
		public event TestSuiteUnloadedHandler TestSuiteUnloadedEvent;
		
		public event TestSuiteLoadFailureHandler TestSuiteLoadFailureEvent;

		public event RunStartingHandler RunStartingEvent;
		
		public event SuiteStartedHandler SuiteStartedEvent;
		
		public event TestStartedHandler TestStartedEvent;
		
		public event RunFinishedHandler RunFinishedEvent;
		
		public event SuiteFinishedHandler SuiteFinishedEvent;
		
		public event TestFinishedHandler TestFinishedEvent;

		#endregion

		#region Constructor

		public UIActions(TextWriter stdOutWriter, TextWriter stdErrWriter)
		{
			this.stdOutWriter = stdOutWriter;
			this.stdErrWriter = stdErrWriter;
		}

		#endregion

		#region Properties

		public bool AssemblyLoaded
		{
			get { return testDomain != null; }
		}

		public string LoadedAssembly
		{
			get { return testDomain == null ? null : testDomain.AssemblyName; }
		}

		#endregion

		#region EventListener Handlers

		/// <summary>
		/// Trigger event when each test starts
		/// </summary>
		/// <param name="testCase">TestCase that is starting</param>
		public void TestStarted(NUnit.Core.TestCase testCase)
		{
			if ( TestStartedEvent != null )
				TestStartedEvent( testCase );
		}

		/// <summary>
		/// Trigger event when each test finishes
		/// </summary>
		/// <param name="result">Result of the case that finished</param>
		public void TestFinished(TestCaseResult result)
		{
			if ( TestFinishedEvent != null )
				TestFinishedEvent( result );
		}

		/// <summary>
		/// Trigger event when each suite starts
		/// </summary>
		/// <param name="suite">Suite that is starting</param>
		public void SuiteStarted(TestSuite suite)
		{
			if ( SuiteStartedEvent != null )
				SuiteStartedEvent( suite );
		}

		/// <summary>
		/// Trigger event when each suite finishes
		/// </summary>
		/// <param name="result">Result of the suite that finished</param>
		public void SuiteFinished(TestSuiteResult result)
		{
			if ( SuiteFinishedEvent != null )
				SuiteFinishedEvent( result );
		}

		#endregion

		#region Methods

		/// <summary>
		/// Run a testcase or testsuite from the currrent tree
		/// firing the RunStarting and RunFinished events
		/// </summary>
		/// <param name="test">Test to be run</param>
		public void RunTestSuite( UITestNode testInfo )
		{
			if ( RunStartingEvent != null )
				RunStartingEvent( testInfo );

			testDomain.TestFixture = testInfo.FullName;

			TestResult result = testDomain.Run(this);

			if ( RunFinishedEvent != null )
				RunFinishedEvent( result );
		}
	
		/// <summary>
		/// Load an assembly, firing the SuiteLoaded event.
		/// </summary>
		/// <param name="assemblyFileName">Assembly to be loaded</param>
		public void LoadAssembly( string assemblyFileName )
		{
			try
			{
				// Make sure it all works before switching old one out
				NUnit.Framework.TestDomain newDomain = new NUnit.Framework.TestDomain(stdOutWriter, stdErrWriter);
				Test newTest = newDomain.Load(assemblyFileName);
				
				if  ( AssemblyLoaded ) UnloadAssembly();

				testDomain = newDomain;
				currentTest = newTest;

				SetWorkingDirectory(assemblyFileName);

				if ( TestSuiteLoadedEvent != null )
					TestSuiteLoadedEvent( currentTest, assemblyFileName );

				InstallWatcher( assemblyFileName );
			}
			catch( Exception exception )
			{
				if ( TestSuiteLoadFailureEvent != null )
					TestSuiteLoadFailureEvent( assemblyFileName, exception );
			}
		}

		/// <summary>
		/// Unload the current assembly and fire the SuiteUnloaded event
		/// </summary>
		public void UnloadAssembly( )
		{
			if(testDomain != null)
			{
				testDomain.Unload();
				testDomain = null;

				RemoveWatcher();

				if ( TestSuiteUnloadedEvent != null )
					TestSuiteUnloadedEvent( );
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
			// Don't unload the old domain till after the event
			// handlers get a chance to compare the trees.
			try
			{
				TestDomain newDomain = new TestDomain(stdOutWriter, stdErrWriter);
				UITestNode newTest = newDomain.Load(assemblyFileName);

				bool notifyClient = !UIHelper.CompareTree( currentTest, newTest );

				testDomain.Unload();

				testDomain = newDomain;
				currentTest = newTest;


				if ( notifyClient && TestSuiteChangedEvent != null )
						TestSuiteChangedEvent( newTest );
				
			}
			catch( Exception exception )
			{
				if ( TestSuiteLoadFailureEvent != null )
					TestSuiteLoadFailureEvent( assemblyFileName, exception );
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

		public override Object InitializeLifetimeService()
		{
			System.Runtime.Remoting.Lifetime.ILease lease =

				(System.Runtime.Remoting.Lifetime.ILease)base.InitializeLifetimeService(
				);
			if (lease.CurrentState ==
				System.Runtime.Remoting.Lifetime.LeaseState.Initial)
			{
				lease.InitialLeaseTime = TimeSpan.Zero;
			}
			return lease;
		}

		#endregion
	}
}

