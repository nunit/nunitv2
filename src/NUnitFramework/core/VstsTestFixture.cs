using System;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// A TestSuite that wraps a Visual Studio Team System test class
	/// </summary>
	public class VstsTestFixture : GenericTestFixture
	{
		static public readonly TestFixtureParameters Parameters 
			= new TestFixtureParameters
			(
				"vsts",
				"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework",
				"TestClassAttribute",
				"",
				"TestMethodAttribute",
				"",
				"ExpectedExceptionAttribute", //?
				"TestInitializeAttribute",
				"TestCleanupAttribute",
				"ClassInitializeAttribute",
				"ClassCleanupAttribute",
				"IgnoreAttribute" //?
			);

		public VstsTestFixture( Type fixtureType ) : this( fixtureType, 0 ) { }

		public VstsTestFixture( Type fixtureType, int assemblyKey ) 
			: base( Parameters, fixtureType, assemblyKey )
		{
		}
	}
}
