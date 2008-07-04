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
    public class TestCaseFactoryProvider : ITestCaseProvider
    {
        #region Constants
        public const string FactoryAttribute = "NUnit.Framework.FactoryAttribute";
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
            return Reflect.HasAttribute(method, FactoryAttribute, false);
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
            foreach( FactoryInfo info in GetFactoriesFor(method) )
            {
                if (info.Factory == null)
                    yield return new ParameterSet(RunState.NotRunnable, info.Message);
                else
                    foreach (object o in info.Factory)
                        yield return o;
            }
#else
            ArrayList parameterList = new ArrayList();

            foreach ( FactoryInfo info in GetFactoriesFor(method) )
            {
                if (info.Factory == null)
                    parameterList.Add(
                        new ParameterSet(RunState.NotRunnable, info.Message));
                else
                    foreach (object o in info.Factory)
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
            foreach (Attribute factoryAttr in Reflect.GetAttributes(method, FactoryAttribute, false))
            {
                Type factoryType = Reflect.GetPropertyValue(factoryAttr, FactoryTypeProperty) as Type;
                if (factoryType == null)
                    factoryType = method.DeclaringType;

                string factoryNames = Reflect.GetPropertyValue(factoryAttr, FactoryNameProperty) as string;
                if (factoryNames != null && factoryNames != string.Empty)
                {
                    foreach (string factoryName in factoryNames.Split(new char[] { ',' }))
                    {
                        factories.Add(new FactoryInfo(factoryType, factoryName));
                    }
                }
                else
                {
                    int found = 0;
                    foreach (MemberInfo member in factoryType.GetMembers(
                        BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) )
                    {
                        Attribute testFactoryAttr = Reflect.GetAttribute(member, TestCaseFactoryAttribute, false);
                        if ( testFactoryAttr != null )
                        {
                            Type[] argTypes = Reflect.GetPropertyValue(testFactoryAttr, ArgTypesProperty) as Type[];
                            if (argTypes != null && argTypes.Length == method.GetParameters().Length)
                            {
                                bool isOK = true;
                                for (int i = 0; i < argTypes.Length && isOK; i++)
                                    if (argTypes[i] != method.GetParameters()[i].ParameterType)
                                        isOK = false;

                                if (isOK)
                                {
                                    ++found;
                                    factories.Add(new FactoryInfo(factoryType, member.Name));
                                }
                            }
                        }
                    }
                }
            }
            return factories;
        }
        #endregion

        #region Nested FactoryInfo class
        private class FactoryInfo
        {
            private Type factoryType;
            private string factoryName;
            private IEnumerable factory;
            private string message;

            public FactoryInfo(Type factoryType, string factoryName)
            {
                if (factoryType == null)
                    throw new ArgumentNullException("factoryType");
                if (factoryName == null)
                    throw new ArgumentNullException("factoryName");

                this.factoryType = factoryType;
                this.factoryName = factoryName;
            }

            public IEnumerable Factory
            {
                get 
                {
                    // Don't try to populate factory more than once
                    if (factory == null && message == null)
                    {
                        MemberInfo[] members = factoryType.GetMember(
                            factoryName,
                            MemberTypes.Field | MemberTypes.Method | MemberTypes.Property,
                            BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                        if (members.Length == 0)
                            message = string.Format(
                                "Unable to locate {0}.{1}", factoryType.FullName, factoryName);

                        if (members.Length > 1)
                            message = string.Format(
                                "{0}.{1} is ambiguous", factoryType.FullName, factoryName);

                        object factoryObject = GetFactoryObjectFromMember(members[0]);

                        if (factoryObject == null)
                            message = string.Format("Factory {0} returned null", factoryName);

                        factory = factoryObject as IEnumerable;
                        if (factory == null)
                            message = string.Format("Factory {0} does not implement IEnumerable", factoryName);
                    }

                    return factory; 
                }
            }

            public string Message
            {
                get { return message; }
            }

            private object GetFactoryObjectFromMember(MemberInfo member)
            {
                object factoryObject = null;
                object instance = null;

                switch (member.MemberType)
                {
                    case MemberTypes.Property:
                        PropertyInfo factoryProperty = member as PropertyInfo;
                        MethodInfo getMethod = factoryProperty.GetGetMethod(true);
                        if (!getMethod.IsStatic)
                            instance = FactoryInstanceCache.GetInstanceOf(factoryType);
                        factoryObject = factoryProperty.GetValue(instance, null);
                        break;

                    case MemberTypes.Method:
                        MethodInfo factoryMethod = member as MethodInfo;
                        if (!factoryMethod.IsStatic)
                            instance = FactoryInstanceCache.GetInstanceOf(factoryType);
                        factoryObject = factoryMethod.Invoke(instance, null);
                        break;

                    case MemberTypes.Field:
                        FieldInfo factoryField = member as FieldInfo;
                        if (!factoryField.IsStatic)
                            instance = FactoryInstanceCache.GetInstanceOf(factoryType);
                        factoryObject = factoryField.GetValue(instance);
                        break;
                }

                return factoryObject;
            }
        }
        #endregion

        #region FactoryInstanceCache
        class FactoryInstanceCache
        {
            private static IDictionary instances = new Hashtable();

            public static object GetInstanceOf(Type factoryType)
            {
                object instance = instances[factoryType];
                return instance == null
                    ? instances[factoryType] = Reflect.Construct(factoryType)
                    : instance;
            }

            public static void Clear()
            {
                foreach (object key in instances.Keys)
                {
                    IDisposable factory = instances[key] as IDisposable;
                    if ( factory != null )
                        factory.Dispose();
                    instances.Remove(key);
                }
            }
        }
        #endregion
    }
}
