//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Core
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
