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
using System.Text;

namespace NUnit.ProjectEditor
{
    public interface IProjectModel
    {
        #region Events

        event CommandDelegate ProjectCreated;
        event CommandDelegate ProjectClosed;
        event CommandDelegate ProjectChanged;

        #endregion

        #region Properties

        string XmlText { get; set; }
        Exception Exception { get; set; }

        string ProjectPath { get; set; }
        string BasePath { get; set; }
        bool AutoConfig { get; set; }
        string ActiveConfigName { get; set; }

        ProcessModel ProcessModel { get; set; }
        DomainUsage DomainUsage { get; set; }

        string Name { get; }
        ConfigList Configs { get; }
        string[] ConfigNames { get; }

        bool HasUnsavedChanges { get; }
        bool IsValid { get; }

        #endregion

        #region Methods

        void CreateNewProject();
        void OpenProject(string fileName);
        void CloseProject();
        void SaveProject();
        void SaveProject(string fileName);

        void SynchronizeModel();

        void LoadXml(string xmlText);
        string ToXml();

        #endregion
    }
}
