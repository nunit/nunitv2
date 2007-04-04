// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.Assemblies;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class NamespaceAssemblyTests
	{
		private string testsDll = "mock-assembly.dll";
		private string nonamespaceDLL = "nonamespace-assembly.dll";
		
		[Test]
		public void LoadTestFixtureFromAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestPackage package = new TestPackage( testsDll );
			package.TestName = "NUnit.Tests.Assemblies.MockTestFixture";
			Test suite= builder.Build( package );
			Assert.IsNotNull(suite);
		}

		[Test]
		public void TestRoot()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			Test suite = builder.Build( new TestPackage( testsDll ) );
			Assert.AreEqual(testsDll, suite.TestName.Name);
		}

		[Test]
		public void Hierarchy()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			Test suite = builder.Build( new TestPackage( testsDll ) );
			IList tests = suite.Tests;
			Assert.AreEqual(1, tests.Count);

			Assert.IsTrue(tests[0] is TestSuite, "TestSuite:NUnit - is not correct");
			TestSuite testSuite = (TestSuite)tests[0];
			Assert.AreEqual("NUnit", testSuite.TestName.Name);

			tests = testSuite.Tests;
			Assert.IsTrue(tests[0] is TestSuite, "TestSuite:Tests - is invalid");
			testSuite = (TestSuite)tests[0];
			Assert.AreEqual(1, tests.Count);
			Assert.AreEqual("Tests", testSuite.TestName.Name);

			tests = testSuite.Tests;
			// TODO: Get rid of constants in this test
			Assert.AreEqual(MockAssembly.Fixtures, tests.Count);

			Assert.IsTrue(tests[3] is TestSuite, "TestSuite:singletons - is invalid");
			TestSuite singletonSuite = (TestSuite)tests[3];
			Assert.AreEqual("Singletons", singletonSuite.TestName.Name);
			Assert.AreEqual(1, singletonSuite.Tests.Count);

			Assert.IsTrue(tests[0] is TestSuite, "TestSuite:assemblies - is invalid");
			TestSuite mockSuite = (TestSuite)tests[0];
			Assert.AreEqual("Assemblies", mockSuite.TestName.Name);

			TestSuite mockFixtureSuite = (TestSuite)mockSuite.Tests[0];
			Assert.AreEqual(MockTestFixture.Tests, mockFixtureSuite.Tests.Count);
			
			IList mockTests = mockFixtureSuite.Tests;
			foreach(Test t in mockTests)
			{
				Assert.IsTrue(t is NUnit.Core.TestCase, "should be a TestCase");
			}
		}
			
		[Test]
		public void NoNamespaceInAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			Test suite = builder.Build( new TestPackage( nonamespaceDLL ) );
			Assert.IsNotNull(suite);
			Assert.AreEqual( NoNamespaceTestFixture.Tests, suite.TestCount );

			suite = (TestSuite)suite.Tests[0];
			Assert.IsNotNull(suite);
			Assert.AreEqual( "NoNamespaceTestFixture", suite.TestName.Name );
			Assert.AreEqual( "NoNamespaceTestFixture", suite.TestName.FullName );
		}
	}
}
