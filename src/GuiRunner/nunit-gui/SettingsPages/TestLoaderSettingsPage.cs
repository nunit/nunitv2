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
		private System.Windows.Forms.CheckBox mergeAssembliesCheckBox;
		private System.Windows.Forms.RadioButton singleDomainRadioButton;
		private System.Windows.Forms.RadioButton multiDomainRadioButton;
		private System.Windows.Forms.RadioButton flatTestList;
		private System.Windows.Forms.RadioButton autoNamespaceSuites;
		private System.Windows.Forms.HelpProvider helpProvider1;
        private Label label3;
        private GroupBox groupBox3;
        private RadioButton multiProcessRadioButton;
        private RadioButton separateProcessRadioButton;
        private RadioButton sameProcessRadioButton;
        private Label label2;
        private GroupBox groupBox2;
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
            this.mergeAssembliesCheckBox = new System.Windows.Forms.CheckBox();
            this.singleDomainRadioButton = new System.Windows.Forms.RadioButton();
            this.multiDomainRadioButton = new System.Windows.Forms.RadioButton();
            this.flatTestList = new System.Windows.Forms.RadioButton();
            this.autoNamespaceSuites = new System.Windows.Forms.RadioButton();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.multiProcessRadioButton = new System.Windows.Forms.RadioButton();
            this.separateProcessRadioButton = new System.Windows.Forms.RadioButton();
            this.sameProcessRadioButton = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            this.label1.Size = new System.Drawing.Size(104, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "Test Structure";
            // 
            // mergeAssembliesCheckBox
            // 
            this.helpProvider1.SetHelpString(this.mergeAssembliesCheckBox, "If checked, tests in each assembly will be merged into a single tree.");
            this.mergeAssembliesCheckBox.Location = new System.Drawing.Point(48, 299);
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
            this.singleDomainRadioButton.Location = new System.Drawing.Point(32, 267);
            this.singleDomainRadioButton.Name = "singleDomainRadioButton";
            this.helpProvider1.SetShowHelp(this.singleDomainRadioButton, true);
            this.singleDomainRadioButton.Size = new System.Drawing.Size(272, 24);
            this.singleDomainRadioButton.TabIndex = 9;
            this.singleDomainRadioButton.TabStop = true;
            this.singleDomainRadioButton.Text = "Use a single AppDomain for all tests";
            this.singleDomainRadioButton.Click += new System.EventHandler(this.toggleMultiDomain);
            // 
            // multiDomainRadioButton
            // 
            this.multiDomainRadioButton.AutoCheck = false;
            this.helpProvider1.SetHelpString(this.multiDomainRadioButton, "If selected, each test assembly will be loaded in a separate AppDomain.");
            this.multiDomainRadioButton.Location = new System.Drawing.Point(32, 235);
            this.multiDomainRadioButton.Name = "multiDomainRadioButton";
            this.helpProvider1.SetShowHelp(this.multiDomainRadioButton, true);
            this.multiDomainRadioButton.Size = new System.Drawing.Size(302, 24);
            this.multiDomainRadioButton.TabIndex = 8;
            this.multiDomainRadioButton.Text = "Use a separate AppDomain per Assembly";
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
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 18);
            this.label3.TabIndex = 35;
            this.label3.Text = "Test Processes";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(116, 93);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(340, 8);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            // 
            // multiProcessRadioButton
            // 
            this.multiProcessRadioButton.Enabled = false;
            this.multiProcessRadioButton.Location = new System.Drawing.Point(32, 181);
            this.multiProcessRadioButton.Name = "multiProcessRadioButton";
            this.multiProcessRadioButton.Size = new System.Drawing.Size(319, 21);
            this.multiProcessRadioButton.TabIndex = 36;
            this.multiProcessRadioButton.Text = "Run tests in a separate process per Assembly";
            this.multiProcessRadioButton.CheckedChanged += new System.EventHandler(this.toggleProcessUsage);
            // 
            // separateProcessRadioButton
            // 
            this.separateProcessRadioButton.Location = new System.Drawing.Point(32, 151);
            this.separateProcessRadioButton.Name = "separateProcessRadioButton";
            this.separateProcessRadioButton.Size = new System.Drawing.Size(271, 21);
            this.separateProcessRadioButton.TabIndex = 37;
            this.separateProcessRadioButton.Text = "Run tests in a single separate process";
            this.separateProcessRadioButton.CheckedChanged += new System.EventHandler(this.toggleProcessUsage);
            // 
            // sameProcessRadioButton
            // 
            this.sameProcessRadioButton.Checked = true;
            this.sameProcessRadioButton.Location = new System.Drawing.Point(33, 121);
            this.sameProcessRadioButton.Name = "sameProcessRadioButton";
            this.sameProcessRadioButton.Size = new System.Drawing.Size(270, 21);
            this.sameProcessRadioButton.TabIndex = 38;
            this.sameProcessRadioButton.TabStop = true;
            this.sameProcessRadioButton.Text = "Run tests directly in the NUnit process";
            this.sameProcessRadioButton.CheckedChanged += new System.EventHandler(this.toggleProcessUsage);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 214);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 18);
            this.label2.TabIndex = 40;
            this.label2.Text = "Test Domains";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(116, 214);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(340, 8);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            // 
            // TestLoaderSettingsPage
            // 
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.sameProcessRadioButton);
            this.Controls.Add(this.separateProcessRadioButton);
            this.Controls.Add(this.multiProcessRadioButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.flatTestList);
            this.Controls.Add(this.autoNamespaceSuites);
            this.Controls.Add(this.mergeAssembliesCheckBox);
            this.Controls.Add(this.singleDomainRadioButton);
            this.Controls.Add(this.multiDomainRadioButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "TestLoaderSettingsPage";
            this.Size = new System.Drawing.Size(456, 341);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		
		public override void LoadSettings()
		{
            bool separateProcess = settings.GetSetting("Options.TestLoader.SeparateProcess", false);
            sameProcessRadioButton.Checked = !separateProcess;
            separateProcessRadioButton.Checked = separateProcess;

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

            settings.SaveSetting("Options.TestLoader.SeparateProcess", loader.SeparateProcess = separateProcessRadioButton.Checked);
 
			settings.SaveSetting( "Options.TestLoader.MultiDomain", loader.MultiDomain = multiDomainRadioButton.Checked );
			settings.SaveSetting( "Options.TestLoader.MergeAssemblies", loader.MergeAssemblies = mergeAssembliesCheckBox.Checked );
			settings.SaveSetting( "Options.TestLoader.AutoNamespaceSuites", loader.AutoNamespaceSuites = autoNamespaceSuites.Checked );
		}

		private void toggleTestStructure(object sender, System.EventArgs e)
		{
			bool auto = autoNamespaceSuites.Checked = !autoNamespaceSuites.Checked;
			flatTestList.Checked = !auto;
		}


        private void toggleProcessUsage(object sender, EventArgs e)
        {
            bool sameProcess = sameProcessRadioButton.Checked;
            singleDomainRadioButton.Enabled = sameProcess;
            multiDomainRadioButton.Enabled = sameProcess;
            mergeAssembliesCheckBox.Enabled = sameProcess;
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
                    settings.GetSetting( "Options.TestLoader.SeparateProcess", false ) != separateProcessRadioButton.Checked ||
					settings.GetSetting( "Options.TestLoader.MultiDomain", false ) != multiDomainRadioButton.Checked ||
					settings.GetSetting( "Options.TestLoader.MergeAssemblies", false ) != mergeAssembliesCheckBox.Checked ||
					settings.GetSetting( "Options.TestLoader.AutoNamespaceSuites", true ) != autoNamespaceSuites.Checked;
			}
		}
	}
}

