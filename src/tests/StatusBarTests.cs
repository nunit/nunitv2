namespace NUnit.Tests
{
	using System;
	using NUnit.Framework;
	using NUnit.Core;
	using NUnit.UiKit;

	/// <summary>
	/// Summary description for StatusBarTests.
	/// </summary>
	[TestFixture]
	public class StatusBarTests
	{
		private StatusBar statusBar;
		private MockUIEventSource mockEvents;
		private string testsDll = "mock-assembly.dll";
		TestSuite suite;

		[SetUp]
		public void Setup()
		{
			statusBar = new StatusBar();
			mockEvents = new MockUIEventSource();

			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build( testsDll );
		}

		[Test]
		public void TestConstruction()
		{
			Assertion.AssertEquals( "Status", statusBar.Panels[0].Text );
			Assertion.AssertEquals( "Test Cases : 0", statusBar.Panels[1].Text );
			Assertion.AssertEquals( "Tests Run : 0", statusBar.Panels[2].Text );
			Assertion.AssertEquals( "Failures : 0", statusBar.Panels[3].Text );
			Assertion.AssertEquals( "Time : 0", statusBar.Panels[4].Text );
		}

		[Test]
		public void TestInitialization()
		{
			statusBar.Initialize( 50 );
			Assertion.AssertEquals( "Status", statusBar.Panels[0].Text );
			Assertion.AssertEquals( "Test Cases : 50", statusBar.Panels[1].Text );
			Assertion.AssertEquals( "Tests Run : 0", statusBar.Panels[2].Text );
			Assertion.AssertEquals( "Failures : 0", statusBar.Panels[3].Text );
			Assertion.AssertEquals( "Time : 0", statusBar.Panels[4].Text );
		}

		[Test]
		public void TestFinalDisplay()
		{
			Assertion.AssertEquals( false, statusBar.DisplayTestProgress );
			statusBar.InitializeEvents( mockEvents );

			mockEvents.SimulateTestRun( suite );
			Assertion.AssertEquals( "Completed", statusBar.Panels[0].Text );
			Assertion.AssertEquals( "Test Cases : 7", statusBar.Panels[1].Text );
			Assertion.AssertEquals( "Tests Run : 5", statusBar.Panels[2].Text );
			Assertion.AssertEquals( "Failures : 0", statusBar.Panels[3].Text );
			Assertion.AssertEquals( "Time : 0", statusBar.Panels[4].Text );
		}

		[Test]
		public void TestProgressDisplay()
		{
			statusBar.DisplayTestProgress = true;
			statusBar.InitializeEvents( mockEvents );

			mockEvents.SimulateTestRun( suite );
			Assertion.AssertEquals( "Completed", statusBar.Panels[0].Text );
			Assertion.AssertEquals( "Test Cases : 7", statusBar.Panels[1].Text );
			Assertion.AssertEquals( "Tests Run : 5", statusBar.Panels[2].Text );
			Assertion.AssertEquals( "Failures : 0", statusBar.Panels[3].Text );
			Assertion.AssertEquals( "Time : 0", statusBar.Panels[4].Text );
		}
	}
}
