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
using NUnit.Core.Builders;
using NUnit.Util;
using NUnit.TestUtilities;
using NUnit.TestData.FixtureSetUpTearDown;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class FixtureSetupTearDownTest
	{
		private TestSuiteResult RunTestOnFixture( object fixture )
		{
			TestSuite suite = TestBuilder.MakeFixture( fixture.GetType() );
			suite.Fixture = fixture;
			return (TestSuiteResult)suite.Run( NullListener.NULL );
		}

		[Test]
		public void MakeSureSetUpAndTearDownAreCalled()
		{
			SetUpAndTearDownFixture fixture = new SetUpAndTearDownFixture();
			RunTestOnFixture( fixture );

			Assert.AreEqual(1, fixture.setUpCount);
			Assert.AreEqual(1, fixture.tearDownCount);
		}

		[Test]
		public void CheckInheritedSetUpAndTearDownAreCalled()
		{
			InheritSetUpAndTearDown fixture = new InheritSetUpAndTearDown();
			RunTestOnFixture( fixture );

			Assert.AreEqual(1, fixture.setUpCount);
			Assert.AreEqual(1, fixture.tearDownCount);
		}

		[Test]
		public void CheckInheritedSetUpAndTearDownAreNotCalled()
		{
			DefineInheritSetUpAndTearDown fixture = new DefineInheritSetUpAndTearDown();
			RunTestOnFixture( fixture );

			Assert.AreEqual(0, fixture.setUpCount);
			Assert.AreEqual(0, fixture.tearDownCount);
			Assert.AreEqual(1, fixture.derivedSetUpCount);
			Assert.AreEqual(1, fixture.derivedTearDownCount);
		}

		[Test]
		public void HandleErrorInFixtureSetup() 
		{
			MisbehavingFixture fixture = new MisbehavingFixture();
			fixture.blowUpInSetUp = true;
			TestSuiteResult result = (TestSuiteResult)RunTestOnFixture( fixture );

			Assert.AreEqual( 1, fixture.setUpCount, "setUpCount" );
			Assert.AreEqual( 0, fixture.tearDownCount, "tearDownCOunt" );

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.AreEqual(1, summ.ResultCount);
			Assert.AreEqual(0, summ.TestsNotRun);
			Assert.AreEqual(0, summ.SuitesNotRun);
			
			Assert.IsTrue(result.Executed, "Suite should have executed");
			Assert.IsTrue(result.IsFailure, "Suite should have failed");
			Assert.AreEqual("System.Exception : This was thrown from fixture setup", result.Message, "TestSuite Message");
			Assert.IsNotNull(result.StackTrace, "TestSuite StackTrace should not be null");

			TestResult testResult = ((TestResult)result.Results[0]);
			Assert.IsTrue(testResult.Executed, "Testcase should have executed");
			Assert.AreEqual("TestFixtureSetUp Failed", testResult.Message, "TestSuite Message");
			//			Assert.AreEqual("This was thrown from fixture setup", testResult.Message, "TestCase Message" );
			Assert.AreEqual(testResult.StackTrace, testResult.StackTrace, "TestCase stackTrace should match TestSuite stackTrace" );
		}

		[Test]
		public void RerunFixtureAfterSetUpFixed() 
		{
			MisbehavingFixture fixture = new MisbehavingFixture();
			fixture.blowUpInSetUp = true;
			TestSuiteResult result = RunTestOnFixture( fixture );

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.AreEqual(1, summ.ResultCount);
			Assert.AreEqual(0, summ.TestsNotRun);
			Assert.AreEqual(0, summ.SuitesNotRun);
			Assert.IsTrue(result.Executed, "Suite should have executed");

			//fix the blow up in setup
			fixture.Reinitialize();
			result = RunTestOnFixture( fixture );

			Assert.AreEqual( 1, fixture.setUpCount, "setUpCount" );
			Assert.AreEqual( 1, fixture.tearDownCount, "tearDownCOunt" );

			// should have one suite and one fixture
			summ = new ResultSummarizer(result);
			Assert.AreEqual(1, summ.ResultCount);
			Assert.AreEqual(0, summ.TestsNotRun);
			Assert.AreEqual(0, summ.SuitesNotRun);
		}

		[Test]
		public void HandleIgnoreInFixtureSetup() 
		{
			IgnoreInFixtureSetUp fixture = new IgnoreInFixtureSetUp();
			TestSuiteResult result = RunTestOnFixture( fixture );

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.AreEqual(0, summ.ResultCount);
			Assert.AreEqual(1, summ.TestsNotRun);
			Assert.AreEqual(1, summ.SuitesNotRun);
			Assert.IsFalse(result.Executed, "Suite should not have executed");
			Assert.AreEqual("TestFixtureSetUp called Ignore", result.Message);
			Assert.IsNotNull(result.StackTrace, "StackTrace should not be null");

			TestResult testResult = ((TestResult)result.Results[0]);
			Assert.IsFalse(testResult.Executed, "Testcase should not have executed");
			Assert.AreEqual("TestFixtureSetUp called Ignore", testResult.Message );
		}

		[Test]
		public void HandleErrorInFixtureTearDown() 
		{
			MisbehavingFixture fixture = new MisbehavingFixture();
			fixture.blowUpInTearDown = true;
			TestSuiteResult result = RunTestOnFixture( fixture );
			Assert.AreEqual(1, result.Results.Count);
			Assert.IsTrue(result.Executed, "Suite should have executed");
			Assert.IsTrue(result.IsFailure, "Suite should have failed" );

			Assert.AreEqual( 1, fixture.setUpCount, "setUpCount" );
			Assert.AreEqual( 1, fixture.tearDownCount, "tearDownCOunt" );

			Assert.AreEqual("This was thrown from fixture teardown", result.Message);
			Assert.IsNotNull(result.StackTrace, "StackTrace should not be null");

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.AreEqual(1, summ.ResultCount);
			Assert.AreEqual(0, summ.TestsNotRun);
			Assert.AreEqual(0, summ.SuitesNotRun);
		}

		[Test]
		public void HandleExceptionInFixtureConstructor()
		{
			TestSuite suite = TestBuilder.MakeFixture( typeof( ExceptionInConstructor ) );
			TestSuiteResult result = (TestSuiteResult)suite.Run( NullListener.NULL );

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.AreEqual(1, summ.ResultCount);
			Assert.AreEqual(0, summ.TestsNotRun);
			Assert.AreEqual(0, summ.SuitesNotRun);
			
			Assert.IsTrue(result.Executed, "Suite should have executed");
			Assert.IsTrue(result.IsFailure, "Suite should have failed");
			Assert.AreEqual("System.Exception : This was thrown in constructor", result.Message, "TestSuite Message");
			Assert.IsNotNull(result.StackTrace, "TestSuite StackTrace should not be null");

			TestResult testResult = ((TestResult)result.Results[0]);
			Assert.IsTrue(testResult.Executed, "Testcase should have executed");
			Assert.AreEqual("TestFixtureSetUp Failed", testResult.Message, "TestSuite Message");
			Assert.AreEqual(testResult.StackTrace, testResult.StackTrace, "TestCase stackTrace should match TestSuite stackTrace" );
		}

		[Test]
		public void RerunFixtureAfterTearDownFixed() 
		{
			MisbehavingFixture fixture = new MisbehavingFixture();
			fixture.blowUpInTearDown = true;
			TestSuiteResult result = RunTestOnFixture( fixture );
			Assert.AreEqual(1, result.Results.Count);

			// should have one suite and one fixture
			ResultSummarizer summ = new ResultSummarizer(result);
			Assert.AreEqual(1, summ.ResultCount);
			Assert.AreEqual(0, summ.TestsNotRun);
			Assert.AreEqual(0, summ.SuitesNotRun);

			fixture.Reinitialize();
			result = RunTestOnFixture( fixture );

			Assert.AreEqual( 1, fixture.setUpCount, "setUpCount" );
			Assert.AreEqual( 1, fixture.tearDownCount, "tearDownCOunt" );

			summ = new ResultSummarizer(result);
			Assert.AreEqual(1, summ.ResultCount);
			Assert.AreEqual(0, summ.TestsNotRun);
			Assert.AreEqual(0, summ.SuitesNotRun);
		}

		[Test]
		public void HandleSetUpAndTearDownWithTestInName()
		{
			SetUpAndTearDownWithTestInName fixture = new SetUpAndTearDownWithTestInName();
			RunTestOnFixture( fixture );

			Assert.AreEqual(1, fixture.setUpCount);
			Assert.AreEqual(1, fixture.tearDownCount);
		}

		[Test]
		public void RunningSingleMethodCallsSetUpAndTearDown()
		{
			SetUpAndTearDownFixture fixture = new SetUpAndTearDownFixture();
			TestSuite suite = TestBuilder.MakeFixture( fixture.GetType() );
			suite.Fixture = fixture;
			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)suite.Tests[0];
			
			suite.Run(NullListener.NULL, new Filters.NameFilter( testCase.TestName ) );

			Assert.AreEqual(1, fixture.setUpCount);
			Assert.AreEqual(1, fixture.tearDownCount);
		}

		[Test]
		public void IgnoredFixtureShouldNotCallFixtureSetUpOrTearDown()
		{
			IgnoredFixture fixture = new IgnoredFixture();
			TestSuite suite = new TestSuite("IgnoredFixtureSuite");
			TestSuite fixtureSuite = TestBuilder.MakeFixture( fixture.GetType() );
			suite.Fixture = fixture;
			NUnit.Core.TestCase testCase = (NUnit.Core.TestCase)fixtureSuite.Tests[0];
			suite.Add( fixtureSuite );
			
			fixtureSuite.Run(NullListener.NULL);
			Assert.IsFalse( fixture.setupCalled, "TestFixtureSetUp called running fixture" );
			Assert.IsFalse( fixture.teardownCalled, "TestFixtureTearDown called running fixture" );

			suite.Run(NullListener.NULL);
			Assert.IsFalse( fixture.setupCalled, "TestFixtureSetUp called running enclosing suite" );
			Assert.IsFalse( fixture.teardownCalled, "TestFixtureTearDown called running enclosing suite" );

			testCase.Run(NullListener.NULL);
			Assert.IsFalse( fixture.setupCalled, "TestFixtureSetUp called running a test case" );
			Assert.IsFalse( fixture.teardownCalled, "TestFixtureTearDown called running a test case" );
		}

		[Test]
		public void FixtureWithNoTestsShouldNotCallFixtureSetUpOrTearDown()
		{
			FixtureWithNoTests fixture = new FixtureWithNoTests();
			RunTestOnFixture( fixture );		
			Assert.IsFalse( fixture.setupCalled, "TestFixtureSetUp called running fixture" );
			Assert.IsFalse( fixture.teardownCalled, "TestFixtureTearDown called running fixture" );
		}

        [Test]
        public void DisposeCalledWhenFixtureImplementsIDisposable()
        {
            DisposableFixture fixture = new DisposableFixture();
            RunTestOnFixture(fixture);
            Assert.IsTrue(fixture.disposeCalled);
        }
	}
}
