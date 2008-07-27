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
