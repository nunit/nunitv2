using System;
using System.Reflection;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// A TestSuite that wraps a csUnit test fixture
	/// </summary>
	public class CSUnitTestFixture : GenericTestFixture
	{
		#region TestFixtureParamters for GenericTestFixture
		static public readonly TestFixtureParameters Parameters
			= new TestFixtureParameters
			(
				"csUnit",
				"csUnit",
				"TestFixtureAttribute",
				"Test$",
				"TestAttribute",
				"^(?i)test",
				"ExpectedExceptionAttribute",
				"SetUpAttribute",
				"TearDownAttribute",
				"FixtureSetUpAttribute",
				"FixtureTearDownAttribute",
				"IgnoreAttribute" 
			);
		#endregion

		#region Constructor
		public CSUnitTestFixture( Type fixtureType, int assemblyKey ) 
			: base( CSUnitTestFixture.Parameters, fixtureType, assemblyKey )
		{
		}
		#endregion

		#region Overrides
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
		#endregion
	}
}
