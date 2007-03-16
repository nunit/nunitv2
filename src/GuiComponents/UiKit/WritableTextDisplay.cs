// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for WritableTextDisplay.
	/// </summary>
	public class WritableTextDisplay : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.RichTextBox textBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private TextBoxWriter writer;

		/// <summary>
		/// StringBuilder to hold text until the control is actually  created
		/// </summary>
		//private StringBuilder sb;

		public WritableTextDisplay()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			writer = new TextBoxWriter( textBox );
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.Location = new System.Drawing.Point(0, 0);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(150, 150);
			this.textBox.TabIndex = 0;
			this.textBox.Text = "richTextBox1";
			// 
			// WritableTextDisplay
			// 
			this.Controls.Add(this.textBox);
			this.Name = "WritableTextDisplay";
			this.ResumeLayout(false);

		}
		#endregion

		#region TextBoxWriterMethods

		/// <summary>
		/// Clear both the TextBox and the buffer.
		/// </summary>
		public void Clear()
		{
			textBox.Clear();
//			sb = null;
		}

		#endregion

		#region TextWriter Methods

		public void Flush()
		{
			writer.Flush();
		}

		public void Write( char c )
		{
			writer.Write( c );
		}

		public void Write( string s )
		{
			writer.Write( s );
		}

		public void WriteLine( string s )
		{
			writer.WriteLine( s );
		}

		#endregion
	}
}
