
namespace NUnit.Tests
{
	using System;
	using System.Reflection;
	using NUnit.Framework;
	using NUnit.Core;
	using NUnit.Util;
	using NUnit.Tests.Assemblies;

	/// <summary>
	/// Summary description for TestSuiteFixture.
	/// </summary>
	/// 
	[TestFixture]
	public class TestSuiteTreeViewFixture
	{
		private string testsDll = "mock-assembly.dll";
		private TestSuite suite;
		private TestSuite fixture;

		[SetUp]
		public void SetUp() 
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build( testsDll );

			TestSuite suite2 = new TestSuite("My suite");
			suite2.Add( new MockTestFixture() );
			fixture = (TestSuite)suite2.Tests[0];
		}

		[Test]
		public void LoadSuite()
		{
			Assertion.AssertNotNull(suite);
		}

		private bool AllExpanded( TestNode node)
		{
			if ( node.Nodes.Count == 0 )
				return true;
			
			if ( !node.IsExpanded )
				return false;
			
			foreach( TestNode child in node.Nodes )
				if ( !AllExpanded( child ) )
					return false;

			return true;
		}

		[Test]
		public void BuildTreeView()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);
			Assertion.AssertNotNull( treeView.RootNode );
			Assertion.AssertEquals( 15, treeView.RootNode.GetNodeCount( true ) );
			Assertion.Assert( "Nodes not expanded on load", AllExpanded( treeView.RootNode ) );

			Assertion.AssertEquals( "mock-assembly.dll", treeView.Nodes[0].Text );	
			Assertion.AssertEquals( "NUnit", treeView.Nodes[0].Nodes[0].Text );
			Assertion.AssertEquals( "Tests", treeView.Nodes[0].Nodes[0].Nodes[0].Text );
		}

		[Test]
		public void MapLookup()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);

			TestNode node = treeView[fixture];
			Assertion.AssertNotNull("Lookup failed", node);

			Assertion.AssertEquals(5, node.Nodes.Count);
			Assertion.AssertEquals("MockTest1", node.Nodes[0].Text);
		}

		/// <summary>
		/// The tree view CollapseAll method doesn't seem to work in
		/// this test environment. This replaces it.
		/// </summary>
		private void CollapseAll( TestNode node )
		{
			node.Collapse();
			foreach( TestNode child in node.Nodes )
				CollapseAll( child );
		}

		[Test]
		public void ExpandTest()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);
			CollapseAll( treeView.RootNode );
			Assertion.Assert( "Tree did not collapse", !AllExpanded( treeView.RootNode ) );

			TestNode node = treeView[fixture];
			Assertion.AssertNotNull("Lookup failed", node);

			treeView.Expand( fixture );
			Assertion.Assert( "Expand failed", AllExpanded( node ) );
			Assertion.Assert( "Too much expanded", !AllExpanded( treeView.RootNode ) );
		}

		[Test]
		public void ClearTree()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);
			
			Assertion.AssertNotNull( "Not in map", treeView[fixture] );

			treeView.Clear();
			Assertion.AssertEquals( 0, treeView.Nodes.Count );
			Assertion.AssertNull( "Map not cleared", treeView[fixture] );
		}

		[Test]
		public void SetTestResult()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);

			TestNode node = treeView[fixture];
			Assertion.AssertNotNull( "Not in map", node );

			TestSuiteResult result = new TestSuiteResult( fixture, "My test result" );
			treeView.SetTestResult( result );

			Assertion.AssertNotNull( "Result not set", node.Result );
			Assertion.AssertEquals( "My test result", node.Result.Name );
			Assertion.AssertEquals( node.Test.FullName, node.Result.Test.FullName );
		}

		[Test]
		public void RemoveNode()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);

			TestNode node = treeView[fixture];
			TestNode child = (TestNode)node.Nodes[0];

			treeView.RemoveNode( node );
			Assertion.AssertEquals( 9, treeView.RootNode.GetNodeCount( true ) );
			Assertion.AssertNull( "Still in map", treeView[fixture] );
			Assertion.AssertNull( "Child still in map", treeView[child.Test] );
		}

		[Test]
		public void ReloadTree()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);

			Assertion.AssertEquals( 7, suite.CountTestCases );
			Assertion.AssertEquals( 15, treeView.RootNode.GetNodeCount( true ) );
			
			TestSuite nunitNamespaceSuite = suite.Tests[0] as TestSuite;
			TestSuite testsNamespaceSuite = nunitNamespaceSuite.Tests[0] as TestSuite;
			TestSuite assembliesNamespaceSuite = testsNamespaceSuite.Tests[1] as TestSuite;
			testsNamespaceSuite.Tests.RemoveAt( 1 );
			treeView.Reload( suite );

			Assertion.AssertEquals( 2, suite.CountTestCases );
			Assertion.AssertEquals( 8, treeView.RootNode.GetNodeCount( true ) );

			testsNamespaceSuite.Tests.Insert( 1, assembliesNamespaceSuite );
			treeView.Reload( suite );

			Assertion.AssertEquals( 7, suite.CountTestCases );
			Assertion.AssertEquals( 15, treeView.RootNode.GetNodeCount( true ) );
		}

		[Test]
		[ExpectedException( typeof(ArgumentException) )]
		public void ReloadTreeWithWrongTest()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);

			TestSuite suite2 = new TestSuite( "WrongSuite" );
			treeView.Reload( suite2 );
		}
	}
}
