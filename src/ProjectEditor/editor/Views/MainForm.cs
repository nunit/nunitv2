// ***********************************************************************
// Copyright (c) 2010 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NUnit.ProjectEditor
{
    public partial class MainForm : Form, IProjectView
    {
        #region Instance Variables

        private ProjectPresenter presenter;

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public ProjectPresenter Presenter
        {
            set { this.presenter = value;  }
        }

        public IXmlView XmlView
        {
            get { return xmlView; }
        }

        public IPropertyView PropertyView
        {
            get { return propertyView; }
        }

        public SelectedView SelectedView
        {
            get { return (SelectedView)tabControl1.SelectedIndex; }
        }

        #endregion

        #region Menu Handlers

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.CreateNewProject();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.OpenProject();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.CloseProject();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.SaveProject();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.SaveProjectAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox box = new AboutBox();
            box.ShowDialog(this);
        }

        #endregion

        #region IProjectView Members

        public event EventHandler SelectedViewChanging;
        public event EventHandler SelectedViewChanged;

        public bool CloseCommandEnabled
        {
            set { this.closeToolStripMenuItem.Enabled = value; }
        }

        public bool SaveCommandsEnabled
        {
            set
            {
                this.saveToolStripMenuItem.Enabled = value;
                this.saveAsToolStripMenuItem.Enabled = value;
            }
        }

        public string GetOpenPath()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "Open Project";
            dlg.Filter = "Test Projects (*.nunit)|*.nunit";
            dlg.FilterIndex = 1;
            dlg.FileName = "";

            return dlg.ShowDialog(this) == DialogResult.OK
                ? dlg.FileName
                : null;
        }

        public string GetSaveAsPath()
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Title = "Save As";
            dlg.Filter = "Test Projects (*.nunit)|*.nunit";
            dlg.FilterIndex = 1;
            dlg.FileName = "";

            return dlg.ShowDialog(this) == DialogResult.OK
                ? dlg.FileName
                : null;
        }

        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            presenter.CloseProject();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (SelectedViewChanging != null)
                try
                {
                    SelectedViewChanging(this, EventArgs.Empty);
                }
                catch (Exception)
                {
                    e.Cancel = true;
                }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (SelectedViewChanged != null)
                SelectedViewChanged(this, EventArgs.Empty);
        }
    }
}
