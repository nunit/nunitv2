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
	/// 
	/// Most of the events pass out a reference to a test suite, either
	/// directly or in a TestResult. Since the tests are actually remote
	/// objects, the references become invalid when the appdomain is 
	/// unloaded, which can happen at any time. Currently, this requires
	/// that the client maintain a certain discipline in the user of 
	/// the references. Specific guarantees for the lifetime of references
	/// passed to each delegate are provided in comments below.
	/// 
	/// At some time, we may want to change this so that clients
	/// don't have to be so aware of what is happening under the covers.
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

		/// <summary>
		/// Test suite was loaded. Client should not be holding any
		/// previous references, since a SuiteUnloadedEvent will 
		/// have preceded this one. The Test reference passed and
		/// any contained references are valid until another event
		/// occurs to invalidate them.
		/// </summary>
		public delegate void SuiteLoadedHandler( Test test, string assemblyFileName);
		public event SuiteLoadedHandler SuiteLoadedEvent;
		
		/// <summary>
		/// Current test suite has changed. The previously received
		/// references are valid for use during this call but must
		/// be replaced by newly supplied references for later use.
		/// NOTE: This is rather inconvenient for the client but
		/// is required in order to reflect changes made to the 
		/// tests while the client is running.
		/// </summary>
		public delegate void SuiteChangedHandler( Test test );
		public event SuiteChangedHandler SuiteChangedEvent;
		
		/// <summary>
		/// Test suite unloaded. The previously loaded test suite 
		/// reference, and all it's contained test references are
		/// valid for use during this call but not after.
		/// </summary>
		public delegate void SuiteUnloadedHandler( );
		public event SuiteUnloadedHandler SuiteUnloadedEvent;
		
		/// <summary>
		/// A failure occured in loading an assembly. This may be as
		/// a result of a client request to load an assembly or as a 
		/// result of an asynchronous change that required reloading 
		/// the assembly. If the assemblyFileName is the same as that
		/// which is currently held by the client, then all references
		/// should be considered immediately invalid. If the name is
		/// different, then old references are still valid - unless,
		/// of course, an AssemblyUnloadEvent has been processed.
		/// </summary>
		public delegate void AssemblyLoadFailureHandler( string assemblyFileName, Exception exception );
		public event AssemblyLoadFailureHandler AssemblyLoadFailureEvent;

		/// <summary>
		/// Test run starting. To allow for changes in the test runner,
		/// consider the Test reference as only valid for this call.
		/// </summary>
		public delegate void RunStartingHandler( Test test );
		public event RunStartingHandler RunStartingEvent;
		
		/// <summary>
		/// A Suite has started. Reference is only valid for this call.
		/// </summary>
		public delegate void SuiteStartedHandler( TestSuite suite );
		public event SuiteStartedHandler SuiteStartedEvent;
		
		/// <summary>
		/// A test has started. Reference is only valid for this call.
		/// </summary>
		public delegate void TestStartedHandler( NUnit.Core.TestCase testCase );
		public event TestStartedHandler TestStartedEvent;
		
		/// <summary>
		/// A test has finished. Reference is only valid for this call.
		/// </summary>
		public delegate void TestFinishedHandler( TestCaseResult result );
		public event TestFinishedHandler TestFinishedEvent;
		
		/// <summary>
		/// A Suite has finished. Reference is only valid for this call.
		/// </summary>
		public delegate void SuiteFinishedHandler( TestSuiteResult result );
		public event SuiteFinishedHandler SuiteFinishedEvent;
		
		/// <summary>
		/// Test run finished. To allow for changes in the test runner,
		/// consider the TestResult reference as only valid for this call.
		/// </summary>
		public delegate void RunFinishedHandler( TestResult result );
		public event RunFinishedHandler RunFinishedEvent;
		
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
		public void RunTestSuite( Test test )
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
			try
			{
				TestDomain oldDomain = testDomain;
				TestDomain newDomain = new TestDomain(stdOutWriter, stdErrWriter);
				Test newTest = newDomain.Load(assemblyFileName);

				if(!UIHelper.CompareTree( currentTest, newTest ) )
				{
					testDomain = newDomain;
					currentTest = newTest;

					if ( SuiteChangedEvent != null )
						SuiteChangedEvent( newTest );
				}
					
				if ( testDomain == newDomain )
					oldDomain.Unload();
				else
					newDomain.Unload();
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

