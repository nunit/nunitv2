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
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.Assemblies;

namespace NUnit.Tests.Core
{
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
			TestSuite suite = builder.Build( testsDll, "NUnit.Tests.Assemblies.MockTestFixture" );
			Assert.IsNotNull(suite);
		}

		[Test]
		public void TestRoot()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build(testsDll);
			Assert.AreEqual(testsDll, suite.Name);
		}

		[Test]
		public void Hierarchy()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build(testsDll);
			ArrayList tests = suite.Tests;
			Assert.AreEqual(1, tests.Count);

			Assert.IsTrue(tests[0] is TestSuite, "TestSuite:NUnit - is not correct");
			TestSuite testSuite = (TestSuite)tests[0];
			Assert.AreEqual("NUnit", testSuite.Name);

			tests = testSuite.Tests;
			Assert.IsTrue(tests[0] is TestSuite, "TestSuite:Tests - is invalid");
			testSuite = (TestSuite)tests[0];
			Assert.AreEqual(1, tests.Count);
			Assert.AreEqual("Tests", testSuite.Name);

			tests = testSuite.Tests;
			Assert.AreEqual(3, tests.Count);
			tests.Sort();

			Assert.IsTrue(tests[1] is TestSuite, "TestSuite:singletons - is invalid");
			TestSuite singletonSuite = (TestSuite)tests[1];
			Assert.AreEqual("Singletons", singletonSuite.Name);
			Assert.AreEqual(1, singletonSuite.Tests.Count);

			MockTestFixture mockTestFixture = new MockTestFixture();			
			Assert.IsTrue(tests[0] is TestSuite, "TestSuite:assemblies - is invalid");
			TestSuite mockSuite = (TestSuite)tests[0];
			Assert.AreEqual("Assemblies", mockSuite.Name);

			TestSuite mockFixtureSuite = (TestSuite)mockSuite.Tests[0];
			Assert.AreEqual(MockTestFixture.Tests, mockFixtureSuite.Tests.Count);
			
			ArrayList mockTests = mockFixtureSuite.Tests;
			foreach(Test t in mockTests)
			{
				Assert.IsTrue(t is NUnit.Core.TestCase, "should be a TestCase");
			}
		}
			
		[Test]
		public void NoNamespaceInAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build( nonamespaceDLL );
			Assert.IsNotNull(suite);
			Assert.AreEqual( NoNamespaceTestFixture.Tests, suite.CountTestCases() );

			suite = (TestSuite)suite.Tests[0];
			Assert.IsNotNull(suite);
			Assert.AreEqual( "NoNamespaceTestFixture", suite.Name );
			Assert.AreEqual( "NoNamespaceTestFixture", suite.FullName );
		}
	}
}
