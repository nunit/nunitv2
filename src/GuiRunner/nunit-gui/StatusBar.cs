using System;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.Gui
{

	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class StatusBar : System.Windows.Forms.StatusBar
	{
		private StatusBarPanel statusPanel = new StatusBarPanel();
		private StatusBarPanel testCountPanel = new StatusBarPanel();
		private StatusBarPanel testsRunPanel = new StatusBarPanel();
		private StatusBarPanel failuresPanel = new StatusBarPanel();
		private StatusBarPanel timePanel = new StatusBarPanel();

		private int testCount = 0;
		private int testsRun = 0;
		private int failures = 0;
		private int time = 0;

		private bool displayProgress = false;

		// ToDo: replace with an interface
		private UIEvents uiEvents;

		public bool DisplayTestProgress
		{
			get { return displayProgress; }
			set { displayProgress = value; }
		}

		public StatusBar()
		{
			Panels.Add( statusPanel );
			Panels.Add( testCountPanel );
			Panels.Add( testsRunPanel );
			Panels.Add( failuresPanel );
			Panels.Add( timePanel );

			statusPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			statusPanel.Text = "Status";

			testCountPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			testsRunPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			failuresPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			timePanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;

			ShowPanels = true;
			DisplayPanels();
		}

		public void InitializeEvents( UIEvents uiEvents )
		{
			this.uiEvents = uiEvents;

			uiEvents.TestStartedEvent += new TestStartedHandler( OnTestStarted );
			uiEvents.TestFinishedEvent += new TestFinishedHandler( OnTestFinished );
			uiEvents.RunFinishedEvent += new RunFinishedHandler( OnRunFinished );
		}

		public void Initialize( int testCount )
		{
			this.statusPanel.Text = "Status";

			this.testCount = testCount;
			this.testsRun = 0;
			this.failures = 0;
			this.time = 0;

			DisplayPanels();
		}

		private void DisplayPanels()
		{
			DisplayTestCount();
			DisplayTestsRun();
			DisplayFailures();
			DisplayTime();
		}

		private void DisplayTestCount()
		{
			this.testCountPanel.Text = "Test Cases : " + testCount.ToString();
		}

		private void DisplayTestsRun()
		{
			this.testsRunPanel.Text = "Tests Run : " + testsRun.ToString();
		}

		private void DisplayFailures()
		{
			this.failuresPanel.Text = "Failures : " + failures.ToString();
		}

		private void DisplayTime()
		{
			this.timePanel.Text = "Time : " + time.ToString();
		}

		private void DisplayResults(TestResult results)
		{
			ResultSummarizer summarizer = new ResultSummarizer(results);

			int failureCount = summarizer.Failures;

			failuresPanel.Text = "Failures : " + failureCount.ToString();
			testsRunPanel.Text = "Tests Run : " + summarizer.ResultCount.ToString();
			timePanel.Text = "Time : " + summarizer.Time.ToString();
		}

		public void OnTestStarted( TestInfo testCase )
		{
			statusPanel.Text = "Running : " + testCase.Name;
		}

		public void OnRunFinished(TestResult result)
		{
			statusPanel.Text = "Completed";

			if ( !DisplayTestProgress )
				DisplayResults( result );
		}

		public void OnTestFinished( TestCaseResult result )
		{
			if ( DisplayTestProgress && result.Executed )
			{
				++testsRun;
				DisplayTestsRun();
				if ( result.IsFailure ) 
				{
					++failures;
					DisplayFailures();
				}
			}
		}
	}
}
