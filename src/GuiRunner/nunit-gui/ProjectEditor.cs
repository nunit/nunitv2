#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

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
	/// <summary>
	/// Summary description for ProjectEditor.
	/// </summary>
	public class ProjectEditor : System.Windows.Forms.Form
	{
		#region Instance Variables

		private NUnitProject project;
		private ProjectConfig selectedConfig;
		private string selectedAssembly;

		private System.Windows.Forms.Button editConfigsButton;
		private System.Windows.Forms.ComboBox configComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl projectTabControl;
		private System.Windows.Forms.ColumnHeader fileNameHeader;
		private System.Windows.Forms.ColumnHeader fullPathHeader;
		private System.Windows.Forms.TabPage generalTabPage;
		private System.Windows.Forms.TabPage assemblyTabPage;
		private System.Windows.Forms.Button addVSProjectButton;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button editAssemblyButton;
		private System.Windows.Forms.Button addAssemblyButton;
		private System.Windows.Forms.Button deleteAssemblyButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox applicationBaseTextBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox configFileTextBox;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.Label label5;
		private CP.Windows.Forms.ExpandingLabel projectPathLabel;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox privateBinPathTextBox;
		private System.Windows.Forms.Button browseBasePathButton;
		private NUnit.UiKit.AssemblyListBox assemblyListBox;
		private System.Windows.Forms.RadioButton noBinPathRadioButton;
		private System.Windows.Forms.RadioButton manualBinPathRadioButton;
		private System.Windows.Forms.RadioButton autoBinPathRadioButton;
		private System.Windows.Forms.Label label7;
		private System.ComponentModel.IContainer components;

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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ProjectEditor));
			this.editConfigsButton = new System.Windows.Forms.Button();
			this.configComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.projectTabControl = new System.Windows.Forms.TabControl();
			this.generalTabPage = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.autoBinPathRadioButton = new System.Windows.Forms.RadioButton();
			this.manualBinPathRadioButton = new System.Windows.Forms.RadioButton();
			this.noBinPathRadioButton = new System.Windows.Forms.RadioButton();
			this.browseBasePathButton = new System.Windows.Forms.Button();
			this.privateBinPathTextBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.configFileTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.applicationBaseTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.assemblyTabPage = new System.Windows.Forms.TabPage();
			this.assemblyListBox = new NUnit.UiKit.AssemblyListBox();
			this.addVSProjectButton = new System.Windows.Forms.Button();
			this.editAssemblyButton = new System.Windows.Forms.Button();
			this.addAssemblyButton = new System.Windows.Forms.Button();
			this.deleteAssemblyButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.fileNameHeader = new System.Windows.Forms.ColumnHeader();
			this.fullPathHeader = new System.Windows.Forms.ColumnHeader();
			this.closeButton = new System.Windows.Forms.Button();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.label5 = new System.Windows.Forms.Label();
			this.projectPathLabel = new CP.Windows.Forms.ExpandingLabel();
			this.projectTabControl.SuspendLayout();
			this.generalTabPage.SuspendLayout();
			this.assemblyTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// editConfigsButton
			// 
			this.editConfigsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.editConfigsButton, "Add, remove or rename configurations.");
			this.editConfigsButton.Location = new System.Drawing.Point(424, 32);
			this.editConfigsButton.Name = "editConfigsButton";
			this.helpProvider1.SetShowHelp(this.editConfigsButton, true);
			this.editConfigsButton.Size = new System.Drawing.Size(104, 23);
			this.editConfigsButton.TabIndex = 4;
			this.editConfigsButton.Text = "&Edit Configs...";
			this.editConfigsButton.Click += new System.EventHandler(this.editConfigsButton_Click);
			// 
			// configComboBox
			// 
			this.configComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.configComboBox, "Select the configuration to edit");
			this.configComboBox.Location = new System.Drawing.Point(120, 32);
			this.configComboBox.Name = "configComboBox";
			this.helpProvider1.SetShowHelp(this.configComboBox, true);
			this.configComboBox.Size = new System.Drawing.Size(296, 24);
			this.configComboBox.TabIndex = 3;
			this.configComboBox.SelectedIndexChanged += new System.EventHandler(this.configComboBox_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(23, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(89, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Select Config:";
			// 
			// projectTabControl
			// 
			this.projectTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.projectTabControl.Controls.Add(this.generalTabPage);
			this.projectTabControl.Controls.Add(this.assemblyTabPage);
			this.projectTabControl.Location = new System.Drawing.Point(16, 64);
			this.projectTabControl.Name = "projectTabControl";
			this.projectTabControl.SelectedIndex = 0;
			this.projectTabControl.Size = new System.Drawing.Size(512, 256);
			this.projectTabControl.TabIndex = 5;
			// 
			// generalTabPage
			// 
			this.generalTabPage.Controls.Add(this.label7);
			this.generalTabPage.Controls.Add(this.autoBinPathRadioButton);
			this.generalTabPage.Controls.Add(this.manualBinPathRadioButton);
			this.generalTabPage.Controls.Add(this.noBinPathRadioButton);
			this.generalTabPage.Controls.Add(this.browseBasePathButton);
			this.generalTabPage.Controls.Add(this.privateBinPathTextBox);
			this.generalTabPage.Controls.Add(this.label6);
			this.generalTabPage.Controls.Add(this.configFileTextBox);
			this.generalTabPage.Controls.Add(this.label4);
			this.generalTabPage.Controls.Add(this.applicationBaseTextBox);
			this.generalTabPage.Controls.Add(this.label3);
			this.generalTabPage.Location = new System.Drawing.Point(4, 25);
			this.generalTabPage.Name = "generalTabPage";
			this.generalTabPage.Size = new System.Drawing.Size(504, 227);
			this.generalTabPage.TabIndex = 0;
			this.generalTabPage.Text = "General";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(56, 160);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(40, 16);
			this.label7.TabIndex = 11;
			this.label7.Text = "Value:";
			// 
			// autoBinPathRadioButton
			// 
			this.autoBinPathRadioButton.Location = new System.Drawing.Point(32, 96);
			this.autoBinPathRadioButton.Name = "autoBinPathRadioButton";
			this.autoBinPathRadioButton.Size = new System.Drawing.Size(328, 24);
			this.autoBinPathRadioButton.TabIndex = 10;
			this.autoBinPathRadioButton.Text = "Generated automatically from assembly locations";
			this.autoBinPathRadioButton.CheckedChanged += new System.EventHandler(this.autoBinPathRadioButton_CheckedChanged);
			// 
			// manualBinPathRadioButton
			// 
			this.manualBinPathRadioButton.Location = new System.Drawing.Point(32, 128);
			this.manualBinPathRadioButton.Name = "manualBinPathRadioButton";
			this.manualBinPathRadioButton.Size = new System.Drawing.Size(424, 24);
			this.manualBinPathRadioButton.TabIndex = 9;
			this.manualBinPathRadioButton.Text = "Specified manually";
			this.manualBinPathRadioButton.CheckedChanged += new System.EventHandler(this.manualBinPathRadioButton_CheckedChanged);
			// 
			// noBinPathRadioButton
			// 
			this.noBinPathRadioButton.Location = new System.Drawing.Point(32, 192);
			this.noBinPathRadioButton.Name = "noBinPathRadioButton";
			this.noBinPathRadioButton.Size = new System.Drawing.Size(424, 24);
			this.noBinPathRadioButton.TabIndex = 8;
			this.noBinPathRadioButton.Text = "None - or specified in Configuration File";
			this.noBinPathRadioButton.CheckedChanged += new System.EventHandler(this.noBinPathRadioButton_CheckedChanged);
			// 
			// browseBasePathButton
			// 
			this.browseBasePathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.browseBasePathButton, "Browse to locate ApplicationBase directory.");
			this.browseBasePathButton.Image = ((System.Drawing.Image)(resources.GetObject("browseBasePathButton.Image")));
			this.browseBasePathButton.Location = new System.Drawing.Point(472, 8);
			this.browseBasePathButton.Name = "browseBasePathButton";
			this.helpProvider1.SetShowHelp(this.browseBasePathButton, true);
			this.browseBasePathButton.Size = new System.Drawing.Size(24, 24);
			this.browseBasePathButton.TabIndex = 7;
			this.browseBasePathButton.Click += new System.EventHandler(this.browseBasePathButton_Click);
			// 
			// privateBinPathTextBox
			// 
			this.privateBinPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.privateBinPathTextBox, "Path searched when probing for private asemblies. Directories must be descendants" +
				" of the ApplicationBase.");
			this.privateBinPathTextBox.Location = new System.Drawing.Point(104, 160);
			this.privateBinPathTextBox.Name = "privateBinPathTextBox";
			this.helpProvider1.SetShowHelp(this.privateBinPathTextBox, true);
			this.privateBinPathTextBox.Size = new System.Drawing.Size(384, 22);
			this.privateBinPathTextBox.TabIndex = 5;
			this.privateBinPathTextBox.Text = "";
			this.privateBinPathTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.privateBinPathTextBox_Validating);
			this.privateBinPathTextBox.Validated += new System.EventHandler(this.privateBinPathTextBox_Validated);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(16, 72);
			this.label6.Name = "label6";
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
			this.configFileTextBox.Location = new System.Drawing.Point(192, 40);
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
			this.label4.Location = new System.Drawing.Point(16, 40);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(153, 18);
			this.label4.TabIndex = 2;
			this.label4.Text = "Configuration File Name:";
			// 
			// applicationBaseTextBox
			// 
			this.applicationBaseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.applicationBaseTextBox, "The ApplicationBase for locating assemblies. Must be equal to or under the projec" +
				"t directory. Defaults to the project directory if not set. Used as current direc" +
				"tory when running tests.");
			this.applicationBaseTextBox.Location = new System.Drawing.Point(152, 8);
			this.applicationBaseTextBox.Name = "applicationBaseTextBox";
			this.helpProvider1.SetShowHelp(this.applicationBaseTextBox, true);
			this.applicationBaseTextBox.Size = new System.Drawing.Size(312, 22);
			this.applicationBaseTextBox.TabIndex = 1;
			this.applicationBaseTextBox.Text = "";
			this.applicationBaseTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.applicationBaseTextBox_Validating);
			this.applicationBaseTextBox.Validated += new System.EventHandler(this.applicationBaseTextBox_Validated);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(105, 18);
			this.label3.TabIndex = 0;
			this.label3.Text = "ApplicationBase:";
			// 
			// assemblyTabPage
			// 
			this.assemblyTabPage.Controls.Add(this.assemblyListBox);
			this.assemblyTabPage.Controls.Add(this.addVSProjectButton);
			this.assemblyTabPage.Controls.Add(this.editAssemblyButton);
			this.assemblyTabPage.Controls.Add(this.addAssemblyButton);
			this.assemblyTabPage.Controls.Add(this.deleteAssemblyButton);
			this.assemblyTabPage.Controls.Add(this.label2);
			this.assemblyTabPage.Location = new System.Drawing.Point(4, 25);
			this.assemblyTabPage.Name = "assemblyTabPage";
			this.assemblyTabPage.Size = new System.Drawing.Size(504, 227);
			this.assemblyTabPage.TabIndex = 1;
			this.assemblyTabPage.Text = "Assemblies";
			// 
			// assemblyListBox
			// 
			this.helpProvider1.SetHelpString(this.assemblyListBox, "Checked assemblies will have tests loaded in the UI. Tests (if any) in unchecked " +
				"assemblies will not be loaded. All listed assemblies are watched for changes and" +
				" used in determining the PrivateBinPath.");
			this.assemblyListBox.Location = new System.Drawing.Point(24, 32);
			this.assemblyListBox.Name = "assemblyListBox";
			this.helpProvider1.SetShowHelp(this.assemblyListBox, true);
			this.assemblyListBox.Size = new System.Drawing.Size(328, 174);
			this.assemblyListBox.TabIndex = 6;
			this.assemblyListBox.ThreeDCheckBoxes = true;
			this.assemblyListBox.SelectedIndexChanged += new System.EventHandler(this.assemblyListBox_SelectedIndexChanged);
			this.assemblyListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.assemblyListBox_ItemCheck);
			// 
			// addVSProjectButton
			// 
			this.addVSProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.addVSProjectButton, "Add the output assembly from a VS project to this project. Assembly will be added" +
				" to each configuration that exists in the VS project.");
			this.addVSProjectButton.Location = new System.Drawing.Point(368, 72);
			this.addVSProjectButton.Name = "addVSProjectButton";
			this.helpProvider1.SetShowHelp(this.addVSProjectButton, true);
			this.addVSProjectButton.Size = new System.Drawing.Size(128, 23);
			this.addVSProjectButton.TabIndex = 3;
			this.addVSProjectButton.Text = "Add &VS Project...";
			this.addVSProjectButton.Click += new System.EventHandler(this.addVSProjectButton_Click);
			// 
			// editAssemblyButton
			// 
			this.editAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.editAssemblyButton, "Modify the path to the selected assembly.");
			this.editAssemblyButton.Location = new System.Drawing.Point(368, 104);
			this.editAssemblyButton.Name = "editAssemblyButton";
			this.helpProvider1.SetShowHelp(this.editAssemblyButton, true);
			this.editAssemblyButton.Size = new System.Drawing.Size(128, 23);
			this.editAssemblyButton.TabIndex = 4;
			this.editAssemblyButton.Text = "&Edit Path...";
			this.editAssemblyButton.Click += new System.EventHandler(this.editAssemblyButton_Click);
			// 
			// addAssemblyButton
			// 
			this.addAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.addAssemblyButton, "Add an assembly to this configuration.");
			this.addAssemblyButton.Location = new System.Drawing.Point(368, 40);
			this.addAssemblyButton.Name = "addAssemblyButton";
			this.helpProvider1.SetShowHelp(this.addAssemblyButton, true);
			this.addAssemblyButton.Size = new System.Drawing.Size(128, 23);
			this.addAssemblyButton.TabIndex = 2;
			this.addAssemblyButton.Text = "&Add Assembly...";
			this.addAssemblyButton.Click += new System.EventHandler(this.addAssemblyButton_Click);
			// 
			// deleteAssemblyButton
			// 
			this.deleteAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpProvider1.SetHelpString(this.deleteAssemblyButton, "Remove the selected assembly from the configuration.");
			this.deleteAssemblyButton.Location = new System.Drawing.Point(368, 136);
			this.deleteAssemblyButton.Name = "deleteAssemblyButton";
			this.helpProvider1.SetShowHelp(this.deleteAssemblyButton, true);
			this.deleteAssemblyButton.Size = new System.Drawing.Size(128, 23);
			this.deleteAssemblyButton.TabIndex = 5;
			this.deleteAssemblyButton.Text = "&Remove Assembly";
			this.deleteAssemblyButton.Click += new System.EventHandler(this.deleteAssemblyButton_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(464, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Indicate assemblies with tests by checking the box.";
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
			this.closeButton.Location = new System.Drawing.Point(424, 328);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(104, 23);
			this.closeButton.TabIndex = 6;
			this.closeButton.Text = "Close";
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(24, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 16);
			this.label5.TabIndex = 0;
			this.label5.Text = "Project Path:";
			// 
			// projectPathLabel
			// 
			this.projectPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.projectPathLabel.Location = new System.Drawing.Point(128, 8);
			this.projectPathLabel.Name = "projectPathLabel";
			this.projectPathLabel.Size = new System.Drawing.Size(392, 16);
			this.projectPathLabel.TabIndex = 1;
			// 
			// ProjectEditor
			// 
			this.AcceptButton = this.closeButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.closeButton;
			this.ClientSize = new System.Drawing.Size(538, 374);
			this.Controls.Add(this.projectPathLabel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.projectTabControl);
			this.Controls.Add(this.editConfigsButton);
			this.Controls.Add(this.configComboBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.closeButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(376, 400);
			this.Name = "ProjectEditor";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NUnit Test Project Editor";
			this.TransparencyKey = System.Drawing.Color.Green;
			this.Load += new System.EventHandler(this.ProjectEditor_Load);
			this.projectTabControl.ResumeLayout(false);
			this.generalTabPage.ResumeLayout(false);
			this.assemblyTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Static Edit Method

		public static DialogResult Edit( NUnitProject project )
		{
			ProjectEditor editor = new ProjectEditor( project );
			return editor.ShowDialog();
		}

		#endregion

		#region Config ComboBox Methods and Events

		private void configComboBox_Populate()
		{
			configComboBox.Items.Clear();

			if ( selectedConfig == null )
				selectedConfig = project.ActiveConfig;

			int selectedIndex = -1; 
			foreach( string name in project.Configs.Names )
			{
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

			foreach( AssemblyListItem assembly in selectedConfig.Assemblies )
			{
				int index = assemblyListBox.Items.Add( 
					assembly.FullPath, 
					assembly.HasTests ? CheckState.Checked : CheckState.Unchecked );

				if ( assembly.FullPath == selectedAssembly )
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
				selectedAssembly = null;

				editAssemblyButton.Enabled = false;
				deleteAssemblyButton.Enabled = false;
			}
			else 
			{
				selectedAssembly = (string)assemblyListBox.SelectedItem;

				editAssemblyButton.Enabled = true;
				deleteAssemblyButton.Enabled = true;
			}
		}

		#endregion

		#region Validation Events

		private void applicationBaseTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( applicationBaseTextBox.Text != String.Empty )
			{
				string applicationBase = null;

				try
				{
					applicationBase = Path.Combine( project.BasePath, applicationBaseTextBox.Text );
					DirectoryInfo info = new DirectoryInfo( applicationBase );
				}
				catch( Exception exception )
				{
					applicationBaseTextBox.SelectAll();
					UserMessage.DisplayFailure( exception, "Invalid Entry" );
					e.Cancel = true;
				}

				if ( !ProjectPath.SamePathOrUnder( project.BasePath, applicationBase ) )
				{
					applicationBaseTextBox.SelectAll();
					UserMessage.DisplayFailure( "Path must be equal to or under the project base", "Invalid Entry" );
					e.Cancel = true;
				}			
				else if ( !Directory.Exists( applicationBase ) )
				{
					if ( UserMessage.Ask( "The directory {0} does not exist. Do you want to create it?", "Project Editor" ) == DialogResult.Yes )
						Directory.CreateDirectory( applicationBase );
					else
						e.Cancel = true;
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
				if ( ProjectPath.SamePath( project.BasePath, basePath ) )
					basePath = null;
			}

			selectedConfig.BasePath = basePath;

			// TODO: Test what happens if we set it the same as project base
			if ( selectedConfig.RelativeBasePath == null )
				applicationBaseTextBox.Text = string.Empty;
			else
				applicationBaseTextBox.Text = selectedConfig.RelativeBasePath;
		}

		private void configFileTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string configFile = configFileTextBox.Text;
			if ( configFile != String.Empty )
			{
				try
				{
					FileInfo info = new FileInfo( configFile );
				}
				catch( System.Exception exception )
				{
					configFileTextBox.SelectAll();
					UserMessage.DisplayFailure( exception, "Invalid Entry" );
					e.Cancel = true;
				}

				if ( configFile.IndexOfAny( new char[] { '\\', ':' } ) >= 0 )
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
						DirectoryInfo info = new DirectoryInfo( element );
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

		#region Other UI Events

		private void ProjectEditor_Load(object sender, System.EventArgs e)
		{
			this.Text = string.Format( "{0} - Project Editor", 
				project.Name );

			projectPathLabel.Text = project.ProjectPath;

			addVSProjectButton.Visible = UserSettings.Options.VisualStudioSupport;

			configComboBox_Populate();
		}

		private void editConfigsButton_Click(object sender, System.EventArgs e)
		{
			ConfigurationEditor.Edit( project );
			configComboBox_Populate();
		}

		private void addAssemblyButton_Click(object sender, System.EventArgs e)
		{
			AppUI.TestLoaderUI.AddAssembly( selectedConfig.Name );
			assemblyListBox_Populate();
		}

		private void addVSProjectButton_Click(object sender, System.EventArgs e)
		{
			AppUI.TestLoaderUI.AddVSProject( );
			assemblyListBox_Populate();
		}

		private void editAssemblyButton_Click(object sender, System.EventArgs e)
		{
			AssemblyPathDialog dlg = new AssemblyPathDialog( (string)selectedConfig.Assemblies[assemblyListBox.SelectedIndex].FullPath );
			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				selectedConfig.Assemblies[assemblyListBox.SelectedIndex].FullPath = dlg.Path;
				assemblyListBox_Populate();
			}
		}

		private void deleteAssemblyButton_Click(object sender, System.EventArgs e)
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

		private void closeButton_Click(object sender, System.EventArgs e)
		{
			this.Close();		
		}

		#endregion

		private void browseBasePathButton_Click(object sender, System.EventArgs e)
		{
			FolderBrowser browser = new FolderBrowser( this, project.BasePath );
			browser.Caption = "Project Editor";
			browser.Title = string.Format( "Select ApplicationBase for the {0} configuration", selectedConfig.Name );
			browser.InitialSelection = selectedConfig.BasePath;
			string appbase = browser.BrowseForFolder();
			if ( appbase != null && appbase != selectedConfig.BasePath )
				UpdateApplicationBase( appbase );
		}

		private void assemblyListBox_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			bool hasTests = (e.NewValue == CheckState.Checked);
			if ( hasTests != selectedConfig.Assemblies[e.Index].HasTests )
			{
				selectedConfig.Assemblies[e.Index].HasTests = hasTests;
			}
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
	}
}
