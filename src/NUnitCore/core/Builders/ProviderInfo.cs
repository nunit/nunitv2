// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
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
        private IEnumerable provider;
        private string message;

        public ProviderInfo(Type providerType, string providerName)
        {
            if (providerType == null)
                throw new ArgumentNullException("providerType");
            if (providerName == null)
                throw new ArgumentNullException("providerName");

            this.providerType = providerType;
            this.providerName = providerName;
        }

        public IEnumerable Provider
        {
            get
            {
                // Don't try to populate source more than once
                if (provider == null && message == null)
                {
                    MemberInfo[] members = providerType.GetMember(
                        providerName,
                        MemberTypes.Field | MemberTypes.Method | MemberTypes.Property,
                        BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (members.Length == 0)
                        message = string.Format(
                            "Unable to locate {0}.{1}", providerType.FullName, providerName);
                    else if (members.Length > 1)
                        message = string.Format(
                            "{0}.{1} is ambiguous", providerType.FullName, providerName);
                    else
                    {
                        object providerObject = GetProviderObjectFromMember(members[0]);

                        if (providerObject == null)
                            message = string.Format("Provider {0} returned null", providerName);
                        else
                        {
                            provider = providerObject as IEnumerable;
                            if (provider == null)
                                message = string.Format("Provider {0} does not implement IEnumerable", providerName);
                        }
                    }
                }

                return provider;
            }
        }

        public string Message
        {
            get { return message; }
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
