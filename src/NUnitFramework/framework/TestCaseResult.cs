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
	using System.Text;

	/// <summary>
	/// 
	/// </summary>
	//
	[Serializable]
	public class TestCaseResult : TestResult
	{
		private TestCase testCase;
		private string testCaseName;
		private bool testExecuted;
		private string message;
		private string stackTrace;

		public TestCaseResult(TestCase testCase):base(testCase, testCase.FullName)
		{
			this.testCase = testCase;
			testExecuted = false;
		}

		public TestCaseResult(string testCaseString) : base(null, testCaseString)
		{
			testCase = null;
			testExecuted = false;
			testCaseName = testCaseString;
		}

		public bool Executed
		{
			get { return testExecuted; }
		}

		public void Success() 
		{ 
			testExecuted = true;
			IsFailure = false; 
		}

		public override void NotRun(string reason)
		{
			testExecuted = false;
			message = reason;
		}

		public void Failure(string message, string stackTrace)
		{
			testExecuted = true;
			IsFailure = true;
			this.message = message;
			this.stackTrace = stackTrace;
		}

		public override string Message
		{
			get { return message; }
		}

		public override string StackTrace
		{
			get 
			{ 
				return stackTrace;
			}
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			string name = testCaseName;
			if(testCase != null)
				name = testCase.FullName;
			
			builder.AppendFormat("{0} : " , name);
			if(!IsSuccess)
				builder.Append(message);

			return builder.ToString();
		}

		public override void Accept(ResultVisitor visitor) 
		{
			visitor.visit(this);
		}
	}
}
