// ----------------------------------------------------------------
// ErrorBrowser
// Copyright 2008-2009, Irénée HOTTIER,
// 
// This is free software licensed under the NUnit license, You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Drawing;
using NUnit.UiException.Controls;

namespace NUnit.UiException.Tests.Controls
{
    [TestFixture]
    public class TestExceptionList
    {
        private TestingExceptionList _emptyList;
        private TestingExceptionList _filledList;

        [SetUp]
        public void SetUp()
        {
            _emptyList = new TestingExceptionList();

            _filledList = new TestingExceptionList();
            _filledList.Items.Add(new ErrorItem("File1.cs", "function1()", 1));
            _filledList.Items.Add(new ErrorItem("File2.cs", "function2()", 2));
            _filledList.Items.Add(new ErrorItem(null, "function3()", 3));
            _filledList.Items.Add(new ErrorItem(null, "function4()", 4));
            _filledList.Items.Add(new ErrorItem(null, "function5()", 5));
            _filledList.Items.Add(new ErrorItem("File6.cs", "function6()", 6));

            _emptyList.PageSize = 4;
            _filledList.PageSize = 4;

            return;
        }

        [Test]
        public void Test_Default()
        {
            Assert.That(_emptyList.Items, Is.Not.Null);
            Assert.That(_emptyList.Items.Count, Is.EqualTo(0));
            Assert.That(_emptyList.SelectedItem, Is.Null);
            Assert.That(_emptyList.PageSize, Is.EqualTo(4));
            Assert.That(_emptyList.PageCount, Is.EqualTo(1));
            Assert.That(_emptyList.PageIndex, Is.EqualTo(0));
            Assert.That(_emptyList.DrawingText, Is.True);
            Assert.That(_emptyList.Pagineer.PageIndex, Is.EqualTo(0));
            Assert.That(_emptyList.Pagineer.PageSize, Is.EqualTo(4));
            Assert.That(_emptyList.Pagineer.PageCount, Is.EqualTo(0));
            Assert.That(_emptyList.Pagineer.ToString(), Is.EqualTo("page 0/0"));

            Assert.That(_emptyList.Width, Is.EqualTo(ExceptionList.DEFAULT_ITEM_WIDTH));
            Assert.That(_emptyList.Height,
                Is.EqualTo(ExceptionList.DEFAULT_HEADER_HEIGHT + ExceptionList.DEFAULT_ITEM_HEIGHT *
                _emptyList.PageSize));
            Assert.That(_emptyList.BackColor, Is.EqualTo(Color.White)); 
            

            Assert.That(_filledList.Items.Count, Is.EqualTo(6));
            Assert.That(_filledList.SelectedItem, Is.Null);
            Assert.That(_filledList.PageSize, Is.EqualTo(4));
            Assert.That(_filledList.PageCount, Is.EqualTo(2));
            Assert.That(_filledList.PageIndex, Is.EqualTo(0));
            Assert.That(_filledList.DrawingText, Is.False);
            Assert.That(_filledList.Pagineer.PageIndex, Is.EqualTo(0));
            Assert.That(_filledList.Pagineer.PageSize, Is.EqualTo(4));
            Assert.That(_filledList.Pagineer.PageCount, Is.EqualTo(2));
            Assert.That(_filledList.Pagineer.ToString(), Is.EqualTo("page 1/2"));

            return;
        }

        [Test]
        public void Test_Cannot_Resize_Control()
        {
            int width;
            int height;

            // check that size are fixed by the control

            width = _emptyList.Width;
            height = _emptyList.Height;

            _emptyList.Width = 10;
            _emptyList.Height = 12;

            Assert.That(_emptyList.Width, Is.EqualTo(width));
            Assert.That(_emptyList.Height, Is.EqualTo(height));

            return;
        }

        [Test]
        public void Test_PageSize()
        {
            // test to pass
            // set a correct value in PageSize

            _emptyList.PageSize = 2;
            Assert.That(_emptyList.PageSize, Is.EqualTo(2));

            // test to fail
            // value lesser than 1 are ignored

            _emptyList.PageSize = 0;
            Assert.That(_emptyList.PageSize, Is.EqualTo(1));

            return;
        }

        [Test]
        public void Test_PageIndex()
        {
            // test to pass
            // set a correct value in PageIndex

            _filledList.PageIndex = 1;
            Assert.That(_filledList.PageIndex, Is.EqualTo(1));            

            // test to fail
            // correct value lesser than 0

            _filledList.PageIndex = -1;
            Assert.That(_filledList.PageIndex, Is.EqualTo(0));

            // test to fail
            // correct value greater or equal to PageCount

            _filledList.PageIndex = _filledList.PageCount;
            Assert.That(_filledList.PageIndex, Is.EqualTo(_filledList.PageCount - 1));

            return;
        }

        [Test]
        public void Test_VisibleItemsAt()
        {
            // test to pass
            // check visible items on existing pages

            Assert.That(_filledList.VisibleItemsAt(0), Is.EqualTo(4));
            Assert.That(_filledList.VisibleItemsAt(1), Is.EqualTo(2));

            // test to fail
            // check that calling VisibleItemsAt() on unknown pages
            // always returns 0.

            Assert.That(_filledList.VisibleItemsAt(2), Is.EqualTo(0));
            Assert.That(_filledList.VisibleItemsAt(-1), Is.EqualTo(0));

            return;
        }

        [Test]
        public void Test_Populating()
        {
            int h;

            _emptyList.RepaintNotification = 0;

            // user changes height to see 3 items only.
            //  => control should resizes its height
            //  => control should repaint

            h = _emptyList.Height;
            _emptyList.PageSize = 3;
            Assert.That(_emptyList.Height, Is.LessThan(h));
            Assert.That(_emptyList.RepaintNotification, Is.EqualTo(1));

            // populate _emptyList
            //  => control should repaint any time an formatter is added

            _emptyList.RepaintNotification = 0;
            _emptyList.Items.Add(new ErrorItem());
            Assert.That(_emptyList.Pagineer.ItemCount, Is.EqualTo(1));
            Assert.That(_emptyList.RepaintNotification, Is.EqualTo(1));
            _emptyList.Items.Add(new ErrorItem());
            Assert.That(_emptyList.Pagineer.ItemCount, Is.EqualTo(2));
            Assert.That(_emptyList.RepaintNotification, Is.EqualTo(2));
            
            // when clearing items
            // control should repaint

            _emptyList.RepaintNotification = 0;
            _emptyList.Items.Clear();
            Assert.That(_emptyList.RepaintNotification, Is.EqualTo(1));
            Assert.That(_emptyList.Pagineer.ItemCount, Is.EqualTo(0));

            return;
        }

        [Test]
        public void Test_GetSelectedPointCoordinate()
        {
            Rectangle rect;
            Point pos;

            rect = _filledList.RectangleToScreen(new Rectangle(0, 0, 0, 0));

            _filledList.SelectedItemIndex = 0;
            pos = new Point(rect.X, rect.Y + ExceptionList.DEFAULT_HEADER_HEIGHT);
            Assert.That(_filledList.GetLowerLeftBoundOfCurrentItem(), Is.EqualTo(pos));

            _filledList.SelectedItemIndex = 1;
            pos = new Point(rect.X, rect.Y + ExceptionList.DEFAULT_HEADER_HEIGHT + ExceptionList.DEFAULT_ITEM_HEIGHT);
            Assert.That(_filledList.GetLowerLeftBoundOfCurrentItem(), Is.EqualTo(pos));

            _filledList.SelectedItemIndex = 2;
            pos = new Point(rect.X, 
                rect.Y + ExceptionList.DEFAULT_HEADER_HEIGHT + ExceptionList.DEFAULT_ITEM_HEIGHT * 2);
            Assert.That(_filledList.GetLowerLeftBoundOfCurrentItem(), Is.EqualTo(pos));

            _filledList.SelectedItemIndex = 3;
            pos = new Point(rect.X,
                rect.Y + ExceptionList.DEFAULT_HEADER_HEIGHT + ExceptionList.DEFAULT_ITEM_HEIGHT * 3);
            Assert.That(_filledList.GetLowerLeftBoundOfCurrentItem(), Is.EqualTo(pos));

            _filledList.PageIndex = 1;
            _filledList.SelectedItemIndex = 4;
            pos = new Point(rect.X, rect.Y + ExceptionList.DEFAULT_HEADER_HEIGHT);
            Assert.That(_filledList.GetLowerLeftBoundOfCurrentItem(), Is.EqualTo(pos));

            return;
        }

        [Test]
        public void Test_Mouse_Interactions()
        {
            int y1;
            int y2;
            int y3;

            // moving mouse cursor on the control should
            // highlight formatter right under the cursor

            y1 = ExceptionList.DEFAULT_HEADER_HEIGHT;
            y2 = y1 + ExceptionList.DEFAULT_ITEM_HEIGHT;
            y3 = y2 + ExceptionList.DEFAULT_ITEM_HEIGHT;
            _filledList.RepaintNotification = 0;

            _filledList.OnMouseMove(0, 0);
            Assert.That(_filledList.SelectedItem, Is.Null);
            Assert.That(_filledList.RepaintNotification, Is.EqualTo(0));

            _filledList.OnMouseMove(0, y1 + 1);
            Assert.That(_filledList.SelectedItem, Is.Not.Null);
            Assert.That(_filledList.SelectedItem.LineNumber, Is.EqualTo(1));
            Assert.That(_filledList.RepaintNotification, Is.EqualTo(1));

            _filledList.OnMouseMove(0, y2 + 1);
            Assert.That(_filledList.SelectedItem, Is.Not.Null);
            Assert.That(_filledList.SelectedItem.LineNumber, Is.EqualTo(2));
            Assert.That(_filledList.RepaintNotification, Is.EqualTo(2));

            // cannot select formatter with no source attachments

            _filledList.OnMouseMove(0, y3 + 1);
            Assert.That(_filledList.SelectedItem, Is.Null);
            Assert.That(_filledList.RepaintNotification, Is.EqualTo(3));
          
            // changing page startingPosition clear the selection

            _filledList.RepaintNotification = 0;

            _filledList.PageIndex = 1;
            Assert.That(_filledList.SelectedItem, Is.Null);

            _filledList.OnMouseMove(0, y3 + 1);
            Assert.That(_filledList.SelectedItem, Is.Null); // no formatter at this location

            _filledList.OnMouseMove(0, y2 + 1);
            Assert.That(_filledList.SelectedItem, Is.Not.Null);
            Assert.That(_filledList.SelectedItem.LineNumber, Is.EqualTo(6));

            // clearing items should reset SelectedItem to null

            _filledList.PageIndex = 0;
            _filledList.OnMouseMove(0, y1 + 1);
            Assert.That(_filledList.SelectedItem, Is.Not.Null);

            _filledList.Items.Clear();
            Assert.That(_filledList.SelectedItem, Is.Null);

            return;
        }

        #region TestingExceptionList

        class TestingExceptionList : ExceptionList
        {
            private int _repaintNotification;

            public new bool DrawingText
            {
                get { return (base.DrawingText); }
            }

            public int RepaintNotification
            {
                get { return (_repaintNotification); }
                set { _repaintNotification = value; }
            }

            protected override void OnRepaint()
            {
                _repaintNotification++;
            }

            public new void OnMouseMove(int x, int y)
            {
                base.OnMouseMove(x, y);
            }

            public new int VisibleItemsAt(int pageIndex)
            {
                return (base.VisibleItemsAt(pageIndex));
            }            
        }

        #endregion
    }
}
