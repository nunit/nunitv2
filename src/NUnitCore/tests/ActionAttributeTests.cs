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
            "SetUpFixture",
            "Fixture",
            "Interface",
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
        public void FirstFourDefinitionSites_BeforeSuite_ExecuteFirst_InOrder()
        {
            for(int i = 0; i < 4; i++)
            {
                string prefix = string.Format("{0}.BeforeTestSuite-", _definitionSites[i]);
                
                Assert.IsTrue(
                    ActionAttributeFixture.Results[i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1}", prefix, i));
            }
        }


        [Test]
        public void FirstFourDefinitionSites_AfterSuite_ExecuteLast_InOrder()
        {
            int lastIndex = ActionAttributeFixture.Results.Count - 1;
            for (int i = lastIndex; i > lastIndex - 4; i--)
            {
                string prefix = string.Format("{0}.AfterTestSuite-", _definitionSites[lastIndex - i]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1}", prefix, i));
            }
        }

        [Test]
        public void FirstFourDefinitionSites_BeforeTest_ExecuteInOrder_ForSomeOtherTest()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeOtherTest") - 4;
            for (int i = startIndex; i < startIndex; i++)
            {
                string prefix = string.Format("{0}.BeforeTestCase-", _definitionSites[i - startIndex]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1}", prefix, i));
            }
        }

        [Test]
        public void FirstFourDefinitionSites_AfterTest_ExecuteInOrder_ForSomeOtherTest()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeOtherTest");
            for (int i = 1; i <= 4; i++)
            {
                string prefix = string.Format("{0}.AfterTestCase-", _definitionSites[4 - i]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[startIndex + i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1}", prefix, i));
            }
        }

        [Test]
        public void AllDefinitionSites_BeforeTest_ExecuteInOrder_ForSomeTestCase1()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeTest-Case1") - 5;
            for (int i = startIndex; i < startIndex; i++)
            {
                string prefix = string.Format("{0}.BeforeTestCase-", _definitionSites[i - startIndex]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1}", prefix, i));
            }
        }

        [Test]
        public void AllDefinitionSites_AfterTest_ExecuteInOrder_ForSomeTestCase1()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeTest-Case1");
            for (int i = 1; i <= 5; i++)
            {
                string prefix = string.Format("{0}.AfterTestCase-", _definitionSites[5 - i]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[startIndex + i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1}", prefix, i));
            }
        }

        [Test]
        public void AllDefinitionSites_BeforeTest_ExecuteInOrder_ForSomeTestCase2()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeTest-Case2") - 5;
            for (int i = startIndex; i < startIndex; i++)
            {
                string prefix = string.Format("{0}.BeforeTestCase-", _definitionSites[i - startIndex]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1}", prefix, i));
            }
        }

        [Test]
        public void AllDefinitionSites_AfterTest_ExecuteInOrder_ForSomeTestCase2()
        {
            int startIndex = ActionAttributeFixture.Results.IndexOf("SomeTest-Case2");
            for (int i = 1; i <= 5; i++)
            {
                string prefix = string.Format("{0}.AfterTestCase-", _definitionSites[5 - i]);

                Assert.IsTrue(
                    ActionAttributeFixture.Results[startIndex + i].StartsWith(prefix),
                    string.Format("Did not find prefix '{0}' at index {1}", prefix, i));
            }
        }

        [Test]
        public void MethodDefinedSite_BeforeSuite_BeforeSomeTestCase1()
        {
            int testCase = ActionAttributeFixture.Results.IndexOf("SomeTest-Case1");
            Assert.IsTrue(testCase > ActionAttributeFixture.Results.IndexOf("Method.BeforeTestSuite-ActionAttributeFixture"));
        }

        [Test]
        public void MethodDefinedSite_AfterSuite_BeforeSomeTestCase2()
        {
            int testCase = ActionAttributeFixture.Results.IndexOf("SomeTest-Case2");
            Assert.IsTrue(testCase < ActionAttributeFixture.Results.IndexOf("Method.AfterTestSuite-ActionAttributeFixture"));
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
