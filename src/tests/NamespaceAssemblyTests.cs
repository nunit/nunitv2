#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

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
		private string nonamespaceDLL = "nonamespace-assembly.dll";
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
		public void Hierarchy()
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
			Assertion.AssertEquals(1, tests.Count);
			Assertion.AssertEquals("Tests", testSuite.Name);

			tests = testSuite.Tests;
			Assertion.AssertEquals(3, tests.Count);

			Assertion.Assert("TestSuite:singletons - is invalid", tests[1] is TestSuite);
			TestSuite singletonSuite = (TestSuite)tests[2];
			Assertion.AssertEquals("Singletons", singletonSuite.Name);
			Assertion.AssertEquals(1, singletonSuite.Tests.Count);

			MockTestFixture mockTestFixture = new MockTestFixture();			
			Assertion.Assert("TestSuite:assemblies - is invalid", tests[1] is TestSuite);
			TestSuite mockSuite = (TestSuite)tests[1];
			Assertion.AssertEquals("Assemblies", mockSuite.Name);

			TestSuite mockFixtureSuite = (TestSuite)mockSuite.Tests[0];
			Assertion.AssertEquals(5, mockFixtureSuite.Tests.Count);
			
			ArrayList mockTests = mockFixtureSuite.Tests;
			foreach(Test t in mockTests)
			{
				Assertion.Assert("should be a TestCase", t is NUnit.Core.TestCase);
			}
		}
			
		[Test]
		public void NoNamespaceInAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build( nonamespaceDLL );
			Assertion.Assert(suite != null);
			Assertion.AssertEquals( 3, suite.CountTestCases );
		}
	}
}
