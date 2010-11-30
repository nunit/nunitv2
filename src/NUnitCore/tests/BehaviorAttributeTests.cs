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

        #region  BeforeTestFixture() & AfterTestFixture() indicies

        private int _fixtureLevel_BeforeTestFixture_PriorityOne_Index = -1;
        private int _fixtureLevel_BeforeTestFixture_PriorityTwo_Index = -1;
        private int _fixtureLevel_BeforeTestFixture_PriorityThree_Index = -1;

        private int _fixtureLevel_AfterTestFixture_PriorityOne_Index = -1;
        private int _fixtureLevel_AfterTestFixture_PriorityTwo_Index = -1;
        private int _fixtureLevel_AfterTestFixture_PriorityThree_Index = -1;

        private int _interfaceLevel_BeforeTestFixture_PriorityTwo_Index = -1;
        private int _interfaceLevel_AfterTestFixture_PriorityTwo_Index = -1;

        #endregion

        #region BeforeTest() & AfterTest() indicies

        private int _fixtureLevel_BeforeTest_PriorityOne_Index = -1;
        private int _fixtureLevel_BeforeTest_PriorityTwo_Index = -1;
        private int _fixtureLevel_BeforeTest_PriorityThree_Index = -1;

        private int _fixtureLevel_AfterTest_PriorityOne_Index = -1;
        private int _fixtureLevel_AfterTest_PriorityTwo_Index = -1;
        private int _fixtureLevel_AfterTest_PriorityThree_Index = -1;

        private int _interfaceLevel_BeforeTest_PriorityTwo_Index = -1;
        private int _interfaceLevel_AfterTest_PriorityTwo_Index = -1;

        private int _testLevel_BeforeTest_PriorityOne_Index = -1;
        private int _testLevel_BeforeTest_PriorityTwo_Index = -1;
        private int _testLevel_BeforeTest_PriorityThree_Index = -1;

        private int _testLevel_AfterTest_PriorityOne_Index = -1;
        private int _testLevel_AfterTest_PriorityTwo_Index = -1;
        private int _testLevel_AfterTest_PriorityThree_Index = -1;

        #endregion

        [SetUp]
        public void Setup()
        {
            _result = TestBuilder.RunTestFixture(typeof(BehaviorAttributeFixture));

            #region  BeforeTestFixture() & AfterTestFixture() indicies

            _fixtureLevel_BeforeTestFixture_PriorityOne_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.BeforeTestFixture-1");
            _fixtureLevel_BeforeTestFixture_PriorityTwo_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.BeforeTestFixture-2");
            _fixtureLevel_BeforeTestFixture_PriorityThree_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.BeforeTestFixture-3");

            _fixtureLevel_AfterTestFixture_PriorityOne_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.AfterTestFixture-1");
            _fixtureLevel_AfterTestFixture_PriorityTwo_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.AfterTestFixture-2");
            _fixtureLevel_AfterTestFixture_PriorityThree_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.AfterTestFixture-3");

            _interfaceLevel_BeforeTestFixture_PriorityTwo_Index = BehaviorAttributeFixture.Results.IndexOf("Interface.BeforeTestFixture-2");
            _interfaceLevel_AfterTestFixture_PriorityTwo_Index = BehaviorAttributeFixture.Results.IndexOf("Interface.AfterTestFixture-2");

            #endregion

            #region update BeforeTest() & AfterTest() indicies

            _fixtureLevel_BeforeTest_PriorityOne_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.BeforeTest-1");
            _fixtureLevel_BeforeTest_PriorityTwo_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.BeforeTest-2");
            _fixtureLevel_BeforeTest_PriorityThree_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.BeforeTest-3");

            _fixtureLevel_AfterTest_PriorityOne_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.AfterTest-1");
            _fixtureLevel_AfterTest_PriorityTwo_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.AfterTest-2");
            _fixtureLevel_AfterTest_PriorityThree_Index = BehaviorAttributeFixture.Results.IndexOf("Fixture.AfterTest-3");

            _interfaceLevel_BeforeTest_PriorityTwo_Index = BehaviorAttributeFixture.Results.IndexOf("Interface.BeforeTest-2");
            _interfaceLevel_AfterTest_PriorityTwo_Index = BehaviorAttributeFixture.Results.IndexOf("Interface.AfterTest-2");

            _testLevel_BeforeTest_PriorityOne_Index = BehaviorAttributeFixture.Results.IndexOf("Test.BeforeTest-1");
            _testLevel_BeforeTest_PriorityTwo_Index = BehaviorAttributeFixture.Results.IndexOf("Test.BeforeTest-2");
            _testLevel_BeforeTest_PriorityThree_Index = BehaviorAttributeFixture.Results.IndexOf("Test.BeforeTest-3");

            _testLevel_AfterTest_PriorityOne_Index = BehaviorAttributeFixture.Results.IndexOf("Test.AfterTest-1");
            _testLevel_AfterTest_PriorityTwo_Index = BehaviorAttributeFixture.Results.IndexOf("Test.AfterTest-2");
            _testLevel_AfterTest_PriorityThree_Index = BehaviorAttributeFixture.Results.IndexOf("Test.AfterTest-3");

            #endregion
        }

        [Test]
        public void TestRunsSuccessfully()
        {
            Assert.IsTrue(_result.IsSuccess);
            Assert.Contains("SomeTest", BehaviorAttributeFixture.Results);
        }

        #region Tests for BeforeTestFixture() and AfterTestFixture invocation ordering

        [Test]
        public void FixtureLevelBehavior_BeforeTestFixture_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureLevel_BeforeTestFixture_PriorityThree_Index < _fixtureLevel_BeforeTestFixture_PriorityTwo_Index, "Priority 3 fixture-level behavior should run BeforeTestFixture() before fixture-level behaviors of priority <= 2");
            Assert.IsTrue(_fixtureLevel_BeforeTestFixture_PriorityTwo_Index < _fixtureLevel_BeforeTestFixture_PriorityOne_Index, "Priority 2 fixture-level behavior should run BeforeTestFixture() before fixture-level behaviors of priority <= 1");
        }

        [Test]
        public void FixtureLevelBehavior_AfterTestFixture_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureLevel_AfterTestFixture_PriorityThree_Index > _fixtureLevel_AfterTestFixture_PriorityTwo_Index, "Priority 3 fixture-level test behavior should run AfterTestFixture() after fixture-level behaviors of priority <= 2");
            Assert.IsTrue(_fixtureLevel_AfterTestFixture_PriorityTwo_Index > _fixtureLevel_AfterTestFixture_PriorityOne_Index, "Priority 2 fixture-level test behavior should run AfterTestFixture() after fixture-level behaviors of priority <= 1");
        }

        [Test]
        public void InterfaceLevelBehavior_BeforeTestFixture_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureLevel_BeforeTestFixture_PriorityThree_Index < _interfaceLevel_BeforeTestFixture_PriorityTwo_Index, "Priority 3 fixture-level behavior should run BeforeTestFixture() before interface-level behaviors of priority <= 2");
            Assert.IsTrue(_interfaceLevel_BeforeTestFixture_PriorityTwo_Index < _fixtureLevel_BeforeTestFixture_PriorityOne_Index, "Priority 2 interface-level behavior should run BeforeTestFixture() before fixture-level behaviors of priority <= 1");
        }

        [Test]
        public void InterfaceLevelBehavior_AfterTestFixture_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureLevel_AfterTestFixture_PriorityThree_Index > _interfaceLevel_AfterTestFixture_PriorityTwo_Index, "Priority 3 fixture-level test behavior should run AfterTestFixture() after interface-level behaviors of priority <= 2");
            Assert.IsTrue(_interfaceLevel_AfterTestFixture_PriorityTwo_Index > _fixtureLevel_AfterTestFixture_PriorityOne_Index, "Priority 2 interface-level test behavior should run AfterTestFixture() after fixture-level behaviors of priority <= 1");
        }

        #endregion

        #region Tests for BeforeTest() and AfterTest() invocation ordering

        [Test]
        public void FixtureLevelBehavior_BeforeTest_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureLevel_BeforeTest_PriorityThree_Index < _fixtureLevel_BeforeTest_PriorityTwo_Index, "Priority 3 fixture-level behavior should run BeforeTest() before fixture-level behaviors of priority <= 2");
            Assert.IsTrue(_fixtureLevel_BeforeTest_PriorityTwo_Index < _fixtureLevel_BeforeTest_PriorityOne_Index, "Priority 2 fixture-level behavior should run BeforeTest() before fixture-level behaviors of priority <= 1");
        }

        [Test]
        public void FixtureLevelBehavior_AfterTest_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureLevel_AfterTest_PriorityThree_Index > _fixtureLevel_AfterTest_PriorityTwo_Index, "Priority 3 fixture-level test behavior should run AfterTest() after fixture-level behaviors of priority <= 2");
            Assert.IsTrue(_fixtureLevel_AfterTest_PriorityTwo_Index > _fixtureLevel_AfterTest_PriorityOne_Index, "Priority 2 fixture-level test behavior should run AfterTest() after fixture-level behaviors of priority <= 1");
        }

        [Test]
        public void InterfaceLevelBehavior_BeforeTest_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureLevel_BeforeTest_PriorityThree_Index < _interfaceLevel_BeforeTest_PriorityTwo_Index, "Priority 3 fixture-level behavior should run BeforeTest() before interface-level behaviors of priority <= 2");
            Assert.IsTrue(_interfaceLevel_BeforeTest_PriorityTwo_Index < _fixtureLevel_BeforeTest_PriorityOne_Index, "Priority 2 interface-level behavior should run BeforeTest() before fixture-level behaviors of priority <= 1");
        }

        [Test]
        public void InterfaceLevelBehavior_AfterTest_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureLevel_AfterTest_PriorityThree_Index > _interfaceLevel_AfterTest_PriorityTwo_Index, "Priority 3 fixture-level test behavior should run AfterTest() after interface-level behaviors of priority <= 2");
            Assert.IsTrue(_interfaceLevel_AfterTest_PriorityTwo_Index > _fixtureLevel_AfterTest_PriorityOne_Index, "Priority 2 interface-level test behavior should run AfterTest() after fixture-level behaviors of priority <= 1");
        }

        [Test]
        public void TestLevelBehavior_BeforeTest_InCorrectOrder()
        {
            Assert.IsTrue(_testLevel_BeforeTest_PriorityThree_Index < _testLevel_BeforeTest_PriorityTwo_Index, "Priority 3 test-level behavior should run BeforeTest() before test-level behaviors of priority <= 2");
            Assert.IsTrue(_testLevel_BeforeTest_PriorityTwo_Index < _testLevel_BeforeTest_PriorityOne_Index, "Priority 2 test-level behavior should run BeforeTest() before test-level behaviors of priority <= 1");
        }

        [Test]
        public void TestLevelBehavior_AfterTest_InCorrectOrder()
        {
            Assert.IsTrue(_testLevel_AfterTest_PriorityThree_Index > _testLevel_AfterTest_PriorityTwo_Index, "Priority 3 test-level behavior should run AfterTest() after test-level behaviors of priority <= 2");
            Assert.IsTrue(_testLevel_AfterTest_PriorityTwo_Index > _testLevel_AfterTest_PriorityOne_Index, "Priority 2 test-level behavior should run AfterTest() after test-level behaviors of priority <= 1");
        }

        #endregion
    }
}
