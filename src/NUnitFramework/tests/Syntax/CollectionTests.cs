using System;
using System.Collections;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Syntax
{
    namespace Classic
    {
        public class CollectionTests
        {
            [Test]
            public void AllItemsTests()
            {
                object[] ints = new object[] { 1, 2, 3, 4 };
                object[] doubles = new object[] { 0.99, 2.1, 3.0, 4.05 };
                object[] strings = new object[] { "abc", "bad", "cab", "bad", "dad" };

                CollectionAssert.AllItemsAreNotNull(ints);
                CollectionAssert.AllItemsAreInstancesOfType(ints, typeof(int));
                CollectionAssert.AllItemsAreInstancesOfType(strings, typeof(string));
                CollectionAssert.AllItemsAreUnique(ints);

            }

            [Test]
            public void CollectionContainsTests()
            {
                int[] iarray = new int[] { 1, 2, 3 };
                string[] sarray = new string[] { "a", "b", "c" };

                Assert.Contains(3, iarray);
                Assert.Contains("b", sarray);
                CollectionAssert.Contains(iarray, 3);
                CollectionAssert.Contains(sarray, "b");
                CollectionAssert.DoesNotContain(sarray, "x");
                // Showing that Contains uses object equality
                CollectionAssert.DoesNotContain(iarray, 1.0d);
            }

            [Test]
            public void CollectionEquivalenceTests()
            {
                int[] ints1to5 = new int[] { 1, 2, 3, 4, 5 };
                int[] twothrees = new int[] { 1, 2, 3, 3, 4, 5 };
                int[] twofours = new int[] { 1, 2, 3, 4, 4, 5 };

                CollectionAssert.AreEquivalent(new int[] { 2, 1, 4, 3, 5 }, ints1to5);
                CollectionAssert.AreNotEquivalent(new int[] { 2, 2, 4, 3, 5 }, ints1to5);
                CollectionAssert.AreNotEquivalent(new int[] { 2, 4, 3, 5 }, ints1to5);
                CollectionAssert.AreNotEquivalent(new int[] { 2, 2, 1, 1, 4, 3, 5 }, ints1to5);
                CollectionAssert.AreNotEquivalent(twothrees, twofours);
            }

            [Test]
            public void SubsetTests()
            {
                int[] ints1to5 = new int[] { 1, 2, 3, 4, 5 };

                CollectionAssert.IsSubsetOf(new int[] { 1, 3, 5 }, ints1to5);
                CollectionAssert.IsSubsetOf(new int[] { 1, 2, 3, 4, 5 }, ints1to5);
                CollectionAssert.IsNotSubsetOf(new int[] { 2, 4, 6 }, ints1to5);
                CollectionAssert.IsNotSubsetOf(new int[] { 1, 2, 2, 2, 5 }, ints1to5);
            }

            [Test]
            public void OrderedTests()
            {
                int[] ints = new int[] { 1, 3, 5 };
                string[] strings = new string[] { "a", "b", "c" };

                CollectionAssert.IsOrdered(ints);
                CollectionAssert.IsOrdered(strings);
            }
        }
    }

    namespace Helpers
    {
        public class CollectionTests
        {
            [Test]
            public void AllItemsTests()
            {
                object[] ints = new object[] { 1, 2, 3, 4 };
                object[] doubles = new object[] { 0.99, 2.1, 3.0, 4.05 };
                object[] strings = new object[] { "abc", "bad", "cab", "bad", "dad" };

                Assert.That(ints, Is.All.Not.Null);
                Assert.That(ints, Has.None.Null);
                Assert.That(ints, Is.All.InstanceOfType(typeof(int)));
                Assert.That(ints, Has.All.InstanceOfType(typeof(int)));
                Assert.That(strings, Is.All.InstanceOfType(typeof(string)));
                Assert.That(strings, Has.All.InstanceOfType(typeof(string)));
                Assert.That(ints, Is.Unique);
                // Only available using new syntax
                Assert.That(strings, Is.Not.Unique);
                Assert.That(ints, Is.All.GreaterThan(0));
                Assert.That(ints, Has.All.GreaterThan(0));
                Assert.That(ints, Has.None.LessThanOrEqualTo(0));
                Assert.That(strings, Text.All.Contains("a"));
                Assert.That(strings, Has.All.Contains("a"));
                Assert.That(strings, Has.Some.StartsWith("ba"));
                Assert.That(strings, Has.Some.Property("Length", 3));
                Assert.That(strings, Has.Some.StartsWith("BA").IgnoreCase);
                Assert.That(doubles, Has.Some.EqualTo(1.0).Within(.05));
            }

            [Test]
            public void SomeItemTests()
            {
                object[] mixed = new object[] { 1, 2, "3", null, "four", 100 };
                object[] strings = new object[] { "abc", "bad", "cab", "bad", "dad" };

                Assert.That(mixed, Has.Some.Null);
                Assert.That(mixed, Has.Some.InstanceOfType(typeof(int)));
                Assert.That(mixed, Has.Some.InstanceOfType(typeof(string)));
                Assert.That(strings, Has.Some.StartsWith("ba"));
                Assert.That(strings, Has.Some.Not.StartsWith("ba"));
            }

            [Test]
            public void NoItemTests()
            {
                object[] ints = new object[] { 1, 2, 3, 4, 5 };
                object[] strings = new object[] { "abc", "bad", "cab", "bad", "dad" };

                Assert.That(ints, Has.None.Null);
                Assert.That(ints, Has.None.InstanceOfType(typeof(string)));
                Assert.That(ints, Has.None.GreaterThan(99));
                Assert.That(strings, Has.None.StartsWith("qu"));
            }

            [Test]
            public void CollectionContainsTests()
            {
                int[] iarray = new int[] { 1, 2, 3 };
                string[] sarray = new string[] { "a", "b", "c" };

                Assert.That(iarray, Has.Member(3));
                Assert.That(sarray, Has.Member("b"));
                Assert.That(sarray, Has.No.Member("x"));
                // Showing that Contains uses object equality
                Assert.That(iarray, Has.No.Member(1.0d));

                // Only available using the new syntax
                // Note that EqualTo and SameAs do NOT give
                // identical results to Contains because 
                // Contains uses Object.Equals()
                Assert.That(iarray, Has.Some.EqualTo(3));
                Assert.That(iarray, Has.Member(3));
                Assert.That(sarray, Has.Some.EqualTo("b"));
                Assert.That(sarray, Has.None.EqualTo("x"));
                Assert.That(iarray, Has.None.SameAs(1.0d));
                Assert.That(iarray, Has.All.LessThan(10));
                Assert.That(sarray, Has.All.Length(1));
                Assert.That(sarray, Has.None.Property("Length").GreaterThan(3));
            }

            [Test]
            public void CollectionEquivalenceTests()
            {
                int[] ints1to5 = new int[] { 1, 2, 3, 4, 5 };
                int[] twothrees = new int[] { 1, 2, 3, 3, 4, 5 };
                int[] twofours = new int[] { 1, 2, 3, 4, 4, 5 };

                Assert.That(new int[] { 2, 1, 4, 3, 5 }, Is.EquivalentTo(ints1to5));
                Assert.That(new int[] { 2, 2, 4, 3, 5 }, Is.Not.EquivalentTo(ints1to5));
                Assert.That(new int[] { 2, 4, 3, 5 }, Is.Not.EquivalentTo(ints1to5));
                Assert.That(new int[] { 2, 2, 1, 1, 4, 3, 5 }, Is.Not.EquivalentTo(ints1to5));
            }

            [Test]
            public void SubsetTests()
            {
                int[] ints1to5 = new int[] { 1, 2, 3, 4, 5 };

                Assert.That(new int[] { 1, 3, 5 }, Is.SubsetOf(ints1to5));
                Assert.That(new int[] { 1, 2, 3, 4, 5 }, Is.SubsetOf(ints1to5));
                Assert.That(new int[] { 2, 4, 6 }, Is.Not.SubsetOf(ints1to5));
            }

            [Test]
            public void OrderedTests()
            {
                int[] ints = new int[] { 1, 3, 5 };
                string[] strings = new string[] { "a", "b", "c" };

                Assert.That(ints, Is.Ordered());
                Assert.That(strings, Is.Ordered());
            }
        }
    }

    namespace Inherited
    {
        public class CollectionTests : AssertionHelper
        {
            [Test]
            public void AllItemsTests()
            {
                object[] ints = new object[] { 1, 2, 3, 4 };
                object[] doubles = new object[] { 0.99, 2.1, 3.0, 4.05 };
                object[] strings = new object[] { "abc", "bad", "cab", "bad", "dad" };

                Expect(ints, All.Not.Null);
                Expect(ints, None.Null);
                Expect(ints, All.InstanceOfType(typeof(int)));
                Expect(strings, All.InstanceOfType(typeof(string)));
                Expect(ints, Unique);
                //// Only available using new syntax
                Expect(strings, Not.Unique);
                Expect(ints, All.GreaterThan(0));
                Expect(ints, None.LessThanOrEqualTo(0));
                Expect(strings, All.Contains("a"));
                Expect(strings, Some.StartsWith("ba"));
                Expect(strings, Some.StartsWith("BA").IgnoreCase);
                Expect(doubles, Some.EqualTo(1.0).Within(.05));
            }

            [Test]
            public void SomeItemTests()
            {
                object[] mixed = new object[] { 1, 2, "3", null, "four", 100 };
                object[] strings = new object[] { "abc", "bad", "cab", "bad", "dad" };

                Expect(mixed, Some.Null);
                Expect(mixed, Some.InstanceOfType(typeof(int)));
                Expect(mixed, Some.InstanceOfType(typeof(string)));
                Expect(strings, Some.StartsWith("ba"));
                Expect(strings, Some.Not.StartsWith("ba"));
            }

            [Test]
            public void NoItemTests()
            {
                object[] ints = new object[] { 1, 2, 3, 4, 5 };
                object[] strings = new object[] { "abc", "bad", "cab", "bad", "dad" };

                Expect(ints, None.Null);
                Expect(ints, None.InstanceOfType(typeof(string)));
                Expect(ints, None.GreaterThan(99));
                Expect(strings, None.StartsWith("qu"));
            }

            [Test]
            public void CollectionContainsTests()
            {
                int[] iarray = new int[] { 1, 2, 3 };
                string[] sarray = new string[] { "a", "b", "c" };

                // Inherited syntax
                Expect(iarray, Contains(3));
                Expect(sarray, Contains("b"));
                Expect(sarray, Not.Contains("x"));

                // Only available using new syntax
                // Note that EqualTo and SameAs do NOT give
                // identical results to Contains because 
                // Contains uses Object.Equals()
                Expect(iarray, Some.EqualTo(3));
                Expect(sarray, Some.EqualTo("b"));
                Expect(sarray, None.EqualTo("x"));
                Expect(iarray, All.LessThan(10));
                Expect(sarray, All.Length(1));
                Expect(sarray, None.Property("Length").GreaterThan(3));
            }

            [Test]
            public void CollectionEquivalenceTests()
            {
                int[] ints1to5 = new int[] { 1, 2, 3, 4, 5 };
                int[] twothrees = new int[] { 1, 2, 3, 3, 4, 5 };
                int[] twofours = new int[] { 1, 2, 3, 4, 4, 5 };

                Expect(new int[] { 2, 1, 4, 3, 5 }, EquivalentTo(ints1to5));
                Expect(new int[] { 2, 2, 4, 3, 5 }, Not.EquivalentTo(ints1to5));
                Expect(new int[] { 2, 4, 3, 5 }, Not.EquivalentTo(ints1to5));
                Expect(new int[] { 2, 2, 1, 1, 4, 3, 5 }, Not.EquivalentTo(ints1to5));
            }

            [Test]
            public void SubsetTests()
            {
                int[] ints1to5 = new int[] { 1, 2, 3, 4, 5 };

                Expect(new int[] { 1, 3, 5 }, SubsetOf(ints1to5));
                Expect(new int[] { 1, 2, 3, 4, 5 }, SubsetOf(ints1to5));
                Expect(new int[] { 2, 4, 6 }, Not.SubsetOf(ints1to5));
            }

            [Test]
            public void OrderedTests()
            {
                int[] ints = new int[] { 1, 3, 5 };
                string[] strings = new string[] { "a", "b", "c" };

                Expect(ints, Ordered());
                Expect(strings, Ordered());
            }
        }
    }
}
