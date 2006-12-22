using System;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for AbstractSettingsStorage.
	/// </summary>
	public abstract class AbstractSettingsStorage : ISettingsStorage
	{
		#region ISettings Members

		public abstract object GetSetting(string settingName);

		/// <summary>
		/// Load a setting from this storage or return a default value
		/// </summary>
		/// <param name="settingName">Name of setting to load</param>
		/// <param name="defaultValue">Value to return if the seeting is not present</param>
		/// <returns>Value of the setting or the default</returns>
		public object GetSetting(string settingName, object defaultValue)
		{
			object result = GetSetting(settingName );

			if ( result == null )
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Load an integer setting from this storage or return a default value
		/// </summary>
		/// <param name="settingName">Name of setting to load</param>
		/// <param name="defaultValue">Value to return if the seeting is not present</param>
		/// <returns>Value of the setting or the default</returns>
		public int GetSetting(string settingName, int defaultValue)
		{
			object result = GetSetting(settingName );

			if ( result == null )
				return defaultValue;

			if ( result is int )
				return (int)result;

			return Int32.Parse( result.ToString() );
		}

		public bool GetSetting(string settingName, bool defaultValue)
		{
			return GetSetting( settingName, defaultValue ? 1 : 0 ) == 1;
		}

		/// <summary>
		/// Load a string setting from this storage or return a default value
		/// </summary>
		/// <param name="settingName">Name of setting to load</param>
		/// <param name="defaultValue">Value to return if the seeting is not present</param>
		/// <returns>Value of the setting or the default</returns>
		public string GetSetting(string settingName, string defaultValue)
		{
			object result = GetSetting(settingName );

			if ( result == null )
				return defaultValue;

			if ( result is string )
				return (string)result;

			return result.ToString();
		}

		/// <summary>
		/// Remove a setting from the storage
		/// </summary>
		/// <param name="settingName">Name of the setting to remove</param>
		public abstract void RemoveSetting(string settingName);

		/// <summary>
		/// Save a setting in this storage
		/// </summary>
		/// <param name="settingName">Name of the setting to save</param>
		/// <param name="settingValue">Value to be saved</param>
		public abstract void SaveSetting(string settingName, object settingValue);

		/// <summary>
		/// Save the value of one of the group's integer settings
		/// in a type-safe manner.
		/// </summary>
		/// <param name="settingName">Name of the setting to save</param>
		/// <param name="settingValue">Value to be saved</param>
		public void SaveSetting( string settingName, bool settingValue )
		{
			SaveSetting( settingName, settingValue ? 1 : 0 );
		}
		#endregion

		#region ISettingsStorage Members

		public abstract ISettingsStorage MakeChildStorage(string name);

		#endregion

		#region IDisposable Members

		public abstract void Dispose();

		#endregion
	}
}
