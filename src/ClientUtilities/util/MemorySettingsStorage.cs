using System;
using System.Collections;

namespace NUnit.Util
{
	public class MemorySettingsStorage : ISettingsStorage
	{
		private Hashtable settings = new Hashtable();

		#region SettingsStorage Members

		public int SettingsCount
		{
			get
			{
				return settings.Count;
			}
		}

		public bool ChildStorageExists(string name)
		{
			return false;
		}

		public ISettingsStorage MakeChildStorage(string name)
		{
			return new MemorySettingsStorage();
		}

		public void Clear()
		{
			// TODO:  Add MemorySettingsStorage.Clear implementation
			throw new NotImplementedException();
		}

		public object LoadSetting(string settingName)
		{
			return settings[settingName];
		}

		public int LoadIntSetting(string settingName)
		{
			return LoadIntSetting( settingName, 0 );
		}

		public string LoadStringSetting(string settingName)
		{
			object result = LoadSetting( settingName );
			
			if ( result == null || result is string )
				return (string) result;

			return result.ToString();
		}

		public object LoadSetting(string settingName, object defaultValue)
		{
			object result = LoadSetting(settingName );

			if ( result == null )
				result = defaultValue;

			return result;
		}

		public int LoadIntSetting(string settingName, int defaultValue)
		{
			object result = LoadSetting(settingName );

			if ( result == null )
				return defaultValue;

			if ( result is int )
				return (int)result;

			return Int32.Parse( result.ToString() );
		}

		public string LoadStringSetting(string settingName, string defaultValue)
		{
			object result = LoadSetting(settingName );

			if ( result == null )
				return defaultValue;

			if ( result is string )
				return (string)result;

			return result.ToString();
		}

		public void RemoveSetting(string settingName)
		{
			settings.Remove( settingName );
		}

		public void SaveSetting(string settingName, object settingValue)
		{
			settings[settingName] = settingValue;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			// TODO:  Add MemorySettingsStorage.Dispose implementation
		}

		#endregion
	}
}
