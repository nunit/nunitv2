using System;
using Nunit.Framework;
using Nunit.Core;

namespace Nunit.Tests
{
	/// <summary>
	/// Summary description for TestFixtureExtension.
	/// </summary>
	/// 
	[TestFixture]
	public class TestFixtureExtension
	{
		private TestSuite suite;

		[TestFixture]
		private abstract class BaseTestFixture : TestFixture
		{
			internal bool baseSetup = false;
			internal bool baseTeardown = false;

			protected override void SetUp()
			{ baseSetup = true; }

			protected override void TearDown()
			{ baseTeardown = true; }
		}

		private class DerivedTestFixture : BaseTestFixture
		{
			[Test]
			public void Success()
			{
				Assert(true);
			}
		}

		private class SetUpDerivedTestFixture : BaseTestFixture
		{
			[SetUp]
			public void Init()
			{
				base.SetUp();
			}

			[Test]
			public void Success()
			{
				Assert(true);
			}
		}

		[SetUp] public void LoadFixture()
		{
			string testsDll = "Nunit.Tests.dll";
			suite = TestSuiteBuilder.Build("Nunit.Tests.TestFixtureExtension+DerivedTestFixture", testsDll);
		}

		[Test] 
		public void CheckMultipleSetUp()
		{
			SetUpDerivedTestFixture testFixture = new SetUpDerivedTestFixture();
			TestSuite suite = new TestSuite("SetUpDerivedTestFixture");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assertion.AssertEquals(true, testFixture.baseSetup);		}

		[Test]
		public void DerivedTest()
		{
			Assertion.AssertNotNull(suite);

			TestSuite fixture = (TestSuite)suite.Tests[0];
			Assertion.AssertNotNull(fixture);

			TestResult result = fixture.Run(NullListener.NULL);
			Assertion.Assert(result.IsSuccess);
		}

		[Test]
		public void InheritSetup()
		{
			DerivedTestFixture testFixture = new DerivedTestFixture();
			TestSuite suite = new TestSuite("DerivedTestFixtureSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assertion.AssertEquals(true, testFixture.baseSetup);
		}

		[Test]
		public void InheritTearDown()
		{
			DerivedTestFixture testFixture = new DerivedTestFixture();
			TestSuite suite = new TestSuite("DerivedTestFixtureSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assertion.AssertEquals(true, testFixture.baseTeardown);
		}
	}
}
