using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core.Builders;

namespace NUnit.Core.Tests
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
			Type fixtureType = typeof(IgnoredTestCaseFixture);
			Test test = TestCaseBuilder.Make( fixtureType, "CallsIgnore" );
			TestSuite suite = TestFixtureBuilder.Make(fixtureType);
			suite.Add(test);
			TestResult result = test.Run( NullListener.NULL);
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
			//IgnoredTestSuiteFixture testFixture = new IgnoredTestSuiteFixture();
			TestSuite suite = new TestSuite("IgnoredTestFixture");
			suite.Add( TestFixtureBuilder.Make( typeof( IgnoredTestSuiteFixture ) ) );
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
			TestSuite testFixture = TestFixtureBuilder.Make( typeof( IgnoreInSetUpFixture ) );
			TestSuiteResult fixtureResult = (TestSuiteResult)testFixture.Run( NullListener.NULL);

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

		[Test]
		public void IgnoreWithUserMessage()
		{
			try
			{
				Assert.Ignore( "my message" );
			}
			catch( IgnoreException ex )
			{
				Assert.AreEqual( "my message", ex.Message );
			}
		}

		[Test]
		public void IgnoreWithUserMessage_OneArg()
		{
			try
			{
				Assert.Ignore( "The number is {0}", 5 );
			}
			catch( IgnoreException ex )
			{
				Assert.AreEqual( "The number is 5", ex.Message );
			}
		}

		[Test]
		public void IgnoreWithUserMessage_ThreeArgs()
		{
			try
			{
				Assert.Ignore( "The numbers are {0}, {1} and {2}", 1, 2, 3 );
			}
			catch( IgnoreException ex )
			{
				Assert.AreEqual( "The numbers are 1, 2 and 3", ex.Message );
			}
		}

		[Test]
		public void IgnoreWithUserMessage_ArrayOfArgs()
		{
			try
			{
			Assert.Ignore( "The numbers are {0}, {1} and {2}", new object[] { 1, 2, 3 } );
			}
			catch( IgnoreException ex )
			{
				Assert.AreEqual( "The numbers are 1, 2 and 3", ex.Message );
			}
		}
	}
}
