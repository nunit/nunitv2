// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// TestFixtureBuilder contains static methods for building
	/// TestFixtures from types. It uses builtin SuiteBuilders
	/// and any installed extensions to do it.
	/// </summary>
	public class TestFixtureBuilder
	{
		public static bool CanBuildFrom( Type type )
		{
			return CoreExtensions.Host.SuiteBuilders.CanBuildFrom( type );
		}

		/// <summary>
		/// Build a test fixture from a given type.
		/// </summary>
		/// <param name="type">The type to be used for the fixture</param>
		/// <returns>A TestSuite if the fixture can be built, null if not</returns>
		public static Test BuildFrom( Type type )
		{
			Test suite = CoreExtensions.Host.SuiteBuilders.BuildFrom( type );

			if ( suite != null )
				suite = CoreExtensions.Host.TestDecorators.Decorate( suite, type );

			return suite;
		}

		/// <summary>
		/// Build a fixture from an object. 
		/// </summary>
		/// <param name="fixture">The object to be used for the fixture</param>
		/// <returns>A TestSuite if fixture type can be built, null if not</returns>
		public static Test BuildFrom( object fixture )
		{
			Test suite = BuildFrom( fixture.GetType() );
			if( suite != null)
				suite.Fixture = fixture;
			return suite;
		}

		public static string GetAssemblyPath( Type fixtureType )
		{
			return GetAssemblyPath( fixtureType.Assembly );
		}

		public static string GetAssemblyPath( Assembly assembly )
		{
			Uri uri = new Uri( assembly.CodeBase );
			string path = uri.LocalPath;
			if ( uri.Fragment != null && uri.Fragment.StartsWith( "#" ) )
				path += uri.Fragment;
			return path;
		}

		/// <summary>
		/// Private constructor to prevent instantiation
		/// </summary>
		private TestFixtureBuilder() { }
	}
}
