using System;

namespace NUnit.Core.Builders
{
	/// <summary>
	///  Builder for Visual Studio Team System test fixtures
	/// </summary>
	public class VstsTestFixtureBuilder : GenericTestFixtureBuilder
	{
		public VstsTestFixtureBuilder() : base( 
			VstsTestFixture.Parameters, null ) { } // TODO: No test case builder yet

		protected override TestSuite Construct( Type type, int assemblyKey )
		{
			return new VstsTestFixture( type, assemblyKey );
		}
	}

}
