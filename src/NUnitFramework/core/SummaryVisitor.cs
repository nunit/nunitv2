//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Core
{
	using System;

	/// <summary>
	/// Summary description for SiummaryVisitor.
	/// </summary>
	public class SummaryVisitor : ResultVisitor
	{
		private int totalCount;
		private int failureCount;
		private int testsNotRun;
		private int suitesNotRun;
		
		private double time;
		private string name;
		private bool initialized;

		public SummaryVisitor()
		{
			totalCount = 0;
			initialized = false;
		}

		public void visit(TestCaseResult caseResult) 
		{
			if(caseResult.Executed)
			{
				totalCount++;
				if(caseResult.IsFailure)
					failureCount++;
			}
			else
				testsNotRun++;
		}

		public void visit(TestSuiteResult suiteResult) 
		{
			SetNameandTime(suiteResult.Name, suiteResult.Time);

			
			
			foreach (TestResult result in suiteResult.Results)
			{
				result.Accept(this);
			}
			
			if(!suiteResult.Executed)
				suitesNotRun++;
		}

		public double Time
		{
			get { return time; }
		}

		private void SetNameandTime(string name, double time)
		{
			if(!initialized)
			{
				this.time = time;
				this.name = name;
				initialized = true;
			}
		}

		public bool Success
		{
			get { return (failureCount == 0); }
		}

		public int Count
		{
			get { return totalCount; }
		}

		public int Failures
		{
			get { return failureCount; }
		}

		public int TestsNotRun
		{
			get { return testsNotRun; }
		}

		public int SuitesNotRun
		{
			get { return suitesNotRun; }
		}

		public string Name
		{
			get { return name; }
		}
	}
}
