#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Util
{
	using System;
	using System.Collections;

	/// <summary>
	/// SettingsGroup is the base class representing a group
	/// of user or system settings. All storge of settings
	/// is delegated to a SettingsStorage.
	/// </summary>
	public class SettingsGroup : ISettings, IDisposable
	{
		#region Constructor

		/// <summary>
		/// Construct a settings group.
		/// </summary>
		/// <param name="storage">Storage for the group settings</param>
		public SettingsGroup( ISettingsStorage storage )
		{
			this.storage = storage;
		}

		#endregion

		#region Properties

		private ISettingsStorage storage;

		/// <summary>
		/// The storage used for the group settings
		/// </summary>
		public ISettingsStorage Storage
		{
			get { return storage; }
		}

		#endregion

		#region ISettings Members

		/// <summary>
		/// Load the value of one of the group's settings
		/// </summary>
		/// <param name="settingName">Name of setting to load</param>
		/// <returns>Value of the setting or null</returns>
		public object GetSetting( string settingName )
		{
			return storage.GetSetting( settingName );
		}

		/// <summary>
		/// Load the value of one of the group's settings or return a default value
		/// </summary>
		/// <param name="settingName">Name of setting to load</param>
		/// <param name="defaultValue">Value to return if the seeting is not present</param>
		/// <returns>Value of the setting or the default</returns>
		public object GetSetting( string settingName, object defaultValue )
		{
			return storage.GetSetting( settingName, defaultValue );
		}

		/// <summary>
		/// Load the value of one of the group's integer settings
		/// in a type-safe manner or return a default value
		/// </summary>
		/// <param name="settingName">Name of setting to load</param>
		/// <param name="defaultValue">Value to return if the seeting is not present</param>
		/// <returns>Value of the setting or the default</returns>
		public int GetSetting( string settingName, int defaultValue )
		{
			return storage.GetSetting( settingName, defaultValue );
		}

		/// <summary>
		/// Load the value of one of the group's boolean settings
		/// in a type-safe manner.
		/// </summary>
		/// <param name="settingName">Name of setting to load</param>
		/// <param name="defaultValue">Value of the setting or the default</param>
		/// <returns>Value of the setting</returns>
		public bool GetSetting( string settingName, bool defaultValue )
		{
			return storage.GetSetting( settingName, defaultValue );
		}

		/// <summary>
		/// Load the value of one of the group's string settings
		/// in a type-safe manner or return a default value
		/// </summary>
		/// <param name="settingName">Name of setting to load</param>
		/// <param name="defaultValue">Value to return if the seeting is not present</param>
		/// <returns>Value of the setting or the default</returns>
		public string GetSetting( string settingName, string defaultValue )
		{
			return storage.GetSetting( settingName, defaultValue );
		}

		/// <summary>
		/// Remove a setting from the group
		/// </summary>
		/// <param name="settingName">Name of the setting to remove</param>
		public void RemoveSetting( string settingName )
		{
			storage.RemoveSetting( settingName );
		}

		/// <summary>
		/// Save the value of one of the group's settings
		/// </summary>
		/// <param name="settingName">Name of the setting to save</param>
		/// <param name="settingValue">Value to be saved</param>
		public void SaveSetting( string settingName, object settingValue )
		{
			storage.SaveSetting( settingName, settingValue );
		}

		/// <summary>
		/// Save the value of one of the group's boolean settings
		/// in a type-safe manner.
		/// </summary>
		/// <param name="settingName">Name of the setting to save</param>
		/// <param name="settingValue">Value to be saved</param>
		public void SaveSetting( string settingName, bool settingValue )
		{
			storage.SaveSetting( settingName, settingValue );
		}

		#endregion

		#region IDisposable Members
		/// <summary>
		/// Dispose of this group by disposing of it's storage implementation
		/// </summary>
		public void Dispose()
		{
			if ( storage != null )
			{
				storage.Dispose();
				storage = null;
			}
		}
		#endregion
	}
}
