using System;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// SetUpFixtureBuilder knows how to build a SetUpFixture.
	/// </summary>
	public class SetUpFixtureBuilder : ISuiteBuilder
	{	
		public SetUpFixtureBuilder()
		{
			//
			// TODO: Add constructor logic here	//
		}

		#region ISuiteBuilder Members

		public TestSuite BuildFrom(Type type, int assemblyKey)
		{
			return new SetUpFixture( type, assemblyKey );
		}

		public bool CanBuildFrom(Type type)
		{
			return Reflect.HasAttribute( type, "NUnit.Framework.SetUpFixtureAttribute", false );
		}
		#endregion
	}
}
