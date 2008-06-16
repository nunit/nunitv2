using System;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    /// <summary>
    /// DataSourceProvider provides data for methods
    /// annotated with the FactoryAttribute.
    /// </summary>
    public class DataSourceProvider : IParameterProvider
    {
        #region IParameterProvider Members

        /// <summary>
        /// Determine whether any ParameterSets
        /// are available for a method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>True if any parameters are available, otherwise false.</returns>
        public bool HasParametersFor(MethodInfo method)
        {
            return Reflect.HasAttribute(method, NUnitFramework.FactoryAttribute, false);
        }

        /// <summary>
        /// Return a list providing ParameterSets
        /// for use in running a test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IEnumerable GetParametersFor(MethodInfo method)
        {
#if NET_2_0
            foreach (Attribute attr in Reflect.GetAttributes(method, NUnitFramework.FactoryAttribute, false))
            {
                Type factoryType = Reflect.GetPropertyValue(attr, "FactoryType") as Type;
                string problem;

                IEnumerable factory = GetFactory(method, attr, out problem);

                if (factory == null)
                    yield return new ParameterSet(RunState.NotRunnable, problem);
                else
                    foreach (object o in factory)
                        yield return o;
            }

#else
            ArrayList parameterList = new ArrayList();

            foreach (Attribute attr in Reflect.GetAttributes(method, NUnitFramework.FactoryAttribute, false))
            {
                string problem;

                IEnumerable factory = GetFactory(method, attr, out problem);

                if (factory == null)
                    parameterList.Add(
                        new ParameterSet(RunState.NotRunnable, problem));
                else
                    foreach (object o in factory)
                        parameterList.Add(o);
            }

            return parameterList;
#endif
        }
        #endregion

        #region Helper Methods
        private static IEnumerable GetFactory( MethodInfo method, Attribute attr, out string problem )
        {
            problem = null;

            Type factoryType = Reflect.GetPropertyValue(attr, "FactoryType") as Type;
            if (factoryType == null)
                factoryType = method.DeclaringType;

            string factoryName = Reflect.GetPropertyValue(attr, "FactoryName") as string;
            if (factoryName != null && factoryName != string.Empty)
            {
                MemberInfo[] members = factoryType.GetMember(
                    factoryName,
                    MemberTypes.Field | MemberTypes.Method | MemberTypes.Property,
                    BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (members.Length == 0)
                {
                    problem = string.Format("Unable to locate {0}.{1} was not found", factoryType.FullName, factoryName);
                    return null;
                }

                if (members.Length > 1)
                {
                    problem = string.Format("{0}.{1} is ambiguous", factoryType.FullName, factoryName);
                    return null;
                }

                object factoryObject = null;
                MemberInfo member = members[0];
                object instance = null;

                switch (member.MemberType)
                {
                    case MemberTypes.Property:
                        PropertyInfo factoryProperty = member as PropertyInfo;
                        MethodInfo getMethod = factoryProperty.GetGetMethod(true);
                        if (!getMethod.IsStatic)
                            instance = Reflect.Construct(factoryType);
                        factoryObject = factoryProperty.GetValue(instance, null);
                        break;

                    case MemberTypes.Method:
                        MethodInfo factoryMethod = member as MethodInfo;
                        factoryObject = factoryMethod.Invoke(instance, null);
                        break;

                    case MemberTypes.Field:
                        FieldInfo factoryField = member as FieldInfo;
                        if (!factoryField.IsStatic)
                            instance = Reflect.Construct(factoryType);
                        factoryObject = factoryField.GetValue(instance);
                        break;
                }

                if (factoryObject == null)
                {
                    problem = string.Format("Factory {0} returned null", factoryName);
                    return null;
                }

                IEnumerable factory = factoryObject as IEnumerable;
                if (factory == null)
                {
                    problem = string.Format("Property {0} does not implement IEnumerable", factoryName);
                    return null;
                }

                return factory;
            }

            problem = "No DataSource provided on attribute";
            return null;
        }
        #endregion
    }
}
