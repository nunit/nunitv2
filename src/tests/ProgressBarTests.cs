namespace NUnit.Tests
{
	using System;
	using System.Drawing;
	using NUnit.Framework;
	using NUnit.Core;
	using NUnit.UiKit;

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

		[SetUp]
		public void Setup()
		{
			progressBar = new ProgressBar();
			mockEvents = new MockUIEventSource();

			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build( testsDll );
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
			mockEvents.SimulateTestRun( suite );
			Assertion.AssertEquals( 0, progressBar.Minimum );
			Assertion.AssertEquals( 7, progressBar.Maximum );
			Assertion.AssertEquals( 1, progressBar.Step );
			Assertion.AssertEquals( 7, progressBar.Value );
			Assertion.AssertEquals( Color.Yellow, progressBar.ForeColor );
		}
	}
}
