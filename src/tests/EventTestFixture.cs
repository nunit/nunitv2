/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
namespace NUnit.Tests
{
	using System;
	using NUnit.Framework;
	using NUnit.Core;

	/// <summary>
	/// Summary description for EventTestFixture.
	/// </summary>
	/// 
	[TestFixture]
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
			internal int testCaseStart = 0;
			internal int testCaseFinished = 0;
			internal int suiteStarted = 0;
			internal int suiteFinished = 0;

			public void TestStarted(NUnit.Core.TestCase testCase)
			{
				testCaseStart++;
			}
			
			public void TestFinished(TestCaseResult result)
			{
				testCaseFinished++;
			}

			public void SuiteStarted(TestSuite suite)
			{
				suiteStarted++;
			}

			public void SuiteFinished(TestSuiteResult result)
			{
				suiteFinished++;
			}
		}

		[Test]
		public void CheckEventListening()
		{
			TestSuite testSuite = TestSuiteBuilder.Build(testsDll);
			
			EventCounter counter = new EventCounter();
			TestResult result = testSuite.Run(counter);
			Assertion.AssertEquals(testSuite.CountTestCases, counter.testCaseStart);
			Assertion.AssertEquals(testSuite.CountTestCases, counter.testCaseFinished);

			int suites = SuiteCount(testSuite);
			Assertion.AssertEquals(suites, counter.suiteStarted);
			Assertion.AssertEquals(suites, counter.suiteFinished);
		}
	}
}
