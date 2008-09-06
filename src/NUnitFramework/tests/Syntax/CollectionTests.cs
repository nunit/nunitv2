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

    public class CollectionOrderedTest_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<ordered System.Collections.Comparer>";
            staticSyntax = Is.Ordered().Descending;
            inheritedSyntax = Helper().Ordered().Descending;
            builderSyntax = Builder().Ordered().Descending;
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

    public class CollectionOrderedTest_Comparer_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            IComparer comparer = new Comparer(System.Globalization.CultureInfo.CurrentCulture);
            parseTree = "<ordered System.Collections.Comparer>";
            staticSyntax = Is.Ordered(comparer).Descending;
            inheritedSyntax = Helper().Ordered(comparer).Descending;
            builderSyntax = Builder().Ordered(comparer).Descending;
        }
    }

    public class CollectionOrderedByTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<ordered System.Collections.Comparer SomePropertyName>";
            staticSyntax = Is.OrderedBy("SomePropertyName");
            inheritedSyntax = Helper().OrderedBy("SomePropertyName");
            builderSyntax = Builder().OrderedBy("SomePropertyName");
        }
    }

    public class CollectionOrderedByTest_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<ordered System.Collections.Comparer SomePropertyName>";
            staticSyntax = Is.OrderedBy("SomePropertyName").Descending;
            inheritedSyntax = Helper().OrderedBy("SomePropertyName").Descending;
            builderSyntax = Builder().OrderedBy("SomePropertyName").Descending;
        }
    }

    public class CollectionOrderedByTest_Comparer : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<ordered System.Collections.Comparer SomePropertyName>";
            staticSyntax = Is.OrderedBy("SomePropertyName", Comparer.Default);
            inheritedSyntax = Helper().OrderedBy("SomePropertyName", Comparer.Default);
            builderSyntax = Builder().OrderedBy("SomePropertyName", Comparer.Default);
        }
    }

    public class CollectionOrderedByTest_Comparer_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<ordered System.Collections.Comparer SomePropertyName>";
            staticSyntax = Is.OrderedBy("SomePropertyName", Comparer.Default).Descending;
            inheritedSyntax = Helper().OrderedBy("SomePropertyName", Comparer.Default).Descending;
            builderSyntax = Builder().OrderedBy("SomePropertyName", Comparer.Default).Descending;
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
