using System;
using System.IO;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// AbstractTestRunner is the abstract base class for TestRunner 
	/// implementations that run tests directly rather than delegating
	/// to a downstream runner. Only one method must be overridden to
	/// derive a functional TestRunner.
	/// </summary>
	public abstract class AbstractTestRunner : LongLivingMarshalByRefObject, TestRunner
	{
		#region Instance variables
		/// <summary>
		/// The loaded test suite
		/// </summary>
		private TestSuite suite;

		/// <summary>
		/// Saved paths of the assemblies we loaded - used to set 
		/// current directory when we are running the tests.
		/// </summary>
		protected string[] assemblies;

		/// <summary>
		/// The currently set filter
		/// </summary>
		protected IFilter testFilter;

		/// <summary>
		/// Results from the last test run
		/// </summary>
		protected TestResult[] testResults;

		#endregion

		#region Constructors

		/// <summary>
		/// Construct with stdOut and stdErr writers
		/// </summary>
		public AbstractTestRunner()
		{
			testFilter = EmptyFilter.Empty;
		}

		#endregion

		#region Properties

		public IList TestFrameworks
		{
			get { return TestFramework.GetLoadedFrameworks(); }
		}

		/// <summary>
		/// Results from the last test run
		/// </summary>
		public TestResult[] Results
		{
			get { return testResults; }
		}

		public IFilter Filter
		{
			get { return testFilter; }
			set { testFilter = value; }
		}
		#endregion

		#region Methods for Loading Tests

		/// <summary>
		/// Load an assembly
		/// </summary>
		/// <param name="assemblyName"></param>
		public Test Load( string assemblyName )
		{
			this.assemblies = new string[] { assemblyName };
			TestSuiteBuilder builder = new TestSuiteBuilder();
			this.suite = builder.Build( assemblyName );
			return suite;
		}

		/// <summary>
		/// Load a particular test in an assembly
		/// </summary>
		public Test Load( string assemblyName, string testName )
		{
			this.assemblies = new string[] { assemblyName };
			TestSuiteBuilder builder = new TestSuiteBuilder();
			this.suite = builder.Build( assemblyName, testName );
			return suite;
		}

		public Test Load( TestProject testProject )
		{
			this.assemblies = (string[])testProject.Assemblies.Clone();
			TestSuiteBuilder builder = new TestSuiteBuilder();
			this.suite = builder.Build( testProject );
			return suite;
		}

		/// <summary>
		/// Load a particular test from a test project
		/// </summary>
		public Test Load( TestProject project, string testName )
		{
			this.assemblies = (string[])project.Assemblies.Clone();
			TestSuiteBuilder builder = new TestSuiteBuilder();
			this.suite = builder.Build( project, testName );
			return suite;
		}

		public void Unload()
		{
			this.suite = null; // All for now
		}

		#endregion

		#region Methods for Counting TestCases

		public int CountTestCases( string testName )
		{
			Test test = FindTest( suite, testName );
			return test == null ? 0 : test.CountTestCases();
		}

		public int CountTestCases(string[] testNames ) 
		{
			int count = 0;
			foreach( string testName in testNames)
				count += this.CountTestCases( testName );

			return count;
		}

		#endregion

		#region GetCategories Method
		public ICollection GetCategories()
		{
			return CategoryManager.Categories;
		}
		#endregion

		#region Methods for Running Tests

		public virtual TestResult Run( EventListener listener )
		{
			Test[] tests = new Test[] { suite };
			TestResult[] results = doRun( listener, tests );
			return results[0];
		}

		public virtual TestResult[] Run( EventListener listener, string[] testNames )
		{
			if ( testNames == null || testNames.Length == 0 )
				return doRun( listener, new Test[] { suite } );
			else
				return doRun( listener, FindTests( suite, testNames ) );
		}

#if STARTRUN_SUPPORT
		public virtual void StartRun( EventListener listener )
		{
			doStartRun( listener, null );
		}

		public virtual void StartRun( EventListener listener, string[] testNames )
		{
			if ( testNames == null || testNames.Length == 0 )
				doStartRun( listener, null );
			else
				doStartRun( listener, testNames );
		}
#endif

		public virtual void Wait()
		{
			// Wait isn't implemented!!!
			// throw new NotImplementedException();
		}

		#endregion

		#region Helper Routines

		private Test FindTest(Test test, string fullName)
		{
			if(test.UniqueName.Equals(fullName)) return test;
			if(test.FullName.Equals(fullName)) return test;
			
			Test result = null;
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

		private Test[] FindTests( Test test, string[] names )
		{
			Test[] tests = new Test[ names.Length ];

			int index = 0;
			foreach( string name in names )
				tests[index++] = FindTest( test, name );

			return tests;
		}
		#endregion

		#region Abstract Properties and Methods
		/// <summary>
		/// Override to indicate whether a test is running
		/// </summary>
		public abstract bool Running
		{
			get;
		}

		/// <summary>
		/// All Run calls eventually come down to this method, which
		/// must be overridden by each derived class.
		/// </summary>
		/// <param name="listener">Interface to receive EventListener notifications</param></param>
		/// <param name="tests">Array of Tests to be run</param>
		/// <returns>Array of TestResults corresponding to each Test run</returns>
		protected abstract TestResult[] doRun( EventListener listener, Test[] tests );

#if STARTRUN_SUPPORT
		/// <summary>
		/// All StartRun calls eventually come down to this method, which
		/// must be overridden by each derived class.
		/// </summary>
		/// <param name="listener">Interface to receive EventListener notifications</param></param>
		/// <param name="tests">Names of the tests to be run</param>
		protected abstract void doStartRun( EventListener listener, string[] testNames );
#endif

		/// <summary>
		/// Override this methnod to Cancel a run, if possible
		/// </summary>
		public abstract void CancelRun();

		#endregion
	}

}
