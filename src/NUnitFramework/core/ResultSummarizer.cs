//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Core
{
	using System;

	/// <summary>
	/// Summary description for ResultSummarizer.
	/// </summary>
	public class ResultSummarizer
	{
		private SummaryVisitor visitor = new SummaryVisitor();

		public ResultSummarizer(TestResult result)
		{
			result.Accept(visitor);
		}

		public string Name
		{
			get { return visitor.Name; }
		}

		public bool Success
		{
			get { return visitor.Success; }
		}

		public int ResultCount
		{
			get { return visitor.Count; }
		}

//		public int Errors
//		{
//			get { return visitor.Errors; }
//		}

		public int Failures 
		{
			get { return visitor.Failures; }
		}

		public double Time
		{
			get { return visitor.Time; }
		}

		public int TestsNotRun
		{
			get { return visitor.TestsNotRun; }
		}

		public int SuitesNotRun
		{
			get { return visitor.SuitesNotRun; }
		}
	}
}
