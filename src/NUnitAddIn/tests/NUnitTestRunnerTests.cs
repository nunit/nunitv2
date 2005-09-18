namespace NUnit.AddInRunner.Tests
{
    using System;
    using System.Text;
    using NUnit.Framework;
    using TestDriven.Framework;
    using System.Reflection;
    using System.Diagnostics;
    using System.Threading;
    using Microsoft.CSharp;

    [TestFixture]
    public class NUnitTestRunnerTests
    {
        [Test]
        public void RunMember_Test()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            MemberInfo member = new ThreadStart(new Examples.MockTestFixture().Test1).Method;
            TestResultSummary summary = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(1, testListener.TestFinishedCount, "Expect 1 test to finnish");
            Assert.IsNotNull(summary, "Check that TestResultSummary was returned");
        }

        [Test]
        public void RunMember_TestFixture()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            MemberInfo member = typeof(Examples.MockTestFixture);
            TestResultSummary summary = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(2, testListener.TestFinishedCount, "expect 2 tests to finnish");
            Assert.IsNotNull(summary, "Check that TestResultSummary was returned");
        }

        [Test]
        public void RunNamespace()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string ns = typeof(Examples.MockTestFixture).Namespace;
            TestResultSummary summary = testRunner.RunNamespace(testListener, assembly, ns);
            Assert.AreEqual(3, testListener.TestFinishedCount, "expect 3 tests to finnish");
            Assert.IsNotNull(summary, "Check that TestResultSummary was returned");
        }

        class MockTestListener : ITestListener
        {
            public int TestFinishedCount;
            public int TestResultsUrlCount;
            public int WriteLineCount;

            public void TestFinished(TestResultSummary summary)
            {
                this.TestFinishedCount++;
            }

            public void TestResultsUrl(string resultsUrl)
            {
                this.TestResultsUrlCount++;
            }

            public void WriteLine(string text, Category category)
            {
                this.WriteLineCount++;
            }
        }
    }

    namespace Examples
    {
        [TestFixture]
        public class MockTestFixture
        {
            [Test]
            public void Test1()
            {
            }

            [Test]
            public void Test2()
            {
            }
        }

        [TestFixture]
        public class MockTestFixture1
        {
            [Test]
            public void Test1()
            {
            }
        }
    }
}
