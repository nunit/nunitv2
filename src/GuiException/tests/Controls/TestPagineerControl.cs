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
using NUnit.UiException;
using System.Drawing;
using NUnit.UiException.Controls;

namespace NUnit.UiException.Tests.Controls
{
    [TestFixture]
    public class TestPagineerControl
    {
        private TestingPagineerControl _empty;
        private TestingPagineerControl _filled;

        private int _notificationPageIndexChanged;
        private int _notificationPageSizeChanged;
        private int _notificationRepaint;

        [SetUp]
        public void SetUp()
        {
            _empty = new TestingPagineerControl();

            _filled = new TestingPagineerControl();
            _filled.ItemCount = 12;
            _filled.PageSize = 5;
            _filled.PageIndex = 0;

            _filled.PageIndexChanged += new EventHandler(_filled_PageIndexChanged);
            _notificationPageIndexChanged = 0;

            _filled.Repainting += new EventHandler(_filled_Repainting);
            _notificationRepaint = 0;

            _filled.PageSizeChanged += new EventHandler(_filled_PageSizeChanged);
            _notificationPageSizeChanged = 0;

            return;
        }

        void _filled_PageSizeChanged(object sender, EventArgs e) {
            _notificationPageSizeChanged++;
        }

        void _filled_Repainting(object sender, EventArgs e)
        {
            _notificationRepaint++;
        }

        void _filled_PageIndexChanged(object sender, EventArgs e)
        {
            _notificationPageIndexChanged++;
        }

        [Test]
        public void Test_Default()
        {
            Assert.That(_empty.ItemCount, Is.EqualTo(0));
            Assert.That(_empty.PageSize, Is.EqualTo(10));
            Assert.That(_empty.PageIndex, Is.EqualTo(0));
            Assert.That(_empty.PageCount, Is.EqualTo(0));
            Assert.That(_empty.FirstVisibleItemIndex, Is.EqualTo(-1));
            Assert.That(_empty.LastVisibleItemIndex, Is.EqualTo(-1));
            Assert.That(_empty.ToString(), Is.EqualTo("page 0/0"));

            Assert.That(_empty.Width, Is.EqualTo(260));
            Assert.That(_empty.Height, Is.EqualTo(16));

            Assert.That(_empty.BackColor, Is.EqualTo(Color.White));

            Assert.That(_filled.ToString(), Is.EqualTo("page 1/3"));

            return;
        }

        [Test]
        public void Test_Can_Set_Properties()
        {
            _empty.ItemCount = 12;
            _empty.PageSize = 5;
            _empty.PageIndex = 1;

            Assert.That(_empty.ItemCount, Is.EqualTo(12));
            Assert.That(_empty.PageSize, Is.EqualTo(5));
            Assert.That(_empty.PageIndex, Is.EqualTo(1));
            Assert.That(_empty.PageCount, Is.EqualTo(3));

            //
            // NUnit.UiException.Tests ability to correct PageIndex when
            // assigning incorrect values
            //

            _empty.PageIndex = -1;
            Assert.That(_empty.PageIndex, Is.EqualTo(0));

            _empty.PageIndex = 0;
            Assert.That(_empty.PageIndex, Is.EqualTo(0));

            _empty.PageIndex = 1;
            Assert.That(_empty.PageIndex, Is.EqualTo(1));

            _empty.PageIndex = 2;
            Assert.That(_empty.PageIndex, Is.EqualTo(2));

            _empty.PageIndex = 3;
            Assert.That(_empty.PageIndex, Is.EqualTo(2));

            //
            // NUnit.UiException.Tests ability to correct PageSize when assigning
            // incorrect value
            //
            _empty.PageSize = 0;
            Assert.That(_empty.PageSize, Is.EqualTo(1));

            //
            // NUnit.UiException.Tests ability to correct ItemCount when assigning
            // negative value
            //
            _empty.ItemCount = -1;
            Assert.That(_empty.ItemCount, Is.EqualTo(0));

            // test to fail
            // set ItemCount and PageIndex to 0
            _empty.ItemCount = 0;
            _empty.PageIndex = 0;
            Assert.That(_empty.PageIndex, Is.EqualTo(0));

            return;
        }       

        [Test]
        public void Test_FirstIndex()
        {
            Assert.That(_filled.FirstVisibleItemIndex, Is.EqualTo(0));
            Assert.That(_filled.LastVisibleItemIndex, Is.EqualTo(4));

            _filled.PageIndex = 1;
            Assert.That(_filled.FirstVisibleItemIndex, Is.EqualTo(5));
            Assert.That(_filled.LastVisibleItemIndex, Is.EqualTo(9));

            _filled.PageIndex = 2;
            Assert.That(_filled.FirstVisibleItemIndex, Is.EqualTo(10));
            Assert.That(_filled.LastVisibleItemIndex, Is.EqualTo(11));

            return;
        }

        [Test]
        public void Test_NextPage()
        {
            Assert.That(_filled.NextPage(), Is.True);
            Assert.That(_filled.PageIndex, Is.EqualTo(1));

            Assert.That(_filled.NextPage(), Is.True);
            Assert.That(_filled.PageIndex, Is.EqualTo(2));

            Assert.That(_filled.NextPage(), Is.False);
            Assert.That(_filled.PageIndex, Is.EqualTo(2));

            return;
        }

        [Test]
        public void Test_PrevPage()
        {
            _filled.PageIndex = 2;

            Assert.That(_filled.PrevPage(), Is.True);
            Assert.That(_filled.PageIndex, Is.EqualTo(1));

            Assert.That(_filled.PrevPage(), Is.True);
            Assert.That(_filled.PageIndex, Is.EqualTo(0));

            Assert.That(_filled.PrevPage(), Is.False);
            Assert.That(_filled.PageIndex, Is.EqualTo(0));

            return;
        }

        [Test]
        public void Test_Setting_PageIndex_FirePageChangedEvent()
        {
            _filled.PageIndex = 1;
            Assert.That(_notificationPageIndexChanged, Is.EqualTo(1));

            _filled.PageIndex = 2;
            Assert.That(_notificationPageIndexChanged, Is.EqualTo(2));

            _filled.PageIndex = 2;
            Assert.That(_notificationPageIndexChanged, Is.EqualTo(2));

            return;
        }

        [Test]
        public void Test_Setting_PageSize_FirePageSizeChangedEvent()
        {
            _filled.PageSize = 1;
            Assert.That(_notificationPageSizeChanged, Is.EqualTo(1));

            _filled.PageSize = 2;
            Assert.That(_notificationPageSizeChanged, Is.EqualTo(2));

            _filled.PageSize = 2;
            Assert.That(_notificationPageSizeChanged, Is.EqualTo(2));

            return;
        }

        [Test]
        public void Test_Setting_PageIndex_InvalidateControl()
        {
            _filled.PageIndex = 1;
            Assert.That(_notificationRepaint, Is.EqualTo(1));

            _filled.PageIndex = 2;
            Assert.That(_notificationRepaint, Is.EqualTo(2));

            return;
        }

        [Test]
        public void Test_Setting_PageSize_InvalidateControl()
        {
            _filled.PageSize = 4;
            Assert.That(_notificationRepaint, Is.EqualTo(1));

            _filled.PageSize = 4;
            Assert.That(_notificationRepaint, Is.EqualTo(1));

            return;
        }

        [Test]
        public void Test_Setting_ItemCount_InvalidateControl()
        {
            _filled.ItemCount = 2;
            Assert.That(_notificationRepaint, Is.EqualTo(1));

            _filled.ItemCount = 2;
            Assert.That(_notificationRepaint, Is.EqualTo(1));

            return;
        }

        [Test]
        public void Test_HandleMouseClick()
        {
            _filled.HandleMouseClick(_filled.Width, 0);
            Assert.That(_filled.PageIndex, Is.EqualTo(1));

            _filled.HandleMouseClick(_filled.Width, 0);
            Assert.That(_filled.PageIndex, Is.EqualTo(2));

            _filled.HandleMouseClick(0, 0);
            Assert.That(_filled.PageIndex, Is.EqualTo(1));

            return;
        }

        [Test]
        public void Test_ToString()
        {
            Assert.That(_filled.ToString(), Is.EqualTo("page 1/3"));

            _filled.PageIndex = 1;
            Assert.That(_filled.ToString(), Is.EqualTo("page 2/3"));

            return;
        }

        [Test]
        public void Test_Zeroing_ItemCount_Reset_Properties()
        {
            _filled.ItemCount = 3;
            _filled.PageSize = 1;
            _filled.PageIndex = 2;

            _filled.ItemCount = 4;
            Assert.That(_filled.PageIndex, Is.EqualTo(2));

            _filled.ItemCount = 0;
            Assert.That(_filled.PageIndex, Is.EqualTo(0));

            return;
        }

        #region TestingPagineerControl        

        class TestingPagineerControl :
            PagineerControl
        {
            public event EventHandler Repainting;

            protected override void Repaint()
            {
                if (Repainting != null)
                    Repainting(this, new EventArgs());

                return;
            }

            public new void HandleMouseClick(int x, int y) {
                base.HandleMouseClick(x, y);
            }          
        }

        #endregion
    }
}
