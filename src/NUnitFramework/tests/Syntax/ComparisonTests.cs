using System;

namespace NUnit.Framework.Syntax
{
    namespace Classic
    {
        public class ComparisonTests
        {
            [Test]
            public void SimpleComparisonTests()
            {
                Assert.Greater(7, 3);
                Assert.GreaterOrEqual(7, 3);
                Assert.GreaterOrEqual(7, 7);
                Assert.Less(3, 7);
                Assert.LessOrEqual(3, 7);
                Assert.LessOrEqual(3, 3);
            }
        }
    }

    namespace Helpers
    {
        public class ComparisonTests : AssertionHelper
        {
            [Test]
            public void SimpleComparisonTests()
            {
                Assert.That(7, Is.GreaterThan(3));
                Assert.That(7, Is.GreaterThanOrEqualTo(3));
                Assert.That(7, Is.AtLeast(3));
                Assert.That(7, Is.GreaterThanOrEqualTo(7));
                Assert.That(7, Is.AtLeast(7));
                Assert.That(3, Is.LessThan(7));
                Assert.That(3, Is.LessThanOrEqualTo(7));
                Assert.That(3, Is.AtMost(7));
                Assert.That(3, Is.LessThanOrEqualTo(3));
                Assert.That(3, Is.AtMost(3));
            }
        }
    }

    namespace Inherited
    {
        public class ComparisonTests : AssertionHelper
        {
            [Test]
            public void SimpleComparisonTests()
            {
                Expect(7, GreaterThan(3));
                Expect(7, GreaterThanOrEqualTo(3));
                Expect(7, AtLeast(3));
                Expect(7, GreaterThanOrEqualTo(7));
                Expect(7, AtLeast(7));
                Expect(3, LessThan(7));
                Expect(3, LessThanOrEqualTo(7));
                Expect(3, AtMost(7));
                Expect(3, LessThanOrEqualTo(3));
                Expect(3, AtMost(3));
            }
        }
    }
}
