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
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NUnit.Util;

namespace NUnit.UiKit
{
	/// <summary>
	/// ConfigurationEditor form is designed for adding, deleting
	/// and renaming configurations from a project.
	/// </summary>
	public class ConfigurationEditor : System.Windows.Forms.Form
	{
		#region Instance Variables

		private NUnitProject project;

		private int selectedIndex = -1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListBox configListBox;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button renameButton;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button activeButton;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.Button closeButton;

		#endregion

		#region Construction and Disposal

		public ConfigurationEditor( NUnitProject project )
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ConfigurationEditor));
			this.configListBox = new System.Windows.Forms.ListBox();
			this.removeButton = new System.Windows.Forms.Button();
			this.renameButton = new System.Windows.Forms.Button();
			this.closeButton = new System.Windows.Forms.Button();
			this.addButton = new System.Windows.Forms.Button();
			this.activeButton = new System.Windows.Forms.Button();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.SuspendLayout();
			// 
			// configListBox
			// 
			this.configListBox.AccessibleDescription = resources.GetString("configListBox.AccessibleDescription");
			this.configListBox.AccessibleName = resources.GetString("configListBox.AccessibleName");
			this.configListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("configListBox.Anchor")));
			this.configListBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("configListBox.BackgroundImage")));
			this.configListBox.ColumnWidth = ((int)(resources.GetObject("configListBox.ColumnWidth")));
			this.configListBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("configListBox.Dock")));
			this.configListBox.Enabled = ((bool)(resources.GetObject("configListBox.Enabled")));
			this.configListBox.Font = ((System.Drawing.Font)(resources.GetObject("configListBox.Font")));
			this.helpProvider1.SetHelpKeyword(this.configListBox, resources.GetString("configListBox.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.configListBox, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("configListBox.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.configListBox, resources.GetString("configListBox.HelpString"));
			this.configListBox.HorizontalExtent = ((int)(resources.GetObject("configListBox.HorizontalExtent")));
			this.configListBox.HorizontalScrollbar = ((bool)(resources.GetObject("configListBox.HorizontalScrollbar")));
			this.configListBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("configListBox.ImeMode")));
			this.configListBox.IntegralHeight = ((bool)(resources.GetObject("configListBox.IntegralHeight")));
			this.configListBox.ItemHeight = ((int)(resources.GetObject("configListBox.ItemHeight")));
			this.configListBox.Location = ((System.Drawing.Point)(resources.GetObject("configListBox.Location")));
			this.configListBox.Name = "configListBox";
			this.configListBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("configListBox.RightToLeft")));
			this.configListBox.ScrollAlwaysVisible = ((bool)(resources.GetObject("configListBox.ScrollAlwaysVisible")));
			this.helpProvider1.SetShowHelp(this.configListBox, ((bool)(resources.GetObject("configListBox.ShowHelp"))));
			this.configListBox.Size = ((System.Drawing.Size)(resources.GetObject("configListBox.Size")));
			this.configListBox.TabIndex = ((int)(resources.GetObject("configListBox.TabIndex")));
			this.configListBox.Visible = ((bool)(resources.GetObject("configListBox.Visible")));
			this.configListBox.SelectedIndexChanged += new System.EventHandler(this.configListBox_SelectedIndexChanged);
			// 
			// removeButton
			// 
			this.removeButton.AccessibleDescription = resources.GetString("removeButton.AccessibleDescription");
			this.removeButton.AccessibleName = resources.GetString("removeButton.AccessibleName");
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("removeButton.Anchor")));
			this.removeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("removeButton.BackgroundImage")));
			this.removeButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("removeButton.Dock")));
			this.removeButton.Enabled = ((bool)(resources.GetObject("removeButton.Enabled")));
			this.removeButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("removeButton.FlatStyle")));
			this.removeButton.Font = ((System.Drawing.Font)(resources.GetObject("removeButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.removeButton, resources.GetString("removeButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.removeButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("removeButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.removeButton, resources.GetString("removeButton.HelpString"));
			this.removeButton.Image = ((System.Drawing.Image)(resources.GetObject("removeButton.Image")));
			this.removeButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("removeButton.ImageAlign")));
			this.removeButton.ImageIndex = ((int)(resources.GetObject("removeButton.ImageIndex")));
			this.removeButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("removeButton.ImeMode")));
			this.removeButton.Location = ((System.Drawing.Point)(resources.GetObject("removeButton.Location")));
			this.removeButton.Name = "removeButton";
			this.removeButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("removeButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.removeButton, ((bool)(resources.GetObject("removeButton.ShowHelp"))));
			this.removeButton.Size = ((System.Drawing.Size)(resources.GetObject("removeButton.Size")));
			this.removeButton.TabIndex = ((int)(resources.GetObject("removeButton.TabIndex")));
			this.removeButton.Text = resources.GetString("removeButton.Text");
			this.removeButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("removeButton.TextAlign")));
			this.removeButton.Visible = ((bool)(resources.GetObject("removeButton.Visible")));
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// renameButton
			// 
			this.renameButton.AccessibleDescription = resources.GetString("renameButton.AccessibleDescription");
			this.renameButton.AccessibleName = resources.GetString("renameButton.AccessibleName");
			this.renameButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("renameButton.Anchor")));
			this.renameButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("renameButton.BackgroundImage")));
			this.renameButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("renameButton.Dock")));
			this.renameButton.Enabled = ((bool)(resources.GetObject("renameButton.Enabled")));
			this.renameButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("renameButton.FlatStyle")));
			this.renameButton.Font = ((System.Drawing.Font)(resources.GetObject("renameButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.renameButton, resources.GetString("renameButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.renameButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("renameButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.renameButton, resources.GetString("renameButton.HelpString"));
			this.renameButton.Image = ((System.Drawing.Image)(resources.GetObject("renameButton.Image")));
			this.renameButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("renameButton.ImageAlign")));
			this.renameButton.ImageIndex = ((int)(resources.GetObject("renameButton.ImageIndex")));
			this.renameButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("renameButton.ImeMode")));
			this.renameButton.Location = ((System.Drawing.Point)(resources.GetObject("renameButton.Location")));
			this.renameButton.Name = "renameButton";
			this.renameButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("renameButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.renameButton, ((bool)(resources.GetObject("renameButton.ShowHelp"))));
			this.renameButton.Size = ((System.Drawing.Size)(resources.GetObject("renameButton.Size")));
			this.renameButton.TabIndex = ((int)(resources.GetObject("renameButton.TabIndex")));
			this.renameButton.Text = resources.GetString("renameButton.Text");
			this.renameButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("renameButton.TextAlign")));
			this.renameButton.Visible = ((bool)(resources.GetObject("renameButton.Visible")));
			this.renameButton.Click += new System.EventHandler(this.renameButton_Click);
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
			this.closeButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// addButton
			// 
			this.addButton.AccessibleDescription = resources.GetString("addButton.AccessibleDescription");
			this.addButton.AccessibleName = resources.GetString("addButton.AccessibleName");
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("addButton.Anchor")));
			this.addButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("addButton.BackgroundImage")));
			this.addButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("addButton.Dock")));
			this.addButton.Enabled = ((bool)(resources.GetObject("addButton.Enabled")));
			this.addButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("addButton.FlatStyle")));
			this.addButton.Font = ((System.Drawing.Font)(resources.GetObject("addButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.addButton, resources.GetString("addButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.addButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("addButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.addButton, resources.GetString("addButton.HelpString"));
			this.addButton.Image = ((System.Drawing.Image)(resources.GetObject("addButton.Image")));
			this.addButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("addButton.ImageAlign")));
			this.addButton.ImageIndex = ((int)(resources.GetObject("addButton.ImageIndex")));
			this.addButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("addButton.ImeMode")));
			this.addButton.Location = ((System.Drawing.Point)(resources.GetObject("addButton.Location")));
			this.addButton.Name = "addButton";
			this.addButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("addButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.addButton, ((bool)(resources.GetObject("addButton.ShowHelp"))));
			this.addButton.Size = ((System.Drawing.Size)(resources.GetObject("addButton.Size")));
			this.addButton.TabIndex = ((int)(resources.GetObject("addButton.TabIndex")));
			this.addButton.Text = resources.GetString("addButton.Text");
			this.addButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("addButton.TextAlign")));
			this.addButton.Visible = ((bool)(resources.GetObject("addButton.Visible")));
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// activeButton
			// 
			this.activeButton.AccessibleDescription = resources.GetString("activeButton.AccessibleDescription");
			this.activeButton.AccessibleName = resources.GetString("activeButton.AccessibleName");
			this.activeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("activeButton.Anchor")));
			this.activeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("activeButton.BackgroundImage")));
			this.activeButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("activeButton.Dock")));
			this.activeButton.Enabled = ((bool)(resources.GetObject("activeButton.Enabled")));
			this.activeButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("activeButton.FlatStyle")));
			this.activeButton.Font = ((System.Drawing.Font)(resources.GetObject("activeButton.Font")));
			this.helpProvider1.SetHelpKeyword(this.activeButton, resources.GetString("activeButton.HelpKeyword"));
			this.helpProvider1.SetHelpNavigator(this.activeButton, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("activeButton.HelpNavigator"))));
			this.helpProvider1.SetHelpString(this.activeButton, resources.GetString("activeButton.HelpString"));
			this.activeButton.Image = ((System.Drawing.Image)(resources.GetObject("activeButton.Image")));
			this.activeButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("activeButton.ImageAlign")));
			this.activeButton.ImageIndex = ((int)(resources.GetObject("activeButton.ImageIndex")));
			this.activeButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("activeButton.ImeMode")));
			this.activeButton.Location = ((System.Drawing.Point)(resources.GetObject("activeButton.Location")));
			this.activeButton.Name = "activeButton";
			this.activeButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("activeButton.RightToLeft")));
			this.helpProvider1.SetShowHelp(this.activeButton, ((bool)(resources.GetObject("activeButton.ShowHelp"))));
			this.activeButton.Size = ((System.Drawing.Size)(resources.GetObject("activeButton.Size")));
			this.activeButton.TabIndex = ((int)(resources.GetObject("activeButton.TabIndex")));
			this.activeButton.Text = resources.GetString("activeButton.Text");
			this.activeButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("activeButton.TextAlign")));
			this.activeButton.Visible = ((bool)(resources.GetObject("activeButton.Visible")));
			this.activeButton.Click += new System.EventHandler(this.activeButton_Click);
			// 
			// helpProvider1
			// 
			this.helpProvider1.HelpNamespace = resources.GetString("helpProvider1.HelpNamespace");
			// 
			// ConfigurationEditor
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
			this.Controls.Add(this.activeButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.renameButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.configListBox);
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
			this.Name = "ConfigurationEditor";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.helpProvider1.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
			this.ShowInTaskbar = false;
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Load += new System.EventHandler(this.ConfigurationEditor_Load);
			this.ResumeLayout(false);

		}
		#endregion

		#region UI Event Handlers

		private void ConfigurationEditor_Load(object sender, System.EventArgs e)
		{
			FillListBox();
			if ( configListBox.Items.Count > 0 )
				configListBox.SelectedIndex = selectedIndex = 0;
		}

		private void removeButton_Click(object sender, System.EventArgs e)
		{	
			if ( project.Configs.Count == 1 )
			{
				string msg = "Removing the last configuration will make the project unloadable until you add another configuration.\r\rAre you sure you want to remove the configuration?";
				
				if( UserMessage.Ask( msg, "Remove Configuration" ) == DialogResult.No )
					return;
			}

			project.Configs.RemoveAt( selectedIndex );
			FillListBox();
		}

		private void renameButton_Click(object sender, System.EventArgs e)
		{
			RenameConfiguration( project.Configs[selectedIndex].Name );
		}

		private void addButton_Click(object sender, System.EventArgs e)
		{
			using( AddConfigurationDialog dlg = new AddConfigurationDialog( project ) )
			{
				this.Site.Container.Add( dlg );
				if ( dlg.ShowDialog() == DialogResult.OK )
					FillListBox();
			}
		}

		private void activeButton_Click(object sender, System.EventArgs e)
		{
			project.SetActiveConfig( selectedIndex );
			//AppUI.TestLoader.LoadConfig( project.Configs[selectedIndex].Name );
			FillListBox();
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void configListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			selectedIndex = configListBox.SelectedIndex;
			activeButton.Enabled = selectedIndex >= 0 && project.Configs[selectedIndex].Name != project.ActiveConfigName;
			renameButton.Enabled = addButton.Enabled = selectedIndex >= 0;
			removeButton.Enabled = selectedIndex >= 0 && configListBox.Items.Count > 0;
		}

		#endregion

		#region Helper Methods

		private void RenameConfiguration( string oldName )
		{
			using( RenameConfigurationDialog dlg = 
					   new RenameConfigurationDialog( project, oldName ) )
			{
				this.Site.Container.Add( dlg );
				if ( dlg.ShowDialog() == DialogResult.OK )
				{
					project.Configs[oldName].Name = dlg.ConfigurationName;
					FillListBox();
				}
			}
		}

		private void FillListBox()
		{
			configListBox.Items.Clear();
			int count = 0;

			foreach( ProjectConfig config in project.Configs )
			{
				string name = config.Name;

				if ( name == project.ActiveConfigName )
					name += " (active)";
				
				configListBox.Items.Add( name );
				count++;
			}	
		
			if ( count > 0 )
			{
				if( selectedIndex >= count )
					selectedIndex = count - 1;

				configListBox.SelectedIndex = selectedIndex;
			}
			else selectedIndex = -1;
		}

		#endregion

	}
}
