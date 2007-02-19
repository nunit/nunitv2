// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

namespace NUnit.Core.Tests
{
	using System;
	using NUnit.Framework;	
	using NUnit.Core;

	/// <summary>
	/// Summary description for TestResultTests.
	/// </summary>
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
		public void TestCaseDefault()
		{
			Assert.AreEqual( RunState.Runnable, caseResult.RunState );
		}

		[Test]
		public void TestCaseSuccess()
		{
			caseResult.Success();
			Assert.IsTrue(caseResult.IsSuccess, "result should be success");
		}

		[Test]
		public void TestCaseNotRun()
		{
			caseResult.Ignore( "because" );
			Assert.AreEqual( false, caseResult.Executed );
			Assert.AreEqual( "because", caseResult.Message );
		}

		[Test]
		public void TestCaseFailure()
		{
			caseResult.Failure("message", "stack trace");
			Assert.IsTrue(caseResult.IsFailure);
			Assert.IsFalse(caseResult.IsSuccess);
			Assert.AreEqual("message",caseResult.Message);
			Assert.AreEqual("stack trace",caseResult.StackTrace);
		}
	}
}
