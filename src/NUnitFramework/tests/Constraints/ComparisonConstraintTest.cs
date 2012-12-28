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
    #region ComparisonConstraintTest
    public abstract class ComparisonConstraintTest : ConstraintTestBaseWithArgumentException
    {
        protected ComparisonConstraint comparisonConstraint;

        [Test]
        public void UsesProvidedIComparer()
        {
            MyComparer comparer = new MyComparer();
            comparisonConstraint.Using(comparer).Matches(0);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparer : IComparer
        {
            public bool Called;

            public int Compare(object x, object y)
            {
                Called = true;
                return Comparer.Default.Compare(x, y);
            }
        }

#if CLR_2_0 || CLR_4_0
        [Test]
        public void UsesProvidedComparerOfT()
        {
            MyComparer<int> comparer = new MyComparer<int>();
            comparisonConstraint.Using(comparer).Matches(0);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparer<T> : IComparer<T>
        {
            public bool Called;

            public int Compare(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y);
            }
        }

        [Test]
        public void UsesProvidedComparisonOfT()
        {
            MyComparison<int> comparer = new MyComparison<int>();
            comparisonConstraint.Using(new Comparison<int>(comparer.Compare)).Matches(0);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparison<T>
        {
            public bool Called;

            public int Compare(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y);
            }
        }

#if CS_3_0 || CS_4_0 || CS_5_0
        [Test]
        public void UsesProvidedLambda()
        {
            Comparison<int> comparer = (x, y) => x.CompareTo(y);
            comparisonConstraint.Using(comparer).Matches(0);
        }
#endif
#endif
    }
    #endregion

    #region Test Classes
    class ClassWithIComparable : IComparable
    {
        private int val;

        public ClassWithIComparable(int val)
        {
            this.val = val;
        }

        public int CompareTo(object x)
        {
            ClassWithIComparable other = x as ClassWithIComparable;
            if (x is ClassWithIComparable)
                return val.CompareTo(other.val);

            throw new ArgumentException();
        }
    }

#if CLR_2_0 || CLR_4_0
    class ClassWithIComparableOfT : IComparable<ClassWithIComparableOfT>
    {
        private int val;

        public ClassWithIComparableOfT(int val)
        {
            this.val = val;
        }

        public int CompareTo(ClassWithIComparableOfT other)
        {
            return val.CompareTo(other.val);
        }
    }
#endif
    #endregion
}