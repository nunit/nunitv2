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
using System.Xml;

namespace NUnit.ProjectEditor
{
    public class ProjectModel : IProjectModel
    {
        #region Static Fields

        /// <summary>
        /// Used to generate default names for projects
        /// </summary>
        private static int projectSeed = 0;

        /// <summary>
        /// The extension used for test projects
        /// </summary>
        private static readonly string nunitExtension = ".nunit";

        #endregion

        private enum ProjectUpdateState
        {
            NoChanges,
            XmlTextHasChanges,
            XmlDocHasChanges
        }

        #region Instance Fields

        /// <summary>
        /// The original text from which the project was loaded.
        /// Updated from the doc when the xml view is displayed
        /// and from the view when the user edits it.
        /// </summary>
        string xmlText;

        /// <summary>
        /// The XmlDocument representing the loaded project. It
        /// is generated from the text when the project is loaded
        /// unless an exception is thrown. It is modified as the
        /// user makes changes.
        /// </summary>
        XmlDocument xmlDoc;

        /// <summary>
        /// An exception thrown when trying to build the xml
        /// document from the xml text.
        /// </summary>
        Exception exception;

        /// <summary>
        /// The top-level (NUnitProject) node
        /// </summary>
        XmlNode projectNode;

        /// <summary>
        /// The Settings node in the xml doc
        /// </summary>
        XmlNode settingsNode;

        /// <summary>
        /// Path to the file storing this project
        /// </summary>
        private string projectPath;

        ///// <summary>
        ///// Collection of configs for the project
        ///// </summary>
        private ConfigList configs;

        /// <summary>
        /// True if the Xml Document has been changed
        /// what 
        /// </summary>
        private ProjectUpdateState projectUpdateState;

        /// <summary>
        /// True if the project has been changed and not yet saved
        /// </summary>
        private bool hasUnsavedChanges;

        #endregion

        #region Constructors

        public ProjectModel() : this(GenerateProjectName()) { }

        public ProjectModel(string projectPath)
        {
            this.xmlDoc = new XmlDocument();
            this.projectPath = Path.GetFullPath(projectPath);

            xmlDoc.NodeChanged += new XmlNodeChangedEventHandler(xmlDoc_Changed);
            xmlDoc.NodeInserted += new XmlNodeChangedEventHandler(xmlDoc_Changed);
            xmlDoc.NodeRemoved += new XmlNodeChangedEventHandler(xmlDoc_Changed);
        }

        #endregion

        #region IProjectModel Members

        #region Events

        public event CommandDelegate ProjectCreated;
        public event CommandDelegate ProjectClosed;
        public event CommandDelegate ProjectChanged;

        #endregion

        #region Properties

        public string XmlText
        {
            get { return xmlText; }
            set 
            { 
                xmlText = value;
                projectUpdateState = ProjectUpdateState.XmlTextHasChanges;
            }
        }

        public Exception Exception
        {
            get { return exception; }
            set
            {
                exception = value;
                projectUpdateState = ProjectUpdateState.XmlTextHasChanges;
            }
        }

        /// <summary>
        /// Gets or sets the path to which a project will be saved.
        /// </summary>
        public string ProjectPath
        {
            get { return projectPath; }
            set
            {
                string newProjectPath = Path.GetFullPath(value);
                if (newProjectPath != projectPath)
                {
                    projectPath = newProjectPath;
                }
            }
        }

        /// <summary>
        /// The base path for the project. Constructor sets
        /// it to the directory part of the project path.
        /// </summary>
        public string BasePath
        {
            get { return GetProjectLevelAttribute("appbase"); }
            set { SetProjectLevelAttribute("appbase", value); }
        }

        /// <summary>
        /// The name of the project.
        /// </summary>
        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(projectPath); }
        }

        public bool AutoConfig
        {
            get { return GetProjectLevelAttribute("autoconfig", false); }
            set { SetProjectLevelAttribute("autoconfig", value.ToString()); }
        }

        public string ActiveConfigName
        {
            get
            {
                string activeConfigName = GetProjectLevelAttribute("activeconfig");

                // In case the previous active config was removed
                if (!Configs.Contains(activeConfigName))
                    activeConfigName = null;

                // In case no active config is set or it was removed
                if (activeConfigName == null && configs.Count > 0)
                    activeConfigName = configs[0].Name;

                return activeConfigName;
            }
            set
            {
                int index = -1;

                if (value != null)
                {
                    for (int i = 0; i < configs.Count; i++)
                    {
                        if (configs[i].Name == value)
                        {
                            index = i;
                            break;
                        }
                    }
                }

                if (index >= 0)
                    SetProjectLevelAttribute("activeconfig", value);
                else
                    RemoveProjectLevelAttribute("activeconfig");
            }
        }

        public string DefaultConfigurationFile
        {
            get
            {
                // TODO: Check this
                return Path.GetFileNameWithoutExtension(projectPath) + ".config";
            }
        }

        public ProcessModel ProcessModel
        {
            get { return GetProjectLevelAttribute("processModel", ProcessModel.Default); }
            set { SetProjectLevelAttribute("processModel", value); }
        }

        public DomainUsage DomainUsage
        {
            get { return GetProjectLevelAttribute("domainUsage", DomainUsage.Default); }
            set { SetProjectLevelAttribute("domainUsage", value); }
        }

        public ConfigList Configs
        {
            get { return configs; }
        }

        public string[] ConfigNames
        {
            get
            {
                string[] configList = new string[configs.Count];
                for (int i = 0; i < configs.Count; i++)
                    configList[i] = configs[i].Name;

                return configList;
            }
        }

        public bool HasUnsavedChanges
        {
            get { return hasUnsavedChanges; }
        }

        public bool IsValid
        {
            get { return exception == null; }
        }

        #endregion

        #region Methods

        public void CreateNewProject()
        {
            this.xmlText = "<NUnitProject />";
            
            UpdateXmlDocFromXmlText();
            hasUnsavedChanges = false;

            if (ProjectCreated != null)
                ProjectCreated();
        }

        public void OpenProject(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            this.xmlText = rdr.ReadToEnd();
            rdr.Close();

            this.projectPath = Path.GetFullPath(fileName);
            UpdateXmlDocFromXmlText();

            if (ProjectCreated != null)
                ProjectCreated();
        }

        public void CloseProject()
        {
            if (ProjectClosed != null)
                ProjectClosed();
        }
      
        public void SaveProject()
        {
            XmlTextWriter writer = new XmlTextWriter(
                ProjectPathFromFile(projectPath),
                System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            WriteXmlTo(writer);

            hasUnsavedChanges = false;
        }

        public void SaveProject(string fileName)
        {
            projectPath = fileName;
            SaveProject();
        }

        public void SynchronizeModel()
        {
            switch (this.projectUpdateState)
            {
                case ProjectUpdateState.XmlTextHasChanges:
                    UpdateXmlDocFromXmlText();
                    break;

                case ProjectUpdateState.XmlDocHasChanges:
                    UpdateXmlTextFromXmlDoc();
                    break;
            }
        }

        #region Load Methods

        public void Load()
        {
            StreamReader rdr = new StreamReader(this.projectPath);
            this.xmlText = rdr.ReadToEnd();
            rdr.Close();

            LoadXml(this.xmlText);

            this.hasUnsavedChanges = false;
        }

        public void LoadXml(string xmlText)
        {
            try
            {
                this.xmlDoc.LoadXml(xmlText);

                this.projectNode = xmlDoc.FirstChild;
                if (projectNode.Name != "NUnitProject")
                    throw new ProjectFormatException("Top level element must be <NUnitProject...>.");

                this.settingsNode = projectNode.SelectSingleNode("Settings");
                this.configs = new ConfigList(this.projectNode);
            }
            catch (ProjectFormatException)
            {
                throw;
            }
            catch (XmlException e)
            {
                throw new ProjectFormatException(e.Message, e.LineNumber, e.LinePosition);
            }
            catch (Exception e)
            {
                // TODO: Figure out line numbers
                throw new ProjectFormatException(e.Message);
            }
        }

        #endregion

        #region Save methods

        public void Save()
        {
            xmlText = this.ToXml();
            StreamWriter writer = new StreamWriter(
                ProjectPathFromFile(projectPath), 
                false,
                System.Text.Encoding.UTF8);
            writer.Write(xmlText);
            writer.Close();

            //XmlTextWriter writer = new XmlTextWriter(
            //    ProjectPathFromFile(projectPath),
            //    System.Text.Encoding.UTF8);
            //writer.Formatting = Formatting.Indented;

            //WriteXmlTo(writer);

            hasUnsavedChanges = false;
        }

        public void Save(string fileName)
        {
            this.projectPath = Path.GetFullPath(fileName);
            Save();
        }

        public string ToXml()
        {
            StringWriter buffer = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(buffer);
            writer.Formatting = Formatting.Indented;

            WriteXmlTo(writer);

            return buffer.ToString();
        }

        public void WriteXmlTo(XmlTextWriter writer)
        {
#if !XML_DOC_WRITER
            xmlDoc.WriteTo(writer);
            writer.Close();
#else
            writer.WriteStartElement("NUnitProject");

            string basePath = this.BasePath;

            if (Configs.Count > 0 || basePath != null || AutoConfig ||
                ProcessModel != ProcessModel.Default || DomainUsage != DomainUsage.Default)
            {
                writer.WriteStartElement("Settings");
                if (Configs.Count > 0)
                    writer.WriteAttributeString("activeconfig", ActiveConfigName);
                if (basePath != null)
                    writer.WriteAttributeString("appbase", basePath);
                if (AutoConfig)
                    writer.WriteAttributeString("attr", "true");
                if (ProcessModel != ProcessModel.Default)
                    writer.WriteAttributeString("processModel", ProcessModel.ToString());
                if (DomainUsage != DomainUsage.Default)
                    writer.WriteAttributeString("domainUsage", DomainUsage.ToString());
                writer.WriteEndElement();
            }

            foreach (ProjectConfig config in Configs)
            {
                writer.WriteStartElement("Config");
                writer.WriteAttributeString("name", config.Name);

                string appbase = config.BasePath;
                if (appbase != null)
                    writer.WriteAttributeString("appbase", appbase);

                string configFile = config.ConfigurationFile;
                if (configFile != null && configFile != DefaultConfigurationFile)
                    writer.WriteAttributeString("configfile", config.ConfigurationFile);

                if (config.BinPathType == BinPathType.Manual)
                    writer.WriteAttributeString("binpath", config.PrivateBinPath);
                else
                    writer.WriteAttributeString("binpathtype", config.BinPathType.ToString());

                if (config.RuntimeFramework != null)
                    writer.WriteAttributeString("runtimeFramework", config.RuntimeFramework.ToString());

                foreach (string assembly in config.Assemblies)
                {
                    writer.WriteStartElement("assembly");
                    writer.WriteAttributeString("path", assembly);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.Close();
#endif
        }

        #endregion

        #endregion

        #endregion

        #region Event Handlers

        void xmlDoc_Changed(object sender, XmlNodeChangedEventArgs e)
        {
            hasUnsavedChanges = true;
            projectUpdateState = ProjectUpdateState.XmlDocHasChanges;

            if (this.ProjectChanged != null)
                ProjectChanged();
        }

        #endregion

        #region Private Properties and Helper Methods

        private string DefaultBasePath
        {
            get { return Path.GetDirectoryName(projectPath); }
        }

        public static bool IsProjectFile(string path)
        {
            return Path.GetExtension(path) == nunitExtension;
        }

        private static string ProjectPathFromFile(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path) + nunitExtension;
            return Path.Combine(Path.GetDirectoryName(path), fileName);
        }

        private static string GenerateProjectName()
        {
            return string.Format("Project{0}", ++projectSeed);
        }

        private void UpdateXmlDocFromXmlText()
        {
            try
            {
                LoadXml(xmlText);
                exception = null;
                projectUpdateState = ProjectUpdateState.NoChanges;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
        }

        private void UpdateXmlTextFromXmlDoc()
        {
            xmlText = this.ToXml();
            projectUpdateState = ProjectUpdateState.NoChanges;
        }

        private string GetProjectLevelAttribute(string name)
        {
            if (settingsNode == null)
                return null;

            return XmlHelper.GetAttribute(settingsNode, name);
        }

        private bool GetProjectLevelAttribute(string name, bool defaultValue)
        {
            string val = GetProjectLevelAttribute(name);
            return val == null
                ? defaultValue
                : bool.Parse(val);
        }

        private T GetProjectLevelAttribute<T>(string name, T defaultValue)
        {
            if (settingsNode == null)
                return defaultValue;

            return XmlHelper.GetAttributeAsEnum(settingsNode, name, defaultValue);
        }

        private void SetProjectLevelAttribute(string name, object value)
        {
            if (settingsNode == null)
            {
                settingsNode = xmlDoc.CreateElement("Settings");
                projectNode.InsertAfter(settingsNode, null);
            }

            XmlHelper.SetAttribute(settingsNode, name, value);
        }

        private void RemoveProjectLevelAttribute(string name)
        {
            if (settingsNode != null)
                XmlHelper.RemoveAttribute(settingsNode, name);
        }

        #endregion
    }
}
