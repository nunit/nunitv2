using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace NUnit.Core
{
	/// <summary>
	/// Interface for loading and running tests
	/// </summary>
	public interface TestRunner
	{
		/// <summary>
		/// Load an assembly
		/// </summary>
		/// <param name="assemblyName"></param>
		Test Load( string assemblyName );

		/// <summary>
		/// Load a particular test in an assembly
		/// </summary>
		Test Load( string assemblyName, string testName );

		/// <summary>
		/// Load multiple assemblies
		/// </summary>
		Test Load( string projectName, IList assemblies );

		/// <summary>
		/// Load a particular test in a set of assemblies
		/// </summary>
		Test Load( string projectName, IList assemblies, string testName );

		/// <summary>
		/// Unload all tests previously loaded
		/// </summary>
		void Unload();

		/// <summary>
		/// Count test cases starting at a set of roots
		/// </summary>
		int CountTestCases(IList testNames);

		/// <summary>
		/// Get the collectiion of categories used by the runner;
		/// </summary>
		/// <returns></returns>
		ICollection GetCategories(); 

		/// <summary>
		/// Run the loaded tests using a test filter
		/// </summary>
		TestResult Run(NUnit.Core.EventListener listener, IFilter filter);

		/// <summary>
		/// Run all loaded tests
		/// </summary>
		TestResult Run(NUnit.Core.EventListener listener);
		
		/// <summary>
		/// Run a particular loaded test
		/// </summary>
		TestResult Run(NUnit.Core.EventListener listener, string testName);
		
		/// <summary>
		/// Run a set of loaded tests
		/// </summary>
		TestResult Run(NUnit.Core.EventListener listener, IList testNames);	

		/// <summary>
		/// Load, Run and Unload a test assembly
		/// </summary>
		TestResult RunTest( EventListener listener, string assemblyName );
		
		/// <summary>
		/// Load, Run and Unload a particular test
		/// </summary>
		TestResult RunTest( EventListener listener, string assemblyName, string testName );
		
		/// <summary>
		/// Run a set of tests in an assembly
		/// </summary>
		TestResult RunTest( EventListener listener, IList assemblies );
	}
}
