using System;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core
{
    public class DynamicTestMethod : NUnitTestMethod
    {
        public DynamicTestMethod(MethodInfo method) : base(method) { }

        public override void RunTestMethod(TestResult testResult)
        {
            ParameterInfo[] parameters = this.Method.GetParameters();
            int argsNeeded = parameters.Length;

            ITestCaseProvider provider = (ITestCaseProvider)CoreExtensions.Host.GetExtensionPoint("TestCaseProviders");
            if (!provider.HasTestCasesFor(this.Method))
                testResult.SetResult(ResultState.Error, "No arguments were provided", null);
            else
            {
                testResult.Success();

                foreach (object o in provider.GetTestCasesFor(this.Method))
                {
                    ParameterSet parms = o as ParameterSet;
                    if (parms == null)
                        parms = ParameterSet.FromDataSource(o);
                    this.arguments = parms.Arguments;
                    TestResult partResult = new TestResult(this);
                    base.RunTestMethod(partResult);
                    testResult.AddResult(partResult);
                }
            }
        }
    }
}
