//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Extensions
{
	using System;
	using NUnit.Core;

	/// <summary>
	/// Summary description for RepeatedTest.
	/// </summary>
	public class RepeatedTest : Test
	{
		private int repeatCount;
		private TestCase testCase;

		public RepeatedTest(TestCase testCase, int repeatCount) : base(testCase.Name)
		{
			this.testCase = testCase;
			this.repeatCount = repeatCount;
		}

		public override int CountTestCases 
		{
			get { return repeatCount; }
		}

		public override TestResult Run(EventListener listener)
		{
			TestSuiteResult suiteResult = new TestSuiteResult(this, testCase.Name);

			for(int i = 0; i < repeatCount; i++)
			{
				TestResult testResult = testCase.Run(NullListener.NULL);
				suiteResult.AddResult(testResult);
		
				if(!testResult.IsSuccess)
					break;
			}
		
			return suiteResult;
		}
	}
}
