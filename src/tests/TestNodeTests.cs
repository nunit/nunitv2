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
		NUnit.Core.TestCase testCase1;

		public TestNodeTests()
		{
		}

		[SetUp]
		public void SetUp()
		{
			MockTestFixture mock = new MockTestFixture();
			testSuite = new TestSuite("MyTestSuite");
			testSuite.Add( mock );

			testFixture = (TestSuite)testSuite.Tests[0];

			testCase1 = (NUnit.Core.TestCase)testFixture.Tests[0];
		}

		[Test]
		public void ConstructFromTest()
		{
			TestNode node1 = new TestNode( testSuite );
			Assertion.AssertEquals( "MyTestSuite", node1.Text );

			TestNode node2 = new TestNode( testFixture );
			Assertion.AssertEquals( "MockTestFixture", node2.Text );

			TestNode node3 = new TestNode( testCase1 );
			Assertion.AssertEquals( "MockTest1", node3.Text );
		}

		[Test]
		public void ConstructFromResult()
		{
			TestSuiteResult result1 = new TestSuiteResult( testSuite, "xxxxx" );
			TestNode node1 = new TestNode( result1 );
			Assertion.AssertEquals( "MyTestSuite", node1.Text );
			Assertion.AssertEquals( "MyTestSuite", node1.Test.Name );

			TestSuiteResult result2 = new TestSuiteResult( testFixture, "xxxxx" );
			TestNode node2 = new TestNode( result2 );
			Assertion.AssertEquals( "MockTestFixture", node2.Text );
			Assertion.AssertEquals( "MockTestFixture", node2.Test.Name );

			TestCaseResult result3 = new TestCaseResult( testCase1 );
			TestNode node3 = new TestNode( result3 );
			Assertion.AssertEquals( "MockTest1", node3.Text );
			Assertion.AssertEquals( "MockTest1", node3.Test.Name );
		}

		[Test]
		[ExpectedException( typeof(ArgumentException ))]
		public void SetResultForWrongTest()
		{
			TestNode node = new TestNode( (Test) testSuite );
			TestSuite suite2 = new TestSuite( "suite2" );
			TestSuiteResult result = new TestSuiteResult( suite2, "xxxxx" );
			node.SetResult( result );
		}

		[Test]
		public void SetResult()
		{
			TestNode node = new TestNode( testCase1 );
			TestCaseResult result = new TestCaseResult( testCase1 );
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
		public void ClearResult()
		{
			TestCaseResult result = new TestCaseResult( testCase1 );
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
			TestCaseResult testCaseResult = new TestCaseResult( testCase1 );
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
