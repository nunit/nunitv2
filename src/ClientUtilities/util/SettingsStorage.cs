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

	/// <summary>
	/// The ISettingsStorage interface is implemented by all
	/// types of backing storage for settings.
	/// </summary>
	public interface ISettingsStorage : IDisposable
	{
		/// <summary>
		/// Load a setting from the storage.
		/// </summary>
		/// <param name="settingName">Name of the setting to load</param>
		/// <returns>Value of the setting or null</returns>
		object GetSetting( string settingName );

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

		/// <summary>
		/// Create a child storage of the same type
		/// </summary>
		/// <param name="name">Name of the child storage</param>
		/// <returns>New child storage</returns>
		ISettingsStorage MakeChildStorage( string name );

		/// <summary>
		/// Load settings from external storage if required
		/// by the implementation.
		/// </summary>
		void LoadSettings();

		/// <summary>
		/// Save settings to external storage if required
		/// by the implementation.
		/// </summary>
		void SaveSettings();
	}
}
