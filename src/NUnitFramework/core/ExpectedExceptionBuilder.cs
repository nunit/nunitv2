using System;
using System.Reflection;

namespace NUnit.Core
{
	internal class ExpectedExceptionBuilder : ITestBuilder
	{
		#region ITestBuilder Members

		public TestCase Make(Type fixtureType, MethodInfo method)
		{
			return new ExpectedExceptionTestCase( fixtureType, method );
		}

		public TestCase Make(object fixture, MethodInfo method)
		{
			return new ExpectedExceptionTestCase( fixture, method );
		}

		public TestCase Make (Type fixtureType, MethodInfo method, object attribute)
		{
			return Make(fixtureType, method);
		}

		#endregion
	}
}
