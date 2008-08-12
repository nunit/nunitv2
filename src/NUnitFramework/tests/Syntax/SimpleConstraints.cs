using System;

namespace NUnit.Framework.Syntax
{
    namespace Classic
    {
        [TestFixture]
        public class SimpleConstraints
        {
            [Test]
            public void IsNull()
            {
                Assert.IsNull(null);
            }

            [Test]
            public void IsNotNull()
            {
                Assert.IsNotNull(42);
            }

            [Test]
            public void IsTrue()
            {
                Assert.IsTrue(2 + 2 == 4);
            }

            [Test]
            public void IsFalse()
            {
                Assert.IsFalse(2 + 2 == 5);
            }

            [Test]
            public void IsNaN()
            {
                double d = double.NaN;
                float f = float.NaN;

                Assert.IsNaN(d);
                Assert.IsNaN(f);
            }

            [Test]
            public void EmptyStringTests()
            {
                Assert.IsEmpty("");
                Assert.IsNotEmpty("Hello!");
            }

            [Test]
            public void EmptyCollectionTests()
            {
                Assert.IsEmpty(new bool[0]);
                Assert.IsNotEmpty(new int[] { 1, 2, 3 });
            }
        }
    }

    namespace Helpers
    {
        [TestFixture]
        public class SimpleConstraintTests
        {
            [Test]
            public void IsNull()
            {
                Assert.That(null, Is.Null);
            }

            [Test]
            public void IsNotNull()
            {
                Assert.That(42, Is.Not.Null);
            }

            [Test]
            public void IsTrue()
            {
                Assert.That(2 + 2 == 4, Is.True);
                Assert.That(2 + 2 == 4);
            }

            [Test]
            public void IsFalse()
            {
                Assert.That(2 + 2 == 5, Is.False);
            }

            [Test]
            public void IsNaN()
            {
                double d = double.NaN;
                float f = float.NaN;

                Assert.That(d, Is.NaN);
                Assert.That(f, Is.NaN);
            }

            [Test]
            public void EmptyStringTests()
            {
                Assert.That("", Is.Empty);
                Assert.That("Hello!", Is.Not.Empty);
            }

            [Test]
            public void EmptyCollectionTests()
            {
                Assert.That(new bool[0], Is.Empty);
                Assert.That(new int[] { 1, 2, 3 }, Is.Not.Empty);
            }
        }
    }

    namespace Inherited
    {
        [TestFixture]
        public class SimpleConstraintTests : AssertionHelper
        {
            [Test]
            public void IsNull()
            {
                Expect(null, Null);
            }

            [Test]
            public void IsNotNull()
            {
                Expect(42, Not.Null);
            }

            [Test]
            public void IsTrue()
            {
                Expect(2 + 2 == 4, True);
                Expect(2 + 2 == 4);
            }

            [Test]
            public void IsFalse()
            {
                Expect(2 + 2 == 5, False);
            }

            [Test]
            public void IsNaN()
            {
                double d = double.NaN;
                float f = float.NaN;

                Expect(d, NaN);
                Expect(f, NaN);
            }

            [Test]
            public void EmptyStringTests()
            {
                Expect("", Empty);
                Expect("Hello!", Not.Empty);
            }

            [Test]
            public void EmptyCollectionTests()
            {
                Expect(new bool[0], Empty);
                Expect(new int[] { 1, 2, 3 }, Not.Empty);
            }
        }
    }
}
