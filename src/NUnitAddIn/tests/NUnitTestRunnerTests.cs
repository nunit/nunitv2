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
            TestRunResult result = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(1, testListener.TestFinishedCount, "Expect 1 test to finnish");
            Assert.AreEqual(1, testListener.SuccessCount, "Expect 1 test to succeed");
            Assert.AreEqual(result, TestRunResult.Success, "Check that tests were executed");
        }

        [Test]
        public void RunMember_NoTest_Test()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            MemberInfo member = new ThreadStart(new Examples.NoTests().NoTest).Method;
            TestRunResult result = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(0, testListener.TestFinishedCount, "Expect no tests to finnish");
            Assert.AreEqual(result, TestRunResult.NoTests);
        }

        [Test]
        public void RunMember_NotAMethod_Test()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            MemberInfo member = typeof(NUnit.AddInRunner.Tests.Examples.NoTests).GetField("NotAMethod");
            TestRunResult result = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(0, testListener.TestFinishedCount, "Expect no tests to finnish");
            Assert.AreEqual(result, TestRunResult.NoTests);
        }

        [Test]
        public void RunMember_TestFixture()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            MemberInfo member = typeof(Examples.MockTestFixture);
            TestRunResult result = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(2, testListener.TestFinishedCount, "expect 2 tests to finnish");
            Assert.AreEqual(2, testListener.SuccessCount, "Expect 2 tests to succeed");
            Assert.AreEqual(result, TestRunResult.Success, "Check that tests were executed");
        }

        [Test]
        public void RunNamespace()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string ns = typeof(Examples.MockTestFixture).Namespace;
            TestRunResult result = testRunner.RunNamespace(testListener, assembly, ns);
            Assert.AreEqual(3, testListener.TestFinishedCount, "expect 3 tests to finnish");
            Assert.AreEqual(3, testListener.SuccessCount, "Expect 3 tests to succeed");
            Assert.AreEqual(result, TestRunResult.Success, "Check that tests were executed");
        }

        class MockTestListener : ITestListener
        {
            public int TestFinishedCount;
            public int TestResultsUrlCount;
            public int WriteLineCount;
            public int SuccessCount;
            public int FailureCount;
            public int IgnoredCount;

            public void TestFinished(TestResultSummary summary)
            {
                this.TestFinishedCount++;
                switch (summary.Result)
                {
                    case TestResult.Success:
                        this.SuccessCount++;
                        break;
                    case TestResult.Failure:
                        this.FailureCount++;
                        break;
                    case TestResult.Ignored:
                        this.IgnoredCount++;
                        break;
                }
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

        public class NoTests
        {
            public int NotAMethod;

            public void NoTest()
            {
            }
        }
    }
}
