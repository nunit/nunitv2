//#define RUN_IN_PARALLEL

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
		/// The downstream TestRunners
		/// </summary>
		protected TestRunner[] runners;

		/// <summary>
		/// The loaded test suite
		/// </summary>
		protected TestNode loadedTest;

		/// <summary>
		/// The event listener for the currently running test
		/// </summary>
		protected EventListener listener;

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

		public virtual IList Extensions
		{
			get
			{
				if ( runners == null )
					return null;

				ArrayList extensions = new ArrayList();

				foreach( TestRunner runner in runners )
					extensions.AddRange( runner.Extensions );

				return extensions;
			}
		}

		public virtual TestNode Test
		{
			get
			{
				if ( loadedTest == null && runners != null )
				{
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
					loadedTest = new TestNode( projectName, tests );
					loadedTest.RunnerID = this.runnerID;
				}

				return loadedTest;
			}
		}

		public virtual TestResult TestResult
		{
			get 
			{ 
				if ( runners == null )
					return null;
				
				TestSuiteResult suiteResult = new TestSuiteResult( Test, Test.FullName );

				foreach( TestRunner runner in runners )
					if ( runner.TestResult != null )
						suiteResult.Results.Add( runner.TestResult );

				return suiteResult;
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
			loadedTest = null;
		}
		#endregion

		#region CountTestCases
		public virtual int CountTestCases( TestFilter filter )
		{
			int count = 0;
			foreach( TestRunner runner in runners )
				count += runner.CountTestCases( filter );
			return count;
		}
		#endregion

		#region Methods for Running Tests
		public virtual TestResult Run( EventListener listener )
		{
			return Run( listener, TestFilter.Empty );
		}

		public virtual TestResult Run(EventListener listener, TestFilter filter )
		{
			// Save active listener for derived classes
			this.listener = listener;

			ITest[] tests = new ITest[runners.Length];
			for( int index = 0; index < runners.Length; index++ )
				tests[index] = runners[index].Test;

			this.listener.RunStarted( this.Test.Name, this.CountTestCases( filter ) );

			TestSuiteResult result = new TestSuiteResult( new TestInfo( projectName, tests ), projectName );
			foreach( TestRunner runner in runners )
				result.Results.Add( runner.Run( this, filter ) );

			this.listener.RunFinished( this.TestResult );

			return result;
		}

		public virtual void BeginRun( EventListener listener )
		{
			BeginRun( listener, TestFilter.Empty );
		}

		public virtual void BeginRun( EventListener listener, TestFilter filter )
		{
			// Save active listener for derived classes
			this.listener = listener;

#if RUN_IN_PARALLEL
			this.listener.RunStarted( this.Test.Name, this.CountTestCases( filter ) );

			foreach( TestRunner runner in runners )
				if ( runner.Test != null )
					runner.BeginRun( this, filter );

			//this.listener.RunFinished( this.Results );
#else
			ThreadedTestRunner threadedRunner = new ThreadedTestRunner( this );
			threadedRunner.BeginRun( listener, filter );
#endif
		}

		public virtual TestResult EndRun()
		{
			TestSuiteResult suiteResult = new TestSuiteResult( Test, Test.FullName );
			foreach( TestRunner runner in runners )
				suiteResult.Results.Add( runner.EndRun() );

			return suiteResult;
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

		void NUnit.Core.EventListener.RunFinished(TestResult result)
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
	}
}
