#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Collections;
using System.IO;

namespace NUnit.Core
{
	/// <summary>
	/// The TestRunner Interface allows client code, such as the NUnit console and
	/// gui runners, to load and run tests. This is the lowest level interface generally
	/// supported for running tests and is implemented by the RemoteTestRunner class in
	/// the NUnit core as well as by other classes running on the client side.
	/// 
	/// The Load family of methods is used to load a suite of tests from one or more 
	/// assemblies, returning the resulting test suite to the caller.
	/// 
	/// The SetFilter method provides a general approach to running only a subset
	/// of the tests. Any set filter affects future calls to CountTestCases or Run.
	/// 
	/// The CountTestCases family of methods returns the number of test cases in the
	/// loaded suite, either in its entirety or by taking a subset of tests as roots.
	/// 
	/// The Run family of methods performs a test run synchronously, returning a TestResult
	/// or TestResult[] to the caller. If provided, an EventListener interface will be 
	/// notified of significant events in the running of the tests. Methods to cancel
	/// a run and to wait for a run to complete are also provided. Test results may also 
	/// be obtained by querying the Results property after a run is complete.
	/// 
	/// BeginRun and EndRun provide a simplified form of the asynchronous invocation
	/// pattern used in many places within the .NET framework. Because the current
	/// implementation allows only one run to be in process at a time, an IAsyncResult
	/// is not used at this time.
	/// </summary>
	public interface TestRunner
	{
		#region Properties
//		/// <summary>
//		/// Writer for standard output.
//		/// </summary>
//		TextWriter Out
//		{
//			get; set;
//		}
//
//		/// <summary>
//		/// Writer for error output.
//		/// </summary>
//		TextWriter Error
//		{
//			get; set;
//		}

		/// <summary>
		/// IsTestRunning indicates whether a test is in progress. To retrieve the
		/// results from an asynchronous test run, wait till IsTestRunning is false.
		/// </summary>
		bool Running
		{
			get;
		}

		/// <summary>
		/// Returns the collection of test frameworks referenced by the running
		/// tests. Currently in the interface for diagnostic purposes only.
		/// Should be removed at some point.
		/// </summary>
		IList TestFrameworks
		{
			get;
		}

		/// <summary>
		/// Results from the last test run.
		/// </summary>
		TestResult[] Results
		{
			get;
		}

		/// <summary>
		/// Get or set the current run filter
		/// </summary>
		IFilter Filter
		{
			get; set;
		}
		#endregion

		#region Load and Unload Methods
		/// <summary>
		/// Load all tests from an assembly
		/// </summary>
		/// <param name="assemblyName">The assembly from which tests are to be loaded</param>
		Test Load( string assemblyName );

		/// <summary>
		/// Load a particular test in an assembly
		/// </summary>
		/// <param name="assemblyName">The assembly from which tests are to be loaded</param>
		/// <param name="testName">The name of the test fixture or suite to be loaded</param>
		Test Load( string assemblyName, string testName );

		/// <summary>
		/// Load the assemblies in a test project
		/// </summary>
		/// <param name="testProject">The test project to load</param>
		/// <returns>The loaded test</returns>
		Test Load( TestProject testProject );

		/// <summary>
		/// Load a particular test in a TestProject.
		/// TODO: Decide how to encapsulate a group of assemblies
		/// plus parameters for loading them. See TestProject.cs.
		/// </summary>
		/// <param name="testProject">The test project to load</param>
		/// <param name="testName">The name of the test fixture or suite to be loaded</param>
		/// <returns>The loaded test</returns>
		Test Load( TestProject testProject, string testName );

		/// <summary>
		/// Unload all tests previously loaded
		/// </summary>
		void Unload();
		#endregion

		#region CountTestCases Methods
		/// <summary>
		/// Count Test Cases under a given test name
		/// </summary>
		/// <param name="testName">The name of a test case, fixture or suite</param>
		/// <returns>The number of test cases found</returns>
		int CountTestCases(string testName );

		/// <summary>
		/// Count test cases starting at a set of roots
		/// </summary>
		/// <param name="testNames">An array of names of test cases, fixtures or suites</param>
		/// <returns>The number of test cases found</returns>
		int CountTestCases(string[] testNames);
		#endregion

		#region GetCategories Method
		/// <summary>
		/// Get the collectiion of categories used by the runner.
		/// TODO: Should this really be here? Can't the client
		/// figure it out based on the loaded tests?
		/// </summary>
		/// <returns></returns>
		ICollection GetCategories(); 
		#endregion

		#region Run Methods
		/// <summary>
		/// Run all loaded tests and return a test result. The test is run synchronously,
		/// and the listener interface is notified as it progresses.
		/// </summary>
		/// <param name="listener">Interface to receive EventListener notifications.</param>
		TestResult Run(NUnit.Core.EventListener listener);
		
		/// <summary>
		/// Run a set of loaded tests and return a set of results.  The test is run
		/// synchronously and the listener interface is notified as it progresses.
		/// </summary>
		/// <param name="listener">Interface to receive EventListener notifications</param>
		/// <param name="testNames">The names of the test cases, fixtures or suites to be run</param>
		TestResult[] Run(NUnit.Core.EventListener listener, string[] testNames);

		/// <summary>
		/// Start a run of all loaded tests. The tests are run aynchronously and the 
		/// listener interface is notified as it progresses.
		/// </summary>
		/// <param name="listener">Interface to receive EventListener notifications.</param>
		void BeginRun(NUnit.Core.EventListener listener);
		
		/// <summary>
		/// Start running a set of loaded tests. The tests are run asynchronously and 
		/// the listener interface is notified as it progresses.
		/// </summary>
		/// <param name="listener">Interface to receive EventListener notifications</param>
		/// <param name="testNames">The names of the test cases, fixtures or suites to be run</param>
		void BeginRun(NUnit.Core.EventListener listener, string[] testNames);

		TestResult[] EndRun();

		/// <summary>
		///  Cancel the test run that is in progress. For a synchronous run,
		///  a client wanting to call this must create a separate run thread.
		/// </summary>
		void CancelRun();

		/// <summary>
		/// Wait for the test run in progress to complete. For a synchronous run,
		/// a client wanting to call this must create a separate run thread. In
		/// particular, a gui client calling this method is likely to hang, since
		/// events will not be able to invoke methods on the gui thread.
		/// </summary>
		void Wait();
		#endregion
	}
}
