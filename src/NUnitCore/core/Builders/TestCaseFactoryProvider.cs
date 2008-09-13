// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

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
        public const string TestCasesAttribute = "NUnit.Framework.TestCaseSourceAttribute";
        public const string SourceTypeProperty = "SourceType";
        public const string SourceNameProperty = "SourceName";
        #endregion

        #region ITestCaseProvider Members

        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>True if any cases are available, otherwise false.</returns>
        public bool HasTestCasesFor(MethodInfo method)
        {
            return Reflect.HasAttribute(method, TestCasesAttribute, false);
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
            foreach( ProviderInfo info in GetSourcesFor(method) )
            {
                if (info.Provider == null)
                    yield return new ParameterSet(RunState.NotRunnable, info.Message);
                else
                    foreach (object o in info.Provider)
                        yield return o;
            }
#else
            ArrayList parameterList = new ArrayList();

            foreach ( ProviderInfo info in GetSourcesFor(method) )
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
        private static IList GetSourcesFor(MethodInfo method)
        {
            ArrayList sources = new ArrayList();
            foreach (Attribute sourceAttr in Reflect.GetAttributes(method, TestCasesAttribute, false))
            {
                Type sourceType = Reflect.GetPropertyValue(sourceAttr, SourceTypeProperty) as Type;
                if (sourceType == null)
                    sourceType = method.ReflectedType;

                string sourceName = Reflect.GetPropertyValue(sourceAttr, SourceNameProperty) as string;
                sources.Add(new ProviderInfo(sourceType, sourceName));
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
            return sources;
        }
        #endregion
    }
}
