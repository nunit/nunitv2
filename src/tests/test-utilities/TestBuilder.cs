using System;
using NUnit.Core;
using NUnit.Core.Builders;

namespace NUnit.TestUtilities
{
	/// <summary>
	/// Utility Class used to build NUnit tests for use as test data
	/// </summary>
	public class TestBuilder
	{
		private static NUnitTestFixtureBuilder suiteBuilder = new NUnitTestFixtureBuilder();

		public static TestSuite MakeFixture( Type type )
		{
			return suiteBuilder.BuildFrom( type );
		}

		public static TestSuite MakeFixture( object fixture )
		{
			TestSuite suite = suiteBuilder.BuildFrom( fixture.GetType() );
			suite.Fixture = fixture;
			return suite;
		}

		public static TestCase MakeTestCase( Type type, string methodName )
		{
			return TestCaseBuilder.Make( type, methodName );
		}

		public static TestResult RunTestFixture( Type type )
		{
			return MakeFixture( type ).Run( NullListener.NULL );
		}

		public static TestResult RunTestFixture( object fixture )
		{
			return MakeFixture( fixture ).Run( NullListener.NULL );
		}

		public static TestResult RunTestCase( Type type, string methodName )
		{
			return MakeTestCase( type, methodName ).Run( NullListener.NULL );
		}

		private TestBuilder() { }
	}
}
