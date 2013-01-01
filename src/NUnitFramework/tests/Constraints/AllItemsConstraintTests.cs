// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using System.Collections;
using NUnit.Framework.Tests;

#if CLR_2_0 || CLR_4_0
using RangeConstraint = NUnit.Framework.Constraints.RangeConstraint<int>;
#endif

namespace NUnit.Framework.Constraints
{
    public class AllItemsTests : MessageChecker
    {
        [Test]
        public void AllItemsAreNotNull()
        {
            object[] c = new object[] { 1, "hello", 3, Environment.OSVersion };
            Assert.That(c, new AllItemsConstraint(Is.Not.Null));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AllItemsAreNotNullFails()
        {
            object[] c = new object[] { 1, "hello", null, 3 };
            expectedMessage =
                TextMessageWriter.Pfx_Expected + "all items not null" + Environment.NewLine +
                TextMessageWriter.Pfx_Actual + "< 1, \"hello\", null, 3 >" + Environment.NewLine;
            Assert.That(c, new AllItemsConstraint(new NotConstraint(new EqualConstraint(null))));
        }

        [Test]
        public void AllItemsAreInRange()
        {
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(new RangeConstraint(10, 100)));
        }

        [Test]
        public void AllItemsAreInRange_UsingIComparer()
        {
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(new RangeConstraint(10, 100).Using(Comparer.Default)));
        }

#if CLR_2_0 || CLR_4_0
        [Test]
        public void AllItemsAreInRange_UsingIComparerOfT()
        {
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(new RangeConstraint(10, 100).Using(Comparer.Default)));
        }

        [Test]
        public void AllItemsAreInRange_UsingComparisonOfT()
        {
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(new RangeConstraint(10, 100).Using(Comparer.Default)));
        }
#endif

        [Test, ExpectedException(typeof(AssertionException))]
        public void AllItemsAreInRangeFailureMessage()
        {
            int[] c = new int[] { 12, 27, 19, 32, 107, 99, 26 };
            expectedMessage =
                TextMessageWriter.Pfx_Expected + "all items in range (10,100)" + Environment.NewLine +
                TextMessageWriter.Pfx_Actual + "< 12, 27, 19, 32, 107, 99, 26 >" + Environment.NewLine;
            Assert.That(c, new AllItemsConstraint(new RangeConstraint(10, 100)));
        }

        [Test]
        public void AllItemsAreInstancesOfType()
        {
            object[] c = new object[] { 'a', 'b', 'c' };
            Assert.That(c, new AllItemsConstraint(new InstanceOfTypeConstraint(typeof(char))));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AllItemsAreInstancesOfTypeFailureMessage()
        {
            object[] c = new object[] { 'a', "b", 'c' };
            expectedMessage =
                TextMessageWriter.Pfx_Expected + "all items instance of <System.Char>" + Environment.NewLine +
                TextMessageWriter.Pfx_Actual + "< 'a', \"b\", 'c' >" + Environment.NewLine;
            Assert.That(c, new AllItemsConstraint(new InstanceOfTypeConstraint(typeof(char))));
        }
    }
}
