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
	///		Test Class.
	/// </summary>
	public abstract class Test : MarshalByRefObject
	{
		private string fullName;
		private string testName;
		private bool shouldRun;
		private string ignoreReason;

		protected Test(string pathName, string testName) 
		{ 
			fullName = pathName + "." + testName;
			this.testName = testName;
			shouldRun = true;
		}

		public string IgnoreReason
		{
			get { return ignoreReason; }
			set { ignoreReason = value; }
		}

		public bool ShouldRun
		{
			get { return shouldRun; }
			set { shouldRun = value; }
		}

		public Test(string name)
		{
			fullName = testName = name;
		}

		public string FullName 
		{
			get { return fullName; }
		}

		public string Name
		{
			get { return testName; }
		}

		public abstract int CountTestCases { get; }
		public abstract TestResult Run(EventListener listener);
	}
}
