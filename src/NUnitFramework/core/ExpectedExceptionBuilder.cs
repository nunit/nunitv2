using System;
using System.Reflection;

namespace NUnit.Core
{
	internal class ExpectedExceptionBuilder : ITestBuilder
	{
		#region ITestBuilder Members

		public TestCase Make (Type fixtureType, MethodInfo method, object attribute)
		{
			return new ExpectedExceptionTestCase( fixtureType, method );
		}

		#endregion
	}
}
