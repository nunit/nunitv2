
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
		private Test suite;

		[SetUp]
		public void SetUp() 
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build(testsDll);
		}

		[Test]
		public void LoadSuite()
		{
			Assertion.AssertNotNull(suite);
		}

		[Test]
		public void BuildTreeView()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);
			Assertion.AssertNotNull(treeView.RootNode);

			Assertion.AssertEquals("mock-assembly.dll", treeView.Nodes[0].Text);	
			Assertion.AssertEquals("NUnit", treeView.Nodes[0].Nodes[0].Text);
			Assertion.AssertEquals("Tests", treeView.Nodes[0].Nodes[0].Nodes[0].Text);
		}

		[Test]
		public void MapLookup()
		{
			TestSuiteTreeView treeView = new TestSuiteTreeView();
			treeView.Load(suite);

			TestSuite suite2 = new TestSuite("My suite");
			suite2.Add( new MockTestFixture() );
			TestSuite fixture = (TestSuite)suite2.Tests[0];

			TestNode node = treeView[fixture];
			Assertion.AssertNotNull(node);

			Assertion.AssertEquals(5, node.Nodes.Count);
			Assertion.AssertEquals("MockTest1", node.Nodes[0].Text);
		}
	}
}
