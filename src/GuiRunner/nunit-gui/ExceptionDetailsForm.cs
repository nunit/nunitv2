using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace NUnit.Gui
{
	/// <summary>
	/// Summary description for ExceptionDetailsForm.
	/// </summary>
	public class ExceptionDetailsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button okButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.RichTextBox stackTrace;
		private System.Windows.Forms.Label message;
		private Exception exception;

		public ExceptionDetailsForm( Exception exception )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.exception = exception;
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
			this.okButton = new System.Windows.Forms.Button();
			this.stackTrace = new System.Windows.Forms.RichTextBox();
			this.message = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(362, 444);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			// 
			// stackTrace
			// 
			this.stackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.stackTrace.Location = new System.Drawing.Point(8, 56);
			this.stackTrace.Name = "stackTrace";
			this.stackTrace.ReadOnly = true;
			this.stackTrace.Size = new System.Drawing.Size(784, 376);
			this.stackTrace.TabIndex = 3;
			this.stackTrace.Text = "";
			// 
			// message
			// 
			this.message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.message.Location = new System.Drawing.Point(16, 8);
			this.message.Name = "message";
			this.message.Size = new System.Drawing.Size(776, 40);
			this.message.TabIndex = 2;
			// 
			// ExceptionDetailsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(808, 478);
			this.Controls.Add(this.stackTrace);
			this.Controls.Add(this.message);
			this.Controls.Add(this.okButton);
			this.Name = "ExceptionDetailsForm";
			this.Text = "Exception Details";
			this.Load += new System.EventHandler(this.ExceptionDetailsForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ExceptionDetailsForm_Load(object sender, System.EventArgs e)
		{
			this.message.Text = string.Format( "{0}: {1}", exception.GetType().ToString(), exception.Message );
			this.stackTrace.Text = exception.StackTrace;
		}
	}
}