using System;
using System.Text;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    class ParameterizedTestCaseBuilder : ITestCaseBuilder
    {
        #region ITestCaseBuilder Members

        /// <summary>
        /// Determine if the method is marked as a parameterized test method.
        /// The method must be marked with the TestAttribute and have 
        /// multiple arguments for this to be true. Methods without the
        /// TestAttribute but with one or more TestCaseAttributes are
        /// treated as a special case.
        /// 
        /// Note that this method does not check that the signature
        /// of the method for validity. If we did that here, any
        /// test methods with invalid signatures would be passed
        /// over in silence in the test run. Since we want such
        /// methods to be reported, the check for validity is made
        /// in BuildFrom rather than here.
        /// </summary>
        /// <param name="method">A MethodInfo for the method being used as a test method</param>
        /// <param name="suite">The test suite being built, to which the new test would be added</param>
        /// <returns>True if the builder can create a test case from this method</returns>
        public bool CanBuildFrom(MethodInfo method, Test suite)
        {
            return Reflect.HasAttribute( method, NUnitFramework.TestCaseAttribute, false ) ||
                   Reflect.HasAttribute( method, NUnitFramework.TestAttribute, false ) &&
                   CoreExtensions.Host.TestCaseProviders.HasTestCasesFor( method );
        }

        /// <summary>
        /// Build a Test from the provided MethodInfo. Depending on
        /// whether the method takes arguments and on the availability
        /// of test case data, this method may return a single test
        /// or a group of tests contained in a ParameterizedMethodSuite.
        /// </summary>
        /// <param name="method">The MethodInfo for which a test is to be built</param>
        /// <param name="suite">The test fixture being populated, or null</param>
        /// <returns>A Test representing one or more method invocations</returns>
        public Test BuildFrom(MethodInfo method, Test parent)
        {
            ParameterizedMethodSuite suite = new ParameterizedMethodSuite(method);
            NUnitFramework.ApplyCommonAttributes(method, suite);

            foreach (object o in CoreExtensions.Host.TestCaseProviders.GetTestCasesFor(method))
            {
                ParameterSet parms = o as ParameterSet;
                if (parms == null) parms = ParameterSet.FromDataSource(o);
                TestMethod test = BuildSingleTestMethod(method, parms, suite);

                suite.Add(test);
            }

            return suite;
        }

        /// <summary>
        /// Builds a single NUnitTestMethod, either as a child of the fixture 
        /// or as one of a set of test cases under a ParameterizedTestMethodSuite.
        /// </summary>
        /// <param name="method">The MethodInfo from which to construct the TestMethod</param>
        /// <param name="parms">The ParameterSet to be used, or null</param>
        /// <param name="suite">The ParameterizedMethodSuite being constructed, or null</param>
        /// <returns></returns>
        private static NUnitTestMethod BuildSingleTestMethod(MethodInfo method, ParameterSet parms, TestSuite suite)
        {
            NUnitTestMethod testMethod = new NUnitTestMethod(method);

            if (CheckTestMethodSignature(testMethod, parms))
            {
                NUnitFramework.ApplyCommonAttributes(method, testMethod);
                NUnitFramework.ApplyExpectedExceptionAttribute(method, testMethod);
            }

            if (parms != null)
            {
                if (parms.TestName != null)
                {
                    testMethod.TestName.Name = parms.TestName;
                    testMethod.TestName.FullName = method.DeclaringType.FullName + "." + parms.TestName;
                }
                else if (parms.Arguments != null && suite != null)
                {
                    string suffix = GetArgumentString(parms.Arguments);
                    testMethod.TestName.Name += suffix;
                    testMethod.TestName.FullName += suffix;
                }

                if (parms.ExpectedExceptionName != null)
                {
                    testMethod.ExceptionExpected = true;
                    testMethod.ExpectedExceptionType = parms.ExpectedExceptionType;
                    testMethod.ExpectedExceptionName = parms.ExpectedExceptionName;
                    testMethod.ExpectedMessage = parms.ExpectedExceptionMessage;
                }

                if (parms.Description != null)
                    testMethod.Description = parms.Description;
            }

            return testMethod;
        }

        /// <summary>
        /// Helper method that checks the signature of a TestMethod and
        /// any supplied parameters to determine if the test is valid.
        /// 
        /// Currently, NUnitTestMethods are required to be public, 
        /// non-abstract methods, either static or instance,
        /// returning void. They may take arguments but the values must
        /// be provided or the TestMethod is not considered runnable.
        /// 
        /// Methods not meeting these criteria will be marked as
        /// non-runnable and the method will return false in that case.
        /// </summary>
        /// <param name="testMethod">The TestMethod to be checked. If it
        /// is found to be non-runnable, it will be modified.</param>
        /// <param name="parms">Parameters to be used for this test, or null</param>
        /// <returns>True if the method signature is valid, false if not</returns>
        private static bool CheckTestMethodSignature(TestMethod testMethod, ParameterSet parms)
        {
            MethodInfo method = testMethod.Method;

            if (method.IsAbstract)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Method is abstract";
                return false;
            }

            if (!method.IsPublic)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Method is not public";
                return false;
            }

            //if (parms == null)
            //{
            //    if (method.GetParameters().Length > 0)
            //    {
            //        testMethod.RunState = RunState.NotRunnable;
            //        testMethod.IgnoreReason = "No arguments provided";
            //        return false;
            //    }
            //    else if (!method.ReturnType.Equals(typeof(void)))
            //    {
            //        testMethod.RunState = RunState.NotRunnable;
            //        testMethod.IgnoreReason = "Method has non-void return value";
            //        return false;
            //    }

            //    return true;
            //}

            testMethod.arguments = parms.Arguments;
            testMethod.expectedResult = parms.Result;
            testMethod.RunState = parms.RunState;
            testMethod.IgnoreReason = parms.NotRunReason;

            if (testMethod.RunState != RunState.Runnable)
                return false;

            object[] arglist = parms.Arguments;

            int argsProvided = 0;
            if (arglist != null)
                argsProvided = arglist.Length;

            ParameterInfo[] parameters = method.GetParameters();
            int argsNeeded = parameters.Length;

            if (!method.ReturnType.Equals(typeof(void)) && parms.Result == null && parms.ExpectedExceptionName == null)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Method has non-void return value";
                return false;
            }

            if (argsProvided > 0 && argsNeeded == 0)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Arguments provided for method not taking any";
                return false;
            }

            if (argsProvided == 0 && argsNeeded > 0)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "No arguments were provided";
                return false;
            }

            if (argsProvided != argsNeeded)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Wrong number of arguments provided";
                return false;
            }

            for (int i = 0; i < argsProvided; i++)
            {
                object arg = arglist[i];
                if (arg != null)
                {
                    Type targetType = parameters[i].ParameterType;
                    Type argType = arg.GetType();

                    if (!targetType.IsAssignableFrom(argType))
                    {
                        testMethod.RunState = RunState.NotRunnable;
                        testMethod.IgnoreReason = string.Format(
                            "Argument {0}: Cannot convert from {1} to {2}",
                            i + 1,
                            arg,
                            targetType);
                        return false;
                    }
                }
            }

            return true;
        }

        private static string GetArgumentString(object[] arglist)
        {
            StringBuilder sb = new StringBuilder("(");

            if (arglist != null)
                for (int i = 0; i < arglist.Length; i++)
                {
                    if (i > 0) sb.Append(",");

                    object arg = arglist[i];
                    if (arg == null)
                        sb.Append("null");
                    else
                        sb.Append(arg.ToString());
                }

            sb.Append(")");

            return sb.ToString();
        }
        #endregion
    }
}
