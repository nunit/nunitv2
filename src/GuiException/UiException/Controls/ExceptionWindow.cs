// ----------------------------------------------------------------
// ErrorBrowser
// Copyright 2008-2009, Irénée HOTTIER,
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

namespace NUnit.UiException.Controls
{
    public delegate void ItemStateChangedEventHandler(object sender, ErrorItem item);

    /// <summary>
    /// The window that displays the list of exceptions.
    /// </summary>
    public partial class ExceptionWindow : Form
    {
        /// <summary>
        /// Fired when an item state changed to 'Expanded'.
        /// </summary>
        public event ItemStateChangedEventHandler ItemExpanded;

        /// <summary>
        /// Fired when an item state changed to 'Collapsed'.
        /// </summary>
        public event ItemStateChangedEventHandler ItemCollapsed;

        /// <summary>
        /// Fired when selected item changed.
        /// </summary>
        public event ItemStateChangedEventHandler SelectedItemChanged;

        /// <summary>
        /// Fired when mouse cursor has leaved the window.
        /// </summary>
        public event EventHandler MouseLeaved;

        /// <summary>
        /// The underlying control that display the exception list.
        /// </summary>
        private ExceptionList _exceptionList;

        /// <summary>
        /// The item that fired the event.
        /// </summary>
        private ErrorItem _itemEvent;

        /// <summary>
        /// The watcher for this window responsible to deliver
        /// the MouseLeaved event.
        /// </summary>
        private MouseWatcher _mouseWatcher;

        /// <summary>
        /// Builds a new instance of ExceptionWindow.
        /// </summary>
        public ExceptionWindow()
        {
            InitializeComponent();

            _exceptionList = new ExceptionList();
            _exceptionList.ItemClicked += new EventHandler(_exceptionList_ItemClicked);
            _exceptionList.SelectedItemChanged += new EventHandler(_exceptionList_SelectedItemChanged);
            _itemEvent = null;

            _mouseWatcher = new MouseWatcher();
            _mouseWatcher.Register(_exceptionList);
            _mouseWatcher.MouseLeaved += new EventHandler(_mouseWatcher_MouseLeaved);

            Controls.Add(_exceptionList);

            return;
        }

        /// <summary>
        /// Gives access to the underlying ExceptionList control.
        /// </summary>
        public ExceptionList ExceptionList
        {
            get { return (_exceptionList); }
        }

        /// <summary>
        /// Fired when selected item has changed.
        /// </summary>
        void _exceptionList_SelectedItemChanged(object sender, EventArgs e)
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged(this, _exceptionList.SelectedItem);

            return;
        }

        /// <summary>
        /// Fired when a click occured on an item.
        /// </summary>
        void _exceptionList_ItemClicked(object sender, EventArgs e)
        {
            if (_exceptionList.SelectedItem != _itemEvent)
            {
                if (_itemEvent != null && ItemCollapsed != null)
                    ItemCollapsed(this, _itemEvent);
            }

            _itemEvent = _exceptionList.SelectedItem;

            if (_itemEvent != null && ItemExpanded != null)
                ItemExpanded(this, _itemEvent);

            return;
        }

        /// <summary>
        /// Gives access to the underlying MouseWatcher.
        /// </summary>
        public MouseWatcher MouseWatcher
        {
            get { return (_mouseWatcher); }
        }

        /// <summary>
        /// Fired when mouse cursor has leaved the window.
        /// </summary>
        void _mouseWatcher_MouseLeaved(object sender, EventArgs e)
        {
            if (MouseLeaved != null)
                MouseLeaved(this, e);

            return;
        }

        /// <summary>
        /// Gives access to the underlying Pagineer control.
        /// </summary>
        public PagineerControl Pagineer
        {
            get { return (_exceptionList.Pagineer); }
        }     

        /// <summary>
        /// Shows this window on the given screen coordinates.
        /// </summary>
        /// <param name="visible">True to show the window.</param>
        /// <param name="x">X screen coordinate of the window upper-left corner.</param>
        /// <param name="y">Y screen coordinate of the window upper-left corner.</param>
        public void SetVisible(bool visible, int x, int y)
        {
            Width = _exceptionList.Width;
            Height = _exceptionList.Height;

            this.Location = new Point(x, y);

            Visible = visible;

            return;
        }     
    }
}
