#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

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

			TestResult result = SimulateTest( test );

			FireRunFinished( result );
		}

		private TestResult SimulateTest( TestNode test )
		{
			if ( test.IsSuite )
			{
				FireSuiteStarting( test.TestName );

				TestSuiteResult result = new TestSuiteResult( test, test.TestName.Name );

				foreach( TestNode childTest in test.Tests )
					result.AddResult( SimulateTest( childTest ) );

				FireSuiteFinished( result );

				return result;
			}
			else
			{
				FireTestStarting( test.TestName );
				
				TestCaseResult result = new TestCaseResult( test );
				if ( test.RunState == RunState.Runnable )
					result.RunState = RunState.Executed;
				else
					result.RunState = RunState.Ignored;
				
				FireTestFinished( result );

				return result;
			}
		}
	}
}
