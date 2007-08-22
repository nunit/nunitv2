// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

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
#if TABS_USE_TEXTBOX
		private TextBoxDisplay display;
#else
		private SimpleTextDisplay display;
#endif
		private TextDisplayWriter writer;

		public TextDisplayTabPage()
		{
#if TABS_USE_TEXTBOX
			this.display = new TextBoxDisplay();
#else
			this.display = new SimpleTextDisplay();
#endif
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
