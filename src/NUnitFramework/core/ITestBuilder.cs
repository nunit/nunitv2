using System;
using System.Reflection;

namespace NUnit.Core
{
	public interface ITestBuilder 
	{
		TestCase Make(Type fixtureType, MethodInfo method, object attribute);
	}
}
