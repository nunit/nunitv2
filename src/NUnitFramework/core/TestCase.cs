//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Core
{
	using System;

	/// <summary>
	/// Summary description for TestCase.
	/// </summary>
	public abstract class TestCase : Test
	{
		public TestCase(string path, string name) : base(path, name)
		{}

		public override int CountTestCases 
		{
			get { return 1; }
		}

		public override TestResult Run(EventListener listener)
		{
			TestCaseResult testResult = new TestCaseResult(this);

			listener.TestStarted(this);

			long startTime = DateTime.Now.Ticks;

			Run(testResult);

			long stopTime = DateTime.Now.Ticks;

			double time = ((double)(stopTime - startTime)) / (double)TimeSpan.TicksPerSecond;

			testResult.Time = time;

			listener.TestFinished(testResult);
	
			return testResult;
		}


		public abstract void Run(TestCaseResult result);

	}
}
