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

			Assert.Equals(1, testFixture.setUpCount);
			Assert.Equals(1, testFixture.tearDownCount);
		}

		[Test]
		public void CheckInheritedSetUpAndTearDownAreCalled()
		{
			InheritSetUpAndTearDown testFixture = new InheritSetUpAndTearDown();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assert.Equals(1, testFixture.setUpCount);
			Assert.Equals(1, testFixture.tearDownCount);
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

			Assert.Equals(0, testFixture.setUpCount);
			Assert.Equals(0, testFixture.tearDownCount);
			Assert.Equals(1, testFixture.derivedSetUpCount);
			Assert.Equals(1, testFixture.derivedTearDownCount);
		}

		internal class MisbehavingFixtureSetUp 
		{
			public bool blowUp = true;

			[TestFixtureSetUp]
			public void willBlowUp() 
			{
				if (blowUp)
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

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.Equals(0, summ.ResultCount);
			Assert.Equals(1, summ.TestsNotRun);
			Assert.Equals(1, summ.SuitesNotRun);
			TestResult failedResult = ((TestResult)result.Results[0]);
			Assertion.Assert("Suite should not have executed", !failedResult.Executed);
			String message = failedResult.Message.Substring(0, 108);
			Assert.Equals("System.Exception: This was thrown from fixture setup\r\n   at NUnit.Tests.MisbehavingFixtureSetUp.willBlowUp()", message);
			Assertion.AssertNotNull("StackTrace should not be null", failedResult.StackTrace);
		}

		[Test]
		public void RerunFixtureAfterSetUpFixed() 
		{
			MisbehavingFixtureSetUp testFixture = new MisbehavingFixtureSetUp();
			TestSuite suite = new TestSuite("ASuite");
			suite.Add(testFixture);
			TestSuiteResult result = (TestSuiteResult) suite.Run(NullListener.NULL);

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.Equals(0, summ.ResultCount);
			Assert.Equals(1, summ.TestsNotRun);
			Assert.Equals(1, summ.SuitesNotRun);
			TestResult failedResult = ((TestResult)result.Results[0]);
			Assertion.Assert("Suite should not have executed", !failedResult.Executed);

			//fix the blow up in setup
			testFixture.blowUp = false;
			result = (TestSuiteResult) suite.Run(NullListener.NULL);

			// should have one suite and one fixture
			summ = new ResultSummarizer(result);
			Assert.Equals(1, summ.ResultCount);
			Assert.Equals(0, summ.TestsNotRun);
			Assert.Equals(0, summ.SuitesNotRun);
		}

		internal class MisbehavingFixtureTearDown
		{
			public bool blowUp = true;

			[TestFixtureTearDown]
			public void willBlowUp() 
			{
				if (blowUp)
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
			Assert.Equals(1, result.Results.Count);
			TestResult failedResult = ((TestResult)result.Results[0]);
			Assert.False("Suite should not have executed", failedResult.Executed);
			String message = failedResult.Message.Substring(0, 114);
			Assert.Equals("System.Exception: This was thrown from fixture teardown\r\n   at NUnit.Tests.MisbehavingFixtureTearDown.willBlowUp()", message);
			Assert.NotNull("StackTrace should not be null", failedResult.StackTrace);

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.Equals(1, summ.ResultCount);
			Assert.Equals(0, summ.TestsNotRun);
			Assert.Equals(1, summ.SuitesNotRun);
		}

		[Test]
		public void RerunFixtureAfterTearDownFixed() 
		{
			MisbehavingFixtureTearDown testFixture = new MisbehavingFixtureTearDown();
			TestSuite suite = new TestSuite("ASuite");
			suite.Add(testFixture);
			TestSuiteResult result = (TestSuiteResult) suite.Run(NullListener.NULL);
			Assert.Equals(1, result.Results.Count);

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.Equals(1, summ.ResultCount);
			Assert.Equals(0, summ.TestsNotRun);
			Assert.Equals(1, summ.SuitesNotRun);

			testFixture.blowUp = false;
			result = (TestSuiteResult) suite.Run(NullListener.NULL);
			summ = new ResultSummarizer(result);
			Assert.Equals(1, summ.ResultCount);
			Assert.Equals(0, summ.TestsNotRun);
			Assert.Equals(0, summ.SuitesNotRun);
		}
	}
}
