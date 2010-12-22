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
using System.Xml;

namespace NUnit.ProjectEditor
{
	/// <summary>
	/// Summary description for ConfigList.
	/// </summary>
	public class ConfigList : IEnumerable<ProjectConfig>
	{
        private XmlNode projectNode;

		public ConfigList(XmlNode projectNode) 
		{
            this.projectNode = projectNode;
		}

		#region Properties

        public int Count
        {
            get { return ConfigNodes.Count; }
        }

		public ProjectConfig this[int index]
		{
            get { return new ProjectConfig(ConfigNodes[index]); }
		}

        public ProjectConfig this[string name]
        {
            get
            {
                int index = IndexOf(name);
                return index >= 0 ? this[index] : null;
            }
        }

        private XmlNodeList ConfigNodes
        {
            get { return projectNode.SelectNodes("Config"); }
        }

		#endregion

		#region Methods

        public ProjectConfig Add( string name )
		{
            XmlNode configNode = XmlHelper.AddElement(projectNode, "Config");
            XmlHelper.AddAttribute(configNode, "name", name);

            return new ProjectConfig(configNode);
		}

        public void RemoveAt(int index)
        {
            projectNode.RemoveChild(ConfigNodes[index]);
        }

        public void Remove(string name)
        {
            int index = IndexOf(name);
            if (index >= 0)
            {
                RemoveAt(index);
            }
        }

        private int IndexOf(string name)
        {
            for (int index = 0; index < ConfigNodes.Count; index++)
            {
                if (XmlHelper.GetAttribute(ConfigNodes[index], "name") == name)
                    return index;
            }

            return -1;
        }

        //public bool Contains( ProjectConfig config )
        //{
        //    return InnerList.Contains( config );
        //}

        public bool Contains(string name)
        {
            return IndexOf(name) >= 0;
        }

		#endregion

        #region IEnumerable Members

        public IEnumerator<ProjectConfig> GetEnumerator()
        {
            List<ProjectConfig> list = new List<ProjectConfig>();
            foreach (XmlNode node in ConfigNodes)
                list.Add(new ProjectConfig(node));
            return list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
