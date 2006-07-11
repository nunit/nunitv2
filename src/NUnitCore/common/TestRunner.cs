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
	/// assemblies, returning a tree of TestNodes to the caller.
	/// 
	/// The CountTestCases family of methods returns the number of test cases in the
	/// loaded suite, either in its entirety or by using a filter to count a subset of tests.
	/// 
	/// The Run family of methods performs a test run synchronously, returning a TestResult
	/// or TestResult[] to the caller. If provided, an EventListener interface will be 
	/// notified of significant events in the running of the tests. A filter may be used
    /// to run a subset of the tests.
    ///
    /// BeginRun and EndRun provide a simplified form of the asynchronous invocation
	/// pattern used in many places within the .NET framework. Because the current
	/// implementation allows only one run to be in process at a time, an IAsyncResult
	/// is not used at this time.
    /// 
    /// Methods to cancel a run and to wait for a run to complete are also provided. The 
    /// result of the last run may be obtained by querying the TestResult property.
    /// 
    /// </summary>
	public interface TestRunner
	{
		#region Properties
		/// <summary>
		/// TestRunners are identified by an ID. So long as there
		/// is only one test runner or a single chain of test runners,
		/// the default id of 0 may be used. However, any client that
		/// creates multiple runners must ensure that each one has a
		/// unique ID in order to locate and run specific tests.
		/// </summary>
		int ID
		{
			get;
		}

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
		/// Returns a list of the AssemblyQualifiedName of each loaded extension.
		/// </summary>
		IList Extensions
		{
			get;
		}

		/// <summary>
		/// The loaded test, converted to a tree of TestNodes so they can be
		/// serialized and marshalled to a remote client.
		/// </summary>
		TestNode Test
		{
			get;
		}

		/// <summary>
		/// Result of the last test run.
		/// </summary>
		TestResult TestResult
		{
			get;
		}

		TestRunnerSettings Settings
		{
			get;
		}
		#endregion

		#region Load and Unload Methods
		/// <summary>
		/// Load all tests from an assembly
		/// </summary>
		/// <param name="assemblyName">The assembly from which tests are to be loaded</param>
		bool Load( string assemblyName );

		/// <summary>
		/// Load a particular test in an assembly
		/// </summary>
		/// <param name="assemblyName">The assembly from which tests are to be loaded</param>
		/// <param name="testName">The name of the test fixture or suite to be loaded</param>
		bool Load( string assemblyName, string testName );

		/// <summary>
		/// Load the assemblies in a test project
		/// </summary>
		/// <param name="projectName">The name of the test project being loaded</param>
		/// <param name="assemblies">The assemblies comprising the project</param>
		/// <returns>The loaded test</returns>
		bool Load( string projectName, string[] assemblies );

		/// <summary>
		/// Load a particular test in a TestProject.
		/// </summary>
		/// <param name="projectName">The name of the test project being loaded</param>
		/// <param name="assemblies">The assemblies comprising the project</param>
		/// <param name="testName">The name of the test fixture or suite to be loaded</param>
		/// <returns>The loaded test</returns>
		bool Load( string projectName, string[] assemblies, string testName );

		/// <summary>
		/// Unload all tests previously loaded
		/// </summary>
		void Unload();
		#endregion

		#region CountTestCases Methods
		/// <summary>
		/// Count Test Cases using a filter
		/// </summary>
		/// <param name="testName">The filter to apply</param>
		/// <returns>The number of test cases found</returns>
		int CountTestCases(TestFilter filter );
		#endregion

		#region Run Methods
		/// <summary>
		/// Run all loaded tests and return a test result. The test is run synchronously,
		/// and the listener interface is notified as it progresses.
		/// </summary>
		/// <param name="listener">Interface to receive EventListener notifications.</param>
		TestResult Run(NUnit.Core.EventListener listener);

		/// <summary>
		/// Run selected tests and return a test result. The test is run synchronously,
		/// and the listener interface is notified as it progresses.
		/// </summary>
		/// <param name="listener">Interface to receive EventListener notifications.</param>
		TestResult Run(NUnit.Core.EventListener listener, TestFilter filter);
		
		/// <summary>
		/// Start a run of all loaded tests. The tests are run aynchronously and the 
		/// listener interface is notified as it progresses.
		/// </summary>
		/// <param name="listener">Interface to receive EventListener notifications.</param>
		void BeginRun(NUnit.Core.EventListener listener);

		/// <summary>
		/// Start a run of selected tests. The tests are run aynchronously and the 
		/// listener interface is notified as it progresses.
		/// </summary>
		/// <param name="listener">Interface to receive EventListener notifications.</param>
		void BeginRun(NUnit.Core.EventListener listener, TestFilter filter);
		
		/// <summary>
		/// Wait for an asynchronous run to complete and return the result.
		/// </summary>
		/// <returns>A TestResult for the entire run</returns>
		TestResult EndRun();

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
