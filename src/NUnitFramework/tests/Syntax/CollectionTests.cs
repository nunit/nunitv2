using System;
using System.Collections;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Syntax
{
    public class UniqueTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<uniqueitems>";
            staticSyntax = Is.Unique;
            inheritedSyntax = Helper().Unique;
            builderSyntax = Builder().Unique;
        }
    }

    public class CollectionOrderedTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<ordered System.Collections.Comparer>";
            staticSyntax = Is.Ordered();
            inheritedSyntax = Helper().Ordered();
            builderSyntax = Builder().Ordered();
        }
    }

    public class CollectionOrderedTest_Comparer : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            IComparer comparer = new Comparer(System.Globalization.CultureInfo.CurrentCulture);
            parseTree = "<ordered System.Collections.Comparer>";
            staticSyntax = Is.Ordered(comparer);
            inheritedSyntax = Helper().Ordered(comparer);
            builderSyntax = Builder().Ordered(comparer);
        }
    }

    public class CollectionContainsTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<contains 42>";
            staticSyntax = Has.Member(42);
            inheritedSyntax = Helper().Contains(42);
            builderSyntax = Builder().Contains(42);
        }
    }

    public class CollectionSubsetTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            int[] ints = new int[] { 1, 2, 3 };
            parseTree = "<subsetof System.Int32[]>";
            staticSyntax = Is.SubsetOf(ints);
            inheritedSyntax = Helper().SubsetOf(ints);
            builderSyntax = Builder().SubsetOf(ints);
        }
    }

    public class CollectionEquivalentTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            int[] ints = new int[] { 1, 2, 3 };
            parseTree = "<equivalent System.Int32[]>";
            staticSyntax = Is.EquivalentTo(ints);
            inheritedSyntax = Helper().EquivalentTo(ints);
            builderSyntax = Builder().EquivalentTo(ints);
        }
    }
}
