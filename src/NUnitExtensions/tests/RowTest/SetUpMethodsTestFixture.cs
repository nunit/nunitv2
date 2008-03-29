// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using NUnit.Framework;
using NUnit.Framework.Extensions;

namespace NUnit.Core.Extensions.RowTest
{
	[TestFixture]
	public class SetUpMethodsTestFixture
	{
		private bool testFixtureSetUpHasRun = false;
		private bool setUpHasRun = false;
        private string setUpByTestFixtureSetUp;
        private string setUpByTestSetUp;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			setUpByTestFixtureSetUp = "So Long,";
			testFixtureSetUpHasRun = true;
		}
		
		[SetUp]
		public void SetUp()
		{
			setUpByTestSetUp = "and Thanks for All the Fish";
			setUpHasRun = true;
		}
		
		[RowTest]
		[Row("So Long,", "and Thanks for All the Fish")]
		public void ClassMemberTest (string string1, string string2)
		{
			Assert.IsTrue(testFixtureSetUpHasRun);
			Assert.IsTrue(setUpHasRun);
			Assert.AreEqual(string1, setUpByTestFixtureSetUp);
			Assert.AreEqual(string2, setUpByTestSetUp);
		}

        [Test]
        public void PrivateClassMemberTest()
        {
            Assert.AreEqual(setUpByTestFixtureSetUp, "So Long,");
            Assert.AreEqual(setUpByTestSetUp, "and Thanks for All the Fish");
        }
	}
}
