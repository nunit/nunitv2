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
using NUnit.Util;
using NUnit.UiKit;

namespace NUnit.Gui
{
	/// <summary>
	/// Summary description for OptionsDialog.
	/// </summary>
	public class OptionsDialog : System.Windows.Forms.Form
	{
		private OptionSettings options;

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.CheckBox clearResultsCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox initialDisplayComboBox;
		private System.Windows.Forms.CheckBox reloadOnChangeCheckBox;
		private System.Windows.Forms.CheckBox reloadOnRunCheckBox;
		private System.Windows.Forms.CheckBox visualStudioSupportCheckBox;
		private System.Windows.Forms.CheckBox loadLastProjectCheckBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox recentFilesCountTextBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.ComponentModel.IContainer components = null;

		public static void EditOptions( )
		{
			OptionsDialog dialog = new OptionsDialog( UserSettings.Options );
			dialog.ShowDialog();
		}

		public OptionsDialog( OptionSettings options )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.options = options;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OptionsDialog));
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.loadLastProjectCheckBox = new System.Windows.Forms.CheckBox();
			this.clearResultsCheckBox = new System.Windows.Forms.CheckBox();
			this.reloadOnChangeCheckBox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.initialDisplayComboBox = new System.Windows.Forms.ComboBox();
			this.reloadOnRunCheckBox = new System.Windows.Forms.CheckBox();
			this.visualStudioSupportCheckBox = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.recentFilesCountTextBox = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.okButton.Location = new System.Drawing.Point(77, 312);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 13;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(173, 312);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(67, 23);
			this.cancelButton.TabIndex = 14;
			this.cancelButton.Text = "Cancel";
			// 
			// loadLastProjectCheckBox
			// 
			this.helpProvider1.SetHelpString(this.loadLastProjectCheckBox, "If checked, most recent project is loaded at startup.");
			this.loadLastProjectCheckBox.Location = new System.Drawing.Point(32, 64);
			this.loadLastProjectCheckBox.Name = "loadLastProjectCheckBox";
			this.helpProvider1.SetShowHelp(this.loadLastProjectCheckBox, true);
			this.loadLastProjectCheckBox.Size = new System.Drawing.Size(256, 24);
			this.loadLastProjectCheckBox.TabIndex = 4;
			this.loadLastProjectCheckBox.Text = "Load most recent project at startup.";
			// 
			// clearResultsCheckBox
			// 
			this.helpProvider1.SetHelpString(this.clearResultsCheckBox, "If checked, any prior results are cleared when reloading.");
			this.clearResultsCheckBox.Location = new System.Drawing.Point(32, 216);
			this.clearResultsCheckBox.Name = "clearResultsCheckBox";
			this.helpProvider1.SetShowHelp(this.clearResultsCheckBox, true);
			this.clearResultsCheckBox.Size = new System.Drawing.Size(232, 24);
			this.clearResultsCheckBox.TabIndex = 10;
			this.clearResultsCheckBox.Text = "Clear results when reloading.";
			// 
			// reloadOnChangeCheckBox
			// 
			this.helpProvider1.SetHelpString(this.reloadOnChangeCheckBox, "If checked, the assembly is reloaded whenever it changes. Changes to this setting" +
				" do not take effect until the next time an assembly is loaded.");
			this.reloadOnChangeCheckBox.Location = new System.Drawing.Point(32, 184);
			this.reloadOnChangeCheckBox.Name = "reloadOnChangeCheckBox";
			this.helpProvider1.SetShowHelp(this.reloadOnChangeCheckBox, true);
			this.reloadOnChangeCheckBox.Size = new System.Drawing.Size(256, 24);
			this.reloadOnChangeCheckBox.TabIndex = 9;
			this.reloadOnChangeCheckBox.Text = "Reload when test assembly changes";
			// 
			// label1
			// 
			this.helpProvider1.SetHelpString(this.label1, "");
			this.label1.Location = new System.Drawing.Point(32, 96);
			this.label1.Name = "label1";
			this.helpProvider1.SetShowHelp(this.label1, true);
			this.label1.Size = new System.Drawing.Size(144, 24);
			this.label1.TabIndex = 5;
			this.label1.Text = "Initial display on load:";
			// 
			// initialDisplayComboBox
			// 
			this.initialDisplayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.helpProvider1.SetHelpString(this.initialDisplayComboBox, "Selects the initial display style of the tree when an assembly is loaded");
			this.initialDisplayComboBox.Items.AddRange(new object[] {
																		"Auto",
																		"Expand",
																		"Collapse",
																		"HideTests"});
			this.initialDisplayComboBox.Location = new System.Drawing.Point(200, 96);
			this.initialDisplayComboBox.Name = "initialDisplayComboBox";
			this.helpProvider1.SetShowHelp(this.initialDisplayComboBox, true);
			this.initialDisplayComboBox.Size = new System.Drawing.Size(88, 24);
			this.initialDisplayComboBox.TabIndex = 6;
			// 
			// reloadOnRunCheckBox
			// 
			this.helpProvider1.SetHelpString(this.reloadOnRunCheckBox, "If checked, the assembly is reloaded before each run.");
			this.reloadOnRunCheckBox.Location = new System.Drawing.Point(32, 160);
			this.reloadOnRunCheckBox.Name = "reloadOnRunCheckBox";
			this.helpProvider1.SetShowHelp(this.reloadOnRunCheckBox, true);
			this.reloadOnRunCheckBox.Size = new System.Drawing.Size(264, 24);
			this.reloadOnRunCheckBox.TabIndex = 8;
			this.reloadOnRunCheckBox.Text = "Reload before each test run";
			// 
			// visualStudioSupportCheckBox
			// 
			this.helpProvider1.SetHelpString(this.visualStudioSupportCheckBox, "If checked, Visual Studio projects and solutions may be opened or added to existi" +
				"ng test projects.");
			this.visualStudioSupportCheckBox.Location = new System.Drawing.Point(32, 272);
			this.visualStudioSupportCheckBox.Name = "visualStudioSupportCheckBox";
			this.helpProvider1.SetShowHelp(this.visualStudioSupportCheckBox, true);
			this.visualStudioSupportCheckBox.Size = new System.Drawing.Size(264, 24);
			this.visualStudioSupportCheckBox.TabIndex = 12;
			this.visualStudioSupportCheckBox.Text = "Enable Visual Studio Support";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(32, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Display";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(152, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 24);
			this.label3.TabIndex = 3;
			this.label3.Text = "files in list";
			// 
			// recentFilesCountTextBox
			// 
			this.recentFilesCountTextBox.Location = new System.Drawing.Point(96, 32);
			this.recentFilesCountTextBox.Name = "recentFilesCountTextBox";
			this.recentFilesCountTextBox.Size = new System.Drawing.Size(40, 22);
			this.recentFilesCountTextBox.TabIndex = 2;
			this.recentFilesCountTextBox.Text = "";
			this.recentFilesCountTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.recentFilesCountTextBox_Validating);
			this.recentFilesCountTextBox.Validated += new System.EventHandler(this.recentFilesCountTextBox_Validated);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(288, 128);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Recent Files";
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(16, 144);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(288, 104);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Assembly Reload";
			// 
			// groupBox3
			// 
			this.groupBox3.Location = new System.Drawing.Point(16, 256);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(288, 48);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Visual Studio";
			// 
			// OptionsDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(322, 338);
			this.Controls.Add(this.recentFilesCountTextBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.visualStudioSupportCheckBox);
			this.Controls.Add(this.reloadOnRunCheckBox);
			this.Controls.Add(this.initialDisplayComboBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.loadLastProjectCheckBox);
			this.Controls.Add(this.clearResultsCheckBox);
			this.Controls.Add(this.reloadOnChangeCheckBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NUnit Options";
			this.Load += new System.EventHandler(this.OptionsDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void OptionsDialog_Load(object sender, System.EventArgs e)
		{
			recentFilesCountTextBox.Text = UserSettings.RecentProjects.MaxFiles.ToString();
			loadLastProjectCheckBox.Checked = options.LoadLastProject;
			initialDisplayComboBox.SelectedIndex = options.InitialTreeDisplay;

			reloadOnChangeCheckBox.Enabled = Environment.OSVersion.Platform == System.PlatformID.Win32NT;
			reloadOnChangeCheckBox.Checked = options.ReloadOnChange;
			reloadOnRunCheckBox.Checked = options.ReloadOnRun;
			clearResultsCheckBox.Checked = options.ClearResults;

			visualStudioSupportCheckBox.Checked = options.VisualStudioSupport;
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			if ( options.ReloadOnChange != reloadOnChangeCheckBox.Checked )
			{
				string msg = String.Format(
					"Watching for file changes will be {0} the next time you load an assembly.",
					reloadOnChangeCheckBox.Checked ? "enabled" : "disabled" );

				UserMessage.DisplayInfo( msg, "NUnit Options" );
			}

			options.LoadLastProject = loadLastProjectCheckBox.Checked;
			
			options.ReloadOnChange = reloadOnChangeCheckBox.Checked;
			options.ReloadOnRun = reloadOnRunCheckBox.Checked;
			options.ClearResults = clearResultsCheckBox.Checked;
			options.VisualStudioSupport = visualStudioSupportCheckBox.Checked;

			options.InitialTreeDisplay = initialDisplayComboBox.SelectedIndex;

			DialogResult = DialogResult.OK;

			Close();
		}

		private void recentFilesCountTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			int count = int.Parse( recentFilesCountTextBox.Text );
			if ( count < RecentProjectSettings.MinSize ||
				count > RecentProjectSettings.MaxSize )
			{
				recentFilesCountTextBox.SelectAll();
				UserMessage.DisplayFailure( string.Format( "Number of files must be from {0} to {1}", RecentProjectSettings.MinSize, RecentProjectSettings.MaxSize ) );
				e.Cancel = true;
			}
		}

		private void recentFilesCountTextBox_Validated(object sender, System.EventArgs e)
		{
			int count = int.Parse( recentFilesCountTextBox.Text );
			UserSettings.RecentProjects.MaxFiles = count;
		}
	}
}
