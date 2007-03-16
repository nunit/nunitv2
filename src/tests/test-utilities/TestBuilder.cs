// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Reflection;
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
		private static NUnitTestCaseBuilder testBuilder = new NUnitTestCaseBuilder();

		public static NUnitTestFixture MakeFixture( Type type )
		{
			return (NUnitTestFixture)suiteBuilder.BuildFrom( type );
		}

		public static NUnitTestFixture MakeFixture( object fixture )
		{
			NUnitTestFixture suite = (NUnitTestFixture)suiteBuilder.BuildFrom( fixture.GetType() );
			suite.Fixture = fixture;
			return suite;
		}

		public static NUnitTestMethod MakeTestCase( Type type, string methodName )
		{
			return (NUnitTestMethod)testBuilder.BuildFrom( Reflect.GetNamedMethod( 
				type,
				methodName,
				BindingFlags.Public | BindingFlags.Instance ) );
		}

		public static NUnitTestMethod MakeTestCase( object fixture, string methodName )
		{
			NUnitTestMethod test = MakeTestCase( fixture.GetType(), methodName );
			test.Fixture = fixture;
			return test;
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
