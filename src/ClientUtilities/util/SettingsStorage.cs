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
	using System.IO;

	/// <summary>
	/// Interface to be implemented by any type of settings storage
	/// </summary>
	public interface ISettingsStorage : IDisposable
	{
		#region Properties

		/// <summary>
		/// The number of settings in this group
		/// </summary>
		int SettingsCount { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Find out if a substorage exists
		/// </summary>
		/// <param name="name">Name of the child storage</param>
		/// <returns>True if the storage exists</returns>
		bool ChildStorageExists( string name );

		/// <summary>
		/// Create a child storage of the same type
		/// </summary>
		/// <param name="name">Name of the child storage</param>
		/// <returns>New child storage</returns>
		ISettingsStorage MakeChildStorage( string name );

		/// <summary>
		/// Clear all settings from the storage - empty storage remains
		/// </summary>
		void Clear();

		/// <summary>
		/// Load a setting from the storage.
		/// </summary>
		/// <param name="settingName">Name of the setting to load</param>
		/// <returns>Value of the setting or null</returns>
		object LoadSetting( string settingName );

		/// <summary>
		/// Load an integer setting from the storage
		/// </summary>
		/// <param name="settingName">Name of the setting to load</param>
		/// <returns>Value of the setting or null</returns>
		int LoadIntSetting( string settingName );

		/// <summary>
		/// Load a string setting from the storage
		/// </summary>
		/// <param name="settingName">Name of the setting to load</param>
		/// <returns>Value of the setting or null</returns>
		string LoadStringSetting( string settingName );

		/// <summary>
		/// Load a setting from the storage or return a default value
		/// </summary>
		/// <param name="settingName">Name of the setting to load</param>
		/// <param name="settingName">Value to return if the setting is missing</param>
		/// <returns>Value of the setting or the default value</returns>
		object LoadSetting( string settingName, object defaultValue );

		/// <summary>
		/// Load an integer setting from the storage or return a default value
		/// </summary>
		/// <param name="settingName">Name of the setting to load</param>
		/// <param name="settingName">Value to return if the setting is missing</param>
		/// <returns>Value of the setting or the default value</returns>
		int LoadIntSetting( string settingName, int defaultValue );

		/// <summary>
		/// Load a string setting from the storage or return a default value
		/// </summary>
		/// <param name="settingName">Name of the setting to load</param>
		/// <param name="settingName">Value to return if the setting is missing</param>
		/// <returns>Value of the setting or the default value</returns>
		string LoadStringSetting( string settingName, string defaultValue );

		/// <summary>
		/// Remove a setting from the storage
		/// </summary>
		/// <param name="settingName">Name of the setting to remove</param>
		void RemoveSetting( string settingName );

		/// <summary>
		/// Save a setting in the storage
		/// </summary>
		/// <param name="settingName">Name of the setting to save</param>
		/// <param name="settingValue">Value to be saved</param>
		void SaveSetting( string settingName, object settingValue );

		#endregion
	}
}
