//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Tests
{
	using System;
	using System.Collections;
	using NUnit.Core;
	using NUnit.Framework;

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
			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)rootSuite.Tests[0];
			Assertion.AssertEquals("NUnit.Tests.OneTestCase.TestCase", testCase.FullName);
			Assertion.AssertEquals("TestCase", testCase.Name);
		}

		[Test]
		public void TestExpectedException()
		{
			TestSuite suite = new TestSuite("mock suite");
			suite.Add(new ExpectExceptionTest());
 
			IList tests = suite.Tests;
			TestSuite rootSuite = (TestSuite)tests[0];
			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)rootSuite.Tests[0];
			Assertion.AssertEquals("NUnit.Tests.ExpectExceptionTest.TestSingle", testCase.FullName);
		}
	}
}
