// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.UiKit
{
	public class StatusBar : System.Windows.Forms.StatusBar, TestObserver
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

			// Temporarily remove AutoSize due to Mono 1.2 rc problems
			//
			//testCountPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			testCountPanel.MinWidth = 120;
			//testsRunPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			testsRunPanel.MinWidth = 120;
			//failuresPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			failuresPanel.MinWidth = 104;
			//timePanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			timePanel.MinWidth = 120;

			ShowPanels = true;
			InitPanels();
		}

		// Kluge to keep VS from generating code that sets the Panels for
		// the statusbar. Really, our status bar should be a user control
		// to avoid this and shouldn't allow the panels to be set except
		// according to specific protocols.
		[System.ComponentModel.DesignerSerializationVisibility( 
			 System.ComponentModel.DesignerSerializationVisibility.Hidden )]
		public new System.Windows.Forms.StatusBar.StatusBarPanelCollection Panels
		{
			get 
			{ 
				return base.Panels;
			}
		}

	
		public override string Text
		{
			get { return statusPanel.Text; }
			set { statusPanel.Text = value; }
		}

		public void Initialize( int testCount )
		{
			Initialize( testCount, testCount > 0 ? "Ready" : "" );
		}

		public void Initialize( int testCount, string text )
		{
			this.statusPanel.Text = text;

			this.testCount = testCount;
			this.testsRun = 0;
			this.failures = 0;
			this.time = 0;

			InitPanels();
		}

		private void InitPanels()
		{
			DisplayTestCount();
			this.testsRunPanel.Text = "";
			this.failuresPanel.Text = "";
			this.timePanel.Text = "";
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

		private void DisplayResult(TestResult result)
		{
			ResultSummarizer summarizer = new ResultSummarizer(result);

			int failureCount = summarizer.FailureCount;

			failuresPanel.Text = "Failures : " + failureCount.ToString();
			testsRunPanel.Text = "Tests Run : " + summarizer.ResultCount.ToString();
			timePanel.Text = "Time : " + summarizer.Time.ToString();
		}

		public void OnTestLoaded( object sender, TestEventArgs e )
		{
			Initialize( e.TestCount );
		}

		public void OnTestReloaded( object sender, TestEventArgs e )
		{
			Initialize( e.TestCount, "Reloaded" );
		}

		public void OnTestUnloaded( object sender, TestEventArgs e )
		{
			Initialize( 0, "Unloaded" );
		}

		private void OnRunStarting( object sender, TestEventArgs e )
		{
			Initialize( e.TestCount, "Running :" + e.Name );
			DisplayTestCount();
			DisplayFailures();
			DisplayTime();
		}

		private void OnRunFinished(object sender, TestEventArgs e )
		{
			if ( e.Exception != null )
				statusPanel.Text = "Failed";
			else
			{
				statusPanel.Text = "Completed";
				DisplayResult( e.Result );
			}
		}

		public void OnTestStarting( object sender, TestEventArgs e )
		{
			string fullText = "Running : " + e.TestName.FullName;
			string shortText = "Running : " + e.TestName.Name;

			Graphics g = Graphics.FromHwnd( Handle );
			SizeF sizeNeeded = g.MeasureString( fullText, Font );
			if ( statusPanel.Width >= (int)sizeNeeded.Width )
			{
				statusPanel.Text = fullText;
				statusPanel.ToolTipText = "";
			}
			else
			{
				sizeNeeded = g.MeasureString( shortText, Font );
				statusPanel.Text = statusPanel.Width >= (int)sizeNeeded.Width
					? shortText : e.TestName.Name;
				statusPanel.ToolTipText = e.TestName.FullName;
			}
		}

		private void OnTestFinished( object sender, TestEventArgs e )
		{
			if ( DisplayTestProgress && e.Result.Executed )
			{
				++testsRun;
				DisplayTestsRun();
				if ( e.Result.IsFailure ) 
				{
					++failures;
					DisplayFailures();
				}
			}
		}

		#region TestObserver Members

		public void Subscribe(ITestEvents events)
		{
			events.TestLoaded	+= new TestEventHandler( OnTestLoaded );
			events.TestReloaded	+= new TestEventHandler( OnTestReloaded );
			events.TestUnloaded	+= new TestEventHandler( OnTestUnloaded );

			events.TestStarting	+= new TestEventHandler( OnTestStarting );
			events.TestFinished	+= new TestEventHandler( OnTestFinished );
			events.RunStarting	+= new TestEventHandler( OnRunStarting );
			events.RunFinished	+= new TestEventHandler( OnRunFinished );
		}

		#endregion
	}
}
