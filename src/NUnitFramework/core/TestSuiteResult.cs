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
	using System.Collections;

	/// <summary>
	///		TestSuiteResult
	/// </summary>
	/// 
	[Serializable]
	public class TestSuiteResult : TestResult
	{
		private ArrayList results = new ArrayList();
		private bool executed;
		private string message;
		
		public TestSuiteResult(Test test, string name) : base(test, name)
		{
			executed = false;
		}

		public bool Executed 
		{
			get { return executed; }
			set { executed = value; }
		}

		public void AddResult(TestResult result) 
		{
			results.Add(result);
		}

		public override bool IsSuccess
		{
			get 
			{
				bool result = true;
				foreach(TestResult testResult in results)
					result &= testResult.IsSuccess;
				return result;
			}
		}

		public override bool IsFailure
		{
			get 
			{
				bool result = false;
				foreach(TestResult testResult in results)
					result |= testResult.IsFailure;
				return result;
			}
		}

		public override void NotRun(string message)
		{
			this.Executed = false;
			this.message = message;
		}


		public override string Message
		{
			get { return message; }
		}

		public override string StackTrace
		{
			get { return null; }
		}


		public IList Results
		{
			get { return results; }
		}

		public override void Accept(ResultVisitor visitor) 
		{
			visitor.visit(this);
		}
	}
}
