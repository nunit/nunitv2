// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using NUnit.Core;
using NUnit.Core.Filters;

namespace NUnit.UiKit
{
	/// <summary>
	/// The VisualState class holds the latest visual state for a project.
	/// </summary>
	[Serializable]
	public class VisualState
	{
		#region Fields
		[XmlAttribute]
		public bool ShowCheckBoxes;

		public string TopNode;

		public string SelectedNode;

		public string SelectedCategories;

		public bool ExcludeCategories;

		[XmlArrayItem("Node")]
		public VisualTreeNode[] Nodes;
		#endregion

		#region Static Methods
		public static string GetVisualStateFileName( string testFileName )
		{
			if ( testFileName == null )
				return "VisualState.xml";

			string baseName = testFileName;
			if ( baseName.EndsWith( ".nunit" ) )
				baseName = baseName.Substring( 0, baseName.Length - 6 );
			
			return baseName + ".VisualState.xml";
		}

		public static VisualState LoadFrom( string fileName )
		{
			using ( StreamReader reader = new StreamReader( fileName ) )
			{
				return LoadFrom( reader );
			}
		}

		public static VisualState LoadFrom( TextReader reader )
		{
			XmlSerializer serializer = new XmlSerializer( typeof( VisualState) );
			return (VisualState)serializer.Deserialize( reader );
		}
		#endregion

		#region Constructors
		public VisualState() { }

		public VisualState( TestSuiteTreeView treeView )
		{
			this.ShowCheckBoxes = treeView.CheckBoxes;
			this.TopNode = ((TestSuiteTreeNode)treeView.TopNode).Test.TestName.UniqueName;
			this.SelectedNode = ((TestSuiteTreeNode)treeView.SelectedNode).Test.TestName.UniqueName;
			this.Nodes = new VisualTreeNode[] { new VisualTreeNode((TestSuiteTreeNode)treeView.Nodes[0]) };
			if ( !treeView.CategoryFilter.IsEmpty )
			{
				ITestFilter filter = treeView.CategoryFilter;
				if ( filter is NotFilter )
				{
					filter = ((NotFilter)filter).BaseFilter;
					this.ExcludeCategories = true;
				}

				this.SelectedCategories = filter.ToString();
			}
		}
		#endregion

		#region Instance Methods
        public void Restore(TestSuiteTreeView tree)
        {
            if (ShowCheckBoxes != tree.CheckBoxes)
                tree.CheckBoxes = ShowCheckBoxes;

            foreach (VisualTreeNode visualNode in Nodes)
                visualNode.Restore(tree);

            if (SelectedNode != null)
            {
                TestSuiteTreeNode treeNode = tree[SelectedNode];
                if (treeNode != null)
                    tree.SelectedNode = treeNode;
            }

            if (TopNode != null)
            {
                TestSuiteTreeNode treeNode = tree[TopNode];
                if (treeNode != null)
                    tree.TryToSetTopNode(treeNode);
            }


			if (this.SelectedCategories != null)
			{
				TestFilter filter = new CategoryFilter( this.SelectedCategories.Split( new char[] { ',' } ) );
				if ( this.ExcludeCategories )
					filter = new NotFilter( filter );
				tree.CategoryFilter = filter;
			}
			
			tree.Select();
        }

		public void Save( string fileName )
		{
			using ( StreamWriter writer = new StreamWriter( fileName ) )
			{
				Save( writer );
			}
		}

		public void Save( TextWriter writer )
		{
			XmlSerializer serializer = new XmlSerializer( GetType() );
			serializer.Serialize( writer, this );
		}
		#endregion
	}

	[Serializable]
	public class VisualTreeNode
	{
		[XmlAttribute]
		public string UniqueName;

		[XmlAttribute,System.ComponentModel.DefaultValue(false)]
		public bool Expanded;

		[XmlAttribute,System.ComponentModel.DefaultValue(false)]
		public bool Checked;

		[XmlArrayItem("Node")]
		public VisualTreeNode[] Nodes;

		public VisualTreeNode() { }

		public VisualTreeNode( TestSuiteTreeNode treeNode )
		{
			this.UniqueName = treeNode.Test.TestName.UniqueName;
			this.Expanded = treeNode.IsExpanded;
			this.Checked = treeNode.Checked;
			int count = treeNode.Nodes.Count;
			if ( count > 0 )
			{
				this.Nodes = new VisualTreeNode[count];
				for( int i = 0; i < count; i++ )
					this.Nodes[i] = new VisualTreeNode( (TestSuiteTreeNode)treeNode.Nodes[i] );
			}
		}

        public void Restore(TestSuiteTreeView tree)
        {
            System.Threading.ThreadStart proc = new System.Threading.ThreadStart(RestoreProc);
            this.tree = tree;
            System.Threading.Thread thread = new System.Threading.Thread(proc);

            thread.Start();
        }

        private TestSuiteTreeView tree;

        public void RestoreProc()
        {
            TestSuiteTreeNode treeNode = tree[this.UniqueName];
            if (treeNode != null)
            {
                if (treeNode.IsExpanded != this.Expanded)
                    treeNode.Toggle();

                treeNode.Checked = this.Checked;

                if (this.Nodes != null)
                    foreach (VisualTreeNode childNode in this.Nodes)
                        childNode.Restore(tree);
            }
        }
	}
}
