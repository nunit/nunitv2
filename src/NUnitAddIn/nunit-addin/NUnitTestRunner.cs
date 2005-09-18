namespace NUnit.AddInRunner
{
    using System;
    using System.Text;
    using System.Reflection;
    using TestDriven.Framework;
    using NUnit.Core;
    using System.Diagnostics;

    public class NUnitTestRunner : ITestRunner
    {
        TestSuiteBuilder testSuiteBuilder = new TestSuiteBuilder();

        public TestResultSummary RunAssembly(ITestListener testListener, Assembly assembly)
        {
            IFilter filter = new NoFilter();
            return run(testListener, assembly, filter);
        }

        public TestResultSummary RunMember(ITestListener testListener, Assembly assembly, MemberInfo member)
        {
            if (member is Type)
            {
                Type type = member as Type;
                string fixtureName = type.FullName;
                Filter filter = new Filter(fixtureName);
                return run(testListener, assembly, filter);
            }
            else if (member is MethodInfo)
            {
                MethodInfo method = member as MethodInfo;
                string testName = method.DeclaringType.FullName + "." + method.Name;
                Filter filter = new Filter(testName);
                return run(testListener, assembly, filter);
            }
            else
            {
                return null;
            }
        }

        public TestResultSummary RunNamespace(ITestListener testListener, Assembly assembly, string ns)
        {
            Filter filter = new Filter(ns);
            return run(testListener, assembly, filter);
        }

        TestResultSummary run(ITestListener testListener, Assembly assembly, IFilter filter)
        {
            TestSuite testSuite = this.testSuiteBuilder.Build(assembly.FullName);
            EventListener listener = new ProxyEventListener(testListener);
            TestResult result = testSuite.Run(listener, filter);
            TestResultSummary summary = new TestResultSummary();
            return summary;
        }

        class NoFilter : IFilter
        {
            public bool Pass(TestSuite suite)
            {
                return true;
            }

            public bool Pass(TestCase test)
            {
                return true;
            }
        }

        class Filter : IFilter
        {
            string name;

            public Filter(string name)
            {
                this.name = name;
            }

            public bool Pass(TestSuite suite)
            {
                string fullName = suite.FullName;
                return fullName == this.name || this.name.StartsWith(fullName + ".") || fullName.StartsWith(this.name + ".");
            }

            public bool Pass(TestCase test)
            {
                string fullName = test.FullName;
                return fullName == this.name || fullName.StartsWith(this.name + ".");
            }
        }

        class ProxyEventListener : EventListener
        {
            ITestListener testListener;

            public ProxyEventListener(ITestListener testListener)
            {
                this.testListener = testListener;
            }

            public void RunStarted(Test[] tests)
            {
            }

            public void RunFinished(TestResult[] results)
            {
            }

            public void RunFinished(Exception exception)
            {
            }

            public void TestStarted(TestCase testCase)
            {
            }

            public void TestFinished(TestCaseResult result)
            {
                TestResultSummary summary = new TestResultSummary();
                summary.TotalTests = 1;         // HACK:
                summary.TestRunner = typeof(NUnitTestRunner).AssemblyQualifiedName;
                summary.IsExecuted = result.Executed;
                summary.IsFailure = result.IsFailure;
                summary.IsSuccess = result.IsSuccess;
                summary.Message = result.Message;
                summary.Name = result.Name;
                summary.StackTrace = result.StackTrace;
                summary.TimeSpan = TimeSpan.FromSeconds(result.Time);
                this.testListener.TestFinished(summary);
            }

            public void SuiteStarted(TestSuite suite)
            {
            }

            public void SuiteFinished(TestSuiteResult result)
            {
            }

            public void UnhandledException(Exception exception)
            {
            }

            public void TestOutput(TestOutput testOutput)
            {
            }
        }
    }
}
