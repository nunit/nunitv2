using System;
using System.Reflection;

namespace NUnit.Core
{
	internal class NormalBuilder : ITestBuilder, ITestCaseBuilder
	{
		#region ITestBuilder Members

		public TestCase Make( MethodInfo method )
		{
			return new NormalTestCase( method );
		}

		#endregion

		#region ITestCaseBuilder Members

		public bool CanBuildFrom(MethodInfo method)
		{
			return Reflect.HasAttribute( method, "NUnit.Framework.TestAttribute", false );
		}

		public TestCase BuildFrom(MethodInfo method)
		{
			if ( CanBuildFrom( method ) )
				return new NormalTestCase( method );
			return null;
		}

		#endregion
	}
}
