// ***********************************************************************
// Copyright (c) 2010 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace NUnit.ProjectEditor
{
	/// <summary>
	/// ConfigurationEditor form is designed for adding, deleting
	/// and renaming configurations from a project.
	/// </summary>
	public class ConfigurationEditorView : System.Windows.Forms.Form, IConfigurationEditorView
	{
		#region Instance Variables

        private string[] configList;
        private string activeConfigName;
        private ConfigurationEditor editor;

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

		public ConfigurationEditorView()
		{
			InitializeComponent();
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

        public ConfigurationEditor Editor
        {
            set { this.editor = value; }
        }

        #region Windows Form Designer generated code
        /// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ConfigurationEditorView));
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
			this.helpProvider1.SetHelpString(this.configListBox, "Selects the configuration to operate on.");
			this.configListBox.ItemHeight = 16;
			this.configListBox.Location = new System.Drawing.Point(8, 8);
			this.configListBox.Name = "configListBox";
			this.helpProvider1.SetShowHelp(this.configListBox, true);
			this.configListBox.Size = new System.Drawing.Size(168, 212);
			this.configListBox.TabIndex = 0;
			this.configListBox.SelectedIndexChanged += new System.EventHandler(this.configListBox_SelectedIndexChanged);
			// 
			// removeButton
			// 
			this.helpProvider1.SetHelpString(this.removeButton, "Removes the selected configuration");
			this.removeButton.Location = new System.Drawing.Point(192, 8);
			this.removeButton.Name = "removeButton";
			this.helpProvider1.SetShowHelp(this.removeButton, true);
			this.removeButton.Size = new System.Drawing.Size(96, 32);
			this.removeButton.TabIndex = 1;
			this.removeButton.Text = "&Remove";
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// renameButton
			// 
			this.helpProvider1.SetHelpString(this.renameButton, "Allows renaming the selected configuration");
			this.renameButton.Location = new System.Drawing.Point(192, 48);
			this.renameButton.Name = "renameButton";
			this.helpProvider1.SetShowHelp(this.renameButton, true);
			this.renameButton.Size = new System.Drawing.Size(96, 32);
			this.renameButton.TabIndex = 2;
			this.renameButton.Text = "Re&name...";
			this.renameButton.Click += new System.EventHandler(this.renameButton_Click);
			// 
			// closeButton
			// 
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.helpProvider1.SetHelpString(this.closeButton, "Closes this dialog");
			this.closeButton.Location = new System.Drawing.Point(192, 216);
			this.closeButton.Name = "closeButton";
			this.helpProvider1.SetShowHelp(this.closeButton, true);
			this.closeButton.Size = new System.Drawing.Size(96, 32);
			this.closeButton.TabIndex = 4;
			this.closeButton.Text = "Close";
			// 
			// addButton
			// 
			this.helpProvider1.SetHelpString(this.addButton, "Allows adding a new configuration");
			this.addButton.Location = new System.Drawing.Point(192, 88);
			this.addButton.Name = "addButton";
			this.helpProvider1.SetShowHelp(this.addButton, true);
			this.addButton.Size = new System.Drawing.Size(96, 32);
			this.addButton.TabIndex = 5;
			this.addButton.Text = "&Add...";
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// activeButton
			// 
			this.helpProvider1.SetHelpString(this.activeButton, "Makes the selected configuration active");
			this.activeButton.Location = new System.Drawing.Point(192, 128);
			this.activeButton.Name = "activeButton";
			this.helpProvider1.SetShowHelp(this.activeButton, true);
			this.activeButton.Size = new System.Drawing.Size(96, 32);
			this.activeButton.TabIndex = 6;
			this.activeButton.Text = "&Make Active";
			this.activeButton.Click += new System.EventHandler(this.activeButton_Click);
			// 
			// ConfigurationEditor
			// 
			this.AcceptButton = this.closeButton;
			this.CancelButton = this.closeButton;
			this.ClientSize = new System.Drawing.Size(297, 267);
			this.ControlBox = false;
			this.Controls.Add(this.activeButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.renameButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.configListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConfigurationEditor";
			this.helpProvider1.SetShowHelp(this, false);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Configuration Editor";
			this.ResumeLayout(false);

		}
		#endregion

        #region UI Event Handlers

		private void removeButton_Click(object sender, System.EventArgs e)
		{
            editor.RemoveConfig();
		}

		private void renameButton_Click(object sender, System.EventArgs e)
		{
            editor.RenameConfig();
		}

		private void addButton_Click(object sender, System.EventArgs e)
		{
            editor.AddConfig();
		}

		private void activeButton_Click(object sender, System.EventArgs e)
		{
            editor.MakeActive();
		}

		private void configListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (SelectedConfigChanged != null)
                SelectedConfigChanged(this, EventArgs.Empty);
		}

		#endregion

        #region IConfigurationEditorView Members

        public string[] ConfigList
        {
            set
            {
                this.configList = value;
                FillListBox();
            }
        }

        public string ActiveConfigName
        {
            set
            {
                activeConfigName = value;
                FillListBox();
            }
        }

        private void FillListBox()
        {
            configListBox.Items.Clear();

            foreach (string config in configList)
            {
                string item = config;
                if (item == activeConfigName)
                    item += " (active)";
                configListBox.Items.Add(item);
            }
        }

        public string SelectedConfig
        {
            get 
            { 
                return configListBox.SelectedIndex >= 0
                    ? configList[configListBox.SelectedIndex]
                    : null; 
            }
            set
            {
                for (int i = 0; i < configList.Length; i++)
                    if (configList[i] == value)
                    {
                        configListBox.SelectedIndex = i;
                        break;
                    }
            }
        }

        public bool AddConfigEnabled
        {
            set { addButton.Enabled = value; }
        }

        public bool RenameConfigEnabled
        {
            set { renameButton.Enabled = value; }
        }

        public bool RemoveConfigEnabled
        {
            set { removeButton.Enabled = value; }
        }

        public bool MakeActiveEnabled
        {
            set { activeButton.Enabled = value; }
        }

        public event EventHandler SelectedConfigChanged;

        public string GetNewNameForRename(string oldName)
        {
            string[] configList = new string[configListBox.Items.Count];
            for (int i = 0; i < configListBox.Items.Count; i++)
                configList[i] = configListBox.Items[i].ToString();

            using (RenameConfigurationDialog dlg =
                       new RenameConfigurationDialog(oldName, configList))
            {
                return dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK
                    ? dlg.ConfigurationName : null;
            }
        }

        public bool GetAddConfigData(ref AddConfigData data)
        {
            string[] configList = new string[configListBox.Items.Count];
            for (int i = 0; i < configListBox.Items.Count; i++)
                configList[i] = configListBox.Items[i].ToString();

            using (AddConfigurationDialog dlg = new AddConfigurationDialog(configList, (string)configListBox.SelectedItem))
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    data.ConfigToCreate = dlg.ConfigToCreate;
                    data.ConfigToCopy = dlg.ConfigToCopy;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
    }
}
