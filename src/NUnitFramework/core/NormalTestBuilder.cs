using System;
using System.Reflection;

namespace NUnit.Core
{
	internal class NormalBuilder : ITestBuilder
	{
		#region ITestBuilder Members

		public TestCase Make(Type fixtureType, MethodInfo method)
		{
			return new NormalTestCase(fixtureType, method);
		}

		public TestCase Make(object fixture, MethodInfo method)
		{
			return new NormalTestCase(fixture, method);
		}

		public TestCase Make (Type fixtureType, MethodInfo method, object attribute)
		{
			return Make(fixtureType, method);
		}

		#endregion
	}
}
