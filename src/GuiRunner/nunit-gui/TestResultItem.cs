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
using System;

namespace NUnit.Gui
{
	using NUnit.Core;

	/// <summary>
	/// Summary description for TestResultItem.
	/// </summary>
	public class TestResultItem
	{
		private TestResult testResult;

		public TestResultItem(TestResult result)
		{
			testResult = result;
		}

		public override string ToString()
		{
			return String.Format("{0} : {1}", testResult.Test.Name, testResult.Message);
		}

		public string GetMessage()
		{
			return String.Format("{0} : {1}", testResult.Test.Name, testResult.Message);
		}

		public string StackTrace
		{
			get 
			{
				string stackTrace = "No stack trace is available";
				if(testResult.StackTrace != null)
					stackTrace = StackTraceFilter.Filter(testResult.StackTrace);

				return stackTrace;
			}
		}
	}
}
