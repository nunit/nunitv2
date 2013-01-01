// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class LessThanConstraintTests : ComparisonConstraintTest
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = comparisonConstraint = new LessThanConstraint(5);
            expectedDescription = "less than 5";
            stringRepresentation = "<lessthan 5>";
        }

        internal object[] SuccessData = new object[] { 4, 4.999 };

        internal object[] FailureData = new object[] { 6, 5 };

        internal string[] ActualValues = new string[] { "6", "5" };

        internal object[] InvalidData = new object[] { null, "xxx" };

        [Test]
        public void CanCompareIComparables()
        {
            ClassWithIComparable expected = new ClassWithIComparable(42);
            ClassWithIComparable actual = new ClassWithIComparable(0);
            Assert.That(actual, Is.LessThan(expected));
        }

#if CLR_2_0 || CLR_4_0
        [Test]
        public void CanCompareIComparablesOfT()
        {
            ClassWithIComparableOfT expected = new ClassWithIComparableOfT(42);
            ClassWithIComparableOfT actual = new ClassWithIComparableOfT(0);
            Assert.That(actual, Is.LessThan(expected));
        }
#endif
    }
}
