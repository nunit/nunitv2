using System;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    public class TestCaseParameterProvider : IParameterProvider
    {
        /// <summary>
        /// Determine whether any ParameterSets
        /// are available for a method.
        /// </summary>
        /// <param name="method">A MethodInfo representing the a parameterized test</param>
        /// <returns>True if any are available, otherwise false.</returns>
        public bool HasParametersFor(MethodInfo method)
        {
            return Reflect.HasAttribute(method, NUnitFramework.TestCaseAttribute, false);
        }

        /// <summary>
        /// Return a list providing ParameterSets
        /// for use in running a test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IList GetParametersFor(MethodInfo method)
        {
            Attribute[] testCaseAttrs = Reflect.GetAttributes(method, NUnitFramework.TestCaseAttribute, false);
            ArrayList testCaseArgs = new ArrayList();
            foreach (Attribute attr in testCaseAttrs)
                testCaseArgs.Add( ParameterSet.FromDataSource(attr) );

            return testCaseArgs;
        }
    }
}
