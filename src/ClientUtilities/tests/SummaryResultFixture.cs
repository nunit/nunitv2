// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Core;
	
namespace NUnit.Util.Tests
{
	/// <summary>
	/// Summary description for TestResultTests.
	/// </summary>
	[TestFixture]
	public class SummaryResultFixture
	{
		private TestCaseResult testCase;
		private double time = 0.456;

		private TestSuiteResult NotRunTestSuite()
		{
			TestSuiteResult result = new TestSuiteResult("RootSuite");
			result.RunState = RunState.Executed;

			TestCaseResult testCaseResult = new TestCaseResult("NonRunTestCase");
			testCaseResult.Ignore("No Reason");
			result.AddResult(testCaseResult);

			return result;
		}

		[Test]
		public void TestCountNotRunSuites()
		{
			ResultSummarizer summary = new ResultSummarizer(NotRunTestSuite());
			Assert.AreEqual(1,summary.TestsNotRun);

		}
		private TestSuiteResult MockSuiteResult(string suiteName, bool failure)
		{
			TestSuiteResult result = new TestSuiteResult(suiteName);
			result.Time = time;
			result.RunState = RunState.Executed;

			TestSuiteResult level1SuiteA = new TestSuiteResult("level 1 A");
			result.AddResult(level1SuiteA);
			level1SuiteA.RunState = RunState.Executed;

			TestSuiteResult level1SuiteB = new TestSuiteResult("level 1 B");
			result.AddResult(level1SuiteB);
			level1SuiteB.RunState = RunState.Executed;

			testCase = new TestCaseResult("a test case");
			if(failure) testCase.Failure("argument exception",null);
			else testCase.Success();
			
			level1SuiteA.AddResult(testCase);

			testCase = new TestCaseResult("a successful test");
			testCase.Success();
			level1SuiteB.AddResult(testCase);

			testCase = new TestCaseResult("a not run test");
			testCase.Ignore("test not run");
			level1SuiteB.AddResult(testCase);

			return result;
		}

		[Test]
		public void TotalCountSuccess()
		{
			string suiteName = "Base";
			ResultSummarizer summary = new ResultSummarizer(MockSuiteResult(suiteName, false));

			Assert.AreEqual(suiteName, summary.Name);
			Assert.IsTrue(summary.Success);
			Assert.AreEqual(2, summary.ResultCount);
			Assert.AreEqual(0, summary.FailureCount);
			Assert.AreEqual(1, summary.TestsNotRun);
		}

		[Test]
		public void Failure()
		{
			ResultSummarizer summary = new ResultSummarizer(MockSuiteResult("Base", true));

			Assert.IsFalse(summary.Success);
			Assert.AreEqual(2, summary.ResultCount);
			Assert.AreEqual(1, summary.FailureCount);
			Assert.AreEqual(1, summary.TestsNotRun);
		}

		[Test]
		public void TestTime()
		{
			ResultSummarizer summary = new ResultSummarizer(MockSuiteResult("Base", false));
			Assert.AreEqual(time, summary.Time);
		}
	}
}
