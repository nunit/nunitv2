//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Tests.TestResultSuite
{
	using System;
	using NUnit.Framework;	
	using NUnit.Core;

	/// <summary>
	/// Summary description for TestResultTests.
	/// </summary>
	/// 
	[TestFixture]
	public class TestCaseResultFixture
	{
		private TestCaseResult caseResult;

		[SetUp]
		public void SetUp()
		{
			caseResult = new TestCaseResult("test case result");
		}

		[Test]
		public void TestCaseSuccess()
		{
			Assertion.Assert("result should be success", caseResult.IsSuccess);
		}

		[Test]
		public void TestCaseFailure()
		{
			caseResult.Failure("an assertion failed error",null);
			Assertion.Assert(caseResult.IsFailure);
			Assertion.Assert(!caseResult.IsSuccess);
		}

		[Test]
		public void TestExceptionContents()
		{
			string message = "message";
			caseResult.Failure(message,null);
			Assertion.AssertEquals(message, caseResult.Message);
		}
	}
}
