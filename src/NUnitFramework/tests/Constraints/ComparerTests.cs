using System;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class ComparerTests
    {
        [TestCase(4, 4)]
        [TestCase(4.0d, 4.0d)]
        [TestCase(4.0f, 4.0f)]
        [TestCase(4, 4.0d)]
        [TestCase(4, 4.0f)]
        [TestCase(4.0d, 4)]
        [TestCase(4.0d, 4.0f)]
        [TestCase(4.0f, 4)]
        [TestCase(4.0f, 4.0d)]
        [TestCase(null, null)]
        public void EqualItems(object x, object y)
        {
            Assert.That(NUnitComparer.Default.Compare(x, y) == 0);
            Assert.That(NUnitEqualityComparer.Default.Equals(x, y));
        }

        [TestCase(4, 2)]
        [TestCase(4.0d, 2.0d)]
        [TestCase(4.0f, 2.0f)]
        [TestCase(4, 2.0d)]
        [TestCase(4, 2.0f)]
        [TestCase(4.0d, 2)]
        [TestCase(4.0d, 2.0f)]
        [TestCase(4.0f, 2)]
        [TestCase(4.0f, 2.0d)]
        [TestCase(4, null)]
        public void UnequalItems(object greater, object lesser)
        {
            Assert.That(NUnitComparer.Default.Compare(greater, lesser) > 0);
            Assert.That(NUnitComparer.Default.Compare(lesser, greater) < 0);
            Assert.False(NUnitEqualityComparer.Default.Equals( greater, lesser ));
            Assert.False(NUnitEqualityComparer.Default.Equals( lesser, greater ));
        }

        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.NaN)]
        [TestCase(float.PositiveInfinity)]
        [TestCase(float.NegativeInfinity)]
        [TestCase(float.NaN)]
        public void SpecialValues(object x)
        {
            Assert.That(NUnitEqualityComparer.Default.Equals(x, x));
        }
    }
}
