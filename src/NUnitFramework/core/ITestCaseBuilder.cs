using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// The ITestCaseBuilder interface is exposed by a class that knows how to
	/// build a test case from certain methods. 
	/// </summary>
	public interface ITestCaseBuilder
	{
		/// <summary>
		/// Examine the method and determine if it is suitable for
		/// this builder to use in building a TestCase
		/// </summary>
		/// <param name="method">The method to be used as a test case</param>
		/// <returns>True if the type can be used to build a TestCase</returns>
		bool CanBuildFrom( MethodInfo method );

		/// <summary>
		/// Build a TestSuite from type provided.
		/// </summary>
		/// <param name="method">The method to be used as a test case</param>
		/// <returns>A TestCase or null</returns>
		TestCase BuildFrom( MethodInfo method );
	}
}
