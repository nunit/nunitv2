/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
namespace NUnit.Gui
{
	using System;
	using System.IO;
	using NUnit.Core;
	using NUnit.Util;


	/// <summary>
	/// UIActions handles interactions between a test runner and a client
	/// program - typically the user interface. It implemements the
	/// EventListener interface which is used by the test runner and
	/// fires events in order to provide the information to any component
	/// that is interested. It also fires events for starting the test
	/// run, ending it and successfully loading the assembly.
	/// </summary>
	public class UIActions : MarshalByRefObject, NUnit.Core.EventListener
	{
		/// <summary>
		/// StdOut stream for use by the TestRunner
		/// </summary>
		private TextWriter stdOutWriter;

		/// <summary>
		/// StdErr stream for use by the TestRunner
		/// </summary>
		private TextWriter stdErrWriter;

		/// <summary>
		/// Loads an assembly and executes tests.
		/// </summary>
		private TestRunner testRunner = null;

		public delegate void TestStartedHandler( TestCase testCase );
		public delegate void TestFinishedHandler( TestCaseResult result );
		public delegate void SuiteStartedHandler( TestSuite suite );
		public delegate void SuiteFinishedHandler( TestSuiteResult result );
		public delegate void RunStartingHandler( Test test );
		public delegate void RunFinishedHandler( TestResult result );
		public delegate void AssemblyLoadedHandler( Test test );

		public event TestStartedHandler TestStartedEvent;
		public event TestFinishedHandler TestFinishedEvent;
		public event SuiteStartedHandler SuiteStartedEvent;
		public event SuiteFinishedHandler SuiteFinishedEvent;
		public event RunStartingHandler RunStartingEvent;
		public event RunFinishedHandler RunFinishedEvent;
		public event AssemblyLoadedHandler AssemblyLoadedEvent;

		public UIActions(TextWriter stdOutWriter, TextWriter stdErrWriter)
		{
			this.stdOutWriter = stdOutWriter;
			this.stdErrWriter = stdErrWriter;
		}

		public void TestStarted(TestCase testCase)
		{
			if ( TestStartedEvent != null )
				TestStartedEvent( testCase );
		}

		public void SuiteStarted(TestSuite suite)
		{
			if ( SuiteStartedEvent != null )
				SuiteStartedEvent( suite );
		}

		public void SuiteFinished(TestSuiteResult result)
		{
			if ( SuiteFinishedEvent != null )
				SuiteFinishedEvent( result );
		}

		public void TestFinished(TestCaseResult result)
		{
			if ( TestFinishedEvent != null )
				TestFinishedEvent( result );
		}

		public void RunTestSuite(Test suite)
		{
			if ( RunStartingEvent != null )
				RunStartingEvent( suite );

			testRunner.TestName = suite.FullName;

			TestResult result = testRunner.Run();

			if ( RunFinishedEvent != null )
				RunFinishedEvent( result );
		}

		private Test LoadTestSuite(string assemblyFileName)
		{
			return testRunner.Test;
		}
		
		public void LoadAssembly(string assemblyFileName, Test suite)
		{
			testRunner = new TestRunner(assemblyFileName, this, stdOutWriter, stdErrWriter);
			
			Test test = LoadTestSuite(assemblyFileName);

			if ( AssemblyLoadedEvent != null )
				AssemblyLoadedEvent( test );
		}
	}
}

