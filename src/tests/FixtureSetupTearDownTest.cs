#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using NUnit.Framework;
using NUnit.Core;


namespace NUnit.Tests.Core
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

			Assert.AreEqual(1, testFixture.setUpCount);
			Assert.AreEqual(1, testFixture.tearDownCount);
			Assert.AreEqual(2, suite.CountTestCases );
			Assert.AreEqual(2, ((TestSuite)suite.Tests[0]).Tests.Count );
		}

		[Test]
		public void CheckInheritedSetUpAndTearDownAreCalled()
		{
			InheritSetUpAndTearDown testFixture = new InheritSetUpAndTearDown();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assert.AreEqual(1, testFixture.setUpCount);
			Assert.AreEqual(1, testFixture.tearDownCount);
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

			Assert.AreEqual(0, testFixture.setUpCount);
			Assert.AreEqual(0, testFixture.tearDownCount);
			Assert.AreEqual(1, testFixture.derivedSetUpCount);
			Assert.AreEqual(1, testFixture.derivedTearDownCount);
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
			Assert.AreEqual(0, summ.ResultCount);
			Assert.AreEqual(1, summ.TestsNotRun);
			Assert.AreEqual(1, summ.SuitesNotRun);
			TestResult failedResult = ((TestResult)result.Results[0]);
			Assertion.Assert("Suite should not have executed", !failedResult.Executed);
			String message = failedResult.Message.Substring(0, 113);
			Assert.AreEqual("System.Exception: This was thrown from fixture setup\r\n   at NUnit.Tests.Core.MisbehavingFixtureSetUp.willBlowUp()", message);
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
			Assert.AreEqual(0, summ.ResultCount);
			Assert.AreEqual(1, summ.TestsNotRun);
			Assert.AreEqual(1, summ.SuitesNotRun);
			TestResult failedResult = ((TestResult)result.Results[0]);
			Assertion.Assert("Suite should not have executed", !failedResult.Executed);

			//fix the blow up in setup
			testFixture.blowUp = false;
			result = (TestSuiteResult) suite.Run(NullListener.NULL);

			// should have one suite and one fixture
			summ = new ResultSummarizer(result);
			Assert.AreEqual(1, summ.ResultCount);
			Assert.AreEqual(0, summ.TestsNotRun);
			Assert.AreEqual(0, summ.SuitesNotRun);
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
			Assert.AreEqual(1, result.Results.Count);
			TestResult failedResult = ((TestResult)result.Results[0]);
			Assert.IsFalse(failedResult.Executed, "Suite should not have executed");
			String message = failedResult.Message.Substring(0, 119);
			Assert.AreEqual("System.Exception: This was thrown from fixture teardown\r\n   at NUnit.Tests.Core.MisbehavingFixtureTearDown.willBlowUp()", message);
			Assert.IsNotNull(failedResult.StackTrace, "StackTrace should not be null");

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.AreEqual(1, summ.ResultCount);
			Assert.AreEqual(0, summ.TestsNotRun);
			Assert.AreEqual(1, summ.SuitesNotRun);
		}

		[Test]
		public void RerunFixtureAfterTearDownFixed() 
		{
			MisbehavingFixtureTearDown testFixture = new MisbehavingFixtureTearDown();
			TestSuite suite = new TestSuite("ASuite");
			suite.Add(testFixture);
			TestSuiteResult result = (TestSuiteResult) suite.Run(NullListener.NULL);
			Assert.AreEqual(1, result.Results.Count);

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.AreEqual(1, summ.ResultCount);
			Assert.AreEqual(0, summ.TestsNotRun);
			Assert.AreEqual(1, summ.SuitesNotRun);

			testFixture.blowUp = false;
			result = (TestSuiteResult) suite.Run(NullListener.NULL);
			summ = new ResultSummarizer(result);
			Assert.AreEqual(1, summ.ResultCount);
			Assert.AreEqual(0, summ.TestsNotRun);
			Assert.AreEqual(0, summ.SuitesNotRun);
		}

		internal class SetUpAndTearDownWithTestInName
		{
			internal int setUpCount = 0;
			internal int tearDownCount = 0;

			[TestFixtureSetUp]
			public virtual void TestFixtureSetUp()
			{
				setUpCount++;
			}

			[TestFixtureTearDown]
			public virtual void TestFixtureTearDown()
			{
				tearDownCount++;
			}

			[Test]
			public void Success(){}

			[Test]
			public void EvenMoreSuccess(){}
		}

		[Test]
		public void HandleSetUpAndTearDownWithTestInName()
		{
			SetUpAndTearDownWithTestInName testFixture = new SetUpAndTearDownWithTestInName();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			suite.Run(NullListener.NULL);

			Assert.AreEqual(1, testFixture.setUpCount);
			Assert.AreEqual(1, testFixture.tearDownCount);
			Assert.AreEqual(2, suite.CountTestCases );
			Assert.AreEqual(2, ((TestSuite)suite.Tests[0]).Tests.Count );
		}

		[Test]
		public void RunningSingleMethodCallsSetUpAndTearDown()
		{
			SetUpAndTearDownFixture testFixture = new SetUpAndTearDownFixture();
			TestSuite suite = new TestSuite("SetUpAndTearDownSuite");
			suite.Add(testFixture);
			TestSuite fixtureSuite = (TestSuite)suite.Tests[0];
			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)fixtureSuite.Tests[0];
			
			// Normal Run does not run Fixture Setup and Teardown
			testCase.Run(NullListener.NULL);
			Assert.AreEqual(1, testFixture.setUpCount);
			Assert.AreEqual(1, testFixture.tearDownCount);
		}
	}
}
