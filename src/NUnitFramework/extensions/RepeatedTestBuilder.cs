using System;
using System.Reflection;
using NUnit.Core;

namespace NUnit.Extensions
{
	public class RepeatedTestBuilder : ITestBuilder
	{
		public TestCase Make (Type fixtureType, MethodInfo method)
		{
			return new RepeatedTestCase(fixtureType, method, 1);
		}

		public TestCase Make (object fixture, MethodInfo method)
		{
			return new RepeatedTestCase(fixture, method, 1);
		}

		public TestCase Make (Type fixtureType, MethodInfo method, object attribute)
		{
			RepeatedTestAttribute repeatedAttribute = (RepeatedTestAttribute) attribute;
			return new RepeatedTestCase(fixtureType, method, repeatedAttribute.Count);
		}
	}
}
