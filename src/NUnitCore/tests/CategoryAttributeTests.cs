// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using NUnit.Framework;
using NUnit.TestUtilities;
using NUnit.TestData.CategoryAttributeTests;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for CategoryAttributeTests.
	/// </summary>
	[TestFixture]
	public class CategoryAttributeTests
	{
		TestSuite fixture;

		[SetUp]
		public void CreateFixture()
		{
			fixture = TestBuilder.MakeFixture( typeof( FixtureWithCategories ) );
		}

		[Test]
		public void CategoryOnFixture()
		{
			Assert.Contains( "DataBase", fixture.Categories );
		}

		[Test]
		public void CategoryOnTestCase()
		{
			TestCase test1 = (TestCase)fixture.Tests[0];
			Assert.Contains( "Long", test1.Categories );
		}

		[Test]
		public void CanDeriveFromCategoryAttribute()
		{
			TestCase test2 = (TestCase)fixture.Tests[1];
			Assert.Contains( "Critical", test2.Categories );
		}
	}
}
