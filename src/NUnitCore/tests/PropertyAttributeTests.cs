// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework;
using NUnit.TestUtilities;
using NUnit.TestData.PropertyAttributeTests;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class PropertyAttributeTests
	{
		TestSuite fixture;

		[SetUp]
		public void CreateFixture()
		{
			fixture = TestBuilder.MakeFixture( typeof( FixtureWithProperties ) );
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

		[Test]
		public void CanDeriveFromPropertyAttribute()
		{
			TestCase test3 = (TestCase)fixture.Tests[2];
			Assert.AreEqual( 5, test3.Properties["Priority"] );
		}
	}
}
