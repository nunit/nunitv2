using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Tests.Assemblies;
using NUnit.Core;

namespace NUnit.Tests.Core
{
	[TestFixture]
	public class NameFilterTest
	{
		private MockTestFixture mockTestFixture;
		private TestSuite testSuite;
		private NUnit.Core.TestCase mock3;

		[SetUp]
		public void SetUp() 
		{
			mockTestFixture = new MockTestFixture();
			testSuite = new TestSuite("Mock Test Suite");
			testSuite.Add(mockTestFixture);
			mock3 = (NUnit.Core.TestCase) findTest("MockTest3", testSuite);
		}

		[Test]
		public void SingleNameMatch() 
		{
			string uniqueName = "[0]NUnit.Tests.Assemblies.MockTestFixture.MockTest3";
			Assert.AreEqual(uniqueName, mock3.UniqueName);
			NameFilter filter = new NameFilter(mock3);
			Assert.IsTrue(filter.Pass(mock3), "Name Filter did not pass test case");
			Assert.AreEqual("[0]NUnit.Tests.Assemblies.MockTestFixture", ((TestSuite)testSuite.Tests[0]).UniqueName);
			Assert.IsTrue(filter.Pass((TestSuite)testSuite.Tests[0]), "Name Filter did not pass test suite");
		}

		[Test]
		public void MultipleNameMatch() 
		{
			NUnit.Core.TestCase mock1 = (NUnit.Core.TestCase) findTest("MockTest1", testSuite);
			ArrayList testList = new ArrayList();
			testList.Add(mock3);
			testList.Add(mock1);
			NameFilter filter = new NameFilter(testList);
			Assert.IsTrue(filter.Pass(mock3), "Name Filter did not pass test case");
			Assert.IsTrue(filter.Pass(mock1), "Name Filter did not pass test case");
			Assert.IsTrue(filter.Pass((TestSuite)testSuite.Tests[0]), "Name Filter did not pass test suite");
		}

		[Test]
		public void SuiteNameMatch() 
		{
			NUnit.Core.TestSuite mockTest = (NUnit.Core.TestSuite) findTest("MockTestFixture", testSuite);
			NameFilter filter = new NameFilter(mockTest);
			Assert.IsTrue(filter.Pass(mock3), "Name Filter did not pass test case");
			Assert.IsTrue(filter.Pass(mockTest), "Suite did not pass test case");
			Assert.IsTrue(filter.Pass(testSuite), "Suite did not pass test case");
		}

		[Test]
		public void TestDoesNotMatch() 
		{
			NUnit.Core.TestCase mock1 = (NUnit.Core.TestCase) findTest("MockTest1", testSuite);
			NameFilter filter = new NameFilter(mock1);
			Assert.IsFalse(filter.Pass(mock3), "Name Filter did pass test case");
			Assert.IsTrue(filter.Pass(testSuite), "Name Filter did not pass test suite");
		}

		[Test]
		public void HighLevelSuite() 
		{
			NUnit.Core.TestSuite mockTest = (NUnit.Core.TestSuite) findTest("MockTestFixture", testSuite);
			NameFilter filter = new NameFilter(testSuite);
			Assert.IsTrue(filter.Pass(mock3), "Name Filter did not pass test case");
			Assert.IsTrue(filter.Pass(mockTest), "Name Filter did not pass middle suite");
			Assert.IsTrue(filter.Pass(testSuite), "Name Filter did not pass test suite");
		}

		private Test findTest(string name, Test test) 
		{
			Test result = null;
			if (test.Name == name)
				result = test;
			else if (test.Tests != null)
			{
				foreach(Test t in test.Tests) 
				{
					result = findTest(name, t);
					if (result != null)
						break;
				}
			}

			return result;
		}
	}
}