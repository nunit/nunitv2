// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    /// <summary>
    /// Class to build ether a parameterized or a normal NUnitTestMethod.
    /// There are four cases that the builder must deal with:
    ///   1. The method needs no params and none are provided
    ///   2. The method needs params and they are provided
    ///   3. The method needs no params but they are provided in error
    ///   4. The method needs params but they are not provided
    /// This could have been done using two different builders, but it
    /// turned out to be simpler to have just one. The BuildFrom method
    /// takes a different branch depending on whether any parameters are
    /// provided, but all four cases are dealt with in lower-level methods
    /// </summary>
	public class NUnitTestCaseBuilder : Extensibility.ITestCaseBuilder
	{
		private readonly bool allowOldStyleTests = NUnitFramework.AllowOldStyleTests;

        #region ITestCaseBuilder Methods
        /// <summary>
        /// Determine if the method is marked as an NUnit test method.
        /// The method must normally be marked with the TestAttribute
        /// for this to be true. If the test config file sets 
        /// AllowOldStyleTests to true, then any method beginning 
        /// "test..." (case-insensitive) is treated as a test unless 
        /// it is also marked as a setup or teardown method.
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
            if( Reflect.HasAttribute( method, NUnitFramework.TestAttribute, false ) &&
                !CoreExtensions.Host.TestCaseProviders.HasTestCasesFor( method ) )
                    return true;

            if (allowOldStyleTests)
            {
                Regex regex = new Regex("^(?i:test)");
                if ( regex.Match(method.Name).Success 
					&& !NUnitFramework.IsSetUpMethod( method )
					&& !NUnitFramework.IsTearDownMethod( method )
					&& !NUnitFramework.IsFixtureSetUpMethod( method )
					&& !NUnitFramework.IsFixtureTearDownMethod( method ) )
						return true;
            }

            return false;
        }

		/// <summary>
        /// Build a an NUnitTestMethod from the provided MethodInfo. 
        /// </summary>
        /// <param name="method">The MethodInfo for which a test is to be built</param>
        /// <param name="suite">The test fixture being populated, or null</param>
        /// <returns>A Test representing one or more method invocations</returns>
        public Test BuildFrom(System.Reflection.MethodInfo method, Test suite)
		{
            NUnitTestMethod testMethod = new NUnitTestMethod(method);

            if (CheckTestMethodSignature(testMethod))
            {
                NUnitFramework.ApplyCommonAttributes(method, testMethod);
                NUnitFramework.ApplyExpectedExceptionAttribute(method, testMethod);
            }

            return testMethod;
        }
		#endregion

        #region Helper Methods
	    /// <summary>
        /// Helper method that checks the signature of a TestMethod
        /// to determine if the test is valid.
        /// 
        /// Currently, NUnitTestMethods are required to be public, 
        /// non-abstract methods, either static or instance,
        /// returning void. They may not take arguments - but see
        /// <see>T:ParameterizedTestMethodBuilder</see>.
        /// 
        /// Methods not meeting these criteria will be marked as
        /// non-runnable and the method will return false in that case.
        /// </summary>
        /// <param name="testMethod">The TestMethod to be checked. If it
        /// is found to be non-runnable, it will be modified.</param>
        /// <returns>True if the method signature is valid, false if not</returns>
        private static bool CheckTestMethodSignature(TestMethod testMethod)
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

            if (method.GetParameters().Length > 0)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "No arguments provided";
                return false;
            }
            else if (!method.ReturnType.Equals(typeof(void)))
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Method has non-void return value";
                return false;
            }

            return true;
        }
		#endregion
    }
}
