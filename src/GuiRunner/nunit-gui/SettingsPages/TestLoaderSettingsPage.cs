using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Util;

namespace NUnit.Gui.SettingsPages
{
	public class TestLoaderSettingsPage : NUnit.UiKit.SettingsPage
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox mergeAssembliesCheckBox;
		private System.Windows.Forms.RadioButton singleDomainRadioButton;
		private System.Windows.Forms.RadioButton multiDomainRadioButton;
		private System.Windows.Forms.RadioButton flatTestList;
		private System.Windows.Forms.RadioButton autoNamespaceSuites;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.ComponentModel.IContainer components = null;

		public TestLoaderSettingsPage(string key) : base(key)
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.mergeAssembliesCheckBox = new System.Windows.Forms.CheckBox();
			this.singleDomainRadioButton = new System.Windows.Forms.RadioButton();
			this.multiDomainRadioButton = new System.Windows.Forms.RadioButton();
			this.flatTestList = new System.Windows.Forms.RadioButton();
			this.autoNamespaceSuites = new System.Windows.Forms.RadioButton();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(104, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(352, 8);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Test Structure";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Multiple Assemblies";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(144, 96);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(312, 8);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			// 
			// mergeAssembliesCheckBox
			// 
			this.helpProvider1.SetHelpString(this.mergeAssembliesCheckBox, "If checked, tests in each assembly will be merged into a single tree.");
			this.mergeAssembliesCheckBox.Location = new System.Drawing.Point(48, 184);
			this.mergeAssembliesCheckBox.Name = "mergeAssembliesCheckBox";
			this.helpProvider1.SetShowHelp(this.mergeAssembliesCheckBox, true);
			this.mergeAssembliesCheckBox.Size = new System.Drawing.Size(224, 24);
			this.mergeAssembliesCheckBox.TabIndex = 10;
			this.mergeAssembliesCheckBox.Text = "Merge tests across assemblies";
			// 
			// singleDomainRadioButton
			// 
			this.singleDomainRadioButton.AutoCheck = false;
			this.singleDomainRadioButton.Checked = true;
			this.helpProvider1.SetHelpString(this.singleDomainRadioButton, "If selected, all test assemblies will be loaded in the same AppDomain.");
			this.singleDomainRadioButton.Location = new System.Drawing.Point(32, 152);
			this.singleDomainRadioButton.Name = "singleDomainRadioButton";
			this.helpProvider1.SetShowHelp(this.singleDomainRadioButton, true);
			this.singleDomainRadioButton.Size = new System.Drawing.Size(240, 24);
			this.singleDomainRadioButton.TabIndex = 9;
			this.singleDomainRadioButton.TabStop = true;
			this.singleDomainRadioButton.Text = "Load in a single AppDomain";
			this.singleDomainRadioButton.Click += new System.EventHandler(this.toggleMultiDomain);
			// 
			// multiDomainRadioButton
			// 
			this.multiDomainRadioButton.AutoCheck = false;
			this.helpProvider1.SetHelpString(this.multiDomainRadioButton, "If selected, each test assembly will be loaded in a separate AppDomain.");
			this.multiDomainRadioButton.Location = new System.Drawing.Point(32, 120);
			this.multiDomainRadioButton.Name = "multiDomainRadioButton";
			this.helpProvider1.SetShowHelp(this.multiDomainRadioButton, true);
			this.multiDomainRadioButton.Size = new System.Drawing.Size(240, 24);
			this.multiDomainRadioButton.TabIndex = 8;
			this.multiDomainRadioButton.Text = "Load in separate AppDomains";
			this.multiDomainRadioButton.Click += new System.EventHandler(this.toggleMultiDomain);
			// 
			// flatTestList
			// 
			this.flatTestList.AutoCheck = false;
			this.helpProvider1.SetHelpString(this.flatTestList, "If selected, the tree will consist of a flat list of fixtures, without any higher" +
				"-level structure beyond the assemblies.");
			this.flatTestList.Location = new System.Drawing.Point(32, 56);
			this.flatTestList.Name = "flatTestList";
			this.helpProvider1.SetShowHelp(this.flatTestList, true);
			this.flatTestList.Size = new System.Drawing.Size(216, 24);
			this.flatTestList.TabIndex = 33;
			this.flatTestList.Text = "Flat list of TestFixtures";
			this.flatTestList.Click += new System.EventHandler(this.toggleTestStructure);
			// 
			// autoNamespaceSuites
			// 
			this.autoNamespaceSuites.AutoCheck = false;
			this.autoNamespaceSuites.Checked = true;
			this.helpProvider1.SetHelpString(this.autoNamespaceSuites, "If selected, the tree will follow the namespace structure of the tests, with suit" +
				"es automatically created at each level.");
			this.autoNamespaceSuites.Location = new System.Drawing.Point(32, 24);
			this.autoNamespaceSuites.Name = "autoNamespaceSuites";
			this.helpProvider1.SetShowHelp(this.autoNamespaceSuites, true);
			this.autoNamespaceSuites.Size = new System.Drawing.Size(224, 24);
			this.autoNamespaceSuites.TabIndex = 32;
			this.autoNamespaceSuites.TabStop = true;
			this.autoNamespaceSuites.Text = "Automatic Namespace suites";
			this.autoNamespaceSuites.Click += new System.EventHandler(this.toggleTestStructure);
			// 
			// TestLoaderSettingsPage
			// 
			this.Controls.Add(this.flatTestList);
			this.Controls.Add(this.autoNamespaceSuites);
			this.Controls.Add(this.mergeAssembliesCheckBox);
			this.Controls.Add(this.singleDomainRadioButton);
			this.Controls.Add(this.multiDomainRadioButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Name = "TestLoaderSettingsPage";
			this.Size = new System.Drawing.Size(456, 312);
			this.ResumeLayout(false);

		}
		#endregion
		
		public override void LoadSettings()
		{
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

			settings.SaveSetting( "Options.TestLoader.MultiDomain", loader.MultiDomain = multiDomainRadioButton.Checked );
			settings.SaveSetting( "Options.TestLoader.MergeAssemblies", loader.MergeAssemblies = mergeAssembliesCheckBox.Checked );
			settings.SaveSetting( "Options.TestLoader.AutoNamespaceSuites", loader.AutoNamespaceSuites = autoNamespaceSuites.Checked );
		}

		private void toggleTestStructure(object sender, System.EventArgs e)
		{
			bool auto = autoNamespaceSuites.Checked = !autoNamespaceSuites.Checked;
			flatTestList.Checked = !auto;
		}

		private void toggleMultiDomain(object sender, System.EventArgs e)
		{
			bool multiDomain = multiDomainRadioButton.Checked = ! multiDomainRadioButton.Checked;
			singleDomainRadioButton.Checked = !multiDomain;
			mergeAssembliesCheckBox.Enabled = !multiDomain;
		}

		public override bool HasChangesRequiringReload
		{
			get 
			{
				return 
					settings.GetSetting( "Options.TestLoader.MultiDomain", false ) != multiDomainRadioButton.Checked ||
					settings.GetSetting( "Options.TestLoader.MergeAssemblies", false ) != mergeAssembliesCheckBox.Checked ||
					settings.GetSetting( "Options.TestLoader.AutoNamespaceSuites", true ) != autoNamespaceSuites.Checked;
			}
		}
	}
}

