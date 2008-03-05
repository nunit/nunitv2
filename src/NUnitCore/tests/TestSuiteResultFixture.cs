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
	using NUnit.Tests.Assemblies;
	
	/// <summary>
	/// Summary description for TestResultTests.
	/// </summary>
	/// 
	[TestFixture]
	public class TestSuiteResultFixture
	{
		private TestSuiteResult result;

		[SetUp]
		public void CreateResult()
		{
			Test testFixture = TestFixtureBuilder.BuildFrom( typeof( MockTestFixture ) );
			result = testFixture.Run( NullListener.NULL ) as TestSuiteResult;
		}

		//private TestCaseResult testCase;

//		private TestSuiteResult MockSuiteResult( bool failure )
//		{
//			TestCaseResult testCaseResult = new TestCaseResult( "a test case");
//			if ( failure ) testCaseResult.Failure( "case failed", null );
//
//			TestSuiteResult level1SuiteA = new TestSuiteResult("level 1 A");
//			level1SuiteA.AddResult(testCaseResult);
//
//			TestSuiteResult level1SuiteB = new TestSuiteResult("level 1 B");
//			level1SuiteB.AddResult(new TestCaseResult("a successful test"));
//
//			TestSuiteResult result = new TestSuiteResult("base");
//			result.AddResult(level1SuiteA);
//			result.AddResult(level1SuiteB);
//
//			return result;
//		}

		[Test]
		public void EmptySuite()
		{
			TestSuiteResult result = new TestSuiteResult( new TestInfo( new TestSuite( "empty suite") ) );
			Assert.IsTrue(result.IsSuccess, "result should be success");
		}

		[Test]
		public void SuiteSuccess()
		{
			Assert.IsTrue( result.IsSuccess);
		}

//		[Test]
//		public void TestSuiteFailure()
//		{
////			AssertionException failure = new AssertionException("an assertion failed error");
////			testCase.Failure(failure.Message, failure.StackTrace);
//
////			TestSuiteResult result = MockSuiteResult( true );
//			
//			Assert.IsTrue(result.IsFailure);
//			Assert.IsFalse(result.IsSuccess); 
//
//			IList results = result.Results;
//			TestSuiteResult suiteA = (TestSuiteResult)results[0];
//			Assert.IsTrue(suiteA.IsFailure);
//
//			TestSuiteResult suiteB = (TestSuiteResult)results[1];
//			Assert.IsTrue(suiteB.IsSuccess);
//		}		
	}
}
