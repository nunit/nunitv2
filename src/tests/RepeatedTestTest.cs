#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

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
