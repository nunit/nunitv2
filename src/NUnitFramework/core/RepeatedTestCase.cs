using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for RepeatedTestCase.
	/// </summary>
	public class RepeatedTestCase : TemplateTestCase
	{
		private int count;
		private bool failed = false;

		public RepeatedTestCase(Type fixtureType, MethodInfo method, int count) : base(fixtureType, method)
		{
			this.count = count;
		}

		public RepeatedTestCase(object fixture, MethodInfo method, int count) : base(fixture, method)
		{
			this.count = count;
		}

		protected internal override void ProcessNoException(TestCaseResult testResult)
		{
			testResult.Success();
		}
		
		protected internal override void ProcessException(Exception exception, TestCaseResult testResult)
		{
			failed = true;
			RecordException( exception, testResult );
		}

//		public override void RunTestMethod (TestCaseResult testResult)
//		{
//			TestCaseResult res = new TestCaseResult(this);
//			for (int i = 0; i < count; i++)
//			{
//				base.RunTestMethod (testResult);
//			}
//		}

		public override void doRun(TestCaseResult testResult)
		{
			failed = false;
			int i = 0;
			while (i < count && !failed)
			{
				i++;
				base.doRun (testResult);
			}
		}

	}
}
