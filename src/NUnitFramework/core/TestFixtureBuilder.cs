using System;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// Built-in SuiteBuilder for NUnit TestFixture
	/// </summary>
	public class TestFixtureBuilder : ISuiteBuilder
	{
		private static readonly Type TestFixtureType 
			= typeof( NUnit.Framework.TestFixtureAttribute );

		public bool CanBuildFrom( Type type )
		{
			return !type.IsAbstract && type.IsDefined( TestFixtureType, true );	// Inheritable
		}

		public TestSuite BuildFrom( Type type, int assemblyKey )
		{
			return new TestFixture( type, assemblyKey );
		}
	}
}
