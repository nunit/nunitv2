using System;
using System.IO;
using System.Reflection;
using System.Collections;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    public class NUnitComparer : IComparer
    {
        public static NUnitComparer Default
        {
            get { return new NUnitComparer(); }
        }

        public int Compare(object x, object y)
        {
            if (x == null)
                return y == null ? 0 : -1;
            else if (y == null)
                return +1;

            if (Numerics.IsNumericType(x) && Numerics.IsNumericType(y))
                return Numerics.Compare(x, y);

            if (x is IComparable)
                return ((IComparable)x).CompareTo(y);

            if (y is IComparable)
                return -((IComparable)y).CompareTo(x);

            Type xType = x.GetType();
            Type yType = y.GetType();

            MethodInfo method = xType.GetMethod("CompareTo", new Type[] { yType });
            if (method != null)
                return (int)method.Invoke(x, new object[] { y });

            method = yType.GetMethod("CompareTo", new Type[] { xType });
            if (method != null)
                return -(int)method.Invoke(y, new object[] { x });

            throw new ArgumentException("Neither value implements IComparable or IComparable<T>");
        }
    }
}
