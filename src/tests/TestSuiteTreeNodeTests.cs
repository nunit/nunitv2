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
	using NUnit.Core;
	using NUnit.Framework;
	using NUnit.Util;
	using NUnit.UiKit;
	using NUnit.Tests.Assemblies;

	/// <summary>
	/// Summary description for TestSuiteTreeNodeTests.
	/// </summary>
	[TestFixture]
	public class TestSuiteTreeNodeTests
	{
		TestSuite testSuite;
		TestSuite testFixture;
		NUnit.Core.TestCase testCase;

		UITestNode suiteInfo;
		UITestNode fixtureInfo;
		UITestNode testCaseInfo;

		[SetUp]
		public void SetUp()
		{
			MockTestFixture mock = new MockTestFixture();
			testSuite = new TestSuite("MyTestSuite");
			testSuite.Add( mock );
			suiteInfo = new UITestNode( testSuite );

			testFixture = (TestSuite)testSuite.Tests[0];
			fixtureInfo = new UITestNode( testFixture );

			testCase = (NUnit.Core.TestCase)testFixture.Tests[0];
			testCaseInfo = new UITestNode( testCase );
		}

		[Test]
		public void ConstructFromTestInfo()
		{
			TestSuiteTreeNode node;
			
			node = new TestSuiteTreeNode( suiteInfo );
			Assert.AreEqual( "MyTestSuite", node.Text );

			node = new TestSuiteTreeNode( fixtureInfo );
			Assert.AreEqual( "MockTestFixture", node.Text );

			node = new TestSuiteTreeNode( testCaseInfo );
			Assert.AreEqual( "MockTest1", node.Text );
		}

//		[Test]
//		public void ConstructFromTestResultInfo()
//		{
//			TestSuiteTreeNode node;
//
//			node = new TestSuiteTreeNode( new TestResultInfo( testSuite, "Result 1" ) );
//			Assert.Equals( "MyTestSuite", node.Text );
//			Assert.Equals( "MyTestSuite", node.Test.Name );
//
//			node = new TestSuiteTreeNode( new TestResultInfo( testFixture, "Result 2" ) );
//			Assert.Equals( "MockTestFixture", node.Text );
//			Assert.Equals( "MockTestFixture", node.Test.Name );
//
//			node = new TestSuiteTreeNode( new TestResultInfo( testCase ) );
//			Assert.Equals( "MockTest1", node.Text );
//			Assert.Equals( "MockTest1", node.Test.Name );
//		}

		[Test]
		public void UpdateTest()
		{
			TestSuiteTreeNode node;
			
			node = new TestSuiteTreeNode( suiteInfo );
			UITestNode suiteInfo2 = new UITestNode( new TestSuite( "MyTestSuite" ) );

			node.UpdateTest( suiteInfo2 );
			Assert.AreEqual( "MyTestSuite", node.Test.FullName );
			Assert.AreEqual( 0, node.Test.CountTestCases() );

			node.UpdateTest( suiteInfo );
			Assert.AreEqual( "MyTestSuite", node.Test.FullName );
			Assert.AreEqual( MockTestFixture.Tests, node.Test.CountTestCases() );
		}

		[Test]
		[ExpectedException( typeof(ArgumentException) )]
		public void UpdateUsingWrongTest()
		{
			TestSuiteTreeNode node = new TestSuiteTreeNode( suiteInfo );
			UITestNode suiteInfo2 = new UITestNode( new TestSuite( "NotMyTestSuite" ) );
			node.UpdateTest( suiteInfo2 );
		}

		[Test]
		public void SetResult()
		{
			TestSuiteTreeNode node = new TestSuiteTreeNode( testCaseInfo );
			TestCaseResult result = new TestCaseResult( testCase );

			node.SetResult( result );
			Assert.AreEqual( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", node.Result.Name );
			Assert.AreEqual( TestSuiteTreeNode.NotRunIndex, node.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNode.NotRunIndex, node.SelectedImageIndex );

			result.Success();
			node.SetResult( result );
			Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, node.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, node.SelectedImageIndex );

			result.Failure("message", "stacktrace");
			node.SetResult( result );
			Assert.AreEqual( TestSuiteTreeNode.FailureIndex, node.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNode.FailureIndex, node.SelectedImageIndex );
		}

		[Test]
		[ExpectedException( typeof(ArgumentException ))]
		public void SetResultForWrongTest()
		{
			TestSuiteTreeNode node = new TestSuiteTreeNode( suiteInfo );
			TestSuite suite2 = new TestSuite( "suite2" );
			TestSuiteResult result = new TestSuiteResult( suite2, "xxxxx" );
			node.SetResult( result );
		}

		[Test]
		public void ClearResult()
		{
			TestCaseResult result = new TestCaseResult( testCase );
			result.Failure("message", "stacktrace");

			TestSuiteTreeNode node = new TestSuiteTreeNode( result );
			Assert.AreEqual( TestSuiteTreeNode.FailureIndex, node.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNode.FailureIndex, node.SelectedImageIndex );

			node.ClearResult();
			Assert.AreEqual( null, node.Result );
			Assert.AreEqual( TestSuiteTreeNode.InitIndex, node.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNode.InitIndex, node.SelectedImageIndex );
		}
		
		[Test]
		public void ClearResults()
		{
			TestCaseResult testCaseResult = new TestCaseResult( testCase );
			testCaseResult.Success();
			TestSuiteResult testSuiteResult = new TestSuiteResult( testFixture, "MockTestFixture" );
			testSuiteResult.AddResult( testCaseResult );
			testSuiteResult.Executed = true;

			TestSuiteTreeNode node1 = new TestSuiteTreeNode( testSuiteResult );
			TestSuiteTreeNode node2 = new TestSuiteTreeNode( testCaseResult );
			node1.Nodes.Add( node2 );

			Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, node1.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, node1.SelectedImageIndex );
			Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, node2.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNode.SuccessIndex, node2.SelectedImageIndex );

			node1.ClearResults();

			Assert.AreEqual( TestSuiteTreeNode.InitIndex, node1.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNode.InitIndex, node1.SelectedImageIndex );
			Assert.AreEqual( TestSuiteTreeNode.InitIndex, node2.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNode.InitIndex, node2.SelectedImageIndex );
		}
	}
}
