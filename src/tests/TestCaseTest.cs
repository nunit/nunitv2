//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Tests
{
	using System;
	using Nunit.Framework;
	using Nunit.Tests.Assemblies;
	using Nunit.Core;

	/// <summary>
	/// Summary description for TestCaseTest.
	/// </summary>
	/// 
	[TestFixture]
	public class TestCaseTest
	{
		[Test]
		public void CreateIgnoredTestCase()
		{
			MockTestFixture mockTestFixture = new MockTestFixture();
			TestCase testCase = TestCaseBuilder.Make(mockTestFixture, "MockTest4"); 
			Assertion.AssertEquals(1, testCase.CountTestCases);
			Assertion.AssertEquals(false, testCase.ShouldRun);
			Assertion.AssertEquals("ignoring this test method for now", testCase.IgnoreReason);
		}

		[Test]
		public void RunIgnoredTestCase()
		{
			MockTestFixture mockTestFixture = new MockTestFixture();
			TestCase testCase = TestCaseBuilder.Make(mockTestFixture, "MockTest4"); 
			Assertion.AssertEquals(1, testCase.CountTestCases);
			
			TestResult result = testCase.Run(NullListener.NULL);
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assertion.AssertEquals(0, summarizer.ResultCount);
			Assertion.AssertEquals(1, summarizer.TestsNotRun);
		}
	}
}
