using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// The ITestDecorator interface is exposed by a class that knows how to
	/// enhance the functionality of a test case or suite by decorating it.
	/// </summary>
	public interface ITestDecorator
	{
		/// <summary>
		/// Examine the a TestCase and either return it as is, modify it
		/// or return a different TestCase.
		/// </summary>
		/// <param name="test">The TestCase to be decorated</param>
		/// <param name="method">The MethodInfo used to construct the case</param>
		/// <returns>The resulting TestCase</returns>
		TestCase Decorate( TestCase testCase, MethodInfo method );

		/// <summary>
		/// Examine the a TestSuite and either return it as is, modify it
		/// or return a different TestSuite.
		/// </summary>
		/// <param name="suite">The TestSuite to be decorated</param>
		/// <param name="fixtureType">The Type used to construct the suite</param>
		/// <returns>The resulting TestSuite</returns>
		TestSuite Decorate( TestSuite suite, Type fixtureType );
	}
}
