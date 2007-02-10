#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

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
