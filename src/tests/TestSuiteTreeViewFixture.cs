
namespace NUnit.Tests
{
	using System;
	using System.Reflection;
	using NUnit.Framework;
	using NUnit.Core;
	using NUnit.Util;

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
		}
	}
}
