using System;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for OptionSettings.
	/// </summary>
	public class OptionSettings : SettingsGroup
	{
		private static readonly string NAME = "Options";

		public OptionSettings( ) : base( NAME, UserSettings.GetStorageImpl( NAME ) ) { }

		public OptionSettings( SettingsStorage storage ) : base( NAME, storage ) { }

		public OptionSettings( SettingsGroup parent ) : base( NAME, parent ) { }

		public bool LoadLastProject
		{
			get { return LoadIntSetting( "LoadLastProject", 1 ) != 0; }
			set { SaveIntSetting( "LoadLastProject", value ? 1 : 0 ); }
		}

		public int InitialTreeDisplay
		{
			get { return LoadIntSetting( "InitialTreeDisplay", 0 ); }
			set { SaveIntSetting( "InitialTreeDisplay", value ); }
		}

		public bool ReloadOnRun
		{
			get { return LoadIntSetting( "ReloadOnRun", 1 ) != 0; }
			set { SaveIntSetting( "ReloadOnRun", value ? 1 : 0 ); }
		}

		public bool ReloadOnChange
		{
			get { return LoadIntSetting( "ReloadOnChange", 1 ) != 0; }
			set { SaveIntSetting( "ReloadOnChange", value ? 1 : 0 ); }
		}

		public bool ClearResults
		{
			get { return LoadIntSetting( "ClearResults", 1 ) != 0; }
			set { SaveIntSetting( "ClearResults", value ? 1 : 0 ); }
		}

		public bool VisualStudioSupport
		{
			get { return LoadIntSetting( "VisualStudioSupport", 0 ) != 0; }
			set { SaveIntSetting( "VisualStudioSupport", value ? 1 : 0 ); }
		}
	}
}
