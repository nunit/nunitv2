// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class GreaterThanOrEqualConstraintTests : ComparisonConstraintTest
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = comparisonConstraint = new GreaterThanOrEqualConstraint(5);
            expectedDescription = "greater than or equal to 5";
            stringRepresentation = "<greaterthanorequal 5>";
        }

        internal object[] SuccessData = new object[] { 6, 5 };

        internal object[] FailureData = new object[] { 4 };

        internal string[] ActualValues = new string[] { "4" };

        internal object[] InvalidData = new object[] { null, "xxx" };

        [Test]
        public void CanCompareIComparables()
        {
            ClassWithIComparable expected = new ClassWithIComparable(0);
            ClassWithIComparable actual = new ClassWithIComparable(42);
            Assert.That(actual, Is.GreaterThanOrEqualTo(expected));
        }

#if CLR_2_0 || CLR_4_0
        [Test]
        public void CanCompareIComparablesOfT()
        {
            ClassWithIComparableOfT expected = new ClassWithIComparableOfT(0);
            ClassWithIComparableOfT actual = new ClassWithIComparableOfT(42);
            Assert.That(actual, Is.GreaterThanOrEqualTo(expected));
        }
#endif
    }
}
