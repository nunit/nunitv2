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
	using NUnit.Tests.Singletons;
	using NUnit.Tests.Assemblies;
	using NUnit.Util;

	/// <summary>
	/// Summary description for UtilTest.
	/// </summary>
	/// 
	[TestFixture]
	public class UtilTest
	{
		OneTestCase oneTestFixture;
		MockTestFixture mockTestFixture;

		[SetUp]
		public void SetUp()
		{
			oneTestFixture = new OneTestCase();
			mockTestFixture = new MockTestFixture();
		}

		[Test]
		public void CompareTreeToSelf()
		{
			TestSuite suite = new TestSuite("Test Suite");
			suite.Add(oneTestFixture);

			Assertion.Assert(UIHelper.CompareTree(suite,suite));
		}

		[Test]
		public void CompareStructurallyDifferentTrees()
		{
			TestSuite treeOne = new TestSuite("Test Suite");
			treeOne.Add(oneTestFixture);
			treeOne.Add(oneTestFixture);

			TestSuite treeTwo = new TestSuite("Test Suite");
			treeTwo.Add(oneTestFixture);

			Assertion.Assert(!UIHelper.CompareTree(treeOne,treeTwo));

		}

		[Test]
		public void CompareStructurallyIdenticalTreesWithDifferentNames()
		{
			TestSuite treeOne = new TestSuite("Test Suite One");
			treeOne.Add(oneTestFixture);

			TestSuite treeTwo = new TestSuite("Test Suite Two");
			treeTwo.Add(oneTestFixture);

			Assertion.Assert(!UIHelper.CompareTree(treeOne,treeTwo));
		}

		[Test]
		public void CompareIdenticalTreesWithOneIgnored()
		{
			TestSuite treeOne = new TestSuite("Test Suite One");
			treeOne.ShouldRun = false;
			treeOne.Add(oneTestFixture);

			TestSuite treeTwo = new TestSuite("Test Suite One");
			treeTwo.Add(oneTestFixture);

			Assertion.Assert(!UIHelper.CompareTree(treeOne,treeTwo));
		}

	}
}
