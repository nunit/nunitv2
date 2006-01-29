using System;
using NUnit.Core;

namespace NUnit.Util
{
	#region TestEventHandler delegate

	/// <summary>
	/// The delegate for all events related to running tests
	/// </summary>
	public delegate void TestEventHandler ( object sender, TestEventArgs args );

	#endregion

	#region TestAction enumeration

	/// <summary>
	/// Enumeration used to distiguish test events
	/// </summary>
	public enum TestAction
	{
		// Project Load Events
		ProjectLoading,
		ProjectLoaded,
		ProjectLoadFailed,
		ProjectUnloading,
		ProjectUnloaded,
		ProjectUnloadFailed,
		// Test Load Events
		TestLoading,
		TestLoaded,
		TestLoadFailed,
		TestReloading,
		TestReloaded,
		TestReloadFailed,
		TestUnloading,
		TestUnloaded,
		TestUnloadFailed,
		// Test Run Events
		RunStarting,
		RunFinished,
		SuiteStarting,
		SuiteFinished,
		TestStarting,
		TestFinished,
		TestException,
		TestOutput
}

	#endregion

	/// <summary>
	/// Argument used for all test events
	/// </summary>
	public class TestEventArgs : EventArgs
	{
		#region Instance Variables

		// The action represented by the event
		private TestAction action;

		// The name of the test or other item
		private string name;
		
		// The tests we are running
		private TestInfo test;

		// The results from our tests
		private TestResult[] results;
		
		// The exception causing a failure
		private Exception exception;

		// The test output
		private TestOutput testOutput;

		// The number of tests we are running
		private int testCount;

		#endregion

		#region Constructors

		// TestLoaded, TestUnloaded, TestReloading, TestReloaded
		public TestEventArgs( TestAction action, 
			string name, TestInfo test )
		{
			this.action = action;
			this.name = name;
			this.test = test;
			if ( test != null )
				this.testCount = test.TestCount;
		}

		// ProjectLoading, ProjectLoaded, ProjectUnloading, ProjectUnloaded, TestLoading, TestUnloading
		public TestEventArgs( TestAction action, string name )
		{
			this.action = action;
			this.name = name;
		}

		public TestEventArgs( TestAction action, string name, int testCount )
		{
			this.action = action;
			this.name = name;
			this.testCount = testCount;
		}

		// ProjectLoadFailed, ProjectUnloadFailed, TestLoadFailed, TestUnloadFailed, TestReloadFailed
		public TestEventArgs( TestAction action,
			string name, Exception exception )
		{
			this.action = action;
			this.name = name;
			this.exception = exception;
		}

		// TestStarting, SuiteStarting, 
		public TestEventArgs( TestAction action, TestInfo test )
		{
			this.action = action;
			this.test = test;
		}

		// TestFinished, SuiteFinished
		public TestEventArgs( TestAction action, TestResult result )
		{
			this.action = action;
			this.results = new TestResult[] { result };
		}

		// RunFinished
		public TestEventArgs( TestAction action, TestResult[] results )
		{
			this.action = action;
			this.results = results;
		}

		// RunFinished, TestException
		public TestEventArgs( TestAction action, Exception exception )
		{
			this.action = action;
			this.exception = exception;
		}

		// TestOutput
		public TestEventArgs( TestAction action, TestOutput testOutput )
		{
			this.action = action;
			this.testOutput = testOutput;
		}

		// RunStarting
//		public TestEventArgs( TestAction action, TestInfo[] tests ) 
//		{
//			this.action = action;
//			this.tests = tests;
//		}

		#endregion

		#region Properties

		public TestAction Action
		{
			get { return action; }
		}

		public string Name
		{
			get { return name; }
		}

		public TestInfo Test
		{
			get { return test; }
		}

		public int TestCount 
		{
			get 
			{
//				int count = 0;
//				foreach( TestInfo test in tests )
//					count += test.TestCount;
//				return count; 

				return testCount;
			}
		}

		public TestResult Result
		{
			get { return results == null || results.Length == 0 ? null : results[0]; }
		}

		public TestResult[] Results
		{
			get { return results; }
		}

		public Exception Exception
		{
			get { return exception; }
		}

		public TestOutput TestOutput
		{
			get { return testOutput; }
		}

		#endregion
	}
}
