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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace NUnit.ProjectEditor
{
	/// <summary>
    /// Displays a dialog for creation of a new configuration.
    /// The dialog collects and validates the name and the
    /// name of a configuration to be copied and then adds the
    /// new configuration to the project.
    /// 
    /// A DialogResult of DialogResult.OK indicates that the
    /// configuration was added successfully.
    /// </summary>
	public class AddConfigurationDialog : System.Windows.Forms.Form
	{
		#region Instance variables

		private string[] configList;
        private string initialCopyConfig;

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

		public AddConfigurationDialog( string[] configList, string initialCopyConfig )
		{ 
			InitializeComponent();

			this.configList = configList;
            this.initialCopyConfig = initialCopyConfig;
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
            this.configurationNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationNameTextBox.Location = new System.Drawing.Point(16, 24);
            this.configurationNameTextBox.Name = "configurationNameTextBox";
            this.configurationNameTextBox.Size = new System.Drawing.Size(254, 22);
            this.configurationNameTextBox.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okButton.Location = new System.Drawing.Point(50, 120);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(76, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(155, 120);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            // 
            // configurationComboBox
            // 
            this.configurationComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationComboBox.ItemHeight = 16;
            this.configurationComboBox.Location = new System.Drawing.Point(16, 80);
            this.configurationComboBox.Name = "configurationComboBox";
            this.configurationComboBox.Size = new System.Drawing.Size(256, 24);
            this.configurationComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Configuration Name:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Copy Settings From:";
            // 
            // AddConfigurationDialog
            // 
            this.AcceptButton = this.okButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(280, 149);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.configurationComboBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.configurationNameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddConfigurationDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Configuration";
            this.Load += new System.EventHandler(this.ConfigurationNameDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Properties

		public string ConfigToCreate
		{
			get { return configurationNameTextBox.Text; }
		}

		public string ConfigToCopy
		{
			get 
            {
                int index = configurationComboBox.SelectedIndex;
                return index <= 0 ? null : configList[index-1];
            }
		}

		#endregion

		#region Methods

		private void ConfigurationNameDialog_Load(object sender, System.EventArgs e)
		{
			configurationComboBox.Items.Add( "<none>" );
			configurationComboBox.SelectedIndex = 0;

			foreach( string config in configList )
			{
				int index = configurationComboBox.Items.Add( config );
				if ( config == initialCopyConfig )
					configurationComboBox.SelectedIndex = index;
			}
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			if ( ConfigToCreate == string.Empty )
			{
				MessageBox.Show( 
                    "No configuration name provided", 
                    "Configuration Name Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
				return;
			}

            foreach (string config in configList)
            {
                if (config == ConfigToCreate)
                {
                    // TODO: Need general error message display
                    MessageBox.Show(
                        "A configuration with that name already exists", 
                        "Configuration Name Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }

			DialogResult = DialogResult.OK;

			Close();
		}

		#endregion
	}
}
