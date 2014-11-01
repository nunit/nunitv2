// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using System.Collections;
using NUnit.Framework.Tests;

#if CLR_2_0 || CLR_4_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    public class CollectionEquivalentTests
    {
        [Test]
        public void EqualCollectionsAreEquivalent()
        {
            ICollection set1 = new ICollectionAdapter("x", "y", "z");
            ICollection set2 = new ICollectionAdapter("x", "y", "z");

            Assert.That(new CollectionEquivalentConstraint(set1).Matches(set2));
        }

        [Test]
        public void WorksWithCollectionsOfArrays()
        {
            byte[] array1 = new byte[] { 0x20, 0x44, 0x56, 0x76, 0x1e, 0xff };
            byte[] array2 = new byte[] { 0x42, 0x52, 0x72, 0xef };
            byte[] array3 = new byte[] { 0x20, 0x44, 0x56, 0x76, 0x1e, 0xff };
            byte[] array4 = new byte[] { 0x42, 0x52, 0x72, 0xef };

            ICollection set1 = new ICollectionAdapter(array1, array2);
            ICollection set2 = new ICollectionAdapter(array3, array4);

            Constraint constraint = new CollectionEquivalentConstraint(set1);
            Assert.That(constraint.Matches(set2));

            set2 = new ICollectionAdapter(array4, array3);
            Assert.That(constraint.Matches(set2));
        }

        [Test]
        public void EquivalentIgnoresOrder()
        {
            ICollection set1 = new ICollectionAdapter("x", "y", "z");
            ICollection set2 = new ICollectionAdapter("z", "y", "x");

            Assert.That(new CollectionEquivalentConstraint(set1).Matches(set2));
        }

        [Test]
        public void EquivalentFailsWithDuplicateElementInActual()
        {
            ICollection set1 = new ICollectionAdapter("x", "y", "z");
            ICollection set2 = new ICollectionAdapter("x", "y", "x");

            Assert.False(new CollectionEquivalentConstraint(set1).Matches(set2));
        }

        [Test]
        public void EquivalentFailsWithDuplicateElementInExpected()
        {
            ICollection set1 = new ICollectionAdapter("x", "y", "x");
            ICollection set2 = new ICollectionAdapter("x", "y", "z");

            Assert.False(new CollectionEquivalentConstraint(set1).Matches(set2));
        }

        [Test]
        public void EquivalentHandlesNull()
        {
            ICollection set1 = new ICollectionAdapter(null, "x", null, "z");
            ICollection set2 = new ICollectionAdapter("z", null, "x", null);

            Assert.That(new CollectionEquivalentConstraint(set1).Matches(set2));
        }

        [Test]
        public void EquivalentHonorsIgnoreCase()
        {
            ICollection set1 = new ICollectionAdapter("x", "y", "z");
            ICollection set2 = new ICollectionAdapter("z", "Y", "X");

            Assert.That(new CollectionEquivalentConstraint(set1).IgnoreCase.Matches(set2));
        }

        [Test]
        [TestCaseSource(typeof(IgnoreCaseDataProvider), "TestCases")]
        public void HonorsIgnoreCase(IEnumerable expected, IEnumerable actual)
        {
            var constraint = new CollectionEquivalentConstraint(expected).IgnoreCase;
            Assert.That(constraint.Matches(actual));
        }

        public class IgnoreCaseDataProvider
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new[] { 'A', 'B', 'C' }, new object[] { 'a', 'c', 'b' });
                    yield return new TestCaseData(new[] { "a", "b", "c" }, new object[] { "A", "C", "B" });
                    yield return new TestCaseData(new Dictionary<int, string> { { 2, "b" }, { 1, "a" } }, new Dictionary<int, string> { { 1, "A" }, { 2, "b" } });
                    yield return new TestCaseData(new Dictionary<int, char> { { 1, 'A' } }, new Dictionary<int, char> { { 1, 'a' } });
                    yield return new TestCaseData(new Dictionary<string, int> { { "b", 2 }, { "a", 1 } }, new Dictionary<string, int> { { "A", 1 }, { "b", 2 } });
                    yield return new TestCaseData(new Dictionary<char, int> { { 'A', 1 } }, new Dictionary<char, int> { { 'a', 1 } });
                    yield return new TestCaseData(new Hashtable { { 1, "a" }, { 2, "b" } }, new Hashtable { { 1, "A" }, { 2, "B" } });
                    yield return new TestCaseData(new Hashtable { { 1, 'A' }, { 2, 'B' } }, new Hashtable { { 1, 'a' }, { 2, 'b' } });
                    yield return new TestCaseData(new Hashtable { { "b", 2 }, { "a", 1 } }, new Hashtable { { "A", 1 }, { "b", 2 } });
                    yield return new TestCaseData(new Hashtable { { 'A', 1 } }, new Hashtable { { 'a', 1 } });
                }
            }
        }

#if CS_3_0 || CS_4_0 || CS_5_0
        [Test]
        public void EquivalentHonorsUsing()
        {
            ICollection set1 = new ICollectionAdapter("x", "y", "z");
            ICollection set2 = new ICollectionAdapter("z", "Y", "X");

            Assert.That(new CollectionEquivalentConstraint(set1)
                .Using<string>((x, y) => String.Compare(x, y, true))
                .Matches(set2));
        }

        [Test, Platform("Net-3.5,Mono-3.5,Net-4.0,Mono-4.0")]
        public void WorksWithHashSets()
        {
            var hash1 = new HashSet<string>(new string[] { "presto", "abracadabra", "hocuspocus" });
            var hash2 = new HashSet<string>(new string[] { "abracadabra", "presto", "hocuspocus" });

            Assert.That(new CollectionEquivalentConstraint(hash1).Matches(hash2));
        }

        [Test, Platform("Net-3.5,Mono-3.5,Net-4.0,Mono-4.0")]
        public void WorksWithHashSetAndArray()
        {
            var hash = new HashSet<string>(new string[] { "presto", "abracadabra", "hocuspocus" });
            var array = new string[] { "abracadabra", "presto", "hocuspocus" };

            var constraint = new CollectionEquivalentConstraint(hash);
            Assert.That(constraint.Matches(array));
        }

        [Test, Platform("Net-3.5,Mono-3.5,Net-4.0,Mono-4.0")]
        public void WorksWithArrayAndHashSet()
        {
            var hash = new HashSet<string>(new string[] { "presto", "abracadabra", "hocuspocus" });
            var array = new string[] { "abracadabra", "presto", "hocuspocus" };

            var constraint = new CollectionEquivalentConstraint(array);
            Assert.That(constraint.Matches(hash));
        }

        [Test, Platform("Net-3.5,Mono-3.5,Net-4.0,Mono-4.0")]
        public void FailureMessageWithHashSetAndArray()
        {
            var hash = new HashSet<string>(new string[] { "presto", "abracadabra", "hocuspocus" });
            var array = new string[] { "abracadabra", "presto", "hocusfocus" };

            var constraint = new CollectionEquivalentConstraint(hash);
            Assert.False(constraint.Matches(array));

            TextMessageWriter writer = new TextMessageWriter();
            constraint.WriteMessageTo(writer);
            Assert.That(writer.ToString(), Is.EqualTo(
                "  Expected: equivalent to < \"presto\", \"abracadabra\", \"hocuspocus\" >" + Environment.NewLine +
                "  But was:  < \"abracadabra\", \"presto\", \"hocusfocus\" >" + Environment.NewLine));
            Console.WriteLine(writer.ToString());
        }
#endif
    }
}
