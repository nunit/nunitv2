#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.UiKit
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

		public override string Text
		{
			get { return statusPanel.Text; }
			set { statusPanel.Text = value; }
		}

		public void InitializeEvents( UIEvents uiEvents )
		{
			this.uiEvents = uiEvents;

			uiEvents.TestSuiteUnloadedEvent += new TestSuiteUnloadedHandler( OnSuiteUnloaded );

			uiEvents.TestStartedEvent += new TestStartedHandler( OnTestStarted );
			uiEvents.TestFinishedEvent += new TestFinishedHandler( OnTestFinished );
			uiEvents.RunStartingEvent += new RunStartingHandler( OnRunStarting );
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

		public void OnSuiteUnloaded()
		{
			Initialize( 0 );
		}

		public void OnTestStarted( UITestNode testCase )
		{
			statusPanel.Text = "Running : " + testCase.Name;
		}

		private void OnRunStarting( UITestNode test )
		{
			Initialize( test.CountTestCases );
		}

		private void OnRunFinished(TestResult result)
		{
			statusPanel.Text = "Completed";

			// NOTE: call DisplayResults always since we need the time
			DisplayResults( result );
		}

		private void OnTestFinished( TestCaseResult result )
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
