using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Util;

namespace NUnit.Gui.SettingsPages
{
	public class AdvancedLoaderSettingsPage : NUnit.UiKit.SettingsPage
	{
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox disableShadowCopyCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.ComponentModel.IContainer components = null;

		public AdvancedLoaderSettingsPage( string key ) : base( key )
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
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.disableShadowCopyCheckBox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 16);
			this.label3.TabIndex = 12;
			this.label3.Text = "Shadow Copy";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Location = new System.Drawing.Point(104, 8);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(344, 8);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			// 
			// disableShadowCopyCheckBox
			// 
			this.helpProvider1.SetHelpString(this.disableShadowCopyCheckBox, "If checked, NUnit will disable copying of the assemblies to the shadow copy cache" +
				" by the CLR. With shadow copying disabled, it is not possible to make changes to" +
				" the assemblies while NUnit is running.");
			this.disableShadowCopyCheckBox.Location = new System.Drawing.Point(24, 32);
			this.disableShadowCopyCheckBox.Name = "disableShadowCopyCheckBox";
			this.helpProvider1.SetShowHelp(this.disableShadowCopyCheckBox, true);
			this.disableShadowCopyCheckBox.Size = new System.Drawing.Size(280, 22);
			this.disableShadowCopyCheckBox.TabIndex = 31;
			this.disableShadowCopyCheckBox.Text = "Disable Shadow Copy (default: Enabled)";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 296);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 24);
			this.label1.TabIndex = 32;
			this.label1.Text = "Warning:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(96, 296);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(336, 32);
			this.label2.TabIndex = 33;
			this.label2.Text = "Don\'t change any settings on this page unless you know what you are doing!";
			// 
			// AdvancedLoaderSettingsPage
			// 
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.disableShadowCopyCheckBox);
			this.Name = "AdvancedLoaderSettingsPage";
			this.ResumeLayout(false);

		}
		#endregion

		public override void LoadSettings()
		{
			this.settings = Services.UserSettings;

			disableShadowCopyCheckBox.Checked = !settings.GetSetting( "Options.TestLoader.ShadowCopyFiles", true );
		}

		public override void ApplySettings()
		{
			settings.SaveSetting( "Options.TestLoader.ShadowCopyFiles", !disableShadowCopyCheckBox.Checked );
		}

		public override bool HasChangesRequiringReload
		{
			get
			{
				bool oldSetting = !settings.GetSetting( "Options.TestLoader.ShadowCopyFiles", true );
				return disableShadowCopyCheckBox.Checked != oldSetting;
			}
		}
	}
}

