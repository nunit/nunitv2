/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace NUnit.Util
{
	using NUnit.Core;
	using NUnit.Framework;

	/// <summary>
	/// TestSuiteTreeView is a tree view control
	/// specialized for displaying the tests
	/// in an assembly. Clients should always
	/// use TestNode rather than TreeNode when
	/// dealing with this class to be sure of
	/// calling the proper methods.
	/// </summary>
	public class TestSuiteTreeView : TreeView
	{
		/// <summary>
		/// Hashtable provides direct access to TestNodes
		/// </summary>
		private Hashtable treeMap = new Hashtable();
	
		/// <summary>
		/// The TestNode on which a right click was done
		/// </summary>
		private TestNode _contextNode;


		#region Type-Safe Versions of TreeView members
		/// <summary>
		/// A type-safe version of SelectedNode.
		/// </summary>
		public new TestNode SelectedNode
		{
			get	{ return base.SelectedNode as TestNode;	}
			set	{ base.SelectedNode = value; }
		}

		/// <summary>
		/// A type-safe version of TopNode.
		/// </summary>
		public new TestNode TopNode
		{
			get	{ return base.TopNode as TestNode;	}
		}

		/// <summary>
		/// A type-safe version of GetNodeAt
		/// </summary>
		/// <param name="x">X Position for which the node is to be returned</param>
		/// <param name="y">Y Position for which the node is to be returned</param>
		/// <returns></returns>
		public new TestNode GetNodeAt(int x, int y)
		{
			return base.GetNodeAt(x, y) as TestNode;
		}

		/// <summary>
		/// A type-safe version of GetNodeAt
		/// </summary>
		/// <param name="pt">Position for which the node is to be returned</param>
		/// <returns></returns>
		public new TestNode GetNodeAt(Point pt)
		{
			return base.GetNodeAt(pt) as TestNode;
		}
		#endregion

		#region Additional properties
		/// <summary>
		/// A type-safe way to get the root node
		/// of the tree. Presumed to be unique.
		/// </summary>
		public TestNode RootNode
		{
			get { return Nodes[0] as TestNode; }
		}
		
		/// <summary>
		/// The TestNode that any context menu
		/// commands will operate on.
		/// </summary>
		public TestNode ContextNode
		{
			get	{ return _contextNode; }
		}

		public TestNode this[Test test]
		{
			get { return treeMap[test.FullName] as TestNode; }
		}

		/// <summary>
		/// The currently selected test suite
		/// </summary>
		public Test SelectedSuite
		{
			get 
			{ 
				if ( SelectedNode == null )
					return null;
				
				return SelectedNode.Test; 
			}
		}

		/// <summary>
		/// The test that any context menu commands
		/// will apply to.
		/// </summary>
		public Test ContextSuite
		{
			get	
			{ 
				if ( ContextNode == null )
					return null;

				return ContextNode.Test; 
			}
		}
		#endregion

		/// <summary>
		/// Handles right mouse button down by
		/// remembering the proper context item.
		/// </summary>
		/// <param name="e">MouseEventArgs structure with information about the mouse position and button state</param>
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right )
			{
				TreeNode theNode = GetNodeAt( e.X, e.Y );
				if ( theNode != null )
					_contextNode = theNode as TestNode;
			}

			base.OnMouseDown( e );
		}

		#region Methods

		/// <summary>
		/// Clear all the results in the tree.
		/// </summary>
		public void ClearResults()
		{
			foreach ( TestNode rootNode in Nodes )
				rootNode.ClearResults();
		}

		/// <summary>
		/// Load the tree with a test hierarchy
		/// </summary>
		/// <param name="test">Test suite to be loaded</param>
		public void Load(Test test)
		{
			Clear();
			Nodes.Add(BuildTreeNode(test));
			ExpandAll();
			SelectedNode = RootNode;
		}

		/// <summary>
		/// Clear all the info in the tree.
		/// </summary>
		public void Clear()
		{
			treeMap.Clear();
			Nodes.Clear();
		}

		/// <summary>
		/// Build a tree node from a test
		/// </summary>
		/// <param name="rootTest">The test for which a node is to be built</param>
		/// <returns>A newly constructed TestNode, possibly with descendant nodes</returns>
		private TreeNode BuildTreeNode(Test rootTest)
		{
			TestNode node = new TestNode(rootTest);
			string localName = rootTest.FullName;
			treeMap.Add(rootTest.FullName, node);
			
			if(rootTest is TestSuite)
			{
				TestSuite testSuite = (TestSuite)rootTest;
				foreach(Test test in testSuite.Tests)
				{
					node.Nodes.Add(BuildTreeNode(test));
				}
			}

			return node;
		}

		/// <summary>
		/// Add the result of a test to the tree
		/// </summary>
		/// <param name="result">The result of the test</param>
		public void SetTestResult(TestResult result)
		{
			TestNode node = this[result.Test];	
			if ( node != null )
				node.SetResult( result );
			else
				Console.Error.WriteLine("Could not locate node: " + result.Test.FullName + " in tree map");
		}

		/// <summary>
		/// Find and expand a particular test in the tree
		/// </summary>
		/// <param name="test">The test to expand</param>
		public void Expand( Test test )
		{
			TestNode node = this[test];
			if ( node != null )
				node.Expand();
		}

#if CHARLIE		
        /// <summary>
        /// Collapse all fixtures in the tree
        /// </summary>
		public void CollapseFixtures()
		{
			ExpandFixtures( RootNode, false );
		}

		/// <summary>
		/// Expand all fixtures in the tree
		/// </summary>
		public void ExpandFixtures()
		{
			ExpandFixtures( RootNode, true );
		}

		/// <summary>
		/// Helper routine that expands/collapses fixture nodes
		/// </summary>
		/// <param name="node">The node to operatet on recursively</param>
		/// <param name="expanding">True if expanding, false if collapsing</param>
		/// <returns></returns>
		private bool ExpandFixtures( TestNode node, bool expanding )
		{
			if ( node.Test is TestSuite )
			{
				bool foundChildSuite = false;

				foreach( TestNode child in node.Nodes )
				{
					if ( ExpandFixtures(child, expanding) )
						foundChildSuite = true;
				}

				if (!foundChildSuite)
				{
					if ( expanding )
						node.Expand();
					else
						node.Collapse();
				}

				return true;
			}

			return false;
		}
#endif
        #endregion
	}
}

