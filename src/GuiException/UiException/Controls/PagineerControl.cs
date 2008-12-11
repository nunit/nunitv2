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
using NUnit.UiException.Properties;
using System.Diagnostics;

namespace NUnit.UiException.Controls
{
    /// <summary>
    /// This control reports the current page startingPosition and the total
    /// page count. Besides it draws two arrows buttons with which
    /// user can interact with to modify the page startingPosition.
    /// </summary>
    public partial class PagineerControl
        : UserControl
    {
        /// <summary>
        /// Stores the total page count
        /// </summary>
        private int _itemCount;

        /// <summary>
        /// Stores the number of item to display in one page.
        /// </summary>
        private int _pageSize;

        /// <summary>
        /// Stores the current page startingPosition.
        /// </summary>
        private int _pageIndex;

        /// <summary>
        /// Used to draw the left arrow.
        /// </summary>
        private GraphicArrow _leftArrow;

        /// <summary>
        /// Used to draw the right arrow.
        /// </summary>
        private GraphicArrow _rightArrow;

        /// <summary>
        /// Fired when page startingPosition has changed.
        /// </summary>
        public event EventHandler PageIndexChanged;

        /// <summary>
        /// Fired when page size has changed.
        /// </summary>
        public event EventHandler PageSizeChanged;

        /// <summary>
        /// Used to draw the control with underlined font.
        /// </summary>
        private Font _fontUnderlined;

        /// <summary>
        /// Used to draw the control with regular font.
        /// </summary>
        private Font _fontNormal;

        /// <summary>
        /// A reference either to _fontUnderlined or _fontNormal
        /// </summary>
        private Font _font;

        /// <summary>
        /// Build a new instance of PagineerControl.
        /// </summary>
        public PagineerControl()
        {
            InitializeComponent();

            MouseClick += new MouseEventHandler(PagineerControl_MouseClick);
            MouseDoubleClick += new MouseEventHandler(PagineerControl_MouseClick);
            MouseHover += new EventHandler(PagineerControl_MouseHover);
            MouseLeave += new EventHandler(PagineerControl_MouseLeave);

            _leftArrow = new GraphicArrow(ArrowDirection.Left);
            _rightArrow = new GraphicArrow(ArrowDirection.Right);

            _pageSize = 10;

            Width = 260;
            Height = 16;
            BackColor = Color.White;

            _fontUnderlined = new Font(Font, FontStyle.Underline);
            _fontNormal = Font;
            _font = _fontNormal;

            return;
        }

        /// <summary>
        /// Invoked when mouse cursor has leaved the control.
        /// </summary>
        void PagineerControl_MouseLeave(object sender, EventArgs e)
        {
            // reset font to regular

            _font = _fontNormal;
            Invalidate();
        }

        /// <summary>
        /// Invoked when mouse has entered the control.
        /// </summary>
        void PagineerControl_MouseHover(object sender, EventArgs e)
        {
            // set underlined font

            _font = _fontUnderlined;
            Invalidate();
        }

        /// <summary>
        /// Invoked when a click occured on the control.
        /// </summary>
        void PagineerControl_MouseClick(object sender, MouseEventArgs e)
        {
            HandleMouseClick(e.X, e.Y);
        }

        /// <summary>
        /// Gets or sets the ItemCount value.
        /// </summary>
        public int ItemCount
        {
            get { return (_itemCount); }
            set 
            {
                value = Math.Max(0, value);

                if (_itemCount == value)
                    return;

                _itemCount = value;
                if (_pageIndex >= PageCount)
                    _pageIndex = 0;

                Repaint();

                return;
            }
        }

        /// <summary>
        /// Gets the PageCount according PageSize and ItemCount
        /// values.
        /// </summary>
        public int PageCount {
            get 
            { 
                int quotient;
                int remain;                

                quotient = _itemCount / _pageSize;
                remain = _itemCount % _pageSize;

                if (remain > 0)
                    quotient++;

                return (quotient); 
            }
        }

        /// <summary>
        /// Gets or sets the number of item to be displayed
        /// on one page.
        /// </summary>
        public int PageSize
        {
            get { return (_pageSize); }
            set
            {
                value = Math.Max(1, value);

                if (_pageSize == value)
                    return;

                _pageSize = value;

                if (PageSizeChanged != null)
                    PageSizeChanged(this, new EventArgs());

                Repaint();

                return;
            }
        }

        /// <summary>
        /// Gets or sets the startingPosition of the selected page.
        /// </summary>
        public int PageIndex
        {
            get { return (_pageIndex); }
            set 
            {
                value = Math.Max(0, Math.Min(PageCount - 1, value));

                if (_pageIndex == value)
                    return;

                _pageIndex = value;

                if (PageIndexChanged != null)
                    PageIndexChanged(this, new EventArgs());

                Repaint();

                return;
            }
        }

        /// <summary>
        /// Gets the item startingPosition of the first visible item on
        /// the current page. This value is function of
        /// PageIndex and PageSize.
        /// </summary>
        public int FirstVisibleItemIndex
        {
            get {
                if (_itemCount == 0)
                    return (-1);
                return (_pageIndex * _pageSize);
            }
        }

        /// <summary>
        /// Gets the item startingPosition of the last visible item on
        /// the current page. This value is function of
        /// PageIndex and PageSize.
        /// </summary>
        public int LastVisibleItemIndex
        {
            get {
                if (_itemCount == 0)
                    return (-1);
                return (Math.Min(_itemCount - 1, _pageIndex * _pageSize + (_pageSize - 1)));
            }
        }

        /// <summary>
        /// Attempt to select the page next to the current
        /// startingPosition (if any). Returns true if the operation succeeded;
        /// false otherwise.
        /// </summary>
        /// <returns>True if there is a page next to the current one.</returns>
        public bool NextPage()
        {
            if (!HasNext())
                return (false);

            PageIndex++;

            return (true);
        }

        /// <summary>
        /// Attempt to select the previous page to the current
        /// startingPosition (if any). Returns true if the operation succeeded;
        /// false otherwise.
        /// </summary>
        /// <returns>True if there is a previous page to the current one.</returns>
        public bool PrevPage()
        {
            if (!HasPrev())
                return (false);

            PageIndex--;

            return (true);
        }

        /// <summary>
        /// Check whether there is a page next to the current one.
        /// </summary>
        /// <returns>True if there is a page next to the current one.</returns>
        public bool HasNext()
        {
            return (PageIndex < PageCount - 1);
        }

        /// <summary>
        /// Checks whether there is a previous page to the current one.
        /// </summary>
        /// <returns>True if there is a previous page to the current one.</returns>
        public bool HasPrev()
        {
            return (PageIndex > 0);
        }

        /// <summary>
        /// Paint the control.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            string text;
            SizeF size;
            Brush bkgBrush;
            Brush fontBrush;
            float x_left;
            float x_text;
            float x_right;
            float y_arrow;

            text = ToString();
            size = e.Graphics.MeasureString(text, _font);

            bkgBrush = new SolidBrush(BackColor);
            fontBrush = new SolidBrush(ForeColor);

            e.Graphics.FillRectangle(bkgBrush, 0, 0, Width, Height);
            
            x_text = (Width - size.Width) / 2;
            x_left = x_text - _leftArrow.Size.Width;
            x_right = x_text + size.Width;
            y_arrow = (Height - _leftArrow.Size.Height) / 2;

            e.Graphics.DrawString(text, _font, fontBrush,
                (Width - size.Width) / 2,
                (Height - size.Height) / 2);

            e.Graphics.DrawImage(Resources.Arrows,
                x_left, y_arrow, _leftArrow.Rectangle, GraphicsUnit.Pixel);

            e.Graphics.DrawImage(Resources.Arrows,
                x_right, y_arrow, _rightArrow.Rectangle, GraphicsUnit.Pixel);

            fontBrush.Dispose();
            bkgBrush.Dispose();

            return;
        }

        #region internal definitions

        /// <summary>
        /// Force a repaint.
        /// </summary>
        protected virtual void Repaint() 
        {
            _leftArrow.SetEnabled(HasPrev());
            _rightArrow.SetEnabled(HasNext());

            Invalidate();

            return;
        }

        /// <summary>
        /// Moves the page startingPosition backward or forward along
        /// the value the given X client coordinate.
        /// </summary>
        /// <param name="x">The X client coordinate.</param>
        /// <param name="y">An Y client coordinate.</param>
        protected void HandleMouseClick(int x, int y)
        {
            if (x > Width / 2)
                NextPage();
            else
                PrevPage();

            return;
        }

        public override string ToString()
        {
            return (String.Format("page {0}/{1}", 
                Math.Min(PageIndex + 1, PageCount), PageCount));
        }

        #endregion

        #region GraphicAttribute

        /// <summary>
        /// Helper object to draw an arrow.
        /// </summary>
        class GraphicArrow
        {
            private Size _size;
            private Point[] _pts;
            private int _index;
            private bool _enabled;

            public GraphicArrow(ArrowDirection dir)
            {
                _index = 0;

                _size = new Size(5, 7);

                if (dir == ArrowDirection.Right)
                    _index = 2;

                _pts = new Point[] {
                    new Point(0, 12),
                    new Point(6, 12),
                    new Point(12, 12),
                    new Point(18, 12)
                };

                _enabled = true;

                return;
            }

            public Size Size {
                get { return (_size); }
            }

            public Rectangle Rectangle {
                get {
                    int idx;

                    idx = _index;
                    if (!_enabled)
                        idx++;
                    return (new Rectangle(_pts[idx].X, _pts[idx].Y, Size.Width - 1, Size.Height - 1)); 
                }
            }

            public void SetEnabled(bool enabled) {
                _enabled = enabled;
            }
        }

        #endregion
    }
}
