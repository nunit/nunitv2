/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
namespace NUnit.Gui
{
	using System;
	using System.IO;
	using NUnit.Core;
	using NUnit.Util;
	using NUnit.Framework;


	/// <summary>
	/// UIActions handles interactions between a test runner and a client
	/// program - typically the user interface. It implemements the
	/// EventListener interface which is used by the test runner and
	/// fires events in order to provide the information to any component
	/// that is interested. It also fires events for starting the test
	/// run, ending it and successfully loading the assembly.
	/// </summary>
	public class UIActions : MarshalByRefObject, NUnit.Core.EventListener
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
		private Test currentTest = null;

		/// <summary>
		/// Watcher fires when the assembly changes
		/// </summary>
		private AssemblyWatcher watcher;

		#endregion

		#region Delegates and Events

		public delegate void TestStartedHandler( NUnit.Core.TestCase testCase );
		public delegate void TestFinishedHandler( TestCaseResult result );
		public delegate void SuiteStartedHandler( TestSuite suite );
		public delegate void SuiteFinishedHandler( TestSuiteResult result );
		public delegate void RunStartingHandler( Test test );
		public delegate void RunFinishedHandler( TestResult result );
		public delegate void SuiteLoadedHandler( Test test, string assemblyFileName);
		public delegate void SuiteChangedHandler( Test test );
		public delegate void SuiteUnloadedHandler( );
		public delegate void AssemblyLoadFailureHandler( string assemblyFileName, Exception exception );

		public event TestStartedHandler TestStartedEvent;
		public event TestFinishedHandler TestFinishedEvent;
		public event SuiteStartedHandler SuiteStartedEvent;
		public event SuiteFinishedHandler SuiteFinishedEvent;
		public event RunStartingHandler RunStartingEvent;
		public event RunFinishedHandler RunFinishedEvent;
		public event SuiteLoadedHandler SuiteLoadedEvent;
		public event SuiteChangedHandler SuiteChangedEvent;
		public event SuiteUnloadedHandler SuiteUnloadedEvent;
		public event AssemblyLoadFailureHandler AssemblyLoadFailureEvent;

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
		public void RunTestSuite(Test test)
		{
			if ( RunStartingEvent != null )
				RunStartingEvent( test );

			testDomain.TestFixture = test.FullName;

			TestResult result = testDomain.Run(this);

			if ( RunFinishedEvent != null )
				RunFinishedEvent( result );
		}

		/// <summary>
		/// Load the test suite
		/// </summary>
		/// <param name="assemblyFileName"></param>
		/// <returns></returns>
//		private Test LoadTestSuite(string assemblyFileName)
//		{
//			return testDomain;
//		}
		
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

				if ( SuiteLoadedEvent != null )
					SuiteLoadedEvent( currentTest, assemblyFileName );

				InstallWatcher( assemblyFileName );
			}
			catch( Exception exception )
			{
				if ( AssemblyLoadFailureEvent != null )
					AssemblyLoadFailureEvent( assemblyFileName, exception );
			}
		}

		/// <summary>
		/// Unload the current assembly and fire the SuiteUnloaded event
		/// </summary>
		public void UnloadAssembly( )
		{
			testDomain.Unload();
			testDomain = null;

			RemoveWatcher();

			if ( SuiteUnloadedEvent != null )
				SuiteUnloadedEvent( );
		}

		/// <summary>
		/// Reload the current assembly on command
		/// </summary>
		public void ReloadAssembly()
		{
			LoadAssembly( testDomain.AssemblyName );
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
			// TODO: Some changes to the runner might make this easier.

			try
			{
				TestDomain newDomain = new TestDomain(stdOutWriter, stdErrWriter);
				Test newTest = newDomain.Load(assemblyFileName);

				if(!UIHelper.CompareTree( currentTest, newTest ) )
				{
					testDomain.Unload();

					testDomain = newDomain;
					currentTest = newTest;

					if ( SuiteChangedEvent != null )
						SuiteChangedEvent( newTest );
				}
				else
				{
					newDomain.Unload();
				}
			}
			catch( Exception exception )
			{
				if ( AssemblyLoadFailureEvent != null )
					AssemblyLoadFailureEvent( assemblyFileName, exception );
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

