using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for AssemblyPathDialog.
	/// </summary>
	public class AssemblyPathDialog : System.Windows.Forms.Form
	{
		private string path;

		private System.Windows.Forms.TextBox assemblyPathTextBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AssemblyPathDialog( string path )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			this.path = path;
		}

		public string Path
		{
			get { return path; }
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.assemblyPathTextBox = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// assemblyPathTextBox
			// 
			this.assemblyPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.assemblyPathTextBox.Location = new System.Drawing.Point(8, 16);
			this.assemblyPathTextBox.Name = "assemblyPathTextBox";
			this.assemblyPathTextBox.Size = new System.Drawing.Size(472, 22);
			this.assemblyPathTextBox.TabIndex = 0;
			this.assemblyPathTextBox.Text = "";
			// 
			// okButton
			// 
			this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.okButton.Location = new System.Drawing.Point(160, 48);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 2;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(256, 48);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			// 
			// AssemblyPathDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(490, 78);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.cancelButton,
																		  this.okButton,
																		  this.assemblyPathTextBox});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "AssemblyPathDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Assembly Path";
			this.Load += new System.EventHandler(this.AssemblyPathDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void okButton_Click(object sender, System.EventArgs e)
		{
			path = assemblyPathTextBox.Text;

			if ( !File.Exists( path ) )
			{
				DialogResult answer = UserMessage.Ask( string.Format( 
					"The path {0} does not exist. Do you want to use it anyway?", path ) );
				if ( answer != DialogResult.Yes )
					return;
			}
			
			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void AssemblyPathDialog_Load(object sender, System.EventArgs e)
		{
			assemblyPathTextBox.Text = path;
		}
	}
}
