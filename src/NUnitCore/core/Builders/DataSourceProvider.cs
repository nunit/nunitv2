using System;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    /// <summary>
    /// DataSourceProvider provides data for methods
    /// annotated with the DataSourceAttribute.
    /// </summary>
    public class DataSourceProvider : IParameterProvider
    {
        #region IParameterProvider Members

        /// <summary>
        /// Determine whether any ParameterSets
        /// are available for a method.
        /// </summary>
        /// <param name="method">A MethodInfo representing the a parameterized test</param>
        /// <returns>True if any are available, otherwise false.</returns>
        public bool HasParametersFor(MethodInfo method)
        {
            return Reflect.HasAttribute(method, NUnitFramework.DataSourceAttribute, false);
        }

        /// <summary>
        /// Return a list providing ParameterSets
        /// for use in running a test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IList GetParametersFor(MethodInfo method)
        {
            ArrayList parameterList = new ArrayList();

            foreach (Attribute attr in Reflect.GetAttributes(method, NUnitFramework.DataSourceAttribute, false))
            {
                string problem;

                IEnumerable source = GetDataSource(method, attr, out problem);

                if (source == null)
                    parameterList.Add(
                        new ParameterSet(RunState.NotRunnable, problem));
                else
                    foreach (object o in source)
                        parameterList.Add(ParameterSet.FromDataSource(o));
            }

            return parameterList;
        }
        #endregion

        #region Helper Methods
        private static IEnumerable GetDataSource( MethodInfo method, Attribute attr, out string problem )
        {
            problem = null;

            Type sourceType = Reflect.GetPropertyValue(attr, "SourceType") as Type;
            if (sourceType != null)
            {
                BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty;
                foreach (PropertyInfo prop in sourceType.GetProperties(flags))
                    if (prop.PropertyType == typeof(IEnumerable) )
                        return prop.GetValue(null, null) as IEnumerable;

                problem = string.Format("Type {0} has no static property returning an IEnumerable", sourceType.Name);
                return null;
            }

            string sourceName = Reflect.GetPropertyValue(attr, "SourceName") as string;
            if (sourceName != null && sourceName != string.Empty)
            {
                PropertyInfo sourceProperty = method.DeclaringType.GetProperty(
                    sourceName,
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);
                if (sourceProperty == null)
                {
                    problem = string.Format("The property {0} was not found", sourceName);
                    return null;
                }

                object sourceObject = sourceProperty.GetValue(null, null);
                if (sourceObject == null)
                {
                    problem = string.Format("Property {0} returned null", sourceName);
                    return null;
                }

                IEnumerable source = sourceObject as IEnumerable;
                if (source == null)
                {
                    problem = string.Format("Property {0} does not implement IEnumerable", sourceName);
                    return null;
                }

                return source;
            }

            problem = "No DataSource provided on attribute";
            return null;
        }
        #endregion
    }
}
