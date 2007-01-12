using System;
using System.Reflection;

namespace NUnit.Core.Extensions
{
	/// <summary>
	/// Summary description for MaxTimeTestCase.
	/// </summary>
	public class MaxTimeTestCase : TestCase
	{
		private TestCase testCase;
		private int maxTime = 0;

		public MaxTimeTestCase( TestCase testCase, int maxTime )
			: base( testCase.TestName.FullName, testCase.TestName.Name )
		{
			this.testCase = testCase;
			this.maxTime = maxTime;
		}

		public override void Run(TestCaseResult result)
		{
			testCase.Run( result );
			if ( result.IsSuccess )
			{
				int elapsedTime = (int)(result.Time * 1000);
				if ( elapsedTime > maxTime )
					result.Failure( string.Format( "Elapsed time of {0}ms exceeds maximum of {1}ms", elapsedTime, maxTime ), null );
			}
		}

	}
}
