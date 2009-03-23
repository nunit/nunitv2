// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core.Builders
{
    class ProviderInfo
    {
        private Type providerType;
        private string providerName;

        public ProviderInfo(Type providerType, string providerName)
        {
            if (providerType == null)
                throw new ArgumentNullException("providerType");
            if (providerName == null)
                throw new ArgumentNullException("providerName");

            this.providerType = providerType;
            this.providerName = providerName;
        }

        public string Name
        {
            get { return this.providerName; }
        }

        public IEnumerable Provider
        {
            get
            {
                MemberInfo[] members = providerType.GetMember(
                    providerName,
                    MemberTypes.Field | MemberTypes.Method | MemberTypes.Property,
                    BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (members.Length == 0)
                    throw new Exception(string.Format(
                        "Unable to locate {0}.{1}", providerType.FullName, providerName));

                return (IEnumerable)GetProviderObjectFromMember(members[0]);
            }
        }

        private object GetProviderObjectFromMember(MemberInfo member)
        {
            object providerObject = null;
            object instance = null;

            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    PropertyInfo providerProperty = member as PropertyInfo;
                    MethodInfo getMethod = providerProperty.GetGetMethod(true);
                    if (!getMethod.IsStatic)
                        instance = ProviderCache.GetInstanceOf(providerType);
                    providerObject = providerProperty.GetValue(instance, null);
                    break;

                case MemberTypes.Method:
                    MethodInfo providerMethod = member as MethodInfo;
                    if (!providerMethod.IsStatic)
                        instance = ProviderCache.GetInstanceOf(providerType);
                    providerObject = providerMethod.Invoke(instance, null);
                    break;

                case MemberTypes.Field:
                    FieldInfo providerField = member as FieldInfo;
                    if (!providerField.IsStatic)
                        instance = ProviderCache.GetInstanceOf(providerType);
                    providerObject = providerField.GetValue(instance);
                    break;
            }

            return providerObject;
        }
    }
}
