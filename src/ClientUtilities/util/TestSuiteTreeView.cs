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
using System.IO;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace NUnit.Util
{
	using NUnit.Core;
	using NUnit.UiKit;

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
		/// Whether the browser supports running tests,
		/// or just loading and examining them
		/// </summary>
		private bool runCommandSupported = false;
		
		/// <summary>
		/// Whether or not we track progress of tests visibly in the tree
		/// </summary>
		private bool displayProgress = false;

		/// <summary>
		/// Source of events that the tree responds to and
		/// target for the run command.
		/// </summary>
		private UIActions actions;
		
		public System.Windows.Forms.ImageList treeImages;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// True if the UI should allow a run command to be selected
		/// </summary>
		private bool runCommandEnabled = false;

		#endregion

		#region Construction and Initialization

		public TestSuiteTreeView()
		{
			InitializeComponent();

			this.ContextMenu = new System.Windows.Forms.ContextMenu();
			this.ContextMenu.Popup += new System.EventHandler( ContextMenu_Popup );
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			//Temporary fix till we adjust the namespaces
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TestSuiteTreeView));
			this.treeImages = new System.Windows.Forms.ImageList(this.components);
			// 
			// treeImages
			// 
			this.treeImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.treeImages.ImageSize = new System.Drawing.Size(16, 16);
			this.treeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImages.ImageStream")));
			this.treeImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// TestSuiteTreeView
			// 
			this.ImageIndex = 0;
			this.ImageList = this.treeImages;
			this.SelectedImageIndex = 0;
			this.DoubleClick += new System.EventHandler(this.TestSuiteTreeView_DoubleClick);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.TestSuiteTreeView_DragEnter);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.TestSuiteTreeView_DragDrop);

		}

		public void InitializeEvents( UIActions actions )
		{
			this.actions = actions;

			actions.TestSuiteLoadedEvent += new TestSuiteLoadedHandler( OnSuiteLoaded );
			actions.TestSuiteChangedEvent += new TestSuiteChangedHandler( OnSuiteChanged );
			actions.TestSuiteUnloadedEvent += new TestSuiteUnloadedHandler( OnSuiteUnloaded );
			actions.RunStartingEvent += new RunStartingHandler( OnRunStarting );
			actions.TestFinishedEvent += new TestFinishedHandler( OnTestFinished );
			actions.SuiteFinishedEvent += new SuiteFinishedHandler( OnSuiteFinished );
			actions.RunFinishedEvent += new RunFinishedHandler( OnRunFinished );
		}

		#endregion

		#region Properties and Events

		/// <summary>
		/// Property determining whether the run command
		/// is supported from the tree context menu and
		/// by double-clicking test cases.
		/// </summary>
		public bool RunCommandSupported
		{
			get { return runCommandSupported; }
			set { runCommandSupported = value; }
		}

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
		/// The currently selected test.
		/// </summary>
		public UITestNode SelectedTest
		{
			get 
			{ 
				TestSuiteTreeNode node = (TestSuiteTreeNode)SelectedNode;
				return node.Test;
			}
		}

		/// <summary>
		/// The currently selected test result or null
		/// </summary>
		public TestResult SelectedTestResult
		{
			get 
			{
				TestSuiteTreeNode node = (TestSuiteTreeNode)SelectedNode;
				return node.Result; 
			}
		}

		public event SelectedTestChangedHandler SelectedTestChanged;

		/// <summary>
		/// TestNode corresponding to a test fullname
		/// </summary>
		private TestSuiteTreeNode this[string testName]
		{
			get { return treeMap[testName] as TestSuiteTreeNode; }
		}

		/// <summary>
		/// Test node corresponding to a TestInfo interface
		/// </summary>
		private TestSuiteTreeNode this[TestInfo test]
		{
			get { return this[test.FullName]; }
		}

		/// <summary>
		/// Test node corresponding to a TestResultInfo
		/// </summary>
		private TestSuiteTreeNode this[TestResult result]
		{
			get { return this[result.Test.FullName]; }
		}

		#endregion

		#region Handlers for events related to loading and running tests

		private void OnSuiteLoaded( UITestNode test, string assemblyName )
		{
			Load( test );
			runCommandEnabled = true;
		}

		private void OnSuiteChanged( UITestNode test )
		{
			Invoke( new LoadHandler( Reload ), new object[]{ test } );
			ClearResults();	// ToDo: Make this optional
		}

		private void OnSuiteUnloaded()
		{
			Clear();
			runCommandEnabled = false;
		}

		private void OnRunStarting( UITestNode test )
		{
			ClearResults();
			runCommandEnabled = false;
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
			this[result].Expand();
			runCommandEnabled = true;
		}

		#endregion

		#region Handlers for UI events

		#region Context Menu

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
		/// Build treeview context menu dynamically on popup
		/// </summary>
		private void ContextMenu_Popup(object sender, System.EventArgs e)
		{
			this.ContextMenu.MenuItems.Clear();

			if ( RunCommandSupported )
			{
				MenuItem runMenuItem = new MenuItem( "&Run", new EventHandler( runMenuItem_Click ) );
				runMenuItem.DefaultItem = runMenuItem.Enabled = runCommandEnabled;
			
				this.ContextMenu.MenuItems.Add( runMenuItem );
			}

			if ( contextNode.Nodes.Count > 0 )
			{
				if ( contextNode.IsExpanded )
				{
					MenuItem collapseMenuItem = new MenuItem( 
						"&Collapse", new EventHandler( collapseMenuItem_Click ) );
					collapseMenuItem.DefaultItem = !runCommandEnabled;

					this.ContextMenu.MenuItems.Add( collapseMenuItem );
				}
				else
				{
					MenuItem expandMenuItem = new MenuItem(
						"&Expand", new EventHandler( expandMenuItem_Click ) );
					expandMenuItem.DefaultItem = !runCommandEnabled;
					this.ContextMenu.MenuItems.Add( expandMenuItem );
				}
			}

#if NUNIT_LEAKAGE_TEST
			TestResult result = contextNode.Result;
			if ( result != null )
			{
				this.ContextMenu.MenuItems.Add( "-" );
				this.ContextMenu.MenuItems.Add( string.Format( "Leakage: {0} bytes", result.Leakage ) );
			}
#endif
		}

		/// <summary>
		/// When Expand context menu item is clicked, expand the node
		/// </summary>
		private void expandMenuItem_Click(object sender, System.EventArgs e)
		{
			contextNode.Expand();
		}

		/// <summary>
		/// When Collapse context menu item is clicked, collapse the node
		/// </summary>
		private void collapseMenuItem_Click(object sender, System.EventArgs e)
		{
			contextNode.Collapse();
		}

		/// <summary>
		/// When Run context menu item is clicked, run the test that
		/// was selected when the right click was done.
		/// </summary>
		private void runMenuItem_Click(object sender, System.EventArgs e)
		{
			actions.RunTestSuite( contextNode.Test );
		}

		#endregion

		#region Drag and drop

		/// <summary>
		/// Helper method to determine if an IDataObject is valid
		/// for dropping on the tree view. It must be a the drop
		/// of a single file with a valid assembly file type.
		/// </summary>
		/// <param name="data">IDataObject to be tested</param>
		/// <returns>True if dropping is allowed</returns>
		private bool IsValidFileDrop( IDataObject data )
		{
			if ( !data.GetDataPresent( DataFormats.FileDrop ) )
				return false;

			string [] fileNames = data.GetData( DataFormats.FileDrop ) as string [];
				if ( fileNames == null )
					return false;

			return IsAssemblyFileType( fileNames[0] );
		}

		/// <summary>
		/// Helper method to determine if a file is a valid assembly file type
		/// </summary>
		/// <param name="path">File path</param>
		/// <returns>True if the file type is valid for an assembly</returns>
		private bool IsAssemblyFileType( string path )
		{
			string extension = Path.GetExtension( path );
			return extension == ".dll" || extension == ".exe";
		}

		private void TestSuiteTreeView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if ( IsValidFileDrop( e.Data ) )
			{
				string[] fileNames = e.Data.GetData( DataFormats.FileDrop ) as string[];
					actions.LoadAssembly( fileNames[0] );
			}
		}

		private void TestSuiteTreeView_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if ( IsValidFileDrop( e.Data ) )
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		#endregion

		private void TestSuiteTreeView_DoubleClick(object sender, System.EventArgs e)
		{
			if ( runCommandSupported && runCommandEnabled && SelectedNode.Nodes.Count == 0 )
			{
				actions.RunTestSuite( SelectedTest );
			}	
		}

		#endregion

		#region Public methods to manipulate the tree

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
			SelectedNode = Nodes[0];
		}

		/// <summary>
		/// Reload the tree with a changed test hierarchy
		/// while maintaining as much gui state as possible
		/// </summary>
		/// <param name="test">Test suite to be loaded</param>
		public void Reload( UITestNode test )
		{
			TestSuiteTreeNode rootNode = (TestSuiteTreeNode) Nodes[0];
			if ( !Match( rootNode, test ) )
				throw( new ArgumentException( "Reload called with non-matching test" ) );
				
			UpdateNode( rootNode, test );
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
			if ( node == null )
				throw new ArgumentException( "Test not found in tree" );

			node.SetResult( result );

			if ( DisplayTestProgress )
			{
				Invalidate( node.Bounds );
				Update();
			}
		}

		/// <summary>
		/// Collapse all fixtures in the tree
		/// </summary>
		private void CollapseFixtures()
		{
			foreach( TestSuiteTreeNode node in Nodes )
				CollapseFixturesUnderNode( node );
		}

		/// <summary>
		/// Expand all fixtures in the tree
		/// </summary>
		private void ExpandFixtures()
		{
			foreach( TestSuiteTreeNode node in Nodes )
				ExpandFixturesUnderNode( node );
		}

		#endregion

		#region Helper Methods

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
		private void RemoveNode( TestSuiteTreeNode node )
		{
			if ( contextNode == node )
				contextNode = null;
			RemoveFromMap( node );
			node.Remove();
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

		protected override void OnAfterSelect(System.Windows.Forms.TreeViewEventArgs e)
		{
			if ( SelectedTestChanged != null )
				SelectedTestChanged( SelectedTest );

			base.OnAfterSelect( e );
		}
	}
}

