using System;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// The ISuiteBuilder interface is exposed by a class that knows how to
	/// build a suite from one or more Types. 
	/// </summary>
	public interface ISuiteBuilder
	{
		/// <summary>
		/// Examine the type and determine if it is suitable for
		/// this builder to use in building a TestSuite
		/// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>True if the type can be used to build a TestSuite</returns>
		bool CanBuildFrom( Type type );

		/// <summary>
		/// Build a TestSuite from type provided.
		/// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>A TestSuite</returns>
		TestSuite BuildFrom( Type type, int assemblyKey );
	}
}
