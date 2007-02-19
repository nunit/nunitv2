// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

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
		private System.Windows.Forms.Button browseButton;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AssemblyPathDialog));
			this.assemblyPathTextBox = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.browseButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// assemblyPathTextBox
			// 
			this.assemblyPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.assemblyPathTextBox.Location = new System.Drawing.Point(8, 16);
			this.assemblyPathTextBox.Name = "assemblyPathTextBox";
			this.assemblyPathTextBox.Size = new System.Drawing.Size(440, 22);
			this.assemblyPathTextBox.TabIndex = 0;
			this.assemblyPathTextBox.Text = "";
			// 
			// okButton
			// 
			this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.okButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.okButton.Location = new System.Drawing.Point(160, 48);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 24);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.cancelButton.Location = new System.Drawing.Point(256, 48);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 24);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			// 
			// browseButton
			// 
			this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.browseButton.Image = ((System.Drawing.Image)(resources.GetObject("browseButton.Image")));
			this.browseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.browseButton.Location = new System.Drawing.Point(456, 16);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(24, 24);
			this.browseButton.TabIndex = 4;
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// AssemblyPathDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(489, 77);
			this.Controls.Add(this.browseButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.assemblyPathTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.Name = "AssemblyPathDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Assembly Path";
			this.Load += new System.EventHandler(this.AssemblyPathDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void okButton_Click(object sender, System.EventArgs e)
		{
			string path = assemblyPathTextBox.Text;

			try
			{
				FileInfo info = new FileInfo( path );

				if ( !info.Exists )
				{
					DialogResult answer = UserMessage.Ask( string.Format( 
						"The path {0} does not exist. Do you want to use it anyway?", path ) );
					if ( answer != DialogResult.Yes )
						return;
				}

				DialogResult = DialogResult.OK;
				this.path = path;
				this.Close();
			}
			catch( System.Exception exception )
			{
				assemblyPathTextBox.SelectAll();
				UserMessage.DisplayFailure( exception, "Invalid Entry" );
			}	
		}

		private void AssemblyPathDialog_Load(object sender, System.EventArgs e)
		{
			assemblyPathTextBox.Text = path;
		}

		private void browseButton_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "Select Assembly";
			
			dlg.Filter =
				"Assemblies (*.dll,*.exe)|*.dll;*.exe|" +
				"All Files (*.*)|*.*";

			dlg.InitialDirectory = System.IO.Path.GetDirectoryName( path );
			dlg.FilterIndex = 1;
			dlg.FileName = "";

			if ( dlg.ShowDialog( this ) == DialogResult.OK ) 
			{
				assemblyPathTextBox.Text = dlg.FileName;
			}
		}
	}
}
