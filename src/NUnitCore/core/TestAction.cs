#if CLR_2_0 || CLR_4_0
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NUnit.Core
{
    public class TestAction
    {
        public static readonly ushort TargetsSite = 1;
        public static readonly ushort TargetsTest = 2;
        public static readonly ushort TargetsSuite = 4;

        private static readonly Type _ActionInterfaceType = null;
        private static readonly Type _TestDetailsClassType = null;

        static TestAction()
        {
            _ActionInterfaceType = Type.GetType(NUnitFramework.TestActionInterface);
            _TestDetailsClassType = Type.GetType(NUnitFramework.TestDetailsClass);
        }

        private readonly object _Target;
        private readonly ushort _Targets;

        public TestAction(object target)
        {
            if(target == null)
                throw new ArgumentNullException("target");

            _Target = target;
            _Targets = (ushort) Reflect.GetPropertyValue(target, "Targets");
        }

        public void ExecuteBefore(ITest test)
        {
            Execute(test, "Before");
        }

        public void ExecuteAfter(ITest test)
        {
            Execute(test, "After");
        }

        private void Execute(ITest test, string methodPrefix)
        {
            var method = Reflect.GetNamedMethod(_ActionInterfaceType, methodPrefix + "Test");
            var details = CreateTestDetails(test);

            Reflect.InvokeMethod(method, _Target, details);
        }

        private static object CreateTestDetails(ITest test)
        {
            object fixture = null;
            MethodInfo method = null;

            var testMethod = test as TestMethod;
            if (testMethod != null)
                method = testMethod.Method;

            var testObject = test as Test;
            if(testObject != null)
                fixture = testObject.Fixture;

            return Activator.CreateInstance(_TestDetailsClassType,
                                            fixture,
                                            method,
                                            test.TestName.FullName,
                                            test.TestType,
                                            test.IsSuite);
        }

        public bool DoesTarget(ushort target)
        {
            return (_Targets & target) == target && _Targets != 0;
        }
    }
}
#endif