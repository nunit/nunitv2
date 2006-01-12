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
		private TestID testID1;
		private TestID testID2;
		private TestID testID3;

		[SetUp]
		public void ConstructTestIDs()
		{
			testID1 = new TestID();
			testID2 = new TestID( 5 );
			testID3 = new TestID( testID2 );
		}

		[Test]
		public void CanConstruct()
		{
			Assert.AreNotEqual( testID1.TestKey, testID2.TestKey );
			Assert.AreEqual( 0, testID1.RunnerID );
			Assert.AreEqual( 5, testID2.RunnerID );
		}

		[Test]
		public void CanCompareForEquality()
		{
			Assert.AreNotEqual( testID1, testID2 );
			Assert.AreEqual( testID2, testID3 );
		}

		[Test]
		public void CanAssignRunnerID()
		{
			testID3.RunnerID = 7;
			Assert.AreEqual( 7, testID3.RunnerID );
			Assert.AreNotEqual( testID2, testID3 );
		}
	}
}
