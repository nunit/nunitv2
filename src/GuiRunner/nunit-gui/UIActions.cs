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
	using System.Collections;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using Microsoft.Win32;
	using NUnit.Core;
	using NUnit.Util;


	/// <summary>
	/// Summary description for UIActions.
	/// </summary>
	public class UIActions : MarshalByRefObject, NUnit.Core.EventListener
	{
		private NUnitForm form;
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

		public UIActions(NUnitForm form)
		{
			this.form = form;
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
			testRunner = new TestRunner(assemblyFileName,this,form.stdOutWriter, form.stdErrWriter);
			
			Test test = LoadTestSuite(assemblyFileName);

			if ( AssemblyLoadedEvent != null )
				AssemblyLoadedEvent( test );
		}
	}
}

