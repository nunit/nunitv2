using System;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests.Core
{
	[TestFixture]
	public class TestRunnerThreadTests
	{
		private MockTestRunner mockRunner;
		private TestRunnerThread runnerThread;

		[SetUp]
		public void CreateRunnerThread()
		{
			mockRunner = new MockTestRunner( "TestRunner" );
			runnerThread = new TestRunnerThread( (TestRunner)mockRunner.MockInstance );
			// Set Strict false to avoid faults on the worker thread
			mockRunner.Strict = false;
		}

		[Test]
		public void RunTestSuite()
		{
			mockRunner.Expect( "Run" );

			runnerThread.Run(NullListener.NULL);
			runnerThread.Wait();

			mockRunner.Verify();
		}

		[Test]
		public void RunNamedTest()
		{
			mockRunner.Expect( "Run" );

			runnerThread.Run(NullListener.NULL, "SomeTest" );
			runnerThread.Wait();

			mockRunner.Verify();
		}

		[Test]
		public void RunMultipleTests()
		{
			string[] args = new string[] { "Test1", "Test2", "Test3" };
			mockRunner.Expect( "Run" );

			runnerThread.Run(NullListener.NULL, args );
			runnerThread.Wait();

			mockRunner.Verify();
		}
	}
}
