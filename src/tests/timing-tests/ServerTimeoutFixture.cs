using System;
using System.IO;
using System.Threading;
using NUnit.Core;
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.Tests.TimingTests
{
	[TestFixture]
	public class ServerTimeoutFixture
	{
		private TestDomain domain; 
		private Test test;

		// Test using timeout greater than default of five minutes
		private readonly TimeSpan timeout = TimeSpan.FromMinutes( 6 );

		[SetUp]
		public void MakeAppDomain()
		{
			TextWriter outStream = new ConsoleWriter(Console.Out);
			TextWriter errorStream = new ConsoleWriter(Console.Error);
			domain = new TestDomain( outStream, errorStream );
			test = domain.Load("mock-assembly.dll");
		}

		[TearDown]
		public void UnloadTestDomain()
		{
			domain.Unload();
			domain = null;
		}
			
		[Test]
		public void ServerTimeoutTest()
		{
			// Delay after loading the test
			Thread.Sleep( timeout );

			// Copy all the tests from the remote domain
			// to verify that Test object is connected.
			UITestNode node = new UITestNode( test, true );
			
			// Run the tests, which also verifies that
			// RemoteTestRunner has not been disconnected
			TestResult result = domain.Run( NullListener.NULL );

			// Delay again to let the results "ripen"
			Thread.Sleep( timeout );

			// Visit the results of the test after another delay
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assert.AreEqual(5, summarizer.ResultCount);
			Assert.AreEqual(2, summarizer.TestsNotRun);

			// Make sure we can still access the tests
			// using the Test property of the result
			node = new UITestNode( result.Test, true );
		}
	}
}
