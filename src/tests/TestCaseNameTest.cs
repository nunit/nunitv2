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
	using System.Collections;
	using NUnit.Core;
	using NUnit.Framework;

	[TestFixture]
	public class TestCaseNameTest
	{
		[Test]
		public void TestName()
		{
			TestSuite suite = new TestSuite("mock suite");
			OneTestCase oneTestCase = new OneTestCase();
			suite.Add(oneTestCase);
			
			IList tests = suite.Tests;
			TestSuite rootSuite = (TestSuite)tests[0];
			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)rootSuite.Tests[0];
			Assertion.AssertEquals("NUnit.Tests.OneTestCase.TestCase", testCase.FullName);
			Assertion.AssertEquals("TestCase", testCase.Name);
		}

		[Test]
		public void TestExpectedException()
		{
			TestSuite suite = new TestSuite("mock suite");
			suite.Add(new ExpectExceptionTest());
 
			IList tests = suite.Tests;
			TestSuite rootSuite = (TestSuite)tests[0];
			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)rootSuite.Tests[0];
			Assertion.AssertEquals("NUnit.Tests.ExpectExceptionTest.TestSingle", testCase.FullName);
		}
	}
}
