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

namespace NUnit.ProjectEditor
{
    public class XmlPresenter
    {
        private IProjectModel model;
        private IXmlView view;

        public XmlPresenter(IProjectModel model, IXmlView view)
        {
            this.model = model;
            this.view = view;

            this.model.ProjectChanged += new CommandDelegate(model_Changed);
            view.Changed += new EventHandler(view_Changed);
        }

        public void LoadViewFromModel()
        {
            view.Text = model.XmlText;
            view.Exception = model.Exception;
        }

        public void UpdateModelFromView()
        {
            model.XmlText = view.Text;
        }

        #region Event Handlers

        void model_Changed()
        {
            // TODO: This triggers a second round of events - but should not.
            // It may be resolved after we convert all model events to originate
            // in ProjectModel.
            view.Text = model.XmlText;
            view.Exception = model.Exception;
        }

        void view_Changed(object sender, EventArgs e)
        {
            // TODO: Validation goes here
            model.XmlText = view.Text;
        }

        #endregion
    }
}
