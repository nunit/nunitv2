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
            TestRunState result = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(1, testListener.TestFinishedCount, "Expect 1 test to finnish");
            Assert.AreEqual(1, testListener.SuccessCount, "Expect 1 test to succeed");
            Assert.AreEqual(result, TestRunState.Success, "Check that tests were executed");
        }

        [Test]
        public void RunMember_AbstractType_Test()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            MemberInfo member = typeof(Examples.AbstractTests);
            TestRunState result = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(4, testListener.TestFinishedCount, "Expect tests to finnish");
            Assert.AreEqual(4, testListener.SuccessCount, "Expect tests to succeed");
            Assert.AreEqual(result, TestRunState.Success, "Check that tests were executed");
        }

        [Test]
        public void RunMember_AbstractMethod_Test()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            MemberInfo member = typeof(Examples.AbstractTests).GetMethod("Test1");
            TestRunState result = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(2, testListener.TestFinishedCount, "Expect tests to finnish");
            Assert.AreEqual(2, testListener.SuccessCount, "Expect tests to succeed");
            Assert.AreEqual(result, TestRunState.Success, "Check that tests were executed");
        }

        [Test]
        public void RunMember_Ignored_Test()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            MemberInfo member = typeof(Examples.IgnoredTests);
            TestRunState result = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(2, testListener.TestFinishedCount, "Expect test to finnish");
            Assert.AreEqual(2, testListener.IgnoredCount, "Expect test to be ignored");
            Assert.AreEqual(result, TestRunState.Success, "Check that tests were executed");
        }

        [Test]
        public void RunMember_NoTest_Test()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            MemberInfo member = new ThreadStart(new Examples.NoTests().NoTest).Method;
            TestRunState result = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(0, testListener.TestFinishedCount, "Expect no tests to finnish");
            Assert.AreEqual(result, TestRunState.NoTests);
        }

        [Test]
        public void RunMember_NotAMethod_Test()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            MemberInfo member = typeof(NUnit.AddInRunner.Tests.Examples.NoTests).GetField("NotAMethod");
            TestRunState result = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(0, testListener.TestFinishedCount, "Expect no tests to finnish");
            Assert.AreEqual(result, TestRunState.NoTests);
        }

        [Test]
        public void RunMember_TestFixture()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            MemberInfo member = typeof(Examples.MockTestFixture);
            TestRunState result = testRunner.RunMember(testListener, assembly, member);
            Assert.AreEqual(2, testListener.TestFinishedCount, "expect 2 tests to finnish");
            Assert.AreEqual(2, testListener.SuccessCount, "Expect 2 tests to succeed");
            Assert.AreEqual(result, TestRunState.Success, "Check that tests were executed");
        }

        [Test]
        public void RunNamespace()
        {
            NUnitTestRunner testRunner = new NUnitTestRunner();
            MockTestListener testListener = new MockTestListener();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string ns = typeof(Examples.MockTestFixture).Namespace;
            TestRunState result = testRunner.RunNamespace(testListener, assembly, ns);
            Assert.AreEqual(9, testListener.TestFinishedCount, "expect tests");
            Assert.AreEqual(7, testListener.SuccessCount, "Expect tests to succeed");
            Assert.AreEqual(2, testListener.IgnoredCount, "Expect tests ignored");
            Assert.AreEqual(result, TestRunState.Success, "Check that tests were executed");
        }

        class MockTestListener : ITestListener
        {
            public int TestFinishedCount;
            public int TestResultsUrlCount;
            public int WriteLineCount;
            public int SuccessCount;
            public int FailureCount;
            public int IgnoredCount;

            public void TestFinished(TestResult summary)
            {
                this.TestFinishedCount++;
                switch (summary.State)
                {
                    case TestState.Passed:
                        this.SuccessCount++;
                        break;
                    case TestState.Failed:
                        this.FailureCount++;
                        break;
                    case TestState.Ignored:
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

        [TestFixture]
        public class IgnoredTests
        {
            [Test, Ignore("Ignore Me")]
            public void IgnoreAttributeTest()
            {
            }

            [Test]
            public void IgnoreAssertTest()
            {
                Assert.Ignore("Ignore Me");
            }
        }

        public class NoTests
        {
            public int NotAMethod;

            public void NoTest()
            {
            }
        }

        [TestFixture]
        public abstract class AbstractTests
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

        public class ConcreateTests1 : AbstractTests
        {
        }

        public class ConcreateTests2 : AbstractTests
        {
        }
    }
}
