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
using System.Drawing;
using System.Drawing.Imaging;

namespace NUnit.UiException.Tests
{
    [TestFixture]
    public class TestTextPainter
    {
        private Graphics _g;
        private SolidBrush _brush;
        private TextPainter _painter;

        [SetUp]
        public void SetUp()
        {
            Image img;

            _painter = new TextPainter(new Font("Tahoma", 12));

            img = new Bitmap(200, 50, PixelFormat.Format32bppArgb);
            _g = Graphics.FromImage(img);
            _brush = new SolidBrush(Color.Black);

            return;
        }

        [TearDown]
        public void TearDown()
        {
            _g.Dispose();
            _brush.Dispose();
        }

        [Test]
        public void Test_Default()
        {
            Assert.That(_painter.ClippingPolicy, Is.EqualTo(TextPainter.ClipTextPolicy.RemoveBeginning));

            return;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_GetVisibleText_Throws_NullGraphicsException()
        {
            _painter.GetVisibleText("text", null, 0, 0); // throws exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Ctor_Throws_NullFontException()
        {
            new TextPainter(null); // throws exception
        }

        [Test]
        public void Test_GetVisibleText()
        {
            // check normal case, when there is enough space
            // to draw string completely

            Assert.That(_painter.GetVisibleText("A_very_long_text", _g, 200, 100), Is.EqualTo("A_very_long_text"));

            // check return of GetVisibleText with a limited
            // drawing area. 

            Assert.That(_painter.GetVisibleText("A_very_long_text", _g, 50, 100), Is.Not.Null);
            Assert.That(_painter.GetVisibleText("A_very_long_text", _g, 50, 100).StartsWith("..."), Is.True);

            // changes clipping policy

            _painter.ClippingPolicy = TextPainter.ClipTextPolicy.RemoveEnding;

            Assert.That(_painter.GetVisibleText("A_very_long_text", _g, 50, 100).EndsWith("..."), Is.True);

            // setting null text is okay
            Assert.That(_painter.GetVisibleText(null, _g, 50, 100), Is.EqualTo(""));

            return;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_MeasureText_throws_NullGraphicsException()
        {
            _painter.MeasureText("text", null); // throws exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_GetVisibleText_throws_NullGraphicsException()
        {
            _painter.GetVisibleText("text", null, 100, 10); // throws exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_Draw_throws_NullGraphicsException()
        {
            _painter.Draw("text", null, _brush, 0, 0, 50, 50); // throws exception
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_Draw_throws_NullBrushException()
        {
            _painter.Draw("text", _g, null, 0, 0, 50, 50); // throws exception
        }
    }
}
