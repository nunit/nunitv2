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
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for EventTestFixture.
	/// </summary>
	/// 
	[TestFixture(Description="Tests that proper events are generated when running  test")]
	public class EventTestFixture
	{
		private string testsDll = "mock-assembly.dll";

		private static int SuiteCount(TestSuite suite)
		{
			int suites = 1;

			foreach(Test test in suite.Tests)
			{
				if(test is TestSuite)
					suites += SuiteCount((TestSuite)test);
			}

			return suites;
		}

		internal class EventCounter : EventListener
		{
			internal int runStarted = 0;
			internal int runFinished = 0;
			internal int testCaseStart = 0;
			internal int testCaseFinished = 0;
			internal int suiteStarted = 0;
			internal int suiteFinished = 0;

			public void RunStarted(string name, int testCount)
			{
				runStarted++;
			}

			public void RunFinished(NUnit.Core.TestResult result)
			{
				runFinished++;
			}

			public void RunFinished(Exception exception)
			{
				runFinished++;
			}

			public void TestStarted(TestName testName)
			{
				testCaseStart++;
			}
			
			public void TestFinished(TestCaseResult result)
			{
				testCaseFinished++;
			}

			public void SuiteStarted(TestName suiteName)
			{
				suiteStarted++;
			}

			public void SuiteFinished(TestSuiteResult result)
			{
				suiteFinished++;
			}

			public void UnhandledException( Exception exception )
			{
			}

			public void TestOutput(TestOutput testOutput)
			{
			}
		}

		[Test]
		public void CheckEventListening()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite testSuite = builder.Build(testsDll);
			
			EventCounter counter = new EventCounter();
			testSuite.Run(counter);
			Assert.AreEqual(testSuite.CountTestCases(), counter.testCaseStart);
			Assert.AreEqual(testSuite.CountTestCases(), counter.testCaseFinished);

			int suites = SuiteCount(testSuite);
			Assert.AreEqual(suites, counter.suiteStarted);
			Assert.AreEqual(suites, counter.suiteFinished);
		}
	}
}

