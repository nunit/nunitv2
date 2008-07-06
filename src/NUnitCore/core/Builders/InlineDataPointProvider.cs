using System;
using System.Reflection;
using System.Collections;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    public class InlineDataPointProvider : IDataPointProvider
    {
        private static readonly string ValuesAttribute = "NUnit.Framework.ValuesAttribute";

        private static readonly string ValuesProperty = "Values";

        #region IDataPointProvider Members

        public bool HasDataFor(ParameterInfo parameter)
        {
            return Reflect.HasAttribute(parameter, ValuesAttribute, false);
        }

        public IEnumerable GetDataFor(ParameterInfo parameter)
        {
#if !NET_2_0
			ArrayList testdata = new ArrayList();
#endif
            Attribute attr = Reflect.GetAttribute(parameter, ValuesAttribute, false);
            if (attr != null)
            {
                IEnumerable source = Reflect.GetPropertyValue(attr, ValuesProperty) as IEnumerable;
                if (source != null)
                    foreach (object o in (IEnumerable)Reflect.GetPropertyValue(attr, ValuesProperty))
#if NET_2_0
                        yield return o;
#else
						testdata.Add(o);
#endif
            }
#if !NET_2_0
			return testdata;
#endif
        }

        #endregion
    }
}
