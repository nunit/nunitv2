using System;
using System.Reflection;
using NUnit.Core;

namespace NUnit.Extensions
{
	/// <summary>
	/// Summary description for RepeatedTestCase.
	/// </summary>
	public class RepeatedTestCase : TemplateTestCase
	{
		private int count;
		private bool failed = false;

		public RepeatedTestCase( MethodInfo method, int count ) : base( method )
		{
			this.count = count;
		}

		protected override void ProcessNoException(TestCaseResult testResult)
		{
			testResult.Success();
		}
		
		protected override void ProcessException(Exception exception, TestCaseResult testResult)
		{
			failed = true;
			RecordException( exception, testResult );
		}

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
