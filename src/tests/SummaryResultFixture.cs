#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

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
