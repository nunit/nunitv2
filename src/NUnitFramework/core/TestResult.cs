//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
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
