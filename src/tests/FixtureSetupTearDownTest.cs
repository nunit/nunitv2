using System;
using NUnit.Framework;
using NUnit.Core;


namespace NUnit.Tests
{
	[TestFixture]
	public class FixtureSetupTearDownTest
	{
		internal class SetUpAndTearDownFixture
		{
			internal int setUpCount = 0;
			internal int tearDownCount = 0;

			[TestFixtureSetUp]
			public virtual void Init()
			{
				setUpCount++;
			}

			[TestFixtureTearDown]
			public virtual void Destroy()
			{
				tearDownCount++;
			}

			[Test]
			public void Success(){}

			[Test]
			public void EvenMoreSuccess(){}
		}

		internal class InheritSetUpAndTearDown : SetUpAndTearDownFixture
		{
			[Test]
			public void AnotherTest(){}

			[Test]
			public void YetAnotherTest(){}
		}

		[Test]
		public void MakeSureSetUpAndTearDownAreCalled()
		{
			SetUpAndTearDownFixture testFixture = new SetUpAndTearDownFixture();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assertion.AssertEquals(1, testFixture.setUpCount);
			Assertion.AssertEquals(1, testFixture.tearDownCount);
		}

		[Test]
		public void CheckInheritedSetUpAndTearDownAreCalled()
		{
			InheritSetUpAndTearDown testFixture = new InheritSetUpAndTearDown();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assertion.AssertEquals(1, testFixture.setUpCount);
			Assertion.AssertEquals(1, testFixture.tearDownCount);
		}

		internal class DefineInheritSetUpAndTearDown : SetUpAndTearDownFixture
		{
			internal int derivedSetUpCount;
			internal int derivedTearDownCount;

			[TestFixtureSetUp]
			public override void Init()
			{
				derivedSetUpCount++;
			}

			[TestFixtureTearDown]
			public override void Destroy()
			{
				derivedTearDownCount++;
			}

			[Test]
			public void AnotherTest(){}

			[Test]
			public void YetAnotherTest(){}
		}

		[Test]
		public void CheckInheritedSetUpAndTearDownAreNotCalled()
		{
			DefineInheritSetUpAndTearDown testFixture = new DefineInheritSetUpAndTearDown();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assertion.AssertEquals(0, testFixture.setUpCount);
			Assertion.AssertEquals(0, testFixture.tearDownCount);
			Assertion.AssertEquals(1, testFixture.derivedSetUpCount);
			Assertion.AssertEquals(1, testFixture.derivedTearDownCount);
		}

		internal class MisbehavingFixtureSetUp 
		{
			[TestFixtureSetUp]
			public void willBlowUp() 
			{
				throw new Exception("This was thrown from fixture setup");
			}

			[Test]
			public void nothingToTest() 
			{
			}
		}

		[Test]
		public void HandleExceptionsInFixtureSetup() 
		{
			MisbehavingFixtureSetUp testFixture = new MisbehavingFixtureSetUp();
			TestSuite suite = new TestSuite("ASuite");
			suite.Add(testFixture);
			TestSuiteResult result = (TestSuiteResult) suite.Run(NullListener.NULL);
			Assertion.AssertEquals(1, result.Results.Count);
			TestResult failedResult = ((TestResult)result.Results[0]);
			Assertion.Assert("Suite should not have executed", !failedResult.Executed);
			String message = failedResult.Message.Substring(0, 108);
			Assertion.AssertEquals("System.Exception: This was thrown from fixture setup\r\n   at NUnit.Tests.MisbehavingFixtureSetUp.willBlowUp()", message);
			Assertion.AssertNotNull("StackTrace should not be null", failedResult.StackTrace);
		}

		internal class MisbehavingFixtureTearDown
		{
			[TestFixtureTearDown]
			public void willBlowUp() 
			{
				throw new Exception("This was thrown from fixture teardown");
			}

			[Test]
			public void nothingToTest() 
			{
			}
		}

		[Test]
		public void HandleExceptionsInFixtureTearDown() 
		{
			MisbehavingFixtureTearDown testFixture = new MisbehavingFixtureTearDown();
			TestSuite suite = new TestSuite("ASuite");
			suite.Add(testFixture);
			TestSuiteResult result = (TestSuiteResult) suite.Run(NullListener.NULL);
			Assertion.AssertEquals(1, result.Results.Count);
			TestResult failedResult = ((TestResult)result.Results[0]);
			Assertion.Assert("Suite should not have executed", !failedResult.Executed);
			String message = failedResult.Message.Substring(0, 114);
			Assertion.AssertEquals("System.Exception: This was thrown from fixture teardown\r\n   at NUnit.Tests.MisbehavingFixtureTearDown.willBlowUp()", message);
			Assertion.AssertNotNull("StackTrace should not be null", failedResult.StackTrace);
		}
	}
}
