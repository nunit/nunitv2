using System;

namespace NUnit.Util
{
	/// <summary>
	/// RecentAssemblySettings holds settings for recent projects
	/// </summary>
	public class RecentProjectSettings : RecentFileSettings
	{
		private static readonly string NAME = "Recent-Projects";
		
		public RecentProjectSettings( ) : base ( NAME ) { }

		public RecentProjectSettings( SettingsStorage storage ) 
			: base( NAME, storage ) { }

		public RecentProjectSettings( SettingsGroup parent ) 
			: base( NAME, parent ) { }
	}
}
