#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
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
