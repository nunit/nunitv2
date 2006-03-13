using System;
using System.Reflection;

namespace NUnit.Core.Extensions
{
	/// <summary>
	/// Summary description for RepeatedTestCase.
	/// </summary>
	public class OldRepeatedTestCase : TestMethod
	{
		private int count;
		private bool failed = false;

		public OldRepeatedTestCase( MethodInfo method, int count ) : base( method )
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

	/// <summary>
	/// RepeatedTestCase aggregates another test case and runs it
	/// a specified number of times.
	/// </summary>
	public class RepeatedTestCase : AbstractTestCaseDecoration
	{
		// The number of times to run the test
		int count;

		public RepeatedTestCase( TestCase testCase, int count )
			: base( testCase )
		{
			this.count = count;
		}

		public override void Run(TestCaseResult result)
		{
			// So testCase can get the fixture
			testCase.Parent = this.Parent;

			for( int i = 0; i < count; i++ )
			{
				testCase.Run( result );
				if ( result.IsFailure )
					return;
			}
		}
	}
}
