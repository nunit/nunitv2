using System;
using System.Reflection;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// A TestSuite that wraps a csUnit test fixture
	/// </summary>
	public class CSUnitTestFixture : GenericTestFixture
	{
		static public readonly TestFixtureParameters Parameters
			= new TestFixtureParameters(
			"csUnit",
			"csUnit.TestFixtureAttribute",
			"csUnit.TestAttribute",
			"csUnit.ExpectedExceptionAttribute",
			"csUnit.SetUpAttribute",
			"csUnit.TearDownAttribute",
			"csUnit.TestFixtureSetUpAttribute",
			"csUnit.TestFixtureTearDownAttribute",
			"NUnit.Framework.ExplicitAttribute",
			"NUnit.Framework.CategoryAttribute",
			"csUnit.IgnoreAttribute",
			"NUnit.Framework.PlatformAttribute" );
	
		public CSUnitTestFixture( Type fixtureType, int assemblyKey ) 
			: base( CSUnitTestFixture.Parameters, fixtureType, assemblyKey )
		{
		}

		protected override MethodInfo GetSetUpMethod()
		{
			MethodInfo method = base.GetSetUpMethod();

			if ( method == null )
				method = Reflect.GetNamedMethod( FixtureType, "SetUp", 
					BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
			
			return method;
		}

		protected override MethodInfo GetTearDownMethod()
		{
			MethodInfo method = base.GetTearDownMethod();

			if ( method == null )
				method = Reflect.GetNamedMethod( FixtureType, "TearDown", 
					BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
			
			return method;
		}
	}
}
