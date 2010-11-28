using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.TestData;
using NUnit.TestUtilities;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class BehaviorAttributeTests
    {
        private TestResult _result = null;

        [SetUp]
        public void Setup()
        {
            BehaviorAttributeFixture.Results = new List<string>();
            _result = TestBuilder.RunTestFixture(typeof(BehaviorAttributeFixture));
        }

        [Test]
        public void TestRunsSuccessfully()
        {
            Assert.IsTrue(_result.IsSuccess);
            Assert.Contains("SomeTest", BehaviorAttributeFixture.Results);
        }

        [Test]
        public void TestLevelBehavior_BeforeTest_InCorrectOrder()
        {
            int priorityOneIndex = BehaviorAttributeFixture.Results.IndexOf("Test.BeforeTest-1");
            int priorityTwoIndex = BehaviorAttributeFixture.Results.IndexOf("Test.BeforeTest-2");
            int priorityThreeIndex = BehaviorAttributeFixture.Results.IndexOf("Test.BeforeTest-3");

            Assert.IsTrue(priorityThreeIndex < priorityTwoIndex, "Priority 3 test-level behavior should run BeforeTest() before test-level behaviors of priority <= 2");
            Assert.IsTrue(priorityTwoIndex < priorityOneIndex, "Priority 2 test-level behavior should run BeforeTest() before test-level behaviors of priority <= 1");
        }

        [Test]
        public void TestLevelBehavior_AfterTest_InCorrectOrder()
        {
            int priorityOneIndex = BehaviorAttributeFixture.Results.IndexOf("Test.AfterTest-1");
            int priorityTwoIndex = BehaviorAttributeFixture.Results.IndexOf("Test.AfterTest-2");
            int priorityThreeIndex = BehaviorAttributeFixture.Results.IndexOf("Test.AfterTest-3");

            Assert.IsTrue(priorityThreeIndex > priorityTwoIndex, "Priority 3 test-level behavior should run AfterTest() after test-level behaviors of priority <= 2");
            Assert.IsTrue(priorityTwoIndex > priorityOneIndex, "Priority 2 test-level behavior should run AfterTest() after test-level behaviors of priority <= 1");
        }

        [Test]
        public void FixtureLevelBehavior_BeforeTest_InCorrectOrder()
        {
            int priorityOneIndex = BehaviorAttributeFixture.Results.IndexOf("Fixture.BeforeTest-1");
            int priorityTwoIndex = BehaviorAttributeFixture.Results.IndexOf("Fixture.BeforeTest-2");
            int priorityThreeIndex = BehaviorAttributeFixture.Results.IndexOf("Fixture.BeforeTest-3");

            Assert.IsTrue(priorityThreeIndex < priorityTwoIndex, "Priority 3 fixture-level behavior should run BeforeTest() before fixture-level behaviors of priority <= 2");
            Assert.IsTrue(priorityTwoIndex < priorityOneIndex, "Priority 2 fixture-level behavior should run BeforeTest() before fixture-level behaviors of priority <= 1");
        }

        [Test]
        public void FixtureLevelBehavior_AfterTest_InCorrectOrder()
        {
            int priorityOneIndex = BehaviorAttributeFixture.Results.IndexOf("Fixture.AfterTest-1");
            int priorityTwoIndex = BehaviorAttributeFixture.Results.IndexOf("Fixture.AfterTest-2");
            int priorityThreeIndex = BehaviorAttributeFixture.Results.IndexOf("Fixture.AfterTest-3");

            Assert.IsTrue(priorityThreeIndex > priorityTwoIndex, "Priority 3 fixture-level test behavior should run AfterTest() after fixture-level behaviors of priority <= 2");
            Assert.IsTrue(priorityTwoIndex > priorityOneIndex, "Priority 2 fixture-level test behavior should run AfterTest() after fixture-level behaviors of priority <= 1");
        }
    }
}
