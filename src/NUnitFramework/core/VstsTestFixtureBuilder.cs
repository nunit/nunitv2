using System;

namespace NUnit.Core.Builders
{
	/// <summary>
	///  Builder for Visual Studio Team System test fixtures
	/// </summary>
	public class VstsTestFixtureBuilder : GenericTestFixtureBuilder
	{
		public VstsTestFixtureBuilder() : base( VstsTestFixture.Parameters ) { }

		protected override TestSuite MakeSuite( Type type, int assemblyKey )
		{
			return new VstsTestFixture( type, assemblyKey );
		}

		protected override void AddTestCases(Type fixtureType)
		{
			using( new AddinState() )
			{
				//AddinManager.Addins.Add( new VstsTestCaseBuilder() );
				base.AddTestCases (fixtureType);
			}
		}

	}

}
