// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************
using System;
using NUnit.Framework;

namespace NUnit.TestData
{
    [TestFixture]
    public class ThreadingFixture
    {
        public static bool TearDownWasRun;

        [SetUp]
        public void SetUp()
        {
            TearDownWasRun = false;
        }

        [TearDown]
        public void TearDown()
        {
            TearDownWasRun = true;
        }

        [Test, Timeout(50)]
        public void InfiniteLoopWith50msTimeout()
        {
            TearDownWasRun = false;
            while (true) { }
        }
		
		[Test, RequiresThread]
		public void MethodWithThreeAsserts()
		{
			Assert.True(true);
			Assert.True(true);
			Assert.True(true);
		}
    }

    [TestFixture, Timeout(50)]
    public class ThreadingFixtureWithTimeout
    {
        [Test]
        public void Test1() { }
        [Test]
        public void Test2WithInfiniteLoop() { while (true) { } }
        [Test]
        public void Test3() { }
    }
}
