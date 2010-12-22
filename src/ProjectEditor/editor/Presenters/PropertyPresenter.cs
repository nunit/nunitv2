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
using System.IO;
using System.Text;

namespace NUnit.ProjectEditor
{
    /// <summary>
    /// The ProjectPresenter handles presentation of the project as
    /// a set of properties, which the ProjectView is expected to
    /// display.
    /// </summary>
    public class PropertyPresenter
    {
        private IProjectModel model;
        private IPropertyView view;
        private IMessageBoxCreator mbox;

        public PropertyPresenter(IProjectModel model, IPropertyView view, IMessageBoxCreator mbox)
        {
            this.model = model;
            this.view = view;
            this.mbox = mbox;

            SetProcessModelOptions();
            SetDomainUsageOptions();
            SetRuntimeOptions();
            SetRuntimeVersionOptions();

            view.BrowseForProjectBase += new CommandDelegate(BrowseForProjectBase);
            view.BrowseForConfigBase += new CommandDelegate(BrowseForConfigBase);
            view.EditConfigs += new CommandDelegate(EditConfigs);
            view.AddAssembly += new CommandDelegate(AddAssembly);
            view.RemoveAssembly += new CommandDelegate(RemoveAssembly);
            view.BrowseForAssembly += new CommandDelegate(BrowseForAssembly);

            view.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChange);

            //model.ProjectCreated += new CommandDelegate(LoadViewFromModel);
        }

        public void LoadViewFromModel()
        {
            view.Visible = true;

            view.ProjectPath = model.ProjectPath;
            view.ProjectBase = model.BasePath == null ? model.ProjectPath : model.BasePath;
            view.ActiveConfigName = model.ActiveConfigName;

            view.ProcessModel = model.ProcessModel.ToString();
            view.DomainUsage = model.DomainUsage.ToString();

            view.ConfigList = model.ConfigNames;
        }

        #region Command Event Handlers

        private void BrowseForProjectBase()
        {
            string message = "Select ApplicationBase for the model as a whole.";
            string projectBase = view.BrowseForFolder(message, view.ProjectBase);
            if (projectBase != null && projectBase != model.BasePath)
                view.ProjectBase = model.BasePath = projectBase;
        }

        private void BrowseForConfigBase()
        {
            string message = string.Format(
                "Select ApplicationBase for the {0} configuration, if different from the model as a whole.",
                model.Configs[view.SelectedConfig].Name);
            string initialFolder = view.ApplicationBase;
            if (initialFolder == string.Empty)
                initialFolder = view.ProjectBase;

            string appbase = view.BrowseForFolder(message, initialFolder);
            if (appbase != null && appbase != view.ApplicationBase)
                UpdateApplicationBase(appbase);
        }

        private void UpdateApplicationBase(string appbase)
        {
            string basePath = null;

            if (appbase != String.Empty)
            {
                basePath = Path.Combine(model.BasePath, appbase);
                if (PathUtils.SamePath(model.BasePath, basePath))
                    basePath = null;
            }

            ProjectConfig selectedConfig = model.Configs[view.SelectedConfig];
            view.ApplicationBase = selectedConfig.BasePath = basePath;

            // TODO: Test what happens if we set it the same as project base
            //if (selectedConfig.RelativeBasePath == null)
            //    applicationBaseTextBox.Text = string.Empty;
            //else
            //    applicationBaseTextBox.Text = selectedConfig.RelativeBasePath;
        }

        private void EditConfigs()
        {
            using (ConfigurationEditorView editorView = new ConfigurationEditorView())
            {
                editorView.Editor = new ConfigurationEditor(model, editorView);
                editorView.ShowDialog();
            }

            view.ConfigList = model.ConfigNames;
            view.ActiveConfigName = model.ActiveConfigName;
        }

        private void AddAssembly()
        {
            string assemblyPath = view.GetAssemblyPath();

            if (assemblyPath != null)
            {
                ProjectConfig selectedConfig = model.Configs[view.SelectedConfig];
                selectedConfig.Assemblies.Add(assemblyPath);
                SetAssemblyList();
            }

            //OpenFileDialog dlg = new OpenFileDialog();
            //dlg.Title = "Add Assemblies To Project";
            //dlg.InitialDirectory = selectedConfig.BasePath;

            ////if ( VisualStudioSupport )
            ////    dlg.Filter =
            ////        "Projects & Assemblies(*.csproj,*.vbproj,*.vjsproj, *.vcproj,*.dll,*.exe )|*.csproj;*.vjsproj;*.vbproj;*.vcproj;*.dll;*.exe|" +
            ////        "Visual Studio Projects (*.csproj,*.vjsproj,*.vbproj,*.vcproj)|*.csproj;*.vjsproj;*.vbproj;*.vcproj|" +
            ////        "C# Projects (*.csproj)|*.csproj|" +
            ////        "J# Projects (*.vjsproj)|*.vjsproj|" +
            ////        "VB Projects (*.vbproj)|*.vbproj|" +
            ////        "C++ Projects (*.vcproj)|*.vcproj|" +
            ////        "Assemblies (*.dll,*.exe)|*.dll;*.exe";
            ////else
            //dlg.Filter = "Assemblies (*.dll,*.exe)|*.dll;*.exe";

            //dlg.FilterIndex = 1;
            //dlg.FileName = "";

            //if (dlg.ShowDialog(this) != DialogResult.OK)
            //    return;

            //if (PathUtils.IsAssemblyFileType(assemblyPath))
            //{
            //    selectedConfig.Assemblies.Add(assemblyPath);
            //    return;
            //}
            //else if (VSProject.IsProjectFile(dlg.FileName))
            //    try
            //    {
            //        VSProject vsProject = new VSProject(dlg.FileName);
            //        MessageBoxButtons buttons;
            //        string msg;

            //        if (vsProject.Configs.Contains(selectedConfig.Name))
            //        {
            //            msg = "The project being added may contain multiple configurations;\r\r" +
            //                "Select\tYes to add all configurations found.\r" +
            //                "\tNo to add only the " + selectedConfig.Name + " configuration.\r" +
            //                "\tCancel to exit without modifying the project.";
            //            buttons = MessageBoxButtons.YesNoCancel;
            //        }
            //        else
            //        {
            //            msg = "The project being added may contain multiple configurations;\r\r" +
            //                "Select\tOK to add all configurations found.\r" +
            //                "\tCancel to exit without modifying the project.";
            //            buttons = MessageBoxButtons.OKCancel;
            //        }

            //        DialogResult result = UserMessage.Ask(msg, buttons);
            //        if (result == DialogResult.Yes || result == DialogResult.OK)
            //        {
            //            project.Add(vsProject);
            //            return;
            //        }
            //        else if (result == DialogResult.No)
            //        {
            //            foreach (string assembly in vsProject.Configs[selectedConfig.Name].Assemblies)
            //                selectedConfig.Assemblies.Add(assembly);
            //            return;
            //        }

            //        assemblyListBox_Populate();
            //    }
            //    catch (Exception ex)
            //    {
            //        UserMessage.DisplayFailure(ex.Message, "Invalid VS Project");
            //    }
        }

        private void RemoveAssembly()
        {
            if (mbox.AskYesNoQuestion(string.Format("Remove {0} from model?", view.SelectedAssembly), "Remove Assembly"))
            {
                int index = view.SelectedAssemblyIndex;
                ProjectConfig selectedConfig = model.Configs[view.SelectedConfig];
                selectedConfig.Assemblies.RemoveAt(index);
                SetAssemblyList();
            }
        }

        private void BrowseForAssembly()
        {
            string assemblyPath = view.GetAssemblyPath();

            if (assemblyPath != null)
            {
                ProjectConfig selectedConfig = model.Configs[view.SelectedConfig];
                selectedConfig.Assemblies[view.SelectedAssemblyIndex] = assemblyPath;
                SetAssemblyList();
            }
        }

        #endregion

        #region PropertyChange Handling

        private void OnPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            ProjectConfig selectedConfig = view.SelectedConfig >= 0
                ? model.Configs[view.SelectedConfig]
                : null;

            switch (e.PropertyName)
            {
                case "ProjectBase":
                    string projectBase = view.ProjectBase;
                    if (projectBase == string.Empty)
                        projectBase = Path.GetDirectoryName(model.ProjectPath);
                    
                    if (ValidateDirectoryPath("ProjectBase", view.ProjectBase))
                        model.BasePath = projectBase; ;
                    break;

                case "ProcessModel":
                    model.ProcessModel = (ProcessModel)Enum.Parse(typeof(ProcessModel), view.ProcessModel);
                    SetDomainUsageOptions();
                    break;

                case "DomainUsage":
                    model.DomainUsage = (DomainUsage)Enum.Parse(typeof(DomainUsage), view.DomainUsage);
                    break;

                case "SelectedConfig":
                    if (view.SelectedConfig >= 0)
                    {
                        RuntimeFramework framework = selectedConfig.RuntimeFramework;
                        if (framework == null)
                        {
                            view.Runtime = RuntimeType.Any.ToString();
                        }
                        else
                        {
                            view.Runtime = framework.Runtime.ToString();
                            view.RuntimeVersion = framework.ClrVersion.ToString();
                        }

                        view.ApplicationBase = selectedConfig.RelativeBasePath;
                        view.ConfigurationFile = selectedConfig.ConfigurationFile;
                        view.BinPathType = selectedConfig.BinPathType;
                        if (selectedConfig.BinPathType == BinPathType.Manual)
                            view.PrivateBinPath = selectedConfig.PrivateBinPath;
                        else
                            view.PrivateBinPath = string.Empty;

                        SetAssemblyList();
                    }
                    else
                    {
                        view.ApplicationBase = null;
                        view.ConfigurationFile = null;
                        view.PrivateBinPath = null;
                        view.BinPathType = BinPathType.Auto;

                        view.AssemblyList = new string[0];
                        view.AssemblyPath = null;

                        

                        //((System.Windows.Forms.Control)view).Enabled = false;
                    }

                    break;

                case "Runtime":
                case "RuntimeVersion":
                    RuntimeType runtime = (RuntimeType)Enum.Parse(typeof(RuntimeType), view.Runtime);
                    
                    try
                    {
                        Version version = new Version(view.RuntimeVersion);
                        selectedConfig.RuntimeFramework = new RuntimeFramework(runtime, version);
                    }
                    catch (Exception ex)
                    {
                        view.ErrorMessage("RuntimeVersion", ex.Message);
                    }
                    break;

                case "ApplicationBase":
                    string basePath = null;

                    if (view.ApplicationBase != String.Empty)
                    {
                        if (!ValidateDirectoryPath("ApplicationBase", view.ApplicationBase))
                            break;

                        basePath = Path.Combine(model.BasePath, view.ApplicationBase);
                        if (PathUtils.SamePath(model.BasePath, basePath))
                            basePath = null;
                    }

                    selectedConfig.BasePath = basePath;

                    // TODO: Test what happens if we set it the same as project base
                    if (selectedConfig.RelativeBasePath == null)
                        view.ApplicationBase = string.Empty;
                    else
                        view.ApplicationBase = selectedConfig.RelativeBasePath;
                    break;

                case "DefaultConfigurationFile":
                    string configFile = view.ConfigurationFile;

                    if (configFile == string.Empty)
                        selectedConfig.ConfigurationFile = null;
                    else if (ValidateFilePath("DefaultConfigurationFile", configFile))
                    {
                        if (configFile == Path.GetFileName(configFile))
                            selectedConfig.ConfigurationFile = view.ConfigurationFile;
                        else
                            view.ErrorMessage("DefaultConfigurationFile", "Must be file name only - without directory path");
                    }
                    break;

                case "BinPathType":
                    if (selectedConfig != null)
                        selectedConfig.BinPathType = view.BinPathType;
                    view.PrivateBinPathEnabled = view.BinPathType == BinPathType.Manual;
                    break;

                case "PrivateBinPath":
                    if (view.PrivateBinPath == string.Empty)
                        selectedConfig.PrivateBinPath = null;
                    else
                    {
                        foreach (string dir in view.PrivateBinPath.Split(Path.PathSeparator))
                        {
                            if (!ValidateDirectoryPath("PrivateBinPath", dir))
                                return;
                            if (Path.IsPathRooted(dir))
                            {
                                view.ErrorMessage("PrivateBinPath", "Components must all be relative paths");
                                return;
                            }
                        }

                        selectedConfig.PrivateBinPath = view.PrivateBinPath;
                    }
                    break;

                case "SelectedAssembly":
                    if (view.SelectedAssemblyIndex == -1)
                    {
                        view.AssemblyPath = null;
                        view.RemoveAssemblyEnabled = false;
                        view.EditAssemblyEnabled = false;
                    }
                    else
                    {
                        view.AssemblyPath = 
                            selectedConfig.Assemblies[view.SelectedAssemblyIndex];
                        view.RemoveAssemblyEnabled = true;
                        view.EditAssemblyEnabled = true;
                    }
                    break;

                case "AssemblyPath":
                    if (ValidateFilePath("AssemblyPath", view.AssemblyPath))
                    {
                        selectedConfig.Assemblies[view.SelectedAssemblyIndex] = view.AssemblyPath;
                        SetAssemblyList();
                    }
                    break;
            }
        }

        #endregion

        #region Helper Methods

        private void SetProcessModelOptions()
        {
            view.ProcessModelOptions = Enum.GetNames(typeof(ProcessModel));
        }

        private void SetDomainUsageOptions()
        {
            view.DomainUsageOptions = view.ProcessModel == ProcessModel.Multiple.ToString()
                ? new string[] { "Default", "Single" }
                : new string[] { "Default", "Single", "Multiple" };
        }

        private void SetRuntimeOptions()
        {
            view.RuntimeOptions = new string[] { "Any", "Net", "Mono" };
        }

        private void SetRuntimeVersionOptions()
        {
            string[] versions = new string[RuntimeFramework.KnownClrVersions.Length];

            for (int i = 0; i < RuntimeFramework.KnownClrVersions.Length; i++)
                versions[i] = RuntimeFramework.KnownClrVersions[i].ToString(3);

            view.RuntimeVersionOptions = versions;
        }

        private void SetAssemblyList()
        {
            ProjectConfig selectedConfig = model.Configs[view.SelectedConfig];
            string[] assemblyList = selectedConfig.Assemblies.ToArray();
            view.AssemblyList = assemblyList;

            if (assemblyList.Length == 0)
                view.AssemblyPath = "";
        }

        private bool ValidateDirectoryPath(string property, string path)
        {
            try
            {
                new DirectoryInfo(path);
                return true;
            }
            catch (Exception ex)
            {
                view.ErrorMessage(property, ex.Message);
                return false;
            }
        }

        private bool ValidateFilePath(string property, string path)
        {
            try
            {
                new FileInfo(path);
                return true;
            }
            catch (Exception ex)
            {
                view.ErrorMessage(property, ex.Message);
                return false;
            }
        }

        #endregion
    }
}
