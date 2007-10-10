using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Util;

namespace NUnit.Gui.SettingsPages
{
	public class TestLoaderSettingsTab : NUnit.UiKit.SettingsPage
	{
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.RadioButton flatTestList;
		private System.Windows.Forms.RadioButton autoNamespaceSuites;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.CheckBox mergeAssembliesCheckBox;
		private System.Windows.Forms.RadioButton singleDomainRadioButton;
		private System.Windows.Forms.RadioButton multiDomainRadioButton;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox rerunOnChangeCheckBox;
		private System.Windows.Forms.CheckBox reloadOnRunCheckBox;
		private System.Windows.Forms.CheckBox reloadOnChangeCheckBox;
		private System.ComponentModel.IContainer components = null;

		public TestLoaderSettingsTab(string key) : base( key )
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
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.flatTestList = new System.Windows.Forms.RadioButton();
			this.autoNamespaceSuites = new System.Windows.Forms.RadioButton();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.mergeAssembliesCheckBox = new System.Windows.Forms.CheckBox();
			this.singleDomainRadioButton = new System.Windows.Forms.RadioButton();
			this.multiDomainRadioButton = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rerunOnChangeCheckBox = new System.Windows.Forms.CheckBox();
			this.reloadOnRunCheckBox = new System.Windows.Forms.CheckBox();
			this.reloadOnChangeCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox7.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox7
			// 
			this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox7.Controls.Add(this.flatTestList);
			this.groupBox7.Controls.Add(this.autoNamespaceSuites);
			this.groupBox7.Location = new System.Drawing.Point(11, 16);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(288, 80);
			this.groupBox7.TabIndex = 3;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Test Structure";
			// 
			// flatTestList
			// 
			this.flatTestList.Location = new System.Drawing.Point(24, 48);
			this.flatTestList.Name = "flatTestList";
			this.flatTestList.Size = new System.Drawing.Size(216, 24);
			this.flatTestList.TabIndex = 1;
			this.flatTestList.Text = "Flat list of TestFixtures";
			// 
			// autoNamespaceSuites
			// 
			this.autoNamespaceSuites.Location = new System.Drawing.Point(24, 16);
			this.autoNamespaceSuites.Name = "autoNamespaceSuites";
			this.autoNamespaceSuites.Size = new System.Drawing.Size(224, 24);
			this.autoNamespaceSuites.TabIndex = 0;
			this.autoNamespaceSuites.Text = "Automatic Namespace suites";
			// 
			// groupBox6
			// 
			this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox6.Controls.Add(this.mergeAssembliesCheckBox);
			this.groupBox6.Controls.Add(this.singleDomainRadioButton);
			this.groupBox6.Controls.Add(this.multiDomainRadioButton);
			this.groupBox6.Location = new System.Drawing.Point(11, 104);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(288, 104);
			this.groupBox6.TabIndex = 4;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Multiple Assemblies";
			// 
			// mergeAssembliesCheckBox
			// 
			this.mergeAssembliesCheckBox.Location = new System.Drawing.Point(40, 72);
			this.mergeAssembliesCheckBox.Name = "mergeAssembliesCheckBox";
			this.mergeAssembliesCheckBox.Size = new System.Drawing.Size(224, 24);
			this.mergeAssembliesCheckBox.TabIndex = 2;
			this.mergeAssembliesCheckBox.Text = "Merge tests across assemblies";
			// 
			// singleDomainRadioButton
			// 
			this.singleDomainRadioButton.Checked = true;
			this.singleDomainRadioButton.Location = new System.Drawing.Point(24, 48);
			this.singleDomainRadioButton.Name = "singleDomainRadioButton";
			this.singleDomainRadioButton.Size = new System.Drawing.Size(240, 24);
			this.singleDomainRadioButton.TabIndex = 1;
			this.singleDomainRadioButton.TabStop = true;
			this.singleDomainRadioButton.Text = "Load in a single AppDomain";
			// 
			// multiDomainRadioButton
			// 
			this.multiDomainRadioButton.Location = new System.Drawing.Point(24, 24);
			this.multiDomainRadioButton.Name = "multiDomainRadioButton";
			this.multiDomainRadioButton.Size = new System.Drawing.Size(240, 24);
			this.multiDomainRadioButton.TabIndex = 0;
			this.multiDomainRadioButton.Text = "Load in separate AppDomains";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.rerunOnChangeCheckBox);
			this.groupBox2.Controls.Add(this.reloadOnRunCheckBox);
			this.groupBox2.Controls.Add(this.reloadOnChangeCheckBox);
			this.groupBox2.Location = new System.Drawing.Point(13, 216);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(288, 120);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Assembly Reload";
			// 
			// rerunOnChangeCheckBox
			// 
			this.rerunOnChangeCheckBox.Enabled = false;
			this.rerunOnChangeCheckBox.Location = new System.Drawing.Point(40, 80);
			this.rerunOnChangeCheckBox.Name = "rerunOnChangeCheckBox";
			this.rerunOnChangeCheckBox.Size = new System.Drawing.Size(200, 24);
			this.rerunOnChangeCheckBox.TabIndex = 10;
			this.rerunOnChangeCheckBox.Text = "Re-run last tests run";
			// 
			// reloadOnRunCheckBox
			// 
			this.reloadOnRunCheckBox.Location = new System.Drawing.Point(19, 28);
			this.reloadOnRunCheckBox.Name = "reloadOnRunCheckBox";
			this.reloadOnRunCheckBox.Size = new System.Drawing.Size(237, 23);
			this.reloadOnRunCheckBox.TabIndex = 8;
			this.reloadOnRunCheckBox.Text = "Reload before each test run";
			// 
			// reloadOnChangeCheckBox
			// 
			this.reloadOnChangeCheckBox.Location = new System.Drawing.Point(19, 55);
			this.reloadOnChangeCheckBox.Name = "reloadOnChangeCheckBox";
			this.reloadOnChangeCheckBox.Size = new System.Drawing.Size(245, 25);
			this.reloadOnChangeCheckBox.TabIndex = 9;
			this.reloadOnChangeCheckBox.Text = "Reload when test assembly changes";
			// 
			// TestLoaderSettingsTab
			// 
			this.Controls.Add(this.groupBox7);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox2);
			this.Name = "TestLoaderSettingsTab";
			this.Size = new System.Drawing.Size(310, 344);
			this.groupBox7.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public override void LoadSettings()
		{
			reloadOnChangeCheckBox.Enabled = Environment.OSVersion.Platform == System.PlatformID.Win32NT;
			reloadOnChangeCheckBox.Checked = settings.GetSetting( "Options.TestLoader.ReloadOnChange", true );
			rerunOnChangeCheckBox.Checked = settings.GetSetting( "Options.TestLoader.RerunOnChange", false );
			reloadOnRunCheckBox.Checked = settings.GetSetting( "Options.TestLoader.ReloadOnRun", true );

			bool multiDomain = settings.GetSetting( "Options.TestLoader.MultiDomain", false );
			multiDomainRadioButton.Checked = multiDomain;
			singleDomainRadioButton.Checked = !multiDomain;
			mergeAssembliesCheckBox.Enabled = !multiDomain;
			mergeAssembliesCheckBox.Checked = settings.GetSetting( "Options.TestLoader.MergeAssemblies", false );
			autoNamespaceSuites.Checked = settings.GetSetting( "Options.TestLoader.AutoNamespaceSuites", true );
			flatTestList.Checked = !autoNamespaceSuites.Checked;
		}

		public override void ApplySettings()
		{
			TestLoader loader = Services.TestLoader;

			settings.SaveSetting( "Options.TestLoader.ReloadOnChange", loader.ReloadOnChange = reloadOnChangeCheckBox.Checked );
			settings.SaveSetting( "Options.TestLoader.RerunOnChange", loader.RerunOnChange = rerunOnChangeCheckBox.Checked );
			settings.SaveSetting( "Options.TestLoader.ReloadOnRun", loader.ReloadOnRun = reloadOnRunCheckBox.Checked );

			settings.SaveSetting( "Options.TestLoader.MultiDomain", loader.MultiDomain = multiDomainRadioButton.Checked );
			settings.SaveSetting( "Options.TestLoader.MergeAssemblies", loader.MergeAssemblies = mergeAssembliesCheckBox.Checked );
			settings.SaveSetting( "Options.TestLoader.AutoNamespaceSuites", loader.AutoNamespaceSuites = autoNamespaceSuites.Checked );
		}

		public override bool HasChangesRequiringReload
		{
			get 
			{
				return 
					settings.GetSetting( "Options.TestLoader.ReloadOnChange", true ) != reloadOnChangeCheckBox.Checked ||
					settings.GetSetting( "Options.TestLoader.MultiDomain", false ) != multiDomainRadioButton.Checked ||
					settings.GetSetting( "Options.TestLoader.MergeAssemblies", false ) != mergeAssembliesCheckBox.Checked ||
					settings.GetSetting( "Options.TestLoader.AutoNamespaceSuites", true ) != autoNamespaceSuites.Checked;
			}
		}

		private void singleDomainRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			mergeAssembliesCheckBox.Enabled = singleDomainRadioButton.Checked;
		}

		private void reloadOnChangeCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			rerunOnChangeCheckBox.Enabled = reloadOnChangeCheckBox.Checked;
			rerunOnChangeCheckBox.Checked = false;
		}

	}
}

