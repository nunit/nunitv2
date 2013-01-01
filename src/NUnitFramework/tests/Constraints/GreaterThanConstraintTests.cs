// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class GreaterThanConstraintTests : ComparisonConstraintTest
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = comparisonConstraint = new GreaterThanConstraint(5);
            expectedDescription = "greater than 5";
            stringRepresentation = "<greaterthan 5>";
        }

        internal object[] SuccessData = new object[] { 6, 5.001 };

        internal object[] FailureData = new object[] { 4, 5 };

        internal string[] ActualValues = new string[] { "4", "5" };

        internal object[] InvalidData = new object[] { null, "xxx" };

        [Test]
        public void CanCompareIComparables()
        {
            ClassWithIComparable expected = new ClassWithIComparable(0);
            ClassWithIComparable actual = new ClassWithIComparable(42);
            Assert.That(actual, Is.GreaterThan(expected));
        }

#if CLR_2_0 || CLR_4_0
        [Test]
        public void CanCompareIComparablesOfT()
        {
            ClassWithIComparableOfT expected = new ClassWithIComparableOfT(0);
            ClassWithIComparableOfT actual = new ClassWithIComparableOfT(42);
            Assert.That(actual, Is.GreaterThan(expected));
        }
#endif
    }
}
