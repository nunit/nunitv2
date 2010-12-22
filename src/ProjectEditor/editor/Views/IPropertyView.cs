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
using System.Collections;
using System.ComponentModel;

namespace NUnit.ProjectEditor
{
    public delegate void CommandDelegate();

    public interface IPropertyView : INotifyPropertyChanged
    {
        #region Events

        event CommandDelegate BrowseForProjectBase;
        event CommandDelegate BrowseForConfigBase;
        event CommandDelegate EditConfigs;
        event CommandDelegate AddAssembly;
        event CommandDelegate RemoveAssembly;
        event CommandDelegate BrowseForAssembly;

        #endregion

        #region Properties

        string ProjectPath { get; set; }
        string ProjectBase { get; set; }
        string ProcessModel { get; set; }
        string DomainUsage { get; set; }
        string ActiveConfigName { get; set; }

        string[] ConfigList { set; }

        int SelectedConfig { get; set; }

        #region Properties Applying to Selected Config

        string Runtime { get; set; }
        string RuntimeVersion { get; set; }
        string ApplicationBase { get; set; }
        string ConfigurationFile { get; set; }

        BinPathType BinPathType { get; set; }
        string PrivateBinPath { get; set; }

        string[] AssemblyList { set; }
        string SelectedAssembly { get; }
        int SelectedAssemblyIndex { get; }
        string AssemblyPath { get; set; }

        bool PrivateBinPathEnabled { set; }
        bool AddAssemblyEnabled { set; }
        bool RemoveAssemblyEnabled { set; }
        bool EditAssemblyEnabled { set; }

        bool Visible { get; set; }

        #endregion

        #region Selection Options for Lists

        string[] ProcessModelOptions { get; set; }
        string[] DomainUsageOptions { get; set; }
        string[] RuntimeOptions { get; set; }
        string[] RuntimeVersionOptions { get; set; }

        #endregion

        #endregion

        #region Methods

        string BrowseForFolder(string message, string initialPath);
        string GetAssemblyPath();
        void ErrorMessage(string property, string message);

        #endregion
    }
}
