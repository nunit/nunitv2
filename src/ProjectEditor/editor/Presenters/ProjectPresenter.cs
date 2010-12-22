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
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NUnit.ProjectEditor
{
    /// <summary>
    /// ProjectEditor is the top-level presenter with subordinate 
    /// presenters for each view of the project. It directly handles
    /// the menu commands from the top-level view and coordinates 
    /// changes in the two different submodels.
    /// </summary>
    public class ProjectPresenter
    {
        private IProjectView view;
        private IProjectModel model;
        private IMessageBoxCreator mbox;

        private PropertyPresenter propertyPresenter;
        private XmlPresenter xmlPresenter;

        #region Constructor

        public ProjectPresenter(IProjectModel model, IProjectView view, IMessageBoxCreator mbox)
        {
            this.model = model;
            this.view = view;
            this.mbox = mbox;

            // Set up handlers for the view
            view.SelectedViewChanging += new EventHandler(view_SelectedViewChanging);
            view.SelectedViewChanged += new EventHandler(view_SelectedViewChanged);
            
            // Set up handlers for model events
            model.ProjectCreated += new CommandDelegate(OnProjectCreated);
            model.ProjectClosed += new CommandDelegate(OnProjectClosed);
        }

        void view_SelectedViewChanging(object sender, EventArgs e)
        {
            view.SaveCommandsEnabled = false;
            model.SynchronizeModel();
            view.SaveCommandsEnabled = true; // Won't be executed if an exception is thrown
        }

        void view_SelectedViewChanged(object sender, EventArgs e)
        {
            switch (view.SelectedView)
            {
                case SelectedView.PropertyView:
                    propertyPresenter.LoadViewFromModel();
                    break;

                case SelectedView.XmlView:
                    xmlPresenter.LoadViewFromModel();
                    break;
            }
        }

        #endregion

        #region Command Event Handlers

        public void CreateNewProject()
        {
            model.CreateNewProject();
            view.CloseCommandEnabled = true;
            view.SaveCommandsEnabled = true;
        }

        public void OpenProject()
        {
            string path = view.GetOpenPath();
            if (path != null)
            {
                model.OpenProject(path);
                view.CloseCommandEnabled = true;
                view.SaveCommandsEnabled = model.IsValid;
            }
        }

        public void CloseProject()
        {
            if (model.HasUnsavedChanges &&
                mbox.AskYesNoQuestion(string.Format("Do you want to save changes to {0}?", model.Name)))
                    SaveProject();

            model.CloseProject();
            view.CloseCommandEnabled = false;
            view.SaveCommandsEnabled = false;
        }

        public void SaveProject()
        {
            if (IsValidWritableProjectPath(model.ProjectPath))
            {
                model.SaveProject();
            }
            else
            {
                this.SaveProjectAs();
            }
        }

        public void SaveProjectAs()
        {
            string path = view.GetSaveAsPath();
            if (path != null)
            {
                model.SaveProject(path);
                view.PropertyView.ProjectPath = model.ProjectPath;
            }
        }

        #endregion

        #region Model Event Handlers

        void OnProjectCreated()
        {
            view.XmlView.Visible = true;
            view.XmlView.Text = model.XmlText;

            view.PropertyView.Visible = true;
            propertyPresenter = new PropertyPresenter(model, view.PropertyView, mbox);
            xmlPresenter = new XmlPresenter(model, view.XmlView);

            propertyPresenter.LoadViewFromModel();
        }

        void OnProjectClosed()
        {

            view.XmlView.Text = null;

            view.XmlView.Visible = false;
            view.PropertyView.Visible = false;
        }

        #endregion

        #region Helper Methods

        private static bool IsValidWritableProjectPath(string path)
        {
            if (!Path.IsPathRooted(path))
                return false;

            if (!ProjectModel.IsProjectFile(path))
                return false;

            if (!File.Exists(path))
                return true;

            return (File.GetAttributes(path) & FileAttributes.ReadOnly) == 0;
        }

        #endregion
    }
}
