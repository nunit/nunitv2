// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using System.Text;

namespace NUnit.Core.Builders
{
    class ProviderCache
    {
        private static IDictionary instances = new Hashtable();

        public static object GetInstanceOf(Type providerType)
        {
            object instance = instances[providerType];
            return instance == null
                ? instances[providerType] = Reflect.Construct(providerType)
                : instance;
        }

        public static void Clear()
        {
            foreach (object key in instances.Keys)
            {
                IDisposable provider = instances[key] as IDisposable;
                if (provider != null)
                    provider.Dispose();
                instances.Remove(key);
            }
        }
    }
}
