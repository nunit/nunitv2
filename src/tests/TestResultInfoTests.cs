namespace NUnit.Tests
{
	using System;
	using NUnit.Core;
	using NUnit.Framework;
	using NUnit.Util;
	using NUnit.Tests.Assemblies;

	/// <summary>
	/// Summary description for TestResultInfoTests.
	/// </summary>
	[TestFixture]
	public class TestResultInfoTests
	{
		TestSuiteResult testSuiteResult;
		TestSuiteResult testFixtureResult;
		TestCaseResult failureResult;
		TestCaseResult successResult;
		TestCaseResult notRunResult;

		[SetUp]
		public void SetUp()
		{
			MockTestFixture mock = new MockTestFixture();
			TestSuite testSuite = new TestSuite("MyTestSuite");
			testSuite.Add( mock );
			testSuiteResult = new TestSuiteResult( testSuite, "Result 1" );

			TestSuite testFixture = (TestSuite)testSuite.Tests[0];
			testFixtureResult = new TestSuiteResult( testFixture, "Result 2" );

			NUnit.Core.TestCase failureCase = (NUnit.Core.TestCase)testFixture.Tests[0];
			failureResult = new TestCaseResult( failureCase );
			failureResult.Failure("error message", "stack trace");

			NUnit.Core.TestCase successCase = (NUnit.Core.TestCase)testFixture.Tests[1];
			successResult = new TestCaseResult( successCase );
			successResult.Success();

			NUnit.Core.TestCase notRunCase = (NUnit.Core.TestCase)testFixture.Tests[4];
			notRunResult = new TestCaseResult( notRunCase );
		}

		[Test]
		public void Construction()
		{
			TestResultInfo result;

			result = new TestResultInfo( testSuiteResult );
			Assertion.AssertEquals( "Result 1", result.Name );
			Assertion.AssertEquals( "MyTestSuite", result.Test.Name );
			Assertion.AssertEquals( "MyTestSuite", result.Test.FullName );
			Assertion.Assert( !result.Executed );

			result = new TestResultInfo( testFixtureResult );
			Assertion.AssertEquals( "Result 2", result.Name );
			Assertion.AssertEquals( "MockTestFixture", result.Test.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture", result.Test.FullName );
			Assertion.Assert( !result.Executed );

			result = new TestResultInfo( failureResult );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", result.Name );
			Assertion.AssertEquals( "MockTest1", result.Test.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", result.Test.FullName );
			Assertion.Assert( "Executed", result.Executed );
			Assertion.Assert( "IsFailure", result.IsFailure );
			Assertion.AssertEquals( "error message", result.Message );
			Assertion.AssertEquals( "stack trace", result.StackTrace );

			result = new TestResultInfo( successResult );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest2", result.Name );
			Assertion.AssertEquals( "MockTest2", result.Test.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest2", result.Test.FullName );
			Assertion.Assert( "Executed", result.Executed );
			Assertion.Assert( "IsSuccess", result.IsSuccess );

			result = new TestResultInfo( notRunResult );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest4", result.Name );
			Assertion.AssertEquals( "MockTest4", result.Test.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest4", result.Test.FullName );
			Assertion.Assert( "!Executed", !result.Executed );
			Assertion.AssertEquals( "ignoring this test method for now", result.Test.IgnoreReason );
		}

		[Test]
		public void Conversion()
		{
			TestResultInfo result;
			
			result = testSuiteResult;
			Assertion.AssertEquals( "Result 1", result.Name );
			Assertion.AssertEquals( "MyTestSuite", result.Test.Name );
			Assertion.AssertEquals( "MyTestSuite", result.Test.FullName );
			Assertion.Assert( !result.Executed );

			result = testFixtureResult;
			Assertion.AssertEquals( "Result 2", result.Name );
			Assertion.AssertEquals( "MockTestFixture", result.Test.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture", result.Test.FullName );
			Assertion.Assert( !result.Executed );

			result = failureResult;
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", result.Name );
			Assertion.AssertEquals( "MockTest1", result.Test.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", result.Test.FullName );
			Assertion.Assert( "Executed", result.Executed );
			Assertion.Assert( "IsFailure", result.IsFailure );
			Assertion.AssertEquals( "error message", result.Message );
			Assertion.AssertEquals( "stack trace", result.StackTrace );

			result = successResult;
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest2", result.Name );
			Assertion.AssertEquals( "MockTest2", result.Test.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest2", result.Test.FullName );
			Assertion.Assert( "Executed", result.Executed );
			Assertion.Assert( "IsSuccess", result.IsSuccess );

			result = notRunResult;
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest4", result.Name );
			Assertion.AssertEquals( "MockTest4", result.Test.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest4", result.Test.FullName );
			Assertion.Assert( "!Executed", !result.Executed );
			Assertion.AssertEquals( "ignoring this test method for now", result.Test.IgnoreReason );
		}
	}
}
