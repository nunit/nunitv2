using System;
using System.Collections;
using System.Reflection;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// ComparisonAdapter class centralizes all comparisons of
    /// values in NUnit, adapting to the use of any provided
    /// IComparer, IComparer&lt;T&gt; or Comparison&lt;T&gt;
    /// </summary>
    public class ComparisonAdapter
    {
        private IComparer comparer;

        /// <summary>
        /// Construct a default ComparisonAdapter
        /// </summary>
        public ComparisonAdapter() { }

        /// <summary>
        /// Construct a ComparisonAdapter for an IComparer
        /// </summary>
        public ComparisonAdapter(IComparer comparer)
        {
            this.comparer = comparer;
        }

        /// <summary>
        /// Compares two objects
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public virtual int Compare(object expected, object actual)
        {
            if (expected == null)
                throw new ArgumentException("Cannot compare using a null reference", "expected");

            if (actual == null)
                throw new ArgumentException("Cannot compare to null reference", "actual");

            if (comparer != null)
                return comparer.Compare(expected, actual);

            if (expected is IComparable)
                return Numerics.Compare((IComparable)expected, actual);
            else if (actual is IComparable)
                return -Numerics.Compare((IComparable)actual, expected);

            throw new ArgumentException("At least one object must implement IComparable to compare");
        }
    }

#if NET_2_0
    /// <summary>
    /// ComparisonAdapter&lt;T&gt; extends ComparisonAdapter and
    /// allows use of an IComparer&lt;T&gt; or Comparison&lt;T&gt;
    /// to actually perform the comparison.
    /// </summary>
    public class ComparisonAdapter<T> : ComparisonAdapter
    {
        private IComparer<T> genericComparer;
        private Comparison<T> comparison;

        /// <summary>
        /// Construct a ComparisonAdapter for an IComparer&lt;T&gt;
        /// </summary>
        public ComparisonAdapter(IComparer<T> comparer)
        {
            this.genericComparer = comparer;
        }

        /// <summary>
        /// Construct a ComparisonAdapter for a Comparison&lt;T&gt;
        /// </summary>
        public ComparisonAdapter(Comparison<T> comparer)
        {
            this.comparison = comparer;
        }

        /// <summary>
        /// Compare two objects
        /// </summary>
        public override int Compare(object expected, object actual)
        {
            if ( expected is T )
                return Compare((T)expected, actual);

            return base.Compare(expected, actual);
        }

        /// <summary>
        /// Compare a Type T to an object
        /// </summary>
        public int Compare(T expected, object actual)
        {
            if (genericComparer != null)
            {
                if (!typeof(T).IsAssignableFrom(actual.GetType()))
                    throw new ArgumentException("Cannot compare to " + actual.ToString());

                return genericComparer.Compare(expected, (T)actual);
            }

            if (comparison != null)
            {
                if (!typeof(T).IsAssignableFrom(actual.GetType()))
                    throw new ArgumentException("Cannot compare to " + actual.ToString());

                return comparison.Invoke(expected, (T)actual);
            }

            return base.Compare(expected, actual);
        }
    }
#endif
}
