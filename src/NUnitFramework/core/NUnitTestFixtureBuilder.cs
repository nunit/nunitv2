using System;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// Built-in SuiteBuilder for NUnit TestFixture
	/// </summary>
	public class NUnitTestFixtureBuilder : GenericTestFixtureBuilder
	{ 
		public NUnitTestFixtureBuilder() 
			: base( NUnitTestFixture.Parameters, new NUnitTestCaseBuilder() ) { }

		protected override TestSuite Construct( Type type, int assemblyKey )
		{
			return new NUnitTestFixture( type, assemblyKey );
		}
	}
}