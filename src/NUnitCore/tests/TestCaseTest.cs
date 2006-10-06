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
using System.Reflection;
using NUnit.Framework;
using NUnit.Tests.Assemblies;
using NUnit.Util;
using NUnit.TestData.TestCaseTest;

namespace NUnit.Core.Tests
{
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
			Type fixtureType = typeof(MockTestFixture);
			TestCase testCase = TestCaseBuilder.Make( fixtureType, "MockTest4" );
			Assert.AreEqual(1, testCase.TestCount);
			Assert.AreEqual( RunState.Ignored, testCase.RunState );
			Assert.AreEqual("ignoring this test method for now", testCase.IgnoreReason);
		}

		[Test]
		public void RunIgnoredTestCase()
		{
			Type fixtureType = typeof(MockTestFixture);
			TestCase testCase = TestCaseBuilder.Make( fixtureType, "MockTest4" );
			Assert.AreEqual(1, testCase.TestCount);
			
			TestResult result = testCase.Run(NullListener.NULL);
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assert.AreEqual(0, summarizer.ResultCount);
			Assert.AreEqual(1, summarizer.TestsNotRun);
		}

		[Test]
		public void LoadMethodCategories() 
		{
			Type fixtureType = typeof(HasCategories);
			TestCase testCase = TestCaseBuilder.Make( fixtureType, "ATest" );
			Assert.IsNotNull(testCase);
			Assert.IsNotNull(testCase.Categories);
			Assert.AreEqual(2, testCase.Categories.Count);
		}
	}
}
