using System;

namespace NUnit.Core
{
	/// <summary>
	/// Class to implement an NUnit test fixture
	/// </summary>
	public class NUnitTestFixture : GenericTestFixture
	{
		static public readonly TestFixtureParameters Parameters
			= new TestFixtureParameters
			(
				"NUnit",
				"NUnit.Framework",
				"TestFixtureAttribute",
				"",
				"TestAttribute",
				"^Test",
				"ExpectedExceptionAttribute",
				"SetUpAttribute",
				"TearDownAttribute",
				"TestFixtureSetUpAttribute",
				"TestFixtureTearDownAttribute",
				"IgnoreAttribute" 
			);
		
		public NUnitTestFixture( Type fixtureType ) : this( fixtureType, 0 ) { }

		public NUnitTestFixture( Type fixtureType, int assemblyKey ) 
			: base( Parameters, fixtureType, assemblyKey )
		{
		}
	}
}
