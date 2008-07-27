using System;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    /// <summary>
    /// TestCaseFactoryProvider provides data for methods
    /// annotated with the FactoryiesAttribute.
    /// </summary>
    public class TestCaseFactoryProvider : ITestCaseProvider
    {
        #region Constants
        public const string FactoriesAttribute = "NUnit.Framework.FactoriesAttribute";
        public const string FactoryTypeProperty = "FactoryType";
        public const string FactoryNameProperty = "FactoryName";

        public const string TestCaseFactoryAttribute = "NUnit.Framework.TestCaseFactoryAttribute";
        public const string ArgTypesProperty = "ArgTypes";
        #endregion

        #region ITestCaseProvider Members

        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>True if any cases are available, otherwise false.</returns>
        public bool HasTestCasesFor(MethodInfo method)
        {
            return Reflect.HasAttribute(method, FactoriesAttribute, false);
        }

        /// <summary>
        /// Return an IEnumerable providing test cases for use in
        /// running a parameterized test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IEnumerable GetTestCasesFor(MethodInfo method)
        {
#if NET_2_0
            foreach( ProviderInfo info in GetFactoriesFor(method) )
            {
                if (info.Provider == null)
                    yield return new ParameterSet(RunState.NotRunnable, info.Message);
                else
                    foreach (object o in info.Provider)
                        yield return o;
            }
#else
            ArrayList parameterList = new ArrayList();

            foreach ( ProviderInfo info in GetFactoriesFor(method) )
            {
                if (info.Provider == null)
                    parameterList.Add(
                        new ParameterSet(RunState.NotRunnable, info.Message));
                else
                    foreach (object o in info.Provider)
                        parameterList.Add(o);
            }

            return parameterList;
#endif
        }
        #endregion

        #region Helper Methods
        private static IList GetFactoriesFor(MethodInfo method)
        {
            ArrayList factories = new ArrayList();
            foreach (Attribute factoryAttr in Reflect.GetAttributes(method, FactoriesAttribute, false))
            {
                Type factoryType = Reflect.GetPropertyValue(factoryAttr, FactoryTypeProperty) as Type;
                if (factoryType == null)
                    factoryType = method.DeclaringType;

                string factoryNames = Reflect.GetPropertyValue(factoryAttr, FactoryNameProperty) as string;
                if (factoryNames != null && factoryNames != string.Empty)
                {
                    foreach (string factoryName in factoryNames.Split(new char[] { ',' }))
                    {
                        factories.Add(new ProviderInfo(factoryType, factoryName));
                    }
                }
                //else
                //{
                //    int found = 0;
                //    foreach (MemberInfo member in factoryType.GetMembers(
                //        BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) )
                //    {
                //        Attribute testFactoryAttr = Reflect.GetAttribute(member, TestCaseFactoryAttribute, false);
                //        if ( testFactoryAttr != null )
                //        {
                //            Type[] argTypes = Reflect.GetPropertyValue(testFactoryAttr, ArgTypesProperty) as Type[];
                //            if (argTypes != null && argTypes.Length == method.GetParameters().Length)
                //            {
                //                bool isOK = true;
                //                for (int i = 0; i < argTypes.Length && isOK; i++)
                //                    if (argTypes[i] != method.GetParameters()[i].ParameterType)
                //                        isOK = false;

                //                if (isOK)
                //                {
                //                    ++found;
                //                    factories.Add(new ProviderInfo(factoryType, member.Name));
                //                }
                //            }
                //        }
                //    }
                //}
            }
            return factories;
        }
        #endregion
    }
}
