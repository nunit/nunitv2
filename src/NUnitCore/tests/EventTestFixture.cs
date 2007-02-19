// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for EventTestFixture.
	/// </summary>
	/// 
	[TestFixture(Description="Tests that proper events are generated when running  test")]
	public class EventTestFixture
	{
		private string testsDll = "mock-assembly.dll";

		private static int SuiteCount(Test suite)
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
			internal int runStarted = 0;
			internal int runFinished = 0;
			internal int testCaseStart = 0;
			internal int testCaseFinished = 0;
			internal int suiteStarted = 0;
			internal int suiteFinished = 0;

			public void RunStarted(string name, int testCount)
			{
				runStarted++;
			}

			public void RunFinished(NUnit.Core.TestResult result)
			{
				runFinished++;
			}

			public void RunFinished(Exception exception)
			{
				runFinished++;
			}

			public void TestStarted(TestName testName)
			{
				testCaseStart++;
			}
			
			public void TestFinished(TestCaseResult result)
			{
				testCaseFinished++;
			}

			public void SuiteStarted(TestName suiteName)
			{
				suiteStarted++;
			}

			public void SuiteFinished(TestSuiteResult result)
			{
				suiteFinished++;
			}

			public void UnhandledException( Exception exception )
			{
			}

			public void TestOutput(TestOutput testOutput)
			{
			}
		}

		[Test]
		public void CheckEventListening()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			Test testSuite = builder.Build( new TestPackage( testsDll ) );
			
			EventCounter counter = new EventCounter();
			testSuite.Run(counter);
			Assert.AreEqual(testSuite.CountTestCases(TestFilter.Empty), counter.testCaseStart);
			Assert.AreEqual(testSuite.CountTestCases(TestFilter.Empty), counter.testCaseFinished);

			int suites = SuiteCount(testSuite);
			Assert.AreEqual(suites, counter.suiteStarted);
			Assert.AreEqual(suites, counter.suiteFinished);
		}
	}
}

