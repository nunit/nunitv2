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
            Attribute attr = Reflect.GetAttribute(parameter, ValuesAttribute, false);
            return attr != null
                ? Reflect.GetPropertyValue(attr, ValuesProperty) as IEnumerable
                : null;
        }

        #endregion
    }
}
