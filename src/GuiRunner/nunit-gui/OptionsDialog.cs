using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NUnit.Util;

namespace NUnit.Gui
{
	/// <summary>
	/// Summary description for OptionsDialog.
	/// </summary>
	public class OptionsDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox loadLastAssemblyCheckBox;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox enableWatcherCheckBox;
		private System.Windows.Forms.CheckBox expandOnLoadCheckBox;
		private System.Windows.Forms.CheckBox hideTestCasesCheckBox;
		private System.Windows.Forms.CheckBox clearResultsCheckBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public OptionsDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.expandOnLoadCheckBox = new System.Windows.Forms.CheckBox();
			this.loadLastAssemblyCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.enableWatcherCheckBox = new System.Windows.Forms.CheckBox();
			this.hideTestCasesCheckBox = new System.Windows.Forms.CheckBox();
			this.clearResultsCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(120, 240);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 0;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(208, 240);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(67, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.hideTestCasesCheckBox,
																					this.expandOnLoadCheckBox,
																					this.loadLastAssemblyCheckBox});
			this.groupBox1.Location = new System.Drawing.Point(8, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(376, 112);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Startup";
			// 
			// expandOnLoadCheckBox
			// 
			this.expandOnLoadCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.expandOnLoadCheckBox.Location = new System.Drawing.Point(24, 56);
			this.expandOnLoadCheckBox.Name = "expandOnLoadCheckBox";
			this.expandOnLoadCheckBox.Size = new System.Drawing.Size(336, 24);
			this.expandOnLoadCheckBox.TabIndex = 4;
			this.expandOnLoadCheckBox.Text = "Expand tree on loading assembly";
			this.expandOnLoadCheckBox.CheckedChanged += new System.EventHandler(this.expandOnLoadCheckBox_CheckedChanged);
			// 
			// loadLastAssemblyCheckBox
			// 
			this.loadLastAssemblyCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.loadLastAssemblyCheckBox.Location = new System.Drawing.Point(24, 24);
			this.loadLastAssemblyCheckBox.Name = "loadLastAssemblyCheckBox";
			this.loadLastAssemblyCheckBox.Size = new System.Drawing.Size(336, 24);
			this.loadLastAssemblyCheckBox.TabIndex = 3;
			this.loadLastAssemblyCheckBox.Text = "Load last assembly on startup";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.clearResultsCheckBox,
																					this.enableWatcherCheckBox});
			this.groupBox2.Location = new System.Drawing.Point(8, 136);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(376, 88);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Assembly Changes";
			// 
			// enableWatcherCheckBox
			// 
			this.enableWatcherCheckBox.Location = new System.Drawing.Point(24, 24);
			this.enableWatcherCheckBox.Name = "enableWatcherCheckBox";
			this.enableWatcherCheckBox.Size = new System.Drawing.Size(336, 24);
			this.enableWatcherCheckBox.TabIndex = 0;
			this.enableWatcherCheckBox.Text = "Watch for file changes and reload the assembly";
			// 
			// hideTestCasesCheckBox
			// 
			this.hideTestCasesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.hideTestCasesCheckBox.Location = new System.Drawing.Point(56, 80);
			this.hideTestCasesCheckBox.Name = "hideTestCasesCheckBox";
			this.hideTestCasesCheckBox.Size = new System.Drawing.Size(304, 24);
			this.hideTestCasesCheckBox.TabIndex = 5;
			this.hideTestCasesCheckBox.Text = "Hide test cases when expanding";
			// 
			// clearResultsCheckBox
			// 
			this.clearResultsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.clearResultsCheckBox.Location = new System.Drawing.Point(56, 56);
			this.clearResultsCheckBox.Name = "clearResultsCheckBox";
			this.clearResultsCheckBox.Size = new System.Drawing.Size(312, 24);
			this.clearResultsCheckBox.TabIndex = 1;
			this.clearResultsCheckBox.Text = "Clear test results when reloading";
			// 
			// OptionsDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(392, 268);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox2,
																		  this.groupBox1,
																		  this.cancelButton,
																		  this.okButton});
			this.Name = "OptionsDialog";
			this.Text = "NUnit Options";
			this.Load += new System.EventHandler(this.OptionsDialog_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void OptionsDialog_Load(object sender, System.EventArgs e)
		{
			loadLastAssemblyCheckBox.Checked = UserSettings.Options.LoadLastAssembly;
			expandOnLoadCheckBox.Checked = UserSettings.Options.ExpandOnLoad;
			hideTestCasesCheckBox.Enabled = expandOnLoadCheckBox.Checked;
			hideTestCasesCheckBox.Checked = UserSettings.Options.HideTestCases;

			enableWatcherCheckBox.Checked = UserSettings.Options.EnableWatcher;
			clearResultsCheckBox.Checked = UserSettings.Options.ClearResults;
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			UserSettings.Options.LoadLastAssembly = loadLastAssemblyCheckBox.Checked;
			UserSettings.Options.ExpandOnLoad = expandOnLoadCheckBox.Checked;
			UserSettings.Options.HideTestCases = hideTestCasesCheckBox.Checked;
			
			UserSettings.Options.EnableWatcher = enableWatcherCheckBox.Checked;
			UserSettings.Options.ClearResults = clearResultsCheckBox.Checked;

			this.Close();
		}

		private void expandOnLoadCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			hideTestCasesCheckBox.Enabled = expandOnLoadCheckBox.Checked;
		}
	}
}
