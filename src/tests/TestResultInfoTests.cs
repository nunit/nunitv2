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
