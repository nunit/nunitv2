namespace NUnit.Tests
{
	using System;
	using System.Drawing;
	using NUnit.Framework;
	using NUnit.Core;
	using NUnit.UiKit;
	using NUnit.Util;

	/// <summary>
	/// Summary description for ProgressBarTests.
	/// </summary>
	[TestFixture]
	public class ProgressBarTests
	{
		private ProgressBar progressBar;
		private MockUIEventSource mockEvents;
		private string testsDll = "mock-assembly.dll";
		private TestSuite suite;
		int testCount;

		[SetUp]
		public void Setup()
		{
			progressBar = new ProgressBar();

			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build( testsDll );

			mockEvents = new MockUIEventSource( testsDll, suite );
		}

		[Test]
		public void TestInitialization()
		{
			progressBar.Initialize( suite );

			Assertion.AssertEquals( 0, progressBar.Minimum );
			Assertion.AssertEquals( 7, progressBar.Maximum );
			Assertion.AssertEquals( 1, progressBar.Step );
			Assertion.AssertEquals( 0, progressBar.Value );
			Assertion.AssertEquals( Color.Lime, progressBar.ForeColor );
		}

		[Test]
		public void TestProgressDisplay()
		{
			progressBar.InitializeEvents( mockEvents );
			mockEvents.TestFinishedEvent += new TestEventHandler( OnTestFinished );

			testCount = 0;
			mockEvents.SimulateTestRun();
			
			Assertion.AssertEquals( 0, progressBar.Minimum );
			Assertion.AssertEquals( 7, progressBar.Maximum );
			Assertion.AssertEquals( 1, progressBar.Step );
			Assertion.AssertEquals( 7, progressBar.Value );
			Assertion.AssertEquals( Color.Yellow, progressBar.ForeColor );
		}

		private void OnTestFinished( object sender, TestEventArgs e )
		{
			++testCount;
			// Assumes delegates are called in order of adding
			Assertion.AssertEquals( testCount, progressBar.Value );
		}
	}
}
