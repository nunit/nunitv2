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
		private MockTestEventSource mockEvents;
		private string testsDll = "mock-assembly.dll";
		private TestSuite suite;
		int testCount;

		[SetUp]
		public void Setup()
		{
			progressBar = new ProgressBar();

			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build( testsDll );

			mockEvents = new MockTestEventSource( testsDll, suite );
		}

		[Test]
		public void TestProgressDisplay()
		{
			progressBar.Initialize( mockEvents );
			mockEvents.TestFinished += new TestEventHandler( OnTestFinished );

			testCount = 0;
			mockEvents.SimulateTestRun();
			
			Assert.Equals( 0, progressBar.Minimum );
			Assert.Equals( 7, progressBar.Maximum );
			Assert.Equals( 1, progressBar.Step );
			Assert.Equals( 7, progressBar.Value );
			Assert.Equals( Color.Yellow, progressBar.ForeColor );
		}

		private void OnTestFinished( object sender, TestEventArgs e )
		{
			++testCount;
			// Assumes delegates are called in order of adding
			Assert.Equals( testCount, progressBar.Value );
		}
	}
}
