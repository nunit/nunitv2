using System;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests.Assertions
{
	/// <summary>
	/// Tests of IgnoreException and Assert.Ignore
	/// </summary>
	[TestFixture]
	public class IgnoreFixture
	{
		[Test]
		public void IgnoreThrowsIgnoreException()
		{
			//Note that we can't use ExpectedException here because
			//Assert.Ignore takes precedence and the test is ignored.
			try
			{
				Assert.Ignore("I threw this!");
			}
			catch(IgnoreException ex)
			{
				Assert.AreEqual("I threw this!", ex.Message);
			}
		}

		[Test]
		public void IgnoreWorksForTestCase()
		{
			IgnoredTestCaseFixture fixture = new IgnoredTestCaseFixture();
			Test test = TestCaseBuilder.Make( fixture, "CallsIgnore" );
			TestResult result = test.Run( NullListener.NULL, null );
			Assert.IsFalse( result.Executed, "TestCase should not run" );
			Assert.AreEqual( "Ignore me", result.Message );
		}

		[TestFixture]
		internal class IgnoredTestCaseFixture
		{
			[Test]
			public void CallsIgnore()
			{
				Assert.Ignore("Ignore me");
			}
		}

		[Test]
		public void IgnoreWorksForTestSuite()
		{
			IgnoredTestSuiteFixture testFixture = new IgnoredTestSuiteFixture();
			TestSuite suite = new TestSuite("IgnoredTestFixture");
			suite.Add(testFixture);
			TestSuiteResult result = (TestSuiteResult)suite.Run( NullListener.NULL);

			TestSuiteResult fixtureResult = (TestSuiteResult)result.Results[0];
			Assert.IsFalse( fixtureResult.Executed, "Fixture should not have been executed" );
			
			foreach( TestResult testResult in fixtureResult.Results )
				Assert.IsFalse( testResult.Executed, "Test case should not have been executed" );
		}

		[TestFixture]
		internal class IgnoredTestSuiteFixture
		{
			[TestFixtureSetUp]
			public void FixtureSetUp()
			{
				Assert.Ignore("Ignore this fixture");
			}

			[Test]
			public void ATest()
			{
			}

			[Test]
			public void AnotherTest()
			{
			}
		}

		[Test]
		public void IgnoreWorksFromSetUp()
		{
			IgnoreInSetUpFixture testFixture = new IgnoreInSetUpFixture();
			TestSuite suite = new TestSuite("IgnoredTestFixture");
			suite.Add(testFixture);
			TestSuiteResult result = (TestSuiteResult)suite.Run( NullListener.NULL);

			TestSuiteResult fixtureResult = (TestSuiteResult)result.Results[0];
			Assert.IsTrue( fixtureResult.Executed, "Fixture should have been executed" );
			
			foreach( TestResult testResult in fixtureResult.Results )
				Assert.IsFalse( testResult.Executed, "Test case should not have been executed" );
		}

		[TestFixture]
		internal class IgnoreInSetUpFixture
		{
			[SetUp]
			public void SetUp()
			{
				Assert.Ignore( "Ignore this test" );
			}

			[Test]
			public void Test1()
			{
			}

			[Test]
			public void Test2()
			{
			}
		}
	}
}
