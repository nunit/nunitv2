#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace NUnit.UiKit
{
	using NUnit.Core;
	using NUnit.Util;

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
		#region Instance Variables

		/// <summary>
		/// Hashtable provides direct access to TestNodes
		/// </summary>
		private Hashtable treeMap = new Hashtable();
	
		/// <summary>
		/// The TestNode on which a right click was done
		/// </summary>
		private TestSuiteTreeNode contextNode;

		/// <summary>
		/// Whether or not we track progress of tests visibly in the tree
		/// </summary>
		private bool displayProgress = false;

		private UIEvents uievents;

		#endregion

		#region Properties

		/// <summary>
		/// Property determining whether tree should redraw nodes
		/// as tests are complete in order to show progress.
		/// </summary>
		public bool DisplayTestProgress
		{
			get { return displayProgress; }
			set { displayProgress = value; }
		}

		/// <summary>
		/// A type-safe version of SelectedNode.
		/// </summary>
		public new TestSuiteTreeNode SelectedNode
		{
			get	{ return base.SelectedNode as TestSuiteTreeNode;	}
			set	{ base.SelectedNode = value; }
		}

		/// <summary>
		/// A type-safe version of TopNode.
		/// </summary>
		public new TestSuiteTreeNode TopNode
		{
			get	{ return base.TopNode as TestSuiteTreeNode;	}
		}

		/// <summary>
		/// A type-safe way to get the root node
		/// of the tree. Presumed to be unique.
		/// </summary>
		public TestSuiteTreeNode RootNode
		{
			get { return Nodes[0] as TestSuiteTreeNode; }
		}
		
		/// <summary>
		/// The TestNode that any context menu
		/// commands will operate on.
		/// </summary>
		public TestSuiteTreeNode ContextNode
		{
			get	{ return contextNode; }
		}

		/// <summary>
		/// TestNode corresponding to a test fullname
		/// </summary>
		public TestSuiteTreeNode this[string testName]
		{
			get { return treeMap[testName] as TestSuiteTreeNode; }
		}

		/// <summary>
		/// Test node corresponding to a Test
		/// </summary>
		public TestSuiteTreeNode this[Test test]
		{
			get { return this[test.FullName]; }
		}

		/// <summary>
		/// Test node corresponding to a TestInfo
		/// </summary>
		public TestSuiteTreeNode this[UITestNode test]
		{
			get { return this[test.FullName]; }
		}

		/// <summary>
		/// Test node corresponding to a TestResultInfo
		/// </summary>
		public TestSuiteTreeNode this[TestResult result]
		{
			get { return this[result.Test.FullName]; }
		}

		#endregion

		#region Methods

		public void InitializeEvents( UIEvents uievents )
		{
			this.uievents = uievents;

			uievents.TestSuiteLoadedEvent += new TestSuiteLoadedHandler( OnSuiteLoaded );
			uievents.TestSuiteChangedEvent += new TestSuiteChangedHandler( OnSuiteChanged );
			uievents.TestSuiteUnloadedEvent += new TestSuiteUnloadedHandler( OnSuiteUnloaded );
			uievents.RunStartingEvent += new RunStartingHandler( OnRunStarting );
			uievents.TestFinishedEvent += new TestFinishedHandler( OnTestFinished );
			uievents.SuiteFinishedEvent += new SuiteFinishedHandler( OnSuiteFinished );
			uievents.RunFinishedEvent += new RunFinishedHandler( OnRunFinished );
		}

		private void OnRunStarting( UITestNode test )
		{
			ClearResults();
		}

		private void OnSuiteLoaded( UITestNode test, string assemblyName )
		{
			Load( test );
		}

		private void OnSuiteChanged( UITestNode test )
		{
			Reload( test );
			ClearResults();	// ToDo: Make this optional
		}

		private void OnSuiteUnloaded()
		{
			Clear();
		}

		private void OnTestFinished( TestCaseResult result )
		{
			SetTestResult(result);
		}

		private void OnSuiteFinished( TestSuiteResult result )
		{
			SetTestResult(result);
		}

		private void OnRunFinished( TestResult result )
		{
			Expand( result.Test );
		}

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
					contextNode = theNode as TestSuiteTreeNode;
			}

			base.OnMouseDown( e );
		}

		/// <summary>
		/// A type-safe version of GetNodeAt
		/// </summary>
		/// <param name="x">X Position for which the node is to be returned</param>
		/// <param name="y">Y Position for which the node is to be returned</param>
		/// <returns></returns>
		public new TestSuiteTreeNode GetNodeAt(int x, int y)
		{
			return base.GetNodeAt(x, y) as TestSuiteTreeNode;
		}

		/// <summary>
		/// A type-safe version of GetNodeAt
		/// </summary>
		/// <param name="pt">Position for which the node is to be returned</param>
		/// <returns></returns>
		public new TestSuiteTreeNode GetNodeAt(Point pt)
		{
			return base.GetNodeAt(pt) as TestSuiteTreeNode;
		}

		/// <summary>
		/// Clear all the results in the tree.
		/// </summary>
		public void ClearResults()
		{
			foreach ( TestSuiteTreeNode rootNode in Nodes )
				rootNode.ClearResults();
		}

		/// <summary>
		/// Load the tree with a test hierarchy
		/// </summary>
		/// <param name="test">Test to be loaded</param>
		public void Load( UITestNode test )
		{
			Clear();
			AddTreeNodes( Nodes, test, false );
			ExpandAll();
			SelectedNode = Nodes[0] as TestSuiteTreeNode;
		}

		/// <summary>
		/// Add nodes to the tree constructed from a test
		/// </summary>
		/// <param name="nodes">The TreeNodeCollection to which the new node should  be added</param>
		/// <param name="rootTest">The test for which a node is to be built</param>
		/// <param name="highlight">If true, highlight the text for this node in the tree</param>
		/// <returns>A newly constructed TestNode, possibly with descendant nodes</returns>
		private TestSuiteTreeNode AddTreeNodes( IList nodes, UITestNode rootTest, bool highlight )
		{
			TestSuiteTreeNode node = new TestSuiteTreeNode( rootTest );
//			if ( highlight ) node.ForeColor = Color.Blue;
			treeMap.Add( node.Test.FullName, node );
			nodes.Add( node );
			
			if ( rootTest.IsSuite )
			{
				foreach( UITestNode test in rootTest.Tests )
					AddTreeNodes( node.Nodes, test, highlight );
			}

			return node;
		}

		private void RemoveFromMap( TestSuiteTreeNode node )
		{
			foreach( TestSuiteTreeNode child in node.Nodes )
				RemoveFromMap( child );

			treeMap.Remove( node.Test.FullName );
		}

		/// <summary>
		/// Remove a node from the tree itself and the hashtable
		/// </summary>
		/// <param name="node">Node to remove</param>
		public void RemoveNode( TestSuiteTreeNode node )
		{
			if ( contextNode == node )
				contextNode = null;
			RemoveFromMap( node );
			node.Remove();
		}

		/// <summary>
		/// Reload the tree with a changed test hierarchy
		/// while maintaining as much gui state as possible
		/// </summary>
		/// <param name="test">Test suite to be loaded</param>
		public void Reload( UITestNode test )
		{
			if ( !Match( RootNode, test ) )
				throw( new ArgumentException( "Reload called with non-matching test" ) );
				
			UpdateNode( RootNode, test );
		}

		/// <summary>
		/// Helper routine that compares a node with a test
		/// </summary>
		/// <param name="node">Node to compare</param>
		/// <param name="test">Test to compare</param>
		/// <returns>True if the test has the same name</returns>
		private bool Match( TestSuiteTreeNode node, UITestNode test )
		{
			return node.Test.FullName == test.FullName;
		}

		/// <summary>
		/// A node has been matched with a test, so update it
		/// and then process child nodes and tests recursively.
		/// If a child was added or removed, then this node
		/// will expand itself.
		/// </summary>
		/// <param name="node">Node to be updated</param>
		/// <param name="test">Test to plug into node</param>
		/// <returns>True if a child node was added or deleted</returns>
		private bool UpdateNode( TestSuiteTreeNode node, UITestNode test )
		{
			node.UpdateTest( test );
			
			if ( !test.IsSuite )
				return false;

			bool showChildren = UpdateNodes( node.Nodes, test.Tests );

			if ( showChildren ) node.Expand();

			return showChildren;
		}

		/// <summary>
		/// Match a set of nodes against a set of tests.
		/// Remove nodes that are no longer represented
		/// in the tests. Update any nodes that match.
		/// Add new nodes for new tests.
		/// </summary>
		/// <param name="nodes">List of nodes to be matched</param>
		/// <param name="tests">List of tests to be matched</param>
		/// <returns>True if the parent should expand to show that something was added or deleted</returns>
		private bool UpdateNodes( IList nodes, IList tests )
		{
			bool showChanges = false;

			foreach( TestSuiteTreeNode node in nodes )
				if ( NodeWasDeleted( node, tests ) )
				{
					RemoveNode( node );
					showChanges = true;
				}

			foreach( UITestNode test in tests )
			{
				TestSuiteTreeNode node = this[ test ];
				if ( node == null )
				{
					AddTreeNodes( nodes, test, true );
					showChanges = true;
				}
				else
					UpdateNode( node, test );
			}

			return showChanges;
		}

		/// <summary>
		/// Helper returns true if the node test is not in
		/// the list of tests provided.
		/// </summary>
		/// <param name="node">Node to examine</param>
		/// <param name="tests">List of tests to match with node</param>
		private bool NodeWasDeleted( TestSuiteTreeNode node, IList tests )
		{
			foreach ( UITestNode test in tests )
				if( Match( node, test ) )
					return false;

			return true;
		}

		/// <summary>
		/// Delegate for use in invoking the tree loader
		/// from the watcher thread.
		/// </summary>
		private delegate void LoadHandler( UITestNode test );
		
		/// <summary>
		/// Called to load the tree from the watcher thread.
		/// </summary>
		/// <param name="test"></param>
		public void InvokeLoadHandler( UITestNode test )
		{
			Invoke( new LoadHandler( Reload ), new object[]{ test } );
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
		/// Add the result of a test to the tree
		/// </summary>
		/// <param name="result">The result of the test</param>
		public void SetTestResult(TestResult result)
		{
			TestSuiteTreeNode node = this[result];	
			if ( node != null )
			{
				node.SetResult( result );

				if ( DisplayTestProgress )
				{
					Invalidate( node.Bounds );
					Update();
				}
			}
//			else
//				Console.Error.WriteLine("Could not locate node: " + result.Test.FullName + " in tree map");
		}

		/// <summary>
		/// Find and expand a particular test in the tree
		/// </summary>
		/// <param name="test">The test to expand</param>
		public void Expand( TestInfo test )
		{
			TestSuiteTreeNode node = this[test.FullName];
			if ( node != null )
				node.Expand();
		}

		/// <summary>
        /// Collapse all fixtures in the tree
        /// </summary>
		public void CollapseFixtures()
		{
			CollapseFixturesUnderNode( RootNode );
		}

		/// <summary>
		/// Helper collapses all fixtures under a node
		/// </summary>
		/// <param name="node">Node under which to collapse fixtures</param>
		private void CollapseFixturesUnderNode( TestSuiteTreeNode node )
		{
			if ( node.Test.IsFixture )
				node.Collapse();
			else 
				foreach( TestSuiteTreeNode child in node.Nodes )
					CollapseFixturesUnderNode( child );		
		}

		/// <summary>
		/// Expand all fixtures in the tree
		/// </summary>
		public void ExpandFixtures()
		{
			ExpandFixturesUnderNode( RootNode );
		}

		/// <summary>
		/// Helper expands all fixtures under a node
		/// </summary>
		/// <param name="node">Node under which to expand fixtures</param>
		private void ExpandFixturesUnderNode( TestSuiteTreeNode node )
		{
			if ( node.Test.IsFixture )
				node.Expand();
			else 
				foreach( TestSuiteTreeNode child in node.Nodes )
					ExpandFixturesUnderNode( child );		
		}
        #endregion
	}
}

