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
	using NUnit.Tests.Assemblies;
	using NUnit.Core;

	/// <summary>
	/// Summary description for TestCaseTest.
	/// </summary>
	/// 
	[TestFixture]
	public class TestCaseTest
	{
		[Test]
		public void CreateIgnoredTestCase()
		{
			MockTestFixture mockTestFixture = new MockTestFixture();
			NUnit.Core.TestCase testCase = TestCaseBuilder.Make(mockTestFixture, "MockTest4"); 
			Assertion.AssertEquals(1, testCase.CountTestCases);
			Assertion.AssertEquals(false, testCase.ShouldRun);
			Assertion.AssertEquals("ignoring this test method for now", testCase.IgnoreReason);
		}

		[Test]
		public void RunIgnoredTestCase()
		{
			MockTestFixture mockTestFixture = new MockTestFixture();
			NUnit.Core.TestCase testCase = TestCaseBuilder.Make(mockTestFixture, "MockTest4"); 
			Assertion.AssertEquals(1, testCase.CountTestCases);
			
			TestResult result = testCase.Run(NullListener.NULL);
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assertion.AssertEquals(0, summarizer.ResultCount);
			Assertion.AssertEquals(1, summarizer.TestsNotRun);
		}
	}
}
