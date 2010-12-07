using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using NUnit.Framework;
using NUnit.TestData.ActionAttributeTests;
using NUnit.TestUtilities;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class ActionAttributeTests
    {
        private class ActionAttributeFixtureFilter : TestFilter
        {
            public override bool Match(ITest test)
            {
                return test.TestName.FullName.StartsWith(typeof(ActionAttributeFixture).FullName);
            }
        }

        private TestResult _result = null;

        #region Define BeforeSuite() & AfterSuite() indicies

        private int _setupFixtureDefined_BeforeSuite_PriorityFourIndex = -1;
        private int _setupFixtureDefined_AfterSuite_PriorityFourIndex = -1;

        private int _fixtureDefined_BeforeSuite_PriorityOne_Index = -1;
        private int _fixtureDefined_BeforeSuite_PriorityTwo_Index = -1;
        private int _fixtureDefined_BeforeSuite_PriorityThree_Index = -1;

        private int _fixtureDefined_AfterSuite_PriorityOne_Index = -1;
        private int _fixtureDefined_AfterSuite_PriorityTwo_Index = -1;
        private int _fixtureDefined_AfterSuite_PriorityThree_Index = -1;

        private int _interfaceDefined_BeforeSuite_PriorityTwo_Index = -1;
        private int _interfaceDefined_AfterSuite_PriorityTwo_Index = -1;

        #endregion

        #region Define BeforeTest() & AfterTest() indicies

        private int _setupFixtureDefined_BeforeTest_PriorityFourIndex = -1;
        private int _setupFixtureDefined_AfterTest_PriorityFourIndex = -1;

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

        [TestFixtureSetUp]
        public void Setup()
        {
            ActionAttributeFixture.Results = new StringCollection();

            TestSuiteBuilder builder = new TestSuiteBuilder();
            TestPackage package = new TestPackage(AssemblyHelper.GetAssemblyPath(typeof(ActionAttributeFixture)));
            package.TestName = typeof(ActionAttributeFixture).Namespace;

            Test suite = builder.Build(package);
            _result = suite.Run(new NullListener(), new ActionAttributeFixtureFilter());

            #region Update BeforeSuite() & AfterSuite() indicies

            _setupFixtureDefined_BeforeSuite_PriorityFourIndex = ActionAttributeFixture.Results.IndexOf("SetUpFixture.BeforeSuite-4");
            _setupFixtureDefined_AfterSuite_PriorityFourIndex = ActionAttributeFixture.Results.IndexOf("SetUpFixture.AfterSuite-4");

            _fixtureDefined_BeforeSuite_PriorityOne_Index = ActionAttributeFixture.Results.IndexOf("Fixture.BeforeSuite-1");
            _fixtureDefined_BeforeSuite_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Fixture.BeforeSuite-2");
            _fixtureDefined_BeforeSuite_PriorityThree_Index = ActionAttributeFixture.Results.IndexOf("Fixture.BeforeSuite-3");

            _fixtureDefined_AfterSuite_PriorityOne_Index = ActionAttributeFixture.Results.IndexOf("Fixture.AfterSuite-1");
            _fixtureDefined_AfterSuite_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Fixture.AfterSuite-2");
            _fixtureDefined_AfterSuite_PriorityThree_Index = ActionAttributeFixture.Results.IndexOf("Fixture.AfterSuite-3");

            _interfaceDefined_BeforeSuite_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Interface.BeforeSuite-2");
            _interfaceDefined_AfterSuite_PriorityTwo_Index = ActionAttributeFixture.Results.IndexOf("Interface.AfterSuite-2");

            #endregion

            #region Update BeforeTest() & AfterTest() indicies

            _setupFixtureDefined_BeforeTest_PriorityFourIndex = ActionAttributeFixture.Results.IndexOf("SetUpFixture.BeforeTest-4");
            _setupFixtureDefined_AfterTest_PriorityFourIndex = ActionAttributeFixture.Results.IndexOf("SetUpFixture.AfterTest-4");

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

        [Test]
        public void SetUpFixtureDefinedAction_WrapsEntireSuite()
        {
            Assert.IsTrue(_setupFixtureDefined_BeforeSuite_PriorityFourIndex == 0, "Highest priority setup-fixture-defined action should run first.");
            Assert.IsTrue(_setupFixtureDefined_AfterSuite_PriorityFourIndex == ActionAttributeFixture.Results.Count - 1, "Highest priority setup-fixture-defined action should run AfterSuite() last.");
        }

        [Test]
        public void SetUpFixtureDefinedAction_BeforeTest_InCorrectOrder()
        {
            Assert.IsTrue(_setupFixtureDefined_BeforeTest_PriorityFourIndex < _fixtureDefined_BeforeTest_PriorityThree_Index);
        }

        [Test]
        public void SetUpFixtureDefinedAction_AfterTest_InCorrectOrder()
        {
            Assert.IsTrue(_setupFixtureDefined_AfterTest_PriorityFourIndex > _fixtureDefined_AfterTest_PriorityThree_Index);
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
