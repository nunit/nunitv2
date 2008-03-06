// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

namespace NUnit.Util
{
	using System;
	using NUnit.Core;

	/// <summary>
	/// Summary description for ResultSummarizer.
	/// </summary>
	public class ResultSummarizer
	{
		private int resultCount = 0;
		private int failureCount = 0;
		private int skipCount = 0;
		private int ignoreCount = 0;
		private int suitesNotRun = 0;
		
		private double time = 0.0d;
		private string name;

		public ResultSummarizer() { }

		public ResultSummarizer(TestResult result)
		{
			Summarize(result);
		}

		public ResultSummarizer(TestResult[] results)
		{
			foreach( TestResult result in results )
				Summarize(result);
		}

		public void Summarize( TestResult result )
		{
			if (this.name == null )
			{
				this.name = result.Name;
				this.time = result.Time;
			}

			if (result.Test.IsSuite)
			{
				if (!result.Executed)
					suitesNotRun++;
			}
			else
			{
				switch (result.RunState)
				{
					case RunState.Executed:
						resultCount++;
						if (result.IsFailure)
							failureCount++;
						break;
					case RunState.Ignored:
						ignoreCount++;
						break;
					case RunState.Explicit:
					case RunState.NotRunnable:
					case RunState.Runnable:
					case RunState.Skipped:
					default:
						skipCount++;
						break;
				}
			}

			if ( result.HasResults )
				foreach (TestResult childResult in result.Results)
					Summarize( childResult );
		}

		public string Name
		{
			get { return name; }
		}

		public bool Success
		{
			get { return failureCount == 0; }
		}

		public int ResultCount
		{
			get { return resultCount; }
		}

//		public int Errors
//		{
//			get { return visitor.Errors; }
//		}

		public int FailureCount 
		{
			get { return failureCount; }
		}

		public int SkipCount
		{
			get { return skipCount; }
		}

		public int IgnoreCount
		{
			get { return ignoreCount; }
		}

		public double Time
		{
			get { return time; }
		}

		public int TestsNotRun
		{
			get { return skipCount + ignoreCount; }
		}

		public int SuitesNotRun
		{
			get { return suitesNotRun; }
		}
	}
}
