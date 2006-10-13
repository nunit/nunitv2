#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
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
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Core
{
	using System;
	using System.Collections;
	using System.Reflection;

	/// <summary>
	/// The abstract TestCase class represents a single test case.
	/// In the present implementation, the only derived class is
	/// TestMethod, but we allow for future test cases which are
	/// implemented in other ways.
	/// </summary>
	public abstract class TestCase : Test
	{
		public TestCase( string path, string name ) : base( path, name ) { }

		public TestCase( MethodInfo method ) : base ( method ) { }

		public override int CountTestCases( ITestFilter filter ) 
		{
			if (Filter(filter))
				return 1;

			return 0;
		}

		public override TestResult Run(EventListener listener, ITestFilter filter)
		{
			return Run( listener ); // Ignore filter for now
		}

		public override TestResult Run( EventListener listener )
		{
			TestCaseResult testResult = new TestCaseResult( this );

			listener.TestStarted( this.TestName );
			long startTime = DateTime.Now.Ticks;

            switch (this.RunState)
            {
                case RunState.Runnable:
				case RunState.Explicit:
                    Run(testResult);
                    break;
                case RunState.Skipped:
                    testResult.Skip(IgnoreReason);
                    break;
                default:
                case RunState.NotRunnable:
                case RunState.Ignored:
                    testResult.Ignore(IgnoreReason);
                    break;
            }

			long stopTime = DateTime.Now.Ticks;
			double time = ((double)(stopTime - startTime)) / (double)TimeSpan.TicksPerSecond;
			testResult.Time = time;

			listener.TestFinished(testResult);
			return testResult;
		}

		public override string TestType
		{
			get { return "Test Case"; }
		}

		public override bool IsSuite
		{
			get { return false; }
		}

		public override IList Tests
		{
			get { return null; }
		}

		public abstract void Run(TestCaseResult result);
	}
}
