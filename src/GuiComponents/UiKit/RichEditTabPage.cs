using System;
using System.IO;
using System.Windows.Forms;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for RichEditTabPage.
	/// </summary>
	public class RichEditTabPage : TabPage
	{
		private RichTextBox textBox;
		private TextBoxWriter writer;

		public RichEditTabPage()
		{
			this.textBox = new RichTextBox();
			this.textBox.Multiline = true;
			this.textBox.Dock = DockStyle.Fill;

			this.Controls.Add( textBox );

			this.writer = new TextBoxWriter( textBox );
		}

		public TextWriter Writer
		{
			get { return writer; }
		}

		public void Clear()
		{
			this.textBox.Clear();
		}
	}
}
