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
	using NUnit.Extensions;

	/// <summary>
	/// Summary description for RepeatedTestTest.
	/// </summary>
	/// 
	[TestFixture]
	public class RepeatedTestTest
	{
		private TestSuite testSuite;

		public RepeatedTestTest()
		{
			testSuite = new TestSuite("RepeatedTest Suite");
			testSuite.Add(new SuccessTest());
			testSuite.Add(new SuccessTest());
		}

		[Test]
		public void RepeatTestFiveTimes()
		{
			NUnit.Core.TestCase baseCase = TestCaseBuilder.Make(new SuccessTest(), "Success");
			Test repeatTest = new RepeatedTest(baseCase, 5);
			Assertion.Assert(repeatTest.CountTestCases == 5);

			TestResult result = repeatTest.Run(NullListener.NULL);
			Assertion.Assert(result.IsSuccess);

			Assertion.Assert(result is TestSuiteResult);
			TestSuiteResult suiteResult=(TestSuiteResult)result;
			
			Assertion.AssertEquals(5,suiteResult.Results.Count);
		}
	}
}

/*
 * 
 * 
 * 
 * 
 * 
 * 
 *
 
namespace NUnit.Tests 
{
	using System;
	using NUnit.Framework;
	using NUnit.Extensions;

	/// <summary>
	/// Testing the RepeatedTest support.
	/// </summary>
	public class RepeatedTestTest: TestCase 
	{
		private TestSuite fSuite;
		/// <summary>
		/// 
		/// </summary>
		public class SuccessTest: TestCase 
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="name"></param>
			public SuccessTest(String name) : base(name) {}
			/// <summary>
			/// 
			/// </summary>
			public void Success() {}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public RepeatedTestTest(string name) : base(name) 
		{
			fSuite= new TestSuite();
			fSuite.AddTest(new SuccessTest("success"));
			fSuite.AddTest(new SuccessTest("success"));
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestRepeatedOnce() 
		{
			ITest test= new RepeatedTest(fSuite, 1);
			Assertion.AssertEquals(2, test.CountTestCases);
			TestResult result= new TestResult();
			test.Run(result);
			Assertion.AssertEquals(2, result.RunCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestRepeatedMoreThanOnce() 
		{
			ITest test= new RepeatedTest(fSuite, 3);
			Assertion.AssertEquals(6, test.CountTestCases);
			TestResult result= new TestResult();
			test.Run(result);
			Assertion.AssertEquals(6, result.RunCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestRepeatedZero() 
		{
			ITest test= new RepeatedTest(fSuite, 0);
			Assertion.AssertEquals(0, test.CountTestCases);
			TestResult result= new TestResult();
			test.Run(result);
			Assertion.AssertEquals(0, result.RunCount);
		}
		/// <summary>
		/// 
		/// </summary>
		public void TestRepeatedNegative() 
		{
			try 
			{
				ITest test= new RepeatedTest(fSuite, -1);
			} 
			catch (ArgumentOutOfRangeException) 
			{
				return;
			}
			Assertion.Fail("Should throw an ArgumentOutOfRangeException");
		}
	}
} 
*/
