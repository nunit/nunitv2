using System;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class PropertyAttributeTests
	{
		TestSuite fixture;

		[SetUp]
		public void CreateFixture()
		{
			fixture = TestFixtureBuilder.Make( typeof( FixtureWithProperties ) );
		}

		[Test]
		public void PropertyWithStringValue()
		{
			TestCase test1 = (TestCase)fixture.Tests[0];
			Assert.AreEqual( "Charlie", test1.Properties["user"] );
		}

		[Test]
		public void PropertiesWithNumericValues()
		{
			TestCase test2 = (TestCase)fixture.Tests[1];
			Assert.AreEqual( 10.0, test2.Properties["X"] );
			Assert.AreEqual( 17.0, test2.Properties["Y"] );
		}

		[Test]
		public void PropertyWorksOnFixtures()
		{
			Assert.AreEqual( "SomeClass", fixture.Properties["ClassUnderTest"] );
		}

		[TestFixture, Property("ClassUnderTest","SomeClass" )]
		class FixtureWithProperties
		{
			[Test, Property("user","Charlie")]
			public void Test1() { }

			[Test, Property("X",10.0), Property("Y",17.0)]
			public void Test2() { }
		}
	}
}
