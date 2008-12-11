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
using NUnit.Framework;
using NUnit.UiException;
using System.Drawing;
using NUnit.UiException.Controls;

namespace NUnit.UiException.Tests.Controls
{
    [TestFixture]
    public class TestCodeBox
    {
        private InternalCodeBox _empty;
        private InternalCodeBox _filled;

        private int _repaintNotification;
        private int _textChangedNotification;

        [SetUp]
        public void SetUp()
        {
            _empty = new InternalCodeBox();

            _filled = new InternalCodeBox();
            _filled.Text = "111\r\n" +
                           "222\r\n" +
                           "333\r\n";
            _filled.HighlightedLine = 1;

            _filled.Repainted += new RepaintEventArgs(_filled_Repainted);
            _filled.TextChanged += new EventHandler(_filled_TextChanged);

            _repaintNotification = 0;            

            return;
        }

        void _filled_TextChanged(object sender, EventArgs e)
        {
            _textChangedNotification++;
        }

        void _filled_Repainted(object sender, EventArgs e)
        {
            _repaintNotification++;
        }

        [Test]
        public void Test_Default()
        {
            Assert.That(_empty.Text, Is.EqualTo(""));
            Assert.That(_empty.HighlightedLine, Is.EqualTo(0));
            Assert.That(_empty.Viewport, Is.Not.Null);
            Assert.That(_empty.FirstLine, Is.EqualTo(""));
            Assert.That(_empty.CurrentLineNumber, Is.EqualTo(1));
            Assert.That(_empty.MouseWheelDistance, Is.EqualTo(CodeBox.DEFAULT_MOUSEWHEEL_DISTANCE));

            Assert.That(_empty.Viewport.CharHeight, Is.GreaterThan(1));
            Assert.That(_empty.Viewport.CharWidth, Is.GreaterThan(1));
            Assert.That(_empty.Viewport.Width, Is.GreaterThan(1));
            Assert.That(_empty.Viewport.Height, Is.GreaterThan(1));

            return;
        }

        [Test]
        public void Test_Filled()
        {
            Assert.That(_filled.Text, Is.EqualTo("111\r\n222\r\n333\r\n"));
            Assert.That(_filled.HighlightedLine, Is.EqualTo(1));
            Assert.That(_filled.FirstLine, Is.EqualTo("111"));
            Assert.That(_filled.CurrentLineNumber, Is.EqualTo(1));

            return;
        }

        [Test]
        public void Test_Setting_MouseWheelDistance()
        {
            _filled.MouseWheelDistance = 4;
            Assert.That(_filled.MouseWheelDistance, Is.EqualTo(4));

            _filled.MouseWheelDistance = 6;
            Assert.That(_filled.MouseWheelDistance, Is.EqualTo(6));

            return;
        }

        [Test]
        public void Test_Setting_Text()
        {
            _filled.Text = "hello world";
            Assert.That(_repaintNotification, Is.EqualTo(1));
            Assert.That(_textChangedNotification, Is.EqualTo(1));

            return;
        }

        [Test]
        public void Test_Setting_Size_Invalidate_Box()
        {
            _filled.Width = 200;
            Assert.That(_repaintNotification, Is.EqualTo(1));

            _filled.Height = 400;
            Assert.That(_repaintNotification, Is.EqualTo(2));

            Assert.That(_filled.Viewport.Width, Is.EqualTo(200));
            Assert.That(_filled.Viewport.Height, Is.EqualTo(400));

            return;
        }

        [Test]
        public void Test_Setting_HighlighedLine_Invalidate_Box()
        {
            _filled.HighlightedLine = 2;
            Assert.That(_repaintNotification, Is.EqualTo(1));
        }

        [Test]
        public void Test_Changing_Location_Invalidate_Box()
        {
            _filled.Viewport.Location = new PointF(0, 1);
            Assert.That(_repaintNotification, Is.EqualTo(1));
        }

        [Test]
        public void Test_TranslateView()
        {
            _filled.Text = "******\r\n******\r\n******\r\n******\r\n******\r\n";
            _filled.Viewport.SetCharSize(1, 1);
            _filled.Viewport.SetViewport(1, 1);

            _filled.TranslateView(0, 0);
            Assert.That(_filled.Viewport.Location, Is.EqualTo(new PointF(0, 0)));

            _filled.TranslateView(2, 1);
            Assert.That(_filled.Viewport.Location, Is.EqualTo(new PointF(2, 1)));

            _filled.TranslateView(3, 1);
            Assert.That(_filled.Viewport.Location, Is.EqualTo(new PointF(5, 2)));

            return;
        }

        [Test]
        public void Test_CurrentLineNumber()
        {
            _filled.Viewport.SetViewport(1, 1);
            _filled.Viewport.SetCharSize(1, 1);

            Assert.That(_filled.CurrentLineNumber, Is.EqualTo(1));

            _filled.TranslateView(0, 1000);

            Assert.That(_filled.CurrentLineNumber,
                Is.EqualTo(_filled.Viewport.TextSource.LineCount));

            _filled.TranslateView(0, -2000);
            Assert.That(_filled.CurrentLineNumber, Is.EqualTo(1));

            return;
        }

        [Test]
        public void Test_MouseWheel_Up()
        {
            _filled.Viewport.SetViewport(1, 1);
            _filled.Viewport.SetCharSize(1, 1);

            _filled.Viewport.SetPosition(0, 2);

            _filled.MouseWheelDistance = 1;           

            _filled.HandleMouseWheelUp();
            Assert.That(_filled.Viewport.Location, Is.EqualTo(new PointF(0, 1)));

            _filled.HandleMouseWheelUp();
            Assert.That(_filled.Viewport.Location, Is.EqualTo(new PointF(0, 0)));

            return;
        }

        [Test]
        public void Test_MouseWheel_Down()
        {
            _filled.Viewport.SetViewport(1, 1);
            _filled.Viewport.SetCharSize(1, 1);

            _filled.Viewport.SetPosition(0, 0);

            _filled.MouseWheelDistance = 1;

            _filled.HandleMouseWheelDown();
            Assert.That(_filled.Viewport.Location, Is.EqualTo(new PointF(0, 1)));

            _filled.HandleMouseWheelDown();
            Assert.That(_filled.Viewport.Location, Is.EqualTo(new PointF(0, 2)));

            return;
        }

        #region InternalCodeBox

        delegate void RepaintEventArgs(object sender, EventArgs e);

        class InternalCodeBox :
            CodeBox
        {
            public event RepaintEventArgs Repainted;

            protected override void Repaint()
            {
                base.Repaint();

                if (Repainted != null)
                    Repainted(this, new EventArgs());

                return;
            }

            public new void HandleMouseWheelUp()
            {
                base.HandleMouseWheelUp();
            }

            public new void HandleMouseWheelDown()
            {
                base.HandleMouseWheelDown();
            }
        }

        #endregion
    }
}
