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
    public class ConfigurationEditor
    {
        #region Instance Variables

        private IProjectModel model;
        private IConfigurationEditorView view;

        #endregion

        #region Constructor

        public ConfigurationEditor(IProjectModel model, IConfigurationEditorView view)
        {
            this.model = model;
            this.view = view;

            view.SelectedConfigChanged += new EventHandler(view_SelectedConfigChanged);

            UpdateConfigList();
        }

        #endregion

        #region Command Event Handlers

        public void AddConfig()
        {
            AddConfigData data = new AddConfigData();
            if (view.GetAddConfigData(ref data))
            {
                model.Configs.Add(data.ConfigToCreate);
                ProjectConfig newConfig = model.Configs[data.ConfigToCreate];

                if (data.ConfigToCopy != null)
                {
                    ProjectConfig copyConfig = model.Configs[data.ConfigToCopy];
                    if (copyConfig != null)
                    {
                        newConfig.BasePath = copyConfig.BasePath;
                        newConfig.BinPathType = copyConfig.BinPathType;
                        if (newConfig.BinPathType == BinPathType.Manual)
                            newConfig.PrivateBinPath = copyConfig.PrivateBinPath;
                        newConfig.ConfigurationFile = copyConfig.ConfigurationFile;
                        newConfig.RuntimeFramework = copyConfig.RuntimeFramework;

                        foreach (string assembly in copyConfig.Assemblies)
                            newConfig.Assemblies.Add(assembly);
                    }
                }

                UpdateConfigList();
            }
        }

        public void RenameConfig()
        {
            string[] configList = model.ConfigNames;
            string oldName = view.SelectedConfig;

            string newName = view.GetNewNameForRename(oldName);

            if (newName != null)
            {
                model.Configs[oldName].Name = newName;
                UpdateConfigList();
            }
        }

        public void RemoveConfig()
        {
            model.Configs.Remove(view.SelectedConfig);

            UpdateConfigList();
        }

        public void MakeActive()
        {
            model.ActiveConfigName = view.SelectedConfig;

            UpdateConfigList();
        }

        #endregion

        #region Other UI Event Handlers

        void view_SelectedConfigChanged(object sender, EventArgs e)
        {
            string selectedConfig = view.SelectedConfig;

            view.MakeActiveEnabled = selectedConfig != null && selectedConfig != model.ActiveConfigName;
            view.RenameConfigEnabled = view.AddConfigEnabled = selectedConfig != null;
            view.RemoveConfigEnabled = selectedConfig != null && model.Configs.Count > 0;
        }

        #endregion

        #region Helper Methods

        private void UpdateConfigList()
        {
            string selectedConfig = view.SelectedConfig;
            bool foundSelectedConfig = false;

            int count = model.Configs.Count;
            string[] configList = new string[count];

            for (int i = 0; i < count; i++)
            {
                string config = model.Configs[i].Name;

                configList[i] = config;
                if (config == selectedConfig)
                    foundSelectedConfig = true;
            }

            view.ConfigList = configList;
            view.ActiveConfigName = model.ActiveConfigName;

            if (foundSelectedConfig)
                view.SelectedConfig = selectedConfig;
            else if (configList.Length > 0)
                view.SelectedConfig = configList[0];
            else
                view.SelectedConfig = null;
        }

        #endregion
    }
}
