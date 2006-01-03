using System;
using System.IO;
using System.Threading;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// SimpleTestRunner is the simplest direct-running TestRunner. It
	/// passes the event listener interface that is provided on to the tests
	/// to use directly and does nothing to redirect text output. Both
	/// Run and BeginRun are actually synchronous, although the client
	/// can usually ignore this. BeginRun + EndRun operates as expected.
	/// </summary>
	public class SimpleTestRunner : LongLivingMarshalByRefObject, TestRunner
	{
		#region Instance Variables

		/// <summary>
		/// The loaded test suite
		/// </summary>
		private TestSuite suite;

		/// <summary>
		/// Saved paths of the assemblies we loaded - used to set 
		/// current directory when we are running the tests.
		/// </summary>
		private string[] assemblies;

		/// <summary>
		/// The currently set filter
		/// </summary>
		private IFilter testFilter;

		/// <summary>
		/// Results from the last test run
		/// </summary>
		private TestResult[] testResults;

		/// <summary>
		/// The thread on which Run was called. Set to the
		/// current thread while a run is in process.
		/// </summary>
		private Thread runThread;
		#endregion

		#region Constructor
		public SimpleTestRunner()
		{
			this.testFilter = EmptyFilter.Empty;
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
		
		public virtual bool Running
		{
			get { return runThread != null && runThread.IsAlive; }
		}
		#endregion

		#region Methods for Loading Tests

		/// <summary>
		/// Load an assembly
		/// </summary>
		/// <param name="assemblyName"></param>
		public TestNode Load( string assemblyName )
		{
			return Load( assemblyName, string.Empty );
		}

		/// <summary>
		/// Load a particular test in an assembly
		/// </summary>
		public TestNode Load( string assemblyName, string testName )
		{
			this.assemblies = new string[] { assemblyName };
			TestSuiteBuilder builder = new TestSuiteBuilder();
			this.suite = builder.Build( assemblyName, testName );
			return suite == null ? null : new TestNode( suite );
		}

		/// <summary>
		/// Load the assemblies in a test project
		/// </summary>
		/// <param name="projectName">The name of the test project being loaded</param>
		/// <param name="assemblies">The assemblies comprising the project</param>
		/// <returns>The loaded test</returns>
		public TestNode Load( string projectName, string[] assemblies )
		{
			return Load( projectName, assemblies, string.Empty );
		}

		/// <summary>
		/// Load a particular test in a TestProject.
		/// </summary>
		/// <param name="projectName">The name of the test project being loaded</param>
		/// <param name="assemblies">The assemblies comprising the project</param>
		/// <param name="testName">The name of the test fixture or suite to be loaded</param>
		/// <returns>The loaded test</returns>
		public TestNode Load( string projectName, string[] assemblies, string testName )
		{
			this.assemblies = (string[])assemblies.Clone();
			TestSuiteBuilder builder = new TestSuiteBuilder();
			this.suite = builder.Build( projectName, assemblies, testName );
			return suite == null ? null : new TestNode( suite );
		}

		/// <summary>
		/// Unload all tests previously loaded
		/// </summary>
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
			TestResult[] results = Run( listener, tests );
			return results[0];
		}

		public virtual TestResult[] Run( EventListener listener, string[] testNames )
		{
			if ( testNames == null || testNames.Length == 0 )
				return Run( listener, new Test[] { suite } );
			else
				return Run( listener, FindTests( suite, testNames ) );
		}

		public void BeginRun( EventListener listener )
		{
			BeginRun( listener, null );
		}

		public virtual void BeginRun( EventListener listener, string[] testNames )
		{
			testResults = this.Run( listener, testNames );
		}

		public virtual TestResult[] EndRun()
		{
			return Results;
		}

		/// <summary>
		/// Wait is a NOP for SimpleTestRunner
		/// </summary>
		public virtual void Wait()
		{
		}

		public virtual void CancelRun()
		{
			if (this.runThread != null)
			{
				// Cancel Synchronous run only if on another thread
				if ( runThread == Thread.CurrentThread )
					throw new InvalidOperationException( "May not CancelRun on same thread that is running the test" );

				// Make a copy of runThread, which will be set to 
				// null when the thread terminates.
				Thread cancelThread = this.runThread;

				// Tell the thread to abort
				this.runThread.Abort();
				
				// Wake up the thread if necessary
				// Figure out if we need to do an interupt
				if ( (cancelThread.ThreadState & ThreadState.WaitSleepJoin ) != 0 )
					cancelThread.Interrupt();
			}
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

		/// <summary>
		/// Private method to run a set of tests. This routine is the workhorse
		/// that is called anytime tests are run.
		/// </summary>
		private TestResult[] Run( EventListener listener, Test[] tests )
		{
			Addins.Save();

			try
			{
				// Take note of the fact that we are running
				this.runThread = Thread.CurrentThread;

				// Create an array for the results
				testResults = new TestResult[ tests.Length ];

				// Signal that we are starting the run
				TestInfo[] info = new TestInfo[tests.Length];
				int index = 0;
				foreach( Test test in tests )
					info[index++] = new TestInfo( test );
				listener.RunStarted( info );
				
				// Run each test, saving the results
				index = 0;
				foreach( Test test in tests )
				{
					string workingDir = null;
					if ( !(test is TestAssembly) ) // TestAssembly sets the cwd itself
					{
						Test t = test;
						do
						{
							t = t.Parent;
						}
						while( t != null && !(t is TestAssembly) );
						
						if ( t != null )
							workingDir = Path.GetDirectoryName( t.Name );
					}

					using( new DirectorySwapper( workingDir ) )
					{
						testResults[index++] = test.Run( listener, Filter );
					}
				}

				// Signal that we are done
				listener.RunFinished( testResults );

				// Return result array
				return testResults;
			}
			catch( Exception exception )
			{
				// Signal that we finished with an exception
				listener.RunFinished( exception );
				// Rethrow - should we do this?
				throw;
			}
			finally
			{
				runThread = null;
				Addins.Restore();
			}
		}
		#endregion
	}
}
