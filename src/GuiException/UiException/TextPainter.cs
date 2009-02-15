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
using System.Drawing;

namespace NUnit.UiException
{
    /// <summary>
    /// Helper to draw a rectangle bounded string of characters into a Graphics object.
    /// If the length of the string exceeds the size of the destination rectangle, TextPainter
    /// draws only visible part of the text selecting either the start or the end of the
    /// string according the value of ClippingTextPolicy.
    /// </summary>
    public class TextPainter
    {
        public enum ClipTextPolicy
        {
            RemoveBeginning,
            RemoveEnding,
        }

        private Font _font;
        private ClipTextPolicy _policy;

        public TextPainter(Font font)
        {
            UiExceptionHelper.CheckNotNull(font, "font");

            _font = font;

            return;
        }

        public Font Font
        {
            get { return (_font); }
        }

        public Size MeasureText(string text, Graphics g)
        {
            SizeF res;

            UiExceptionHelper.CheckNotNull(g, "g");

            res = g.MeasureString(text, _font);

            return (new Size((int)Math.Round(res.Width), (int)Math.Round(res.Height)));
        }

        public string GetVisibleText(string text, Graphics g, int width, int height)
        {
            string str;
            Size sz;

            UiExceptionHelper.CheckNotNull(g, "g");

            if (text == null)
                text = "";

            sz = MeasureText(text, g);
            if (sz.Width <= width)
                return (text);

            str = text;
            while (str.Length > 0 && sz.Width > width)
            {
                str = str.Substring(1);
                sz = MeasureText("..." + str, g);
            }

            if (_policy == ClipTextPolicy.RemoveBeginning)
                return ("..." + str);

            return (str + "...");
        }      

        public void Draw(string text, Graphics g, Brush textBrush, int left, int top, int width, int height)
        {
            UiExceptionHelper.CheckNotNull(g, "g");
            UiExceptionHelper.CheckNotNull(textBrush, "textBrush");

            if (text == null)
                text = "";

            text = GetVisibleText(text, g, width, height);
            g.DrawString(text, _font, textBrush, left, top);

            return;
        }

        public ClipTextPolicy ClippingPolicy
        {
            get { return (_policy); }
            set { _policy = value; }
        }       
    }
}

