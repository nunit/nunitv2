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
