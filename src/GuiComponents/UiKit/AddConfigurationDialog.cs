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

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for AssemblyNameDialog.
	/// </summary>
	public class AddConfigurationDialog : System.Windows.Forms.Form
	{
		#region Instance variables

		private NUnitProject project;
		private string configurationName;
		private string copyConfigurationName;

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox configurationNameTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox configurationComboBox;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Construction and Disposal

		public AddConfigurationDialog( NUnitProject project )
		{ 
			InitializeComponent();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AddConfigurationDialog));
			this.configurationNameTextBox = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.configurationComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// configurationNameTextBox
			// 
			this.configurationNameTextBox.AccessibleDescription = resources.GetString("configurationNameTextBox.AccessibleDescription");
			this.configurationNameTextBox.AccessibleName = resources.GetString("configurationNameTextBox.AccessibleName");
			this.configurationNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("configurationNameTextBox.Anchor")));
			this.configurationNameTextBox.AutoSize = ((bool)(resources.GetObject("configurationNameTextBox.AutoSize")));
			this.configurationNameTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("configurationNameTextBox.BackgroundImage")));
			this.configurationNameTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("configurationNameTextBox.Dock")));
			this.configurationNameTextBox.Enabled = ((bool)(resources.GetObject("configurationNameTextBox.Enabled")));
			this.configurationNameTextBox.Font = ((System.Drawing.Font)(resources.GetObject("configurationNameTextBox.Font")));
			this.configurationNameTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("configurationNameTextBox.ImeMode")));
			this.configurationNameTextBox.Location = ((System.Drawing.Point)(resources.GetObject("configurationNameTextBox.Location")));
			this.configurationNameTextBox.MaxLength = ((int)(resources.GetObject("configurationNameTextBox.MaxLength")));
			this.configurationNameTextBox.Multiline = ((bool)(resources.GetObject("configurationNameTextBox.Multiline")));
			this.configurationNameTextBox.Name = "configurationNameTextBox";
			this.configurationNameTextBox.PasswordChar = ((char)(resources.GetObject("configurationNameTextBox.PasswordChar")));
			this.configurationNameTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("configurationNameTextBox.RightToLeft")));
			this.configurationNameTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("configurationNameTextBox.ScrollBars")));
			this.configurationNameTextBox.Size = ((System.Drawing.Size)(resources.GetObject("configurationNameTextBox.Size")));
			this.configurationNameTextBox.TabIndex = ((int)(resources.GetObject("configurationNameTextBox.TabIndex")));
			this.configurationNameTextBox.Text = resources.GetString("configurationNameTextBox.Text");
			this.configurationNameTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("configurationNameTextBox.TextAlign")));
			this.configurationNameTextBox.Visible = ((bool)(resources.GetObject("configurationNameTextBox.Visible")));
			this.configurationNameTextBox.WordWrap = ((bool)(resources.GetObject("configurationNameTextBox.WordWrap")));
			// 
			// okButton
			// 
			this.okButton.AccessibleDescription = resources.GetString("okButton.AccessibleDescription");
			this.okButton.AccessibleName = resources.GetString("okButton.AccessibleName");
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("okButton.Anchor")));
			this.okButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("okButton.BackgroundImage")));
			this.okButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("okButton.Dock")));
			this.okButton.Enabled = ((bool)(resources.GetObject("okButton.Enabled")));
			this.okButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("okButton.FlatStyle")));
			this.okButton.Font = ((System.Drawing.Font)(resources.GetObject("okButton.Font")));
			this.okButton.Image = ((System.Drawing.Image)(resources.GetObject("okButton.Image")));
			this.okButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("okButton.ImageAlign")));
			this.okButton.ImageIndex = ((int)(resources.GetObject("okButton.ImageIndex")));
			this.okButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("okButton.ImeMode")));
			this.okButton.Location = ((System.Drawing.Point)(resources.GetObject("okButton.Location")));
			this.okButton.Name = "okButton";
			this.okButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("okButton.RightToLeft")));
			this.okButton.Size = ((System.Drawing.Size)(resources.GetObject("okButton.Size")));
			this.okButton.TabIndex = ((int)(resources.GetObject("okButton.TabIndex")));
			this.okButton.Text = resources.GetString("okButton.Text");
			this.okButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("okButton.TextAlign")));
			this.okButton.Visible = ((bool)(resources.GetObject("okButton.Visible")));
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.AccessibleDescription = resources.GetString("cancelButton.AccessibleDescription");
			this.cancelButton.AccessibleName = resources.GetString("cancelButton.AccessibleName");
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cancelButton.Anchor")));
			this.cancelButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cancelButton.BackgroundImage")));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cancelButton.Dock")));
			this.cancelButton.Enabled = ((bool)(resources.GetObject("cancelButton.Enabled")));
			this.cancelButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("cancelButton.FlatStyle")));
			this.cancelButton.Font = ((System.Drawing.Font)(resources.GetObject("cancelButton.Font")));
			this.cancelButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelButton.Image")));
			this.cancelButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.ImageAlign")));
			this.cancelButton.ImageIndex = ((int)(resources.GetObject("cancelButton.ImageIndex")));
			this.cancelButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cancelButton.ImeMode")));
			this.cancelButton.Location = ((System.Drawing.Point)(resources.GetObject("cancelButton.Location")));
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cancelButton.RightToLeft")));
			this.cancelButton.Size = ((System.Drawing.Size)(resources.GetObject("cancelButton.Size")));
			this.cancelButton.TabIndex = ((int)(resources.GetObject("cancelButton.TabIndex")));
			this.cancelButton.Text = resources.GetString("cancelButton.Text");
			this.cancelButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.TextAlign")));
			this.cancelButton.Visible = ((bool)(resources.GetObject("cancelButton.Visible")));
			// 
			// configurationComboBox
			// 
			this.configurationComboBox.AccessibleDescription = resources.GetString("configurationComboBox.AccessibleDescription");
			this.configurationComboBox.AccessibleName = resources.GetString("configurationComboBox.AccessibleName");
			this.configurationComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("configurationComboBox.Anchor")));
			this.configurationComboBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("configurationComboBox.BackgroundImage")));
			this.configurationComboBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("configurationComboBox.Dock")));
			this.configurationComboBox.Enabled = ((bool)(resources.GetObject("configurationComboBox.Enabled")));
			this.configurationComboBox.Font = ((System.Drawing.Font)(resources.GetObject("configurationComboBox.Font")));
			this.configurationComboBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("configurationComboBox.ImeMode")));
			this.configurationComboBox.IntegralHeight = ((bool)(resources.GetObject("configurationComboBox.IntegralHeight")));
			this.configurationComboBox.ItemHeight = ((int)(resources.GetObject("configurationComboBox.ItemHeight")));
			this.configurationComboBox.Location = ((System.Drawing.Point)(resources.GetObject("configurationComboBox.Location")));
			this.configurationComboBox.MaxDropDownItems = ((int)(resources.GetObject("configurationComboBox.MaxDropDownItems")));
			this.configurationComboBox.MaxLength = ((int)(resources.GetObject("configurationComboBox.MaxLength")));
			this.configurationComboBox.Name = "configurationComboBox";
			this.configurationComboBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("configurationComboBox.RightToLeft")));
			this.configurationComboBox.Size = ((System.Drawing.Size)(resources.GetObject("configurationComboBox.Size")));
			this.configurationComboBox.TabIndex = ((int)(resources.GetObject("configurationComboBox.TabIndex")));
			this.configurationComboBox.Text = resources.GetString("configurationComboBox.Text");
			this.configurationComboBox.Visible = ((bool)(resources.GetObject("configurationComboBox.Visible")));
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
			this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
			this.label1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.ImageAlign")));
			this.label1.ImageIndex = ((int)(resources.GetObject("label1.ImageIndex")));
			this.label1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label1.ImeMode")));
			this.label1.Location = ((System.Drawing.Point)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label1.RightToLeft")));
			this.label1.Size = ((System.Drawing.Size)(resources.GetObject("label1.Size")));
			this.label1.TabIndex = ((int)(resources.GetObject("label1.TabIndex")));
			this.label1.Text = resources.GetString("label1.Text");
			this.label1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.TextAlign")));
			this.label1.Visible = ((bool)(resources.GetObject("label1.Visible")));
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
			this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
			this.label2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.ImageAlign")));
			this.label2.ImageIndex = ((int)(resources.GetObject("label2.ImageIndex")));
			this.label2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label2.ImeMode")));
			this.label2.Location = ((System.Drawing.Point)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label2.RightToLeft")));
			this.label2.Size = ((System.Drawing.Size)(resources.GetObject("label2.Size")));
			this.label2.TabIndex = ((int)(resources.GetObject("label2.TabIndex")));
			this.label2.Text = resources.GetString("label2.Text");
			this.label2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.TextAlign")));
			this.label2.Visible = ((bool)(resources.GetObject("label2.Visible")));
			// 
			// AddConfigurationDialog
			// 
			this.AcceptButton = this.okButton;
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.CancelButton = this.cancelButton;
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.configurationComboBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.configurationNameTextBox);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "AddConfigurationDialog";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.ShowInTaskbar = false;
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Load += new System.EventHandler(this.ConfigurationNameDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		public string ConfigurationName
		{
			get { return configurationName; }
		}

		public string CopyConfigurationName
		{
			get { return copyConfigurationName; }
		}

		#endregion

		#region Methods

		private void ConfigurationNameDialog_Load(object sender, System.EventArgs e)
		{
			configurationComboBox.Items.Add( "<none>" );
			configurationComboBox.SelectedIndex = 0;

			foreach( ProjectConfig config in project.Configs )
			{
				int index = configurationComboBox.Items.Add( config.Name );
				if ( config.Name == project.ActiveConfigName )
					configurationComboBox.SelectedIndex = index;
			}
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			configurationName = configurationNameTextBox.Text;

			if ( configurationName == string.Empty )
			{
				UserMessage.Display( "No configuration name provided", "Configuration Name Error" );
				return;
			}

			if ( project.Configs.Contains( configurationName ) )
			{
				// TODO: Need general error message display
				UserMessage.Display( "A configuration with that name already exists", "Configuration Name Error" );
				return;
			}

			// ToDo: Move more of this to project
			ProjectConfig newConfig = new ProjectConfig( configurationName );
				
			copyConfigurationName = null;
			if ( configurationComboBox.SelectedIndex > 0 )
			{		
				copyConfigurationName = (string)configurationComboBox.SelectedItem;
				ProjectConfig copyConfig = project.Configs[copyConfigurationName];
				if ( copyConfig != null )
					foreach( AssemblyListItem item in copyConfig.Assemblies )
						newConfig.Assemblies.Add( item.FullPath );
			}

			project.Configs.Add( newConfig );
			DialogResult = DialogResult.OK;

			Close();
		}

		#endregion
	}
}
