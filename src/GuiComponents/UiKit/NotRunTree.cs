using System;
using System.Windows.Forms;
using NUnit.Core;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for NotRunTree.
	/// </summary>
	public class NotRunTree : TreeView
	{
		public NotRunTree()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void Add( TestResult result )
		{
			Invoke( new AddNodeHandler( AddNode ), new object[] { result } );
		}

		private delegate void AddNodeHandler( TestResult result );

		private void AddNode( TestResult result )
		{
			TreeNode node = new TreeNode(result.Name);
			TreeNode reasonNode = new TreeNode("Reason: " + result.Message);
			node.Nodes.Add(reasonNode);

			Nodes.Add( node );
		}
	}
}
