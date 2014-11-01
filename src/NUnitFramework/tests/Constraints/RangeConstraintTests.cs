// ****************************************************************
// Copyright 2011, Charlie Poole
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
    [TestFixture]
    public class RangeConstraintTests : ConstraintTestBaseWithArgumentException
    {
#if CLR_2_0 || CLR_4_0
        RangeConstraint<int> rangeConstraint;
#else
        RangeConstraint rangeConstraint;
#endif

        [SetUp]
        public void SetUp()
        {
#if CLR_2_0 || CLR_4_0
            theConstraint = rangeConstraint = new RangeConstraint<int>(5, 42);
#else
            theConstraint = rangeConstraint = new RangeConstraint(5, 42);
#endif
            expectedDescription = "in range (5,42)";
            stringRepresentation = "<range 5 42>";
        }

        internal object[] SuccessData = new object[] { 5, 23, 42 };

        internal object[] FailureData = new object[] { 4, 43 };

        internal string[] ActualValues = new string[] { "4", "43" };

        internal object[] InvalidData = new object[] { null, "xxx" };

        [Test]
        public void UsesProvidedIComparer()
        {
            MyComparer comparer = new MyComparer();
            Assert.That(rangeConstraint.Using(comparer).Matches(19));
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
            Assert.That(rangeConstraint.Using(comparer).Matches(19));
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
            Assert.That(rangeConstraint.Using(new Comparison<int>(comparer.Compare)).Matches(19));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [Test]
        [ExpectedException( typeof( ArgumentException ))]
        public void ShouldThrowExceptionIfFromIsLessThanTo()
        {
            var comparer = new MyComparer<int>();
            rangeConstraint = new RangeConstraint<int>( 42, 5 );
            rangeConstraint.Using(comparer).Matches(19);
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
            Assert.That(rangeConstraint.Using(comparer).Matches(19));
        }
#endif
#endif
    }
}
