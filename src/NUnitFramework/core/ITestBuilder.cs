using System;
using System.Reflection;

namespace NUnit.Core
{
	public interface ITestBuilder 
	{
		TestCase Make( MethodInfo method );
	}
}
