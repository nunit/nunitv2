using System;

namespace NUnit.Core
{
	/// <summary>
	/// Class to implement an NUnit test fixture
	/// </summary>
	public class NUnitTestFixture : GenericTestFixture
	{
		static public readonly TestFixtureParameters Parameters
			= new TestFixtureParameters(
			"NUnit",
			"NUnit.Framework.TestFixtureAttribute",
			"NUnit.Framework.TestAttribute",
			"NUnit.Framework.ExpectedExceptionAttribute",
			"NUnit.Framework.SetUpAttribute",
			"NUnit.Framework.TearDownAttribute",
			"NUnit.Framework.TestFixtureSetUpAttribute",
			"NUnit.Framework.TestFixtureTearDownAttribute",
			"NUnit.Framework.ExplicitAttribute",
			"NUnit.Framework.CategoryAttribute",
			"NUnit.Framework.IgnoreAttribute",
			"NUnit.Framework.PlatformAttribute" );
		
		public NUnitTestFixture( Type fixtureType ) : this( fixtureType, 0 ) { }

		public NUnitTestFixture( Type fixtureType, int assemblyKey ) 
			: base( Parameters, fixtureType, assemblyKey )
		{
		}
	}
}
