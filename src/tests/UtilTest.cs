//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
//
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

	}
}
