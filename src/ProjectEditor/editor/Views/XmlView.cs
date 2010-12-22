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
using System.Windows.Forms;

namespace NUnit.ProjectEditor
{
    public partial class XmlView : UserControl, IXmlView
    {
        public event EventHandler Changed;

        public XmlView()
        {
            InitializeComponent();
        }

        public override string Text
        {
            get { return richTextBox1.Text; }
            set { richTextBox1.Text = value; }
        }

        public Exception Exception
        {
            set
            {
                if (value != null)
                {
                    errorMessageLabel.Visible = true;
                    errorMessageLabel.Text = value.Message;

                    richTextBox1.Dock = DockStyle.Top;
                    richTextBox1.Height = this.ClientSize.Height - errorMessageLabel.Height;

                    ProjectFormatException ex = value as ProjectFormatException;
                    if (ex != null)
                    {
                        int offset = richTextBox1.GetFirstCharIndexFromLine(ex.LineNumber) + ex.LinePosition - 1;
                        richTextBox1.Select(offset, 3);
                    }
                }
                else
                {
                    errorMessageLabel.Visible = false;
                    richTextBox1.Dock = DockStyle.Fill;
                }
            }
        }

        //private void richTextBox1_TextChanged(object sender, EventArgs e)
        //{
        //    if (this.Changed != null)
        //        this.Changed(this, EventArgs.Empty);
        //}

        private void richTextBox1_Validated(object sender, EventArgs e)
        {
            if (this.Changed != null)
                this.Changed(this, EventArgs.Empty);
        }
    }
}
