using System;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for TestIDTests.
	/// </summary>
	[TestFixture]
	public class TestIDTests
	{
		[Test]
		public void ClonedTestIDsAreEqual()
		{
			TestID testID = new TestID();
			Assert.AreEqual( testID, testID.Clone() );
		}

		[Test]
		public void DifferentTestIDsAreNotEqual()
		{
			TestID testID1 = new TestID();
			TestID testID2 = new TestID();
			Assert.AreNotEqual( testID1, testID2 );
		}

		[Test]
		public void TestIDsWithDifferentRunnersAreNotEqual()
		{
			TestID testID1 = new TestID();
			TestID testID2 = (TestID)testID1.Clone();
			testID1.RunnerID = 5;
			testID2.RunnerID = 7;
			Assert.AreNotEqual( testID1, testID2 );
		}
	}
}
