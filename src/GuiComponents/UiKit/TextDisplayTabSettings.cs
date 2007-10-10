using System;
using System.Collections;

namespace NUnit.UiKit
{
	public class TextDisplayTabSettings
	{
		private TabInfo[] tabInfo;
		private NUnit.Util.ISettings settings;

		public void LoadSettings()
		{
			LoadSettings( NUnit.Util.Services.UserSettings );
		}

		public void LoadSettings(NUnit.Util.ISettings settings)
		{
			this.settings = settings;

			ArrayList info = new ArrayList();
			for( int i = 0; ; i++ )
			{
				string prefix = string.Format( "Gui.TextOutput.Tab{0}.",i );
				string text = (string)settings.GetSetting(prefix + "Title");
				if ( text == null )
					break;

				TextDisplayContent content =
					(TextDisplayContent)settings.GetSetting(prefix + "Content", TextDisplayContent.Empty );
				bool visible = settings.GetSetting( prefix + "Visible", true );
				info.Add( new TabInfo( prefix, text, content, visible ));
			}

			if ( info.Count > 0 )		
				tabInfo = (TabInfo[])info.ToArray(typeof(TabInfo));
			else 
				LoadDefaults();
		}

		public void LoadDefaults()
		{
			tabInfo = new TabInfo[4];

			// Get any legacy settings
			bool mergeErrorOutput = settings.GetSetting( "Gui.ResultTabs.MergeErrorOutput", false );
			bool mergeTraceOutput = settings.GetSetting( "Gui.ResultTabs.MergeTraceOutput", false );

			TextDisplayContent content = TextDisplayContent.Out;
			if ( mergeErrorOutput )
				content |= TextDisplayContent.Error;
			if ( mergeTraceOutput )
				content |= TextDisplayContent.Trace;
			if ( settings.GetSetting( "Gui.ResultTabs.DisplayTestLabels", false ) )
				content |= TextDisplayContent.Labels;
			bool visible = settings.GetSetting( "Gui.ResultTabs.DisplayConsoleOutputTab", true );
			tabInfo[0] = new TabInfo( "Gui.TextOutput.Tab0.", "Console.Out", content, visible );

			content = mergeErrorOutput ? TextDisplayContent.Empty : TextDisplayContent.Error;
			visible = settings.GetSetting( "Gui.ResultTabs.DisplayConsoleErrorTab", true );
			tabInfo[1] = new TabInfo( "Gui.TextOutput.Tab1.", "Console.Error", content, visible );

			content = mergeTraceOutput ? TextDisplayContent.Empty : TextDisplayContent.Trace;
			visible = settings.GetSetting( "Gui.ResultTabs.DisplayTraceTab", true );
			tabInfo[2] = new TabInfo( "Gui.TextOutput.Tab2.", "Trace", content, visible );

			visible = settings.GetSetting( "Gui.ResultTabs.DisplayLoggingTab", true );
			tabInfo[3] = new TabInfo( "Gui.TextOutput.Tab3.", "Log", TextDisplayContent.Log, visible );
		}

		public void ApplySettings()
		{
			int index = 0;
			foreach( TabInfo tab in tabInfo )
			{
				string prefix = string.Format( "Gui.TextOutput.Tab{0}.", index++ );
				settings.SaveSetting( prefix + "Title", tab.Title );
				settings.SaveSetting( prefix + "Content", tab.Content );
				settings.SaveSetting( prefix + "Visible", tab.Visible );
			}

			// Remove any higher numbered tabs
			for(;;)
			{
				string prefix = string.Format( "Gui.TextOutput.Tab{0}.", index++ );
				if ( settings.GetSetting( prefix + "Title" ) == null )
					break;
				settings.RemoveSetting( prefix + "Title" );
				settings.RemoveSetting( prefix + "Content" );
			    settings.RemoveSetting( prefix + "Visible" );
			}

			// Remove legacy settings if present
			settings.RemoveSetting( "Gui.ResultTabs.MergeErrorOutput" );
			settings.RemoveSetting( "Gui.ResultTabs.MergeTraceOutput" );
			settings.RemoveSetting( "Gui.ResultTabs.DisplayTestLabels" );
		}

		public TabInfo[] Tabs
		{
			get { return tabInfo; }
		}
	
		public struct TabInfo
		{
			public string Prefix;
			public string Title;
			public TextDisplayContent Content;
			public bool Visible;

			public TabInfo( string prefix, string title, TextDisplayContent content, bool visible )
			{
				this.Prefix = prefix;
				this.Title = title;
				this.Content = content;
				this.Visible = visible;
			}
		}
	}
}
