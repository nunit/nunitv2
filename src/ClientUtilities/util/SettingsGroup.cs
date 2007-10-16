// ****************************************************************
// Copyright 2002-2003, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

namespace NUnit.Util
{
	using System;

	/// <summary>
	/// SettingsGroup is the base class representing a group
	/// of user or system settings. All storge of settings
	/// is delegated to a SettingsStorage.
	/// </summary>
	public class SettingsGroup : ISettings, IDisposable
	{
		#region Instance Fields
		protected ISettingsStorage storage;
		#endregion

		#region Constructors

		/// <summary>
		/// Construct a settings group.
		/// </summary>
		/// <param name="storage">Storage for the group settings</param>
		public SettingsGroup( ISettingsStorage storage )
		{
			this.storage = storage;
		}

		/// <summary>
		/// Protected constructor for use by derived classes that
		/// set the storage themselves or don't use a storage.
		/// </summary>
		protected SettingsGroup()
		{
		}
		#endregion

		#region Properties

		public event SettingsEventHandler Changed;

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
			object result = GetSetting(settingName );

			if ( result == null )
				result = defaultValue;

			return result;
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
			object result = GetSetting(settingName );

			if ( result == null )
				return defaultValue;

			return ResultAsInt( result );
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
			object result = GetSetting(settingName );

			if ( result == null )
				return defaultValue;

			// Handle legacy formats
//			if ( result is int )
//				return (int)result == 1;
//
//			if ( result is string )
//			{
//				if ( (string)result == "1" ) return true;
//				if ( (string)result == "0" ) return false;
//			}

			return ResultAsBoolean( result );
		}

		/// <summary>
		/// Load the value of one of the group's string settings
		/// in a type-safe manner or return a default value
		/// </summary>
		/// <param name="settingName">Name of setting to load</param>
		/// <param name="defaultValue">Value to return if the setting is not present</param>
		/// <returns>Value of the setting or the default</returns>
		public string GetSetting( string settingName, string defaultValue )
		{
			object result = GetSetting(settingName );

			if ( result == null )
				return defaultValue;

			return ResultAsString( result );
		}

		/// <summary>
		/// Load the value of one of the group's enum settings
		/// in a type-safe manner or return a default value
		/// </summary>
		/// <param name="settingName">Name of setting to load</param>
		/// <param name="defaultValue">Value to return if the setting is not present</param>
		/// <returns>Value of the setting or the default</returns>
		public System.Enum GetSetting( string settingName, System.Enum defaultValue )
		{
			object result = GetSetting(settingName );

			if ( result == null )
				return defaultValue;

			return ResultAsEnum( result, defaultValue.GetType() );
		}

		/// <summary>
		/// Remove a setting from the group
		/// </summary>
		/// <param name="settingName">Name of the setting to remove</param>
		public void RemoveSetting( string settingName )
		{
			storage.RemoveSetting( settingName );

			if ( Changed != null )
				Changed( this, new SettingsEventArgs( settingName ) );
		}

		/// <summary>
		/// Remove a group of settings
		/// </summary>
		/// <param name="GroupName"></param>
		public void RemoveGroup( string groupName )
		{
			storage.RemoveGroup( groupName );
		}

		/// <summary>
		/// Save the value of one of the group's settings
		/// </summary>
		/// <param name="settingName">Name of the setting to save</param>
		/// <param name="settingValue">Value to be saved</param>
		public void SaveSetting( string settingName, object settingValue )
		{
			object oldValue = storage.GetSetting( settingName );

			// Avoid signaling "changes" when there is not really a change
			if ( oldValue != null )
			{
				if( settingValue is string && ResultAsString(oldValue) == (string)settingValue ||
					settingValue is int && ResultAsInt(oldValue) == (int)settingValue ||
					settingValue is bool && ResultAsBoolean(oldValue) == (bool)settingValue ||
					settingValue is Enum && ResultAsEnum(oldValue, settingValue.GetType()).Equals(settingValue) )
						return;
			}

			storage.SaveSetting( settingName, settingValue );

			if ( Changed != null )
				Changed( this, new SettingsEventArgs( settingName ) );
		}
		#endregion

		#region Conversion Helpers
		private string ResultAsString( object result )
		{
			return result is string
				? (string) result
				: result.ToString();
		}

		private int ResultAsInt( object result )
		{
			return result is int
				? (int) result
				: Int32.Parse( result.ToString() );
		}

		private bool ResultAsBoolean( object result )
		{
			return result is bool
				? (bool) result 
				: Boolean.Parse( result.ToString() );
		}

		private System.Enum ResultAsEnum( object result, Type enumType )
		{
			if ( result is System.Enum )
				return (System.Enum) result;
				
			return (System.Enum)System.Enum.Parse( enumType, result.ToString(), true );
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
