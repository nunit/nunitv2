using System;

namespace NUnit.Core.Builders
{
	public abstract class GenericTestFixtureBuilder : ISuiteBuilder
	{
		protected string TestFixtureType;

		public bool CanBuildFrom( Type type )
		{
			return !type.IsAbstract && Reflect.HasAttribute( type, TestFixtureType, true );	// Inheritable
		}

		public TestSuite BuildFrom( Type type, int assemblyKey )
		{
			return new TestFixture( type, assemblyKey );
		}
	}

	/// <summary>
	/// Built-in SuiteBuilder for NUnit TestFixture
	/// </summary>
	public class TestFixtureBuilder : GenericTestFixtureBuilder
	{
		public TestFixtureBuilder()
		{
			TestFixtureType	= "NUnit.Framework.TestFixtureAttribute";
		}
	}

	public class CsUnitTestFixtureBuilder : GenericTestFixtureBuilder
	{
		public CsUnitTestFixtureBuilder()
		{
			TestFixtureType = "csUnit.TestFixtureAttribute";
		}
	}

	public class VstsTestFixtureBuilder : GenericTestFixtureBuilder
	{
		public VstsTestFixtureBuilder()
		{
			TestFixtureType = "Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.TestClassAttribute";
		}
	}
}
