//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Tests
{
	using System;
	using System.Collections;
	using Nunit.Core;
	using Nunit.Framework;

	[TestFixture]
	public class TestCaseNameTest
	{
		[Test]
		public void TestName()
		{
			TestSuite suite = new TestSuite("mock suite");
			OneTestCase oneTestCase = new OneTestCase();
			suite.Add(oneTestCase);
			
			IList tests = suite.Tests;
			TestSuite rootSuite = (TestSuite)tests[0];
			TestCase testCase = (TestCase)rootSuite.Tests[0];
			Assertion.AssertEquals("Nunit.Tests.OneTestCase.TestCase", testCase.FullName);
			Assertion.AssertEquals("TestCase", testCase.Name);
		}

		[Test]
		public void TestExpectedException()
		{
			TestSuite suite = new TestSuite("mock suite");
			suite.Add(new ExpectExceptionTest());
 
			IList tests = suite.Tests;
			TestSuite rootSuite = (TestSuite)tests[0];
			TestCase testCase = (TestCase)rootSuite.Tests[0];
			Assertion.AssertEquals("Nunit.Tests.ExpectExceptionTest.TestSingle", testCase.FullName);
		}
	}
}
