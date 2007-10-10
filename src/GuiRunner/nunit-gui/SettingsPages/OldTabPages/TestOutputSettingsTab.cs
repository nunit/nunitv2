using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NUnit.Gui.SettingsPages
{
	public class TestOutputSettingsTab : NUnit.UiKit.SettingsPage
	{
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox errorsTabCheckBox;
		private System.Windows.Forms.CheckBox failureToolTips;
		private System.Windows.Forms.CheckBox enableWordWrap;
		private System.Windows.Forms.CheckBox notRunTabCheckBox;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.CheckBox consoleOutputCheckBox;
		private System.Windows.Forms.CheckBox labelTestOutputCheckBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox consoleErrrorCheckBox;
		private System.Windows.Forms.RadioButton mergeErrors;
		private System.Windows.Forms.RadioButton separateErrors;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.RadioButton separateTrace;
		private System.Windows.Forms.CheckBox traceOutputCheckBox;
		private System.Windows.Forms.RadioButton mergeTrace;
		private System.ComponentModel.IContainer components = null;

		public TestOutputSettingsTab(string key) : base(key)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.errorsTabCheckBox = new System.Windows.Forms.CheckBox();
			this.failureToolTips = new System.Windows.Forms.CheckBox();
			this.enableWordWrap = new System.Windows.Forms.CheckBox();
			this.notRunTabCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.consoleOutputCheckBox = new System.Windows.Forms.CheckBox();
			this.labelTestOutputCheckBox = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.consoleErrrorCheckBox = new System.Windows.Forms.CheckBox();
			this.mergeErrors = new System.Windows.Forms.RadioButton();
			this.separateErrors = new System.Windows.Forms.RadioButton();
			this.panel2 = new System.Windows.Forms.Panel();
			this.separateTrace = new System.Windows.Forms.RadioButton();
			this.traceOutputCheckBox = new System.Windows.Forms.CheckBox();
			this.mergeTrace = new System.Windows.Forms.RadioButton();
			this.groupBox3.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.errorsTabCheckBox);
			this.groupBox3.Controls.Add(this.failureToolTips);
			this.groupBox3.Controls.Add(this.enableWordWrap);
			this.groupBox3.Controls.Add(this.notRunTabCheckBox);
			this.groupBox3.Location = new System.Drawing.Point(8, 8);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(296, 128);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Test Results";
			// 
			// errorsTabCheckBox
			// 
			this.errorsTabCheckBox.Checked = true;
			this.errorsTabCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.errorsTabCheckBox.Location = new System.Drawing.Point(16, 16);
			this.errorsTabCheckBox.Name = "errorsTabCheckBox";
			this.errorsTabCheckBox.Size = new System.Drawing.Size(248, 24);
			this.errorsTabCheckBox.TabIndex = 0;
			this.errorsTabCheckBox.Text = "Display Errors and Failures";
			// 
			// failureToolTips
			// 
			this.failureToolTips.Location = new System.Drawing.Point(32, 40);
			this.failureToolTips.Name = "failureToolTips";
			this.failureToolTips.Size = new System.Drawing.Size(202, 19);
			this.failureToolTips.TabIndex = 1;
			this.failureToolTips.Text = "Display Failure ToolTips";
			// 
			// enableWordWrap
			// 
			this.enableWordWrap.Location = new System.Drawing.Point(32, 64);
			this.enableWordWrap.Name = "enableWordWrap";
			this.enableWordWrap.Size = new System.Drawing.Size(248, 19);
			this.enableWordWrap.TabIndex = 2;
			this.enableWordWrap.Text = "Enable Word Wrap";
			// 
			// notRunTabCheckBox
			// 
			this.notRunTabCheckBox.Checked = true;
			this.notRunTabCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.notRunTabCheckBox.Location = new System.Drawing.Point(16, 96);
			this.notRunTabCheckBox.Name = "notRunTabCheckBox";
			this.notRunTabCheckBox.Size = new System.Drawing.Size(264, 16);
			this.notRunTabCheckBox.TabIndex = 3;
			this.notRunTabCheckBox.Text = "Display Tests Not Run";
			// 
			// groupBox8
			// 
			this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox8.Controls.Add(this.consoleOutputCheckBox);
			this.groupBox8.Controls.Add(this.labelTestOutputCheckBox);
			this.groupBox8.Controls.Add(this.panel1);
			this.groupBox8.Controls.Add(this.panel2);
			this.groupBox8.Location = new System.Drawing.Point(8, 144);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(298, 232);
			this.groupBox8.TabIndex = 2;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Text Output";
			// 
			// consoleOutputCheckBox
			// 
			this.consoleOutputCheckBox.Checked = true;
			this.consoleOutputCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.consoleOutputCheckBox.Location = new System.Drawing.Point(16, 18);
			this.consoleOutputCheckBox.Name = "consoleOutputCheckBox";
			this.consoleOutputCheckBox.Size = new System.Drawing.Size(240, 16);
			this.consoleOutputCheckBox.TabIndex = 0;
			this.consoleOutputCheckBox.Text = "Display Console Standard Output";
			// 
			// labelTestOutputCheckBox
			// 
			this.labelTestOutputCheckBox.Location = new System.Drawing.Point(32, 40);
			this.labelTestOutputCheckBox.Name = "labelTestOutputCheckBox";
			this.labelTestOutputCheckBox.Size = new System.Drawing.Size(237, 19);
			this.labelTestOutputCheckBox.TabIndex = 1;
			this.labelTestOutputCheckBox.Text = "Label Test Cases";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.consoleErrrorCheckBox);
			this.panel1.Controls.Add(this.mergeErrors);
			this.panel1.Controls.Add(this.separateErrors);
			this.panel1.Location = new System.Drawing.Point(8, 64);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(272, 80);
			this.panel1.TabIndex = 2;
			// 
			// consoleErrrorCheckBox
			// 
			this.consoleErrrorCheckBox.Checked = true;
			this.consoleErrrorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.consoleErrrorCheckBox.Location = new System.Drawing.Point(8, 8);
			this.consoleErrrorCheckBox.Name = "consoleErrrorCheckBox";
			this.consoleErrrorCheckBox.Size = new System.Drawing.Size(232, 24);
			this.consoleErrrorCheckBox.TabIndex = 0;
			this.consoleErrrorCheckBox.Text = "Display Console Error Output";
			// 
			// mergeErrors
			// 
			this.mergeErrors.Location = new System.Drawing.Point(32, 56);
			this.mergeErrors.Name = "mergeErrors";
			this.mergeErrors.Size = new System.Drawing.Size(192, 16);
			this.mergeErrors.TabIndex = 2;
			this.mergeErrors.Text = "Merge with Console Output";
			// 
			// separateErrors
			// 
			this.separateErrors.Checked = true;
			this.separateErrors.Location = new System.Drawing.Point(32, 32);
			this.separateErrors.Name = "separateErrors";
			this.separateErrors.Size = new System.Drawing.Size(224, 16);
			this.separateErrors.TabIndex = 1;
			this.separateErrors.TabStop = true;
			this.separateErrors.Text = "In Separate Tab";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.separateTrace);
			this.panel2.Controls.Add(this.traceOutputCheckBox);
			this.panel2.Location = new System.Drawing.Point(8, 144);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(272, 80);
			this.panel2.TabIndex = 3;
			// 
			// separateTrace
			// 
			this.separateTrace.Checked = true;
			this.separateTrace.Location = new System.Drawing.Point(32, 32);
			this.separateTrace.Name = "separateTrace";
			this.separateTrace.Size = new System.Drawing.Size(224, 16);
			this.separateTrace.TabIndex = 1;
			this.separateTrace.TabStop = true;
			this.separateTrace.Text = "In Separate Tab";
			// 
			// traceOutputCheckBox
			// 
			this.traceOutputCheckBox.Checked = true;
			this.traceOutputCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.traceOutputCheckBox.Location = new System.Drawing.Point(8, 8);
			this.traceOutputCheckBox.Name = "traceOutputCheckBox";
			this.traceOutputCheckBox.Size = new System.Drawing.Size(208, 16);
			this.traceOutputCheckBox.TabIndex = 0;
			this.traceOutputCheckBox.Text = "Display Trace Output";
			// 
			// mergeTrace
			// 
			this.mergeTrace.Location = new System.Drawing.Point(0, 0);
			this.mergeTrace.Name = "mergeTrace";
			this.mergeTrace.TabIndex = 0;
			// 
			// TestOutputSettingsTab
			// 
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox8);
			this.Name = "TestOutputSettingsTab";
			this.Size = new System.Drawing.Size(310, 384);
			this.groupBox3.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public override void LoadSettings()
		{
			errorsTabCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayErrorsTab", true );
			notRunTabCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayNotRunTab", true );
			consoleOutputCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayConsoleOutputTab", true );
			consoleErrrorCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayConsoleErrorTab", true )
				|| settings.GetSetting( "Gui.ResultTabs.MergeErrorOutput", false );
			traceOutputCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayTraceTab", true )
				|| settings.GetSetting( "Gui.ResultTabs.MergeTraceOutput", false );

			labelTestOutputCheckBox.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayTestLabels", false );
			failureToolTips.Checked = settings.GetSetting( "Gui.ResultTabs.ErrorsTab.ToolTipsEnabled", true );
			enableWordWrap.Checked = settings.GetSetting( "Gui.ResultTabs.ErrorsTab.WordWrapEnabled", true );

			mergeErrors.Checked = settings.GetSetting( "Gui.ResultTabs.MergeErrorOutput", false );
			mergeTrace.Checked = settings.GetSetting( "Gui.ResultTabs.MergeTraceOutput", false );
		}

		public override void ApplySettings()
		{
			settings.SaveSetting( "Gui.ResultTabs.DisplayTestLabels", labelTestOutputCheckBox.Checked );
			settings.SaveSetting( "Gui.ResultTabs.ErrorsTab.ToolTipsEnabled", failureToolTips.Checked );
			settings.SaveSetting( "Gui.ResultTabs.ErrorsTab.WordWrapEnabled", enableWordWrap.Checked );
		

			settings.SaveSetting( "Gui.ResultTabs.DisplayErrorsTab", errorsTabCheckBox.Checked );
			settings.SaveSetting( "Gui.ResultTabs.DisplayNotRunTab", notRunTabCheckBox.Checked );
			settings.SaveSetting( "Gui.ResultTabs.DisplayConsoleOutputTab", consoleOutputCheckBox.Checked );
			settings.SaveSetting( "Gui.ResultTabs.DisplayConsoleErrorTab", consoleErrrorCheckBox.Checked && separateErrors.Checked );
			settings.SaveSetting( "Gui.ResultTabs.DisplayTraceTab", traceOutputCheckBox.Checked && separateTrace.Checked );

			settings.SaveSetting( "Gui.ResultTabs.MergeErrorOutput", mergeErrors.Checked );
			settings.SaveSetting( "Gui.ResultTabs.MergeTraceOutput", mergeTrace.Checked );
		}

		private void errorsTabCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			this.failureToolTips.Enabled = this.enableWordWrap.Enabled = this.errorsTabCheckBox.Checked;
		}

		private void consoleOutputCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			this.labelTestOutputCheckBox.Enabled = this.consoleOutputCheckBox.Checked;
		}

		private void consoleErrrorCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			this.separateErrors.Enabled = this.mergeErrors.Enabled = this.consoleErrrorCheckBox.Checked;
		}

		private void traceOutputCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			this.separateTrace.Enabled = this.mergeTrace.Enabled = this.traceOutputCheckBox.Checked;
		}

	}
}

