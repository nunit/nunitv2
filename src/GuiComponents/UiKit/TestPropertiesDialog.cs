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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using NUnit.Core;
using NUnit.Util;
using CP.Windows.Forms;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for TestPropertiesDialog.
	/// </summary>
	public class TestPropertiesDialog : System.Windows.Forms.Form
	{
		#region Instance Variables;

		private TestSuiteTreeNode node;
		private ITest test;
		private TestResult result;
		private Image pinnedImage;
		private Image unpinnedImage;

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Label testCaseCount;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private CP.Windows.Forms.ExpandingLabel ignoreReason;
		private System.Windows.Forms.Label shouldRun;
		private System.Windows.Forms.Label label2;
		private CP.Windows.Forms.ExpandingLabel fullName;
		private System.Windows.Forms.Label label1;
		private CP.Windows.Forms.ExpandingLabel stackTrace;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label elapsedTime;
		private CP.Windows.Forms.ExpandingLabel message;
		private System.Windows.Forms.TabPage resultsTab;
		private System.Windows.Forms.TabPage testTab;
		private System.Windows.Forms.Label testResult;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox pinButton;
		private System.Windows.Forms.Label label6;
		private CP.Windows.Forms.ExpandingLabel description;
		private System.Windows.Forms.Label assertCount;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Construction and Disposal

		public TestPropertiesDialog( TestSuiteTreeNode node )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.node = node;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.testTab = new System.Windows.Forms.TabPage();
			this.description = new CP.Windows.Forms.ExpandingLabel();
			this.label6 = new System.Windows.Forms.Label();
			this.testCaseCount = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.ignoreReason = new CP.Windows.Forms.ExpandingLabel();
			this.shouldRun = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.fullName = new CP.Windows.Forms.ExpandingLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.resultsTab = new System.Windows.Forms.TabPage();
			this.assertCount = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.testResult = new System.Windows.Forms.Label();
			this.stackTrace = new CP.Windows.Forms.ExpandingLabel();
			this.label12 = new System.Windows.Forms.Label();
			this.elapsedTime = new System.Windows.Forms.Label();
			this.message = new CP.Windows.Forms.ExpandingLabel();
			this.pinButton = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.testTab.SuspendLayout();
			this.resultsTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.testTab);
			this.tabControl1.Controls.Add(this.resultsTab);
			this.tabControl1.Location = new System.Drawing.Point(7, 7);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(346, 142);
			this.tabControl1.TabIndex = 13;
			// 
			// testTab
			// 
			this.testTab.Controls.Add(this.description);
			this.testTab.Controls.Add(this.label6);
			this.testTab.Controls.Add(this.testCaseCount);
			this.testTab.Controls.Add(this.label5);
			this.testTab.Controls.Add(this.label4);
			this.testTab.Controls.Add(this.ignoreReason);
			this.testTab.Controls.Add(this.shouldRun);
			this.testTab.Controls.Add(this.label2);
			this.testTab.Controls.Add(this.fullName);
			this.testTab.Controls.Add(this.label1);
			this.testTab.Location = new System.Drawing.Point(4, 22);
			this.testTab.Name = "testTab";
			this.testTab.Size = new System.Drawing.Size(338, 116);
			this.testTab.TabIndex = 0;
			this.testTab.Text = "Test";
			// 
			// description
			// 
			this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.description.CopySupported = true;
			this.description.Location = new System.Drawing.Point(87, 43);
			this.description.Name = "description";
			this.description.Size = new System.Drawing.Size(244, 14);
			this.description.TabIndex = 29;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(13, 42);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(67, 14);
			this.label6.TabIndex = 28;
			this.label6.Text = "Description:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// testCaseCount
			// 
			this.testCaseCount.Location = new System.Drawing.Point(87, 70);
			this.testCaseCount.Name = "testCaseCount";
			this.testCaseCount.Size = new System.Drawing.Size(40, 13);
			this.testCaseCount.TabIndex = 27;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(13, 96);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(67, 14);
			this.label5.TabIndex = 26;
			this.label5.Text = "Reason:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(13, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(67, 14);
			this.label4.TabIndex = 25;
			this.label4.Text = "Test Count:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ignoreReason
			// 
			this.ignoreReason.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ignoreReason.CopySupported = true;
			this.ignoreReason.Location = new System.Drawing.Point(87, 95);
			this.ignoreReason.Name = "ignoreReason";
			this.ignoreReason.Size = new System.Drawing.Size(244, 14);
			this.ignoreReason.TabIndex = 24;
			// 
			// shouldRun
			// 
			this.shouldRun.Location = new System.Drawing.Point(273, 70);
			this.shouldRun.Name = "shouldRun";
			this.shouldRun.Size = new System.Drawing.Size(27, 13);
			this.shouldRun.TabIndex = 23;
			this.shouldRun.Text = "Yes";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(167, 70);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 13);
			this.label2.TabIndex = 22;
			this.label2.Text = "Runnable?";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// fullName
			// 
			this.fullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.fullName.CopySupported = true;
			this.fullName.Location = new System.Drawing.Point(87, 14);
			this.fullName.Name = "fullName";
			this.fullName.Size = new System.Drawing.Size(244, 14);
			this.fullName.TabIndex = 21;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 14);
			this.label1.TabIndex = 20;
			this.label1.Text = "Full Name:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// resultsTab
			// 
			this.resultsTab.Controls.Add(this.assertCount);
			this.resultsTab.Controls.Add(this.label3);
			this.resultsTab.Controls.Add(this.testResult);
			this.resultsTab.Controls.Add(this.stackTrace);
			this.resultsTab.Controls.Add(this.label12);
			this.resultsTab.Controls.Add(this.elapsedTime);
			this.resultsTab.Controls.Add(this.message);
			this.resultsTab.Location = new System.Drawing.Point(4, 22);
			this.resultsTab.Name = "resultsTab";
			this.resultsTab.Size = new System.Drawing.Size(338, 116);
			this.resultsTab.TabIndex = 1;
			this.resultsTab.Text = "Result";
			// 
			// assertCount
			// 
			this.assertCount.Location = new System.Drawing.Point(193, 13);
			this.assertCount.Name = "assertCount";
			this.assertCount.Size = new System.Drawing.Size(118, 14);
			this.assertCount.TabIndex = 37;
			this.assertCount.Text = "Asserts:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(20, 42);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 13);
			this.label3.TabIndex = 36;
			this.label3.Text = "Message:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// testResult
			// 
			this.testResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.testResult.Location = new System.Drawing.Point(20, 14);
			this.testResult.Name = "testResult";
			this.testResult.Size = new System.Drawing.Size(60, 14);
			this.testResult.TabIndex = 35;
			this.testResult.Text = "Failure";
			// 
			// stackTrace
			// 
			this.stackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.stackTrace.CopySupported = true;
			this.stackTrace.Expansion = CP.Windows.Forms.TipWindow.ExpansionStyle.Both;
			this.stackTrace.Location = new System.Drawing.Point(87, 69);
			this.stackTrace.Name = "stackTrace";
			this.stackTrace.Size = new System.Drawing.Size(244, 39);
			this.stackTrace.TabIndex = 29;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(13, 69);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(60, 14);
			this.label12.TabIndex = 28;
			this.label12.Text = "Stack:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// elapsedTime
			// 
			this.elapsedTime.Location = new System.Drawing.Point(87, 14);
			this.elapsedTime.Name = "elapsedTime";
			this.elapsedTime.Size = new System.Drawing.Size(86, 14);
			this.elapsedTime.TabIndex = 27;
			this.elapsedTime.Text = "Time:";
			// 
			// message
			// 
			this.message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.message.CopySupported = true;
			this.message.Expansion = CP.Windows.Forms.TipWindow.ExpansionStyle.Both;
			this.message.Location = new System.Drawing.Point(87, 42);
			this.message.Name = "message";
			this.message.Size = new System.Drawing.Size(238, 13);
			this.message.TabIndex = 25;
			// 
			// pinButton
			// 
			this.pinButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pinButton.Appearance = System.Windows.Forms.Appearance.Button;
			this.pinButton.Location = new System.Drawing.Point(337, 7);
			this.pinButton.Name = "pinButton";
			this.pinButton.Size = new System.Drawing.Size(16, 17);
			this.pinButton.TabIndex = 14;
			this.pinButton.Click += new System.EventHandler(this.pinButton_Click);
			this.pinButton.CheckedChanged += new System.EventHandler(this.pinButton_CheckedChanged);
			// 
			// TestPropertiesDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(361, 150);
			this.Controls.Add(this.pinButton);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TestPropertiesDialog";
			this.ShowInTaskbar = false;
			this.Text = "Test Properties";
			this.Load += new System.EventHandler(this.TestPropertiesDialog_Load);
			this.tabControl1.ResumeLayout(false);
			this.testTab.ResumeLayout(false);
			this.resultsTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		#region Properties

		[Browsable( false )]
		public bool Pinned
		{
			get { return pinButton.Checked; }
			set { pinButton.Checked = value; }
		}

		#endregion

		#region Methods

		private void closeButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void SetTitleBarText()
		{
			string type;
			if ( test.IsTestCase )
				type = "Case";
			else if ( test.IsFixture )
				type = "Fixture";
			else
				type = "Suite";

			this.Text = string.Format( "Test {0} Properties - {1}", type, test.ShortName );
		}

		/// <summary>
		/// Set up all dialog fields when it loads
		/// </summary>
		private void TestPropertiesDialog_Load(object sender, System.EventArgs e)
		{
			pinnedImage = new Bitmap( typeof( TestPropertiesDialog ), "pinned.gif" );
			unpinnedImage = new Bitmap( typeof( TestPropertiesDialog ), "unpinned.gif" );
			pinButton.Image = unpinnedImage;

			DisplayProperties();

			node.TreeView.AfterSelect += new TreeViewEventHandler( OnSelectedNodeChanged );	
		}

		private void OnSelectedNodeChanged( object sender, TreeViewEventArgs e )
		{
			if ( pinButton.Checked )
			{
				DisplayProperties( (TestSuiteTreeNode)e.Node );
			}
			else
				this.Close();
		}

		public void DisplayProperties( )
		{
			DisplayProperties( this.node );
		}

		public void DisplayProperties( TestSuiteTreeNode node)
		{
			this.node = node;
			this.test = node.Test;
			this.result = node.Result;

			SetTitleBarText();

			// Initialize Test Tab
			fullName.Text = test.FullName;
			shouldRun.Text = test.ShouldRun ? "Yes" : "No";
			description.Text = test.Description;
			ignoreReason.Text = test.IgnoreReason;
			testCaseCount.Text = test.CountTestCases().ToString();

			// Initialize Result Tab
			if ( result == null || !result.Executed )
				tabControl1.TabPages.Remove( resultsTab );
			else
			{
				if ( !tabControl1.TabPages.Contains( resultsTab ) )
				{
					tabControl1.TabPages.Add( resultsTab );
				}

				testResult.Text = result.IsSuccess ? "Success" : "Failure";
				// message may have a leading blank line
				// TODO: take care of this in label?
				message.Text = TrimLeadingBlankLines( result.Message );
				elapsedTime.Text = string.Format( "Time: {0}", result.Time );
				assertCount.Text = string.Format( "Asserts: {0}", result.AssertCount );
				stackTrace.Text = result.StackTrace;
			}
		}

		private string TrimLeadingBlankLines( string s )
		{
			if ( s == null ) return s;

			int start = 0;
			for( int i = 0; i < s.Length; i++ )
			{
				switch( s[i] )
				{
					case ' ':
					case '\t':
						break;
					case '\r':
					case '\n':
						start = i + 1;
						break;

					default:
						goto getout;
				}
			}

			getout:
			return start == 0 ? s : s.Substring( start );
		}

		protected override bool ProcessKeyPreview(ref System.Windows.Forms.Message m)
		{
			const int ESCAPE = 27;
			const int WM_CHAR = 258;

			if (m.Msg == WM_CHAR && m.WParam.ToInt32() == ESCAPE )
			{
				this.Close();
				return true;
			}

			return base.ProcessKeyEventArgs( ref m ); 
		}

		private void pinButton_Click(object sender, System.EventArgs e)
		{
			if ( pinButton.Checked )
				pinButton.Image = pinnedImage;
			else
				pinButton.Image = unpinnedImage;
		}

		private void pinButton_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}
	}

	#endregion
}
