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
		private Test[] tests;

		// The results from our tests
		private TestResult[] results;
		
		// The exception causing a failure
		private Exception exception;

		// The test output
		private TestOutput testOutput;

		// TODO: Remove this count of test cases
		private int count;

		#endregion

		#region Constructors

		public TestEventArgs( TestAction action, 
			string name, Test test )
		{
			this.action = action;
			this.name = name;
			this.tests = new Test[] { test };
		}

		public TestEventArgs( TestAction action, string name )
		{
			this.action = action;
			this.name = name;
		}

		public TestEventArgs( TestAction action,
			string name, Exception exception )
		{
			this.action = action;
			this.name = name;
			this.exception = exception;
		}

		public TestEventArgs( TestAction action, Test test )
		{
			this.action = action;
			this.tests = new Test[] { test };
			this.count = test.CountTestCases();
		}

		public TestEventArgs( TestAction action, TestResult result )
		{
			this.action = action;
			this.results = new TestResult[] { result };
		}

		public TestEventArgs( TestAction action, TestResult[] results )
		{
			this.action = action;
			this.results = results;
		}

		public TestEventArgs( TestAction action, Exception exception )
		{
			this.action = action;
			this.exception = exception;
		}

		public TestEventArgs( TestAction action, TestOutput testOutput )
		{
			this.action = action;
			this.testOutput = testOutput;
		}

		public TestEventArgs( TestAction action, Test[] tests, int count) 
		{
			this.action = action;
			this.tests = tests;
			this.count = count;
		}

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

		public Test Test
		{
			get { return tests == null || tests.Length == 0 ? null : tests[0]; }
		}

		public Test[] Tests 
		{
			get { return tests; }
		}

		public int TestCount 
		{
			get { return count; }
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
