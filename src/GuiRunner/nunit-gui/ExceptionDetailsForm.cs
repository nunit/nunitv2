using System;
using System.Text;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExceptionDetailsForm));
			this.okButton = new System.Windows.Forms.Button();
			this.stackTrace = new System.Windows.Forms.RichTextBox();
			this.message = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.AccessibleDescription = resources.GetString("okButton.AccessibleDescription");
			this.okButton.AccessibleName = resources.GetString("okButton.AccessibleName");
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("okButton.Anchor")));
			this.okButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("okButton.BackgroundImage")));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
			// 
			// stackTrace
			// 
			this.stackTrace.AccessibleDescription = resources.GetString("stackTrace.AccessibleDescription");
			this.stackTrace.AccessibleName = resources.GetString("stackTrace.AccessibleName");
			this.stackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("stackTrace.Anchor")));
			this.stackTrace.AutoSize = ((bool)(resources.GetObject("stackTrace.AutoSize")));
			this.stackTrace.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stackTrace.BackgroundImage")));
			this.stackTrace.BulletIndent = ((int)(resources.GetObject("stackTrace.BulletIndent")));
			this.stackTrace.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("stackTrace.Dock")));
			this.stackTrace.Enabled = ((bool)(resources.GetObject("stackTrace.Enabled")));
			this.stackTrace.Font = ((System.Drawing.Font)(resources.GetObject("stackTrace.Font")));
			this.stackTrace.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("stackTrace.ImeMode")));
			this.stackTrace.Location = ((System.Drawing.Point)(resources.GetObject("stackTrace.Location")));
			this.stackTrace.MaxLength = ((int)(resources.GetObject("stackTrace.MaxLength")));
			this.stackTrace.Multiline = ((bool)(resources.GetObject("stackTrace.Multiline")));
			this.stackTrace.Name = "stackTrace";
			this.stackTrace.ReadOnly = true;
			this.stackTrace.RightMargin = ((int)(resources.GetObject("stackTrace.RightMargin")));
			this.stackTrace.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("stackTrace.RightToLeft")));
			this.stackTrace.ScrollBars = ((System.Windows.Forms.RichTextBoxScrollBars)(resources.GetObject("stackTrace.ScrollBars")));
			this.stackTrace.Size = ((System.Drawing.Size)(resources.GetObject("stackTrace.Size")));
			this.stackTrace.TabIndex = ((int)(resources.GetObject("stackTrace.TabIndex")));
			this.stackTrace.Text = resources.GetString("stackTrace.Text");
			this.stackTrace.Visible = ((bool)(resources.GetObject("stackTrace.Visible")));
			this.stackTrace.WordWrap = ((bool)(resources.GetObject("stackTrace.WordWrap")));
			this.stackTrace.ZoomFactor = ((System.Single)(resources.GetObject("stackTrace.ZoomFactor")));
			// 
			// message
			// 
			this.message.AccessibleDescription = resources.GetString("message.AccessibleDescription");
			this.message.AccessibleName = resources.GetString("message.AccessibleName");
			this.message.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("message.Anchor")));
			this.message.AutoSize = ((bool)(resources.GetObject("message.AutoSize")));
			this.message.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("message.Dock")));
			this.message.Enabled = ((bool)(resources.GetObject("message.Enabled")));
			this.message.Font = ((System.Drawing.Font)(resources.GetObject("message.Font")));
			this.message.Image = ((System.Drawing.Image)(resources.GetObject("message.Image")));
			this.message.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("message.ImageAlign")));
			this.message.ImageIndex = ((int)(resources.GetObject("message.ImageIndex")));
			this.message.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("message.ImeMode")));
			this.message.Location = ((System.Drawing.Point)(resources.GetObject("message.Location")));
			this.message.Name = "message";
			this.message.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("message.RightToLeft")));
			this.message.Size = ((System.Drawing.Size)(resources.GetObject("message.Size")));
			this.message.TabIndex = ((int)(resources.GetObject("message.TabIndex")));
			this.message.Text = resources.GetString("message.Text");
			this.message.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("message.TextAlign")));
			this.message.Visible = ((bool)(resources.GetObject("message.Visible")));
			// 
			// ExceptionDetailsForm
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.stackTrace);
			this.Controls.Add(this.message);
			this.Controls.Add(this.okButton);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "ExceptionDetailsForm";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Resize += new System.EventHandler(this.ExceptionDetailsForm_Resize);
			this.Load += new System.EventHandler(this.ExceptionDetailsForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ExceptionDetailsForm_Load(object sender, System.EventArgs e)
		{
			this.message.Text = FormatMessage( exception );
			SetMessageLabelSize();

			this.stackTrace.Text = FormatStackTrace( exception );
		}

		private string FormatMessage( Exception exception )
		{
			StringBuilder sb = new StringBuilder();

			for( Exception ex = exception; ex != null; ex = ex.InnerException )
			{
				if ( ex != exception ) sb.Append( "\r\n----> " );
				sb.Append( ex.GetType().ToString() );
				sb.Append( ": " );
				sb.Append( ex.Message );
			}

			return sb.ToString();
		}

		private string FormatStackTrace( Exception exception )
		{
			StringBuilder sb = new StringBuilder();
			AppendStackTrace( sb, exception );

			return sb.ToString();
		}

		private void AppendStackTrace( StringBuilder sb, Exception ex )
		{
			if ( ex.InnerException != null )
				AppendStackTrace( sb, ex.InnerException );

			sb.Append( ex.GetType().ToString() );
			sb.Append( "...\r\n" );
			sb.Append( ex.StackTrace );
			sb.Append( "\r\n\r\n" );
		}

		private void ExceptionDetailsForm_Resize(object sender, System.EventArgs e)
		{
			SetMessageLabelSize();
		}

		private void SetMessageLabelSize()
		{
			Rectangle rect = message.ClientRectangle;
			Graphics g = Graphics.FromHwnd( Handle );
			SizeF sizeNeeded = g.MeasureString( message.Text, message.Font, rect.Width );
			int delta = sizeNeeded.ToSize().Height - rect.Height;
			
			message.Height += delta;
			stackTrace.Top += delta;
			stackTrace.Height -= delta;
		}
	}
}