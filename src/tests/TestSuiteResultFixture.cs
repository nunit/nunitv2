//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Tests.TestResultTests
{
	using System;
	using System.Collections;
	using Nunit.Framework;
	using Nunit.Core;
	
	/// <summary>
	/// Summary description for TestResultTests.
	/// </summary>
	/// 
	[TestFixture]
	public class TestSuiteResultFixture
	{
		private TestCaseResult testCase;

		private TestSuiteResult MockSuiteResult()
		{
			TestSuiteResult result = new TestSuiteResult(null, "base");

			TestSuiteResult level1SuiteA = new TestSuiteResult(null, "level 1 A");
			result.AddResult(level1SuiteA);

			TestSuiteResult level1SuiteB = new TestSuiteResult(null, "level 1 B");
			result.AddResult(level1SuiteB);

			testCase = new TestCaseResult("a test case");
			level1SuiteA.AddResult(testCase);

			level1SuiteB.AddResult(new TestCaseResult("a successful test"));

			return result;
		}

		[Test]
		public void EmptySuite()
		{
			TestSuiteResult result = new TestSuiteResult(null, "base suite");
			Assertion.Assert("result should be success", result.IsSuccess);
		}

		[Test]
		public void SuiteSuccess()
		{
			Assertion.Assert(MockSuiteResult().IsSuccess);
		}

		[Test]
		public void TestSuiteFailure()
		{
			TestSuiteResult result = MockSuiteResult();
			AssertionException failure = new AssertionException("an assertion failed error");
			testCase.Failure(failure.Message, failure.StackTrace);
			
			Assertion.Assert(result.IsFailure);
			Assertion.Assert(!result.IsSuccess); 

			IList results = result.Results;
			TestSuiteResult suiteA = (TestSuiteResult)results[0];
			Assertion.Assert(suiteA.IsFailure);

			TestSuiteResult suiteB = (TestSuiteResult)results[1];
			Assertion.Assert(suiteB.IsSuccess);
		}		
	}
}
