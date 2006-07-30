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
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox projectBaseTextBox;
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
			this.label8 = new System.Windows.Forms.Label();
			this.projectBaseTextBox = new System.Windows.Forms.TextBox();
			this.projectTabControl.SuspendLayout();
			this.generalTabPage.SuspendLayout();
			this.assemblyTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// editConfigsButton
			// 
			this.editConfigsButton.AccessibleDescription = resources.GetString("editConfigsButton.AccessibleDescription");
			this.editConfigsButton.AccessibleName = resources.GetString("editConfigsButton.AccessibleName");
			this.editConfigsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("editConfigsButton.Anchor")));
			this.editConfigsButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("editConfigsButton.BackgroundImage")));
			this.editConfigsButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("editConfigsButton.Dock")));
			this.editConfigsButton.Enabled = ((bool)(resources.GetObject("editConfigsButton.Enabled")));
			this.editConfigsButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("editConfigsButton.FlatStyle")));
			this.editConfigsButton.Font = ((System.Drawing.Font)(resources.GetObject("editConfigsButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.editConfigsButton, resources.GetString("editConfigsButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.editConfigsButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("editConfigsButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.editConfigsButton, resources.GetString("editConfigsButton.HelpString"));
			this.editConfigsButton.Image = ((System.Drawing.Image)(resources.GetObject("editConfigsButton.Image")));
			this.editConfigsButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("editConfigsButton.ImageAlign")));
			this.editConfigsButton.ImageIndex = ((int)(resources.GetObject("editConfigsButton.ImageIndex")));
			this.editConfigsButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("editConfigsButton.ImeMode")));
			this.editConfigsButton.Location = ((System.Drawing.Point)(resources.GetObject("editConfigsButton.Location")));
			this.editConfigsButton.Name = "editConfigsButton";
			this.editConfigsButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("editConfigsButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.editConfigsButton, ((bool)(resources.GetObject("editConfigsButton.ShowHelp"))));
			this.editConfigsButton.Size = ((System.Drawing.Size)(resources.GetObject("editConfigsButton.Size")));
			this.editConfigsButton.TabIndex = ((int)(resources.GetObject("editConfigsButton.TabIndex")));
			this.editConfigsButton.Text = resources.GetString("editConfigsButton.Text");
			this.editConfigsButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("editConfigsButton.TextAlign")));
			this.editConfigsButton.Visible = ((bool)(resources.GetObject("editConfigsButton.Visible")));
			this.editConfigsButton.Click += new System.EventHandler(this.editConfigsButton_Click);
			// 
			// configComboBox
			// 
			this.configComboBox.AccessibleDescription = resources.GetString("configComboBox.AccessibleDescription");
			this.configComboBox.AccessibleName = resources.GetString("configComboBox.AccessibleName");
			this.configComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("configComboBox.Anchor")));
			this.configComboBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("configComboBox.BackgroundImage")));
			this.configComboBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("configComboBox.Dock")));
			this.configComboBox.Enabled = ((bool)(resources.GetObject("configComboBox.Enabled")));
			this.configComboBox.Font = ((System.Drawing.Font)(resources.GetObject("configComboBox.Font")));
			this.helpProvider1.SetHelpKeyword(this.configComboBox, resources.GetString("configComboBox.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.configComboBox, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("configComboBox.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.configComboBox, resources.GetString("configComboBox.HelpString"));
			this.configComboBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("configComboBox.ImeMode")));
			this.configComboBox.IntegralHeight = ((bool)(resources.GetObject("configComboBox.IntegralHeight")));
			this.configComboBox.ItemHeight = ((int)(resources.GetObject("configComboBox.ItemHeight")));
			this.configComboBox.Location = ((System.Drawing.Point)(resources.GetObject("configComboBox.Location")));
			this.configComboBox.MaxDropDownItems = ((int)(resources.GetObject("configComboBox.MaxDropDownItems")));
			this.configComboBox.MaxLength = ((int)(resources.GetObject("configComboBox.MaxLength")));
			this.configComboBox.Name = "configComboBox";
			this.configComboBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("configComboBox.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.configComboBox, ((bool)(resources.GetObject("configComboBox.ShowHelp"))));
			this.configComboBox.Size = ((System.Drawing.Size)(resources.GetObject("configComboBox.Size")));
			this.configComboBox.TabIndex = ((int)(resources.GetObject("configComboBox.TabIndex")));
			this.configComboBox.Text = resources.GetString("configComboBox.Text");
			this.configComboBox.Visible = ((bool)(resources.GetObject("configComboBox.Visible")));
			this.configComboBox.SelectedIndexChanged += new System.EventHandler(this.configComboBox_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AccessibleDescription = resources.GetString("label1.AccessibleDescription");
			this.label1.AccessibleName = resources.GetString("label1.AccessibleName");
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label1.Anchor")));
			this.label1.AutoSize = ((bool)(resources.GetObject("label1.AutoSize")));
			this.label1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label1.Dock")));
			this.label1.Enabled = ((bool)(resources.GetObject("label1.Enabled")));
			this.label1.Font = ((System.Drawing.Font)(resources.GetObject("label1.Font")));
			this.helpProvider1.SetHelpKeyword(this.label1, resources.GetString("label1.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.label1, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("label1.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.label1, resources.GetString("label1.HelpString"));
			this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
			this.label1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.ImageAlign")));
			this.label1.ImageIndex = ((int)(resources.GetObject("label1.ImageIndex")));
			this.label1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label1.ImeMode")));
			this.label1.Location = ((System.Drawing.Point)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label1.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.label1, ((bool)(resources.GetObject("label1.ShowHelp"))));
			this.label1.Size = ((System.Drawing.Size)(resources.GetObject("label1.Size")));
			this.label1.TabIndex = ((int)(resources.GetObject("label1.TabIndex")));
			this.label1.Text = resources.GetString("label1.Text");
			this.label1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.TextAlign")));
			this.label1.Visible = ((bool)(resources.GetObject("label1.Visible")));
			// 
			// projectTabControl
			// 
			this.projectTabControl.AccessibleDescription = resources.GetString("projectTabControl.AccessibleDescription");
			this.projectTabControl.AccessibleName = resources.GetString("projectTabControl.AccessibleName");
			this.projectTabControl.Alignment = ((System.Windows.Forms.TabAlignment)(resources.GetObject("projectTabControl.Alignment")));
			this.projectTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("projectTabControl.Anchor")));
			this.projectTabControl.Appearance = ((System.Windows.Forms.TabAppearance)(resources.GetObject("projectTabControl.Appearance")));
			this.projectTabControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("projectTabControl.BackgroundImage")));
			this.projectTabControl.Controls.Add(this.generalTabPage);
			this.projectTabControl.Controls.Add(this.assemblyTabPage);
			this.projectTabControl.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("projectTabControl.Dock")));
			this.projectTabControl.Enabled = ((bool)(resources.GetObject("projectTabControl.Enabled")));
			this.projectTabControl.Font = ((System.Drawing.Font)(resources.GetObject("projectTabControl.Font")));
			this.helpProvider1.SetHelpKeyword(this.projectTabControl, resources.GetString("projectTabControl.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.projectTabControl, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("projectTabControl.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.projectTabControl, resources.GetString("projectTabControl.HelpString"));
			this.projectTabControl.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("projectTabControl.ImeMode")));
			this.projectTabControl.ItemSize = ((System.Drawing.Size)(resources.GetObject("projectTabControl.ItemSize")));
			this.projectTabControl.Location = ((System.Drawing.Point)(resources.GetObject("projectTabControl.Location")));
			this.projectTabControl.Name = "projectTabControl";
			this.projectTabControl.Padding = ((System.Drawing.Point)(resources.GetObject("projectTabControl.Padding")));
			this.projectTabControl.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("projectTabControl.RightToLeft")));
			this.projectTabControl.SelectedIndex = 0;
			this.helpProvider1.SetShowHelp(this.projectTabControl, ((bool)(resources.GetObject("projectTabControl.ShowHelp"))));
			this.projectTabControl.ShowToolTips = ((bool)(resources.GetObject("projectTabControl.ShowToolTips")));
			this.projectTabControl.Size = ((System.Drawing.Size)(resources.GetObject("projectTabControl.Size")));
			this.projectTabControl.TabIndex = ((int)(resources.GetObject("projectTabControl.TabIndex")));
			this.projectTabControl.Text = resources.GetString("projectTabControl.Text");
			this.projectTabControl.Visible = ((bool)(resources.GetObject("projectTabControl.Visible")));
			// 
			// generalTabPage
			// 
			this.generalTabPage.AccessibleDescription = resources.GetString("generalTabPage.AccessibleDescription");
			this.generalTabPage.AccessibleName = resources.GetString("generalTabPage.AccessibleName");
			this.generalTabPage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("generalTabPage.Anchor")));
			this.generalTabPage.AutoScroll = ((bool)(resources.GetObject("generalTabPage.AutoScroll")));
			this.generalTabPage.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("generalTabPage.AutoScrollMargin")));
			this.generalTabPage.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("generalTabPage.AutoScrollMinSize")));
			this.generalTabPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("generalTabPage.BackgroundImage")));
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
			this.generalTabPage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("generalTabPage.Dock")));
			this.generalTabPage.Enabled = ((bool)(resources.GetObject("generalTabPage.Enabled")));
			this.generalTabPage.Font = ((System.Drawing.Font)(resources.GetObject("generalTabPage.Font")));
			this.helpProvider1.SetHelpKeyword(this.generalTabPage, resources.GetString("generalTabPage.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.generalTabPage, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("generalTabPage.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.generalTabPage, resources.GetString("generalTabPage.HelpString"));
			this.generalTabPage.ImageIndex = ((int)(resources.GetObject("generalTabPage.ImageIndex")));
			this.generalTabPage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("generalTabPage.ImeMode")));
			this.generalTabPage.Location = ((System.Drawing.Point)(resources.GetObject("generalTabPage.Location")));
			this.generalTabPage.Name = "generalTabPage";
			this.generalTabPage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("generalTabPage.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.generalTabPage, ((bool)(resources.GetObject("generalTabPage.ShowHelp"))));
			this.generalTabPage.Size = ((System.Drawing.Size)(resources.GetObject("generalTabPage.Size")));
			this.generalTabPage.TabIndex = ((int)(resources.GetObject("generalTabPage.TabIndex")));
			this.generalTabPage.Text = resources.GetString("generalTabPage.Text");
			this.generalTabPage.ToolTipText = resources.GetString("generalTabPage.ToolTipText");
			this.generalTabPage.Visible = ((bool)(resources.GetObject("generalTabPage.Visible")));
			// 
			// label7
			// 
			this.label7.AccessibleDescription = resources.GetString("label7.AccessibleDescription");
			this.label7.AccessibleName = resources.GetString("label7.AccessibleName");
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label7.Anchor")));
			this.label7.AutoSize = ((bool)(resources.GetObject("label7.AutoSize")));
			this.label7.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label7.Dock")));
			this.label7.Enabled = ((bool)(resources.GetObject("label7.Enabled")));
			this.label7.Font = ((System.Drawing.Font)(resources.GetObject("label7.Font")));
			this.helpProvider1.SetHelpKeyword(this.label7, resources.GetString("label7.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.label7, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("label7.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.label7, resources.GetString("label7.HelpString"));
			this.label7.Image = ((System.Drawing.Image)(resources.GetObject("label7.Image")));
			this.label7.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label7.ImageAlign")));
			this.label7.ImageIndex = ((int)(resources.GetObject("label7.ImageIndex")));
			this.label7.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label7.ImeMode")));
			this.label7.Location = ((System.Drawing.Point)(resources.GetObject("label7.Location")));
			this.label7.Name = "label7";
			this.label7.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label7.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.label7, ((bool)(resources.GetObject("label7.ShowHelp"))));
			this.label7.Size = ((System.Drawing.Size)(resources.GetObject("label7.Size")));
			this.label7.TabIndex = ((int)(resources.GetObject("label7.TabIndex")));
			this.label7.Text = resources.GetString("label7.Text");
			this.label7.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label7.TextAlign")));
			this.label7.Visible = ((bool)(resources.GetObject("label7.Visible")));
			// 
			// autoBinPathRadioButton
			// 
			this.autoBinPathRadioButton.AccessibleDescription = resources.GetString("autoBinPathRadioButton.AccessibleDescription");
			this.autoBinPathRadioButton.AccessibleName = resources.GetString("autoBinPathRadioButton.AccessibleName");
			this.autoBinPathRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("autoBinPathRadioButton.Anchor")));
			this.autoBinPathRadioButton.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("autoBinPathRadioButton.Appearance")));
			this.autoBinPathRadioButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("autoBinPathRadioButton.BackgroundImage")));
			this.autoBinPathRadioButton.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("autoBinPathRadioButton.CheckAlign")));
			this.autoBinPathRadioButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("autoBinPathRadioButton.Dock")));
			this.autoBinPathRadioButton.Enabled = ((bool)(resources.GetObject("autoBinPathRadioButton.Enabled")));
			this.autoBinPathRadioButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("autoBinPathRadioButton.FlatStyle")));
			this.autoBinPathRadioButton.Font = ((System.Drawing.Font)(resources.GetObject("autoBinPathRadioButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.autoBinPathRadioButton, resources.GetString("autoBinPathRadioButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.autoBinPathRadioButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("autoBinPathRadioButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.autoBinPathRadioButton, resources.GetString("autoBinPathRadioButton.HelpString"));
			this.autoBinPathRadioButton.Image = ((System.Drawing.Image)(resources.GetObject("autoBinPathRadioButton.Image")));
			this.autoBinPathRadioButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("autoBinPathRadioButton.ImageAlign")));
			this.autoBinPathRadioButton.ImageIndex = ((int)(resources.GetObject("autoBinPathRadioButton.ImageIndex")));
			this.autoBinPathRadioButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("autoBinPathRadioButton.ImeMode")));
			this.autoBinPathRadioButton.Location = ((System.Drawing.Point)(resources.GetObject("autoBinPathRadioButton.Location")));
			this.autoBinPathRadioButton.Name = "autoBinPathRadioButton";
			this.autoBinPathRadioButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("autoBinPathRadioButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.autoBinPathRadioButton, ((bool)(resources.GetObject("autoBinPathRadioButton.ShowHelp"))));
			this.autoBinPathRadioButton.Size = ((System.Drawing.Size)(resources.GetObject("autoBinPathRadioButton.Size")));
			this.autoBinPathRadioButton.TabIndex = ((int)(resources.GetObject("autoBinPathRadioButton.TabIndex")));
			this.autoBinPathRadioButton.Text = resources.GetString("autoBinPathRadioButton.Text");
			this.autoBinPathRadioButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("autoBinPathRadioButton.TextAlign")));
			this.autoBinPathRadioButton.Visible = ((bool)(resources.GetObject("autoBinPathRadioButton.Visible")));
			this.autoBinPathRadioButton.CheckedChanged += new System.EventHandler(this.autoBinPathRadioButton_CheckedChanged);
			// 
			// manualBinPathRadioButton
			// 
			this.manualBinPathRadioButton.AccessibleDescription = resources.GetString("manualBinPathRadioButton.AccessibleDescription");
			this.manualBinPathRadioButton.AccessibleName = resources.GetString("manualBinPathRadioButton.AccessibleName");
			this.manualBinPathRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("manualBinPathRadioButton.Anchor")));
			this.manualBinPathRadioButton.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("manualBinPathRadioButton.Appearance")));
			this.manualBinPathRadioButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("manualBinPathRadioButton.BackgroundImage")));
			this.manualBinPathRadioButton.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("manualBinPathRadioButton.CheckAlign")));
			this.manualBinPathRadioButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("manualBinPathRadioButton.Dock")));
			this.manualBinPathRadioButton.Enabled = ((bool)(resources.GetObject("manualBinPathRadioButton.Enabled")));
			this.manualBinPathRadioButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("manualBinPathRadioButton.FlatStyle")));
			this.manualBinPathRadioButton.Font = ((System.Drawing.Font)(resources.GetObject("manualBinPathRadioButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.manualBinPathRadioButton, resources.GetString("manualBinPathRadioButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.manualBinPathRadioButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("manualBinPathRadioButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.manualBinPathRadioButton, resources.GetString("manualBinPathRadioButton.HelpString"));
			this.manualBinPathRadioButton.Image = ((System.Drawing.Image)(resources.GetObject("manualBinPathRadioButton.Image")));
			this.manualBinPathRadioButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("manualBinPathRadioButton.ImageAlign")));
			this.manualBinPathRadioButton.ImageIndex = ((int)(resources.GetObject("manualBinPathRadioButton.ImageIndex")));
			this.manualBinPathRadioButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("manualBinPathRadioButton.ImeMode")));
			this.manualBinPathRadioButton.Location = ((System.Drawing.Point)(resources.GetObject("manualBinPathRadioButton.Location")));
			this.manualBinPathRadioButton.Name = "manualBinPathRadioButton";
			this.manualBinPathRadioButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("manualBinPathRadioButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.manualBinPathRadioButton, ((bool)(resources.GetObject("manualBinPathRadioButton.ShowHelp"))));
			this.manualBinPathRadioButton.Size = ((System.Drawing.Size)(resources.GetObject("manualBinPathRadioButton.Size")));
			this.manualBinPathRadioButton.TabIndex = ((int)(resources.GetObject("manualBinPathRadioButton.TabIndex")));
			this.manualBinPathRadioButton.Text = resources.GetString("manualBinPathRadioButton.Text");
			this.manualBinPathRadioButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("manualBinPathRadioButton.TextAlign")));
			this.manualBinPathRadioButton.Visible = ((bool)(resources.GetObject("manualBinPathRadioButton.Visible")));
			this.manualBinPathRadioButton.CheckedChanged += new System.EventHandler(this.manualBinPathRadioButton_CheckedChanged);
			// 
			// noBinPathRadioButton
			// 
			this.noBinPathRadioButton.AccessibleDescription = resources.GetString("noBinPathRadioButton.AccessibleDescription");
			this.noBinPathRadioButton.AccessibleName = resources.GetString("noBinPathRadioButton.AccessibleName");
			this.noBinPathRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("noBinPathRadioButton.Anchor")));
			this.noBinPathRadioButton.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("noBinPathRadioButton.Appearance")));
			this.noBinPathRadioButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("noBinPathRadioButton.BackgroundImage")));
			this.noBinPathRadioButton.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("noBinPathRadioButton.CheckAlign")));
			this.noBinPathRadioButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("noBinPathRadioButton.Dock")));
			this.noBinPathRadioButton.Enabled = ((bool)(resources.GetObject("noBinPathRadioButton.Enabled")));
			this.noBinPathRadioButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("noBinPathRadioButton.FlatStyle")));
			this.noBinPathRadioButton.Font = ((System.Drawing.Font)(resources.GetObject("noBinPathRadioButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.noBinPathRadioButton, resources.GetString("noBinPathRadioButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.noBinPathRadioButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("noBinPathRadioButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.noBinPathRadioButton, resources.GetString("noBinPathRadioButton.HelpString"));
			this.noBinPathRadioButton.Image = ((System.Drawing.Image)(resources.GetObject("noBinPathRadioButton.Image")));
			this.noBinPathRadioButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("noBinPathRadioButton.ImageAlign")));
			this.noBinPathRadioButton.ImageIndex = ((int)(resources.GetObject("noBinPathRadioButton.ImageIndex")));
			this.noBinPathRadioButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("noBinPathRadioButton.ImeMode")));
			this.noBinPathRadioButton.Location = ((System.Drawing.Point)(resources.GetObject("noBinPathRadioButton.Location")));
			this.noBinPathRadioButton.Name = "noBinPathRadioButton";
			this.noBinPathRadioButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("noBinPathRadioButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.noBinPathRadioButton, ((bool)(resources.GetObject("noBinPathRadioButton.ShowHelp"))));
			this.noBinPathRadioButton.Size = ((System.Drawing.Size)(resources.GetObject("noBinPathRadioButton.Size")));
			this.noBinPathRadioButton.TabIndex = ((int)(resources.GetObject("noBinPathRadioButton.TabIndex")));
			this.noBinPathRadioButton.Text = resources.GetString("noBinPathRadioButton.Text");
			this.noBinPathRadioButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("noBinPathRadioButton.TextAlign")));
			this.noBinPathRadioButton.Visible = ((bool)(resources.GetObject("noBinPathRadioButton.Visible")));
			this.noBinPathRadioButton.CheckedChanged += new System.EventHandler(this.noBinPathRadioButton_CheckedChanged);
			// 
			// browseBasePathButton
			// 
			this.browseBasePathButton.AccessibleDescription = resources.GetString("browseBasePathButton.AccessibleDescription");
			this.browseBasePathButton.AccessibleName = resources.GetString("browseBasePathButton.AccessibleName");
			this.browseBasePathButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("browseBasePathButton.Anchor")));
			this.browseBasePathButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("browseBasePathButton.BackgroundImage")));
			this.browseBasePathButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("browseBasePathButton.Dock")));
			this.browseBasePathButton.Enabled = ((bool)(resources.GetObject("browseBasePathButton.Enabled")));
			this.browseBasePathButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("browseBasePathButton.FlatStyle")));
			this.browseBasePathButton.Font = ((System.Drawing.Font)(resources.GetObject("browseBasePathButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.browseBasePathButton, resources.GetString("browseBasePathButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.browseBasePathButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("browseBasePathButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.browseBasePathButton, resources.GetString("browseBasePathButton.HelpString"));
			this.browseBasePathButton.Image = ((System.Drawing.Image)(resources.GetObject("browseBasePathButton.Image")));
			this.browseBasePathButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("browseBasePathButton.ImageAlign")));
			this.browseBasePathButton.ImageIndex = ((int)(resources.GetObject("browseBasePathButton.ImageIndex")));
			this.browseBasePathButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("browseBasePathButton.ImeMode")));
			this.browseBasePathButton.Location = ((System.Drawing.Point)(resources.GetObject("browseBasePathButton.Location")));
			this.browseBasePathButton.Name = "browseBasePathButton";
			this.browseBasePathButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("browseBasePathButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.browseBasePathButton, ((bool)(resources.GetObject("browseBasePathButton.ShowHelp"))));
			this.browseBasePathButton.Size = ((System.Drawing.Size)(resources.GetObject("browseBasePathButton.Size")));
			this.browseBasePathButton.TabIndex = ((int)(resources.GetObject("browseBasePathButton.TabIndex")));
			this.browseBasePathButton.Text = resources.GetString("browseBasePathButton.Text");
			this.browseBasePathButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("browseBasePathButton.TextAlign")));
			this.browseBasePathButton.Visible = ((bool)(resources.GetObject("browseBasePathButton.Visible")));
			this.browseBasePathButton.Click += new System.EventHandler(this.browseBasePathButton_Click);
			// 
			// privateBinPathTextBox
			// 
			this.privateBinPathTextBox.AccessibleDescription = resources.GetString("privateBinPathTextBox.AccessibleDescription");
			this.privateBinPathTextBox.AccessibleName = resources.GetString("privateBinPathTextBox.AccessibleName");
			this.privateBinPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("privateBinPathTextBox.Anchor")));
			this.privateBinPathTextBox.AutoSize = ((bool)(resources.GetObject("privateBinPathTextBox.AutoSize")));
			this.privateBinPathTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("privateBinPathTextBox.BackgroundImage")));
			this.privateBinPathTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("privateBinPathTextBox.Dock")));
			this.privateBinPathTextBox.Enabled = ((bool)(resources.GetObject("privateBinPathTextBox.Enabled")));
			this.privateBinPathTextBox.Font = ((System.Drawing.Font)(resources.GetObject("privateBinPathTextBox.Font")));
			this.helpProvider1.SetHelpKeyword(this.privateBinPathTextBox, resources.GetString("privateBinPathTextBox.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.privateBinPathTextBox, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("privateBinPathTextBox.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.privateBinPathTextBox, resources.GetString("privateBinPathTextBox.HelpString"));
			this.privateBinPathTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("privateBinPathTextBox.ImeMode")));
			this.privateBinPathTextBox.Location = ((System.Drawing.Point)(resources.GetObject("privateBinPathTextBox.Location")));
			this.privateBinPathTextBox.MaxLength = ((int)(resources.GetObject("privateBinPathTextBox.MaxLength")));
			this.privateBinPathTextBox.Multiline = ((bool)(resources.GetObject("privateBinPathTextBox.Multiline")));
			this.privateBinPathTextBox.Name = "privateBinPathTextBox";
			this.privateBinPathTextBox.PasswordChar = ((char)(resources.GetObject("privateBinPathTextBox.PasswordChar")));
			this.privateBinPathTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("privateBinPathTextBox.RightToLeft")));
			this.privateBinPathTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("privateBinPathTextBox.ScrollBars")));
			this.helpProvider1.SetShowHelp(this.privateBinPathTextBox, ((bool)(resources.GetObject("privateBinPathTextBox.ShowHelp"))));
			this.privateBinPathTextBox.Size = ((System.Drawing.Size)(resources.GetObject("privateBinPathTextBox.Size")));
			this.privateBinPathTextBox.TabIndex = ((int)(resources.GetObject("privateBinPathTextBox.TabIndex")));
			this.privateBinPathTextBox.Text = resources.GetString("privateBinPathTextBox.Text");
			this.privateBinPathTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("privateBinPathTextBox.TextAlign")));
			this.privateBinPathTextBox.Visible = ((bool)(resources.GetObject("privateBinPathTextBox.Visible")));
			this.privateBinPathTextBox.WordWrap = ((bool)(resources.GetObject("privateBinPathTextBox.WordWrap")));
			this.privateBinPathTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.privateBinPathTextBox_Validating);
			this.privateBinPathTextBox.Validated += new System.EventHandler(this.privateBinPathTextBox_Validated);
			// 
			// label6
			// 
			this.label6.AccessibleDescription = resources.GetString("label6.AccessibleDescription");
			this.label6.AccessibleName = resources.GetString("label6.AccessibleName");
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label6.Anchor")));
			this.label6.AutoSize = ((bool)(resources.GetObject("label6.AutoSize")));
			this.label6.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label6.Dock")));
			this.label6.Enabled = ((bool)(resources.GetObject("label6.Enabled")));
			this.label6.Font = ((System.Drawing.Font)(resources.GetObject("label6.Font")));
			this.helpProvider1.SetHelpKeyword(this.label6, resources.GetString("label6.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.label6, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("label6.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.label6, resources.GetString("label6.HelpString"));
			this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
			this.label6.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label6.ImageAlign")));
			this.label6.ImageIndex = ((int)(resources.GetObject("label6.ImageIndex")));
			this.label6.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label6.ImeMode")));
			this.label6.Location = ((System.Drawing.Point)(resources.GetObject("label6.Location")));
			this.label6.Name = "label6";
			this.label6.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label6.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.label6, ((bool)(resources.GetObject("label6.ShowHelp"))));
			this.label6.Size = ((System.Drawing.Size)(resources.GetObject("label6.Size")));
			this.label6.TabIndex = ((int)(resources.GetObject("label6.TabIndex")));
			this.label6.Text = resources.GetString("label6.Text");
			this.label6.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label6.TextAlign")));
			this.label6.Visible = ((bool)(resources.GetObject("label6.Visible")));
			// 
			// configFileTextBox
			// 
			this.configFileTextBox.AccessibleDescription = resources.GetString("configFileTextBox.AccessibleDescription");
			this.configFileTextBox.AccessibleName = resources.GetString("configFileTextBox.AccessibleName");
			this.configFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("configFileTextBox.Anchor")));
			this.configFileTextBox.AutoSize = ((bool)(resources.GetObject("configFileTextBox.AutoSize")));
			this.configFileTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("configFileTextBox.BackgroundImage")));
			this.configFileTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("configFileTextBox.Dock")));
			this.configFileTextBox.Enabled = ((bool)(resources.GetObject("configFileTextBox.Enabled")));
			this.configFileTextBox.Font = ((System.Drawing.Font)(resources.GetObject("configFileTextBox.Font")));
			this.helpProvider1.SetHelpKeyword(this.configFileTextBox, resources.GetString("configFileTextBox.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.configFileTextBox, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("configFileTextBox.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.configFileTextBox, resources.GetString("configFileTextBox.HelpString"));
			this.configFileTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("configFileTextBox.ImeMode")));
			this.configFileTextBox.Location = ((System.Drawing.Point)(resources.GetObject("configFileTextBox.Location")));
			this.configFileTextBox.MaxLength = ((int)(resources.GetObject("configFileTextBox.MaxLength")));
			this.configFileTextBox.Multiline = ((bool)(resources.GetObject("configFileTextBox.Multiline")));
			this.configFileTextBox.Name = "configFileTextBox";
			this.configFileTextBox.PasswordChar = ((char)(resources.GetObject("configFileTextBox.PasswordChar")));
			this.configFileTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("configFileTextBox.RightToLeft")));
			this.configFileTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("configFileTextBox.ScrollBars")));
			this.helpProvider1.SetShowHelp(this.configFileTextBox, ((bool)(resources.GetObject("configFileTextBox.ShowHelp"))));
			this.configFileTextBox.Size = ((System.Drawing.Size)(resources.GetObject("configFileTextBox.Size")));
			this.configFileTextBox.TabIndex = ((int)(resources.GetObject("configFileTextBox.TabIndex")));
			this.configFileTextBox.Text = resources.GetString("configFileTextBox.Text");
			this.configFileTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("configFileTextBox.TextAlign")));
			this.configFileTextBox.Visible = ((bool)(resources.GetObject("configFileTextBox.Visible")));
			this.configFileTextBox.WordWrap = ((bool)(resources.GetObject("configFileTextBox.WordWrap")));
			this.configFileTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.configFileTextBox_Validating);
			this.configFileTextBox.Validated += new System.EventHandler(this.configFileTextBox_Validated);
			// 
			// label4
			// 
			this.label4.AccessibleDescription = resources.GetString("label4.AccessibleDescription");
			this.label4.AccessibleName = resources.GetString("label4.AccessibleName");
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label4.Anchor")));
			this.label4.AutoSize = ((bool)(resources.GetObject("label4.AutoSize")));
			this.label4.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label4.Dock")));
			this.label4.Enabled = ((bool)(resources.GetObject("label4.Enabled")));
			this.label4.Font = ((System.Drawing.Font)(resources.GetObject("label4.Font")));
			this.helpProvider1.SetHelpKeyword(this.label4, resources.GetString("label4.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.label4, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("label4.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.label4, resources.GetString("label4.HelpString"));
			this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
			this.label4.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.ImageAlign")));
			this.label4.ImageIndex = ((int)(resources.GetObject("label4.ImageIndex")));
			this.label4.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label4.ImeMode")));
			this.label4.Location = ((System.Drawing.Point)(resources.GetObject("label4.Location")));
			this.label4.Name = "label4";
			this.label4.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label4.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.label4, ((bool)(resources.GetObject("label4.ShowHelp"))));
			this.label4.Size = ((System.Drawing.Size)(resources.GetObject("label4.Size")));
			this.label4.TabIndex = ((int)(resources.GetObject("label4.TabIndex")));
			this.label4.Text = resources.GetString("label4.Text");
			this.label4.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.TextAlign")));
			this.label4.Visible = ((bool)(resources.GetObject("label4.Visible")));
			// 
			// applicationBaseTextBox
			// 
			this.applicationBaseTextBox.AccessibleDescription = resources.GetString("applicationBaseTextBox.AccessibleDescription");
			this.applicationBaseTextBox.AccessibleName = resources.GetString("applicationBaseTextBox.AccessibleName");
			this.applicationBaseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("applicationBaseTextBox.Anchor")));
			this.applicationBaseTextBox.AutoSize = ((bool)(resources.GetObject("applicationBaseTextBox.AutoSize")));
			this.applicationBaseTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("applicationBaseTextBox.BackgroundImage")));
			this.applicationBaseTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("applicationBaseTextBox.Dock")));
			this.applicationBaseTextBox.Enabled = ((bool)(resources.GetObject("applicationBaseTextBox.Enabled")));
			this.applicationBaseTextBox.Font = ((System.Drawing.Font)(resources.GetObject("applicationBaseTextBox.Font")));
			this.helpProvider1.SetHelpKeyword(this.applicationBaseTextBox, resources.GetString("applicationBaseTextBox.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.applicationBaseTextBox, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("applicationBaseTextBox.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.applicationBaseTextBox, resources.GetString("applicationBaseTextBox.HelpString"));
			this.applicationBaseTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("applicationBaseTextBox.ImeMode")));
			this.applicationBaseTextBox.Location = ((System.Drawing.Point)(resources.GetObject("applicationBaseTextBox.Location")));
			this.applicationBaseTextBox.MaxLength = ((int)(resources.GetObject("applicationBaseTextBox.MaxLength")));
			this.applicationBaseTextBox.Multiline = ((bool)(resources.GetObject("applicationBaseTextBox.Multiline")));
			this.applicationBaseTextBox.Name = "applicationBaseTextBox";
			this.applicationBaseTextBox.PasswordChar = ((char)(resources.GetObject("applicationBaseTextBox.PasswordChar")));
			this.applicationBaseTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("applicationBaseTextBox.RightToLeft")));
			this.applicationBaseTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("applicationBaseTextBox.ScrollBars")));
			this.helpProvider1.SetShowHelp(this.applicationBaseTextBox, ((bool)(resources.GetObject("applicationBaseTextBox.ShowHelp"))));
			this.applicationBaseTextBox.Size = ((System.Drawing.Size)(resources.GetObject("applicationBaseTextBox.Size")));
			this.applicationBaseTextBox.TabIndex = ((int)(resources.GetObject("applicationBaseTextBox.TabIndex")));
			this.applicationBaseTextBox.Text = resources.GetString("applicationBaseTextBox.Text");
			this.applicationBaseTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("applicationBaseTextBox.TextAlign")));
			this.applicationBaseTextBox.Visible = ((bool)(resources.GetObject("applicationBaseTextBox.Visible")));
			this.applicationBaseTextBox.WordWrap = ((bool)(resources.GetObject("applicationBaseTextBox.WordWrap")));
			this.applicationBaseTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.applicationBaseTextBox_Validating);
			this.applicationBaseTextBox.Validated += new System.EventHandler(this.applicationBaseTextBox_Validated);
			// 
			// label3
			// 
			this.label3.AccessibleDescription = resources.GetString("label3.AccessibleDescription");
			this.label3.AccessibleName = resources.GetString("label3.AccessibleName");
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label3.Anchor")));
			this.label3.AutoSize = ((bool)(resources.GetObject("label3.AutoSize")));
			this.label3.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label3.Dock")));
			this.label3.Enabled = ((bool)(resources.GetObject("label3.Enabled")));
			this.label3.Font = ((System.Drawing.Font)(resources.GetObject("label3.Font")));
			this.helpProvider1.SetHelpKeyword(this.label3, resources.GetString("label3.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.label3, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("label3.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.label3, resources.GetString("label3.HelpString"));
			this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
			this.label3.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.ImageAlign")));
			this.label3.ImageIndex = ((int)(resources.GetObject("label3.ImageIndex")));
			this.label3.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label3.ImeMode")));
			this.label3.Location = ((System.Drawing.Point)(resources.GetObject("label3.Location")));
			this.label3.Name = "label3";
			this.label3.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label3.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.label3, ((bool)(resources.GetObject("label3.ShowHelp"))));
			this.label3.Size = ((System.Drawing.Size)(resources.GetObject("label3.Size")));
			this.label3.TabIndex = ((int)(resources.GetObject("label3.TabIndex")));
			this.label3.Text = resources.GetString("label3.Text");
			this.label3.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.TextAlign")));
			this.label3.Visible = ((bool)(resources.GetObject("label3.Visible")));
			// 
			// assemblyTabPage
			// 
			this.assemblyTabPage.AccessibleDescription = resources.GetString("assemblyTabPage.AccessibleDescription");
			this.assemblyTabPage.AccessibleName = resources.GetString("assemblyTabPage.AccessibleName");
			this.assemblyTabPage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("assemblyTabPage.Anchor")));
			this.assemblyTabPage.AutoScroll = ((bool)(resources.GetObject("assemblyTabPage.AutoScroll")));
			this.assemblyTabPage.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("assemblyTabPage.AutoScrollMargin")));
			this.assemblyTabPage.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("assemblyTabPage.AutoScrollMinSize")));
			this.assemblyTabPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("assemblyTabPage.BackgroundImage")));
			this.assemblyTabPage.Controls.Add(this.assemblyListBox);
			this.assemblyTabPage.Controls.Add(this.addVSProjectButton);
			this.assemblyTabPage.Controls.Add(this.editAssemblyButton);
			this.assemblyTabPage.Controls.Add(this.addAssemblyButton);
			this.assemblyTabPage.Controls.Add(this.deleteAssemblyButton);
			this.assemblyTabPage.Controls.Add(this.label2);
			this.assemblyTabPage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("assemblyTabPage.Dock")));
			this.assemblyTabPage.Enabled = ((bool)(resources.GetObject("assemblyTabPage.Enabled")));
			this.assemblyTabPage.Font = ((System.Drawing.Font)(resources.GetObject("assemblyTabPage.Font")));
			this.helpProvider1.SetHelpKeyword(this.assemblyTabPage, resources.GetString("assemblyTabPage.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.assemblyTabPage, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("assemblyTabPage.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.assemblyTabPage, resources.GetString("assemblyTabPage.HelpString"));
			this.assemblyTabPage.ImageIndex = ((int)(resources.GetObject("assemblyTabPage.ImageIndex")));
			this.assemblyTabPage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("assemblyTabPage.ImeMode")));
			this.assemblyTabPage.Location = ((System.Drawing.Point)(resources.GetObject("assemblyTabPage.Location")));
			this.assemblyTabPage.Name = "assemblyTabPage";
			this.assemblyTabPage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("assemblyTabPage.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.assemblyTabPage, ((bool)(resources.GetObject("assemblyTabPage.ShowHelp"))));
			this.assemblyTabPage.Size = ((System.Drawing.Size)(resources.GetObject("assemblyTabPage.Size")));
			this.assemblyTabPage.TabIndex = ((int)(resources.GetObject("assemblyTabPage.TabIndex")));
			this.assemblyTabPage.Text = resources.GetString("assemblyTabPage.Text");
			this.assemblyTabPage.ToolTipText = resources.GetString("assemblyTabPage.ToolTipText");
			this.assemblyTabPage.Visible = ((bool)(resources.GetObject("assemblyTabPage.Visible")));
			// 
			// assemblyListBox
			// 
			this.assemblyListBox.AccessibleDescription = resources.GetString("assemblyListBox.AccessibleDescription");
			this.assemblyListBox.AccessibleName = resources.GetString("assemblyListBox.AccessibleName");
			this.assemblyListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("assemblyListBox.Anchor")));
			this.assemblyListBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("assemblyListBox.BackgroundImage")));
			this.assemblyListBox.ColumnWidth = ((int)(resources.GetObject("assemblyListBox.ColumnWidth")));
			this.assemblyListBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("assemblyListBox.Dock")));
			this.assemblyListBox.Enabled = ((bool)(resources.GetObject("assemblyListBox.Enabled")));
			this.assemblyListBox.Font = ((System.Drawing.Font)(resources.GetObject("assemblyListBox.Font")));
			this.helpProvider1.SetHelpKeyword(this.assemblyListBox, resources.GetString("assemblyListBox.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.assemblyListBox, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("assemblyListBox.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.assemblyListBox, resources.GetString("assemblyListBox.HelpString"));
			this.assemblyListBox.HorizontalExtent = ((int)(resources.GetObject("assemblyListBox.HorizontalExtent")));
			this.assemblyListBox.HorizontalScrollbar = ((bool)(resources.GetObject("assemblyListBox.HorizontalScrollbar")));
			this.assemblyListBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("assemblyListBox.ImeMode")));
			this.assemblyListBox.IntegralHeight = ((bool)(resources.GetObject("assemblyListBox.IntegralHeight")));
			this.assemblyListBox.Location = ((System.Drawing.Point)(resources.GetObject("assemblyListBox.Location")));
			this.assemblyListBox.Name = "assemblyListBox";
			this.assemblyListBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("assemblyListBox.RightToLeft")));
			this.assemblyListBox.ScrollAlwaysVisible = ((bool)(resources.GetObject("assemblyListBox.ScrollAlwaysVisible")));
			this.helpProvider1.SetShowHelp(this.assemblyListBox, ((bool)(resources.GetObject("assemblyListBox.ShowHelp"))));
			this.assemblyListBox.Size = ((System.Drawing.Size)(resources.GetObject("assemblyListBox.Size")));
			this.assemblyListBox.TabIndex = ((int)(resources.GetObject("assemblyListBox.TabIndex")));
			this.assemblyListBox.ThreeDCheckBoxes = true;
			this.assemblyListBox.Visible = ((bool)(resources.GetObject("assemblyListBox.Visible")));
			this.assemblyListBox.SelectedIndexChanged += new System.EventHandler(this.assemblyListBox_SelectedIndexChanged);
			this.assemblyListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.assemblyListBox_ItemCheck);
			// 
			// addVSProjectButton
			// 
			this.addVSProjectButton.AccessibleDescription = resources.GetString("addVSProjectButton.AccessibleDescription");
			this.addVSProjectButton.AccessibleName = resources.GetString("addVSProjectButton.AccessibleName");
			this.addVSProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("addVSProjectButton.Anchor")));
			this.addVSProjectButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("addVSProjectButton.BackgroundImage")));
			this.addVSProjectButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("addVSProjectButton.Dock")));
			this.addVSProjectButton.Enabled = ((bool)(resources.GetObject("addVSProjectButton.Enabled")));
			this.addVSProjectButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("addVSProjectButton.FlatStyle")));
			this.addVSProjectButton.Font = ((System.Drawing.Font)(resources.GetObject("addVSProjectButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.addVSProjectButton, resources.GetString("addVSProjectButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.addVSProjectButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("addVSProjectButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.addVSProjectButton, resources.GetString("addVSProjectButton.HelpString"));
			this.addVSProjectButton.Image = ((System.Drawing.Image)(resources.GetObject("addVSProjectButton.Image")));
			this.addVSProjectButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("addVSProjectButton.ImageAlign")));
			this.addVSProjectButton.ImageIndex = ((int)(resources.GetObject("addVSProjectButton.ImageIndex")));
			this.addVSProjectButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("addVSProjectButton.ImeMode")));
			this.addVSProjectButton.Location = ((System.Drawing.Point)(resources.GetObject("addVSProjectButton.Location")));
			this.addVSProjectButton.Name = "addVSProjectButton";
			this.addVSProjectButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("addVSProjectButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.addVSProjectButton, ((bool)(resources.GetObject("addVSProjectButton.ShowHelp"))));
			this.addVSProjectButton.Size = ((System.Drawing.Size)(resources.GetObject("addVSProjectButton.Size")));
			this.addVSProjectButton.TabIndex = ((int)(resources.GetObject("addVSProjectButton.TabIndex")));
			this.addVSProjectButton.Text = resources.GetString("addVSProjectButton.Text");
			this.addVSProjectButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("addVSProjectButton.TextAlign")));
			this.addVSProjectButton.Visible = ((bool)(resources.GetObject("addVSProjectButton.Visible")));
			this.addVSProjectButton.Click += new System.EventHandler(this.addVSProjectButton_Click);
			// 
			// editAssemblyButton
			// 
			this.editAssemblyButton.AccessibleDescription = resources.GetString("editAssemblyButton.AccessibleDescription");
			this.editAssemblyButton.AccessibleName = resources.GetString("editAssemblyButton.AccessibleName");
			this.editAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("editAssemblyButton.Anchor")));
			this.editAssemblyButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("editAssemblyButton.BackgroundImage")));
			this.editAssemblyButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("editAssemblyButton.Dock")));
			this.editAssemblyButton.Enabled = ((bool)(resources.GetObject("editAssemblyButton.Enabled")));
			this.editAssemblyButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("editAssemblyButton.FlatStyle")));
			this.editAssemblyButton.Font = ((System.Drawing.Font)(resources.GetObject("editAssemblyButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.editAssemblyButton, resources.GetString("editAssemblyButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.editAssemblyButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("editAssemblyButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.editAssemblyButton, resources.GetString("editAssemblyButton.HelpString"));
			this.editAssemblyButton.Image = ((System.Drawing.Image)(resources.GetObject("editAssemblyButton.Image")));
			this.editAssemblyButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("editAssemblyButton.ImageAlign")));
			this.editAssemblyButton.ImageIndex = ((int)(resources.GetObject("editAssemblyButton.ImageIndex")));
			this.editAssemblyButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("editAssemblyButton.ImeMode")));
			this.editAssemblyButton.Location = ((System.Drawing.Point)(resources.GetObject("editAssemblyButton.Location")));
			this.editAssemblyButton.Name = "editAssemblyButton";
			this.editAssemblyButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("editAssemblyButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.editAssemblyButton, ((bool)(resources.GetObject("editAssemblyButton.ShowHelp"))));
			this.editAssemblyButton.Size = ((System.Drawing.Size)(resources.GetObject("editAssemblyButton.Size")));
			this.editAssemblyButton.TabIndex = ((int)(resources.GetObject("editAssemblyButton.TabIndex")));
			this.editAssemblyButton.Text = resources.GetString("editAssemblyButton.Text");
			this.editAssemblyButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("editAssemblyButton.TextAlign")));
			this.editAssemblyButton.Visible = ((bool)(resources.GetObject("editAssemblyButton.Visible")));
			this.editAssemblyButton.Click += new System.EventHandler(this.editAssemblyButton_Click);
			// 
			// addAssemblyButton
			// 
			this.addAssemblyButton.AccessibleDescription = resources.GetString("addAssemblyButton.AccessibleDescription");
			this.addAssemblyButton.AccessibleName = resources.GetString("addAssemblyButton.AccessibleName");
			this.addAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("addAssemblyButton.Anchor")));
			this.addAssemblyButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("addAssemblyButton.BackgroundImage")));
			this.addAssemblyButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("addAssemblyButton.Dock")));
			this.addAssemblyButton.Enabled = ((bool)(resources.GetObject("addAssemblyButton.Enabled")));
			this.addAssemblyButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("addAssemblyButton.FlatStyle")));
			this.addAssemblyButton.Font = ((System.Drawing.Font)(resources.GetObject("addAssemblyButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.addAssemblyButton, resources.GetString("addAssemblyButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.addAssemblyButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("addAssemblyButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.addAssemblyButton, resources.GetString("addAssemblyButton.HelpString"));
			this.addAssemblyButton.Image = ((System.Drawing.Image)(resources.GetObject("addAssemblyButton.Image")));
			this.addAssemblyButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("addAssemblyButton.ImageAlign")));
			this.addAssemblyButton.ImageIndex = ((int)(resources.GetObject("addAssemblyButton.ImageIndex")));
			this.addAssemblyButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("addAssemblyButton.ImeMode")));
			this.addAssemblyButton.Location = ((System.Drawing.Point)(resources.GetObject("addAssemblyButton.Location")));
			this.addAssemblyButton.Name = "addAssemblyButton";
			this.addAssemblyButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("addAssemblyButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.addAssemblyButton, ((bool)(resources.GetObject("addAssemblyButton.ShowHelp"))));
			this.addAssemblyButton.Size = ((System.Drawing.Size)(resources.GetObject("addAssemblyButton.Size")));
			this.addAssemblyButton.TabIndex = ((int)(resources.GetObject("addAssemblyButton.TabIndex")));
			this.addAssemblyButton.Text = resources.GetString("addAssemblyButton.Text");
			this.addAssemblyButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("addAssemblyButton.TextAlign")));
			this.addAssemblyButton.Visible = ((bool)(resources.GetObject("addAssemblyButton.Visible")));
			this.addAssemblyButton.Click += new System.EventHandler(this.addAssemblyButton_Click);
			// 
			// deleteAssemblyButton
			// 
			this.deleteAssemblyButton.AccessibleDescription = resources.GetString("deleteAssemblyButton.AccessibleDescription");
			this.deleteAssemblyButton.AccessibleName = resources.GetString("deleteAssemblyButton.AccessibleName");
			this.deleteAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("deleteAssemblyButton.Anchor")));
			this.deleteAssemblyButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("deleteAssemblyButton.BackgroundImage")));
			this.deleteAssemblyButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("deleteAssemblyButton.Dock")));
			this.deleteAssemblyButton.Enabled = ((bool)(resources.GetObject("deleteAssemblyButton.Enabled")));
			this.deleteAssemblyButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("deleteAssemblyButton.FlatStyle")));
			this.deleteAssemblyButton.Font = ((System.Drawing.Font)(resources.GetObject("deleteAssemblyButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.deleteAssemblyButton, resources.GetString("deleteAssemblyButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.deleteAssemblyButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("deleteAssemblyButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.deleteAssemblyButton, resources.GetString("deleteAssemblyButton.HelpString"));
			this.deleteAssemblyButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteAssemblyButton.Image")));
			this.deleteAssemblyButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("deleteAssemblyButton.ImageAlign")));
			this.deleteAssemblyButton.ImageIndex = ((int)(resources.GetObject("deleteAssemblyButton.ImageIndex")));
			this.deleteAssemblyButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("deleteAssemblyButton.ImeMode")));
			this.deleteAssemblyButton.Location = ((System.Drawing.Point)(resources.GetObject("deleteAssemblyButton.Location")));
			this.deleteAssemblyButton.Name = "deleteAssemblyButton";
			this.deleteAssemblyButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("deleteAssemblyButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.deleteAssemblyButton, ((bool)(resources.GetObject("deleteAssemblyButton.ShowHelp"))));
			this.deleteAssemblyButton.Size = ((System.Drawing.Size)(resources.GetObject("deleteAssemblyButton.Size")));
			this.deleteAssemblyButton.TabIndex = ((int)(resources.GetObject("deleteAssemblyButton.TabIndex")));
			this.deleteAssemblyButton.Text = resources.GetString("deleteAssemblyButton.Text");
			this.deleteAssemblyButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("deleteAssemblyButton.TextAlign")));
			this.deleteAssemblyButton.Visible = ((bool)(resources.GetObject("deleteAssemblyButton.Visible")));
			this.deleteAssemblyButton.Click += new System.EventHandler(this.deleteAssemblyButton_Click);
			// 
			// label2
			// 
			this.label2.AccessibleDescription = resources.GetString("label2.AccessibleDescription");
			this.label2.AccessibleName = resources.GetString("label2.AccessibleName");
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label2.Anchor")));
			this.label2.AutoSize = ((bool)(resources.GetObject("label2.AutoSize")));
			this.label2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label2.Dock")));
			this.label2.Enabled = ((bool)(resources.GetObject("label2.Enabled")));
			this.label2.Font = ((System.Drawing.Font)(resources.GetObject("label2.Font")));
			this.helpProvider1.SetHelpKeyword(this.label2, resources.GetString("label2.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.label2, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("label2.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.label2, resources.GetString("label2.HelpString"));
			this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
			this.label2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.ImageAlign")));
			this.label2.ImageIndex = ((int)(resources.GetObject("label2.ImageIndex")));
			this.label2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label2.ImeMode")));
			this.label2.Location = ((System.Drawing.Point)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label2.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.label2, ((bool)(resources.GetObject("label2.ShowHelp"))));
			this.label2.Size = ((System.Drawing.Size)(resources.GetObject("label2.Size")));
			this.label2.TabIndex = ((int)(resources.GetObject("label2.TabIndex")));
			this.label2.Text = resources.GetString("label2.Text");
			this.label2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.TextAlign")));
			this.label2.Visible = ((bool)(resources.GetObject("label2.Visible")));
			// 
			// fileNameHeader
			// 
			this.fileNameHeader.Text = resources.GetString("fileNameHeader.Text");
			this.fileNameHeader.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("fileNameHeader.TextAlign")));
			this.fileNameHeader.Width = ((int)(resources.GetObject("fileNameHeader.Width")));
			// 
			// fullPathHeader
			// 
			this.fullPathHeader.Text = resources.GetString("fullPathHeader.Text");
			this.fullPathHeader.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("fullPathHeader.TextAlign")));
			this.fullPathHeader.Width = ((int)(resources.GetObject("fullPathHeader.Width")));
			// 
			// closeButton
			// 
			this.closeButton.AccessibleDescription = resources.GetString("closeButton.AccessibleDescription");
			this.closeButton.AccessibleName = resources.GetString("closeButton.AccessibleName");
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("closeButton.Anchor")));
			this.closeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("closeButton.BackgroundImage")));
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("closeButton.Dock")));
			this.closeButton.Enabled = ((bool)(resources.GetObject("closeButton.Enabled")));
			this.closeButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("closeButton.FlatStyle")));
			this.closeButton.Font = ((System.Drawing.Font)(resources.GetObject("closeButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.closeButton, resources.GetString("closeButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.closeButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("closeButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.closeButton, resources.GetString("closeButton.HelpString"));
			this.closeButton.Image = ((System.Drawing.Image)(resources.GetObject("closeButton.Image")));
			this.closeButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("closeButton.ImageAlign")));
			this.closeButton.ImageIndex = ((int)(resources.GetObject("closeButton.ImageIndex")));
			this.closeButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("closeButton.ImeMode")));
			this.closeButton.Location = ((System.Drawing.Point)(resources.GetObject("closeButton.Location")));
			this.closeButton.Name = "closeButton";
			this.closeButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("closeButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.closeButton, ((bool)(resources.GetObject("closeButton.ShowHelp"))));
			this.closeButton.Size = ((System.Drawing.Size)(resources.GetObject("closeButton.Size")));
			this.closeButton.TabIndex = ((int)(resources.GetObject("closeButton.TabIndex")));
			this.closeButton.Text = resources.GetString("closeButton.Text");
			this.closeButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("closeButton.TextAlign")));
			this.closeButton.Visible = ((bool)(resources.GetObject("closeButton.Visible")));
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// helpProvider1
			// 
			this.helpProvider1.HelpNamespace = resources.GetString("helpProvider1.HelpNamespace");
			// 
			// label5
			// 
			this.label5.AccessibleDescription = resources.GetString("label5.AccessibleDescription");
			this.label5.AccessibleName = resources.GetString("label5.AccessibleName");
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label5.Anchor")));
			this.label5.AutoSize = ((bool)(resources.GetObject("label5.AutoSize")));
			this.label5.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label5.Dock")));
			this.label5.Enabled = ((bool)(resources.GetObject("label5.Enabled")));
			this.label5.Font = ((System.Drawing.Font)(resources.GetObject("label5.Font")));
			this.helpProvider1.SetHelpKeyword(this.label5, resources.GetString("label5.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.label5, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("label5.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.label5, resources.GetString("label5.HelpString"));
			this.label5.Image = ((System.Drawing.Image)(resources.GetObject("label5.Image")));
			this.label5.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.ImageAlign")));
			this.label5.ImageIndex = ((int)(resources.GetObject("label5.ImageIndex")));
			this.label5.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label5.ImeMode")));
			this.label5.Location = ((System.Drawing.Point)(resources.GetObject("label5.Location")));
			this.label5.Name = "label5";
			this.label5.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label5.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.label5, ((bool)(resources.GetObject("label5.ShowHelp"))));
			this.label5.Size = ((System.Drawing.Size)(resources.GetObject("label5.Size")));
			this.label5.TabIndex = ((int)(resources.GetObject("label5.TabIndex")));
			this.label5.Text = resources.GetString("label5.Text");
			this.label5.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.TextAlign")));
			this.label5.Visible = ((bool)(resources.GetObject("label5.Visible")));
			// 
			// projectPathLabel
			// 
			this.projectPathLabel.AccessibleDescription = resources.GetString("projectPathLabel.AccessibleDescription");
			this.projectPathLabel.AccessibleName = resources.GetString("projectPathLabel.AccessibleName");
			this.projectPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("projectPathLabel.Anchor")));
			this.projectPathLabel.AutoSize = ((bool)(resources.GetObject("projectPathLabel.AutoSize")));
			this.projectPathLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("projectPathLabel.Dock")));
			this.projectPathLabel.Enabled = ((bool)(resources.GetObject("projectPathLabel.Enabled")));
			this.projectPathLabel.Font = ((System.Drawing.Font)(resources.GetObject("projectPathLabel.Font")));
			this.helpProvider1.SetHelpKeyword(this.projectPathLabel, resources.GetString("projectPathLabel.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.projectPathLabel, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("projectPathLabel.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.projectPathLabel, resources.GetString("projectPathLabel.HelpString"));
			this.projectPathLabel.Image = ((System.Drawing.Image)(resources.GetObject("projectPathLabel.Image")));
			this.projectPathLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("projectPathLabel.ImageAlign")));
			this.projectPathLabel.ImageIndex = ((int)(resources.GetObject("projectPathLabel.ImageIndex")));
			this.projectPathLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("projectPathLabel.ImeMode")));
			this.projectPathLabel.Location = ((System.Drawing.Point)(resources.GetObject("projectPathLabel.Location")));
			this.projectPathLabel.Name = "projectPathLabel";
			this.projectPathLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("projectPathLabel.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.projectPathLabel, ((bool)(resources.GetObject("projectPathLabel.ShowHelp"))));
			this.projectPathLabel.Size = ((System.Drawing.Size)(resources.GetObject("projectPathLabel.Size")));
			this.projectPathLabel.TabIndex = ((int)(resources.GetObject("projectPathLabel.TabIndex")));
			this.projectPathLabel.Text = resources.GetString("projectPathLabel.Text");
			this.projectPathLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("projectPathLabel.TextAlign")));
			this.projectPathLabel.Visible = ((bool)(resources.GetObject("projectPathLabel.Visible")));
			// 
			// label8
			// 
			this.label8.AccessibleDescription = resources.GetString("label8.AccessibleDescription");
			this.label8.AccessibleName = resources.GetString("label8.AccessibleName");
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label8.Anchor")));
			this.label8.AutoSize = ((bool)(resources.GetObject("label8.AutoSize")));
			this.label8.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label8.Dock")));
			this.label8.Enabled = ((bool)(resources.GetObject("label8.Enabled")));
			this.label8.Font = ((System.Drawing.Font)(resources.GetObject("label8.Font")));
			this.helpProvider1.SetHelpKeyword(this.label8, resources.GetString("label8.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.label8, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("label8.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.label8, resources.GetString("label8.HelpString"));
			this.label8.Image = ((System.Drawing.Image)(resources.GetObject("label8.Image")));
			this.label8.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label8.ImageAlign")));
			this.label8.ImageIndex = ((int)(resources.GetObject("label8.ImageIndex")));
			this.label8.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label8.ImeMode")));
			this.label8.Location = ((System.Drawing.Point)(resources.GetObject("label8.Location")));
			this.label8.Name = "label8";
			this.label8.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label8.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.label8, ((bool)(resources.GetObject("label8.ShowHelp"))));
			this.label8.Size = ((System.Drawing.Size)(resources.GetObject("label8.Size")));
			this.label8.TabIndex = ((int)(resources.GetObject("label8.TabIndex")));
			this.label8.Text = resources.GetString("label8.Text");
			this.label8.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label8.TextAlign")));
			this.label8.Visible = ((bool)(resources.GetObject("label8.Visible")));
			// 
			// projectBaseTextBox
			// 
			this.projectBaseTextBox.AccessibleDescription = resources.GetString("projectBaseTextBox.AccessibleDescription");
			this.projectBaseTextBox.AccessibleName = resources.GetString("projectBaseTextBox.AccessibleName");
			this.projectBaseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("projectBaseTextBox.Anchor")));
			this.projectBaseTextBox.AutoSize = ((bool)(resources.GetObject("projectBaseTextBox.AutoSize")));
			this.projectBaseTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("projectBaseTextBox.BackgroundImage")));
			this.projectBaseTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("projectBaseTextBox.Dock")));
			this.projectBaseTextBox.Enabled = ((bool)(resources.GetObject("projectBaseTextBox.Enabled")));
			this.projectBaseTextBox.Font = ((System.Drawing.Font)(resources.GetObject("projectBaseTextBox.Font")));
			this.helpProvider1.SetHelpKeyword(this.projectBaseTextBox, resources.GetString("projectBaseTextBox.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.projectBaseTextBox, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("projectBaseTextBox.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.projectBaseTextBox, resources.GetString("projectBaseTextBox.HelpString"));
			this.projectBaseTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("projectBaseTextBox.ImeMode")));
			this.projectBaseTextBox.Location = ((System.Drawing.Point)(resources.GetObject("projectBaseTextBox.Location")));
			this.projectBaseTextBox.MaxLength = ((int)(resources.GetObject("projectBaseTextBox.MaxLength")));
			this.projectBaseTextBox.Multiline = ((bool)(resources.GetObject("projectBaseTextBox.Multiline")));
			this.projectBaseTextBox.Name = "projectBaseTextBox";
			this.projectBaseTextBox.PasswordChar = ((char)(resources.GetObject("projectBaseTextBox.PasswordChar")));
			this.projectBaseTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("projectBaseTextBox.RightToLeft")));
			this.projectBaseTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("projectBaseTextBox.ScrollBars")));
			this.helpProvider1.SetShowHelp(this.projectBaseTextBox, ((bool)(resources.GetObject("projectBaseTextBox.ShowHelp"))));
			this.projectBaseTextBox.Size = ((System.Drawing.Size)(resources.GetObject("projectBaseTextBox.Size")));
			this.projectBaseTextBox.TabIndex = ((int)(resources.GetObject("projectBaseTextBox.TabIndex")));
			this.projectBaseTextBox.Text = resources.GetString("projectBaseTextBox.Text");
			this.projectBaseTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("projectBaseTextBox.TextAlign")));
			this.projectBaseTextBox.Visible = ((bool)(resources.GetObject("projectBaseTextBox.Visible")));
			this.projectBaseTextBox.WordWrap = ((bool)(resources.GetObject("projectBaseTextBox.WordWrap")));
			this.projectBaseTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.projectBaseTextBox_Validating);
			this.projectBaseTextBox.Validated += new System.EventHandler(this.projectBaseTextBox_Validated);
			// 
			// ProjectEditor
			// 
			this.AcceptButton = this.closeButton;
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.CancelButton = this.closeButton;
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.projectBaseTextBox);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.projectPathLabel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.projectTabControl);
			this.Controls.Add(this.editConfigsButton);
			this.Controls.Add(this.configComboBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.closeButton);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.helpProvider1.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this, resources.GetString("$this.HelpString"));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximizeBox = false;
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimizeBox = false;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "ProjectEditor";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.helpProvider1.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
			this.ShowInTaskbar = false;
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.TransparencyKey = System.Drawing.Color.Green;
			this.Load += new System.EventHandler(this.ProjectEditor_Load);
			this.projectTabControl.ResumeLayout(false);
			this.generalTabPage.ResumeLayout(false);
			this.assemblyTabPage.ResumeLayout(false);
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

//		private UserSettings _userSettings;
//		private UserSettings UserSettings
//		{
//			get
//			{
//				if ( _userSettings == null )
//					_userSettings = (UserSettings)GetService( typeof( UserSettings ) );
//				//TODO: Remove this, or create the service in the test
//				if ( _userSettings == null )
//					_userSettings = new UserSettings(
//						new RegistrySettingsStorage( NUnitRegistry.CurrentUser ) );
//				return _userSettings;
//			}
//		}

		private TestLoaderUI TestLoaderUI
		{
			get
			{
				return (TestLoaderUI)GetService( typeof( TestLoaderUI ) );
			}
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
		
			addAssemblyButton.Enabled = editAssemblyButton.Enabled = deleteAssemblyButton.Enabled = project.Configs.Count > 0;
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

		#region Project Base
		private void projectBaseTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( projectBaseTextBox.Text != string.Empty )
			{
				string projectBase = null;
				try
				{
					projectBase = projectBaseTextBox.Text;
					DirectoryInfo info = new DirectoryInfo( projectBase );
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
			if ( projectBaseTextBox.Text == string.Empty )
				projectBaseTextBox.Text = project.DefaultBasePath;

			project.BasePath = projectBaseTextBox.Text;
		}
		#endregion

		#region Configuration ApplicationBase
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

		#region Config File
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

		#region PrivateBinPath
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

		#endregion

		#region Other UI Events

		private void ProjectEditor_Load(object sender, System.EventArgs e)
		{
			this.Text = string.Format( "{0} - Project Editor", 
				project.Name );

			projectPathLabel.Text = project.ProjectPath;

			projectBaseTextBox.Text = project.BasePath;

			addVSProjectButton.Visible = this.VisualStudioSupport;

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
			TestLoaderUI.AddAssembly( this, selectedConfig.Name );
			assemblyListBox_Populate();
		}

		private void addVSProjectButton_Click(object sender, System.EventArgs e)
		{
			TestLoaderUI.AddVSProject( this );
			configComboBox_Populate();
		}

		private void editAssemblyButton_Click(object sender, System.EventArgs e)
		{
			using( AssemblyPathDialog dlg = new AssemblyPathDialog( 
					   (string)selectedConfig.Assemblies[assemblyListBox.SelectedIndex].FullPath ) )
			{
				this.Site.Container.Add( dlg );
				if ( dlg.ShowDialog() == DialogResult.OK )
				{
					selectedConfig.Assemblies[assemblyListBox.SelectedIndex].FullPath = dlg.Path;
					assemblyListBox_Populate();
				}
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
