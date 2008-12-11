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
using NUnit.TestUtilities;
using System.Windows.Forms;
using NUnit.UiException.Tests.data;
using System.Drawing;
using NUnit.UiException.Controls;

namespace NUnit.UiException.Tests.Controls
{
    [TestFixture]
    public class TestCodeBoxComposite
    {
        TestResource _textCodeFile;

        private TestingCodeBoxComposite _empty;
        private TestingCodeBoxComposite _filled;

        private ExceptionItem _existingItem;
        private ExceptionItem _unknownItem;

        private int _leaveNotification;

        [SetUp]
        public void SetUp()
        {
            _empty = new TestingCodeBoxComposite();
            _filled = new TestingCodeBoxComposite();

            _textCodeFile = new TestResource("TextCode.txt");
            _existingItem = new ExceptionItem(_textCodeFile.Path, 13);
            _unknownItem = new ExceptionItem("unknown_file.txt", 1);

            _filled.ExceptionSource = _existingItem;

            Assert.That(_empty.CodeBox, Is.Not.Null);
            Assert.That(_empty.ScrollingDistance, Is.GreaterThan(0));

            _filled.MouseLeaveWindow += new EventHandler(_filled_CursorLeaveWindow);

            return;
        }

        [TearDown]
        public void TearDown()
        {
            if (_textCodeFile != null)
            {
                _textCodeFile.Dispose();
                _textCodeFile = null;
            }
        }

        void _filled_CursorLeaveWindow(object sender, EventArgs e)
        {
            _leaveNotification++;
        }

        [Test]
        public void Test_Can_Set_ItemSource()
        {
            Assert.That(_empty.ExceptionSource, Is.Null);
            Assert.That(_empty.CodeBox.Text, Is.EqualTo(""));

            _empty.ExceptionSource = _existingItem;
            Assert.That(_empty.ExceptionSource, Is.EqualTo(_existingItem));
            Assert.That(_empty.CodeBox.HighlightedLine, Is.EqualTo(13), "bad highlighted line");
            Assert.That(_empty.ErrorMessage, Is.Null);

            _empty.ExceptionSource = _unknownItem;
            Assert.That(_empty.ExceptionSource, Is.EqualTo(_unknownItem));
            Assert.That(_empty.ErrorMessage, Is.Not.Null);
            Assert.That(_empty.ErrorMessage, Text.StartsWith("Fail to open file"));
            

            _empty.ExceptionSource = null;
            Assert.That(_empty.CodeBox.CurrentLineNumber, Is.EqualTo(0));

            return;
        }       

        [Test]
        public void Test_HandleMouseHoverLeft()
        {
            Assert.That(_filled.Translation, Is.EqualTo(CodeBoxComposite.TranslationDirection.None));
            Assert.That(_filled.Timer.Enabled, Is.False);

            _filled.HandleMouseHoverLeft();

            Assert.That(_filled.Translation, Is.EqualTo(CodeBoxComposite.TranslationDirection.Left));
            Assert.That(_filled.Timer.Enabled, Is.True);

            return;
        }

        [Test]
        public void Test_HandleMouseHoverRight()
        {
            Assert.That(_filled.Translation, Is.EqualTo(CodeBoxComposite.TranslationDirection.None));
            Assert.That(_filled.Timer.Enabled, Is.False);

            _filled.HandleMouseHoverRight();

            Assert.That(_filled.Translation, Is.EqualTo(CodeBoxComposite.TranslationDirection.Right));
            Assert.That(_filled.Timer.Enabled, Is.True);

            return;
        }

        [Test]
        public void Test_HandleMouseHoverUp()
        {
            Assert.That(_filled.Translation, Is.EqualTo(CodeBoxComposite.TranslationDirection.None));
            Assert.That(_filled.Timer.Enabled, Is.False);

            _filled.HandleMouseHoverUp();

            Assert.That(_filled.Translation, Is.EqualTo(CodeBoxComposite.TranslationDirection.Up));
            Assert.That(_filled.Timer.Enabled, Is.True);            

            return;
        }

        [Test]
        public void Test_HandleTimerTick_Up()
        {
            int startingLine;

            startingLine = _filled.CodeBox.CurrentLineNumber;
            _filled.ScrollingDistance = _filled.CodeBox.Viewport.CharHeight;            

            _filled.HandleMouseHoverUp();
            Assert.That(_filled.CodeBox.CurrentLineNumber, Is.EqualTo(startingLine));

            _filled.HandleTimerTick();
            Assert.That(_filled.CodeBox.CurrentLineNumber, Is.EqualTo(startingLine - 1));

            _filled.HandleTimerTick();
            Assert.That(_filled.CodeBox.CurrentLineNumber, Is.EqualTo(startingLine - 2));

            for (int i = 0; i < 200; ++i)
                _filled.HandleTimerTick();

            Assert.That(_filled.CodeBox.CurrentLineNumber, Is.EqualTo(1));
            Assert.That(_filled.Timer.Enabled, Is.False);
           
            return;
        }

        [Test]
        public void Test_ScrollingDistance()
        {
            int startingLine;

            startingLine = _filled.CodeBox.CurrentLineNumber;

            _filled.ScrollingDistance = 2;
            Assert.That(_filled.ScrollingDistance, Is.EqualTo(2));

            _filled.ScrollingDistance = _filled.CodeBox.Viewport.CharHeight + 0.5d;
            Assert.That(_filled.CodeBox.CurrentLineNumber, Is.EqualTo(startingLine));

            _filled.CodeBox.TranslateView(0, _filled.ScrollingDistance);
            Assert.That(_filled.CodeBox.CurrentLineNumber, Is.EqualTo(startingLine + 1));

            return;
        }

        [Test]
        public void Test_HandleMouseHoverDown()
        {            
            Assert.That(_filled.Translation, Is.EqualTo(CodeBoxComposite.TranslationDirection.None));
            Assert.That(_filled.Timer.Enabled, Is.False);

            _filled.HandleMouseHoverDown();

            Assert.That(_filled.Translation, Is.EqualTo(CodeBoxComposite.TranslationDirection.Down));
            Assert.That(_filled.Timer.Enabled, Is.True);
        }

        [Test]
        public void Test_HandleTimerTick_Left()
        {
            _filled.CodeBox.Viewport.Location = new PointF(3, 0);

            _filled.ScrollingDistance = 2;

            _filled.HandleMouseHoverLeft();

            _filled.HandleTimerTick();
            Assert.That(_filled.CodeBox.Viewport.Location, Is.EqualTo(new PointF(1, 0)));
            Assert.That(_filled.Timer.Enabled, Is.True);

            _filled.HandleTimerTick();
            Assert.That(_filled.CodeBox.Viewport.Location, Is.EqualTo(new PointF(0, 0)));
            Assert.That(_filled.Timer.Enabled, Is.False);

            return;
        }

        [Test]
        public void Test_HandleTimerTick_Right()
        {
            double textwidth;
            double w;
            int i;

            w = _filled.CodeBox.Viewport.Width;
            textwidth = _filled.CodeBox.Viewport.TextSource.MaxLength * _filled.CodeBox.Viewport.CharWidth;

            _filled.CodeBox.Viewport.Location = new PointF(0, 0);

            _filled.ScrollingDistance = 2;

            _filled.HandleMouseHoverRight();

            _filled.HandleTimerTick();
            Assert.That(_filled.CodeBox.Viewport.Location, Is.EqualTo(new PointF(2, 0)));
            Assert.That(_filled.Timer.Enabled, Is.True);

            _filled.HandleTimerTick();
            Assert.That(_filled.CodeBox.Viewport.Location, Is.EqualTo(new PointF(4, 0)));

            _filled.ScrollingDistance = 100;

            for (i = 0; i < 100; ++i)
            {
                _filled.HandleTimerTick();
            }

            Assert.That(_filled.CodeBox.Viewport.Location.X, Is.LessThanOrEqualTo(textwidth - w));
            Assert.That(_filled.Timer.Enabled, Is.False);

            return;
        }

        [Test]
        public void Test_HandleMouseHoverCode()
        {
            _filled.HandleMouseHoverUp();

            _filled.HandleMouseHoverCode();

            Assert.That(_filled.Translation, Is.EqualTo(CodeBoxComposite.TranslationDirection.None));
            Assert.That(_filled.Timer.Enabled, Is.False);

            return;
        }

        [Test]
        public void Test_HandleTimerTick_Down()
        {
            int startingLine;

            startingLine = _filled.CodeBox.CurrentLineNumber;

            _filled.ScrollingDistance = _filled.CodeBox.Viewport.CharHeight + 0.5d;

            _filled.HandleMouseHoverDown();
            Assert.That(_filled.CodeBox.CurrentLineNumber, Is.EqualTo(startingLine));

            _filled.HandleTimerTick();
            Assert.That(_filled.CodeBox.CurrentLineNumber, Is.EqualTo(startingLine + 1));

            _filled.HandleTimerTick();
            Assert.That(_filled.CodeBox.CurrentLineNumber, Is.EqualTo(startingLine + 2));

            for (int i = 0; i < 200; ++i)
                _filled.HandleTimerTick();

            Assert.That(_filled.CodeBox.CurrentLineNumber, Is.EqualTo(42));
            Assert.That(_filled.Timer.Enabled, Is.False);           

            return;
        }

        [Test]
        public void Test_Yield_Focus_To_CodeBox()
        {
            _filled.ActiveControl = null;
            Assert.That(_filled.ActiveControl, Is.Null);

            _filled.HandleMouseHoverCode();

            Assert.That(_filled.ActiveControl, Is.EqualTo(_filled.CodeBox));

            return;
        }

        [Test]
        public void Test_Ability_To_Disable_HoverButtons()
        {
            double max;

            max = 5000; 
            
            _filled.CodeBox.Viewport.SetPosition(0, 0);
            Assert.That(_filled.BtnUp.Enabled, Is.False);
            Assert.That(_filled.BtnLeft.Enabled, Is.False);
            Assert.That(_filled.BtnRight.Enabled, Is.True);
            Assert.That(_filled.BtnDown.Enabled, Is.True);
            
            _filled.CodeBox.Viewport.SetPosition(max, 0);
            Assert.That(_filled.BtnUp.Enabled, Is.False);
            Assert.That(_filled.BtnLeft.Enabled, Is.True);
            Assert.That(_filled.BtnRight.Enabled, Is.False);
            Assert.That(_filled.BtnDown.Enabled, Is.True);

            _filled.CodeBox.Viewport.SetPosition(0, max);
            Assert.That(_filled.BtnUp.Enabled, Is.True);
            Assert.That(_filled.BtnLeft.Enabled, Is.False);
            Assert.That(_filled.BtnRight.Enabled, Is.True);
            Assert.That(_filled.BtnDown.Enabled, Is.False);

            return;
        }

        [Test]
        public void Test_HandleLeaveEvent()
        {
            _filled.HandleMouseHoverDown();
            Assert.That(_filled.Translation, Is.EqualTo(CodeBoxComposite.TranslationDirection.Down));
            Assert.That(_filled.Timer.Enabled, Is.True);

            Assert.That(_leaveNotification, Is.EqualTo(0));

            _filled.HandleLeaveEvent();

            Assert.That(_filled.Translation, Is.EqualTo(CodeBoxComposite.TranslationDirection.None));
            Assert.That(_filled.Timer.Enabled, Is.False);

            Assert.That(_leaveNotification, Is.EqualTo(1));

            return;
        }
        
        #region CodeControl

        class TestingCodeBoxComposite :
            CodeBoxComposite
        {
            public new string ErrorMessage {
                get { return (base.ErrorMessage); }
            }

            public new CodeBox CodeBox {
                get { return (base.CodeBox); }
            }

            public new void HandleMouseHoverUp() {
                base.HandleMouseHoverUp();
            }

            public new void HandleMouseHoverDown() {
                base.HandleMouseHoverDown();
            }

            public new void HandleMouseHoverLeft() {
                base.HandleMouseHoverLeft();
            }

            public new void HandleMouseHoverRight() {
                base.HandleMouseHoverRight();
            }

            public new void HandleMouseHoverCode() {
                base.HandleMouseHoverCode();
            }

            public new TranslationDirection Translation {
                get { return (base.Translation); }
            }

            public new void HandleLeaveEvent()
            {
                base.HandleLeaveEvent();
            }

            public new Timer Timer {
                get { return (base.Timer); }
            }

            public new void HandleTimerTick() {
                base.HandleTimerTick();
            }

            public new HoverButton BtnUp {
                get { return (base.BtnUp); }
            }

            public new HoverButton BtnLeft {
                get { return (base.BtnLeft); }
            }

            public new HoverButton BtnRight {
                get { return (base.BtnRight); }
            }

            public new HoverButton BtnDown {
                get { return (base.BtnDown); }
            }
        }

        #endregion
    }
}
