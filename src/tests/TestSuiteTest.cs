//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
//
namespace NUnit.Tests
{
	using System;
	using NUnit.Framework;
	using NUnit.Core;
	using NUnit.Tests.Assemblies;
	using System.Collections;

	/// <summary>
	/// Summary description for TestSuiteTest.
	/// </summary>
	/// 
	[TestFixture]
	public class TestSuiteTest
	{
		OneTestCase oneTestFixture;
		MockTestFixture mockTestFixture;
		TestSuite noTestSuite;

		[SetUp]
		public void SetUp()
		{
			oneTestFixture = new OneTestCase();
			mockTestFixture = new MockTestFixture();

			EmptyFixture fixture = new EmptyFixture();
			noTestSuite = new TestSuite("No Tests");
			noTestSuite.Add(fixture);
		}

		[Test]
		public void AddTestFixture()
		{
			TestSuite testSuite = new TestSuite("Mock Test Suite");
			testSuite.Add(oneTestFixture);

			ArrayList tests = testSuite.Tests;
			Test test = (Test)tests[0];
			Assertion.Assert("Expected a TestSuite",test is TestSuite);
			Assertion.AssertEquals("OneTestCase",test.Name);
		}

		[Test]
		public void TestCaseCountinFixture()
		{
			TestSuite testSuite = new TestSuite("Mock Test Suite");
			testSuite.Add(mockTestFixture);

			ArrayList tests = testSuite.Tests;
			Test test = (Test)tests[0];
			Assertion.Assert("Expected a TestSuite",test is TestSuite);
			Assertion.AssertEquals(mockTestFixture.GetType().Name,test.Name);

			Assertion.AssertEquals(5, testSuite.CountTestCases);
		}

		[Test]
		public void InheritedTestCount()
		{
			InheritedTestFixture testFixture = new InheritedTestFixture();
			TestSuite suite = new TestSuite("mock");
			suite.Add(testFixture);

			Assertion.AssertEquals(2, suite.CountTestCases);
		}

		[Test]
		public void SuiteRunInitialized()
		{
			MockTestFixture fixture = new MockTestFixture();
			TestSuite suite = new TestSuite("mock");
			suite.Add(fixture);

			Assertion.Assert("default state is to run TestSuite", suite.ShouldRun);
		}

		[Test]
		public void SuiteWithNoTests()
		{
			ArrayList tests = noTestSuite.Tests;
			Assertion.AssertEquals(1, tests.Count);
			TestSuite testSuite = (TestSuite)tests[0];

			Assertion.Assert("ShouldRun should be false because there are no tests", !testSuite.ShouldRun);
			Assertion.AssertEquals(testSuite.Name + " does not have any tests", testSuite.IgnoreReason);
		}

		[Test]
		public void RunNoTestSuite()
		{
			Assertion.AssertEquals(0, noTestSuite.CountTestCases);
			
			TestResult result = noTestSuite.Run(NullListener.NULL);

			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assertion.AssertEquals(0, summarizer.ResultCount);
			Assertion.AssertEquals(0, summarizer.TestsNotRun);
			Assertion.AssertEquals(1, summarizer.SuitesNotRun);
		}
	}
}
