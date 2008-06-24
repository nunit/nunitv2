using System;
using System.Reflection;

namespace NUnit.Core
{
    public class DynamicTestMethod : NUnitTestMethod
    {
        public DynamicTestMethod(MethodInfo method) : base(method) { }

        public override void RunTestMethod(TestResult testResult)
        {
            base.RunTestMethod(testResult);
        }
    }
}
