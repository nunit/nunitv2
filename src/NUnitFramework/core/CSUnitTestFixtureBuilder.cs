using System;
using System.Reflection;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// Builder for csunit test fixtures
	/// </summary>
	public class CSUnitTestFixtureBuilder : GenericTestFixtureBuilder
	{
		public CSUnitTestFixtureBuilder() 
			: base( CSUnitTestFixture.Parameters, new CSUnitTestCaseBuilder() ) { }

		public override bool CanBuildFrom( Type type )
		{
			return 	base.CanBuildFrom( type ) || type.Name.EndsWith( "Test" );
		}

		protected override TestSuite Construct( Type type, int assemblyKey )
		{
			return new CSUnitTestFixture( type, assemblyKey );
		}
	}
}
