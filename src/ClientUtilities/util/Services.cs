using System;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for Services
	/// </summary>
	public class Services : NUnit.Core.Services
	{
		#region DomainManager
		private static DomainManager domainManager;
		public static DomainManager DomainManager
		{
			get
			{
				if ( domainManager == null )
					domainManager = (DomainManager)ServiceManager.Services.GetService( typeof( DomainManager ) );

				return domainManager;
			}
		}
		#endregion

		#region UserSettings
		private static SettingsService userSettings;
		public static SettingsService UserSettings
		{
			get 
			{ 
				if ( userSettings == null )
					userSettings = (SettingsService)ServiceManager.Services.GetService( typeof( SettingsService ) );

				return userSettings; 
			}
		}
		
		#endregion

		#region RecentFilesService
		private static RecentFiles recentFiles;
		public static RecentFiles RecentFiles
		{
			get
			{
				if ( recentFiles == null )
					recentFiles = (RecentFiles)ServiceManager.Services.GetService( typeof( RecentFiles ) );

				return recentFiles;
			}
		}
		#endregion
	}
}
