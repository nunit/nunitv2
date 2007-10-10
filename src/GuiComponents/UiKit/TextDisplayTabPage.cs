// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
#define TABS_USE_TEXTBOX
using System;
using System.IO;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.UiKit
{
	[Flags]
	public enum TextDisplayContent
	{
		Empty = 0,
		Out = 1,
		Error = 2,
		Trace = 4,
		Log = 8,
		Labels = 64,
		LabelOnlyOnOutput = 128
	}

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

		private string prefix;

		public TextDisplayTabPage()
		{
#if TABS_USE_TEXTBOX
			this.display = new TextBoxDisplay();
#else
			this.display = new SimpleTextDisplay();
#endif
			this.display.Dock = DockStyle.Fill;

			this.Controls.Add( display );
		}

		public TextDisplayTabPage( TextDisplayTabSettings.TabInfo tabInfo ) : this()
		{
			this.prefix = tabInfo.Prefix;
			this.Text = tabInfo.Title;
			this.Display.Content = tabInfo.Content;
			Services.UserSettings.Changed += new SettingsEventHandler(UserSettings_Changed);
		}

		public TextDisplay Display
		{
			get { return this.display; }
		}

		private void UserSettings_Changed(object sender, SettingsEventArgs args)
		{
			string settingName = args.SettingName;
			if ( settingName.StartsWith( prefix ) )
			switch(settingName.Substring(prefix.Length))
			{
				case "Title":
					this.Text = Services.UserSettings.GetSetting( settingName, "Console.Out" );
					break;
				case "Content":
					this.Display.Content = 
						(TextDisplayContent)Services.UserSettings.GetSetting( settingName, TextDisplayContent.Out );
					break;
			}
		}
	}
}
