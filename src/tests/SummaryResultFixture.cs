//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Tests
{
	using System;
	using System.Collections;
	using NUnit.Framework;
	using NUnit.Core;
	
	/// <summary>
	/// Summary description for TestResultTests.
	/// </summary>
	/// 
	[TestFixture]
	public class SummaryResultFixture
	{
		private TestCaseResult testCase;
		private double time = 0.456;

		private TestSuiteResult NotRunTestSuite()
		{
			TestSuiteResult result = new TestSuiteResult(null,"RootSuite");
			result.Executed = false;

			TestCaseResult testCaseResult = new TestCaseResult("NonRunTestCase");
			testCaseResult.NotRun("No Reason");
			result.AddResult(testCaseResult);

			return result;
		}

		[Test]
		public void TestCountNotRunSuites()
		{
			ResultSummarizer summary = new ResultSummarizer(NotRunTestSuite());
			Assertion.AssertEquals(1,summary.TestsNotRun);

		}
		private TestSuiteResult MockSuiteResult(string suiteName, bool failure)
		{
			TestSuiteResult result = new TestSuiteResult(null, suiteName);
			result.Time = time;
			result.Executed = true;

			TestSuiteResult level1SuiteA = new TestSuiteResult(null, "level 1 A");
			result.AddResult(level1SuiteA);
			level1SuiteA.Executed = true;

			TestSuiteResult level1SuiteB = new TestSuiteResult(null, "level 1 B");
			result.AddResult(level1SuiteB);
			level1SuiteB.Executed = true;

			testCase = new TestCaseResult("a test case");
			if(failure) testCase.Failure("argument exception",null);
			else testCase.Success();
			
			level1SuiteA.AddResult(testCase);

			testCase = new TestCaseResult("a successful test");
			testCase.Success();
			level1SuiteB.AddResult(testCase);

			testCase = new TestCaseResult("a not run test");
			testCase.NotRun("test not run");
			level1SuiteB.AddResult(testCase);

			return result;
		}

		[Test]
		public void TotalCountSuccess()
		{
			string suiteName = "Base";
			ResultSummarizer summary = new ResultSummarizer(MockSuiteResult(suiteName, false));

			Assertion.AssertEquals(suiteName, summary.Name);
			Assertion.Assert(summary.Success);
			Assertion.AssertEquals(2, summary.ResultCount);
			Assertion.AssertEquals(0, summary.Failures);
			Assertion.AssertEquals(1, summary.TestsNotRun);
		}

		[Test]
		public void Failure()
		{
			ResultSummarizer summary = new ResultSummarizer(MockSuiteResult("Base", true));

			Assertion.Assert(!summary.Success);
			Assertion.AssertEquals(2, summary.ResultCount);
			Assertion.AssertEquals(1, summary.Failures);
			Assertion.AssertEquals(1, summary.TestsNotRun);
		}

		[Test]
		public void TestTime()
		{
			ResultSummarizer summary = new ResultSummarizer(MockSuiteResult("Base", false));
			Assertion.AssertEquals(time, summary.Time);
		}
	}
}
