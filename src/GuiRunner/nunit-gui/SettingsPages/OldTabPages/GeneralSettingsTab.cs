using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NUnit.Util;
using NUnit.UiKit;

namespace NUnit.Gui.SettingsPages
{
	/// <summary>
	/// Summary description for GeneralOptions.
	/// </summary>
	public class GeneralSettingsTab : NUnit.UiKit.SettingsPage
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox recentFilesCountTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox loadLastProjectCheckBox;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox initialDisplayComboBox;
		private System.Windows.Forms.CheckBox clearResultsCheckBox;
		private System.Windows.Forms.CheckBox saveVisualStateCheckBox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox visualStudioSupportCheckBox;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.CheckBox shadowCopyCheckBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GeneralSettingsTab(string key) : base(key)
		{
			// This call is required by the Windows.Forms Form Designer.
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
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.recentFilesCountTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.loadLastProjectCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.initialDisplayComboBox = new System.Windows.Forms.ComboBox();
			this.clearResultsCheckBox = new System.Windows.Forms.CheckBox();
			this.saveVisualStateCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.visualStudioSupportCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.shadowCopyCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.recentFilesCountTextBox);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.loadLastProjectCheckBox);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(288, 100);
			this.groupBox1.TabIndex = 30;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Recent Files";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(144, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 24);
			this.label3.TabIndex = 26;
			this.label3.Text = "files in list";
			// 
			// recentFilesCountTextBox
			// 
			this.recentFilesCountTextBox.Location = new System.Drawing.Point(86, 24);
			this.recentFilesCountTextBox.Name = "recentFilesCountTextBox";
			this.recentFilesCountTextBox.Size = new System.Drawing.Size(40, 22);
			this.recentFilesCountTextBox.TabIndex = 25;
			this.recentFilesCountTextBox.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(19, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 16);
			this.label2.TabIndex = 24;
			this.label2.Text = "Display";
			// 
			// loadLastProjectCheckBox
			// 
			this.loadLastProjectCheckBox.Location = new System.Drawing.Point(19, 64);
			this.loadLastProjectCheckBox.Name = "loadLastProjectCheckBox";
			this.loadLastProjectCheckBox.Size = new System.Drawing.Size(250, 24);
			this.loadLastProjectCheckBox.TabIndex = 27;
			this.loadLastProjectCheckBox.Text = "Load most recent project at startup.";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.initialDisplayComboBox);
			this.groupBox2.Controls.Add(this.clearResultsCheckBox);
			this.groupBox2.Controls.Add(this.saveVisualStateCheckBox);
			this.groupBox2.Location = new System.Drawing.Point(8, 112);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(288, 112);
			this.groupBox2.TabIndex = 31;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Tree View";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(20, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 24);
			this.label1.TabIndex = 28;
			this.label1.Text = "Initial display on load:";
			// 
			// initialDisplayComboBox
			// 
			this.initialDisplayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.initialDisplayComboBox.ItemHeight = 16;
			this.initialDisplayComboBox.Items.AddRange(new object[] {
																		"Auto",
																		"Expand",
																		"Collapse",
																		"HideTests"});
			this.initialDisplayComboBox.Location = new System.Drawing.Point(172, 24);
			this.initialDisplayComboBox.Name = "initialDisplayComboBox";
			this.initialDisplayComboBox.Size = new System.Drawing.Size(87, 24);
			this.initialDisplayComboBox.TabIndex = 29;
			// 
			// clearResultsCheckBox
			// 
			this.clearResultsCheckBox.Location = new System.Drawing.Point(20, 48);
			this.clearResultsCheckBox.Name = "clearResultsCheckBox";
			this.clearResultsCheckBox.Size = new System.Drawing.Size(232, 24);
			this.clearResultsCheckBox.TabIndex = 30;
			this.clearResultsCheckBox.Text = "Clear results when reloading.";
			// 
			// saveVisualStateCheckBox
			// 
			this.saveVisualStateCheckBox.Location = new System.Drawing.Point(20, 80);
			this.saveVisualStateCheckBox.Name = "saveVisualStateCheckBox";
			this.saveVisualStateCheckBox.Size = new System.Drawing.Size(248, 24);
			this.saveVisualStateCheckBox.TabIndex = 31;
			this.saveVisualStateCheckBox.Text = "Save Visual State of each project";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.visualStudioSupportCheckBox);
			this.groupBox3.Location = new System.Drawing.Point(8, 232);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(288, 48);
			this.groupBox3.TabIndex = 32;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Visual Studio";
			// 
			// visualStudioSupportCheckBox
			// 
			this.visualStudioSupportCheckBox.Location = new System.Drawing.Point(16, 16);
			this.visualStudioSupportCheckBox.Name = "visualStudioSupportCheckBox";
			this.visualStudioSupportCheckBox.Size = new System.Drawing.Size(224, 25);
			this.visualStudioSupportCheckBox.TabIndex = 29;
			this.visualStudioSupportCheckBox.Text = "Enable Visual Studio Support";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.shadowCopyCheckBox);
			this.groupBox4.Location = new System.Drawing.Point(8, 288);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(288, 48);
			this.groupBox4.TabIndex = 33;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Shadow Copy";
			// 
			// shadowCopyCheckBox
			// 
			this.shadowCopyCheckBox.Location = new System.Drawing.Point(16, 16);
			this.shadowCopyCheckBox.Name = "shadowCopyCheckBox";
			this.shadowCopyCheckBox.Size = new System.Drawing.Size(240, 22);
			this.shadowCopyCheckBox.TabIndex = 30;
			this.shadowCopyCheckBox.Text = "Enable Shadow Copy";
			// 
			// GeneralSettingsTab
			// 
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "GeneralSettingsTab";
			this.Size = new System.Drawing.Size(310, 344);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public override void LoadSettings()
		{
			this.settings = Services.UserSettings;

			recentFilesCountTextBox.Text = Services.RecentFiles.MaxFiles.ToString();
			loadLastProjectCheckBox.Checked = settings.GetSetting( "Options.LoadLastProject", true );

			initialDisplayComboBox.SelectedIndex = (int)(TestSuiteTreeView.DisplayStyle)settings.GetSetting( "Gui.TestTree.InitialTreeDisplay", TestSuiteTreeView.DisplayStyle.Auto );
			clearResultsCheckBox.Checked = settings.GetSetting( "Options.TestLoader.ClearResultsOnReload", true );
			saveVisualStateCheckBox.Checked = settings.GetSetting( "Gui.TestTree.SaveVisualState", true );

			shadowCopyCheckBox.Checked = settings.GetSetting( "Options.TestLoader.ShadowCopyFiles", true );

			visualStudioSupportCheckBox.Checked = settings.GetSetting( "Options.TestLoader.VisualStudioSupport", false );
		}

		public override void ApplySettings()
		{
			settings.SaveSetting( "Options.LoadLastProject", loadLastProjectCheckBox.Checked );

			settings.SaveSetting( "Gui.TestTree.InitialTreeDisplay", (TestSuiteTreeView.DisplayStyle)initialDisplayComboBox.SelectedIndex );
			settings.SaveSetting( "Options.TestLoader.ClearResultsOnReload", clearResultsCheckBox.Checked );
			settings.SaveSetting( "Gui.TestTree.SaveVisualState", saveVisualStateCheckBox.Checked );
		
			settings.SaveSetting( "Options.TestLoader.VisualStudioSupport", visualStudioSupportCheckBox.Checked );

			settings.SaveSetting( "Options.TestLoader.ShadowCopyFiles", Services.TestLoader.ShadowCopyFiles = shadowCopyCheckBox.Checked );
		}

		public override bool HasChangesRequiringReload
		{
			get
			{
				return settings.GetSetting( "Options.TestLoader.ShadowCopyFiles", true ) != shadowCopyCheckBox.Checked;
			}
		}

		private void recentFilesCountTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( recentFilesCountTextBox.Text.Length == 0 )
			{
				recentFilesCountTextBox.Text = Services.RecentFiles.MaxFiles.ToString();
				recentFilesCountTextBox.SelectAll();
				e.Cancel = true;
			}
			else
			{
				string errmsg = null;

				try
				{
					int count = int.Parse( recentFilesCountTextBox.Text );

					if ( count < RecentFilesService.MinSize ||
						count > RecentFilesService.MaxSize )
					{
						errmsg = string.Format( "Number of files must be from {0} to {1}", 
							RecentFilesService.MinSize, RecentFilesService.MaxSize );
					}
				}
				catch
				{
					errmsg = "Number of files must be numeric";
				}

				if ( errmsg != null )
				{
					recentFilesCountTextBox.SelectAll();
					UserMessage.DisplayFailure( errmsg );
					e.Cancel = true;
				}
			}
		}

		private void recentFilesCountTextBox_Validated(object sender, System.EventArgs e)
		{
			int count = int.Parse( recentFilesCountTextBox.Text );
			Services.RecentFiles.MaxFiles = count;
		}
	}
}
