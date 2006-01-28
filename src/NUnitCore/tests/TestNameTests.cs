using System;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class TestNameTests
	{
		private TestName weakName1;
		private TestName weakName2;
		private TestName strongName1;
		private TestName strongName2;

		[SetUp]
		public void CreateTestNames()
		{
			weakName1 = new TestName();
			weakName2 = new TestName();
			weakName1.FullName = weakName2.FullName = "Name.Of.This.Test";

			strongName1 = new TestName();
			strongName2 = new TestName();
			strongName1.FullName = strongName2.FullName = "Name.Of.This.Test";
			strongName1.TestID = new TestID();
			strongName2.TestID = new TestID();
		}

		[Test]
		public void CanCompareWeakTestNames()
		{
			Assert.AreEqual( weakName1, weakName2 );

			weakName2.FullName = "A.Different.Name";
			Assert.AreNotEqual( weakName1, weakName2 );
		}

		[Test]
		public void CanCompareStrongTestNames()
		{
			Assert.AreNotEqual( strongName1, strongName2 );

			strongName2.TestID = strongName1.TestID;
			Assert.AreEqual( strongName1, strongName2 );

			strongName2.FullName = "A.Different.Name";
			Assert.AreNotEqual( strongName1, strongName2 );
		}

		[Test]
		public void CanCompareWeakAndStrongTestNames()
		{
			Assert.AreNotEqual( weakName1, strongName1 );
		}

		[Test]
		public void TestNamesWithDifferentRunnerIDsAreNotEqual()
		{
			weakName2.RunnerID = 7;
			Assert.AreEqual( 0, weakName1.RunnerID );
			Assert.AreNotEqual( weakName1, weakName2 );

			strongName1.RunnerID = 3;
			strongName2.RunnerID = 5;
			strongName2.TestID = strongName1.TestID;
			Assert.AreNotEqual( strongName1, strongName2 );
		}

		[Test]
		public void ClonedTestNamesAreEqual()
		{
			Assert.AreEqual( weakName1, weakName1.Clone() );
			Assert.AreEqual( strongName1, strongName1.Clone() );
		}

		[Test]
		public void CanDisplayUniqueNames()
		{
			Assert.AreEqual( "[0]Name.Of.This.Test", weakName1.UniqueName );
			Assert.AreEqual( "[0-" + strongName1.TestID.ToString() + "]Name.Of.This.Test", strongName1.UniqueName );
		}

		[Test]
		public void CanParseSimpleTestNames()
		{
			TestName tn = TestName.Parse( "Name.Of.This.Test" );
			Assert.AreEqual( "Name.Of.This.Test", tn.FullName );
		}

		[Test]
		public void CanParseWeakTestNames()
		{
			TestName testName = TestName.Parse( weakName1.UniqueName );
			Assert.AreEqual( weakName1, testName );

			weakName1.RunnerID = 7;
			testName = TestName.Parse( weakName1.UniqueName );
			Assert.AreEqual( weakName1, testName );
		}

		[Test]
		public void CanParseStrongTestNames()
		{
			TestName testName = TestName.Parse( strongName1.UniqueName );
			Assert.AreEqual( strongName1, testName );

			strongName1.RunnerID = 7;
			testName = TestName.Parse( strongName1.UniqueName );
			Assert.AreEqual( strongName1, testName );
		}
	}
}
