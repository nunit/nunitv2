#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Tests.UiKit
{
	using System;
	using System.Reflection;
	using System.Windows.Forms;
	using NUnit.Framework;
	using NUnit.Core;
	using NUnit.Util;
	using NUnit.UiKit;
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
			Assert.IsNotNull(suite);
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
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);
			Assert.IsNotNull( treeView.Nodes[0] );
			Assert.AreEqual( 16, treeView.GetNodeCount( true ) );
			Assert.AreEqual( "mock-assembly.dll", treeView.Nodes[0].Text );	
			Assert.AreEqual( "NUnit", treeView.Nodes[0].Nodes[0].Text );
			Assert.AreEqual( "Tests", treeView.Nodes[0].Nodes[0].Nodes[0].Text );
		}

		[Test]
		public void BuildFromResult()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			TestResult result = suite.Run( new NullListener() );
			treeView.Load( result );
			Assert.AreEqual( 16, treeView.GetNodeCount( true ) );
			
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
					Assert.IsTrue( node.Result.Executed, "MockTestFixture: Executed=false" );

					TestSuiteTreeNode test1 = node.Nodes[0] as TestSuiteTreeNode;
					Assert.AreEqual( "MockTest1", test1.Text );
					Assert.IsNotNull( test1.Result, "No Result on TestCase" );
					Assert.IsTrue( test1.Result.Executed, "MockTest1: Executed=false" );
					Assert.IsTrue( test1.Result.IsSuccess, "MockTest1: IsSuccess=false");
					Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, test1.ImageIndex );

					TestSuiteTreeNode test4 = node.Nodes[3] as TestSuiteTreeNode;
					Assert.IsFalse( test4.Result.Executed, "MockTest4: Executed=true" );
					Assert.AreEqual( TestSuiteTreeNode.NotRunIndex, test4.ImageIndex );
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
			return (TestSuiteTreeNode)treeView.Nodes[0].Nodes[0].Nodes[0].Nodes[1].Nodes[0];
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
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);
			
			treeView.Clear();
			Assert.AreEqual( 0, treeView.Nodes.Count );
		}

		[Test]
		public void SetTestResult()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);

			TestSuiteResult result = new TestSuiteResult( fixture, "My test result" );
			treeView.SetTestResult( result );

			TestSuiteTreeNode fixtureNode = FixtureNode( treeView );
			Assert.IsNotNull(fixtureNode.Result,  "Result not set" );
			Assert.AreEqual( "My test result", fixtureNode.Result.Name );
			Assert.AreEqual( fixtureNode.Test.FullName, fixtureNode.Result.Test.FullName );
		}

		[Test]
		public void ReloadTree()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);

			Assert.AreEqual( 7, suite.CountTestCases );
			Assert.AreEqual( 16, treeView.GetNodeCount( true ) );
			
			TestSuite nunitNamespaceSuite = suite.Tests[0] as TestSuite;
			TestSuite testsNamespaceSuite = nunitNamespaceSuite.Tests[0] as TestSuite;
			TestSuite assembliesNamespaceSuite = testsNamespaceSuite.Tests[1] as TestSuite;
			testsNamespaceSuite.Tests.RemoveAt( 1 );
			treeView.Reload( suite );

			Assert.AreEqual( 2, suite.CountTestCases );
			Assert.AreEqual( 9, treeView.GetNodeCount( true ) );

			testsNamespaceSuite.Tests.Insert( 1, assembliesNamespaceSuite );
			treeView.Reload( suite );

			Assert.AreEqual( 7, suite.CountTestCases );
			Assert.AreEqual( 16, treeView.GetNodeCount( true ) );
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
