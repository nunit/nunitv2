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
	public abstract class AggregatingTestRunner : LongLivingMarshalByRefObject, TestRunner, EventListener
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

		protected IFilter filter;

		protected string projectName;

		#endregion

		#region Constructors
		public AggregatingTestRunner() : this( 0 ) { }
		public AggregatingTestRunner( int runnerID )
		{
			this.runnerID = runnerID;
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

				// Temporary
				if ( runners.Length == 1 )
					return runners[0].Test;

				ITest[] tests = new ITest[runners.Length];
				for( int index = 0; index < runners.Length; index++ )
					tests[index] = runners[index].Test;

				return new TestNode( projectName, tests );
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
					results.AddRange( runner.Results );

				return (TestResult[])results.ToArray( typeof(TestResult) );
			}
		}

		public virtual IFilter Filter
		{
			get { return this.filter; }
			set 
			{ 
				this.filter = value;
 
				foreach( TestRunner runner in runners )
					runner.Filter = filter;
			}
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

			TestSuiteResult result = new TestSuiteResult( new TestInfo( projectName, tests ), projectName );
			foreach( TestRunner runner in runners )
				result.Results.Add( runner.Run( listener ) );

			return result;
		}

		public virtual TestResult[] Run(EventListener listener, string[] testNames)
		{
			// Save active listener for derived classes
			this.listener = listener;
			ArrayList results = new ArrayList();

			foreach( TestRunner runner in runners )
				results.AddRange( runner.Run( listener, testNames ) );

			return (TestResult[])results.ToArray( typeof( TestResult ) );
		}

		public virtual void BeginRun( EventListener listener )
		{
			// Save active listener for derived classes
			this.listener = listener;

			System.Threading.Thread thread = new System.Threading.Thread( new System.Threading.ThreadStart( runnerProc ) );
			thread.Start();
		}

		private void runnerProc()
		{
			this.listener.RunStarted( new TestInfo[]{ this.Test } );

			foreach( TestRunner runner in runners )
				runner.Run( this );

			this.listener.RunFinished( this.Results );
		}

		public virtual void BeginRun( EventListener listener, string[] testNames )
		{
			// Save active listener for derived classes
			this.listener = listener;

			foreach( TestRunner runner in runners )
				runner.BeginRun( listener, testNames );
		}

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

		public void RunStarted(TestInfo[] tests)
		{
			// Ignore - we provide our own
		}

		public void RunFinished(Exception exception)
		{
			// Ignore - we provide our own
		}

		void NUnit.Core.EventListener.RunFinished(TestResult[] results)
		{
			// Ignore - we provide our own
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
	}
}
