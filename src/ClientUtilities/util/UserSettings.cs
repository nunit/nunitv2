using System;
using System.Drawing;

namespace NUnit.Util
{
	/// <summary>
	/// UserSettings represents the main group of per-user
	/// settings used by NUnit.
	/// </summary>
	public class UserSettings : SettingsGroup
	{
		private UserSettings()
			: base( "UserSettings", GetStorageImpl( )  ) { }

		public static SettingsStorage GetStorageImpl()
		{
			return new RegistrySettingsStorage( NUnitRegistry.CurrentUser );
		}

		public static SettingsStorage GetStorageImpl( string name )
		{
			return new RegistrySettingsStorage( name, NUnitRegistry.CurrentUser );
		}

//		public static NUnitGuiSettings NUnitGui
//		{
//			get { return new NUnitGuiSettings( GetStorageImpl( "NUnitGui" ) ); }
//		}

		public static FormSettings Form
		{
			get { return new FormSettings( GetStorageImpl( "Form" ) ); }
		}

		public static RecentAssemblySettings RecentAssemblies
		{
			get { return new RecentAssemblySettings( GetStorageImpl( "Recent-Assemblies" ) ); }
		}
	}
}
