using System;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for OptionSettings.
	/// </summary>
	public class OptionSettings : SettingsGroup
	{
		private static readonly string NAME = "Options";

		private static readonly string LOAD_LAST_ASSEMBLY = "LoadLastAssembly";
		private static readonly string EXPAND_ON_LOAD = "ExpandOnLoad";
		private static readonly string HIDE_TEST_CASES = "HideTestCases";
		private static readonly string ENABLE_WATCHER = "EnableWatcher";
		private static readonly string CLEAR_RESULTS = "ClearResults";

		public OptionSettings( ) : base( NAME, UserSettings.GetStorageImpl( NAME ) ) { }

		public OptionSettings( SettingsStorage storage ) : base( NAME, storage ) { }

		public OptionSettings( SettingsGroup parent ) : base( NAME, parent ) { }

		public bool LoadLastAssembly
		{
			get { return LoadIntSetting( LOAD_LAST_ASSEMBLY, 1 ) != 0; }
			set { SaveIntSetting( LOAD_LAST_ASSEMBLY, value ? 1 : 0 ); }
		}

		public bool ExpandOnLoad
		{
			get { return LoadIntSetting( EXPAND_ON_LOAD, 1 ) != 0; }
			set { SaveIntSetting( EXPAND_ON_LOAD, value ? 1 : 0 ); }
		}

		public bool HideTestCases
		{
			get { return LoadIntSetting( HIDE_TEST_CASES, 0 ) != 0; }
			set { SaveIntSetting( HIDE_TEST_CASES, value ? 1 : 0 ); }
		}

		public bool EnableWatcher
		{
			get { return LoadIntSetting( ENABLE_WATCHER, 1 ) != 0; }
			set { SaveIntSetting( ENABLE_WATCHER, value ? 1 : 0 ); }
		}

		public bool ClearResults
		{
			get { return LoadIntSetting( CLEAR_RESULTS, 1 ) != 0; }
			set { SaveIntSetting( CLEAR_RESULTS, value ? 1 : 0 ); }
		}
	}
}
