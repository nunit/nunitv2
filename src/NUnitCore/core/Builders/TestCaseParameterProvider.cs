using System;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    public class TestCaseParameterProvider : IParameterProvider
    {
        public bool HasParametersFor( MethodInfo method )
        {
            return Reflect.HasAttribute(method, NUnitFramework.TestCaseAttribute, false);
        }

        public IList GetParametersFor(MethodInfo method)
        {
            Attribute[] testCaseAttrs = Reflect.GetAttributes(method, NUnitFramework.TestCaseAttribute, false);
            ArrayList testCaseArgs = new ArrayList();
            foreach (Attribute attr in testCaseAttrs)
            {
                ParameterSet parms = ParameterSet.FromAttribute(attr);
                testCaseArgs.Add(parms);
            }
            return testCaseArgs;
        }
    }
}
