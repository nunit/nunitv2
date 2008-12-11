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
using System.Diagnostics;
using NUnit.UiException.Controls;

namespace NUnit.UiException
{
    /// <summary>
    /// This enum indicate in which order exceptions should
    /// be listed in the underlying window.
    /// </summary>
    public enum ExceptionOrder
    {
        /// <summary>
        /// If set, exceptions are listed from the StackTrace in
        /// the reverse order.
        /// </summary>
        Reverse,

        /// <summary>
        /// If set, exceptions are listed from the StackTrace
        /// in the same order.
        /// </summary>
        Normal
    }

    /// <summary>
    /// Front class for the NUnit Exception Trace System.
    /// 
    /// Use this class to draw an hyperlink that when clicked opens a
    /// featurless window filled with the content of the current Stack Trace.
    /// </summary>
    public partial class ExceptionBrowserLink : UserControl
    {
        /// <summary>
        /// The StackTrace parser.
        /// </summary>
        private StackTraceParser _parser;

        /// <summary>
        /// The window that shows the exception list.
        /// </summary>
        private ExceptionWindow _formStack;

        /// <summary>
        /// The window that shows source code.
        /// </summary>
        private CodeBoxWindow _formCode;

        /// <summary>
        /// The value of the current StackTrace.
        /// </summary>
        private string _trace;

        /// <summary>
        /// The order in which listing exception
        /// in the exception window.
        /// </summary>
        private ExceptionOrder _order;

        /// <summary>
        /// The text caption shown by the link.
        /// </summary>
        private string _caption;

        /// <summary>
        /// Build a new instance of StackExceptionLink.
        /// </summary>
        public ExceptionBrowserLink()
        {
            InitializeComponent();

            _parser = new StackTraceParser();

            _formCode = new CodeBoxWindow();

            _formStack = new ExceptionWindow();
            _formStack.MouseWatcher.Register(_formCode);

            _formStack.ItemExpanded += new ItemStateChangedEventHandler(StackControler_ItemExpanded);
            _formStack.ItemCollapsed += new ItemStateChangedEventHandler(StackControler_ItemCollapsed);
            _formStack.SelectedItemChanged += new ItemStateChangedEventHandler(StackControler_ItemCollapsed);

            _formStack.MouseLeaved += new EventHandler(StackControler_MouseLeaved);

            this.Caption = "Browse exception details here";
            ForeColor = Color.Blue;
            Font = new Font(Font.FontFamily, Font.Size, FontStyle.Underline, GraphicsUnit.Point);

            _order = ExceptionOrder.Reverse;

            return;
        }

        /// <summary>
        /// Gets or sets the text of the link.
        /// </summary>
        public string Caption
        {
            get { return (_caption); }
            set 
            {
                if (value == null)
                    value = "";
                _caption = value;
            }
        }

        /// <summary>
        /// Gets or sets the enum that notify in which
        /// order exception should be listed in the exception window.
        /// </summary>
        public ExceptionOrder ExceptionOrder
        {
            get { return (_order); }
            set
            {
                if (_order == value)
                    return;

                _order = value;
                Items.Reverse();

                return;
            }
        }

        /// <summary>
        /// Gives access to the underlying ExceptionItemCollection.
        /// This property is automatically reset each time a new
        /// value is assigned in StackTrace property.
        /// </summary>
        public ExceptionItemCollection Items
        {
            get { return (_formStack.ExceptionList.Items); }
        }

        /// <summary>
        /// Gives access to the pagineer control of the window
        /// that list exceptions.
        /// </summary>
        public PagineerControl Pagineer
        {
            get { return (_formStack.Pagineer); }
        }

        /// <summary>
        /// Gives access to the window that list exceptions.
        /// </summary>
        public ExceptionWindow FormStack
        {
            get { return (_formStack); }
        }

        /// <summary>
        /// Gives access to the window that shows source code.
        /// </summary>
        public CodeBoxWindow FormCode
        {
            get { return (_formCode); }
        }

        /// <summary>
        /// Gets or sets the DirectorySeparator character
        /// for the current operating system.
        /// </summary>
        public char DirectorySeparator
        {
            get { return (_parser.DirectorySeparator); }
            set { _parser.DirectorySeparator = value; }
        }

        /// <summary>
        /// Gets or sets the extension for a source file.
        /// The assigned value should be prefixed by a dot
        /// character as in the following example: '.cs'
        /// </summary>
        public string FileExtension
        {
            get { return (_parser.FileExtension); }
            set { _parser.FileExtension = value; }
        }

        /// <summary>
        /// Gets or sets the StackTrace value as supplyied by NUnit.
        /// </summary>
        public string StackTrace
        {
            get { return (_trace); }
            set 
            { 
                _trace = value;
                Items.Clear();

                _parser.Parse(value);

                foreach (ExceptionItem item in _parser.Items)
                    Items.Add(item);

                if (_order == ExceptionOrder.Reverse)
                    Items.Reverse();

                this.Visible = Items.Count > 0;

                return;
            }
        }

        /// <summary>
        /// Draw the control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Brush brush;

            brush = new SolidBrush(ForeColor);
            e.Graphics.DrawString(Caption, Font, brush, 0, 0);
            brush.Dispose();

            return;
        }

        /// <summary>
        /// Invoked when mouse cursor has leaved the window
        /// that list exceptions.
        /// </summary>
        void StackControler_MouseLeaved(object sender, EventArgs e)
        {
            // hide the window if visible

            if (_formStack.Visible)
                _formStack.Visible = false;
        }

        /// <summary>
        /// Invoked when user has performed a click on an expandable
        /// item in the window that list exceptions.
        /// </summary>
        void StackControler_ItemExpanded(object sender, ExceptionItem item)
        {
            Point ptCode;
            Point ptItem;

            // retrieves screen coordinate of the item where
            // the click occured and show the source code window
            // near this position.

            ptItem = _formStack.ExceptionList.GetLowerLeftBoundOfCurrentItem();
            ptCode = new Point(
                ptItem.X + 23,
                ptItem.Y + ExceptionList.DEFAULT_ITEM_HEIGHT);

            _formCode.Show(ptCode.X, ptCode.Y, item);

            return;
        }

        /// <summary>
        /// Invoked when user has performed an action to collapse
        /// the expanded item.
        /// </summary>
        void StackControler_ItemCollapsed(object sender, ExceptionItem item)
        {
            // hide source code window if visible

            _formCode.Visible = false;
        }               

        /// <summary>
        /// Invoked when a click has occured on the hyperlink control.
        /// </summary>
        private void StackExceptionLink_MouseClick(object sender, MouseEventArgs e)
        {            
            Point pt;

            // Show the exception window near
            // the click position

            pt = PointToScreen(new Point(e.X, e.Y));
            _formStack.SetVisible(true, pt.X, pt.Y);           

            return;
        }
    }
}
