// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework;
using NUnit.Framework.Extensions;
using NUnit.TestData;

namespace NUnit.Core.Extensions.Tests
{
	/// <summary>
	/// Tests for MaxTime decoration.
	/// </summary>
	[TestFixture]
	public class MaxTimeDecoratorTests
	{
		[Test,MaxTime(1000)]
		public void MaxTimeNotExceeded()
		{
		}

        // TODO: We need a way to simulate the clock reliably
        [Test]
        public void MaxTimeExceeded()
        {
            Test test = TestFixtureBuilder.BuildFrom(typeof (MaxTimeFixture));
            TestResult result = test.Run(NullListener.NULL);
            Assert.AreEqual(ResultState.Failure, result.ResultState);
            result = (TestResult)result.Results[0];
            StringAssert.IsMatch(@"Elapsed time of \d*ms exceeds maximum of 1ms", result.Message);
        }

        [Test, MaxTime(1000)]
        [ExpectedException(typeof(AssertionException), ExpectedMessage = "Intentional Failure")]
        public void FailureReport()
        {
            Assert.Fail("Intentional Failure");
        }

        [Test, MaxTime(1)]
        [ExpectedException(typeof(AssertionException), ExpectedMessage = "Intentional Failure")]
		public void FailureReportHasPriorityOverMaxTime()
		{
			System.Threading.Thread.Sleep(10);
			Assert.Fail("Intentional Failure");
		}

        [Test, MaxTime(1000), ExpectedException]
        public void ErrorReport()
        {
            throw new Exception();
        }

        [Test, MaxTime(1), ExpectedException]
        public void ErrorReportHasPriorityOverMaxTime()
        {
            System.Threading.Thread.Sleep((10));
            throw new Exception();
        }
    }
}
