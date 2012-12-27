// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.Collections;
#if CLR_2_0 || CLR_4_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// CollectionItemsEqualConstraint is the abstract base class for all
    /// collection constraints that apply some notion of item equality
    /// as a part of their operation.
    /// </summary>
    public abstract class CollectionItemsEqualConstraint : CollectionConstraint
    {
        // This is internal so that ContainsConstraint can set it
        // TODO: Figure out a way to avoid this indirection
        internal NUnitEqualityComparer comparer = NUnitEqualityComparer.Default;

        /// <summary>
        /// Construct an empty CollectionConstraint
        /// </summary>
        public CollectionItemsEqualConstraint() { }

        /// <summary>
        /// Construct a CollectionConstraint
        /// </summary>
        /// <param name="arg"></param>
        public CollectionItemsEqualConstraint(object arg) : base(arg) { }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public CollectionItemsEqualConstraint IgnoreCase
        {
            get
            {
                comparer.IgnoreCase = true;
                return this;
            }
        }

        /// <summary>
        /// Flag the constraint to use the supplied IComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public CollectionItemsEqualConstraint Using(IComparer comparer)
        {
            this.comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

#if CLR_2_0 || CLR_4_0
        /// <summary>
        /// Flag the constraint to use the supplied IComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public CollectionItemsEqualConstraint Using<T>(IComparer<T> comparer)
        {
            this.comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied Comparison object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public CollectionItemsEqualConstraint Using<T>(Comparison<T> comparer)
        {
            this.comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied IEqualityComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public CollectionItemsEqualConstraint Using(IEqualityComparer comparer)
        {
            this.comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied IEqualityComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public CollectionItemsEqualConstraint Using<T>(IEqualityComparer<T> comparer)
        {
            this.comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }
#endif
        #endregion

        /// <summary>
        /// Compares two collection members for equality
        /// </summary>
        protected bool ItemsEqual(object x, object y)
        {
            Tolerance tolerance = Tolerance.Zero;
            return comparer.AreEqual(x, y, ref tolerance);
        }

        /// <summary>
        /// Return a new CollectionTally for use in making tests
        /// </summary>
        /// <param name="c">The collection to be included in the tally</param>
        protected CollectionTally Tally(IEnumerable c)
        {
            return new CollectionTally(comparer, c);
        }
    }
}
