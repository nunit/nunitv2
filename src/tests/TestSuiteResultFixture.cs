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
