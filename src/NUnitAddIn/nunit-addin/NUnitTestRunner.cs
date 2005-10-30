namespace NUnit.AddInRunner
{
    using System;
    using System.Text;
    using System.Reflection;
    using TestDriven.Framework;
    using TDF = TestDriven.Framework;
    using NUnit.Core;
    using NUC = NUnit.Core;
    using System.Diagnostics;

    public class NUnitTestRunner : ITestRunner
    {
        TestSuiteBuilder testSuiteBuilder = new TestSuiteBuilder();

        public TestRunResult RunAssembly(ITestListener testListener, Assembly assembly)
        {
            IFilter filter = new NoFilter();
            return run(testListener, assembly, filter);
        }

        public TestRunResult RunMember(ITestListener testListener, Assembly assembly, MemberInfo member)
        {
            if (member is Type)
            {
                Type type = member as Type;
                IFilter filter = new TypeFilter(assembly, type);
                return run(testListener, assembly, filter);
            }
            else if (member is MethodInfo)
            {
                MethodInfo method = member as MethodInfo;
                IFilter filter = new MethodFilter(assembly, method);
                return run(testListener, assembly, filter);
            }
            else
            {
                return TestRunResult.NoTests;
            }
        }

        public TestRunResult RunNamespace(ITestListener testListener, Assembly assembly, string ns)
        {
            IFilter filter = new NamespaceFilter(assembly, ns);
            return run(testListener, assembly, filter);
        }

        TestRunResult run(ITestListener testListener, Assembly assembly, IFilter filter)
        {
            TestSuite testSuite = this.testSuiteBuilder.Build(assembly.FullName);
            int totalTestCases = testSuite.CountTestCases(filter);
            if (totalTestCases == 0)
            {
                return TestRunResult.NoTests;
            }

            EventListener listener = new ProxyEventListener(testListener, totalTestCases);
            NUC.TestResult result = testSuite.Run(listener, filter);
            return toTestRunResult(result);
        }

        static TestRunResult toTestRunResult(NUC.TestResult result)
        {
            if (result.IsFailure)
            {
                return TestRunResult.Failure;
            }
            else if (result.Executed)
            {
                return TestRunResult.Success;
            }
            else
            {
                return TestRunResult.NoTests;
            }
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

        class MethodFilter : IFilter
        {
            Assembly assembly;
            MethodInfo method;

            public MethodFilter(Assembly assembly, MethodInfo method)
            {
                this.assembly = assembly;
                this.method = method;
            }

            public bool Pass(TestSuite suite)
            {
                return true;
            }

            public bool Pass(TestCase test)
            {
                string testFullName = test.FullName;
                if (testFullName == this.method.ReflectedType.FullName + "." + this.method.Name)
                {
                    return true;
                }

                return false;
            }
        }

        class TypeFilter : IFilter
        {
            Assembly assembly;
            Type type;

            public TypeFilter(Assembly assembly, Type type)
            {
                this.assembly = assembly;
                this.type = type;
            }

            public bool Pass(TestSuite suite)
            {
                return true;
            }

            public bool Pass(TestCase test)
            {
                string testFullName = test.FullName;
                if (testFullName.StartsWith(this.type.FullName + "."))
                {
                    return true;
                }

                return false;
            }
        }

        class NamespaceFilter : IFilter
        {
            Assembly assembly;
            string ns;

            public NamespaceFilter(Assembly assembly, string ns)
            {
                this.assembly = assembly;
                this.ns = ns;
            }

            public bool Pass(TestSuite suite)
            {
                return true;
            }

            public bool Pass(TestCase test)
            {
                string testFullName = test.FullName;
                if (this.ns == "" || testFullName.StartsWith(this.ns + "."))
                {
                    return true;
                }

                return false;
            }
        }

        class ProxyEventListener : EventListener
        {
            ITestListener testListener;
            int totalTestCases;

            public ProxyEventListener(ITestListener testListener, int totalTestCases)
            {
                this.testListener = testListener;
                this.totalTestCases = totalTestCases;
            }

            public void RunStarted(Test[] tests)
            {
            }

            public void RunFinished(NUC.TestResult[] results)
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
                summary.TotalTests = totalTestCases;
                summary.TestRunner = typeof(NUnitTestRunner).AssemblyQualifiedName;
                summary.Result = toTestResult(result);
                summary.Message = result.Message;
                summary.Name = result.Name;
                summary.StackTrace = result.StackTrace;
                summary.TimeSpan = TimeSpan.FromSeconds(result.Time);
                this.testListener.TestFinished(summary);
            }

            static TDF.TestResult toTestResult(TestCaseResult result)
            {
                if (result.IsFailure)
                {
                    return TDF.TestResult.Failure;
                }

                if (result.IsSuccess)
                {
                    return TDF.TestResult.Success;
                }

                if (!result.Executed)
                {
                    // NOTE: Does this always mean ignored?
                    return TDF.TestResult.Ignored;
                }

                // NOTE: What would this mean?
                return TDF.TestResult.Ignored;
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
