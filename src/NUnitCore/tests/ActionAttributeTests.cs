using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.TestData;
using NUnit.TestUtilities;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class ActionAttributeTests
    {
        private TestResult _result = null;

        #region  BeforeSuite() & AfterSuite() indicies

        private int _fixtureDefined_BeforeSuite_PriorityOne_Index = -1;
        private int _fixtureDefined_BeforeSuite_PriorityTwo_Index = -1;
        private int _fixtureDefined_BeforeSuite_PriorityThree_Index = -1;

        private int _fixtureDefined_AfterSuite_PriorityOne_Index = -1;
        private int _fixtureDefined_AfterSuite_PriorityTwo_Index = -1;
        private int _fixtureDefined_AfterSuite_PriorityThree_Index = -1;

        private int _interfaceDefined_BeforeSuite_PriorityTwo_Index = -1;
        private int _interfaceDefined_AfterSuite_PriorityTwo_Index = -1;

        #endregion

        #region BeforeTest() & AfterTest() indicies

        private int _fixtureDefined_BeforeTest_PriorityOne_Index = -1;
        private int _fixtureDefined_BeforeTest_PriorityTwo_Index = -1;
        private int _fixtureDefined_BeforeTest_PriorityThree_Index = -1;

        private int _fixtureDefined_AfterTest_PriorityOne_Index = -1;
        private int _fixtureDefined_AfterTest_PriorityTwo_Index = -1;
        private int _fixtureDefined_AfterTest_PriorityThree_Index = -1;

        private int _interfaceDefined_BeforeTest_PriorityTwo_Index = -1;
        private int _interfaceDefined_AfterTest_PriorityTwo_Index = -1;

        private int _testDefined_BeforeTest_PriorityOne_Index = -1;
        private int _testDefined_BeforeTest_PriorityTwo_Index = -1;
        private int _testDefined_BeforeTest_PriorityThree_Index = -1;

        private int _testDefined_AfterTest_PriorityOne_Index = -1;
        private int _testDefined_AfterTest_PriorityTwo_Index = -1;
        private int _testDefined_AfterTest_PriorityThree_Index = -1;

        #endregion

        [SetUp]
        public void Setup()
        {
            _result = TestBuilder.RunTestFixture(typeof(ActionAttributeFixture));

            #region  BeforeSuite() & AfterSuite() indicies

            _fixtureDefined_BeforeSuite_PriorityOne_Index = ActionAttributeFixture.Results.IndexOf("Fixture.BeforeSuite-1");
            _fixtureDefined_BeforeSuite_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Fixture.BeforeSuite-2");
            _fixtureDefined_BeforeSuite_PriorityThree_Index = ActionAttributeFixture.Results.IndexOf("Fixture.BeforeSuite-3");

            _fixtureDefined_AfterSuite_PriorityOne_Index = ActionAttributeFixture.Results.IndexOf("Fixture.AfterSuite-1");
            _fixtureDefined_AfterSuite_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Fixture.AfterSuite-2");
            _fixtureDefined_AfterSuite_PriorityThree_Index = ActionAttributeFixture.Results.IndexOf("Fixture.AfterSuite-3");

            _interfaceDefined_BeforeSuite_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Interface.BeforeSuite-2");
            _interfaceDefined_AfterSuite_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Interface.AfterSuite-2");

            #endregion

            #region update BeforeTest() & AfterTest() indicies

            _fixtureDefined_BeforeTest_PriorityOne_Index = ActionAttributeFixture.Results.IndexOf("Fixture.BeforeTest-1");
            _fixtureDefined_BeforeTest_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Fixture.BeforeTest-2");
            _fixtureDefined_BeforeTest_PriorityThree_Index = ActionAttributeFixture.Results.IndexOf("Fixture.BeforeTest-3");

            _fixtureDefined_AfterTest_PriorityOne_Index = ActionAttributeFixture.Results.IndexOf("Fixture.AfterTest-1");
            _fixtureDefined_AfterTest_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Fixture.AfterTest-2");
            _fixtureDefined_AfterTest_PriorityThree_Index = ActionAttributeFixture.Results.IndexOf("Fixture.AfterTest-3");

            _interfaceDefined_BeforeTest_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Interface.BeforeTest-2");
            _interfaceDefined_AfterTest_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Interface.AfterTest-2");

            _testDefined_BeforeTest_PriorityOne_Index = ActionAttributeFixture.Results.IndexOf("Test.BeforeTest-1");
            _testDefined_BeforeTest_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Test.BeforeTest-2");
            _testDefined_BeforeTest_PriorityThree_Index = ActionAttributeFixture.Results.IndexOf("Test.BeforeTest-3");

            _testDefined_AfterTest_PriorityOne_Index = ActionAttributeFixture.Results.IndexOf("Test.AfterTest-1");
            _testDefined_AfterTest_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Test.AfterTest-2");
            _testDefined_AfterTest_PriorityThree_Index = ActionAttributeFixture.Results.IndexOf("Test.AfterTest-3");

            #endregion
        }

        [Test]
        public void TestRunsSuccessfully()
        {
            Assert.IsTrue(_result.IsSuccess);
            Assert.Contains("SomeTest", ActionAttributeFixture.Results);
        }

        #region Tests for BeforeSuite() and AfterSuite invocation ordering

        [Test]
        public void FixtureDefinedAction_BeforeSuite_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureDefined_BeforeSuite_PriorityThree_Index < _fixtureDefined_BeforeSuite_PriorityTwo_Index, "Priority 3 fixture-defined action should run BeforeSuite() before fixture-defined actions of priority <= 2");
            Assert.IsTrue(_fixtureDefined_BeforeSuite_PriorityTwo_Index < _fixtureDefined_BeforeSuite_PriorityOne_Index, "Priority 2 fixture-defined action should run BeforeSuite() before fixture-defined actions of priority <= 1");
        }

        [Test]
        public void FixtureDefinedAction_AfterSuite_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureDefined_AfterSuite_PriorityThree_Index > _fixtureDefined_AfterSuite_PriorityTwo_Index, "Priority 3 fixture-defined test action should run AfterSuite() after fixture-defined actions of priority <= 2");
            Assert.IsTrue(_fixtureDefined_AfterSuite_PriorityTwo_Index > _fixtureDefined_AfterSuite_PriorityOne_Index, "Priority 2 fixture-defined test action should run AfterSuite() after fixture-defined actions of priority <= 1");
        }

        [Test]
        public void InterfaceLevelAction_BeforeSuite_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureDefined_BeforeSuite_PriorityThree_Index < _interfaceDefined_BeforeSuite_PriorityTwo_Index, "Priority 3 fixture-defined action should run BeforeSuite() before interface-defined actions of priority <= 2");
            Assert.IsTrue(_interfaceDefined_BeforeSuite_PriorityTwo_Index < _fixtureDefined_BeforeSuite_PriorityOne_Index, "Priority 2 interface-defined action should run BeforeSuite() before fixture-defined actions of priority <= 1");
        }

        [Test]
        public void InterfaceLevelAction_AfterSuite_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureDefined_AfterSuite_PriorityThree_Index > _interfaceDefined_AfterSuite_PriorityTwo_Index, "Priority 3 fixture-defined action should run AfterSuite() after interface-defined actions of priority <= 2");
            Assert.IsTrue(_interfaceDefined_AfterSuite_PriorityTwo_Index > _fixtureDefined_AfterSuite_PriorityOne_Index, "Priority 2 interface-defined action should run AfterSuite() after fixture-defined actions of priority <= 1");
        }

        #endregion

        #region Tests for BeforeTest() and AfterTest() invocation ordering

        [Test]
        public void FixtureDefinedAction_BeforeTest_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureDefined_BeforeTest_PriorityThree_Index < _fixtureDefined_BeforeTest_PriorityTwo_Index, "Priority 3 fixture-defined action should run BeforeTest() before fixture-defined actions of priority <= 2");
            Assert.IsTrue(_fixtureDefined_BeforeTest_PriorityTwo_Index < _fixtureDefined_BeforeTest_PriorityOne_Index, "Priority 2 fixture-defined action should run BeforeTest() before fixture-defined actions of priority <= 1");
        }

        [Test]
        public void FixtureDefinedAction_AfterTest_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureDefined_AfterTest_PriorityThree_Index > _fixtureDefined_AfterTest_PriorityTwo_Index, "Priority 3 fixture-defined action should run AfterTest() after fixture-defined actions of priority <= 2");
            Assert.IsTrue(_fixtureDefined_AfterTest_PriorityTwo_Index > _fixtureDefined_AfterTest_PriorityOne_Index, "Priority 2 fixture-defined action should run AfterTest() after fixture-defined actions of priority <= 1");
        }

        [Test]
        public void InterfaceLevelAction_BeforeTest_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureDefined_BeforeTest_PriorityThree_Index < _interfaceDefined_BeforeTest_PriorityTwo_Index, "Priority 3 fixture-defined action should run BeforeTest() before interface-defined actions of priority <= 2");
            Assert.IsTrue(_interfaceDefined_BeforeTest_PriorityTwo_Index < _fixtureDefined_BeforeTest_PriorityOne_Index, "Priority 2 interface-defined action should run BeforeTest() before fixture-defined actions of priority <= 1");
        }

        [Test]
        public void InterfaceLevelAction_AfterTest_InCorrectOrder()
        {
            Assert.IsTrue(_fixtureDefined_AfterTest_PriorityThree_Index > _interfaceDefined_AfterTest_PriorityTwo_Index, "Priority 3 fixture-defined action should run AfterTest() after interface-defined actions of priority <= 2");
            Assert.IsTrue(_interfaceDefined_AfterTest_PriorityTwo_Index > _fixtureDefined_AfterTest_PriorityOne_Index, "Priority 2 interface-defined action should run AfterTest() after fixture-defined actions of priority <= 1");
        }

        [Test]
        public void TestLevelAction_BeforeTest_InCorrectOrder()
        {
            Assert.IsTrue(_testDefined_BeforeTest_PriorityThree_Index < _testDefined_BeforeTest_PriorityTwo_Index, "Priority 3 test-defined action should run BeforeTest() before test-defined actions of priority <= 2");
            Assert.IsTrue(_testDefined_BeforeTest_PriorityTwo_Index < _testDefined_BeforeTest_PriorityOne_Index, "Priority 2 test-defined action should run BeforeTest() before test-defined actions of priority <= 1");
        }

        [Test]
        public void TestLevelAction_AfterTest_InCorrectOrder()
        {
            Assert.IsTrue(_testDefined_AfterTest_PriorityThree_Index > _testDefined_AfterTest_PriorityTwo_Index, "Priority 3 test-defined action should run AfterTest() after test-defined actions of priority <= 2");
            Assert.IsTrue(_testDefined_AfterTest_PriorityTwo_Index > _testDefined_AfterTest_PriorityOne_Index, "Priority 2 test-defined action should run AfterTest() after test-defined actions of priority <= 1");
        }

        #endregion
    }
}
