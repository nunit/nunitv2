//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Tests
{
	using System;
	using System.Collections;
	using System.Reflection;
	using Nunit.Framework;
	using Nunit.Core;
	using Nunit.Tests.Assemblies;

	/// <summary>
	/// Summary description for NamespaceAssemblyTests.
	/// </summary>
	/// 
	[TestFixture]
	public class NamespaceAssemblyTests
	{
		private string testsDll = "mock-assembly.dll";
		private Assembly testAssembly;
		private Type assemblyTestType;
		
		[SetUp]
		public void SetUp() 
		{
			testAssembly = TestSuiteBuilder.Load(testsDll);
			assemblyTestType = testAssembly.GetType("Nunit.Tests.OneTestCase");
		}

		[Test]
		public void LoadTestFixtureFromAssembly()
		{
			TestSuite suite = TestSuiteBuilder.Build("Nunit.Tests.Assemblies.MockTestFixture", testsDll);
			Assertion.Assert(suite != null);
		}

		[Test]
		public void TestRoot()
		{
			TestSuite suite = TestSuiteBuilder.Build(testsDll);
			Assertion.AssertEquals(testsDll, suite.Name);
		}

		[Test]
		public void TestHiearchy()
		{
			TestSuite suite = TestSuiteBuilder.Build(testsDll);
			ArrayList tests = suite.Tests;
			Assertion.AssertEquals(1, tests.Count);

			Assertion.Assert("TestSuite:NUnit - is not correct", tests[0] is TestSuite);
			TestSuite testSuite = (TestSuite)tests[0];
			Assertion.AssertEquals("Nunit", testSuite.Name);

			tests = testSuite.Tests;
			Assertion.Assert("TestSuite:Tests - is invalid", tests[0] is TestSuite);
			testSuite = (TestSuite)tests[0];
			Assertion.AssertEquals("Tests", testSuite.Name);

			tests = testSuite.Tests;
			Assertion.Assert("TestSuite:singletons - is invalid", tests[1] is TestSuite);
			TestSuite singletonSuite = (TestSuite)tests[1];
			Assertion.AssertEquals("Singletons", singletonSuite.Name);
			Assertion.AssertEquals(1, singletonSuite.Tests.Count);

			MockTestFixture mockTestFixture = new MockTestFixture();			
			Assertion.Assert("TestSuite:assemblies - is invalid", tests[0] is TestSuite);
			TestSuite mockSuite = (TestSuite)tests[0];
			Assertion.AssertEquals("Assemblies", mockSuite.Name);

			TestSuite mockFixtureSuite = (TestSuite)mockSuite.Tests[0];
			Assertion.AssertEquals(5, mockFixtureSuite.Tests.Count);
			
			ArrayList mockTests = mockFixtureSuite.Tests;
			foreach(Test t in mockTests)
			{
				Assertion.Assert("should be a TestCase", t is TestCase);
			}
		}
	}
}
