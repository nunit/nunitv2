// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Tests
{
    /// <summary>
    /// This test fixture attempts to exercise all the syntactic
    /// variations of Assert, including the classic syntax as well
    /// as the new, constraint-based syntax, without getting into 
    /// failures, errors or corner cases. Thus, some of the tests 
    /// may be duplicated in other fixtures.
    /// 
    /// This Fixture will eventually be duplicated in other
    /// supported languages.
    /// </summary>
    [TestFixture]
    public class AssertSyntaxTests
    {
        [Test]
        public void IsNull()
        {
            // Classic syntax
            Assert.IsNull(null);
            // New syntax
            Assert.That(null, Is.Null);
        }

        [Test]
        public void IsNotNull()
        {
            // Classic syntax
            Assert.IsNotNull(42);
            // New syntax
            Assert.That(42, Is.Not.Null);
        }

        [Test]
        public void IsTrue()
        {
            // Classic syntax
            Assert.IsTrue(2+2==4);
            // New syntax
            Assert.That(2+2==4, Is.True);
            Assert.That(2+2==4);
        }

        [Test]
        public void IsFalse()
        {
            // Classic syntax
            Assert.IsFalse(2+2==5);
            // New syntax
            Assert.That(2+2== 5, Is.False);
        }

        [Test]
        public void IsNaN()
        {
            // Classic syntax
            Assert.IsNaN(double.NaN);
            Assert.IsNaN(float.NaN);
            // New syntax
            Assert.That(double.NaN, Is.NaN);
            Assert.That(float.NaN, Is.NaN);
        }

        [Test]
        public void EmptyStringTests()
        {
            // Classic syntax
            Assert.IsEmpty("");
            Assert.IsNotEmpty("Hello!");
            // New syntax
            Assert.That("", Is.Empty);
            Assert.That("Hello!", Is.Not.Empty);
        }

        [Test]
        public void EmptyCollectionTests()
        {
            // Classic syntax
            Assert.IsEmpty(new bool[0]);
            Assert.IsNotEmpty(new int[] { 1, 2, 3 });
            // New syntax
            Assert.That(new bool[0], Is.Empty);
            Assert.That(new int[] { 1, 2, 3 }, Is.Not.Empty);
        }

        [Test]
        public void ExactTypeTests()
        {
            // Classic syntax workarounds
            Assert.AreEqual(typeof(string), "Hello".GetType());
            Assert.AreEqual("System.String", "Hello".GetType().FullName);
            Assert.AreNotEqual(typeof(int), "Hello".GetType());
            Assert.AreNotEqual("System.Int32", "Hello".GetType().FullName);
            // Only available using new syntax
            Assert.That("Hello", Is.Type(typeof(string)));
            Assert.That("Hello", Is.Not.Type(typeof(int)));
        }

        [Test]
        public void InstanceOfTypeTests()
        {
            // Classic syntax
            Assert.IsInstanceOfType(typeof(string), "Hello");
            Assert.IsNotInstanceOfType(typeof(string), 5);
            // New Syntax
            Assert.That("Hello", Is.InstanceOfType(typeof(string)));
            Assert.That(5, Is.Not.InstanceOfType(typeof(string)));
        }

        [Test]
        public void AssignableFromTypeTests()
        {
            // Classic syntax
            Assert.IsAssignableFrom(typeof(string), "Hello");
            Assert.IsNotAssignableFrom(typeof(string), 5);
            // New syntax
            Assert.That( "Hello", Is.AssignableFrom(typeof(string)));
            Assert.That( 5, Is.Not.AssignableFrom(typeof(string)));
        }

        [Test]
        public void SubstringTests()
        {
            string phrase = "Hello World!";
            // Classic Syntax
            StringAssert.Contains("World", phrase);
            // New Syntax
            Assert.That(phrase, Is.StringContaining("World"));
            // Only available using new syntax
            Assert.That(phrase, Is.Not.StringContaining("goodbye"));
            Assert.That(phrase, Is.StringContaining("WORLD").IgnoreCase);
			Assert.That(phrase, Is.Not.StringContaining("BYE").IgnoreCase);
        }

        [Test]
        public void StartsWithTests()
        {
            string phrase = "Hello World!";
            // Classic syntax
            StringAssert.StartsWith("Hello", phrase);
            // New syntax
            Assert.That(phrase, Is.StringStarting("Hello"));
            // Only available using new syntax
            Assert.That(phrase, Is.Not.StringStarting("Hi!"));
            Assert.That(phrase, Is.StringStarting("HeLLo").IgnoreCase);
			Assert.That(phrase, Is.Not.StringStarting("HI").IgnoreCase);
        }

        [Test]
        public void EndsWithTests()
        {
            string phrase = "Hello World!";
            // Classic Syntax
            StringAssert.EndsWith("!", phrase);
            // New Syntax
            Assert.That(phrase, Is.StringEnding("!"));
            // Only available using new syntax
            Assert.That(phrase, Is.Not.StringEnding("?"));
            Assert.That(phrase, Is.StringEnding("WORLD!").IgnoreCase);
        }

        [Test]
        public void EqualIgnoringCaseTests()
        {
            string phrase = "Hello World!";
            // Classic syntax
            StringAssert.AreEqualIgnoringCase("hello world!",phrase);
            // New Syntax
            Assert.That(phrase, Is.EqualTo("hello world!").IgnoreCase);
            //Only available using new syntax
            Assert.That(phrase, Is.Not.EqualTo("goodbye world!").IgnoreCase);
            Assert.That(new string[] { "Hello", "World" }, 
                Is.EqualTo(new object[] { "HELLO", "WORLD" }).IgnoreCase);
        }

        [Test]
        public void RegularExpressionTests()
        {
            string phrase = "Now is the time for all good men to come to the aid of their country.";
            // Classic syntax
            StringAssert.IsMatch( "all good men", phrase );
            StringAssert.IsMatch( "Now.*come", phrase );
            // New syntax
            Assert.That( phrase, Is.StringMatching( "all good men" ) );
            Assert.That( phrase, Is.StringMatching( "Now.*come" ) );
            // Only available using new syntax
            Assert.That(phrase, Is.Not.StringMatching("all.*men.*good"));
            Assert.That(phrase, Is.StringMatching("ALL").IgnoreCase);
        }

        [Test]
        public void EqualityTests()
        {
            int[] i3 = new int[] { 1, 2, 3 };
            double[] d3 = new double[] { 1.0, 2.0, 3.0 };
            int[] iunequal = new int[] { 1, 3, 2 };
            // Classic Syntax
            Assert.AreEqual(4, 2 + 2);
            Assert.AreEqual(i3, d3);
            Assert.AreNotEqual(5, 2 + 2);
            Assert.AreNotEqual(i3, iunequal);
            // New syntax
            Assert.That(2 + 2, Is.EqualTo(4));
            Assert.That(2 + 2 == 4);
            Assert.That(i3, Is.EqualTo(d3));
            Assert.That(2 + 2, Is.Not.EqualTo(5));
            Assert.That(i3, Is.Not.EqualTo(iunequal));
        }

        [Test]
        public void EqualityTestsWithTolerance()
        {
            // CLassic syntax
            Assert.AreEqual(5.0d, 4.99d, 0.05d);
            Assert.AreEqual(5.0f, 4.99f, 0.05f);
            // New syntax
            Assert.That(4.99d, Is.EqualTo(5.0d).Within(0.05d));
            Assert.That(4.99f, Is.EqualTo(5.0f).Within(0.05f));
        }

        [Test]
        public void ComparisonTests()
        {
            // Classic Syntax
            Assert.Greater(7, 3);
            Assert.GreaterOrEqual(7, 3);
            Assert.GreaterOrEqual(7, 7);
            // New Syntax
            Assert.That(7, Is.GreaterThan(3));
            Assert.That(7, Is.GreaterThanOrEqualTo(3));
            Assert.That(7, Is.AtLeast(3));
            Assert.That(7, Is.GreaterThanOrEqualTo(7));
            Assert.That(7, Is.AtLeast(7));

            // Classic syntax
            Assert.Less(3, 7);
            Assert.LessOrEqual(3, 7);
            Assert.LessOrEqual(3, 3);
            // New Syntax
            Assert.That(3, Is.LessThan(7));
            Assert.That(3, Is.LessThanOrEqualTo(7));
            Assert.That(3, Is.AtMost(7));
            Assert.That(3, Is.LessThanOrEqualTo(3));
            Assert.That(3, Is.AtMost(3));
        }

        [Test]
        public void AllItemsTests()
        {
            object[] c = new object[] { 1, 2, 3, 4 };
            // Classic syntax
            CollectionAssert.AllItemsAreNotNull(c);
            CollectionAssert.AllItemsAreInstancesOfType(c, typeof(int));
            CollectionAssert.AllItemsAreUnique(c);
            // New syntax
            Assert.That(c, Is.All.Not.Null);
            Assert.That(c, Is.All.InstanceOfType(typeof(int)));
            Assert.That(c, Is.Unique);
            // Only available using new syntax
            Assert.That(c, Is.All.GreaterThan(0));                 
        }

        [Test]
        public void CollectionContainsTests()
        {
            int[] iarray = new int[] { 1, 2, 3 };
            string[] sarray = new string[] { "a", "b", "c" };
            // Classic syntax
            Assert.Contains(3, iarray);
            Assert.Contains("b", sarray);
            CollectionAssert.Contains(iarray, 3);
            CollectionAssert.Contains(sarray, "b");
            CollectionAssert.DoesNotContain(sarray, "x");
            // New syntax
            Assert.That(iarray, Is.CollectionContaining(3));
            Assert.That(sarray, Is.CollectionContaining("b"));
            Assert.That(sarray, Is.Not.CollectionContaining("x"));
        }

        [Test]
        public void CollectionEquivalenceTests()
        {
            int[] ints1to5 = new int[] { 1, 2, 3, 4, 5 };
            // Classic syntax
            CollectionAssert.AreEquivalent(new int[] { 2, 1, 4, 3, 5 }, ints1to5);
            CollectionAssert.AreNotEquivalent(new int[] { 2, 2, 4, 3, 5 }, ints1to5);
            CollectionAssert.AreNotEquivalent(new int[] { 2, 4, 3, 5 }, ints1to5);
            CollectionAssert.AreEquivalent(new int[] { 2, 2, 1, 1, 4, 3, 5 }, ints1to5);
            // New syntax
            Assert.That(new int[] { 2, 1, 4, 3, 5 }, Is.EquivalentTo(ints1to5));
            Assert.That(new int[] { 2, 2, 4, 3, 5 }, Is.Not.EquivalentTo(ints1to5));
            Assert.That(new int[] { 2, 4, 3, 5 }, Is.Not.EquivalentTo(ints1to5));
            Assert.That(new int[] { 2, 2, 1, 1, 4, 3, 5 }, Is.EquivalentTo(ints1to5));
        }

        [Test]
        public void SubsetTests()
        {
            int[] ints1to5 = new int[] { 1, 2, 3, 4, 5 };
            // Classic syntax
            CollectionAssert.IsSubsetOf(new int[] { 1, 3, 5 }, ints1to5);
            CollectionAssert.IsSubsetOf(new int[] { 1, 2, 3, 4, 5 }, ints1to5);
            CollectionAssert.IsNotSubsetOf(new int[] { 2, 4, 6 }, ints1to5);
            // New syntax
            Assert.That(new int[] { 1, 3, 5 }, Is.SubsetOf(ints1to5));
            Assert.That(new int[] { 1, 2, 3, 4, 5 }, Is.SubsetOf(ints1to5));
            Assert.That(new int[] { 2, 4, 6 }, Is.Not.SubsetOf(ints1to5));
        }

        [Test]
        public void NotTests()
        {
            // Not as a separate prefix is only available in the new syntax
            Assert.That(42, Is.Not.Null);
            Assert.That(42, Is.Not.True);
            Assert.That(42, Is.Not.False);
            Assert.That(2.5, Is.Not.NaN);
            Assert.That(2 + 2, Is.Not.EqualTo(3));
            Assert.That(2 + 2, Is.Not.Not.EqualTo(4));
            Assert.That(2 + 2, Is.Not.Not.Not.EqualTo(5));
        }

        [Test]
        public void NotOperator()
        {
            // The ! operator is only available in the new syntax
            Assert.That(42, !Is.Null);
        }

        [Test]
        public void AndOperator()
        {
            // The & operator is only available in the new syntax
            Assert.That(7, Is.GreaterThan(5) & Is.LessThan(10));
        }

        [Test]
        public void OrOperator()
        {
            // The | operator is only available in the new syntax
            Assert.That(3, Is.LessThan(5) | Is.GreaterThan(10));
        }

        [Test]
        public void ComplexTests()
        {
            Assert.That(7, Is.Not.Null & Is.Not.LessThan(5) & Is.Not.GreaterThan(10));
            Assert.That(7, !Is.Null & !Is.LessThan(5) & !Is.GreaterThan(10));
// TODO: Remove #if when mono compiler can handle null
#if MONO
            Constraints.Constraint x = null;
            Assert.That(7, !x & !Is.LessThan(5) & !Is.GreaterThan(10));
#else
            Assert.That(7, !(Constraints.Constraint)null & !Is.LessThan(5) & !Is.GreaterThan(10));
#endif
        }

        // This method contains assertions that should not compile
        // You can check by uncommenting it.
        //public void WillNotCompile()
        //{
        //    Assert.That(42, Is.Not);
        //    Assert.That(42, Is.All);
        //    Assert.That(42, Is.Null.Not);
        //    Assert.That(42, Is.Not.Null.GreaterThan(10));
        //    Assert.That(42, Is.GreaterThan(10).LessThan(99));

        //    object[] c = new object[0];
        //    Assert.That(c, Is.Null.All);
        //    Assert.That(c, Is.Not.All);
        //    Assert.That(c, Is.All.Not);
        //}
    }
}
