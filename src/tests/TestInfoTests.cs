namespace NUnit.Tests
{
	using System;
	using NUnit.Core;
	using NUnit.Framework;
	using NUnit.Util;
	using NUnit.Tests.Assemblies;

	/// <summary>
	/// Summary description for TestInfoTests.
	/// </summary>
	[TestFixture]	
	public class TestInfoTests
	{
		TestSuite testSuite;
		TestSuite testFixture;
		NUnit.Core.TestCase testCase1;

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
		public void Construction()
		{
			TestInfo test1 = new TestInfo( testSuite );
			Assertion.AssertEquals( "MyTestSuite", test1.Name );
			Assertion.AssertEquals( "MyTestSuite", test1.FullName );
			Assertion.Assert( "ShouldRun", test1.ShouldRun );
			Assertion.Assert( "IsSuite", test1.IsSuite );
			Assertion.Assert( "!IsTestCase", !test1.IsTestCase );
			Assertion.Assert( "!IsFixture", !test1.IsFixture );
			Assertion.AssertEquals( 5, test1.CountTestCases );

			TestInfo test2 = new TestInfo( testFixture, true );
			Assertion.AssertEquals( "MockTestFixture", test2.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture", test2.FullName );
			Assertion.Assert( "ShouldRun", test2.ShouldRun );
			Assertion.Assert( "IsSuite", test2.IsSuite );
			Assertion.Assert( "!IsTestCase", !test2.IsTestCase );
			Assertion.Assert( "IsFixture", test2.IsFixture );
			Assertion.AssertEquals( 5, test2.CountTestCases );

			TestInfo test3 = new TestInfo( testCase1 );
			Assertion.AssertEquals( "MockTest1", test3.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", test3.FullName );
			Assertion.Assert( "ShouldRun", test3.ShouldRun );
			Assertion.Assert( "!IsSuite", !test3.IsSuite );
			Assertion.Assert( "IsTestCase", test3.IsTestCase );
			Assertion.Assert( "!IsFixture", !test3.IsFixture );
			Assertion.AssertEquals( 1, test3.CountTestCases );
		}

		[Test]
		public void PopulateTests()
		{
			TestInfo test1 = new TestInfo( testSuite );
			Assertion.Assert( "Default should be to not populate", !test1.Populated );
			test1.PopulateTests();
			Assertion.Assert( "Should be populated after call", test1.Populated );
			TestInfo test1Child = test1.Tests[0] as TestInfo;
			Assertion.Assert( "Child should be populated", test1Child.Populated );

			TestInfo test2 = new TestInfo( testSuite, true );
			Assertion.Assert( "Should be populated initialy", test2.Populated );
			TestInfo test2Child = test2.Tests[0] as TestInfo;
			Assertion.Assert( "Child should be populated", test2Child.Populated );

			TestInfo test3 = new TestInfo( testSuite );
			int count = test3.CountTestCases;
			Assertion.Assert( "CountTestCases should populate", test3.Populated );

			TestInfo test4 = new TestInfo( testSuite );
			TestInfo test4Child = test4.Tests[0] as TestInfo;
			Assertion.Assert( "Accessing Tests should populate", test4.Populated );

			TestInfo test5 = new TestInfo( testSuite );
			bool fixture = test5.IsFixture;
			Assertion.Assert( "IsFixture should populate", test5.Populated );
		}

		[Test]
		public void Conversion()
		{
			TestInfo test1 = testSuite;
			Assertion.AssertEquals( "MyTestSuite", test1.Name );
			Assertion.AssertEquals( "MyTestSuite", test1.FullName );
			Assertion.Assert( "ShouldRun", test1.ShouldRun );
			Assertion.Assert( "IsSuite", test1.IsSuite );
			Assertion.Assert( "!IsTestCase", !test1.IsTestCase );
			Assertion.Assert( "!IsFixture", !test1.IsFixture );
			Assertion.AssertEquals( 5, test1.CountTestCases );

			TestInfo test2 = testFixture;
			Assertion.AssertEquals( "MockTestFixture", test2.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture", test2.FullName );
			Assertion.Assert( "ShouldRun", test2.ShouldRun );
			Assertion.Assert( "IsSuite", test2.IsSuite );
			Assertion.Assert( "!IsTestCase", !test2.IsTestCase );
			Assertion.Assert( "IsFixture", test2.IsFixture );
			Assertion.AssertEquals( 5, test2.CountTestCases );

			TestInfo test3 = testCase1;
			Assertion.AssertEquals( "MockTest1", test3.Name );
			Assertion.AssertEquals( "NUnit.Tests.Assemblies.MockTestFixture.MockTest1", test3.FullName );
			Assertion.Assert( "ShouldRun", test3.ShouldRun );
			Assertion.Assert( "!IsSuite", !test3.IsSuite );
			Assertion.Assert( "IsTestCase", test3.IsTestCase );
			Assertion.Assert( "!IsFixture", !test3.IsFixture );
			Assertion.AssertEquals( 1, test3.CountTestCases );
		}
	}
}
