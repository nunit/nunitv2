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
	using System.Collections;
	using System.Reflection;
	using NUnit.Framework;
	using NUnit.Core;
	using NUnit.Tests.Assemblies;

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
			TestSuiteBuilder builder = new TestSuiteBuilder();
			testAssembly = builder.Load(testsDll);
			assemblyTestType = testAssembly.GetType("NUnit.Tests.OneTestCase");
		}

		[Test]
		public void LoadTestFixtureFromAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build("NUnit.Tests.Assemblies.MockTestFixture", testsDll);
			Assertion.Assert(suite != null);
		}

		[Test]
		public void TestRoot()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build(testsDll);
			Assertion.AssertEquals(testsDll, suite.Name);
		}

		[Test]
		[Ignore("this test is too brittle")]
		public void TestHiearchy()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build(testsDll);
			ArrayList tests = suite.Tests;
			Assertion.AssertEquals(1, tests.Count);

			Assertion.Assert("TestSuite:NUnit - is not correct", tests[0] is TestSuite);
			TestSuite testSuite = (TestSuite)tests[0];
			Assertion.AssertEquals("NUnit", testSuite.Name);

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
				Assertion.Assert("should be a TestCase", t is NUnit.Core.TestCase);
			}
		}
	}
}
