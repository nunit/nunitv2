namespace NUnit.Tests
{
	using System;
	using NUnit.Core;
	using NUnit.Util;
	using NUnit.UiKit;

	/// <summary>
	/// Summary description for MockUiEventSource.
	/// </summary>
	public class MockTestEventSource : TestEventDispatcher
	{
		private string testFileName;
		private Test test;

		public MockTestEventSource( string testFileName, Test test )
		{
			this.test = test;
			this.testFileName = testFileName;
		}

		public void SimulateTestRun()
		{
			FireRunStarting( test );

			TestResult result = SimulateTest( test );

			FireRunFinished( result );
		}

		private TestResult SimulateTest( Test test )
		{
			if ( test.IsSuite )
			{
				FireSuiteStarting( test );

				TestSuiteResult result = new TestSuiteResult( test, test.Name );

				foreach( Test childTest in test.Tests )
					result.AddResult( SimulateTest( childTest ) );

				FireSuiteFinished( result );

				return result;
			}
			else
			{
				FireTestStarting( test );
				
				TestCaseResult result = new TestCaseResult( test as TestCase );
				result.Executed = test.ShouldRun;
				
				FireTestFinished( result );

				return result;
			}
		}
	}
}
