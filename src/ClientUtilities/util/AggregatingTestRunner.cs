namespace NUnit.Util
{
	using System;
	using System.Collections;
	using System.IO;
	using NUnit.Core;

	/// <summary>
	/// AggregatingTestRunner allows running multiple TestRunners
	/// and combining the results.
	/// </summary>
	public abstract class AggregatingTestRunner : LongLivingMarshalByRefObject, TestRunnerEx, EventListener
	{
		#region Instance Variables

		/// <summary>
		/// Our runner ID
		/// </summary>
		protected int runnerID;

		/// <summary>
		/// The downstream TestRunner
		/// </summary>
		protected TestRunner[] runners;

		protected ITest[] loadedTests;

		/// <summary>
		/// The event listener for the currently running test
		/// </summary>
		protected EventListener listener;

		protected ITestFilter filter;

		protected string projectName;

		private TestRunnerSettings settings;
		#endregion

		#region Constructors
		public AggregatingTestRunner() : this( 0 ) { }
		public AggregatingTestRunner( int runnerID )
		{
			this.runnerID = runnerID;
			this.settings = new TestRunnerSettings( this );
			this.settings.Changed += new TestRunnerSettings.SettingsChangedHandler(settings_Changed);
		}
		#endregion

		#region Properties

		public virtual int ID
		{
			get { return runnerID; }
		}

		public virtual bool Running
		{
			get 
			{ 
				foreach( TestRunner runner in runners )
					if ( runner.Running )
						return true;
			
				return false;
			}
		}

		public virtual IList TestFrameworks
		{
			get
			{
				if ( runners == null )
					return null;

				ArrayList frameworks = new ArrayList();

				foreach( TestRunner runner in runners )
					frameworks.AddRange( runner.TestFrameworks );
				
				return frameworks;
			}
		}

		public virtual TestNode Test
		{
			get
			{ 
				if ( runners == null )
					return null;

				// Count non-null tests, in case we specified a fixture
				int count = 0;
				foreach( TestRunner runner in runners )
					if ( runner.Test != null )
						++count;  

				// Copy non-null tests to an array
				int index = 0;
				ITest[] tests = new ITest[count];
				foreach( TestRunner runner in runners )
					if ( runner.Test != null )
						tests[index++] = runner.Test;

				// Return master node containing all the tests
				TestNode rootNode = new TestNode( projectName, tests );
				rootNode.RunnerID = this.runnerID;
				return rootNode;
			}
		}

		public virtual TestResult[] Results
		{
			get 
			{ 
				if ( runners == null )
					return null;
				
				ArrayList results = new ArrayList();

				foreach( TestRunner runner in runners )
					if ( runner.Results != null )
						results.AddRange( runner.Results );

				return (TestResult[])results.ToArray( typeof(TestResult) );
			}
		}

		public virtual ITestFilter Filter
		{
			get { return this.filter; }
			set 
			{ 
				this.filter = value;
 
				foreach( TestRunner runner in runners )
					runner.Filter = filter;
			}
		}

		public TestRunnerSettings Settings
		{
			get { return settings; }
			set { settings = value; }
		}
		#endregion

		#region Load and Unload Methods

        public abstract bool Load(string assemblyName);

        public abstract bool Load(string assemblyName, string testName);
        
        public abstract bool Load(string projectName, string[] assemblies);

        public abstract bool Load(string projectName, string[] assemblies, string testName);

		public bool Load( NUnitProject project )
		{
			return Load( project.ProjectPath, project.ActiveConfig.AbsolutePaths );
		}

		public bool Load( NUnitProject project, string testName )
		{
			return Load( project.ProjectPath, project.ActiveConfig.AbsolutePaths, testName );
		}

		public virtual void Unload()
		{
			foreach( TestRunner runner in runners )
				runner.Unload();
		}

		#endregion

		#region Methods for Counting TestCases

		public virtual int CountTestCases(string testName)
		{
			int count = 0;
			foreach( TestRunner runner in runners )
				count += runner.CountTestCases(testName);
			return count;
		}

		public virtual int CountTestCases(string[] testNames)
		{
			int count = 0;
			foreach( TestRunner runner in runners )
				count += runner.CountTestCases(testNames);
			return count;
		}

		#endregion

		#region GetCategories Method

		public virtual ICollection GetCategories()
		{
			ArrayList categories = new ArrayList();

			foreach( TestRunner runner in runners )
				categories.AddRange( runner.GetCategories() );

			return categories;
		}

		#endregion

		#region Methods for Running Tests

		public virtual TestResult Run(EventListener listener)
		{
			// Save active listener for derived classes
			this.listener = listener;

			ITest[] tests = new ITest[runners.Length];
			for( int index = 0; index < runners.Length; index++ )
				tests[index] = runners[index].Test;

			this.listener.RunStarted( this.Test.Name, this.Test.TestCount );

			TestSuiteResult result = new TestSuiteResult( new TestInfo( projectName, tests ), projectName );
			foreach( TestRunner runner in runners )
				result.Results.Add( runner.Run( this ) );

			this.listener.RunFinished( this.Results );

			return result;
		}

		public virtual TestResult[] Run(EventListener listener, string[] testNames)
		{
			if ( testNames == null || testNames.Length == 0 ) 
				return new TestResult[] { Run( listener ) };

//			TestName[] testNames = new TestName[names.Length];
//			for ( int index = 0; index < names.Length; index++ )
//				testNames[index] = TestName.Parse( names[index] );
//
//			return Run( listener, testNames );

			// Save active listener for derived classes
			this.listener = listener;
			ArrayList results = new ArrayList();

			// Signal that we are starting the run
//			TestInfo[] info = new TestInfo[tests.Length];
//			int index = 0;
//			foreach( Test test in tests )
//				info[index++] = new TestInfo( test );
//			listener.RunStarted( info );
			this.listener.RunStarted( this.Test.Name, this.CountTestCases( testNames ) );

			foreach( string name in testNames )
			{
				TestName tName = TestName.Parse( name );
				foreach( TestRunner runner in runners )
				{
					if ( runner.ID == tName.RunnerID )
					{
						results.AddRange( runner.Run( this, new string[] { name } ) );
						break;
					}
				}
			}

			TestResult[] testResults = (TestResult[])results.ToArray( typeof( TestResult ) );
			this.listener.RunFinished( testResults );

			return testResults;
		}

//		public virtual TestResult[] Run(EventListener listener, TestName[] testNames)
//		{
//			// Save active listener for derived classes
//			this.listener = listener;
//			ArrayList results = new ArrayList();
//
//			this.listener.RunStarted( this.Test.FullName, this.Test.TestCount );
//
//			foreach( TestName testName in testNames )
//			{
//				foreach( TestRunner runner in runners )
//					if ( testName.RunnerID == runner.ID )
//					{
//						results.AddRange( runner.Run( this, new string[] { testName.UniqueName } ) );
//						break;
//					}
//			}
//
//			TestResult[] testResults = (TestResult[])results.ToArray( typeof( TestResult ) );
//			this.listener.RunFinished( testResults );
//
//			return testResults;
//		}

		public virtual void BeginRun( EventListener listener )
		{
			// Save active listener for derived classes
			this.listener = listener;

#if RUN_IN_PARALLEL
			this.listener.RunStarted( new TestInfo[]{ this.Test } );

			foreach( TestRunner runner in runners )
				if ( runner.Test != null )
					runner.BeginRun( this );

			//this.listener.RunFinished( this.Results );
#else
//			System.Threading.Thread thread = new System.Threading.Thread( new System.Threading.ThreadStart( runnerProc ) );
//			thread.Start();

//			TestRunnerThread thread = new TestRunnerThread( this );
//			thread.StartRun( listener, null );

			ThreadedTestRunner threadedRunner = new ThreadedTestRunner( this );
			threadedRunner.BeginRun( listener, null );
#endif
		}
//		private void runnerProc()
//		{
//			this.listener.RunStarted( this.Test.FullName, this.Test.TestCount );
//
//			foreach( TestRunner runner in runners )
//				if ( runner.Test != null )
//					runner.Run( this );
//
//			this.listener.RunFinished( this.Results );
//		}

		public virtual void BeginRun( EventListener listener, string[] testNames )
		{
			// Save active listener for derived classes
			this.listener = listener;

//			foreach( TestRunner runner in runners )
//				runner.BeginRun( listener, testNames );

			ThreadedTestRunner threadedRunner = new ThreadedTestRunner( this );
			threadedRunner.BeginRun( listener, testNames );
		}

//		private void runnerProc2()
//		{
//			this.listener.RunStarted( this.Test.FullName, this.Test.TestCount );
//
//			foreach( TestRunner runner in runners )
//				if ( runner.Test != null )
//					runner.Run( this );
//
//			this.listener.RunFinished( this.Results );
//		}

		public virtual TestResult[] EndRun()
		{
			ArrayList results = new ArrayList();
			foreach( TestRunner runner in runners )
				results.AddRange( runner.EndRun() );
			return (TestResult[])results.ToArray( typeof( TestResult ) );
		}

		public virtual void CancelRun()
		{
			foreach( TestRunner runner in runners )
				runner.CancelRun();
		}

		public virtual void Wait()
		{
			foreach( TestRunner runner in runners )
				runner.Wait();
		}

		#endregion

		#region EventListener Members

		public void TestStarted(TestInfo testCase)
		{
			this.listener.TestStarted( testCase );
		}

		public void RunStarted(string name, int testCount)
		{
			// TODO: We may want to count how many runs are started
			// Ignore - we provide our own
		}

		public void RunFinished(Exception exception)
		{
			// Ignore - we provide our own
		}

		void NUnit.Core.EventListener.RunFinished(TestResult[] results)
		{
			// TODO: Issue combined RunFinished when all runs are done
		}

		public void SuiteFinished(TestSuiteResult result)
		{
			this.listener.SuiteFinished( result );
		}

		public void TestFinished(TestCaseResult result)
		{
			this.listener.TestFinished( result );
		}

		public void UnhandledException(Exception exception)
		{
			this.listener.UnhandledException( exception );
		}

		public void TestOutput(TestOutput testOutput)
		{
			this.listener.TestOutput( testOutput );
		}

		public void SuiteStarted(TestInfo suite)
		{
			this.listener.SuiteStarted( suite );
		}

		#endregion

		#region Handler for Settings Changed Event
		private void settings_Changed(string name, object value)
		{
			if ( runners != null )
				foreach( TestRunner runner in runners )
					if ( runner != null )
						runner.Settings[name] = value;
		}
		#endregion

		#region Helper Methods
		private ITest FindTest(ITest test, string fullName)
		{
			if(test.UniqueName.Equals(fullName)) return test;
			if(test.FullName.Equals(fullName)) return test;
			
			ITest result = null;
			if(test is TestSuite)
			{
				TestSuite suite = (TestSuite)test;
				foreach(Test testCase in suite.Tests)
				{
					result = FindTest(testCase, fullName);
					if(result != null) break;
				}
			}

			return result;
		}

		private ITest[] FindTests( ITest test, string[] names )
		{
			ITest[] tests = new ITest[ names.Length ];

			int index = 0;
			foreach( string name in names )
				tests[index++] = FindTest( test, name );

			return tests;
		}
		#endregion
	}
}
