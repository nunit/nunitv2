// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NUnit.Util;
using NUnit.UiKit;
using CP.Windows.Forms;
using CP.Windows.Shell;

namespace NUnit.Gui
{
	public class ProjectEditor : System.Windows.Forms.Form
	{
		#region Instance Variables

		private NUnitProject project;
		private ProjectConfig selectedConfig;
		private string selectedAssembly;
		private System.Windows.Forms.ColumnHeader fileNameHeader;
		private System.Windows.Forms.ColumnHeader fullPathHeader;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.Label label5;
		private CP.Windows.Forms.ExpandingLabel projectPathLabel;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox projectBaseTextBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TabControl projectTabControl;
		private System.Windows.Forms.TabPage generalTabPage;
		private System.Windows.Forms.RadioButton autoBinPathRadioButton;
		private System.Windows.Forms.RadioButton manualBinPathRadioButton;
		private System.Windows.Forms.RadioButton noBinPathRadioButton;
		private System.Windows.Forms.TextBox privateBinPathTextBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox configFileTextBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox applicationBaseTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TabPage assemblyTabPage;
		private System.Windows.Forms.ListBox assemblyListBox;
		private System.Windows.Forms.Button addAssemblyButton;
		private System.Windows.Forms.Button editConfigsButton;
		private System.Windows.Forms.ComboBox configComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button configBaseBrowseButton;
		private System.Windows.Forms.Button projectBaseBrowseButton;
		private System.Windows.Forms.Button removeAssemblyButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox assemblyPathTextBox;
		private System.Windows.Forms.Button assemblyPathBrowseButton;
		private System.ComponentModel.IContainer components = null;

		#endregion

		#region Construction and Disposal

		public ProjectEditor( NUnitProject project )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.project = project;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ProjectEditor));
			this.fileNameHeader = new System.Windows.Forms.ColumnHeader();
			this.fullPathHeader = new System.Windows.Forms.ColumnHeader();
			this.closeButton = new System.Windows.Forms.Button();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.label5 = new System.Windows.Forms.Label();
			this.projectPathLabel = new CP.Windows.Forms.ExpandingLabel();
			this.label8 = new System.Windows.Forms.Label();
			this.projectBaseTextBox = new System.Windows.Forms.TextBox();
			this.projectTabControl = new System.Windows.Forms.TabControl();
			this.generalTabPage = new System.Windows.Forms.TabPage();
			this.autoBinPathRadioButton = new System.Windows.Forms.RadioButton();
			this.manualBinPathRadioButton = new System.Windows.Forms.RadioButton();
			this.noBinPathRadioButton = new System.Windows.Forms.RadioButton();
			this.configBaseBrowseButton = new System.Windows.Forms.Button();
			this.privateBinPathTextBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.configFileTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.applicationBaseTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.assemblyTabPage = new System.Windows.Forms.TabPage();
			this.assemblyPathTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.assemblyListBox = new System.Windows.Forms.ListBox();
			this.addAssemblyButton = new System.Windows.Forms.Button();
			this.removeAssemblyButton = new System.Windows.Forms.Button();
			this.editConfigsButton = new System.Windows.Forms.Button();
			this.configComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.projectBaseBrowseButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.assemblyPathBrowseButton = new System.Windows.Forms.Button();
			this.projectTabControl.SuspendLayout();
			this.generalTabPage.SuspendLayout();
			this.assemblyTabPage.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// fileNameHeader
			// 
			this.fileNameHeader.Text = "File Name";
			this.fileNameHeader.Width = 100;
			// 
			// fullPathHeader
			// 
			this.fullPathHeader.Text = "Full Path";
			this.fullPathHeader.Width = 256;
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.closeButton.Location = new System.Drawing.Point(408, 415);
			this.closeButton.Name = "closeButton";
			this.helpProvider1.SetShowHelp(this.closeButton, false);
			this.closeButton.Size = new System.Drawing.Size(104, 23);
			this.closeButton.TabIndex = 6;
			this.closeButton.Text = "Close";
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// label5
			// 
			this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label5.Location = new System.Drawing.Point(24, 8);
			this.label5.Name = "label5";
			this.helpProvider1.SetShowHelp(this.label5, false);
			this.label5.Size = new System.Drawing.Size(88, 16);
			this.label5.TabIndex = 0;
			this.label5.Text = "Project Path:";
			// 
			// projectPathLabel
			// 
			this.projectPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.projectPathLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.projectPathLabel.Location = new System.Drawing.Point(128, 8);
			this.projectPathLabel.Name = "projectPathLabel";
			this.helpProvider1.SetShowHelp(this.projectPathLabel, false);
			this.projectPathLabel.Size = new System.Drawing.Size(394, 16);
			this.projectPathLabel.TabIndex = 1;
			// 
			// label8
			// 
			this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label8.Location = new System.Drawing.Point(24, 37);
			this.label8.Name = "label8";
			this.helpProvider1.SetShowHelp(this.label8, false);
			this.label8.Size = new System.Drawing.Size(120, 16);
			this.label8.TabIndex = 7;
			this.label8.Text = "Project Base:";
			// 
			// projectBaseTextBox
			// 
			this.helpProvider1.SetHelpString(this.projectBaseTextBox, "The ApplicationBase for the project. Defaults to the location of the project file" +
				".");
			this.projectBaseTextBox.Location = new System.Drawing.Point(125, 37);
			this.projectBaseTextBox.Name = "projectBaseTextBox";
			this.helpProvider1.SetShowHelp(this.projectBaseTextBox, true);
			this.projectBaseTextBox.Size = new System.Drawing.Size(339, 22);
			this.projectBaseTextBox.TabIndex = 8;
			this.projectBaseTextBox.Text = "";
			this.projectBaseTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.projectBaseTextBox_Validating);
			this.projectBaseTextBox.Validated += new System.EventHandler(this.projectBaseTextBox_Validated);
			// 
			// projectTabControl
			// 
			this.projectTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.projectTabControl.Controls.Add(this.generalTabPage);
			this.projectTabControl.Controls.Add(this.assemblyTabPage);
			this.projectTabControl.ItemSize = new System.Drawing.Size(49, 18);
			this.projectTabControl.Location = new System.Drawing.Point(8, 64);
			this.projectTabControl.Name = "projectTabControl";
			this.projectTabControl.SelectedIndex = 0;
			this.helpProvider1.SetShowHelp(this.projectTabControl, false);
			this.projectTabControl.Size = new System.Drawing.Size(488, 264);
			this.projectTabControl.TabIndex = 9;
			// 
			// generalTabPage
			// 
			this.generalTabPage.Controls.Add(this.autoBinPathRadioButton);
			this.generalTabPage.Controls.Add(this.manualBinPathRadioButton);
			this.generalTabPage.Controls.Add(this.noBinPathRadioButton);
			this.generalTabPage.Controls.Add(this.configBaseBrowseButton);
			this.generalTabPage.Controls.Add(this.privateBinPathTextBox);
			this.generalTabPage.Controls.Add(this.label6);
			this.generalTabPage.Controls.Add(this.configFileTextBox);
			this.generalTabPage.Controls.Add(this.label4);
			this.generalTabPage.Controls.Add(this.applicationBaseTextBox);
			this.generalTabPage.Controls.Add(this.label3);
			this.generalTabPage.Location = new System.Drawing.Point(4, 22);
			this.generalTabPage.Name = "generalTabPage";
			this.helpProvider1.SetShowHelp(this.generalTabPage, false);
			this.generalTabPage.Size = new System.Drawing.Size(480, 238);
			this.generalTabPage.TabIndex = 0;
			this.generalTabPage.Text = "General";
			// 
			// autoBinPathRadioButton
			// 
			this.autoBinPathRadioButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.autoBinPathRadioButton.Location = new System.Drawing.Point(32, 96);
			this.autoBinPathRadioButton.Name = "autoBinPathRadioButton";
			this.helpProvider1.SetShowHelp(this.autoBinPathRadioButton, false);
			this.autoBinPathRadioButton.Size = new System.Drawing.Size(328, 24);
			this.autoBinPathRadioButton.TabIndex = 10;
			this.autoBinPathRadioButton.Text = "Use automatically generated path";
			this.autoBinPathRadioButton.CheckedChanged += new System.EventHandler(this.autoBinPathRadioButton_CheckedChanged);
			// 
			// manualBinPathRadioButton
			// 
			this.manualBinPathRadioButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.manualBinPathRadioButton.Location = new System.Drawing.Point(32, 128);
			this.manualBinPathRadioButton.Name = "manualBinPathRadioButton";
			this.helpProvider1.SetShowHelp(this.manualBinPathRadioButton, false);
			this.manualBinPathRadioButton.TabIndex = 9;
			this.manualBinPathRadioButton.Text = "Use this path:";
			this.manualBinPathRadioButton.CheckedChanged += new System.EventHandler(this.manualBinPathRadioButton_CheckedChanged);
			// 
			// noBinPathRadioButton
			// 
			this.noBinPathRadioButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.noBinPathRadioButton.Location = new System.Drawing.Point(32, 160);
			this.noBinPathRadioButton.Name = "noBinPathRadioButton";
			this.helpProvider1.SetShowHelp(this.noBinPathRadioButton, false);
			this.noBinPathRadioButton.Size = new System.Drawing.Size(424, 24);
			this.noBinPathRadioButton.TabIndex = 8;
			this.noBinPathRadioButton.Text = "None - or specified in Configuration File";
			this.noBinPathRadioButton.CheckedChanged += new System.EventHandler(this.noBinPathRadioButton_CheckedChanged);
			// 
			// configBaseBrowseButton
			// 
			this.configBaseBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.configBaseBrowseButton, "Browse to locate ApplicationBase directory.");
			this.configBaseBrowseButton.Image = ((System.Drawing.Image)(resources.GetObject("configBaseBrowseButton.Image")));
			this.configBaseBrowseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.configBaseBrowseButton.Location = new System.Drawing.Point(448, 8);
			this.configBaseBrowseButton.Name = "configBaseBrowseButton";
			this.helpProvider1.SetShowHelp(this.configBaseBrowseButton, true);
			this.configBaseBrowseButton.Size = new System.Drawing.Size(24, 24);
			this.configBaseBrowseButton.TabIndex = 7;
			this.configBaseBrowseButton.Click += new System.EventHandler(this.configBaseBrowseButton_Click);
			// 
			// privateBinPathTextBox
			// 
			this.privateBinPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.privateBinPathTextBox, "Path searched when probing for private asemblies. Directories must be descendants" +
				" of the ApplicationBase.");
			this.privateBinPathTextBox.Location = new System.Drawing.Point(136, 128);
			this.privateBinPathTextBox.Name = "privateBinPathTextBox";
			this.helpProvider1.SetShowHelp(this.privateBinPathTextBox, true);
			this.privateBinPathTextBox.Size = new System.Drawing.Size(336, 22);
			this.privateBinPathTextBox.TabIndex = 5;
			this.privateBinPathTextBox.Text = "";
			this.privateBinPathTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.privateBinPathTextBox_Validating);
			this.privateBinPathTextBox.Validated += new System.EventHandler(this.privateBinPathTextBox_Validated);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label6.Location = new System.Drawing.Point(16, 72);
			this.label6.Name = "label6";
			this.helpProvider1.SetShowHelp(this.label6, false);
			this.label6.Size = new System.Drawing.Size(97, 18);
			this.label6.TabIndex = 4;
			this.label6.Text = "PrivateBinPath:";
			// 
			// configFileTextBox
			// 
			this.configFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.configFileTextBox, "Configuration file to use when loading assemblies if it exists. Defaults to <proj" +
				"ectname>.config. Must be located in the ApplicationBase directory.");
			this.configFileTextBox.Location = new System.Drawing.Point(176, 40);
			this.configFileTextBox.Name = "configFileTextBox";
			this.helpProvider1.SetShowHelp(this.configFileTextBox, true);
			this.configFileTextBox.Size = new System.Drawing.Size(296, 22);
			this.configFileTextBox.TabIndex = 3;
			this.configFileTextBox.Text = "";
			this.configFileTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.configFileTextBox_Validating);
			this.configFileTextBox.Validated += new System.EventHandler(this.configFileTextBox_Validated);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label4.Location = new System.Drawing.Point(16, 40);
			this.label4.Name = "label4";
			this.helpProvider1.SetShowHelp(this.label4, false);
			this.label4.Size = new System.Drawing.Size(153, 18);
			this.label4.TabIndex = 2;
			this.label4.Text = "Configuration File Name:";
			// 
			// applicationBaseTextBox
			// 
			this.applicationBaseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.applicationBaseTextBox, "The ApplicationBase for this configuration. May be absolute or relative to the pr" +
				"oject base. Defaults to the project base if not set.");
			this.applicationBaseTextBox.Location = new System.Drawing.Point(128, 8);
			this.applicationBaseTextBox.Name = "applicationBaseTextBox";
			this.helpProvider1.SetShowHelp(this.applicationBaseTextBox, true);
			this.applicationBaseTextBox.Size = new System.Drawing.Size(304, 22);
			this.applicationBaseTextBox.TabIndex = 1;
			this.applicationBaseTextBox.Text = "";
			this.applicationBaseTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.applicationBaseTextBox_Validating);
			this.applicationBaseTextBox.Validated += new System.EventHandler(this.applicationBaseTextBox_Validated);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(16, 8);
			this.label3.Name = "label3";
			this.helpProvider1.SetShowHelp(this.label3, false);
			this.label3.Size = new System.Drawing.Size(105, 18);
			this.label3.TabIndex = 0;
			this.label3.Text = "ApplicationBase:";
			// 
			// assemblyTabPage
			// 
			this.assemblyTabPage.Controls.Add(this.assemblyPathBrowseButton);
			this.assemblyTabPage.Controls.Add(this.assemblyPathTextBox);
			this.assemblyTabPage.Controls.Add(this.label2);
			this.assemblyTabPage.Controls.Add(this.assemblyListBox);
			this.assemblyTabPage.Controls.Add(this.addAssemblyButton);
			this.assemblyTabPage.Controls.Add(this.removeAssemblyButton);
			this.assemblyTabPage.Location = new System.Drawing.Point(4, 22);
			this.assemblyTabPage.Name = "assemblyTabPage";
			this.helpProvider1.SetShowHelp(this.assemblyTabPage, false);
			this.assemblyTabPage.Size = new System.Drawing.Size(480, 238);
			this.assemblyTabPage.TabIndex = 1;
			this.assemblyTabPage.Text = "Assemblies";
			this.assemblyTabPage.Visible = false;
			// 
			// assemblyPathTextBox
			// 
			this.assemblyPathTextBox.Location = new System.Drawing.Point(16, 168);
			this.assemblyPathTextBox.Name = "assemblyPathTextBox";
			this.assemblyPathTextBox.Size = new System.Drawing.Size(416, 22);
			this.assemblyPathTextBox.TabIndex = 8;
			this.assemblyPathTextBox.Text = "";
			this.assemblyPathTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.assemblyPathTextBox_Validating);
			this.assemblyPathTextBox.Validated += new System.EventHandler(this.assemblyPathTextBox_Validated);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 144);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Assembly Path:";
			// 
			// assemblyListBox
			// 
			this.helpProvider1.SetHelpString(this.assemblyListBox, "Checked assemblies will have tests loaded in the UI. Tests (if any) in unchecked " +
				"assemblies will not be loaded. All listed assemblies are watched for changes and" +
				" used in determining the PrivateBinPath.");
			this.assemblyListBox.ItemHeight = 16;
			this.assemblyListBox.Location = new System.Drawing.Point(16, 16);
			this.assemblyListBox.Name = "assemblyListBox";
			this.helpProvider1.SetShowHelp(this.assemblyListBox, true);
			this.assemblyListBox.Size = new System.Drawing.Size(368, 116);
			this.assemblyListBox.TabIndex = 6;
			this.assemblyListBox.SelectedIndexChanged += new System.EventHandler(this.assemblyListBox_SelectedIndexChanged);
			// 
			// addAssemblyButton
			// 
			this.addAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.addAssemblyButton, "Add an assembly to this configuration.");
			this.addAssemblyButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.addAssemblyButton.Location = new System.Drawing.Point(392, 24);
			this.addAssemblyButton.Name = "addAssemblyButton";
			this.helpProvider1.SetShowHelp(this.addAssemblyButton, true);
			this.addAssemblyButton.Size = new System.Drawing.Size(80, 23);
			this.addAssemblyButton.TabIndex = 2;
			this.addAssemblyButton.Text = "&Add...";
			this.addAssemblyButton.Click += new System.EventHandler(this.addAssemblyButton_Click);
			// 
			// removeAssemblyButton
			// 
			this.removeAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.removeAssemblyButton, "Remove the selected assembly from the configuration.");
			this.removeAssemblyButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.removeAssemblyButton.Location = new System.Drawing.Point(392, 56);
			this.removeAssemblyButton.Name = "removeAssemblyButton";
			this.helpProvider1.SetShowHelp(this.removeAssemblyButton, true);
			this.removeAssemblyButton.Size = new System.Drawing.Size(80, 23);
			this.removeAssemblyButton.TabIndex = 5;
			this.removeAssemblyButton.Text = "&Remove";
			this.removeAssemblyButton.Click += new System.EventHandler(this.removeAssemblyButton_Click);
			// 
			// editConfigsButton
			// 
			this.editConfigsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.editConfigsButton, "Add, remove or rename configurations.");
			this.editConfigsButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.editConfigsButton.Location = new System.Drawing.Point(385, 24);
			this.editConfigsButton.Name = "editConfigsButton";
			this.helpProvider1.SetShowHelp(this.editConfigsButton, true);
			this.editConfigsButton.Size = new System.Drawing.Size(104, 23);
			this.editConfigsButton.TabIndex = 8;
			this.editConfigsButton.Text = "&Edit Configs...";
			this.editConfigsButton.Click += new System.EventHandler(this.editConfigsButton_Click);
			// 
			// configComboBox
			// 
			this.configComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.configComboBox, "Select the configuration to edit");
			this.configComboBox.ItemHeight = 16;
			this.configComboBox.Location = new System.Drawing.Point(120, 24);
			this.configComboBox.Name = "configComboBox";
			this.helpProvider1.SetShowHelp(this.configComboBox, true);
			this.configComboBox.Size = new System.Drawing.Size(256, 24);
			this.configComboBox.TabIndex = 7;
			this.configComboBox.SelectedIndexChanged += new System.EventHandler(this.configComboBox_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(16, 32);
			this.label1.Name = "label1";
			this.helpProvider1.SetShowHelp(this.label1, false);
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Configuration:";
			// 
			// projectBaseBrowseButton
			// 
			this.projectBaseBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.projectBaseBrowseButton, "Browse to locate ApplicationBase directory.");
			this.projectBaseBrowseButton.Image = ((System.Drawing.Image)(resources.GetObject("projectBaseBrowseButton.Image")));
			this.projectBaseBrowseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.projectBaseBrowseButton.Location = new System.Drawing.Point(472, 40);
			this.projectBaseBrowseButton.Name = "projectBaseBrowseButton";
			this.helpProvider1.SetShowHelp(this.projectBaseBrowseButton, true);
			this.projectBaseBrowseButton.Size = new System.Drawing.Size(24, 24);
			this.projectBaseBrowseButton.TabIndex = 10;
			this.projectBaseBrowseButton.Click += new System.EventHandler(this.projectBaseBrowseButton_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox1.Controls.Add(this.projectTabControl);
			this.groupBox1.Controls.Add(this.editConfigsButton);
			this.groupBox1.Controls.Add(this.configComboBox);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(16, 72);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(505, 336);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Configuration Properties";
			// 
			// assemblyPathBrowseButton
			// 
			this.assemblyPathBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.assemblyPathBrowseButton, "Browse to locate ApplicationBase directory.");
			this.assemblyPathBrowseButton.Image = ((System.Drawing.Image)(resources.GetObject("assemblyPathBrowseButton.Image")));
			this.assemblyPathBrowseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.assemblyPathBrowseButton.Location = new System.Drawing.Point(448, 168);
			this.assemblyPathBrowseButton.Name = "assemblyPathBrowseButton";
			this.helpProvider1.SetShowHelp(this.assemblyPathBrowseButton, true);
			this.assemblyPathBrowseButton.Size = new System.Drawing.Size(24, 24);
			this.assemblyPathBrowseButton.TabIndex = 11;
			this.assemblyPathBrowseButton.Click += new System.EventHandler(this.assemblyPathBrowseButton_Click);
			// 
			// ProjectEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(537, 442);
			this.Controls.Add(this.projectBaseBrowseButton);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.projectBaseTextBox);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.projectPathLabel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.closeButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(376, 400);
			this.Name = "ProjectEditor";
			this.helpProvider1.SetShowHelp(this, false);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NUnit Test Project Editor";
			this.TransparencyKey = System.Drawing.Color.Green;
			this.Load += new System.EventHandler(this.ProjectEditor_Load);
			this.projectTabControl.ResumeLayout(false);
			this.generalTabPage.ResumeLayout(false);
			this.assemblyTabPage.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties
		private bool visualStudioSupport;
		public bool VisualStudioSupport
		{
			get { return visualStudioSupport; }
			set { visualStudioSupport = value; }
		}
		#endregion

		#region Config ComboBox Methods and Events

		private void configComboBox_Populate()
		{
			configComboBox.Items.Clear();

			if ( selectedConfig == null )
				selectedConfig = project.ActiveConfig;

			int selectedIndex = -1; 
			foreach( ProjectConfig config in project.Configs )
			{
				string name = config.Name;
				int index = configComboBox.Items.Add( name );
				if ( name == selectedConfig.Name )
					selectedIndex = index;
			}

			if ( selectedIndex == -1 && configComboBox.Items.Count > 0 )
			{
				selectedIndex = 0;
				selectedConfig = project.Configs[0];
			}

			if ( selectedIndex == -1 )
				selectedConfig = null;
			else
				configComboBox.SelectedIndex = selectedIndex;
		
			addAssemblyButton.Enabled = removeAssemblyButton.Enabled = project.Configs.Count > 0;
		}

		private void configComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			selectedConfig = project.Configs[(string)configComboBox.SelectedItem];
			
			applicationBaseTextBox.Text = selectedConfig.RelativeBasePath;

			configFileTextBox.Text = selectedConfig.ConfigurationFile;

			switch ( selectedConfig.BinPathType )
			{
				case BinPathType.Auto:
					autoBinPathRadioButton.Checked = true;
					break;

				case BinPathType.Manual:
					manualBinPathRadioButton.Checked = true;
					privateBinPathTextBox.Text = selectedConfig.PrivateBinPath;
					break;

				default:
					noBinPathRadioButton.Checked = true;
					break;
			}

			assemblyListBox_Populate();
		}

		#endregion

		#region Assembly ListBox Methods and Events

		private void assemblyListBox_Populate()
		{
			assemblyListBox.Items.Clear();
			int selectedIndex = -1;

			foreach( string assembly in selectedConfig.Assemblies )
			{
				int index = assemblyListBox.Items.Add( Path.GetFileName( assembly ) );

				if ( assembly == selectedAssembly )
					selectedIndex = index;
			}

			if ( assemblyListBox.Items.Count > 0 && selectedIndex == -1)
				selectedIndex = 0;
				
			if ( selectedIndex != -1 )
				assemblyListBox.SelectedIndex = selectedIndex;
		}

		private void assemblyListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( assemblyListBox.SelectedIndex == -1 )
			{
				assemblyPathTextBox.Text = selectedAssembly = null;
				removeAssemblyButton.Enabled = false;
			}
			else 
			{
				assemblyPathTextBox.Text = selectedAssembly = //(string)assemblyListBox.SelectedItem;
					project.ActiveConfig.Assemblies[assemblyListBox.SelectedIndex];
				removeAssemblyButton.Enabled = true;
			}
		}

		#endregion

		#region Project Base Methods and Events
		private void projectBaseBrowseButton_Click(object sender, System.EventArgs e)
		{
			FolderBrowser browser = new FolderBrowser( this );
			browser.Caption = "Project Editor";
			browser.Title = string.Format( "Select ApplicationBase for the project", selectedConfig.Name );
			browser.InitialSelection = project.BasePath;
			string projectBase = browser.BrowseForFolder();
			if ( projectBase != null && projectBase != project.BasePath )
				UpdateProjectBase( projectBase );
		}

		private void projectBaseTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( projectBaseTextBox.Text != string.Empty )
			{
				string projectBase = null;
				try
				{
					projectBase = projectBaseTextBox.Text;
					Directory.Exists( projectBase );
				}
				catch( Exception ex )
				{
					projectBaseTextBox.SelectAll();
					UserMessage.DisplayFailure( ex, "Invalid Entry" );
					e.Cancel = true;
				}

				if ( !Directory.Exists( projectBase ) )
				{
					string msg = string.Format( 
						"The directory {0} does not exist. Do you want to create it?", 
						projectBase );
					switch ( UserMessage.Ask( msg, "Project Editor" ) )
					{
						case DialogResult.Yes:
							Directory.CreateDirectory( projectBase );
							break;
						case DialogResult.Cancel:
							e.Cancel = true;
							break;
						case DialogResult.No:
						default:
							break;
					}
				}
			}
		}

		private void projectBaseTextBox_Validated(object sender, System.EventArgs e)
		{
			UpdateProjectBase( projectBaseTextBox.Text );
		}

		private void UpdateProjectBase( string projectBase )
		{
			if ( projectBase == string.Empty )
				projectBase = project.DefaultBasePath;

			project.BasePath = projectBaseTextBox.Text = projectBase;
		}
		#endregion

		#region Config Base Methods and Events
		private void configBaseBrowseButton_Click(object sender, System.EventArgs e)
		{
			FolderBrowser browser = new FolderBrowser( this, project.BasePath );
			browser.Caption = "Project Editor";
			browser.Title = string.Format( "Select ApplicationBase for the {0} configuration", selectedConfig.Name );
			browser.InitialSelection = selectedConfig.BasePath;
			string appbase = browser.BrowseForFolder();
			if ( appbase != null && appbase != selectedConfig.BasePath )
				UpdateApplicationBase( appbase );
		}

		private void applicationBaseTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( applicationBaseTextBox.Text != String.Empty )
			{
				string applicationBase = null;

				try
				{
					applicationBase = Path.Combine( project.BasePath, applicationBaseTextBox.Text );
					Directory.Exists( applicationBase );
				}
				catch( Exception exception )
				{
					applicationBaseTextBox.SelectAll();
					UserMessage.DisplayFailure( exception, "Invalid Entry" );
					e.Cancel = true;
				}

	/*			if ( !PathUtils.SamePathOrUnder( project.BasePath, applicationBase ) )
				{
					applicationBaseTextBox.SelectAll();
					UserMessage.DisplayFailure( "Path must be equal to or under the project base", "Invalid Entry" );
					e.Cancel = true;
				}			
				else */
				if ( !Directory.Exists( applicationBase ) )
				{
					string msg = string.Format( 
						"The directory {0} does not exist. Do you want to create it?", 
						applicationBase );
					switch ( UserMessage.Ask( msg, "Project Editor" ) )
					{
						case DialogResult.Yes:
							Directory.CreateDirectory( applicationBase );
							break;
						case DialogResult.Cancel:
							e.Cancel = true;
							break;
						case DialogResult.No:
						default:
							break;
					}
				}
			}
		}

		private void applicationBaseTextBox_Validated(object sender, System.EventArgs e)
		{
			UpdateApplicationBase( applicationBaseTextBox.Text );
		}

		private void UpdateApplicationBase( string appbase )
		{
			string basePath = null;

			if ( appbase != String.Empty )
			{
				basePath = Path.Combine( project.BasePath, appbase );
				if ( PathUtils.SamePath( project.BasePath, basePath ) )
					basePath = null;
			}

			selectedConfig.BasePath = basePath;

			// TODO: Test what happens if we set it the same as project base
			if ( selectedConfig.RelativeBasePath == null )
				applicationBaseTextBox.Text = string.Empty;
			else
				applicationBaseTextBox.Text = selectedConfig.RelativeBasePath;
		}
		#endregion

		#region Config File Methods and Events
		private void configFileTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string configFile = configFileTextBox.Text;
			if ( configFile != String.Empty )
			{
				try
				{
					File.Open( Path.Combine( selectedConfig.BasePath, configFile ), FileMode.Open );
				}
				catch( System.Exception exception )
				{
					configFileTextBox.SelectAll();
					UserMessage.DisplayFailure( exception, "Invalid Entry" );
					e.Cancel = true;
				}

				if ( configFile != Path.GetFileName( configFile ) )
				{
					configFileTextBox.SelectAll();
					UserMessage.DisplayFailure( "Specify configuration file as filename and extension only", "Invalid Entry" );
					e.Cancel = true;
				}
			}
		}

		private void configFileTextBox_Validated(object sender, System.EventArgs e)
		{
			if ( configFileTextBox.Text == String.Empty )
				selectedConfig.ConfigurationFile = null;
			else
				selectedConfig.ConfigurationFile = configFileTextBox.Text;
		}
		#endregion

		#region PrivateBinPath Methods and Events
		private void privateBinPathTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string binPath = privateBinPathTextBox.Text;
			if ( binPath != String.Empty )
			{
				string[] elements = binPath.Split( new char[] { ';' } );
				foreach( string element in elements ) 
				{
					try
					{
						Directory.Exists( element );
					}
					catch(System.Exception exception)
					{
						privateBinPathTextBox.Select( binPath.IndexOf( element ), element.Length );
						UserMessage.DisplayFailure( exception, "Invalid Entry" );
						e.Cancel = true;
					}
				}
			}
		}

		private void privateBinPathTextBox_Validated(object sender, System.EventArgs e)
		{
			if ( privateBinPathTextBox.Text == String.Empty )
				selectedConfig.PrivateBinPath = null;
			else
				selectedConfig.PrivateBinPath = privateBinPathTextBox.Text;
		}
		#endregion

		#region Assembly Path Methods and Events
		private void assemblyPathBrowseButton_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "Select Assembly";
			
			dlg.Filter =
				"Assemblies (*.dll,*.exe)|*.dll;*.exe|" +
				"All Files (*.*)|*.*";

			dlg.InitialDirectory = System.IO.Path.GetDirectoryName( assemblyPathTextBox.Text );
			dlg.FilterIndex = 1;
			dlg.FileName = "";

			if ( dlg.ShowDialog( this ) == DialogResult.OK ) 
			{
				selectedConfig.Assemblies[assemblyListBox.SelectedIndex] = dlg.FileName;
				assemblyListBox_Populate();
			}
		}

		private void assemblyPathTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string path = assemblyPathTextBox.Text;

			try
			{
				FileInfo info = new FileInfo( path );

				if ( !info.Exists )
				{
					DialogResult answer = UserMessage.Ask( string.Format( 
						"The path {0} does not exist. Do you want to use it anyway?", path ) );
					if ( answer != DialogResult.Yes )
						e.Cancel = true;
				}
			}
			catch( System.Exception exception )
			{
				assemblyPathTextBox.SelectAll();
				UserMessage.DisplayFailure( exception, "Invalid Entry" );
				e.Cancel = true;
			}		
		}

		private void assemblyPathTextBox_Validated(object sender, System.EventArgs e)
		{
			selectedConfig.Assemblies[assemblyListBox.SelectedIndex] = assemblyPathTextBox.Text;
			assemblyListBox_Populate();
		}
		#endregion

		#region Other UI Events

		private void ProjectEditor_Load(object sender, System.EventArgs e)
		{
			this.Text = string.Format( "{0} - Project Editor", 
				project.Name );

			projectPathLabel.Text = project.ProjectPath;

			projectBaseTextBox.Text = project.BasePath;

			configComboBox_Populate();
		}

		private void editConfigsButton_Click(object sender, System.EventArgs e)
		{
			using( ConfigurationEditor editor = new ConfigurationEditor( project ) )
			{
				this.Site.Container.Add( editor );
				editor.ShowDialog();
			}
			configComboBox_Populate();
		}

		private void addAssemblyButton_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.AddToProject( this, selectedConfig.Name );
			assemblyListBox_Populate();
		}

		private void removeAssemblyButton_Click(object sender, System.EventArgs e)
		{
			if ( UserMessage.Ask( string.Format(
				"Remove {0} from project?", selectedAssembly ) ) == DialogResult.Yes )
			{
				int index = assemblyListBox.SelectedIndex;
				selectedConfig.Assemblies.RemoveAt( index );
				assemblyListBox.Items.RemoveAt( index );
				if ( index >= assemblyListBox.Items.Count )
					--index;

				if ( index >= 0 )
				{
					selectedAssembly = (string)assemblyListBox.Items[index];
					assemblyListBox.SelectedIndex = index;
				}
				else
					selectedAssembly = null;
			}
		}

		private void closeButton_Click(object sender, System.EventArgs e)
		{
			this.Close();		
		}

		private void autoBinPathRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( autoBinPathRadioButton.Checked )
			{
				selectedConfig.BinPathType = BinPathType.Auto;
				privateBinPathTextBox.Enabled = false;
			}
		}

		private void manualBinPathRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( manualBinPathRadioButton.Checked )
			{
				selectedConfig.BinPathType = BinPathType.Manual;
				privateBinPathTextBox.Enabled = true;
			}
		}

		private void noBinPathRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( noBinPathRadioButton.Checked )
			{
				selectedConfig.BinPathType = BinPathType.None;
				privateBinPathTextBox.Enabled = false;
			}
		}
		#endregion
	}
}
