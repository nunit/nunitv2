#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Util
{
	using System;
	using System.IO;
	using System.Collections;
	using System.Threading;
	using System.Configuration;
	using NUnit.Core;
	using NUnit.Core.Filters;


	/// <summary>
	/// TestLoader handles interactions between a test runner and a 
	/// client program - typically the user interface - for the 
	/// purpose of loading, unloading and running tests.
	/// 
	/// It implemements the EventListener interface which is used by 
	/// the test runner and repackages those events, along with
	/// others as individual events that clients may subscribe to
	/// in collaboration with a TestEventDispatcher helper object.
	/// 
	/// TestLoader is quite handy for use with a gui client because
	/// of the large number of events it supports. However, it has
	/// no dependencies on ui components and can be used independently.
	/// </summary>
	public class TestLoader : MarshalByRefObject, NUnit.Core.EventListener, ITestLoader
	{
		#region Instance Variables

		/// <summary>
		/// Our event dispatching helper object
		/// </summary>
		private TestEventDispatcher events;

		/// <summary>
		/// Use MuiltipleTestDomainRunner if true
		/// </summary>
		private bool multiDomain;

		/// <summary>
		/// Merge namespaces across multiple assemblies
		/// </summary>
		private bool mergeAssemblies;

		/// <summary>
		/// Generate suites for each level of namespace containing tests
		/// </summary>
		private bool autoNamespaceSuites;

		/// <summary>
		/// Loads and executes tests. Non-null when
		/// we have loaded a test.
		/// </summary>
		private TestRunner testRunner = null;

		/// <summary>
		/// Our current test project, if we have one.
		/// </summary>
		private NUnitProject testProject = null;

		/// <summary>
		/// The currently loaded test, returned by the testrunner
		/// </summary>
		private ITest loadedTest = null;

		/// <summary>
		/// The test name that was specified when loading
		/// </summary>
		private string loadedTestName = null;

		/// <summary>
		/// Result of the last test run
		/// </summary>
		private TestResult testResult = null;

		/// <summary>
		/// The last exception received when trying to load, unload or run a test
		/// </summary>
		private Exception lastException = null;

		/// <summary>
		/// Watcher fires when the assembly changes
		/// </summary>
		private AssemblyWatcher watcher;

		/// <summary>
		/// Assembly changed during a test and
		/// needs to be reloaded later
		/// </summary>
		private bool reloadPending = false;

		/// <summary>
		/// Indicates whether to watch for changes
		/// and reload the tests when a change occurs.
		/// </summary>
		private bool reloadOnChange = false;

		/// <summary>
		/// Indicates whether to automatically rerun
		/// the tests when a change occurs.
		/// </summary>
		private bool rerunOnChange = false;

		/// <summary>
		/// Indicates whether to reload the tests
		/// before each run.
		/// </summary>
		private bool reloadOnRun = false;

		#endregion

		#region Constructors

		public TestLoader()
			: this( new TestEventDispatcher() ) { }

		public TestLoader(TestEventDispatcher eventDispatcher )
		{
			this.events = eventDispatcher;

			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler( OnUnhandledException );
		}

		#endregion

		#region Properties
		public bool IsProjectLoaded
		{
			get { return testProject != null; }
		}

		public bool IsTestLoaded
		{
			get { return loadedTest != null; }
		}

		public bool Running
		{
			get { return testRunner != null && testRunner.Running; }
		}

		public NUnitProject TestProject
		{
			get { return testProject; }
			set	{ OnProjectLoad( value ); }
		}

		public ITestEvents Events
		{
			get { return events; }
		}

		public string TestFileName
		{
			get { return testProject.ProjectPath; }
		}

		public TestResult TestResult
		{
			get { return testResult; }
		}

		public Exception LastException
		{
			get { return lastException; }
		}

		public bool ReloadOnChange
		{
			get { return reloadOnChange; }
			set { reloadOnChange = value; }
		}

		public bool RerunOnChange
		{
			get { return rerunOnChange; }
			set { rerunOnChange = value; }
		}

		public bool ReloadOnRun
		{
			get { return reloadOnRun; }
			set { reloadOnRun = value; }
		}

		public bool MultiDomain
		{
			get { return multiDomain; }
			set { multiDomain = value; }
		}

		public bool MergeAssemblies
		{
			get { return mergeAssemblies; }
			set { mergeAssemblies = value; }
		}

		public bool AutoNamespaceSuites
		{
			get { return autoNamespaceSuites; }
			set { autoNamespaceSuites = value; }
		}

		public IList AssemblyInfo
		{
			get { return testRunner.AssemblyInfo; }
		}

		public int TestCount
		{
			get { return loadedTest == null ? 0 : loadedTest.TestCount; }
		}
		#endregion

		#region EventListener Handlers

		void EventListener.RunStarted(string name, int testCount)
		{
			events.FireRunStarting( name, testCount );
		}

		void EventListener.RunFinished(NUnit.Core.TestResult testResult)
		{
			this.testResult = testResult;
			events.FireRunFinished( testResult );
		}

		void EventListener.RunFinished(Exception exception)
		{
			this.lastException = exception;
			events.FireRunFinished( exception );
		}

		/// <summary>
		/// Trigger event when each test starts
		/// </summary>
		/// <param name="testCase">TestCase that is starting</param>
		void EventListener.TestStarted(TestName testName)
		{
			events.FireTestStarting( testName );
		}

		/// <summary>
		/// Trigger event when each test finishes
		/// </summary>
		/// <param name="result">Result of the case that finished</param>
		void EventListener.TestFinished(TestCaseResult result)
		{
			events.FireTestFinished( result );
		}

		/// <summary>
		/// Trigger event when each suite starts
		/// </summary>
		/// <param name="suite">Suite that is starting</param>
		void EventListener.SuiteStarted(TestName suiteName)
		{
			events.FireSuiteStarting( suiteName );
		}

		/// <summary>
		/// Trigger event when each suite finishes
		/// </summary>
		/// <param name="result">Result of the suite that finished</param>
		void EventListener.SuiteFinished(TestSuiteResult result)
		{
			events.FireSuiteFinished( result );
		}

		/// <summary>
		/// Trigger event when an unhandled exception (other than ThreadAbordException) occurs during a test
		/// </summary>
		/// <param name="exception">The unhandled exception</param>
		void EventListener.UnhandledException(Exception exception)
		{
			events.FireTestException( exception );
		}

		void OnUnhandledException( object sender, UnhandledExceptionEventArgs args )
		{
			switch( args.ExceptionObject.GetType().FullName )
			{
				case "System.Threading.ThreadAbortException":
					break;
				case "NUnit.Framework.AssertionException":
				default:
					events.FireTestException((Exception)args.ExceptionObject);
					break;
			}
		}

		/// <summary>
		/// Trigger event when output occurs during a test
		/// </summary>
		/// <param name="testOutput">The test output</param>
		void EventListener.TestOutput(TestOutput testOutput)
		{
			events.FireTestOutput( testOutput );
		}

		#endregion

		#region Methods for Loading and Unloading Projects
		
		/// <summary>
		/// Create a new project with default naming
		/// </summary>
		public void NewProject()
		{
			try
			{
				events.FireProjectLoading( "New Project" );

				OnProjectLoad( NUnitProject.NewProject() );
			}
			catch( Exception exception )
			{
				lastException = exception;
				events.FireProjectLoadFailed( "New Project", exception );
			}
		}

		/// <summary>
		/// Create a new project using a given path
		/// </summary>
		public void NewProject( string filePath )
		{
			try
			{
				events.FireProjectLoading( filePath );

				NUnitProject project = new NUnitProject( filePath );

				project.Configs.Add( "Debug" );
				project.Configs.Add( "Release" );			
				project.IsDirty = false;

				OnProjectLoad( project );
			}
			catch( Exception exception )
			{
				lastException = exception;
				events.FireProjectLoadFailed( filePath, exception );
			}
		}

		/// <summary>
		/// Load a new project, optionally selecting the config and fire events
		/// </summary>
		public void LoadProject( string filePath, string configName )
		{
			try
			{
				events.FireProjectLoading( filePath );

				NUnitProject newProject = NUnitProject.LoadProject( filePath );
				if ( configName != null ) 
				{
					newProject.SetActiveConfig( configName );
					newProject.IsDirty = false;
				}

				OnProjectLoad( newProject );
			}
			catch( Exception exception )
			{
				lastException = exception;
				events.FireProjectLoadFailed( filePath, exception );
			}
		}

		/// <summary>
		/// Load a new project using the default config and fire events
		/// </summary>
		public void LoadProject( string filePath )
		{
			LoadProject( filePath, null );
		}

		/// <summary>
		/// Load a project from a list of assemblies and fire events
		/// </summary>
		public void LoadProject( string[] assemblies )
		{
			try
			{
				events.FireProjectLoading( "New Project" );

				NUnitProject newProject = NUnitProject.FromAssemblies( assemblies );

				OnProjectLoad( newProject );
			}
			catch( Exception exception )
			{
				lastException = exception;
				events.FireProjectLoadFailed( "New Project", exception );
			}
		}

		/// <summary>
		/// Unload the current project and fire events
		/// </summary>
		public void UnloadProject()
		{
			string testFileName = TestFileName;

			try
			{
				events.FireProjectUnloading( testFileName );

				if ( IsTestLoaded )
					UnloadTest();

				testProject.Changed -= new ProjectEventHandler( OnProjectChanged );
				testProject = null;

				events.FireProjectUnloaded( testFileName );
			}
			catch (Exception exception )
			{
				lastException = exception;
				events.FireProjectUnloadFailed( testFileName, exception );
			}

		}

		/// <summary>
		/// Common operations done each time a project is loaded
		/// </summary>
		/// <param name="testProject">The newly loaded project</param>
		private void OnProjectLoad( NUnitProject testProject )
		{
			if ( IsProjectLoaded )
				UnloadProject();

			this.testProject = testProject;
			testProject.Changed += new ProjectEventHandler( OnProjectChanged );

			events.FireProjectLoaded( TestFileName );
		}

		private void OnProjectChanged( object sender, ProjectEventArgs e )
		{
			switch ( e.type )
			{
				case ProjectChangeType.ActiveConfig:
					if( TestProject.IsLoadable )
						LoadTest();
					break;

				case ProjectChangeType.AddConfig:
				case ProjectChangeType.UpdateConfig:
					if ( e.configName == TestProject.ActiveConfigName && TestProject.IsLoadable )
						LoadTest();
					break;

				case ProjectChangeType.RemoveConfig:
					if ( IsTestLoaded && TestProject.Configs.Count == 0 )
						UnloadTest();
					break;

				default:
					break;
			}
		}

		#endregion

		#region Methods for Loading and Unloading Tests

		public void LoadTest()
		{
			LoadTest( null );
		}
		
		public void LoadTest( string testName )
		{
			try
			{
				events.FireTestLoading( TestFileName );

				testRunner = CreateRunner();

				bool loaded = TestProject.IsAssemblyWrapper
					? testRunner.Load( TestProject.ActiveConfig.Assemblies[0], testName )
					: testRunner.Load( TestProject.MakeTestPackage(), testName );

				loadedTest = testRunner.Test;
				loadedTestName = testName;
				testResult = null;
				reloadPending = false;
			
				if ( ReloadOnChange )
					InstallWatcher( );

				if ( loaded )
					events.FireTestLoaded( TestFileName, loadedTest );
				else
				{
					lastException = new ApplicationException( string.Format ( "Unable to find test {0} in assembly", testName ) );
					events.FireTestLoadFailed( TestFileName, lastException );
				}
			}
			catch( FileNotFoundException exception )
			{
				lastException = exception;

				foreach( string assembly in TestProject.ActiveConfig.Assemblies )
				{
					if ( Path.GetFileNameWithoutExtension( assembly ) == exception.FileName &&
						!PathUtils.SamePathOrUnder( testProject.ActiveConfig.BasePath, assembly ) )
					{
						lastException = new ApplicationException( string.Format( "Unable to load {0} because it is not located under the AppBase", exception.FileName ), exception );
						break;
					}
				}

				events.FireTestLoadFailed( TestFileName, lastException );
			}
			catch( Exception exception )
			{
				lastException = exception;
				events.FireTestLoadFailed( TestFileName, exception );
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
				string fileName = TestFileName;

				try
				{
					events.FireTestUnloading( fileName );

					RemoveWatcher();

					testRunner.Unload();

					testRunner = null;

					loadedTest = null;
					loadedTestName = null;
					testResult = null;
					reloadPending = false;

					events.FireTestUnloaded( fileName );
				}
				catch( Exception exception )
				{
					lastException = exception;
					events.FireTestUnloadFailed( fileName, exception );
				}
			}
		}

		/// <summary>
		/// Reload the current test on command
		/// </summary>
		public void ReloadTest()
		{
			OnTestChanged( TestFileName );
		}

		/// <summary>
		/// Handle watcher event that signals when the loaded assembly
		/// file has changed. Make sure it's a real change before
		/// firing the SuiteChangedEvent. Since this all happens
		/// asynchronously, we use an event to let ui components
		/// know that the failure happened.
		/// </summary>
		public void OnTestChanged( string testFileName )
		{
			if ( Running )
				reloadPending = true;
			else 
			{
				try
				{
					events.FireTestReloading( testFileName );

					// Don't unload the old domain till after the event
					// handlers get a chance to compare the trees.
					TestRunner newRunner = CreateRunner( );

					if (TestProject.IsAssemblyWrapper)
						newRunner.Load(testProject.ActiveConfig.Assemblies[0]);
					else
						newRunner.Load( testProject.MakeTestPackage(), loadedTestName);

					testRunner.Unload();

					testRunner = newRunner;
					loadedTest = testRunner.Test;
					reloadPending = false;

					events.FireTestReloaded( testFileName, loadedTest );				
				}
				catch( Exception exception )
				{
					lastException = exception;
					events.FireTestReloadFailed( testFileName, exception );
				}
			}

			if ( rerunOnChange )
			{
				testRunner.BeginRun( this );
			}
		}
		#endregion

		#region Methods for Running Tests
		/// <summary>
		/// Run all the tests
		/// </summary>
		public void RunTests()
		{
			RunTests( TestFilter.Empty );
		}

		/// <summary>
		/// Run selected tests using a filter
		/// </summary>
		/// <param name="filter">The filter to be used</param>
		public void RunTests( ITestFilter filter )
		{
			if ( !Running )
			{
				if ( reloadPending || ReloadOnRun )
					ReloadTest();

				testRunner.BeginRun( this, filter );
			}
		}

		/// <summary>
		/// Cancel the currently running test.
		/// Fail silently if there is none to
		/// allow for latency in the UI.
		/// </summary>
		public void CancelTestRun()
		{
			if ( Running )
				testRunner.CancelRun();
		}

		public IList GetCategories() 
		{
			CategoryManager categoryManager = new CategoryManager();
			categoryManager.AddAllCategories( this.loadedTest );
			ArrayList list = new ArrayList( categoryManager.Categories );
			list.Sort();
			return list;
		}
		#endregion

		#region Helper Methods

		/// <summary>
		/// Install our watcher object so as to get notifications
		/// about changes to a test.
		/// </summary>
		private void InstallWatcher()
		{
			if(watcher!=null) watcher.Stop();

			watcher = new AssemblyWatcher( 1000, TestProject.ActiveConfig.Assemblies.ToArray() );
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

		private TestRunner CreateRunner()
		{
			TestRunner runner = multiDomain
				? (TestRunner)new MultipleTestDomainRunner()
				: (TestRunner)new TestDomain();
				
			runner.Settings["MergeAssemblies"] = mergeAssemblies;
			runner.Settings["AutoNamespaceSuites"] = autoNamespaceSuites;

			return runner;
		}
		#endregion

		#region InitializeLifetimeService Override
		public override object InitializeLifetimeService()
		{
			return null;
		}
		#endregion
	}
}
