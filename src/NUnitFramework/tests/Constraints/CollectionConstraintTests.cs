// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using NUnit.Framework.Tests;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class AllItemsTests : NUnit.Framework.Tests.MessageChecker
    {
        [Test]
        public void AllItemsAreNotNull()
        {
            object[] c = new object[] { 1, "hello", 3, Environment.OSVersion };
            Assert.That(c, new AllItemsConstraint(new NotConstraint( new EqualConstraint(null))));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AllItemsAreNotNullFails()
        {
            object[] c = new object[] { 1, "hello", null, 3 };
            expectedMessage = 
				TextMessageWriter.Pfx_Expected + "all items not null" + Environment.NewLine +
                TextMessageWriter.Pfx_Actual + "< 1, \"hello\", null, 3 >" + Environment.NewLine;
            Assert.That(c, new AllItemsConstraint(new NotConstraint( new EqualConstraint(null))));
        }

        [Test]
        public void AllItemsAreInRange()
        {
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(new GreaterThanConstraint(10) & new LessThanConstraint(100)));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AllItemsAreInRangeFailureMessage()
        {
            int[] c = new int[] { 12, 27, 19, 32, 107, 99, 26 };
            expectedMessage = 
                TextMessageWriter.Pfx_Expected + "all items greater than 10 and less than 100" + Environment.NewLine +
                TextMessageWriter.Pfx_Actual   + "< 12, 27, 19, 32, 107, 99, 26 >" + Environment.NewLine;
            Assert.That(c, new AllItemsConstraint(new GreaterThanConstraint(10) & new LessThanConstraint(100)));
        }

        [Test]
        public void AllItemsAreInstancesOfType()
        {
            object[] c = new object[] { 'a', 'b', 'c' };
            Assert.That(c, new AllItemsConstraint(new InstanceOfTypeConstraint(typeof(char))));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AllItemsAreInstancesOfTypeFailureMessage()
        {
            object[] c = new object[] { 'a', "b", 'c' };
            expectedMessage = 
                TextMessageWriter.Pfx_Expected + "all items instance of <System.Char>" + Environment.NewLine +
                TextMessageWriter.Pfx_Actual   + "< 'a', \"b\", 'c' >" + Environment.NewLine;
            Assert.That(c, new AllItemsConstraint(new InstanceOfTypeConstraint(typeof(char))));
        }
    }

    [TestFixture]
    public class CollectionContainsTests
    {
        [Test]
        public void CanTestContentsOfArray()
        {
            object item = "xyz";
            object[] c = new object[] { 123, item, "abc" };
            Assert.That(c, new CollectionContainsConstraint(item));
        }

        [Test]
        public void CanTestContentsOfArrayList()
        {
            object item = "xyz";
            ArrayList list = new ArrayList( new object[] { 123, item, "abc" } );
            Assert.That(list, new CollectionContainsConstraint(item));
        }

        [Test]
        public void CanTestContentsOfSortedList()
        {
            object item = "xyz";
            SortedList list = new SortedList();
            list.Add("a", 123);
            list.Add("b", item);
            list.Add("c", "abc");
            Assert.That(list.Values, new CollectionContainsConstraint(item));
            Assert.That(list.Keys, new CollectionContainsConstraint("b"));
        }

		[Test]
		public void CanTestContentsOfCollectionNotImplementingIList()
		{
			CollectionAdapter ints = new CollectionAdapter(new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9});           
			Assert.That(ints, new CollectionContainsConstraint( 9 ));
		}
    }
}
