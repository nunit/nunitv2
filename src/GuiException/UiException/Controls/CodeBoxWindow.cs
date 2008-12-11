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

namespace NUnit.UiException.Controls
{
    /// <summary>
    /// A window filled with a CodeBoxComposite control.
    /// </summary>
    public partial class CodeBoxWindow : Form
    {
        public CodeBoxWindow()
        {
            InitializeComponent();

            _codeBoxComposite.MouseLeaveWindow += new EventHandler(_codeBoxComposite_CursorLeaveWindow);
            _codeBoxComposite.MouseClickedWindow += new EventHandler(_codeBoxComposite_MouseClickedWindow);

            return;
        }

        void _codeBoxComposite_MouseClickedWindow(object sender, EventArgs e)
        {
            Visible = false;
        }

        /// <summary>
        /// Invoked when the mouse leaves the window.
        /// 
        /// This handler makes the window to hide when mouse cursor leave the area.
        /// </summary>
        void _codeBoxComposite_CursorLeaveWindow(object sender, EventArgs e)
        {
            Visible = false;            
        }
      
        /// <summary>
        /// Show the window at the specified x/y coordinates and setup its content
        /// with the data pointed by item.
        /// </summary>
        /// <param name="x">X value of the left upper corner where to put the window.</param>
        /// <param name="y">Y value of the left upper corner where to put the window.</param>
        /// <param name="item">Data to fill the content of the window with.</param>
        public void Show(int x, int y, ExceptionItem item)
        {           
            _codeBoxComposite.ExceptionSource = item;

            Width = _codeBoxComposite.Width;
            Height = _codeBoxComposite.Height;

            Location = new Point(x, y);
            
            Visible = true;

            return;
        }       
    }
}
