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
