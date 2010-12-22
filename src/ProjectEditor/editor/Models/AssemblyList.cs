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
using System.Xml;

namespace NUnit.ProjectEditor
{
	/// <summary>
	/// Represents a list of assemblies. It stores paths 
	/// that are added and fires an event whenevever it
	/// changes. All paths must be added as absolute paths.
	/// </summary>
	public class AssemblyList : IEnumerable
	{
        private XmlNode configNode;

        public AssemblyList(XmlNode configNode)
        {
            this.configNode = configNode;
        }

		#region Properties

        public int Count
        {
            get { return AssemblyNodes.Count; }
        }

        public string this[int index]
        {
            get { return XmlHelper.GetAttribute(AssemblyNodes[index], "path"); }
            set { XmlHelper.SetAttribute( AssemblyNodes[index], "path", value); }
        }

		#endregion

		#region Methods

        public void Add(string assemblyPath)
        {
            XmlHelper.AddAttribute(
                XmlHelper.AddElement(configNode, "assembly"),
                "path",
                assemblyPath);
        }

        //public void Remove( string assemblyPath )
        //{
        //    for( int index = 0; index < this.Count; index++ )
        //    {
        //        if ( this[index] == assemblyPath )
        //            RemoveAt( index );
        //    }
        //}

        public void RemoveAt(int index)
        {
            configNode.RemoveChild(AssemblyNodes[index]);
        }

        public string[] ToArray()
        {
            XmlNodeList nodes = AssemblyNodes;
            string[] array = new string[nodes.Count];

            for (int i = 0; i < nodes.Count; i++)
                array[i] = XmlHelper.GetAttribute(nodes[i], "path");

            return array;
        }

		#endregion

        #region private Properties

        private XmlNodeList AssemblyNodes
        {
            get { return configNode.SelectNodes("assembly"); }
        }

        private XmlNode GetAssemblyNodes(int index)
        {
            return AssemblyNodes[index];
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            string[] array = this.ToArray();
            return array.GetEnumerator();
        }

        #endregion
    }
}
