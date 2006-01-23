using System;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class TestNameTests
	{
		private TestName testName1;
		private TestName testName2;

		[SetUp]
		public void CreateTestNames()
		{
			testName1 = new TestName();
			testName2 = new TestName();
			testName1.FullName = "Name.Of.This.Test";
			testName2.FullName = "Name.Of.This.Test";
		}

		[Test]
		public void CanCompareWeakTestNames()
		{
			Assert.AreEqual( testName1, testName2 );

			testName2.FullName = "A.Different.Name";
			Assert.AreNotEqual( testName1, testName2 );
		}

		[Test]
		public void CanCompareStrongTestNames()
		{
			testName1.TestID = new TestID();
			Assert.AreNotEqual( testName1, testName2 );

			testName2.TestID = testName1.TestID;
			Assert.AreEqual( testName1, testName2 );

			testName2.TestID = new TestID();
			Assert.AreNotEqual( testName1, testName2 );
		}

		[Test]
		public void ClonedTestNameComparesAsEqual()
		{
			Assert.AreEqual( testName1, testName1.Clone() );
		}
	}
}
