#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
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
			testCountPanel.MinWidth = 120;
			testsRunPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			testsRunPanel.MinWidth = 120;
			failuresPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			failuresPanel.MinWidth = 104;
			timePanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			timePanel.MinWidth = 120;

			ShowPanels = true;
			InitPanels();
		}

		public override string Text
		{
			get { return statusPanel.Text; }
			set { statusPanel.Text = value; }
		}

		public void Initialize( ITestEvents events )
		{
			events.TestLoaded	+= new TestEventHandler( OnTestLoaded );
			events.TestReloaded	+= new TestEventHandler( OnTestReloaded );
			events.TestUnloaded	+= new TestEventHandler( OnTestUnloaded );

			events.TestStarting	+= new TestEventHandler( OnTestStarting );
			events.TestFinished	+= new TestEventHandler( OnTestFinished );
			events.RunStarting	+= new TestEventHandler( OnRunStarting );
			events.RunFinished	+= new TestEventHandler( OnRunFinished );
		}

		public void Initialize( int testCount )
		{
			Initialize( testCount, testCount > 0 ? "Ready" : "" );
		}

		private void Initialize( int testCount, string text )
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

		private void DisplayResults(TestResult[] results)
		{
			ResultSummarizer summarizer = new ResultSummarizer(results);

			int failureCount = summarizer.Failures;

			failuresPanel.Text = "Failures : " + failureCount.ToString();
			testsRunPanel.Text = "Tests Run : " + summarizer.ResultCount.ToString();
			timePanel.Text = "Time : " + summarizer.Time.ToString();
		}

		public void OnTestLoaded( object sender, TestEventArgs e )
		{
			Initialize( e.Test.CountTestCases() );
		}

		public void OnTestReloaded( object sender, TestEventArgs e )
		{
			Initialize( e.Test.CountTestCases(), "Reloaded" );
		}

		public void OnTestUnloaded( object sender, TestEventArgs e )
		{
			Initialize( 0, "Unloaded" );
		}

		private void OnRunStarting( object sender, TestEventArgs e )
		{
			Initialize( e.TestCount, "Running :" + e.Test.Name );
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
				DisplayResults( e.Results );
			}
		}

		public void OnTestStarting( object sender, TestEventArgs e )
		{
			statusPanel.Text = "Running : " + e.Test.Name;
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
	}
}
