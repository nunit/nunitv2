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

	/// <summary>
	/// Summary description for TestResult.
	/// </summary>
	/// 
	[Serializable]
	public abstract class TestResult
	{
		private bool isFailure; 
		private double time;
		private string name;
		private Test test;
		
		protected TestResult(Test test, string name)
		{
			this.name = name;
			this.test = test;
		}

		public virtual string Name
		{
			get{ return name;}
		}

		public Test Test
		{
			get{ return test;}
		}

		public virtual bool IsSuccess
		{
			get { return !(isFailure); }
		}
		
		public virtual bool IsFailure
		{
			get { return isFailure; }
			set { isFailure = value; }
		}

		public double Time 
		{
			get{ return time; }
			set{ time = value; }
		}

		public abstract string Message
		{
			get;
		}

		public abstract string StackTrace
		{
			get;
		}

		public abstract void NotRun(string message);


		public abstract void Accept(ResultVisitor visitor);
	}
}
