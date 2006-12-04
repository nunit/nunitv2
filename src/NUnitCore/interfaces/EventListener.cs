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

namespace NUnit.Core
{
	using System;

	/// <summary>
	/// The EventListener interface is used within the NUnit core to receive 
	/// notifications of significant events while a test is being run. These
	/// events are propogated to any client, which may choose to convert them
	/// to .NET events or to use them directly.
	/// </summary>
	public interface EventListener
	{
		/// <summary>
		/// Called when a test run is starting
		/// </summary>
		/// <param name="name">The name of the test being started</param>
		/// <param name="testCount">The number of test cases under this test</param>
		void RunStarted( string name, int testCount );

		/// <summary>
		/// Called when a run finishes normally
		/// </summary>
		/// <param name="result">The result of the test</param>
		void RunFinished( TestResult result );

		/// <summary>
		/// Called when a run is terminated due to an exception
		/// </summary>
		/// <param name="exception">Exception that was thrown</param>
		void RunFinished( Exception exception );

		/// <summary>
		/// Called when a test case is starting
		/// </summary>
		/// <param name="testName">The name of the test case</param>
		void TestStarted(TestName testName);
			
		/// <summary>
		/// Called when a test case has finished
		/// </summary>
		/// <param name="result">The result of the test</param>
		void TestFinished(TestCaseResult result);

		/// <summary>
		/// Called when a suite is starting
		/// </summary>
		/// <param name="testName">The name of the suite</param>
		void SuiteStarted(TestName testName);

		/// <summary>
		/// Called when a suite has finished
		/// </summary>
		/// <param name="result">The result of the suite</param>
		void SuiteFinished(TestSuiteResult result);

		/// <summary>
		/// Called when an unhandled exception is detected during
		/// the execution of a test run.
		/// </summary>
		/// <param name="exception">The exception thta was detected</param>
		void UnhandledException( Exception exception );

		/// <summary>
		/// Called when the test direts output to the console.
		/// </summary>
		/// <param name="testOutput">A console message</param>
		void TestOutput(TestOutput testOutput);
	}
}
