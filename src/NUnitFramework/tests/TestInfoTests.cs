using System;
using NUnit.Framework;
using NUnit.Tests.Assemblies;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for TestInfoTests.
	/// </summary>
	[TestFixture]
	public class TestInfoTests
	{
		TestSuite testSuite;
		TestSuite testFixture;
		NUnit.Core.TestCase testCase1;

		[SetUp]
		public void SetUp()
		{
			testSuite = new TestSuite("MyTestSuite");
			testFixture = TestFixtureBuilder.Make( typeof( MockTestFixture ) );
			testSuite.Add( testFixture );

			testCase1 = (NUnit.Core.TestCase)testFixture.Tests[0];
		}

		[Test]
		public void ConstructFromFixture()
		{
			TestInfo test = new TestInfo( testFixture );
			Assert.AreEqual( "MockTestFixture", test.Name );
			Assert.AreEqual( "NUnit.Tests.Assemblies.MockTestFixture", test.FullName );
			Assert.IsTrue( test.ShouldRun, "ShouldRun" );
			Assert.IsTrue( test.IsSuite, "IsSuite" );
			Assert.IsFalse( test.IsTestCase, "!IsTestCase" );
			Assert.IsTrue( test.IsFixture, "IsFixture" );
			Assert.AreEqual( MockTestFixture.Tests, test.TestCount );
			Assert.IsNotNull(test.Categories, "Categories should not be null");
			Assert.AreEqual(1, test.Categories.Count);
			Assert.AreEqual("FixtureCategory", (string)test.Categories[0]);
			Assert.AreEqual( testFixture.ID, test.ID, "ID" );
		}

		[Test]
		public void ConstructFromSuite()
		{
			TestInfo test = new TestInfo( testSuite );
			Assert.AreEqual( "MyTestSuite", test.Name );
			Assert.AreEqual( "MyTestSuite", test.FullName );
			Assert.IsTrue( test.ShouldRun, "ShouldRun" );
			Assert.IsTrue( test.IsSuite, "IsSuite" );
			Assert.IsFalse( test.IsTestCase, "!IsTestCase" );
			Assert.IsFalse( test.IsFixture, "!IsFixture" );
			Assert.AreEqual( MockTestFixture.Tests, test.TestCount );
			Assert.AreEqual( testSuite.ID, test.ID, "ID" );
		}

		[Test]
		public void ConstructFromTestCase()
		{
			TestInfo test = new TestInfo( testCase1 );
			Assert.AreEqual( "MockTest1", test.Name );
			Assert.AreEqual( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", test.FullName );
			Assert.IsTrue( test.ShouldRun, "ShouldRun" );
			Assert.IsFalse( test.IsSuite, "!IsSuite" );
			Assert.IsTrue( test.IsTestCase, "IsTestCase" );
			Assert.IsFalse( test.IsFixture, "!IsFixture" );
			Assert.AreEqual( 1, test.TestCount );
			Assert.AreEqual( testCase1.ID, test.ID, "ID" );
		}
	}
}
