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
    public class TestCodeViewport
    {
        private TestingCodeViewport _viewport;
        private TestingCodeViewport _filled;
        private string _text;

        private int _locationNotification;
        private int _textNotification;
        private int _highlightNotification;

        [SetUp]
        public void SetUp()
        {
            DefaultTextManager block;

            _viewport = new TestingCodeViewport();

            _filled = new TestingCodeViewport();
            _filled.SetCharSize(1, 1);
            _filled.SetViewport(2, 4);

            _text = "111\r\n" +
                    "222\r\n" +
                    "333\r\n" +
                    "444\r\n" +
                    "555\r\n" +
                    "666\r\n" +
                    "777\r\n";

            block = new DefaultTextManager();
            block.Text = _text;

            _filled.TextSource = block;

            _filled.LocationChanged += new EventHandler(_filled_LocationChanged);
            _filled.TextChanged += new EventHandler(_filled_TextChanged);
            _filled.HighLightChanged += new EventHandler(_filled_HighLightChanged);

            _textNotification = 0;

            return;
        }

        void _filled_HighLightChanged(object sender, EventArgs e)
        {
            _highlightNotification++;
        }

        void _filled_TextChanged(object sender, EventArgs e)
        {
            _textNotification++;
        }

        void _filled_LocationChanged(object sender, EventArgs e)
        {
            _locationNotification++;
        }

        [Test]
        public void Test_Default()
        {
            Assert.That(_viewport.Width, Is.EqualTo(0));
            Assert.That(_viewport.Height, Is.EqualTo(0));
            Assert.That(_viewport.CharWidth, Is.EqualTo(0));
            Assert.That(_viewport.CharHeight, Is.EqualTo(0));
            Assert.That(_viewport.Text, Is.EqualTo(""));
            Assert.That(_viewport.HighLightedLineIndex, Is.EqualTo(-1));

            Assert.That(_viewport.TextSource, Is.Not.Null);
            Assert.That(_viewport.TextSource.Text, Is.EqualTo(""));

            return;
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "width must be > 0",
            MatchType = MessageMatch.Contains)]
        public void Test_SetViewport_Throws_InvalidWidthException()
        {
            _viewport.SetViewport(0, 1); // throws exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "height must be > 0",
            MatchType = MessageMatch.Contains)]
        public void Test_SetViewport_Throws_InvalidHeightException()
        {
            _viewport.SetViewport(1, 0); // throws exception
        }

        [Test]
        public void Test_SetViewport()
        {
            _viewport.SetViewport(10, 20);
            Assert.That(_viewport.Width, Is.EqualTo(10));
            Assert.That(_viewport.Height, Is.EqualTo(20));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "width must be > 0",
            MatchType = MessageMatch.Contains)]
        public void Test_SetCharSize_Throws_InvalidWidthException()
        {
            _viewport.SetCharSize(0, 1); // throws exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "height must be > 0",
            MatchType = MessageMatch.Contains)]
        public void Test_SetCharSize_Throws_InvalidHeightException()
        {
            _viewport.SetCharSize(1, 0); // throws exception
        }

        [Test]
        public void Test_SetCharSize()
        {
            _viewport.SetCharSize(1, 2);
            Assert.That(_viewport.CharWidth, Is.EqualTo(1));
            Assert.That(_viewport.CharHeight, Is.EqualTo(2));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_SetTextSource_Throws_NullBlockException()
        {
            _viewport.TextSource = null; // throws exception
        }

        [Test]
        public void Test_SetTextSource()
        {
            DefaultTextManager block;

            block = new DefaultTextManager();
            block.Text = "hello world!";

            _viewport.TextSource = block;
            Assert.That(_viewport.Text, Is.EqualTo("hello world!"));
            Assert.That(_viewport.TextSource.Text, Is.EqualTo("hello world!"));

            return;
        }

        [Test]
        public void Test_GetTextUpperBoundCoordinateFromLineIndex()
        {
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(0), Is.EqualTo(new PointF(0, 0)));
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(2), Is.EqualTo(new PointF(0, 2)));
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(5), Is.EqualTo(new PointF(0, 5)));
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(6), Is.EqualTo(new PointF(0, 6)));

            _filled.SetCharSize(1, 0.5f);
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(0), Is.EqualTo(new PointF(0, 0)));
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(2), Is.EqualTo(new PointF(0, 1)));
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(5), Is.EqualTo(new PointF(0, 2.5f)));
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(6), Is.EqualTo(new PointF(0, 3)));

            _filled.SetCharSize(1, 2);
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(0), Is.EqualTo(new PointF(0, 0)));
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(2), Is.EqualTo(new PointF(0, 4)));
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(5), Is.EqualTo(new PointF(0, 10)));
            Assert.That(_filled.GetTextUpperBoundCoordinateFromLineIndex(6), Is.EqualTo(new PointF(0, 12)));

            return;
        }

        [Test]
        public void Test_GetLineIndexFromTextUpperBoundCoordinate()
        {
            Assert.That(_filled.GetLineIndexFromTextUpperBoundCoordinate(0), Is.EqualTo(0));
            Assert.That(_filled.GetLineIndexFromTextUpperBoundCoordinate(2), Is.EqualTo(2));
            Assert.That(_filled.GetLineIndexFromTextUpperBoundCoordinate(5), Is.EqualTo(5));
            Assert.That(_filled.GetLineIndexFromTextUpperBoundCoordinate(6), Is.EqualTo(6));

            _filled.SetCharSize(1, 0.5f);
            Assert.That(_filled.GetLineIndexFromTextUpperBoundCoordinate(0), Is.EqualTo(0));
            Assert.That(_filled.GetLineIndexFromTextUpperBoundCoordinate(0.5d), Is.EqualTo(1));
            Assert.That(_filled.GetLineIndexFromTextUpperBoundCoordinate(0.6d), Is.EqualTo(1));
            Assert.That(_filled.GetLineIndexFromTextUpperBoundCoordinate(5), Is.EqualTo(10));
            Assert.That(_filled.GetLineIndexFromTextUpperBoundCoordinate(6), Is.EqualTo(12));

            return;
        }

        [Test]
        public void Test_ScrollToLine()
        {
            _filled.ScrollToLine(0);
            Assert.That(_filled.Location, Is.EqualTo(new PointF(0, 0)));

            _filled.ScrollToLine(1);
            Assert.That(_filled.Location, Is.EqualTo(new PointF(0, 1)));

            _filled.SetCharSize(1, 2);
            _filled.ScrollToLine(1);
            Assert.That(_filled.Location, Is.EqualTo(new PointF(0, 2)));

            // NUnit.UiException.Tests ability to remain within Text Bounds

            _filled.ScrollToLine(-1);
            Assert.That(_filled.Location.Y, Is.EqualTo(0));

            _filled.ScrollToLine(42);
            Assert.That(_filled.Location.Y,
                Is.EqualTo(_filled.CharHeight * _filled.TextSource.LineCount - _filled.Height));

            return;
        }

        [Test]
        public void Test_Location()
        {
            _filled.HighLightedLineIndex = -10;

            Assert.That(_filled.Location, Is.Not.Null);
            Assert.That(_filled.Location, Is.EqualTo(new PointF(0, 0)));

            Assert.That(_filled.VisibleLines, Is.EqualTo(4));
            Assert.That(_filled[0], Is.Not.Null);

            //
            // Check what can be seen in (0, 0)
            //
            Assert.That(_filled[0], Is.EqualTo(new PaintLineLocation(0, "111", new PointF(0, 0), false)));
            Assert.That(_filled[1], Is.EqualTo(new PaintLineLocation(1, "222", new PointF(0, 1), false)));
            Assert.That(_filled[2], Is.EqualTo(new PaintLineLocation(2, "333", new PointF(0, 2), false)));
            Assert.That(_filled[3], Is.EqualTo(new PaintLineLocation(3, "444", new PointF(0, 3), false)));

            //
            // Check what can be seen in (1, 2)
            //
            _filled.Location = new Point(1, 2);
            Assert.That(_filled.VisibleLines, Is.EqualTo(4));
            Assert.That(_filled[0], Is.EqualTo(new PaintLineLocation(2, "333", new PointF(-1, 0), false)));
            Assert.That(_filled[1], Is.EqualTo(new PaintLineLocation(3, "444", new PointF(-1, 1), false)));
            Assert.That(_filled[2], Is.EqualTo(new PaintLineLocation(4, "555", new PointF(-1, 2), false)));
            Assert.That(_filled[3], Is.EqualTo(new PaintLineLocation(5, "666", new PointF(-1, 3), false)));

            //
            // Check what happens when assigning non
            // integer value to Location.Y
            //
            _filled.Location = new PointF(0, 0.5f);
            Assert.That(_filled.VisibleLines, Is.EqualTo(5));
            Assert.That(_filled[0], Is.EqualTo(new PaintLineLocation(0, "111", new PointF(0, -0.5f), false)));
            Assert.That(_filled[1], Is.EqualTo(new PaintLineLocation(1, "222", new PointF(0, 0.5f), false)));
            Assert.That(_filled[2], Is.EqualTo(new PaintLineLocation(2, "333", new PointF(0, 1.5f), false)));
            Assert.That(_filled[3], Is.EqualTo(new PaintLineLocation(3, "444", new PointF(0, 2.5f), false)));

            //
            // Check what happens when assigning non
            // integer value to Location.Y
            //
            _filled.Location = new PointF(0.5f, 1);
            Assert.That(_filled.VisibleLines, Is.EqualTo(4));
            Assert.That(_filled[0], Is.EqualTo(new PaintLineLocation(1, "222", new PointF(-0.5f, 0), false)));
            Assert.That(_filled[1], Is.EqualTo(new PaintLineLocation(2, "333", new PointF(-0.5f, 1), false)));
            Assert.That(_filled[2], Is.EqualTo(new PaintLineLocation(3, "444", new PointF(-0.5f, 2), false)));
            Assert.That(_filled[3], Is.EqualTo(new PaintLineLocation(4, "555", new PointF(-0.5f, 3), false)));

            //
            // Check what can be seen when viewport.Location.Y
            // makes viewport partially out of the text area
            //
            _filled.Location = new PointF(0, -2);
            Assert.That(_filled.VisibleLines, Is.EqualTo(4));
            Assert.That(_filled[0], Is.EqualTo(new PaintLineLocation(-2, "", new PointF(0, 0), false)));
            Assert.That(_filled[1], Is.EqualTo(new PaintLineLocation(-1, "", new PointF(0, 1), false)));
            Assert.That(_filled[2], Is.EqualTo(new PaintLineLocation( 0, "111", new PointF(0, 2), false)));
            Assert.That(_filled[3], Is.EqualTo(new PaintLineLocation( 1, "222", new PointF(0, 3), false)));

            _filled.Location = new PointF(0, -5);
            Assert.That(_filled.VisibleLines, Is.EqualTo(0));

            _filled.Location = new PointF(0, 5);
            Assert.That(_filled.VisibleLines, Is.EqualTo(2));
            Assert.That(_filled[0], Is.EqualTo(new PaintLineLocation(5, "666", new PointF(0, 0), false)));
            Assert.That(_filled[1], Is.EqualTo(new PaintLineLocation(6, "777", new PointF(0, 1), false)));

            _filled.Location = new PointF(0, 7);
            Assert.That(_filled.VisibleLines, Is.EqualTo(0));

            //
            // Check what can be seen when viewport.Location.X
            // makes viewport partially out of the text area
            //
            _filled.Location = new PointF(-1, 1);
            Assert.That(_filled.VisibleLines, Is.EqualTo(4));
            Assert.That(_filled[0], Is.EqualTo(new PaintLineLocation(1, "222", new PointF(1, 0), false)));
            Assert.That(_filled[1], Is.EqualTo(new PaintLineLocation(2, "333", new PointF(1, 1), false)));
            Assert.That(_filled[2], Is.EqualTo(new PaintLineLocation(3, "444", new PointF(1, 2), false)));
            Assert.That(_filled[3], Is.EqualTo(new PaintLineLocation(4, "555", new PointF(1, 3), false)));

            _filled.Location = new PointF(-5, 1);
            Assert.That(_filled.VisibleLines, Is.EqualTo(0));

            _filled.Location = new PointF(2, 1);
            Assert.That(_filled.VisibleLines, Is.EqualTo(4));
            Assert.That(_filled[0], Is.EqualTo(new PaintLineLocation(1, "222", new PointF(-2, 0), false)));
            Assert.That(_filled[1], Is.EqualTo(new PaintLineLocation(2, "333", new PointF(-2, 1), false)));
            Assert.That(_filled[2], Is.EqualTo(new PaintLineLocation(3, "444", new PointF(-2, 2), false)));
            Assert.That(_filled[3], Is.EqualTo(new PaintLineLocation(4, "555", new PointF(-2, 3), false)));

            _filled.Location = new PointF(5, 1);
            Assert.That(_filled.VisibleLines, Is.EqualTo(0));

            return;
        }

        [Test]
        public void Test_Highlighting()
        {
            _filled.HighLightedLineIndex = 2;
            Assert.That(_filled.HighLightedLineIndex, Is.EqualTo(2));

            Assert.That(_filled[0], Is.EqualTo(new PaintLineLocation(0, "111", new PointF(0, 0), false)));
            Assert.That(_filled[1], Is.EqualTo(new PaintLineLocation(1, "222", new PointF(0, 1), false)));
            Assert.That(_filled[2], Is.EqualTo(new PaintLineLocation(2, "333", new PointF(0, 2), true)));
            Assert.That(_filled[3], Is.EqualTo(new PaintLineLocation(3, "444", new PointF(0, 3), false)));

            return;
        }

        [Test]
        public void Test_Foreach()
        {
            List<PaintLineLocation> lst;

            lst = new List<PaintLineLocation>();
            foreach (PaintLineLocation arg in _filled)
                lst.Add(arg);

            Assert.That(lst.Count, Is.EqualTo(4));
            Assert.That(lst[0], Is.EqualTo(new PaintLineLocation(0, "111", new PointF(0, 0), false)));
            Assert.That(lst[1], Is.EqualTo(new PaintLineLocation(1, "222", new PointF(0, 1), false)));
            Assert.That(lst[2], Is.EqualTo(new PaintLineLocation(2, "333", new PointF(0, 2), false)));
            Assert.That(lst[3], Is.EqualTo(new PaintLineLocation(3, "444", new PointF(0, 3), false)));

            return;
        }

        [Test]
        public void Test_Changing_Location_Fire_Event()
        {
            _filled.Location = new PointF(0, 1);
            Assert.That(_locationNotification, Is.EqualTo(1));
        }

        [Test]
        public void Test_SetTextSource_Fires_Event()
        {
            _filled.TextSource = new DefaultTextManager();
            Assert.That(_textNotification, Is.EqualTo(1));
        }

        [Test]
        public void Test_Changing_HighLight_Fire_Event()
        {
            _filled.HighLightedLineIndex = 1;
            Assert.That(_highlightNotification, Is.EqualTo(1));
        }

        [Test]
        public void Test_SetPosition_Y_With_Top_And_Bottom_Bounds()
        {
            DefaultTextManager block;

            _filled.SetPosition(0, 0);
            Assert.That(_filled.Location, Is.EqualTo(new PointF(0, 0)));

            _filled.SetPosition(0, -1);
            Assert.That(_filled.Location, Is.EqualTo(new PointF(0, 0)));

            _filled.SetPosition(0, 5);
            Assert.That(_filled.Location, Is.EqualTo(new PointF(0, 3)));

            /// NUnit.UiException.Tests that Location.Y cannot become negative

            block = new DefaultTextManager();
            block.Text = "***";

            _filled.TextSource = block;
            _filled.SetPosition(0, 5);
            Assert.That(_filled.Location, Is.EqualTo(new PointF(0, 0)));

            return;
        }

        [Test]
        public void Test_SetPosition_X_With_Left_And_Right_Bounds()
        {
            DefaultTextManager block;

            _filled.SetPosition(-1, 0);
            Assert.That(_filled.Location, Is.EqualTo(new PointF(0, 0)));

            _filled.SetPosition(2, 0);
            Assert.That(_filled.Location, Is.EqualTo(new PointF(1, 0)));

            /// NUnit.UiException.Tests that Location.X cannot become negative

            block = new DefaultTextManager();
            block.Text = "*";

            _filled.TextSource = block;
            _filled.SetPosition(1, 0);
            Assert.That(_filled.Location, Is.EqualTo(new PointF(0, 0)));

            return;
        }

        [Test]
        public void Test_Remaining_Distances()
        {
            //
            // Left
            //

            _filled.SetPosition(0, 0);
            Assert.That(_filled.RemainingLeft, Is.EqualTo(0));

            _filled.SetPosition(1, 0);
            Assert.That(_filled.RemainingLeft, Is.EqualTo(1));

            //
            // Right
            //

            _filled.SetPosition(0, 0);
            Assert.That(_filled.RemainingRight, Is.EqualTo(1));

            _filled.SetPosition(1, 0);
            Assert.That(_filled.RemainingRight, Is.EqualTo(0));

            //
            // Top
            //

            _filled.SetPosition(0, 0);
            Assert.That(_filled.RemainingTop, Is.EqualTo(0));
            _filled.SetPosition(0, 1);
            Assert.That(_filled.RemainingTop, Is.EqualTo(1));

            //
            // Bottom
            //

            _filled.SetPosition(0, 0);
            Assert.That(_filled.RemainingBottom, Is.EqualTo(3));
            _filled.SetPosition(0, 3);
            Assert.That(_filled.RemainingBottom, Is.EqualTo(0));

            return;
        }        

        #region TestingCodeViewport

        class TestingCodeViewport :
            CodeViewport
        {
            public TestingCodeViewport()
            {
                // nothing to do
            }

            public new PointF GetTextUpperBoundCoordinateFromLineIndex(int lineIndex)
            {
                return (base.GetTextUpperBoundCoordinateFromLineIndex(lineIndex));
            }

            public new int GetLineIndexFromTextUpperBoundCoordinate(double y)
            {
                return (base.GetLineIndexFromTextUpperBoundCoordinate(y)); 
            }
        }

        #endregion
    }
}
