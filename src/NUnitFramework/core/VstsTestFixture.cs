using System;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// A TestSuite that wraps a Visual Studio Team System test class
	/// </summary>
	public class VstsTestFixture : GenericTestFixture
	{
		static public readonly TestFixtureParameters Parameters 
			= new TestFixtureParameters(
			"vsts",
			"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.TestClassAttribute",
			"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.TestMethodAttribute",
			"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.ExpectedExceptionAttribute", //?
			"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.TestInitializeAttribute",
			"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.TestCleanupAttribute",
			"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.ClassInitializeAttribute",
			"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.ClassCleanupAttribute",
			"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.Framework.ExplicitAttribute", //?
			"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.Framework.CategoryAttribute", //?
			"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.Framework.IgnoreAttribute", //?
			"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.Framework.PlatformAttribute" ); //?

		public VstsTestFixture( Type fixtureType ) : this( fixtureType, 0 ) { }

		public VstsTestFixture( Type fixtureType, int assemblyKey ) 
			: base( Parameters, fixtureType, assemblyKey )
		{
		}
	}
}
