// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    /// <summary>
    /// TestCaseSourceProvider provides data for methods
    /// annotated with the TestCaseSourceAttribute.
    /// </summary>
    public class TestCaseSourceProvider : ITestCaseProvider
    {
        #region Constants
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
            return Reflect.HasAttribute(method, NUnitFramework.TestCaseSourceAttribute, false);
        }

        /// <summary>
        /// Return an IEnumerable providing test cases for use in
        /// running a parameterized test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IEnumerable GetTestCasesFor(MethodInfo method)
        {
            ArrayList parameterList = new ArrayList();

            foreach ( ProviderInfo info in GetSourcesFor(method) )
            {
                foreach (object o in info.Provider)
                    parameterList.Add(o);
            }

            return parameterList;
        }
        #endregion

        #region Helper Methods
        private static IList GetSourcesFor(MethodInfo method)
        {
            ArrayList sources = new ArrayList();
            foreach (Attribute sourceAttr in Reflect.GetAttributes(method, NUnitFramework.TestCaseSourceAttribute, false))
            {
                Type sourceType = Reflect.GetPropertyValue(sourceAttr, SourceTypeProperty) as Type;
                if (sourceType == null)
                    sourceType = method.ReflectedType;

                string sourceName = Reflect.GetPropertyValue(sourceAttr, SourceNameProperty) as string;
                sources.Add(new ProviderInfo(sourceType, sourceName));
            }
            return sources;
        }
        #endregion
    }
}
