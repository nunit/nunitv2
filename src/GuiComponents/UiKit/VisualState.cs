using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NUnit.UiKit
{
	/// <summary>
	/// The VisualState struct holds the latest visual state for a project.
	/// </summary>
	[Serializable]
	public class VisualState
	{
		[XmlAttribute]
		public bool ShowCheckBoxes;

		public string TopNode;

		public string SelectedNode;

		[XmlArrayItem("Node")]
		public VisualTreeNode[] Nodes;

		public VisualState() { }

		public VisualState( TestSuiteTreeView treeView )
		{
			this.ShowCheckBoxes = treeView.CheckBoxes;
			this.TopNode = ((TestSuiteTreeNode)treeView.TopNode).Test.TestName.UniqueName;
			this.SelectedNode = ((TestSuiteTreeNode)treeView.SelectedNode).Test.TestName.UniqueName;
			this.Nodes = new VisualTreeNode[] { new VisualTreeNode((TestSuiteTreeNode)treeView.Nodes[0]) };
		}

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

            tree.Select();
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
