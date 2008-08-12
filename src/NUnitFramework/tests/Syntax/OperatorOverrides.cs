using System;
using NUnit.Framework.Constraints;

namespace NUnit.Framework.Syntax
{
    namespace Helpers
    {
        public class OperatorOverrides : AssertionHelper
        {
            [Test]
            public void NotOperator()
            {
                Assert.That(42, !Is.Null);
            }

            [Test]
            public void AndOperator()
            {
                Assert.That(7, Is.GreaterThan(5) & Is.LessThan(10));
            }

            [Test]
            public void OrOperator()
            {
                Assert.That(3, Is.LessThan(5) | Is.GreaterThan(10));
            }

            [Test]
            public void ComplexTests()
            {
                Assert.That(7, Is.Not.Null & Is.Not.LessThan(5) & Is.Not.GreaterThan(10));

                Assert.That(7, !Is.Null & !Is.LessThan(5) & !Is.GreaterThan(10));

                // TODO: Remove #if when mono compiler can handle null
#if MONO
            Constraint x = null;
            Assert.That(7, !x & !Is.LessThan(5) & !Is.GreaterThan(10));
#else
                Assert.That(7, !(Constraint)null & !Is.LessThan(5) & !Is.GreaterThan(10));
#endif
            }
        }
    }

    namespace Inherited
    {
        public class OperatorOverrides : AssertionHelper
        {
            [Test]
            public void NotOperator()
            {
                Expect(42, !Null);
            }

            [Test]
            public void AndOperator()
            {
                Expect(7, GreaterThan(5) & LessThan(10));
            }

            [Test]
            public void OrOperator()
            {
                Expect(3, LessThan(5) | GreaterThan(10));
            }

            [Test]
            public void ComplexTests()
            {
                Expect(7, Not.Null & Not.LessThan(5) & Not.GreaterThan(10));

                Expect(7, !Null & !LessThan(5) & !GreaterThan(10));

                // TODO: Remove #if when mono compiler can handle null
#if MONO
            Constraint x = null;
			Expect(7, !x & !LessThan(5) & !GreaterThan(10));
#else
                Expect(7, !(Constraint)null & !LessThan(5) & !GreaterThan(10));
#endif
            }
        }
    }
}
