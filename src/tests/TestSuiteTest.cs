/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
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
