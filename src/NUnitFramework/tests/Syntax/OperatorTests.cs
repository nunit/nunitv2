using System;

namespace NUnit.Framework.Tests.Syntax
{
    namespace Helpers
    {
        public class OperatorTests
        {
            [Test]
            public void NotTests()
            {
                Assert.That(42, Is.Not.Null);
                Assert.That(42, Is.Not.True);
                Assert.That(42, Is.Not.False);
                Assert.That(2.5, Is.Not.NaN);
                Assert.That(2 + 2, Is.Not.EqualTo(3));
                Assert.That(2 + 2, Is.Not.Not.EqualTo(4));
                Assert.That(2 + 2, Is.Not.Not.Not.EqualTo(5));
            }

            [Test]
            public void AndTests()
            {
                Assert.That(7, Is.GreaterThan(5).And.LessThan(10));
                Assert.That(7, Is.Not.Null.And.Not.LessThan(5).And.Not.GreaterThan(10));
            }

            [Test]
            public void OrTests()
            {
                Assert.That(99, Is.LessThan(5).Or.GreaterThan(10));
                Assert.That(99, Is.LessThan(5).Or.GreaterThan(10).Or.EqualTo(7));
                Assert.That(3, Is.LessThan(5).Or.GreaterThan(10).Or.EqualTo(7));
                Assert.That(7, Is.LessThan(5).Or.GreaterThan(10).Or.EqualTo(7));
            }

            [Test]
            public void PrecedenceTests()
            {
                Assert.That(7, Is.LessThan(100).And.GreaterThan(0).Or.EqualTo(999));
                Assert.That(7, Is.EqualTo(999).Or.GreaterThan(0).And.LessThan(100));
                Assert.That(999, Is.LessThan(100).And.GreaterThan(0).Or.EqualTo(999));
                Assert.That(999, Is.EqualTo(999).Or.GreaterThan(0).And.LessThan(100));
                //Assert.That("Hell", StartsWith("H").Or.StartsWith("X") & Length(5));
            }
        }
    }

    namespace Inherited
    {
        public class OperatorTests : AssertionHelper
        {
            [Test]
            public void NotTests()
            {
                Expect(42, Not.Null);
                Expect(42, Not.True);
                Expect(42, Not.False);
                Expect(2.5, Not.NaN);
                Expect(2 + 2, Not.EqualTo(3));
                Expect(2 + 2, Not.Not.EqualTo(4));
                Expect(2 + 2, Not.Not.Not.EqualTo(5));
            }

            [Test]
            public void AndTests()
            {
                Expect(7, GreaterThan(5).And.LessThan(10));
                Expect(7, Not.Null.And.Not.LessThan(5).And.Not.GreaterThan(10));
            }

            [Test]
            public void OrTests()
            {
                Expect(99, LessThan(5).Or.GreaterThan(10));
                Expect(99, LessThan(5).Or.GreaterThan(10).Or.EqualTo(7));
                Expect(3, LessThan(5).Or.GreaterThan(10).Or.EqualTo(7));
                Expect(7, LessThan(5).Or.GreaterThan(10).Or.EqualTo(7));
            }

            [Test]
            public void PrecedenceTests()
            {
                Expect(7, LessThan(100).And.GreaterThan(0).Or.EqualTo(999));
                Expect(7, EqualTo(999).Or.GreaterThan(0).And.LessThan(100));
                Expect(999, LessThan(100).And.GreaterThan(0).Or.EqualTo(999));
                Expect(999, EqualTo(999).Or.GreaterThan(0).And.LessThan(100));
                //Expect("Hell", StartsWith("H").Or.StartsWith("X") & Length(5));
            }
        }
    }
}
