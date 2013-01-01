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
     [TestFixture]
    public class CollectionOrderedTests : MessageChecker
    {
        [Test]
        public void IsOrdered()
        {
            ArrayList al = new ArrayList();
            al.Add("x");
            al.Add("y");
            al.Add("z");

            Assert.That(al, Is.Ordered);
        }

        [Test]
        public void IsOrdered_2()
        {
            ArrayList al = new ArrayList();
            al.Add(1);
            al.Add(2);
            al.Add(3);

            Assert.That(al, Is.Ordered);
        }

        [Test]
        public void IsOrderedDescending()
        {
            ArrayList al = new ArrayList();
            al.Add("z");
            al.Add("y");
            al.Add("x");

            Assert.That(al, Is.Ordered.Descending);
        }

        [Test]
        public void IsOrderedDescending_2()
        {
            ArrayList al = new ArrayList();
            al.Add(3);
            al.Add(2);
            al.Add(1);

            Assert.That(al, Is.Ordered.Descending);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void IsOrdered_Fails()
        {
            ArrayList al = new ArrayList();
            al.Add("x");
            al.Add("z");
            al.Add("y");

            expectedMessage =
                "  Expected: collection ordered" + Environment.NewLine +
                "  But was:  < \"x\", \"z\", \"y\" >" + Environment.NewLine;

            Assert.That(al, Is.Ordered);
        }

        [Test]
        public void IsOrdered_Allows_adjacent_equal_values()
        {
            ArrayList al = new ArrayList();
            al.Add("x");
            al.Add("x");
            al.Add("z");

            Assert.That(al, Is.Ordered);
        }

        [Test, ExpectedException(typeof(ArgumentNullException),
            ExpectedMessage = "index 1", MatchType = MessageMatch.Contains)]
        public void IsOrdered_Handles_null()
        {
            ArrayList al = new ArrayList();
            al.Add("x");
            al.Add(null);
            al.Add("z");

            Assert.That(al, Is.Ordered);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void IsOrdered_TypesMustBeComparable()
        {
            ArrayList al = new ArrayList();
            al.Add(1);
            al.Add("x");

            Assert.That(al, Is.Ordered);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void IsOrdered_AtLeastOneArgMustImplementIComparable()
        {
            ArrayList al = new ArrayList();
            al.Add(new object());
            al.Add(new object());

            Assert.That(al, Is.Ordered);
        }

        [Test]
        public void IsOrdered_Handles_custom_comparison()
        {
            ArrayList al = new ArrayList();
            al.Add(new object());
            al.Add(new object());

            AlwaysEqualComparer comparer = new AlwaysEqualComparer();
            Assert.That(al, Is.Ordered.Using(comparer));
            Assert.That(comparer.Called, "TestComparer was not called");
        }

        [Test]
        public void IsOrdered_Handles_custom_comparison2()
        {
            ArrayList al = new ArrayList();
            al.Add(2);
            al.Add(1);

            TestComparer comparer = new TestComparer();
            Assert.That(al, Is.Ordered.Using(comparer));
            Assert.That(comparer.Called, "TestComparer was not called");
        }

#if CLR_2_0 || CLR_4_0
        [Test]
        public void UsesProvidedComparerOfT()
        {
            ArrayList al = new ArrayList();
            al.Add(1);
            al.Add(2);

            MyComparer<int> comparer = new MyComparer<int>();
            Assert.That(al, Is.Ordered.Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparer<T> : IComparer<T>
        {
            public bool Called;

            public int Compare(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y);
            }
        }

        [Test]
        public void UsesProvidedComparisonOfT()
        {
            ArrayList al = new ArrayList();
            al.Add(1);
            al.Add(2);

            MyComparison<int> comparer = new MyComparison<int>();
            Assert.That(al, Is.Ordered.Using(new Comparison<int>(comparer.Compare)));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparison<T>
        {
            public bool Called;

            public int Compare(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y);
            }
        }

#if CS_3_0 || CS_4_0 || CS_5_0
        [Test]
        public void UsesProvidedLambda()
        {
            ArrayList al = new ArrayList();
            al.Add(1);
            al.Add(2);

            Comparison<int> comparer = (x, y) => x.CompareTo(y);
            Assert.That(al, Is.Ordered.Using(comparer));
        }
#endif
#endif

        [Test]
        public void IsOrderedBy()
        {
            ArrayList al = new ArrayList();
            al.Add(new OrderedByTestClass(1));
            al.Add(new OrderedByTestClass(2));

            Assert.That(al, Is.Ordered.By("Value"));
        }

        [Test]
        public void IsOrderedBy_Comparer()
        {
            ArrayList al = new ArrayList();
            al.Add(new OrderedByTestClass(1));
            al.Add(new OrderedByTestClass(2));

            Assert.That(al, Is.Ordered.By("Value").Using(Comparer.Default));
        }

        [Test]
        public void IsOrderedBy_Handles_heterogeneous_classes_as_long_as_the_property_is_of_same_type()
        {
            ArrayList al = new ArrayList();
            al.Add(new OrderedByTestClass(1));
            al.Add(new OrderedByTestClass2(2));

            Assert.That(al, Is.Ordered.By("Value"));
        }

        class OrderedByTestClass
        {
            private int myValue;

            public int Value
            {
                get { return myValue; }
                set { myValue = value; }
            }

            public OrderedByTestClass(int value)
            {
                Value = value;
            }
        }

        class OrderedByTestClass2
        {
            private int myValue;
            public int Value
            {
                get { return myValue; }
                set { myValue = value; }
            }

            public OrderedByTestClass2(int value)
            {
                Value = value;
            }
        }
    }
}
