//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Core
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
