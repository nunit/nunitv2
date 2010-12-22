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
	public enum BinPathType
	{
		Auto,
		Manual,
		None
	}

	public class ProjectConfig
	{
		#region Instance Variables

        /// <summary>
        /// The XmlNode representing this config
        /// </summary>
        private XmlNode configNode;
        
		/// <summary>
		/// IProject interface of containing project
		/// </summary>
		protected ProjectModel project = null;

        /// <summary>
        /// List of the test assemblies in this config
        /// </summary>
        private AssemblyList assemblies;

		#endregion

		#region Constructor

        public ProjectConfig(XmlNode configNode)
        {
            this.configNode = configNode;
            this.assemblies = new AssemblyList(configNode);
        }

		#endregion

		#region Properties

        public string Name
        {
            get { return GetAttribute("name"); }
            set { SetAttribute("name", value); }
        }

        /// <summary>
        /// The base directory for this config - used
        /// as the application base for loading tests.
        /// </summary>
        public string BasePath
        {
            get { return GetAttribute("appbase"); }
            set { SetAttribute("appbase", value); }
        }

        /// <summary>
        /// The base path relative to the project base
        /// </summary>
        public string RelativeBasePath
        {
            get
            {
                string basePath = BasePath;

                if (project == null || basePath == null || !Path.IsPathRooted(basePath))
                    return basePath;

                return PathUtils.RelativePath(project.BasePath, basePath);
            }
        }

        public string ConfigurationFile
        {
            get { return GetAttribute("configfile"); }
            set { SetAttribute("configfile", value); }
        }

        /// <summary>
        /// The Path.PathSeparator-separated path containing all the
        /// assemblies in the list.
        /// </summary>
        public string PrivateBinPath
        {
            get { return GetAttribute("binpath"); }
            set { SetAttribute("binpath", value); }
        }

        /// <summary>
        /// How our PrivateBinPath is generated
        /// </summary>
        public BinPathType BinPathType
        {
            get 
            { 
                return XmlHelper.GetAttributeAsEnum(
                    configNode, 
                    "binpathtype", 
                    PrivateBinPath == null
                        ? BinPathType.Auto
                        : BinPathType.Manual);
            }
            set { SetAttribute("binpathtype", value); }
        }

        /// <summary>
        /// Return our AssemblyList
        /// </summary>
        public AssemblyList Assemblies
        {
            get { return assemblies; }
        }

        public RuntimeFramework RuntimeFramework
        {
            get 
            { 
                string runtime = GetAttribute("runtimeFramework");
                return runtime == null ? null : RuntimeFramework.Parse(runtime);
            }
            set { SetAttribute("runtimeFramework", value); }
        }

		#endregion

        #region Helper Methods

        private string GetAttribute(string name)
        {
            return XmlHelper.GetAttribute(configNode, name);
        }

        private void SetAttribute(string name, object value)
        {
            XmlHelper.SetAttribute(configNode, name, value);
        }

        #endregion
    }
}
