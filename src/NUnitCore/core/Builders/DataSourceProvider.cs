using System;
using System.Reflection;
using System.Collections;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    /// <summary>
    /// DataSourceProvider supplies data items for individual parameters
    /// from named data sources in the test class or a separate class.
    /// </summary>
    public class DataSourceProvider : IDataPointProvider
    {
        #region Constants
        public const string SourcesAttribute = "NUnit.Framework.DataSourceAttribute";
        public const string SourceTypeProperty = "SourceType";
        public const string SourceNameProperty = "SourceName";
        #endregion

        #region IDataPointProvider Members

        /// <summary>
        /// Determine whether any data sources are available for a parameter.
        /// </summary>
        /// <param name="parameter">A ParameterInfo test parameter</param>
        /// <returns>True if any data is available, otherwise false.</returns>
        public bool HasDataFor(ParameterInfo parameter)
        {
            return Reflect.HasAttribute(parameter, SourcesAttribute, false);
        }

        /// <summary>
        /// Return an IEnumerable providing test data for use with
        /// one parameter of a parameterized test.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IEnumerable GetDataFor(ParameterInfo parameter)
        {
#if NET_2_0 && !MONO
            foreach (ProviderInfo info in GetSourcesFor(parameter))
            {
                if (info.Provider != null)
                    foreach (object o in info.Provider)
                        yield return o;
            }
#else
            ArrayList parameterList = new ArrayList();

            foreach ( ProviderInfo info in GetSourcesFor(parameter) )
            {
                if (info.Provider != null)
                    foreach (object o in info.Provider)
                        parameterList.Add(o);
            }

            return parameterList;
#endif
        }
        #endregion

        #region Helper Methods
        private static IList GetSourcesFor(ParameterInfo parameter)
        {
            ArrayList sources = new ArrayList();
            foreach (Attribute sourceAttr in Reflect.GetAttributes(parameter, SourcesAttribute, false))
            {
                Type sourceType = Reflect.GetPropertyValue(sourceAttr, SourceTypeProperty) as Type;
                if (sourceType == null)
                    sourceType = parameter.Member.ReflectedType;

                string sourceName = Reflect.GetPropertyValue(sourceAttr, SourceNameProperty) as string;
                if (sourceName != null && sourceName.Length > 0)
                {
                    sources.Add(new ProviderInfo(sourceType, sourceName));
                }
            }
            return sources;
        }
        #endregion
    }
}
