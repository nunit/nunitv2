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
using NUnit.TestUtilities;

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

			suite = TestFinder.RequiredChildTest("NUnit", suite);
			suite = TestFinder.RequiredChildTest("Tests", suite);
			Assert.AreEqual(MockAssembly.Fixtures, suite.Tests.Count);

			Test singletonSuite = TestFinder.RequiredChildTest("Singletons", suite);
			Assert.AreEqual(1, singletonSuite.Tests.Count);

			Test mockSuite = TestFinder.RequiredChildTest("Assemblies", suite);
			Assert.AreEqual(1, mockSuite.Tests.Count);

			Test mockFixtureSuite = TestFinder.RequiredChildTest("MockTestFixture", mockSuite);
			Assert.AreEqual(MockTestFixture.Tests, mockFixtureSuite.Tests.Count);

			foreach(Test t in mockFixtureSuite.Tests)
			{
				Assert.IsFalse(t.IsSuite, "Should not be a suite");
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
