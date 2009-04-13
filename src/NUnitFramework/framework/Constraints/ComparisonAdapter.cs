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
    public abstract class ComparisonAdapter
    {
        public static ComparisonAdapter Default
        {
            get { return new DefaultComparisonAdapter(); }
        }

        public static ComparisonAdapter For(IComparer comparer)
        {
            return new ComparerAdapter(comparer);
        }

#if NET_2_0
        public static ComparisonAdapter For<T>(IComparer<T> comparer)
        {
            return new ComparerAdapter<T>(comparer);
        }

        public static ComparisonAdapter For<T>(Comparison<T> comparer)
        {
            return new ComparisonAdapterForComparison<T>(comparer);
        }
#endif

        /// <summary>
        /// Construct a default ComparisonAdapter
        /// </summary>
        /// <summary>
        /// Compares two objects
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public abstract int Compare(object expected, object actual);

        class DefaultComparisonAdapter : ComparerAdapter
        {
            /// <summary>
            /// Construct a default ComparisonAdapter
            /// </summary>
            public DefaultComparisonAdapter() : base( NUnitComparer.Default ) { }
        }

        class ComparerAdapter : ComparisonAdapter
        {
            private IComparer comparer;

            /// <summary>
            /// Construct a ComparisonAdapter for an IComparer
            /// </summary>
            public ComparerAdapter(IComparer comparer)
            {
                this.comparer = comparer;
            }

            /// <summary>
            /// Compares two objects
            /// </summary>
            /// <param name="expected"></param>
            /// <param name="actual"></param>
            /// <returns></returns>
            public override int Compare(object expected, object actual)
            {
                return comparer.Compare(expected, actual);
            }
        }

#if NET_2_0
        /// <summary>
        /// ComparisonAdapter&lt;T&gt; extends ComparisonAdapter and
        /// allows use of an IComparer&lt;T&gt; or Comparison&lt;T&gt;
        /// to actually perform the comparison.
        /// </summary>
        class ComparerAdapter<T> : ComparisonAdapter
        {
            private IComparer<T> comparer;

            /// <summary>
            /// Construct a ComparisonAdapter for an IComparer&lt;T&gt;
            /// </summary>
            public ComparerAdapter(IComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            /// <summary>
            /// Compare a Type T to an object
            /// </summary>
            public override int Compare(object expected, object actual)
            {
                if (!typeof(T).IsAssignableFrom(expected.GetType()))
                    throw new ArgumentException("Cannot compare " + expected.ToString());

                if (!typeof(T).IsAssignableFrom(actual.GetType()))
                    throw new ArgumentException("Cannot compare to " + actual.ToString());

                return comparer.Compare((T)expected, (T)actual);
            }
        }

        class ComparisonAdapterForComparison<T> : ComparisonAdapter
        {
            private Comparison<T> comparison;

            /// <summary>
            /// Construct a ComparisonAdapter for a Comparison&lt;T&gt;
            /// </summary>
            public ComparisonAdapterForComparison(Comparison<T> comparer)
            {
                this.comparison = comparer;
            }

            /// <summary>
            /// Compare a Type T to an object
            /// </summary>
            public override int Compare(object expected, object actual)
            {
                if (!typeof(T).IsAssignableFrom(expected.GetType()))
                    throw new ArgumentException("Cannot compare " + expected.ToString());

                if (!typeof(T).IsAssignableFrom(actual.GetType()))
                    throw new ArgumentException("Cannot compare to " + actual.ToString());

                return comparison.Invoke((T)expected, (T)actual);
            }
        }
#endif
    }
}
