// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

namespace NUnit.Core.Tests
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
	public class TestSuiteResultFixture
	{
		//private TestCaseResult testCase;

		private TestSuiteResult MockSuiteResult( bool failure )
		{
			TestCaseResult testCaseResult = new TestCaseResult("a test case");
			if ( failure ) testCaseResult.Failure( "case failed", null );

			TestSuiteResult level1SuiteA = new TestSuiteResult("level 1 A");
			level1SuiteA.AddResult(testCaseResult);

			TestSuiteResult level1SuiteB = new TestSuiteResult("level 1 B");
			level1SuiteB.AddResult(new TestCaseResult("a successful test"));

			TestSuiteResult result = new TestSuiteResult("base");
			result.AddResult(level1SuiteA);
			result.AddResult(level1SuiteB);

			return result;
		}

		[Test]
		public void EmptySuite()
		{
			TestSuiteResult result = new TestSuiteResult("base suite");
			Assert.IsTrue(result.IsSuccess, "result should be success");
		}

		[Test]
		public void SuiteSuccess()
		{
			Assert.IsTrue(MockSuiteResult( false ).IsSuccess);
		}

		[Test]
		public void TestSuiteFailure()
		{
//			AssertionException failure = new AssertionException("an assertion failed error");
//			testCase.Failure(failure.Message, failure.StackTrace);

			TestSuiteResult result = MockSuiteResult( true );
			
			Assert.IsTrue(result.IsFailure);
			Assert.IsFalse(result.IsSuccess); 

			IList results = result.Results;
			TestSuiteResult suiteA = (TestSuiteResult)results[0];
			Assert.IsTrue(suiteA.IsFailure);

			TestSuiteResult suiteB = (TestSuiteResult)results[1];
			Assert.IsTrue(suiteB.IsSuccess);
		}		
	}
}
