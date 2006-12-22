using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for RichEditTabPage.
	/// </summary>
	public class RichEditTabPage : System.Windows.Forms.TabPage
	{
		private RichTextBox textBox;

		public RichEditTabPage()
		{
			this.textBox = new RichTextBox();
			this.textBox.Dock = DockStyle.Fill;

			this.Controls.Add( textBox );
		}

		public void AppendText( string text )
		{
			this.textBox.AppendText( text );
		}

		public void Clear()
		{
			this.textBox.Clear();
		}
	}
}
