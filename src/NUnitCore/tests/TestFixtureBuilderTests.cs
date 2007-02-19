// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;
using NUnit.TestUtilities;
using NUnit.TestData.TestFixtureBuilderTests;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class TestFixtureBuilderTests
	{
		TestSuiteBuilder builder = new TestSuiteBuilder();

		private Test loadFixture( Type type )
		{
			TestPackage package = new TestPackage( type.Module.Name );
			package.TestName = type.FullName;
			Test suite= builder.Build( package );
			Assert.IsNotNull(suite);

			return suite;
		}

		[Test]
		public void GoodSignature()
		{
			string methodName = "TestVoid";
			Test fixture = loadFixture( typeof( SignatureTestFixture ) );
			Test foundTest = TestFinder.Find( methodName, fixture );
			Assert.IsNotNull( foundTest );
			Assert.AreEqual( RunState.Runnable, foundTest.RunState );
		}

		[Test]
		public void LoadCategories() 
		{
			Test fixture = loadFixture( typeof( HasCategories ) );
			Assert.IsNotNull(fixture);
			Assert.AreEqual(2, fixture.Categories.Count);
		}
	}
}
