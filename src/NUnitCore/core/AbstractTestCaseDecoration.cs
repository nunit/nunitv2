using System;

namespace NUnit.Core
{
	/// <summary>
	/// TestCaseDecorator is used to add functionality to
	/// another TestCase, which it aggregates.
	/// </summary>
	public abstract class AbstractTestCaseDecoration : TestCase
	{
		protected TestCase testCase;

		public AbstractTestCaseDecoration( TestCase testCase )
			: base( testCase.FullName, testCase.Name )
		{
			this.testCase = testCase;
			this.ShouldRun = testCase.ShouldRun;
			this.IgnoreReason = testCase.IgnoreReason;
			this.IsRunnable = testCase.IsRunnable;
		}

		public override int CountTestCases()
		{
			return testCase.CountTestCases();
		}

		public override int TestCount
		{
			get { return testCase.TestCount; }
		}
	}
}
