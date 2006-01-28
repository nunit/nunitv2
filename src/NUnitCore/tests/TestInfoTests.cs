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

		private void CheckConstructionFromTest( ITest expected )
		{
			TestInfo actual = new TestInfo( expected );
			Assert.AreEqual( expected.Name, actual.Name );
			Assert.AreEqual( expected.FullName, actual.FullName );
			Assert.AreEqual( expected.UniqueName, actual.UniqueName );
			Assert.AreEqual( expected.ShouldRun, actual.ShouldRun, "ShouldRun" );
			Assert.AreEqual( expected.IsSuite, actual.IsSuite, "IsSuite" );
			Assert.AreEqual( expected.IsTestCase, actual.IsTestCase, "IsTestCase" );
			Assert.AreEqual( expected.IsFixture, actual.IsFixture, "IsFixture" );
			Assert.AreEqual( expected.TestCount, actual.TestCount, "TestCount" );

			if ( expected.Categories == null )
				Assert.AreEqual( 0, actual.Categories.Count, "Categories" );
			else
			{
				Assert.AreEqual( expected.Categories.Count, actual.Categories.Count, "Categories" );
				for ( int index = 0; index < expected.Categories.Count; index++ )
					Assert.AreEqual( expected.Categories[index], actual.Categories[index], "Category {0}", index );
			}

			Assert.AreEqual( expected.TestName, actual.TestName, "TestName" );
		}

		[Test]
		public void ConstructFromFixture()
		{
			CheckConstructionFromTest( testFixture );
		}

		[Test]
		public void ConstructFromSuite()
		{
			CheckConstructionFromTest( testSuite );
		}

		[Test]
		public void ConstructFromTestCase()
		{
			CheckConstructionFromTest( testCase1 );
		}
	}
}
