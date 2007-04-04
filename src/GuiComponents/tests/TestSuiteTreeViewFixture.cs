// ****************************************************************
// Copyright 2002-2003, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

namespace NUnit.UiKit.Tests
{
	using System;
	using System.Reflection;
	using System.Windows.Forms;
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
		private Test suite;
		private TestSuiteTreeView treeView;

		[SetUp]
		public void SetUp() 
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build( new TestPackage( testsDll ) );

			treeView = new TestSuiteTreeView();
		}

		private bool AllExpanded( TreeNode node)
		{
			if ( node.Nodes.Count == 0 )
				return true;
			
			if ( !node.IsExpanded )
				return false;
			
			return AllExpanded( node.Nodes );
		}

		private bool AllExpanded( TreeNodeCollection nodes )
		{
			foreach( TestSuiteTreeNode node in nodes )
				if ( !AllExpanded( node ) )
					return false;

			return true;
		}

		[Test]
		public void BuildTreeView()
		{
			treeView.Load( new TestNode( suite ) );
			Assert.IsNotNull( treeView.Nodes[0] );
			Assert.AreEqual( MockAssembly.Nodes, treeView.GetNodeCount( true ) );
			Assert.AreEqual( "mock-assembly.dll", treeView.Nodes[0].Text );	
			Assert.AreEqual( "NUnit", treeView.Nodes[0].Nodes[0].Text );
			Assert.AreEqual( "Tests", treeView.Nodes[0].Nodes[0].Nodes[0].Text );
		}

		[Test]
		public void BuildFromResult()
		{
			TestResult result = suite.Run( new NullListener() );
			treeView.Load( result );
			Assert.AreEqual( MockAssembly.Nodes - MockAssembly.Explicit - MockAssembly.ExplicitFixtures, 
				treeView.GetNodeCount( true ) );
			
			TestSuiteTreeNode node = treeView.Nodes[0] as TestSuiteTreeNode;
			Assert.AreEqual( "mock-assembly.dll", node.Text );
			Assert.IsNotNull( node.Result, "No Result on top-level Node" );
	
			node = node.Nodes[0].Nodes[0] as TestSuiteTreeNode;
			Assert.AreEqual( "Tests", node.Text );
			Assert.IsNotNull( node.Result, "No Result on TestSuite" );

			foreach( TestSuiteTreeNode child in node.Nodes )
			{
				if ( child.Text == "Assemblies" )
				{
					node = child.Nodes[0] as TestSuiteTreeNode;
					Assert.AreEqual( "MockTestFixture", node.Text );
					Assert.IsNotNull( node.Result, "No Result on TestFixture" );
					Assert.AreEqual( true, node.Result.Executed, "MockTestFixture: Executed" );

					TestSuiteTreeNode test1 = node.Nodes[0] as TestSuiteTreeNode;
					Assert.AreEqual( "MockTest1", test1.Text );
					Assert.IsNotNull( test1.Result, "No Result on TestCase" );
					Assert.AreEqual( true, test1.Result.Executed, "MockTest1: Executed" );
					Assert.AreEqual( false, test1.Result.IsFailure, "MockTest1: IsFailure");
					Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, test1.ImageIndex );

					TestSuiteTreeNode test4 = node.Nodes[3] as TestSuiteTreeNode;
					Assert.AreEqual( false, test4.Result.Executed, "MockTest4: Executed" );
					Assert.AreEqual( TestSuiteTreeNode.IgnoredIndex, test4.ImageIndex );
					return;
				}
			}

			Assert.Fail( "Cannot locate NUnit.Tests.Assemblies node" );
		}

		/// <summary>
		/// Return the MockTestFixture node from a tree built
		/// from the mock-assembly dll.
		/// </summary>
		private TestSuiteTreeNode FixtureNode( TestSuiteTreeView treeView )
		{
			return (TestSuiteTreeNode)treeView.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[0];
		}

		/// <summary>
		/// The tree view CollapseAll method doesn't seem to work in
		/// this test environment. This replaces it.
		/// </summary>
		private void CollapseAll( TreeNode node )
		{
			node.Collapse();
			CollapseAll( node.Nodes );
		}

		private void CollapseAll( TreeNodeCollection nodes )
		{
			foreach( TreeNode node in nodes )
				CollapseAll( node );
		}

		[Test]
		public void ClearTree()
		{
			treeView.Load( new TestNode( suite ) );
			
			treeView.Clear();
			Assert.AreEqual( 0, treeView.Nodes.Count );
		}

		[Test]
		public void SetTestResult()
		{
			treeView.Load( new TestNode( suite ) );
			
			TestSuite fixture = (TestSuite)findTest( "MockTestFixture", suite );		
			TestSuiteResult result = new TestSuiteResult( new TestInfo( fixture ), "My test result" );
			treeView.SetTestResult( result );

			TestSuiteTreeNode fixtureNode = FixtureNode( treeView );
			Assert.IsNotNull(fixtureNode.Result,  "Result not set" );
			Assert.AreEqual( "My test result", fixtureNode.Result.Name );
			Assert.AreEqual( fixtureNode.Test.TestName.FullName, fixtureNode.Result.Test.TestName.FullName );
		}

		private Test findTest(string name, Test test) 
		{
			Test result = null;
			if (test.TestName.Name == name)
				result = test;
			else if (test.Tests != null)
			{
				foreach(Test t in test.Tests) 
				{
					result = findTest(name, t);
					if (result != null)
						break;
				}
			}

			return result;
		}

		[Test]
		public void ReloadTree()
		{
			// TODO: 
			// This test is not a true simulation of what happens
			// when a test is reloaded because the old nodes don't
			// actually
			treeView.Load( new TestNode( suite ) );

			Assert.AreEqual( MockAssembly.Tests, suite.TestCount );
			Assert.AreEqual( MockAssembly.Nodes, treeView.GetNodeCount( true ) );
			CheckTree( treeView, suite, "initially" );

			TestSuite nunitNamespaceSuite = suite.Tests[0] as TestSuite;
			TestSuite testsNamespaceSuite = nunitNamespaceSuite.Tests[0] as TestSuite;
			TestSuite assembliesNamespaceSuite = testsNamespaceSuite.Tests[0] as TestSuite;
			testsNamespaceSuite.Tests.RemoveAt( 0 );
			ReassignTestIDs( suite );
			
			treeView.Reload( new TestNode( suite ) );
			CheckTree( treeView, suite, "after remove" );

			Assert.AreEqual( MockAssembly.Tests - MockTestFixture.Tests, suite.TestCount );
			Assert.AreEqual( MockAssembly.Nodes - MockTestFixture.Nodes - 1, treeView.GetNodeCount( true ) );

			testsNamespaceSuite.Tests.Insert( 0, assembliesNamespaceSuite );
			ReassignTestIDs( suite );

			treeView.Reload( new TestNode( suite ) );
			CheckTree( treeView, suite, "after insert" );

			Assert.AreEqual( MockAssembly.Tests, suite.TestCount );
			Assert.AreEqual( MockAssembly.Nodes, treeView.GetNodeCount( true ) );
		}

		[Test]
		[ExpectedException( typeof(ArgumentException) )]
		public void ReloadTreeWithWrongTest()
		{
			treeView.Load( new TestNode( suite ) );

			TestSuite suite2 = new TestSuite( "WrongSuite" );
			treeView.Reload( new TestNode( suite2 ) );
		}

		[Test]
		public void ProcessChecks()
		{
			treeView.Load( new TestNode( suite ) );

			Assert.AreEqual(0, treeView.CheckedTests.Length);
			Assert.IsFalse(Checked(treeView.Nodes));

			treeView.Nodes[0].Checked = true;

			treeView.Nodes[0].Nodes[0].Checked = true;

			Assert.AreEqual(2, treeView.CheckedTests.Length);
			Assert.AreEqual(1, treeView.SelectedTests.Length);

			Assert.IsTrue(Checked(treeView.Nodes));

			treeView.ClearCheckedNodes();

			Assert.AreEqual(0, treeView.CheckedTests.Length);
			Assert.IsFalse(Checked(treeView.Nodes));
		}
// TODO: Unused Tests
//		[Test]
//		public void CheckCategory() 
//		{
//			treeView.Load(suite);
//
//			Assert.AreEqual(0, treeView.CheckedTests.Length);
//
//			CheckCategoryVisitor visitor = new CheckCategoryVisitor("MockCategory");
//			treeView.Accept(visitor);
//
//			Assert.AreEqual(2, treeView.CheckedTests.Length);
//		}
//
//		[Test]
//		public void UnCheckCategory() 
//		{
//			treeView.Load(suite);
//
//			Assert.AreEqual(0, treeView.CheckedTests.Length);
//
//			CheckCategoryVisitor visitor = new CheckCategoryVisitor("MockCategory");
//			treeView.Accept(visitor);
//
//			Assert.AreEqual(2, treeView.CheckedTests.Length);
//
//			UnCheckCategoryVisitor unvisitor = new UnCheckCategoryVisitor("MockCategory");
//			treeView.Accept(unvisitor);
//
//			Assert.AreEqual(0, treeView.CheckedTests.Length);
//		}

		private bool Checked(TreeNodeCollection nodes) 
		{
			bool result = false;

			foreach (TreeNode node in nodes) 
			{
				result |= node.Checked;
				if (node.Nodes != null)
					result |= Checked(node.Nodes);
			}

			return result;
		}

		// Reload re-assigns the test IDs, so we do that here
		private void ReassignTestIDs( Test test )
		{
			test.TestName.TestID = new TestID();

			if ( test.IsSuite )
				foreach( Test child in test.Tests )
					ReassignTestIDs( child );
		}

		private void CheckTree( TestSuiteTreeView treeView, Test suite, string msg )
		{
			CheckThatTreeMatchesTests( treeView, suite, "Tree out of order " + msg );
			CheckTreeMap( treeView, suite, "Map incorrect " + msg );
		}

		private void CheckThatTreeMatchesTests( TestSuiteTreeView treeView, Test suite, string msg )
		{
			CheckThatNodeMatchesTest( (TestSuiteTreeNode)treeView.Nodes[0], suite, msg );
		}

		private void CheckThatNodeMatchesTest( TestSuiteTreeNode node, Test test, string msg )
		{
			Assert.AreEqual( test.TestName, node.Test.TestName );

			if ( test.IsSuite )
			{
				Assert.AreEqual( test.Tests.Count, node.Nodes.Count, "{0}: Incorrect count for {1}", msg, test.TestName.FullName );

				for( int index = 0; index < test.Tests.Count; index++ )
				{
					CheckThatNodeMatchesTest( (TestSuiteTreeNode)node.Nodes[index], (Test)test.Tests[index], msg );
				}
			}
		}

		private void CheckTreeMap( TestSuiteTreeView treeView, Test test, string msg )
		{
			TestSuiteTreeNode node = treeView[test.TestName.UniqueName];
			Assert.IsNotNull( node, "{0}: {1} not in map", msg, test.TestName.UniqueName );
			Assert.AreEqual( test.TestName, treeView[test.TestName.UniqueName].Test.TestName, msg );

			if ( test.IsSuite )
				foreach( Test child in test.Tests )
					CheckTreeMap( treeView, child, msg );
		}
	}
}
