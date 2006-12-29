using System;
using System.Windows.Forms;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for RichEditTabPage.
	/// </summary>
	public class RichEditTabPage : TabPage
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
