//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
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
