using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NUnit.Gui.SettingsPages
{
	public class AssemblyReloadSettingsPage : NUnit.UiKit.SettingsPage
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox rerunOnChangeCheckBox;
		private System.Windows.Forms.CheckBox reloadOnRunCheckBox;
		private System.Windows.Forms.CheckBox reloadOnChangeCheckBox;
		private System.ComponentModel.IContainer components = null;

		public AssemblyReloadSettingsPage(string key) : base(key)
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
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rerunOnChangeCheckBox = new System.Windows.Forms.CheckBox();
			this.reloadOnRunCheckBox = new System.Windows.Forms.CheckBox();
			this.reloadOnChangeCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Assembly Reload";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(120, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(344, 8);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			// 
			// rerunOnChangeCheckBox
			// 
			this.rerunOnChangeCheckBox.Enabled = false;
			this.rerunOnChangeCheckBox.Location = new System.Drawing.Point(48, 88);
			this.rerunOnChangeCheckBox.Name = "rerunOnChangeCheckBox";
			this.rerunOnChangeCheckBox.Size = new System.Drawing.Size(200, 24);
			this.rerunOnChangeCheckBox.TabIndex = 13;
			this.rerunOnChangeCheckBox.Text = "Re-run last tests run";
			// 
			// reloadOnRunCheckBox
			// 
			this.reloadOnRunCheckBox.Location = new System.Drawing.Point(24, 24);
			this.reloadOnRunCheckBox.Name = "reloadOnRunCheckBox";
			this.reloadOnRunCheckBox.Size = new System.Drawing.Size(237, 23);
			this.reloadOnRunCheckBox.TabIndex = 11;
			this.reloadOnRunCheckBox.Text = "Reload before each test run";
			// 
			// reloadOnChangeCheckBox
			// 
			this.reloadOnChangeCheckBox.Location = new System.Drawing.Point(24, 56);
			this.reloadOnChangeCheckBox.Name = "reloadOnChangeCheckBox";
			this.reloadOnChangeCheckBox.Size = new System.Drawing.Size(245, 25);
			this.reloadOnChangeCheckBox.TabIndex = 12;
			this.reloadOnChangeCheckBox.Text = "Reload when test assembly changes";
			// 
			// AssemblyReloadSettings
			// 
			this.Controls.Add(this.rerunOnChangeCheckBox);
			this.Controls.Add(this.reloadOnRunCheckBox);
			this.Controls.Add(this.reloadOnChangeCheckBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Name = "AssemblyReloadSettings";
			this.ResumeLayout(false);

		}
		#endregion
	}
}

