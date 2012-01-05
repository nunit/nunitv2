#if CLR_2_0 || CLR_4_0
using System;
using System.Collections;
using System.Collections.Specialized;
using NUnit.Framework;
using NUnit.TestData.ActionAttributeTests;

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
        private readonly string[] _definitionSites = new string[]
        {
            "Assembly",
            "BaseSetUpFixture",
            "SetUpFixture",
            "BaseInterface",
            "BaseFixture",
            "Interface",
            "Fixture",
            "Method"
        };

        [TestFixtureSetUp]
        public void Setup()
        {
            ActionAttributeFixture.Results = new StringCollection();

            TestSuiteBuilder builder = new TestSuiteBuilder();
            TestPackage package = new TestPackage(AssemblyHelper.GetAssemblyPath(typeof(ActionAttributeFixture)));
            package.TestName = typeof(ActionAttributeFixture).Namespace;

            Test suite = builder.Build(package);
            _result = suite.Run(new NullListener(), new ActionAttributeFixtureFilter());
        }

        [Test]
        public void TestsRunsSuccessfully()
        {
            Assert.IsTrue(_result.IsSuccess, "Test run was not successful.");
            Assert.Contains("SomeTest-Case1", ActionAttributeFixture.Results, "Test Case 1 was not run.");
            Assert.Contains("SomeTest-Case2", ActionAttributeFixture.Results, "Test Case 2 was not run.");
            Assert.Contains("SomeOtherTest", ActionAttributeFixture.Results, "SomeOtherTest was not run.");

            foreach(string message in ActionAttributeFixture.Results)
                Console.WriteLine(message);
        }

        [Test]
        public void DefinitionSites_BeforeSuite_ExecuteFirst_InOrder()
        {
            for (int i = 0; i < _definitionSites.Length - 1; i++)
            {
                string prefix = string.Format("{0}.BeforeTestSuite-", _definitionSites[i]);
                
                Assert.IsTrue(
                    ActionAttributeFixture.Results[i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1} in '{2}'", prefix, i, ActionAttributeFixture.Results[i]));
            }
        }


        [Test]
        public void DefinitionSites_AfterSuite_ExecuteLast_InOrder()
        {
            int lastIndex = ActionAttributeFixture.Results.Count - 1;
            for (int i = lastIndex; i > lastIndex - _definitionSites.Length; i--)
            {
                string prefix = string.Format("{0}.AfterTestSuite-", _definitionSites[lastIndex - i]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1} in '{2}'", prefix, i, ActionAttributeFixture.Results[i]));
            }
        }

        [Test]
        public void DefinitionSites_BeforeTest_ExecuteInOrder_ForSomeOtherTest()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeOtherTest") - _definitionSites.Length - 1;
            for (int i = startIndex; i < startIndex; i++)
            {
                string prefix = string.Format("{0}.BeforeTestCase-", _definitionSites[i - startIndex]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1} in '{2}'", prefix, i, ActionAttributeFixture.Results[i]));
            }
        }

        [Test]
        public void DefinitionSites_AfterTest_ExecuteInOrder_ForSomeOtherTest()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeOtherTest");
            for (int i = 1; i <= _definitionSites.Length - 1; i++)
            {
                string prefix = string.Format("{0}.AfterTestCase-", _definitionSites[_definitionSites.Length - 1 - i]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[startIndex + i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1} in '{2}'", prefix, i, ActionAttributeFixture.Results[i]));
            }
        }

        [Test]
        public void AllDefinitionSites_BeforeTest_ExecuteInOrder_ForSomeTestCase1()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeTest-Case1") - _definitionSites.Length;
            for (int i = startIndex; i < startIndex; i++)
            {
                string prefix = string.Format("{0}.BeforeTestCase-", _definitionSites[i - startIndex]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1} in '{2}'", prefix, i, ActionAttributeFixture.Results[i]));
            }
        }

        [Test]
        public void AllDefinitionSites_AfterTest_ExecuteInOrder_ForSomeTestCase1()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeTest-Case1");
            for (int i = 1; i <= _definitionSites.Length; i++)
            {
                string prefix = string.Format("{0}.AfterTestCase-", _definitionSites[_definitionSites.Length - i]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[startIndex + i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1} in '{2}'", prefix, i, ActionAttributeFixture.Results[i]));
            }
        }

        [Test]
        public void AllDefinitionSites_BeforeTest_ExecuteInOrder_ForSomeTestCase2()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeTest-Case2") - _definitionSites.Length;
            for (int i = startIndex; i < startIndex; i++)
            {
                string prefix = string.Format("{0}.BeforeTestCase-", _definitionSites[i - startIndex]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1} in '{2}'", prefix, i, ActionAttributeFixture.Results[i]));
            }
        }

        [Test]
        public void AllDefinitionSites_AfterTest_ExecuteInOrder_ForSomeTestCase2()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeTest-Case2");
            for (int i = 1; i <= _definitionSites.Length; i++)
            {
                string prefix = string.Format("{0}.AfterTestCase-", _definitionSites[_definitionSites.Length - i]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[startIndex + i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1} in '{2}'", prefix, i, ActionAttributeFixture.Results[i]));
            }
        }

        [Test]
        public void MethodDefinedSite_BeforeSuite_BeforeSomeTestCase1()
        {
            int testCase = ActionAttributeFixture.Results.IndexOf("SomeTest-Case1");
            int index = ActionAttributeFixture.Results.IndexOf("Method.BeforeTestSuite-ActionAttributeFixture-SomeTest");
            
            Assert.IsTrue(index >= 0);
            Assert.IsTrue(testCase > index);
        }

        [Test]
        public void MethodDefinedSite_AfterSuite_BeforeSomeTestCase2()
        {
            int testCase = ActionAttributeFixture.Results.IndexOf("SomeTest-Case2");
            int index = ActionAttributeFixture.Results.IndexOf("Method.AfterTestSuite-ActionAttributeFixture-SomeTest");

            Assert.IsTrue(index >= 0);
            Assert.IsTrue(testCase < index);
        }

        [Test]
        public void AllActions_BeforeAndAfterTest_HasAccessToFixture()
        {
            foreach(string message in ActionAttributeFixture.Results)
            {
                if (message.Contains("BeforeTestCase") || message.Contains("AfterTestCase"))
                    Assert.IsTrue(message.Contains(typeof(ActionAttributeFixture).Name), string.Format("'{0}' shows action does not have access to fixture.", message));
            }
        }

        [Test]
        public void AllActions_BeforeAndAfterTest_HasAccessToMethodInfo()
        {
            StringCollection validEndSegments = new StringCollection();
            validEndSegments.AddRange(new string[] {"SomeOtherTest", "SomeTest"});

            foreach (string message in ActionAttributeFixture.Results)
            {
                if (message.Contains("BeforeTestCase") || message.Contains("AfterTestCase"))
                {
                    string endSegment = message.Substring(message.LastIndexOf('-') + 1);

                    Assert.IsTrue(validEndSegments.Contains(endSegment),
                                  string.Format("'{0}' shows action does not have access to method info.", message));
                }
            }
        }
    }
}
#endif