namespace Nunit.Tests
{
	using System;
	using Nunit.Framework;
	using Nunit.Core;

	/// <summary>
	/// Summary description for EventTestFixture.
	/// </summary>
	/// 
	[TestFixture]
	public class EventTestFixture
	{
		private string testsDll = "mock-assembly.dll";

		private static int SuiteCount(TestSuite suite)
		{
			int suites = 1;

			foreach(Test test in suite.Tests)
			{
				if(test is TestSuite)
					suites += SuiteCount((TestSuite)test);
			}

			return suites;
		}

		internal class EventCounter : EventListener
		{
			internal int testCaseStart = 0;
			internal int testCaseFinished = 0;
			internal int suiteStarted = 0;
			internal int suiteFinished = 0;

			public void TestStarted(TestCase testCase)
			{
				testCaseStart++;
			}
			
			public void TestFinished(TestCaseResult result)
			{
				testCaseFinished++;
			}

			public void SuiteStarted(TestSuite suite)
			{
				suiteStarted++;
			}

			public void SuiteFinished(TestSuiteResult result)
			{
				suiteFinished++;
			}
		}

		[Test]
		public void CheckEventListening()
		{
			TestSuite testSuite = TestSuiteBuilder.Build(testsDll);
			
			EventCounter counter = new EventCounter();
			TestResult result = testSuite.Run(counter);
			Assertion.AssertEquals(testSuite.CountTestCases, counter.testCaseStart);
			Assertion.AssertEquals(testSuite.CountTestCases, counter.testCaseFinished);

			int suites = SuiteCount(testSuite);
			Assertion.AssertEquals(suites, counter.suiteStarted);
			Assertion.AssertEquals(suites, counter.suiteFinished);
		}
	}
}
