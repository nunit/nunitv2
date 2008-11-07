// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Text;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    public class ParameterizedTestCaseBuilder : ITestCaseBuilder
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
        public bool CanBuildFrom(MethodInfo method)
        {
            return Reflect.HasAttribute( method, NUnitFramework.TestCaseAttribute, false ) ||
                   Reflect.HasAttribute( method, NUnitFramework.TestCaseSourceAttribute, false ) ||
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
        public Test BuildFrom(MethodInfo method)
        {
            ParameterizedMethodSuite suite = new ParameterizedMethodSuite(method);
            NUnitFramework.ApplyCommonAttributes(method, suite);

            foreach (object source in CoreExtensions.Host.TestCaseProviders.GetTestCasesFor(method))
            {
                ParameterSet parms;

                if ( source == null )
                {
                    parms = new ParameterSet();
                    parms.Arguments = new object[] { null };
                }
                else
                    parms = source as ParameterSet;

                if (parms == null)
                {
                    if (source.GetType().GetInterface("NUnit.Framework.ITestCaseData") != null)
                        parms = ParameterSet.FromDataSource(source);
                    else
                    {
                        parms = new ParameterSet();

                        ParameterInfo[] parameters = method.GetParameters();
                        Type sourceType = source.GetType();

                        if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(sourceType))
                            parms.Arguments = new object[] { source };
                        else if ( source is object[] )
                            parms.Arguments = (object[])source;
                        else if (source is Array)
                        {
                            Array array = (Array)source;
                            if (array.Rank == 1)
                            {
                                parms.Arguments = new object[array.Length];
                                for (int i = 0; i < array.Length; i++)
                                    parms.Arguments[i] = (object)array.GetValue(i);
                            }
                        }
                        else
                            parms.Arguments = new object[] { source };
                    }
                }

                TestMethod test = BuildSingleTestMethod(method, parms);

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
        /// <returns></returns>
        public static NUnitTestMethod BuildSingleTestMethod(MethodInfo method, ParameterSet parms)
        {
            NUnitTestMethod testMethod = new NUnitTestMethod(method);

            // NOTE: After the call to CheckTestMethodSignature, the Method
            // property of testMethod may no longer be the same as the
            // original MethodInfo, so we reassign it here.
            bool signatureOK = CheckTestMethodSignature(testMethod, parms);
            method = testMethod.Method;

            if (signatureOK)
            {
                NUnitFramework.ApplyCommonAttributes(method, testMethod);
                NUnitFramework.ApplyExpectedExceptionAttribute(method, testMethod);
            }

            if (parms != null)
            {
                if (parms.TestName != null)
                {
                    testMethod.TestName.Name = parms.TestName;
                    testMethod.TestName.FullName = method.ReflectedType.FullName + "." + parms.TestName;
                }
                else if (parms.Arguments != null)
                {
                    string name = MethodHelper.GetDisplayName(method, parms.Arguments);
                    testMethod.TestName.Name = name;
                    testMethod.TestName.FullName = method.ReflectedType.FullName + "." + name;
                }

                if (parms.ExpectedExceptionName != null)
                    testMethod.exceptionProcessor = new ExpectedExceptionProcessor( testMethod, parms );

                foreach (string key in parms.Properties.Keys)
                    testMethod.Properties.Add(key, parms.Properties[key]);
                
                // Description is stored in parms.Properties
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
            if (testMethod.Method.IsAbstract)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Method is abstract";
                return false;
            }

            if (!testMethod.Method.IsPublic)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Method is not public";
                return false;
            }

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

            ParameterInfo[] parameters = testMethod.Method.GetParameters();
            int argsNeeded = parameters.Length;

            if (!testMethod.Method.ReturnType.Equals(typeof(void)) && 
                parms.Result == null && parms.ExpectedExceptionName == null)
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

#if NET_2_0
            if (testMethod.Method.IsGenericMethodDefinition)
            {
                Type[] typeArguments = GetTypeArgumentsForMethod(testMethod.Method, arglist);
                foreach( object o in typeArguments )
                if (o == null)
                {
                    testMethod.RunState = RunState.NotRunnable;
                    testMethod.IgnoreReason = "Unable to determine type arguments for fixture";
                    return false;
                }

                testMethod.method = testMethod.Method.MakeGenericMethod(typeArguments);
                parameters = testMethod.Method.GetParameters();

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (arglist[i].GetType() != parameters[i].ParameterType && arglist[i] is IConvertible)
                    {
                        try
                        {
                            arglist[i] = Convert.ChangeType(arglist[i], parameters[i].ParameterType);
                        }
                        catch (Exception)
                        {
                            // Do nothing - the incompatible argument will be reported below
                        }
                    }
                }
            }
#endif

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

#if NET_2_0
        private static Type[] GetTypeArgumentsForMethod(MethodInfo method, object[] arglist)
        {
            Type[] typeParameters = method.GetGenericArguments();
            Type[] typeArguments = new Type[typeParameters.Length];
            ParameterInfo[] parameters = method.GetParameters();

            for (int typeIndex = 0; typeIndex < typeArguments.Length; typeIndex++)
            {
                Type typeParameter = typeParameters[typeIndex];

                for (int argIndex = 0; argIndex < parameters.Length; argIndex++)
                {
                    if (parameters[argIndex].ParameterType.Equals(typeParameter))
                        typeArguments[typeIndex] = TypeHelper.BestCommonType(
                            typeArguments[typeIndex],
                            arglist[argIndex].GetType());
                }
            }

            return typeArguments;
        }
#endif
        #endregion
    }
}
