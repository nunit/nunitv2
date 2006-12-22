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

using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace NUnit.Util
{
	/// <summary>
	/// Implementation of SettingsStorage for NUnit user settings,
	/// based on storage of settings in the registry.
	/// </summary>
	public class RegistrySettingsStorage : AbstractSettingsStorage
	{
		#region Instance Variables

		/// <summary>
		/// If not null, the registry key for this storage
		/// </summary>
		private RegistryKey storageKey;

		#endregion

		#region Construction and Disposal

		/// <summary>
		/// Construct a storage using a registry key. This constructor is
		/// intended for use at the top level of the hierarchy.
		/// </summary>
		/// <param name="storageName">The name to give the storage</param>
		/// <param name="parentKey">The registry Key under which the storage will be created</param>
		public RegistrySettingsStorage( string storageName, RegistryKey parentKey ) 
		{
			this.storageKey = parentKey.CreateSubKey( storageName );
		}

		/// <summary>
		/// Construct a storage on top of a given key, using the key's name
		/// </summary>
		/// <param name="storageKey"></param>
		public RegistrySettingsStorage( RegistryKey storageKey )
		{
			this.storageKey = storageKey;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The registry key used to hold this storage
		/// </summary>
		public RegistryKey StorageKey
		{
			get { return storageKey; }
		}

		#endregion

		#region ISettings Members

		/// <summary>
		/// Load a setting from this storage
		/// </summary>
		/// <param name="settingName">Name of the setting to load</param>
		/// <returns>Value of the setting</returns>
		public override object GetSetting( string settingName )
		{
			return storageKey.GetValue( settingName );
		}

		/// <summary>
		/// Remove a setting from the storage
		/// </summary>
		/// <param name="settingName">Name of the setting to remove</param>
		public override void RemoveSetting( string settingName )
		{
			storageKey.DeleteValue( settingName, false );
		}

		/// <summary>
		/// Save a setting in this storage
		/// </summary>
		/// <param name="settingName">Name of the setting to save</param>
		/// <param name="settingValue">Value to be saved</param>
		public override void SaveSetting( string settingName, object settingValue )
		{
			storageKey.SetValue( settingName, settingValue );
		}

		#endregion

		#region ISettingsStorage Members
		/// <summary>
		/// Make a new child storage under this one
		/// </summary>
		/// <param name="storageName">Name of the child storage to make</param>
		/// <returns>New storage</returns>
		public override ISettingsStorage MakeChildStorage( string storageName )
		{
			return new RegistrySettingsStorage( storageKey.CreateSubKey( storageName ) );
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Dispose of this object by closing the storage key, if any
		/// </summary>
		public override void Dispose()
		{
			if ( storageKey != null )
				storageKey.Close();
		}

		#endregion
	}
}
