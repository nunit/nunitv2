using System;
using System.Reflection;

namespace NUnit.Core
{
	internal class ExpectedExceptionBuilder : ITestBuilder, ITestCaseBuilder
	{
		#region ITestBuilder Members

		public TestCase Make ( MethodInfo method )
		{
			return new ExpectedExceptionTestCase( method );
		}

		#endregion

		#region ITestCaseBuilder Members

		public bool CanBuildFrom(MethodInfo method)
		{
			return Reflect.HasAttribute( method, "NUnit.Framework.ExpectedExceptionAttribute", false );
		}

		public TestCase BuildFrom(MethodInfo method)
		{
			return new ExpectedExceptionTestCase( method );
		}

		#endregion
	}
}
