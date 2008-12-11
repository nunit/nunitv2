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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NUnit.UiException.CSharpParser;

namespace NUnit.UiException.Controls
{
    /// <summary>
    /// Displays a formatted text using same font but with words of different colors.
    /// 
    /// This control could have been replaced by a standard RichTextBox control, but
    /// it turned out that RichTextBox:
    ///     - was hard to configure
    ///     - was hard to set the viewport
    ///     - doesn't use double buffer optimization
    ///     - scrolls text one line at a time without be configurable.
    /// 
    /// CodeBox has been written to address these specific issues in order to display
    /// C# source code where exceptions occured.
    /// </summary>
    public partial class CodeBox :
        UserControl
    {
        public new event EventHandler TextChanged;

        /// <summary>
        /// The distance by which scrolling the text upward or downward.
        /// </summary>
        public const double DEFAULT_MOUSEWHEEL_DISTANCE = 20;

        /// <summary>
        /// These constants below address an issue at measure text time
        /// that sometimes can cause big lines of text to be misaligned.
        /// </summary>
        public const float MEASURECHAR_BIG_WIDTH = 5000f;
        public const float MEASURECHAR_BIG_HEIGHT = 100f;

        /// <summary>
        /// Tracks the current portion of text visible to the user.
        /// </summary>
        private CodeViewport _viewport;

        /// <summary>
        /// Stores the distance by which moving the text upward/downward.
        /// </summary>
        private double _wheelDistance;

        /// <summary>
        /// Store all brushes used to display the text at rendering time.
        /// </summary>
        private Dictionary<ClassificationTag, Brush> _brushes;

        /// <summary>
        /// Build a new instance of CodeBox.
        /// </summary>
        public CodeBox()
        {
            InitializeComponent();

            // set styles to notify underlying control we want
            // using double buffer

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, true);

            // create the underlying view port

            _viewport = new CodeViewport();
            _viewport.TextSource = new CSCode();
            _viewport.TextChanged += new EventHandler(_viewport_Changed);
            _viewport.LocationChanged += new EventHandler(_viewport_Changed);
            _viewport.HighLightChanged += new EventHandler(_viewport_Changed);

            // "Courier New" is a good monospace font to display source code.

            Font = new Font("Courier New", 12);            

            MouseWheel += new MouseEventHandler(CodeBox_MouseWheel);
            Resize += new EventHandler(_size_Changed);

            _wheelDistance = DEFAULT_MOUSEWHEEL_DISTANCE;

            // initialize some gdi resources
            // CodeBox distinguishes 4 text styles, so we use 4 different brushes

            _brushes = new Dictionary<ClassificationTag, Brush>();
            _brushes.Add(ClassificationTag.Code, new SolidBrush(Color.Black));
            _brushes.Add(ClassificationTag.Comment, new SolidBrush(Color.Green));
            _brushes.Add(ClassificationTag.Keyword, new SolidBrush(Color.Blue));
            _brushes.Add(ClassificationTag.String, new SolidBrush(Color.Red));

            return;
        }

        /// <summary>
        /// Gets or sets a new font.
        /// When setting a new font, it is strongly recommended to use
        /// a monospace font, like "Courier".
        /// 
        /// Setting a null Font results in throwing an exception.
        /// </summary>
        public override Font Font
        {
            get { return (base.Font); }

            set {
                TraceExceptionHelper.CheckNotNull(value, "value");

                Graphics gr;
                SizeF size;

                base.Font = value;

                gr = CreateGraphics();
                size = gr.MeasureString("m", Font);

                _viewport.SetCharSize(size.Width, size.Height);
                _viewport.SetViewport(Width, Height);

                return;
            }
        }
        
        /// <summary>
        /// Gets or sets the text to be displayed in the control.
        /// This text represents typically the content of a C# file.
        /// </summary>
        public override string Text
        {
            get { return (_viewport.Text); }
            set
            {
                CSCode block;

                block = new CSCode();
                block.Text = value;
                _viewport.TextSource = block;

                if (TextChanged != null)
                    TextChanged(this, new EventArgs());

                return;
            }
        }

        /// <summary>
        /// Gets or sets the line number where the exception occured in this file.
        /// </summary>
        public int HighlightedLine
        {
            get { return (_viewport.HighLightedLineIndex + 1); }
            set { _viewport.HighLightedLineIndex = value - 1; }
        }

        /// <summary>
        /// Gives access to the underling viewport used by this control.
        /// </summary>
        public CodeViewport Viewport
        {
            get { return (_viewport); }
        }

        /// <summary>
        /// Gets the content of the first visible line of text.
        /// If there is no visible line, the value "" is returned.
        /// </summary>
        public string FirstLine
        {
            get
            {
                if (_viewport.VisibleLines == 0)
                    return ("");
                return (_viewport[0].Text);
            }
        }

        /// <summary>
        /// Gets the current line number, starting from 1.
        /// </summary>
        public int CurrentLineNumber
        {
            get
            {
                if (_viewport.VisibleLines == 0)
                {
                    if (_viewport.Location.Y <= 0)
                        return (1);
                    return (_viewport.TextSource.LineCount);
                }

                return (_viewport[0].LineIndex + 1);
            }
        }

        /// <summary>
        /// Gets or sets the distance by which moving the text upward or
        /// downward when the control handles a mouse wheel event.
        /// </summary>
        public double MouseWheelDistance
        {
            get { return (_wheelDistance); }
            set { _wheelDistance = value; }
        }

        /// <summary>
        /// Translates the view coordinate by adding respectively
        /// tx and ty to the current view coordinates.
        /// </summary>
        /// <param name="tx">horizontal translation value in pixels.</param>
        /// <param name="ty">vertical translation value in pixels.</param>
        public void TranslateView(double tx, double ty)
        {
            PointF pt;

            pt = _viewport.Location;
            _viewport.SetPosition(pt.X + tx, pt.Y + ty);

            return;
        }

        /// <summary>
        /// Force a repaint.
        /// </summary>
        protected virtual void Repaint()
        {
            Invalidate();
        }

        #region event handlers

        /// <summary>
        /// Invoked when a mouse wheel is performed over this control.
        /// </summary>
        void CodeBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                HandleMouseWheelUp();

            if (e.Delta < 0)
                HandleMouseWheelDown();

            return;
        }

        /// <summary>
        /// Invoked when control's size has changed.
        /// </summary>
        void _size_Changed(object sender, EventArgs e)
        {
            _viewport.SetViewport(Width, Height);
            Repaint();
        }

        /// <summary>
        /// Invoked when something has changed in the viewport.
        /// </summary>
        void _viewport_Changed(object sender, EventArgs e)
        {
            Repaint();
        }

        #endregion

        /// <summary>
        /// Invoked when control need to be repainted.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            CSTokenCollection line;
            CSToken token;
            CSCode code;
            Brush whiteBrush;
            Brush redBrush;
            string text;
            float tk_width;
            int i;
            float x;

            whiteBrush = new SolidBrush(Color.White);
            redBrush = new SolidBrush(Color.Red);

            code = (CSCode)_viewport.TextSource;

            e.Graphics.FillRectangle(whiteBrush, 0, 0, Width, Height);

            foreach (PaintLineLocation arg in _viewport)
            {
                // if no exception is reported for the current line,
                // paint the text with multiple brushes to highlight
                // comment, keyword and strings
                if (!arg.IsHighlighted)
                {
                    line = code[arg.LineIndex];
                    x = 0;
                    text = line.Text;

                    for (i = 0; i < line.Count; ++i)
                    {
                        token = line[i];

                        e.Graphics.DrawString(token.Text, Font, _brushes[token.Tag],
                            arg.Location.X + x, arg.Location.Y);

                        tk_width = _measureStringWidth(e.Graphics, Font, text, token.IndexStart, token.Text.Length);

                        x += tk_width;
                    }

                    continue;
                }

                // otherwise, paint the background in red
                // and text in white
                e.Graphics.FillRectangle(redBrush,
                    0, arg.Location.Y, Viewport.Width, (float)Viewport.CharHeight);
                e.Graphics.DrawString(arg.Text, Font, whiteBrush, arg.Location.X, arg.Location.Y);
            }

            redBrush.Dispose();
            whiteBrush.Dispose();

            return;
        }

        /// <summary>
        /// Utility method that measure a region of text in the given string.
        /// </summary>
        /// <param name="g">The graphics instance used to render this text.</param>
        /// <param name="font">The font instance used to render this text.</param>
        /// <param name="text">The text that contains the region to be rendered.</param>
        /// <param name="indexStart">Starting startingPosition of this region.</param>
        /// <param name="length">Length of this region.</param>
        /// <returns>The width of this region of text.</returns>
        private float _measureStringWidth(Graphics g, Font font, string text, int indexStart, int length)
        {
            CharacterRange[] ranges;
            StringFormat sf;
            Region[] regions;

            if (length == 0)
                return (0);

            length = Math.Min(length, text.Length);

            ranges = new CharacterRange[] { new CharacterRange(indexStart, length) };
            sf = new StringFormat();

            // the string of text may contains white spaces that need to
            // be measured correctly.

            sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

            sf.SetMeasurableCharacterRanges(ranges);

            // here : giving a layout too small can cause returned measure
            // to be wrong.

            regions = g.MeasureCharacterRanges(
                text, font, new RectangleF(
                    0, 0, MEASURECHAR_BIG_WIDTH, MEASURECHAR_BIG_HEIGHT), sf);

            return (regions[0].GetBounds(g).Width);
        }

        /// <summary>
        /// Translates view upward.
        /// </summary>
        protected void HandleMouseWheelUp()
        {
            TranslateView(0, -_wheelDistance);
        }

        /// <summary>
        /// Translate view downards.
        /// </summary>
        protected void HandleMouseWheelDown()
        {
            TranslateView(0, _wheelDistance);
        }        
    }
}
