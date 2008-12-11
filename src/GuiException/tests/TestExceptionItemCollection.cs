// ----------------------------------------------------------------
// ExceptionBrowser
// Version 1.0.0
// Copyright 2008, Irénée HOTTIER,
// 
// This is free software licensed under the NUnit license, You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.UiException;
using NUnit.Framework;
using NUnit.UiException.Tests.data;

namespace NUnit.UiException.Tests
{
    [TestFixture]
    public class TestExceptionItemCollection
    {
        TestResource _resourceA;
        TestResource _resourceB;

        private ExceptionItemCollection _items;
        private ExceptionItem _itemA;
        private ExceptionItem _itemB;

        private int _notificationAdded;
        private ExceptionItem _notifiedItem;

        private int _notificationCleared;

        [SetUp]
        public void SetUp()
        {
            _items = new InternalTraceItemCollection();

            _resourceA = new TestResource("HelloWorld.txt");
            _resourceB = new TestResource("TextCode.txt");

            _itemA = new ExceptionItem(_resourceA.Path, 1);
            _itemB = new ExceptionItem(_resourceB.Path, 2);

            _items.ItemAdded += new ItemAddedEventHandler(_items_ItemAdded);
            _notificationAdded = 0;
            _notifiedItem = null;

            _items.CollectionCleared += new EventHandler(_items_CollectionCleared);
            _notificationCleared = 0;

            return;
        }

        [TearDown]
        public void TearDown()
        {
            if (_resourceA != null)
            {
                _resourceA.Dispose();
                _resourceA = null;
            }

            if (_resourceB != null)
            {
                _resourceB.Dispose();
                _resourceB = null;
            }
        }

        void _items_CollectionCleared(object sender, EventArgs e)
        {
            _notificationCleared++;
        }

        void _items_ItemAdded(object sender, ExceptionItem item)
        {
            _notificationAdded++;
            _notifiedItem = item;
        }

        [Test]
        public void Test_TraceItems()
        {
            List<ExceptionItem> lst;

            Assert.That(_items.Count, Is.EqualTo(0));

            _items.Add(_itemA);
            _items.Add(_itemB);

            Assert.That(_items.Count, Is.EqualTo(2));

            Assert.That(_items[0], Is.EqualTo(_itemA));
            Assert.That(_items[1], Is.EqualTo(_itemB));

            lst = new List<ExceptionItem>();
            foreach (ExceptionItem item in _items)
                lst.Add(item);
            Assert.That(lst.Count, Is.EqualTo(2));
            Assert.That(lst[0], Is.EqualTo(_items[0]));
            Assert.That(lst[1], Is.EqualTo(_items[1]));

            return;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException),
            ExpectedMessage = "item",
            MatchType = MessageMatch.Contains)]
        public void Test_Add_Throws_NullItemException()
        {
            _items.Add(null); // throws exception
        }

        [Test]
        public void Test_Clear()
        {
            _items.Add(_itemA);

            Assert.That(_items.Count, Is.EqualTo(1));
            _items.Clear();
            Assert.That(_items.Count, Is.EqualTo(0));

            return;
        }

        [Test]
        public void Test_Contains()
        {
            Assert.That(_items.Contains(null), Is.False);
            Assert.That(_items.Contains(_itemA), Is.False);

            _items.Add(_itemA);

            Assert.That(_items.Contains(_itemA), Is.True);

            return;
        }

        [Test]
        public void Test_Add_Fire_ItemAddedEvent()
        {
            _items.Add(_itemA);
            Assert.That(_notificationAdded, Is.EqualTo(1));
            Assert.That(_notifiedItem, Is.EqualTo(_itemA));

            return;
        }     

        [Test]
        public void Test_Clear_Fire_CollectionClearedEvent()
        {
            _items.Add(_itemA);
            _items.Clear();
            Assert.That(_notificationCleared, Is.EqualTo(1));

            _items.Clear();
            Assert.That(_notificationCleared, Is.EqualTo(1));

            return;
        }

        #region InternalTraceItemCollection

        class InternalTraceItemCollection :
            ExceptionItemCollection
        {
            public InternalTraceItemCollection()
            {
                // nothing to do
            }
        }

        #endregion
    }
}
