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
namespace NUnit.Tests.TestResultTests
{
	using System;
	using System.Collections;
	using NUnit.Framework;
	using NUnit.Core;
	
	/// <summary>
	/// Summary description for TestResultTests.
	/// </summary>
	/// 
	[TestFixture]
	public class TestSuiteResultFixture
	{
		private TestCaseResult testCase;

		private TestSuiteResult MockSuiteResult()
		{
			TestSuiteResult result = new TestSuiteResult(null, "base");

			TestSuiteResult level1SuiteA = new TestSuiteResult(null, "level 1 A");
			result.AddResult(level1SuiteA);

			TestSuiteResult level1SuiteB = new TestSuiteResult(null, "level 1 B");
			result.AddResult(level1SuiteB);

			testCase = new TestCaseResult("a test case");
			level1SuiteA.AddResult(testCase);

			level1SuiteB.AddResult(new TestCaseResult("a successful test"));

			return result;
		}

		[Test]
		public void EmptySuite()
		{
			TestSuiteResult result = new TestSuiteResult(null, "base suite");
			Assertion.Assert("result should be success", result.IsSuccess);
		}

		[Test]
		public void SuiteSuccess()
		{
			Assertion.Assert(MockSuiteResult().IsSuccess);
		}

		[Test]
		public void TestSuiteFailure()
		{
			TestSuiteResult result = MockSuiteResult();
			AssertionException failure = new AssertionException("an assertion failed error");
			testCase.Failure(failure.Message, failure.StackTrace);
			
			Assertion.Assert(result.IsFailure);
			Assertion.Assert(!result.IsSuccess); 

			IList results = result.Results;
			TestSuiteResult suiteA = (TestSuiteResult)results[0];
			Assertion.Assert(suiteA.IsFailure);

			TestSuiteResult suiteB = (TestSuiteResult)results[1];
			Assertion.Assert(suiteB.IsSuccess);
		}		
	}
}
