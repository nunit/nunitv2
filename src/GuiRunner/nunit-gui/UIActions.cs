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
	/// that is interested. It also fires events of it's own for starting
	/// and ending the test run and for loading, unloading or reloading
	/// an assembly.
	/// 
	/// Modifications have been made to help isolate the ui client code
	/// from the loading and unloading of test domains.. Events that 
	/// formerly took a Test, TestCase or TestSuite as an argument, now
	/// use a TestInfo object which gives the same information but isn't
	/// connected to the remote domain.
	/// 
	/// The TestInfo object may in some cases be created using lazy 
	/// evaluation of child TestInfo objects. Since evaluation of these
	/// objects does cause a cross-domain reference, the client code
	/// should access the full tree immediately, rather than at a later
	/// time, if that is what is needed. This will normally happen if
	/// the client building a tree, for example. However, some clients
	/// may only want the name of the test being run and passing the
	/// fully evaluated tree would be unnecessary for them.
	/// 
	/// See comments associated with each event for lifetime limitations 
	/// on the objects passed to the delegates.
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
		private TestInfo currentTest = null;

		/// <summary>
		/// Watcher fires when the assembly changes
		/// </summary>
		private AssemblyWatcher watcher;

		#endregion

		#region Delegates and Events

		/// <summary>
		/// Test suite was loaded. The TestInfo uses lazy evaluation
		/// of child tests, so a client that needs information from
		/// all levels should traverse the tree immediately to ensure
		/// that they are expanded. Clients that only need a field
		/// from the top level test don't need to do that. Since
		/// that's what the clients would normally do anyway, this
		/// should not cause a problem except in pathological cases.
		/// </summary>
		public delegate void SuiteLoadedHandler( TestInfo test, string assemblyFileName);
		public event SuiteLoadedHandler SuiteLoadedEvent;
		
		/// <summary>
		/// Current test suite has changed. The new information should
		/// replace or be merged with the old, depending on the needs
		/// of the client. Lazy evaluation applies here too.
		/// </summary>
		public delegate void SuiteChangedHandler( TestInfo test );
		public event SuiteChangedHandler SuiteChangedEvent;
		
		/// <summary>
		/// Test suite unloaded. The old information is still
		/// available to the client - for example to produce any
		/// reports - but will normally be removed from the UI.
		/// </summary>
		public delegate void SuiteUnloadedHandler();
		public event SuiteUnloadedHandler SuiteUnloadedEvent;
		
		/// <summary>
		/// A failure occured in loading an assembly. This may be as
		/// a result of a client request to load an assembly or as a 
		/// result of an asynchronous change that required reloading 
		/// the assembly. In the first case, the loaded assembly has
		/// not been replaced unless the assemblyFileName is the same.
		/// In the second case, the client will usually treat this
		/// as a sort of involuntary unload.
		/// </summary>
		public delegate void AssemblyLoadFailureHandler( 
			string assemblyFileName, Exception exception );
		public event AssemblyLoadFailureHandler AssemblyLoadFailureEvent;

		/// <summary>
		/// The following events signal that a test run, test suite
		/// or test case has started. If client is holding the entire 
		/// tree of tests that was previously loaded, this TestInfo 
		/// should match one of them, but it won't generally be the 
		/// same object. Best practice is to match the TestInfo with 
		/// one that is already held rather than expanding it to 
		/// create lots of new objects. In the future, these events
		/// may just pass the name of the test.
		/// </summary>
		public delegate void RunStartingHandler( TestInfo test );
		public event RunStartingHandler RunStartingEvent;
		
		public delegate void SuiteStartedHandler( TestInfo suite );
		public event SuiteStartedHandler SuiteStartedEvent;
		
		public delegate void TestStartedHandler( TestInfo testCase );
		public event TestStartedHandler TestStartedEvent;
		
		/// <summary>
		/// The following events signal that a test run, test suite or
		/// test case has finished. Client should make use of the result
		/// value during the life of the call only. If the result is to
		/// be saved in the application, it should be converted to a
		/// TestResultInfo, which will cause it's internal Test reference
		/// to be converted to a local TestInfo object.
		/// 
		/// NOTE: These cannot be converted to use TestResultInfo directly
		/// because some client code makes use of ResultVisitor which would
		/// also have to be changed. Maybe later...
		/// </summary>
		public delegate void RunFinishedHandler( TestResult result );
		public event RunFinishedHandler RunFinishedEvent;
		
		public delegate void SuiteFinishedHandler( TestSuiteResult result );
		public event SuiteFinishedHandler SuiteFinishedEvent;
		
		public delegate void TestFinishedHandler( TestCaseResult result );
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
		public void RunTestSuite( TestInfo testInfo )
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
				TestDomain newDomain = new TestDomain(stdOutWriter, stdErrWriter);
				TestInfo newTest = newDomain.Load(assemblyFileName);

				bool notifyClient = !UIHelper.CompareTree( currentTest, newTest );

				testDomain.Unload();

				testDomain = newDomain;
				currentTest = newTest;


				if ( notifyClient && SuiteChangedEvent != null )
						SuiteChangedEvent( newTest );
				
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

