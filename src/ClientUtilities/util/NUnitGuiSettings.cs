using System;

namespace NUnit.Util
{
	/// <summary>
	/// NUnitGuiSettings hods settings for the GUI test runner
	/// </summary>
	public class NUnitGuiSettings : SettingsGroup
	{
		private static readonly string NAME = "NUnitGui";

		public NUnitGuiSettings( ) : base( NAME, UserSettings.GetStorageImpl( NAME ) ) { }

		public NUnitGuiSettings( SettingsStorage storage ) : base( NAME, storage ) { }

		public NUnitGuiSettings( SettingsGroup parent ) : base( NAME, parent ) { }

		public FormSettings  Form
		{
			get { return new FormSettings( this ); }
		}

//		public GuiOptionsSettings Options
//		{
//			get { return new GuiOptionsSettings( this ); }
//		}
	}
}
