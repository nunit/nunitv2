// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.Assemblies;
using NUnit.TestUtilities;
	
namespace NUnit.Util.Tests
{
	/// <summary>
	/// Summary description for TestResultTests.
	/// </summary>
	[TestFixture]
	public class SummaryResultFixture
	{
		private TestResult result;

		[SetUp]
		public void CreateResult()
		{
			Test testFixture = TestFixtureBuilder.BuildFrom( typeof( MockTestFixture ) );
			result = testFixture.Run( NullListener.NULL );
		}

		[Test]
		public void SummaryMatchesResult()
		{
			ResultSummarizer summary = new ResultSummarizer( result );

			Assert.AreEqual(result.Name, summary.Name);
			Assert.AreEqual(result.Time, summary.Time);
			Assert.IsTrue(summary.Success, "Success");
			Assert.AreEqual(MockTestFixture.Tests - MockTestFixture.NotRun, summary.ResultCount, "ResultCount");
			Assert.AreEqual(0, summary.FailureCount, "FailureCount");
			Assert.AreEqual(MockTestFixture.Ignored, summary.TestsNotRun, "TestsNotRun");
		}

		[Test]
		public void SummaryMatchesResult_SimulatedFailure()
		{
			TestResult caseResult = TestFinder.Find( "MockTest2", result );
			caseResult.Failure( "Simulated Failure", null );

			ResultSummarizer summary = new ResultSummarizer( result );

			Assert.AreEqual(result.Name, summary.Name);
			Assert.AreEqual(result.Time, summary.Time);
			Assert.IsFalse(summary.Success, "Success");
			Assert.AreEqual(MockTestFixture.Tests - MockTestFixture.NotRun, summary.ResultCount, "ResultCount");
			Assert.AreEqual(1, summary.FailureCount, "FailureCount");
			Assert.AreEqual(MockTestFixture.Ignored, summary.TestsNotRun, "TestsNotRun");
		}
	}
}
