using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// Built-in SuiteBuilder for NUnit TestFixture
	/// </summary>
	public class NUnitTestFixtureBuilder : GenericTestFixtureBuilder
	{ 
		#region Constructor
		public NUnitTestFixtureBuilder() : base( NUnitTestFixture.Parameters ) { }
		#endregion

		#region GenericTestFixtureBuilder Overrides
		/// <summary>
		/// Makes an NUnitTestFixture instance
		/// </summary>
		/// <param name="type">The type to be used</param>
		/// <param name="assemblyKey">The assembly key</param>
		/// <returns>A TestSuite or null</returns>
		protected override TestSuite MakeSuite( Type type, int assemblyKey )
		{
			return new NUnitTestFixture( type, assemblyKey );
		}

		/// <summary>
		/// Overrides GenericTestFixtureBuilder.BuildFrom to allow
		/// use of the Category, Explicit and Platform attributes
		/// on NUnitTestFixtures:
		/// </summary>
		/// <param name="type">The type to use in building the fixture</param>
		/// <param name="assemblyKey">The assembly key</param>
		/// <returns>A TestSuite or null</returns>
		public override TestSuite BuildFrom(Type type, int assemblyKey)
		{
			TestSuite suite = base.BuildFrom (type, assemblyKey);

			if ( suite != null )
			{
				PlatformHelper helper = new PlatformHelper();
				if ( !helper.IsPlatformSupported( type ) )
				{
					suite.ShouldRun = false;
					suite.IgnoreReason = "Not running on correct platform";
				}

				suite.Categories = CategoryManager.GetCategories( type );	
				suite.IsExplicit = Reflect.HasAttribute( type, "NUnit.Framework.ExplicitAttribute", false );
			}

			return suite;
		}

		/// <summary>
		/// Adds test cases to the fixture. Overrides the base class 
		/// to install an NUnitTestCaseBuilder while the tests are
		/// being added.
		/// </summary>
		/// <param name="fixtureType">The type of the fixture</param>
		protected override void AddTestCases(Type fixtureType)
		{
			using( new AddinState() )
			{
				Addins.Register( new NUnitTestCaseBuilder() );
				base.AddTestCases (fixtureType);
			}
		}
		#endregion
	}
}