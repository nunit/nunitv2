using System;
using System.IO;
using System.Windows.Forms;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for TextDisplayTabPage.
	/// </summary>
	public class TextDisplayTabPage : TabPage
	{
		private SimpleTextDisplay display;
		private TextDisplayWriter writer;

		public TextDisplayTabPage()
		{
			this.display = new SimpleTextDisplay();
			this.display.Dock = DockStyle.Fill;

			this.Controls.Add( display );

			this.writer = new TextDisplayWriter( display );
		}

		public TextWriter Writer
		{
			get { return writer; }
		}

		public void Clear()
		{
			this.display.Clear();
		}
	}
}
