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

namespace NUnit.Tests
{
	using System;
	using NUnit.Core;
	using NUnit.Framework;
	using NUnit.Util;
	using NUnit.Tests.Assemblies;

	/// <summary>
	/// Summary description for TestNodeTests.
	/// </summary>
	[TestFixture]
	public class TestNodeTests
	{
		TestSuite testSuite;
		TestSuite testFixture;
		NUnit.Core.TestCase testCase;

		TestInfo suiteInfo;
		TestInfo fixtureInfo;
		TestInfo testCaseInfo;

		[SetUp]
		public void SetUp()
		{
			MockTestFixture mock = new MockTestFixture();
			testSuite = new TestSuite("MyTestSuite");
			testSuite.Add( mock );
			suiteInfo = new TestInfo( testSuite );

			testFixture = (TestSuite)testSuite.Tests[0];
			fixtureInfo = new TestInfo( testFixture );

			testCase = (NUnit.Core.TestCase)testFixture.Tests[0];
			testCaseInfo = new TestInfo( testCase );
		}

		[Test]
		public void ConstructFromTestInfo()
		{
			TestNode node;
			
			node = new TestNode( suiteInfo );
			Assertion.AssertEquals( "MyTestSuite", node.Text );

			node = new TestNode( fixtureInfo );
			Assertion.AssertEquals( "MockTestFixture", node.Text );

			node = new TestNode( testCaseInfo );
			Assertion.AssertEquals( "MockTest1", node.Text );
		}

		[Test]
		public void ConstructFromTestResultInfo()
		{
			TestNode node;

			node = new TestNode( new TestResultInfo( testSuite, "Result 1" ) );
			Assertion.AssertEquals( "MyTestSuite", node.Text );
			Assertion.AssertEquals( "MyTestSuite", node.Test.Name );

			node = new TestNode( new TestResultInfo( testFixture, "Result 2" ) );
			Assertion.AssertEquals( "MockTestFixture", node.Text );
			Assertion.AssertEquals( "MockTestFixture", node.Test.Name );

			node = new TestNode( new TestResultInfo( testCase ) );
			Assertion.AssertEquals( "MockTest1", node.Text );
			Assertion.AssertEquals( "MockTest1", node.Test.Name );
		}

		[Test]
		public void UpdateTest()
		{
			TestNode node;
			
			node = new TestNode( suiteInfo );
			TestInfo suiteInfo2 = new TestInfo( new TestSuite( "MyTestSuite" ) );

			node.UpdateTest( suiteInfo2 );
			Assertion.AssertEquals( "MyTestSuite", node.Test.FullName );
			Assertion.AssertEquals( 0, node.Test.CountTestCases );

			node.UpdateTest( suiteInfo );
			Assertion.AssertEquals( "MyTestSuite", node.Test.FullName );
			Assertion.AssertEquals( 5, node.Test.CountTestCases );
		}

		[Test]
		[ExpectedException( typeof(ArgumentException) )]
		public void UpdateUsingWrongTest()
		{
			TestNode node = new TestNode( suiteInfo );
			TestInfo suiteInfo2 = new TestInfo( new TestSuite( "NotMyTestSuite" ) );
			node.UpdateTest( suiteInfo2 );
		}

		[Test]
		public void SetResult()
		{
			TestNode node = new TestNode( testCaseInfo );
			TestCaseResult result = new TestCaseResult( testCase );

			node.SetResult( result );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", node.Result.Name );
			Assertion.AssertEquals( TestNode.NotRunIndex, node.ImageIndex );
			Assertion.AssertEquals( TestNode.NotRunIndex, node.SelectedImageIndex );

			result.Success();
			node.SetResult( result );
			Assertion.AssertEquals( TestNode.SuccessIndex, node.ImageIndex );
			Assertion.AssertEquals( TestNode.SuccessIndex, node.SelectedImageIndex );

			result.Failure("message", "stacktrace");
			node.SetResult( result );
			Assertion.AssertEquals( TestNode.FailureIndex, node.ImageIndex );
			Assertion.AssertEquals( TestNode.FailureIndex, node.SelectedImageIndex );
		}

		[Test]
		[ExpectedException( typeof(ArgumentException ))]
		public void SetResultForWrongTest()
		{
			TestNode node = new TestNode( suiteInfo );
			TestSuite suite2 = new TestSuite( "suite2" );
			TestSuiteResult result = new TestSuiteResult( suite2, "xxxxx" );
			node.SetResult( result );
		}

		[Test]
		public void ClearResult()
		{
			TestCaseResult result = new TestCaseResult( testCase );
			result.Failure("message", "stacktrace");

			TestNode node = new TestNode( result );
			Assertion.AssertEquals( TestNode.FailureIndex, node.ImageIndex );
			Assertion.AssertEquals( TestNode.FailureIndex, node.SelectedImageIndex );

			node.ClearResult();
			Assertion.AssertEquals( null, node.Result );
			Assertion.AssertEquals( TestNode.InitIndex, node.ImageIndex );
			Assertion.AssertEquals( TestNode.InitIndex, node.SelectedImageIndex );
		}
		
		[Test]
		public void ClearResults()
		{
			TestCaseResult testCaseResult = new TestCaseResult( testCase );
			testCaseResult.Success();
			TestSuiteResult testSuiteResult = new TestSuiteResult( testFixture, "MockTestFixture" );
			testSuiteResult.AddResult( testCaseResult );
			testSuiteResult.Executed = true;

			TestNode node1 = new TestNode( testSuiteResult );
			TestNode node2 = new TestNode( testCaseResult );
			node1.Nodes.Add( node2 );

			Assertion.AssertEquals( TestNode.SuccessIndex, node1.ImageIndex );
			Assertion.AssertEquals( TestNode.SuccessIndex, node1.SelectedImageIndex );
			Assertion.AssertEquals( TestNode.SuccessIndex, node2.ImageIndex );
			Assertion.AssertEquals( TestNode.SuccessIndex, node2.SelectedImageIndex );

			node1.ClearResults();

			Assertion.AssertEquals( TestNode.InitIndex, node1.ImageIndex );
			Assertion.AssertEquals( TestNode.InitIndex, node1.SelectedImageIndex );
			Assertion.AssertEquals( TestNode.InitIndex, node2.ImageIndex );
			Assertion.AssertEquals( TestNode.InitIndex, node2.SelectedImageIndex );
		}
	}
}
