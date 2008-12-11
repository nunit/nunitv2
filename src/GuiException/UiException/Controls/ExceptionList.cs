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
using System.Windows.Forms;
using System.Drawing;
using NUnit.UiException.Properties;

namespace NUnit.UiException.Controls
{
    /// <summary>
    /// This control shows the list have ExceptionItems that have been 
    /// reported to the user.
    ///   This control is composited with a pagineer control so the user
    /// can visit all items even if the item count exceeds the display
    /// capacity of the ExceptionList.
    /// </summary>
    public class ExceptionList :
        Control
    {
        /// <summary>
        /// Default setting for PageSize property.
        /// </summary>
        public const int DEFAULT_PAGE_SIZE = 6;

        /// <summary>
        /// Default item width in pixels.
        /// </summary>
        public const int DEFAULT_ITEM_WIDTH = 312;

        /// <summary>
        /// Default item height in pixels.
        /// </summary>
        public const int DEFAULT_ITEM_HEIGHT = 32;

        /// <summary>
        /// Default header height in pixels. (The header correspond to the
        /// part that displays the pagineer).
        /// </summary>
        public const int DEFAULT_HEADER_HEIGHT = 17;

        /// <summary>
        /// Default distance to respect in pixels, between the top side of an item and the
        /// icon of the file that corresponds to this item.
        /// </summary>
        public const int DEFAULT_ICON_MARGIN_TOP = 5;

        /// <summary>
        /// Default distance in pixels to respect, between the top side of an item
        /// and the textual description.
        /// </summary>
        public const int DEFAULT_TEXT_MARGIN_TOP = 2;

        /// <summary>
        /// Default distance in pixels to respect between the right side of the
        /// pagineer control and the right side of the ExceptionList.
        /// </summary>
        public const int DEFAULT_PAGINER_MARGIN_RIGHT = 10;

        /// <summary>
        /// Fired when user clicks on a localized item.
        /// </summary>
        public event EventHandler ItemClicked;

        /// <summary>
        /// Fired when selected item has changed.
        /// </summary>
        public event EventHandler SelectedItemChanged;

        /// <summary>
        /// The object that contains all graphical resources
        /// to paint items.
        /// </summary>
        private TraceItemPaintArgs _pArgs;

        /// <summary>
        /// The current collection of item to be shown.
        /// </summary>
        private ExceptionItemCollection _items;

        /// <summary>
        /// Index of the selected item.
        /// </summary>
        private int _selectedItemIndex;

        /// <summary>
        /// The control that show pagination.
        /// </summary>
        protected PagineerControl _pagineer;

        /// <summary>
        /// Builds a new instance of this control.
        /// </summary>
        public ExceptionList()
        {
            _items = new ExceptionItemCollection();
            _items.ItemAdded += new ItemAddedEventHandler(_items_ItemAdded);
            _items.CollectionCleared += new EventHandler(_items_CollectionCleared);

            _pagineer = new PagineerControl();
            _pagineer.PageSize = DEFAULT_PAGE_SIZE;
            
            _selectedItemIndex = -1;

            Width = DEFAULT_ITEM_WIDTH;
            Height = DEFAULT_HEADER_HEIGHT + PageSize * DEFAULT_ITEM_HEIGHT;
            BackColor = Color.White;

            Controls.Add(_pagineer);

            _pagineer.Top = 1;
            _pagineer.Width = 60;
            _pagineer.Height = DEFAULT_HEADER_HEIGHT - 1;
            _pagineer.Left = Width - (_pagineer.Width + DEFAULT_PAGINER_MARGIN_RIGHT);
            _pagineer.Click += new EventHandler(_pagineer_Click);

            _pArgs = new TraceItemPaintArgs();
            _pArgs.Selected = false;
            _pArgs.Size = new Size(DEFAULT_ITEM_WIDTH, DEFAULT_ITEM_HEIGHT);
            _pArgs.bshBkgDefault = new SolidBrush(Color.White);
            _pArgs.bshBkgSelected = new SolidBrush(Color.FromArgb(100, 196, 225, 255));
            _pArgs.bshTextFile = new SolidBrush(Color.FromArgb(0, 176, 80));
            _pArgs.bshTextFunction = new SolidBrush(Color.Black);
            _pArgs.bshTextLine = new SolidBrush(Color.FromArgb(127, 127, 127));
            _pArgs.penSelection = new Pen(Color.FromArgb(51, 153, 255));
            _pArgs.penSeparator = new Pen(Color.FromArgb(128, 128, 128));
            _pArgs.penOutline = new Pen(Color.FromArgb(197, 192, 175));
            _pArgs.painterFile = new TextPainter(new Font(Font, FontStyle.Regular));
            _pArgs.painterFunction = new TextPainter(new Font(Font, FontStyle.Italic));
            _pArgs.painterLine = new TextPainter(new Font(Font, FontStyle.Regular));

            MouseMove += new MouseEventHandler(ExceptionList_MouseMove);
            MouseClick += new MouseEventHandler(ExceptionList_MouseClick);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            return;
        }

        /// <summary>
        /// Gives access to the underlying item collection managed
        /// by this control.
        /// </summary>
        public ExceptionItemCollection Items
        {
            get { return (_items); }
        }

        /// <summary>
        /// Gives access to the selected item. Returns null
        /// when state contains no selection.
        /// </summary>
        public ExceptionItem SelectedItem
        {
            get
            {
                if (_selectedItemIndex < 0 ||
                    _selectedItemIndex >= _items.Count)
                    return (null);
                return (_items[_selectedItemIndex]);
            }
        }

        /// <summary>
        /// Gets or sets the startingPosition of the item to be selected.
        /// </summary>
        public int SelectedItemIndex
        {
            get { return (_selectedItemIndex); }
            set
            {
                if (_selectedItemIndex == value)
                    return;

                _selectedItemIndex = value;
                OnRepaint();

                if (SelectedItemChanged != null)
                    SelectedItemChanged(this, new EventArgs());

                return;
            }
        }

        /// <summary>
        /// Gives access to the underlying pagineer control.
        /// </summary>
        public PagineerControl Pagineer
        {
            get { return (_pagineer); }
        }

        /// <summary>
        /// Gets or sets the number of item to display on one page.
        /// This is the preferred way to control this settings (over
        /// passing by Pagineer) because this property change the
        /// control's height accordingly.
        /// </summary>
        public int PageSize
        {
            get { return (_pagineer.PageSize); }
            set
            { 
                _pagineer.PageSize = Math.Max(1, value);
                Height = DEFAULT_HEADER_HEIGHT + _pagineer.PageSize * DEFAULT_ITEM_HEIGHT;
                OnRepaint();
            }
        }

        /// <summary>
        /// Gets the page count according the current list and PageSize.
        /// </summary>
        public int PageCount
        {
            get { return (_items.Count / PageSize + 1); }
        }

        /// <summary>
        /// Gets or sets the PageIndex to be shown.
        /// </summary>
        public int PageIndex
        {
            get { return (_pagineer.PageIndex); }
            set
            {
                _selectedItemIndex = -1;
                _pagineer.PageIndex = value;
            }
        }

        /// <summary>
        /// Gets the lower-left bound screen coordinates of the selected item.
        /// </summary>
        /// <returns>A point filled with the lower-left bound coordinate of the selected item.</returns>
        public Point GetLowerLeftBoundOfCurrentItem()
        {
            Rectangle r;
            Point point;

            r = RectangleToScreen(new Rectangle(0, 0, 0, 0));

            point = new Point(r.X, r.Y + DEFAULT_HEADER_HEIGHT + 
                (SelectedItemIndex - _pagineer.FirstVisibleItemIndex) * DEFAULT_ITEM_HEIGHT);

            return (point);
        }

        /// <summary>
        /// A flag telling whether to display a text or the collection of items.
        /// </summary>
        protected bool DrawingText
        {
            get { return (_items.Count == 0); }
        }

        /// <summary>
        /// For testing purpose.
        /// Force a repaint of the control.
        /// </summary>
        protected virtual void OnRepaint()
        {
            Invalidate();

            return;
        }

        /// <summary>
        /// Paint the control.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            InternalItemPainter painter;
            Brush brushBlack;
            Brush brushBkg;
            int i;

            painter = new InternalItemPainter();

            brushBkg = new SolidBrush(BackColor);
            brushBlack = new SolidBrush(Color.Black);

            e.Graphics.FillRectangle(brushBkg, 0, 0, Width, Height);
            e.Graphics.DrawImage(Resources.ExceptionListMarginLeft,
                new RectangleF(0, 0, 22, Height));

            _pArgs.G = e.Graphics;
            
            e.Graphics.FillRectangle(brushBkg,
                new RectangleF(0, 0, Width, DEFAULT_HEADER_HEIGHT));
            e.Graphics.DrawString("Exception traces", Font, brushBlack,
                new RectangleF(2, 1, Width, DEFAULT_HEADER_HEIGHT));

            for (i = 0; i < VisibleItemsAt(PageIndex); ++i)
            {
                _pArgs.Y = DEFAULT_HEADER_HEIGHT + i * DEFAULT_ITEM_HEIGHT;
                _pArgs.Item = _items[PageIndex * PageSize + i];
                _pArgs.Selected = (_pArgs.Item == SelectedItem);
                painter.PaintItem(this, _pArgs);
            }

            e.Graphics.DrawRectangle(_pArgs.penOutline,
                new Rectangle(0, 0, Width - 1, Height - 1));

            brushBlack.Dispose();
            brushBkg.Dispose();

            return;
        }
        
        /// <summary>
        /// Given a pageIndex, returns the count of visible
        /// items at this page.
        /// </summary>
        /// <param name="pageIndex">The pageIndex for which we want to get the
        /// number of visible items.</param>
        /// <returns>The count of visible items at this page.</returns>
        protected int VisibleItemsAt(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex >= PageCount)
                return (0);

            if (pageIndex < PageCount - 1)
                return (PageSize);

            return (_items.Count - pageIndex * PageSize);
        }

        /// <summary>
        /// Handler called when a mouse move event occurs on this control.
        /// This method updates the SelectedItemIndex property by retrieving
        /// the item that is currently under the mouse cursor.
        /// 
        /// Note: only item with source attachment can be referred by
        /// SelectedItemIndex.
        /// </summary>
        /// <param name="x">The client X coordinate of the mouse position.</param>
        /// <param name="y">The client Y coordinate of the mouse position.</param>
        protected void OnMouseMove(int x, int y)
        {
            int index;
            int itemIndex;

            index = (y - DEFAULT_HEADER_HEIGHT) / DEFAULT_ITEM_HEIGHT;
            itemIndex = PageIndex * PageSize + index;

            if (y < DEFAULT_HEADER_HEIGHT ||
                index >= VisibleItemsAt(PageIndex) ||
                !_items[itemIndex].HasSourceAttachment)
            {
                SelectedItemIndex = -1;
                return;
            }

            SelectedItemIndex = itemIndex;

            return;
        }

        /// <summary>
        /// Handler called when a mouse click occurs on this control.
        /// </summary>
        private void ExceptionList_MouseClick(object sender, MouseEventArgs e)
        {
            OnMouseMove(e.X, e.Y);

            if (SelectedItem == null || ItemClicked == null)
                return;

            ItemClicked(this, e);

            return;
        }

        /// <summary>
        /// Handler called when a PageIndexChanged event occurs.
        /// </summary>
        private void _pagineer_Click(object sender, EventArgs e)
        {
            PageIndex = _pagineer.PageIndex;
            OnRepaint();
        }

        /// <summary>
        /// Handler for the mouse move event.
        /// </summary>
        private void ExceptionList_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e.X, e.Y);
        }

        /// <summary>
        /// Handler called when the Clear() method is invoked on
        /// the item collection.
        /// </summary>
        private void _items_CollectionCleared(object sender, EventArgs e)
        {
            _pagineer.ItemCount = 0;
            OnRepaint();
        }

        /// <summary>
        /// Handler called when an item is appended to the item collection.
        /// </summary>
        private void _items_ItemAdded(object sender, ExceptionItem item)
        {
            _pagineer.ItemCount = _items.Count;
            OnRepaint();
        }

        /// <summary>
        /// Handler called when a resize occurs.
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            // The size should not be freely modified.
            // It should be function of:
            //  - DEFAULT_ITEM_WIDTH
            //  - DEFAULT_ITEM_HEIGHT...

            Width = DEFAULT_ITEM_WIDTH;
            Height = DEFAULT_HEADER_HEIGHT + PageSize * DEFAULT_ITEM_HEIGHT;

            return;
        }

        /// <summary>
        /// Contains all graphical resources needed to paint an item.
        /// </summary>
        class TraceItemPaintArgs
        {
            /// <summary>
            /// The brush used to draw the function text part of an item.
            /// </summary>
            public Brush bshTextFunction;

            /// <summary>
            /// The brush used to draw the file text part of an item.
            /// </summary>            
            public Brush bshTextFile;

            /// <summary>
            /// The brush used to draw the line text part of an item.
            /// </summary>
            public Brush bshTextLine;

            /// <summary>
            /// The brush used to paint the background of an item.
            /// </summary>
            public Brush bshBkgDefault;

            /// <summary>
            /// The brush used to paint the background of a selected item.
            /// </summary>
            public Brush bshBkgSelected;

            /// <summary>
            /// The pen used to paint the outline of a selected item.
            /// </summary>
            public Pen penSelection;

            /// <summary>
            /// The pen used to paint a separation line between two items.
            /// </summary>
            public Pen penSeparator;

            /// <summary>
            /// The pen used to paint the default outline of an item.
            /// </summary>
            public Pen penOutline;

            /// <summary>
            /// The item to be painted.
            /// </summary>
            public ExceptionItem Item;

            /// <summary>
            /// Is true, the item is selected.
            /// </summary>
            public bool Selected;

            /// <summary>
            /// A graphical reference to where painting items.
            /// </summary>
            public Graphics G;

            /// <summary>
            /// The top coordinate on the current Graphics context from
            /// where beginning drawing the current item.
            /// </summary>
            public int Y;

            /// <summary>
            /// The size in pixels of the item.
            /// </summary>
            public Size Size;

            /// <summary>
            /// Helper class to paint file text part.
            /// </summary>
            public TextPainter painterFile;

            /// <summary>
            /// Helper class to paint function text part.
            /// </summary>
            public TextPainter painterFunction;

            /// <summary>
            /// Helper class to paint line text part.
            /// </summary>
            public TextPainter painterLine;
        }

        /// <summary>
        /// Helper class to paint an item.
        /// </summary>
        class InternalItemPainter
        {
            /// <summary>
            /// Paint the specified item.
            /// </summary>
            public void PaintItem(ExceptionList sender, TraceItemPaintArgs args)
            {
                string path;
                int fontHeight;
                int leftMargin;
                int columnLeftWidth;
                int columnRightWidth;
                
                fontHeight = args.painterFunction.Font.Height;

                leftMargin = 23;
                columnRightWidth = 70;
                columnLeftWidth = args.Size.Width - columnRightWidth - leftMargin;

                if (args.Item.HasSourceAttachment)
                    args.G.DrawImage(Resources.ExceptionListMarginIcon, 0, args.Y + DEFAULT_ICON_MARGIN_TOP);

                args.painterFunction.Draw(
                    args.Item.ClassName + "." + args.Item.MethodName,
                    args.G, args.bshTextFunction,
                    leftMargin, args.Y + DEFAULT_TEXT_MARGIN_TOP,
                    columnLeftWidth, fontHeight);

                path = args.Item.HasSourceAttachment ?
                    args.Item.Filename :
                    "(no source location)";

                args.painterFile.Draw(path, args.G, args.bshTextFile,
                    leftMargin, args.Y + DEFAULT_TEXT_MARGIN_TOP + fontHeight, columnLeftWidth, fontHeight);

                if (args.Item.HasSourceAttachment)
                    args.painterLine.Draw(
                        "Line " + args.Item.LineNumber,
                        args.G,
                        args.bshTextLine,
                        leftMargin + columnLeftWidth,
                        args.Y + DEFAULT_TEXT_MARGIN_TOP,
                        columnLeftWidth,
                        fontHeight);

                args.G.DrawLine(args.penSeparator, leftMargin, args.Y + args.Size.Height - 1,
                    leftMargin + args.Size.Width, args.Y + args.Size.Height - 1);

                if (args.Selected)
                {
                    args.G.FillRectangle(args.bshBkgSelected,
                        new Rectangle(2, args.Y + 1, args.Size.Width - 5, args.Size.Height - 4));
                    args.G.DrawRectangle(args.penSelection,
                        new Rectangle(2, args.Y + 1, args.Size.Width - 5, args.Size.Height - 4));
                }

                return;
            }
        }
    }
}
