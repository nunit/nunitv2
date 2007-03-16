// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for LegacySuiteTests.
	/// </summary>
//	[TestFixture]
	public class LegacySuiteTests
	{
//		[Test]
		public void SetUpAndTearDownAreCalled()
		{
			TestSuite suite = new LegacySuite( typeof( LegacySuiteWithSetUpAndTearDown ) );
			suite.Run( NullListener.NULL );
			LegacySuiteWithSetUpAndTearDown fixture = (LegacySuiteWithSetUpAndTearDown)suite.Fixture;
			Assert.AreEqual( 1, fixture.setupCount );
			Assert.AreEqual( 1, fixture.teardownCount );
		}

		private class LegacySuiteWithSetUpAndTearDown
		{
			public int setupCount = 0;
			public int teardownCount = 0;

			[Suite]
			public static TestSuite TheSuite
			{
				get { return new TestSuite( "EmptySuite" ); }
			}

			[TestFixtureSetUp]
			public void SetUpMethod()
			{
				setupCount++;
			}

			[TestFixtureTearDown]
			public void TearDownMethod()
			{
				teardownCount++;
			}
		}
	}
}
