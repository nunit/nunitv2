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
