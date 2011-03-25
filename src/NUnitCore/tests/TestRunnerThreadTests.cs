// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

#if NET_3_5
using System;
using NSubstitute;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Core.Filters;

namespace NUnit.Core.Tests
{
    [TestFixture, Platform("Net-3.5,Mono-3.5")]
	public class TestRunnerThreadTests
	{
		private TestRunner mockRunner;
		private TestRunnerThread runnerThread;
        private EventListener listener;

		[SetUp]
		public void CreateRunnerThread()
		{
            mockRunner = Substitute.For<TestRunner>();
			runnerThread = new TestRunnerThread( mockRunner );
            listener = NullListener.NULL;
		}

		[Test]
		public void RunTestSuite()
		{
			runnerThread.StartRun(listener, TestFilter.Empty);
			runnerThread.Wait();

            mockRunner.Received().Run(listener, TestFilter.Empty);
		}

        [Test]
        public void RunNamedTest()
        {
            runnerThread.StartRun(listener, new NameFilter(TestName.Parse("SomeTest")));
            runnerThread.Wait();

            mockRunner.Received().Run(listener, Arg.Any<NameFilter>());
        }

        [Test]
        public void RunMultipleTests()
        {
            NUnit.Core.Filters.NameFilter filter = new NUnit.Core.Filters.NameFilter();
            filter.Add(TestName.Parse("Test1"));
            filter.Add(TestName.Parse("Test2"));
            filter.Add(TestName.Parse("Test3"));

            runnerThread.StartRun(listener, filter);
            runnerThread.Wait();

            mockRunner.Received().Run(listener, filter);
        }
	}
}
#endif
