using System;
using System.Linq.Expressions;
using System.Reflection;

namespace nunit.core.tests.net45
{
	public static class Reflection
	{
		public static MethodInfo GetMethod<T>(Expression<Action<T>> invocation)
		{
			return ((MethodCallExpression) invocation.Body).Method;
		}
	}
}