using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for TestRunner.
	/// </summary>
	public interface TestRunner
	{
		#region Properties

//		bool RunOnSeparateThread { get; set; }
//
//		ApartmentState ApartmentState { get; set; }
//
//		ThreadPriority ThreadPriority { get; set; }

		#endregion

		#region Public Methods

		Test Load( string assemblyName );
		Test Load( string assemblyName, string testName );

		Test Load( string projectName, IList assemblies );
		Test Load( string projectName, IList assemblies, string testName );

		void Unload();

		int CountTestCases(IList testNames);
		
		TestResult Run(NUnit.Core.EventListener listener, TextWriter outText, TextWriter errorText, IFilter filter);
		TestResult Run(NUnit.Core.EventListener listener, TextWriter outText, TextWriter errorText);
		TestResult Run(NUnit.Core.EventListener listener, TextWriter outText, TextWriter errorText, string testName);
		TestResult Run(NUnit.Core.EventListener listener, TextWriter outText, TextWriter errorText, IList testNames);
	
		#endregion
	}
}
