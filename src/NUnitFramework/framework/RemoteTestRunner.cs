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
namespace NUnit.Core
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Runtime.Remoting;

	/// <summary>
	/// Summary description for RemoteTestRunner.
	/// </summary>
	/// 
	[Serializable]
	public class RemoteTestRunner : MarshalByRefObject
	{
		private TestSuite suite;
		private string fullName;
		private NUnit.Core.EventListener handler;
		private string assemblyName;
		private TextWriter stdOutWriter;
		private TextWriter stdErrWriter;

		public RemoteTestRunner() 
		{
		}

		public void Initialize(string assemblyName, NUnit.Core.EventListener handler, TextWriter stdOutWriter, TextWriter stdErrOut)
		{
			this.assemblyName = assemblyName;
			this.handler = handler;
			this.stdErrWriter = stdErrOut;
			this.stdOutWriter = stdOutWriter;
		}

		public void BuildSuite(string assemblyName, NUnit.Core.EventListener handler, TextWriter stdOutWriter, TextWriter stdErrOut) 
		{
			this.assemblyName = assemblyName;
			this.handler = handler;
			this.stdErrWriter = stdErrOut;
			this.stdOutWriter = stdOutWriter;

			BuildSuite();
		}

		public void BuildSuite() 
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build(assemblyName);
			TestName = suite.FullName;
		}

		public TestResult Run()
		{
			Console.SetOut(stdOutWriter);
			Console.SetError(stdErrWriter);

			Test test = FindByName(suite, fullName);
			TestResult result = test.Run(handler);
			return result;
		}

		private Test FindByName(Test test, string fullName)
		{
			if(test.FullName.Equals(fullName)) return test;
			
			Test result = null;
			if(test is TestSuite)
			{
				TestSuite suite = (TestSuite)test;
				foreach(Test testCase in suite.Tests)
				{
					result = FindByName(testCase, fullName);
					if(result != null) break;
				}
			}

			return result;
		}

		public string TestName 
		{
			get { return fullName; }
			set { fullName = value; }
		}
			
		public Test Test
		{
			get 
			{ return suite; }
		}
	}
}

