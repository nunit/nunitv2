using System;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    class DynamicTestCaseBuilder : ITestCaseBuilder
    {
        #region ITestCaseBuilder Members

        /// <summary>
        /// Determine if the method is marked as a dynamic test method.
        /// The method must be marked with the DynamicTestAttribute and 
        /// take one or more arguments.
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
        public bool CanBuildFrom(System.Reflection.MethodInfo method, Test suite)
        {
            return Reflect.HasAttribute(method, NUnitFramework.DynamicTestAttribute, false);
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
        public Test BuildFrom(System.Reflection.MethodInfo method, Test suite)
        {
            return new DynamicTestMethod(method);
        }

        #endregion
    }
}
