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
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.Assemblies;
using System.Collections;

namespace NUnit.Tests.Core
{
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
			Assert.IsTrue(test is TestSuite, "Expected a TestSuite");
			Assert.AreEqual("OneTestCase",test.Name);
		}

		[Test]
		public void TestCaseCountinFixture()
		{
			TestSuite testSuite = new TestSuite("Mock Test Suite");
			testSuite.Add(mockTestFixture);

			ArrayList tests = testSuite.Tests;
			Test test = (Test)tests[0];
			Assert.IsTrue(test is TestSuite, "Expected a TestSuite");
			Assert.AreEqual(mockTestFixture.GetType().Name,test.Name);

			Assert.AreEqual(5, testSuite.CountTestCases);
		}

		[Test]
		public void InheritedTestCount()
		{
			InheritedTestFixture testFixture = new InheritedTestFixture();
			TestSuite suite = new TestSuite("mock");
			suite.Add(testFixture);

			Assert.AreEqual(2, suite.CountTestCases);
		}

		[Test]
		public void SuiteRunInitialized()
		{
			MockTestFixture fixture = new MockTestFixture();
			TestSuite suite = new TestSuite("mock");
			suite.Add(fixture);

			Assert.IsTrue(suite.ShouldRun, "default state is to run TestSuite");
		}

		[Test]
		public void SuiteWithNoTests()
		{
			ArrayList tests = noTestSuite.Tests;
			Assert.AreEqual(1, tests.Count);
			TestSuite testSuite = (TestSuite)tests[0];

			Assert.IsFalse(testSuite.ShouldRun,
				"ShouldRun should be false because there are no tests");
			Assert.AreEqual(testSuite.Name + " does not have any tests", testSuite.IgnoreReason);
		}

		[Test]
		public void RunNoTestSuite()
		{
			Assert.AreEqual(0, noTestSuite.CountTestCases);
			
			TestResult result = noTestSuite.Run(NullListener.NULL);

			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assert.AreEqual(0, summarizer.ResultCount);
			Assert.AreEqual(0, summarizer.TestsNotRun);
			Assert.AreEqual(1, summarizer.SuitesNotRun);
		}
	}
}
