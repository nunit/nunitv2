// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Collections;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.TestUtilities
{
	/// <summary>
	/// Summary description for MockUiEventSource.
	/// </summary>
	public class MockTestEventSource : TestEventDispatcher
	{
		//private string testFileName;
		private TestNode test;

		public MockTestEventSource( TestNode test )
		{
			this.test = test;
			//this.testFileName = testFileName;
		}

		public void SimulateTestRun()
		{
			FireRunStarting( test.TestName.FullName, test.TestCount );

			TestResult result = SimulateTest( test, false );

			FireRunFinished( result );
		}

		private TestResult SimulateTest( TestNode test, bool ignore )
		{
			if ( test.RunState != RunState.Runnable )
				ignore = true;

			if ( test.IsSuite )
			{
				FireSuiteStarting( test.TestName );

				TestSuiteResult result = new TestSuiteResult( test, test.TestName.Name );

				foreach( TestNode childTest in test.Tests )
					result.AddResult( SimulateTest( childTest, ignore ) );

				FireSuiteFinished( result );

				return result;
			}
			else
			{
				FireTestStarting( test.TestName );
				
				TestCaseResult result = new TestCaseResult( test );

				result.RunState = ignore ? RunState.Ignored : RunState.Executed;
				
				FireTestFinished( result );

				return result;
			}
		}
	}
}
