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

namespace NUnit.Gui
{
	/// <summary>
	/// Summary description for ProjectEditor.
	/// </summary>
	public class ProjectEditor : System.Windows.Forms.Form
	{
		#region Instance Variables

		private NUnitProject project;
		private string selectedConfig;
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
		private System.Windows.Forms.ListView assemblyListView;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox applicationBaseTextBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox configFileTextBox;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.Label label5;
		private NUnit.UiKit.ExpandingLabel projectPathLabel;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox privateBinPathTextBox;
		private ListViewItem hoverItem;
		private System.Windows.Forms.CheckBox autoBinPathCheckBox;
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
			this.editConfigsButton = new System.Windows.Forms.Button();
			this.configComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.projectTabControl = new System.Windows.Forms.TabControl();
			this.generalTabPage = new System.Windows.Forms.TabPage();
			this.autoBinPathCheckBox = new System.Windows.Forms.CheckBox();
			this.privateBinPathTextBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.configFileTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.applicationBaseTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.assemblyTabPage = new System.Windows.Forms.TabPage();
			this.addVSProjectButton = new System.Windows.Forms.Button();
			this.editAssemblyButton = new System.Windows.Forms.Button();
			this.addAssemblyButton = new System.Windows.Forms.Button();
			this.deleteAssemblyButton = new System.Windows.Forms.Button();
			this.assemblyListView = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.label2 = new System.Windows.Forms.Label();
			this.fileNameHeader = new System.Windows.Forms.ColumnHeader();
			this.fullPathHeader = new System.Windows.Forms.ColumnHeader();
			this.closeButton = new System.Windows.Forms.Button();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.label5 = new System.Windows.Forms.Label();
			this.projectPathLabel = new NUnit.UiKit.ExpandingLabel();
			this.projectTabControl.SuspendLayout();
			this.generalTabPage.SuspendLayout();
			this.assemblyTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// editConfigsButton
			// 
			this.editConfigsButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
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
			this.configComboBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
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
			this.projectTabControl.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.projectTabControl.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.generalTabPage,
																							this.assemblyTabPage});
			this.projectTabControl.Location = new System.Drawing.Point(16, 64);
			this.projectTabControl.Name = "projectTabControl";
			this.projectTabControl.SelectedIndex = 0;
			this.projectTabControl.Size = new System.Drawing.Size(512, 258);
			this.projectTabControl.TabIndex = 5;
			// 
			// generalTabPage
			// 
			this.generalTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.autoBinPathCheckBox,
																						 this.privateBinPathTextBox,
																						 this.label6,
																						 this.configFileTextBox,
																						 this.label4,
																						 this.applicationBaseTextBox,
																						 this.label3});
			this.generalTabPage.Location = new System.Drawing.Point(4, 25);
			this.generalTabPage.Name = "generalTabPage";
			this.generalTabPage.Size = new System.Drawing.Size(504, 229);
			this.generalTabPage.TabIndex = 0;
			this.generalTabPage.Text = "General";
			// 
			// autoBinPathCheckBox
			// 
			this.autoBinPathCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.autoBinPathCheckBox.Location = new System.Drawing.Point(32, 192);
			this.autoBinPathCheckBox.Name = "autoBinPathCheckBox";
			this.autoBinPathCheckBox.Size = new System.Drawing.Size(456, 24);
			this.autoBinPathCheckBox.TabIndex = 6;
			this.autoBinPathCheckBox.Text = "Automatically add directories containing listed assemblies";
			this.autoBinPathCheckBox.CheckedChanged += new System.EventHandler(this.autoBinPathCheckBox_CheckedChanged);
			// 
			// privateBinPathTextBox
			// 
			this.privateBinPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.helpProvider1.SetHelpString(this.privateBinPathTextBox, "Path searched when probing for private asemblies. Directories must be descendants" +
				" of the ApplicationBase.");
			this.privateBinPathTextBox.Location = new System.Drawing.Point(24, 152);
			this.privateBinPathTextBox.Name = "privateBinPathTextBox";
			this.helpProvider1.SetShowHelp(this.privateBinPathTextBox, true);
			this.privateBinPathTextBox.Size = new System.Drawing.Size(464, 22);
			this.privateBinPathTextBox.TabIndex = 5;
			this.privateBinPathTextBox.Text = "";
			this.privateBinPathTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.privateBinPathTextBox_Validating);
			this.privateBinPathTextBox.Validated += new System.EventHandler(this.privateBinPathTextBox_Validated);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 128);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(464, 16);
			this.label6.TabIndex = 4;
			this.label6.Text = "PrivateBinPath (paths should be under and relative to the Application Base):";
			// 
			// configFileTextBox
			// 
			this.configFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.helpProvider1.SetHelpString(this.configFileTextBox, "Configuration file to use when loading assemblies if it exists. Defaults to <proj" +
				"ectname>.config in the project directory.");
			this.configFileTextBox.Location = new System.Drawing.Point(24, 88);
			this.configFileTextBox.Name = "configFileTextBox";
			this.helpProvider1.SetShowHelp(this.configFileTextBox, true);
			this.configFileTextBox.Size = new System.Drawing.Size(464, 22);
			this.configFileTextBox.TabIndex = 3;
			this.configFileTextBox.Text = "";
			this.configFileTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.configFileTextBox_Validating);
			this.configFileTextBox.Validated += new System.EventHandler(this.configFileTextBox_Validated);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.label4.Location = new System.Drawing.Point(16, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(464, 24);
			this.label4.TabIndex = 2;
			this.label4.Text = "Configuration File (path may be absolute, or relative to the Project Path):";
			// 
			// applicationBaseTextBox
			// 
			this.applicationBaseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.helpProvider1.SetHelpString(this.applicationBaseTextBox, "The ApplicationBase for locating assemblies. Defaults to the project file directo" +
				"ry if not set. Used as current directory when running tests.");
			this.applicationBaseTextBox.Location = new System.Drawing.Point(24, 32);
			this.applicationBaseTextBox.Name = "applicationBaseTextBox";
			this.helpProvider1.SetShowHelp(this.applicationBaseTextBox, true);
			this.applicationBaseTextBox.Size = new System.Drawing.Size(464, 22);
			this.applicationBaseTextBox.TabIndex = 1;
			this.applicationBaseTextBox.Text = "";
			this.applicationBaseTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.applicationBaseTextBox_Validating);
			this.applicationBaseTextBox.Validated += new System.EventHandler(this.applicationBaseTextBox_Validated);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.label3.Location = new System.Drawing.Point(16, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(472, 24);
			this.label3.TabIndex = 0;
			this.label3.Text = "ApplicationBase (path may be absolute, or relative to the Project Path):";
			// 
			// assemblyTabPage
			// 
			this.assemblyTabPage.Controls.AddRange(new System.Windows.Forms.Control[] {
																						  this.addVSProjectButton,
																						  this.editAssemblyButton,
																						  this.addAssemblyButton,
																						  this.deleteAssemblyButton,
																						  this.assemblyListView,
																						  this.label2});
			this.assemblyTabPage.Location = new System.Drawing.Point(4, 25);
			this.assemblyTabPage.Name = "assemblyTabPage";
			this.assemblyTabPage.Size = new System.Drawing.Size(504, 229);
			this.assemblyTabPage.TabIndex = 1;
			this.assemblyTabPage.Text = "Assemblies";
			// 
			// addVSProjectButton
			// 
			this.addVSProjectButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.addVSProjectButton.Location = new System.Drawing.Point(368, 80);
			this.addVSProjectButton.Name = "addVSProjectButton";
			this.addVSProjectButton.Size = new System.Drawing.Size(128, 23);
			this.addVSProjectButton.TabIndex = 3;
			this.addVSProjectButton.Text = "Add &VS Project...";
			this.addVSProjectButton.Click += new System.EventHandler(this.addVSProjectButton_Click);
			// 
			// editAssemblyButton
			// 
			this.editAssemblyButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.editAssemblyButton.Location = new System.Drawing.Point(368, 112);
			this.editAssemblyButton.Name = "editAssemblyButton";
			this.editAssemblyButton.Size = new System.Drawing.Size(128, 23);
			this.editAssemblyButton.TabIndex = 4;
			this.editAssemblyButton.Text = "&Edit Path...";
			this.editAssemblyButton.Click += new System.EventHandler(this.editAssemblyButton_Click);
			// 
			// addAssemblyButton
			// 
			this.addAssemblyButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.addAssemblyButton.Location = new System.Drawing.Point(368, 48);
			this.addAssemblyButton.Name = "addAssemblyButton";
			this.addAssemblyButton.Size = new System.Drawing.Size(128, 23);
			this.addAssemblyButton.TabIndex = 2;
			this.addAssemblyButton.Text = "&Add Assembly...";
			this.addAssemblyButton.Click += new System.EventHandler(this.addAssemblyButton_Click);
			// 
			// deleteAssemblyButton
			// 
			this.deleteAssemblyButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.deleteAssemblyButton.Location = new System.Drawing.Point(368, 144);
			this.deleteAssemblyButton.Name = "deleteAssemblyButton";
			this.deleteAssemblyButton.Size = new System.Drawing.Size(128, 23);
			this.deleteAssemblyButton.TabIndex = 5;
			this.deleteAssemblyButton.Text = "&Remove Assembly";
			this.deleteAssemblyButton.Click += new System.EventHandler(this.deleteAssemblyButton_Click);
			// 
			// assemblyListView
			// 
			this.assemblyListView.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.assemblyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							   this.columnHeader1,
																							   this.columnHeader2});
			this.assemblyListView.FullRowSelect = true;
			this.assemblyListView.HideSelection = false;
			this.assemblyListView.HoverSelection = true;
			this.assemblyListView.Location = new System.Drawing.Point(16, 32);
			this.assemblyListView.MultiSelect = false;
			this.assemblyListView.Name = "assemblyListView";
			this.assemblyListView.Size = new System.Drawing.Size(336, 184);
			this.assemblyListView.TabIndex = 1;
			this.assemblyListView.View = System.Windows.Forms.View.Details;
			this.assemblyListView.Resize += new System.EventHandler(this.assemblyListView_Resize);
			this.assemblyListView.MouseHover += new System.EventHandler(this.assemblyListView_MouseHover);
			this.assemblyListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.assemblyListView_MouseMove);
			this.assemblyListView.SelectedIndexChanged += new System.EventHandler(this.assemblyListView_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "File Name";
			this.columnHeader1.Width = 100;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Full Path";
			this.columnHeader2.Width = 232;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(160, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Assemblies Included:";
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
			this.closeButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Location = new System.Drawing.Point(400, 330);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(128, 23);
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
			this.projectPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
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
			this.ClientSize = new System.Drawing.Size(538, 370);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.projectPathLabel,
																		  this.label5,
																		  this.projectTabControl,
																		  this.editConfigsButton,
																		  this.configComboBox,
																		  this.label1,
																		  this.closeButton});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(376, 400);
			this.Name = "ProjectEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NUnit Test Project Editor";
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

		#region ComboBoxes

		private void UpdateConfigs()
		{
			configComboBox.Items.Clear();

			if ( selectedConfig == null )
				selectedConfig = project.ActiveConfig.Name;

			int selectedIndex = -1; 
			foreach( string name in project.Configs.Names )
			{
				int index = configComboBox.Items.Add( name );
				if ( name == selectedConfig )
					selectedIndex = index;
			}

			if ( selectedIndex == -1 && configComboBox.Items.Count > 0 )
			{
				selectedIndex = 0;
				selectedConfig = project.Configs[0].Name;
			}

			if ( selectedIndex == -1 )
				selectedConfig = null;
			else
				configComboBox.SelectedIndex = selectedIndex;
		}

		private void configComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			selectedConfig = (string)configComboBox.SelectedItem;
			
			applicationBaseTextBox.Text =
				project.Configs[selectedConfig].BasePath;

			configFileTextBox.Text = 
				project.Configs[selectedConfig].ConfigurationFile;

			privateBinPathTextBox.Text =
				project.Configs[selectedConfig].BinPath;

			autoBinPathCheckBox.Checked =
				project.Configs[selectedConfig].AutoBinPath;

			UpdateAssembliesForConfig();
		}

		#endregion

		#region ListViews

		private void UpdateAssembliesForConfig()
		{
			assemblyListView.Items.Clear();
			int selectedIndex = -1;

			foreach( string assemblyPath in project.Configs[selectedConfig].Assemblies )
			{
				string assemblyName = Path.GetFileName( assemblyPath );

				ListViewItem item = new ListViewItem(
					new string[] { 
									 Path.GetFileName( assemblyPath ),
									 assemblyPath } );

				int index = assemblyListView.Items.Add( item ).Index;			

				if ( assemblyName == selectedAssembly )
					selectedIndex = index;
			}

			if ( assemblyListView.Items.Count > 0 && selectedIndex == -1)
				selectedIndex = 0;
				
			if ( selectedIndex != -1 )
				assemblyListView.Items[selectedIndex].Selected = true;
		}

		private void assemblyListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( assemblyListView.SelectedItems.Count == 0 )
			{
				selectedAssembly = null;

				editAssemblyButton.Enabled = false;
				deleteAssemblyButton.Enabled = false;
			}
			else 
			{
				selectedAssembly = assemblyListView.SelectedItems[0].Text;

				editAssemblyButton.Enabled = true;
				deleteAssemblyButton.Enabled = true;
			}
		}

		private void assemblyListView_Resize(object sender, System.EventArgs e)
		{
			assemblyListView.Columns[1].Width =
				assemblyListView.ClientSize.Width - assemblyListView.Columns[0].Width;
		}

		#endregion

		#region Other UI Events

		private void ProjectEditor_Load(object sender, System.EventArgs e)
		{
			this.Text = string.Format( "{0} - Project Editor", 
				project.Name );

			projectPathLabel.Text = project.ProjectPath;

			UpdateConfigs();
		}

		private void editConfigsButton_Click(object sender, System.EventArgs e)
		{
			ConfigurationEditor.Edit( project );
			UpdateConfigs();
		}

		private void addAssemblyButton_Click(object sender, System.EventArgs e)
		{
			AppUI.TestLoaderUI.AddAssembly( selectedConfig );
		}

		private void addVSProjectButton_Click(object sender, System.EventArgs e)
		{
			AppUI.TestLoaderUI.AddVSProject( );
		}
		private void editAssemblyButton_Click(object sender, System.EventArgs e)
		{
			AssemblyPathDialog dlg = new AssemblyPathDialog( (string)project.Configs[selectedConfig].Assemblies[assemblyListView.SelectedIndices[0]] );
			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				project.Configs[selectedConfig].Assemblies[assemblyListView.SelectedIndices[0]] = dlg.Path;
				UpdateAssembliesForConfig();
				if ( AppUI.TestLoader.IsTestLoaded && selectedConfig == project.ActiveConfig.Name )
					AppUI.TestLoader.SetActiveConfig( selectedConfig );
			}
		}

		private void deleteAssemblyButton_Click(object sender, System.EventArgs e)
		{
			int index = assemblyListView.SelectedItems[0].Index;
			project.Configs[selectedConfig].Assemblies.RemoveAt( index );
			assemblyListView.Items.RemoveAt( index );
			if ( index >= assemblyListView.Items.Count )
				--index;

			if ( index >= 0 )
			{
				selectedAssembly = assemblyListView.Items[index].Text;
				assemblyListView.Items[index].Selected = true;
			}
			else
				selectedAssembly = null;
		}

		private void closeButton_Click(object sender, System.EventArgs e)
		{
			this.Close();		
		}

		#endregion

		private void assemblyListView_MouseHover(object sender, System.EventArgs e)
		{
			//Show hoverItem.Text in label
		}

		private void assemblyListView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			hoverItem = assemblyListView.GetItemAt( e.X, e.Y );
		}

		private void applicationBaseTextBox_Validated(object sender, System.EventArgs e)
		{
			project.Configs[selectedConfig].BasePath 
				= applicationBaseTextBox.Text;
		}

		private void configFileTextBox_Validated(object sender, System.EventArgs e)
		{
			project.Configs[selectedConfig].ConfigurationFile 
				= configFileTextBox.Text;
		}

		private void privateBinPathTextBox_Validated(object sender, System.EventArgs e)
		{
			project.Configs[selectedConfig].BinPath
				= privateBinPathTextBox.Text;
		}

		private void autoBinPathCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{			
			project.Configs[selectedConfig].AutoBinPath
				= autoBinPathCheckBox.Checked;
		}

		private void applicationBaseTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				DirectoryInfo info = new DirectoryInfo( applicationBaseTextBox.Text );
			}
			catch( Exception exception )
			{
				applicationBaseTextBox.SelectAll();
				UserMessage.DisplayFailure( exception, "Invalid Entry" );
				e.Cancel = true;
			}
		}

		private void configFileTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				FileInfo info = new FileInfo( configFileTextBox.Text );
			}
			catch( Exception exception )
			{
				configFileTextBox.SelectAll();
				UserMessage.DisplayFailure( exception, "Invalid Entry" );
				e.Cancel = true;
			}
		}

		private void privateBinPathTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
		}
	}
}
