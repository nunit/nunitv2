using System;
using System.Reflection;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// Builder for csunit test fixtures
	/// </summary>
	public class CSUnitTestFixtureBuilder : GenericTestFixtureBuilder
	{
		#region Constructor
		public CSUnitTestFixtureBuilder() : base( CSUnitTestFixture.Parameters ) { }
		#endregion

		#region GenericTestFixtureBuilder Overrides
		/// <summary>
		/// Returns a CSUnitTestFixture
		/// </summary>
		/// <param name="type">The type to use in making the fixture</param>
		/// <param name="assemblyKey">The index of the assembly</param>
		/// <returns>A TestSuite or null</returns>
		protected override TestSuite MakeSuite( Type type, int assemblyKey )
		{
			return new CSUnitTestFixture( type, assemblyKey );
		}
		
		/// <summary>
		/// Adds test cases to the fixture. Overrides the base class 
		/// to install a CSUnitTestCaseBuilder while the tests are
		/// being added.
		/// </summary>
		/// <param name="fixtureType">The type of the fixture</param>
		protected override void AddTestCases(Type fixtureType)
		{
			using( new AddinState() )
			{
				Addins.Register( new CSUnitTestCaseBuilder() );
				base.AddTestCases (fixtureType);
			}
		}
		#endregion
	}
}
