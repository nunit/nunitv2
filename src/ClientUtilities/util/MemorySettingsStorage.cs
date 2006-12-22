using System;
using System.Collections;

namespace NUnit.Util
{
	public class MemorySettingsStorage : AbstractSettingsStorage
	{
		private Hashtable settings = new Hashtable();

		#region SettingsStorage Members

		public override ISettingsStorage MakeChildStorage(string name)
		{
			return new MemorySettingsStorage();
		}

		#endregion

		#region ISettings Members

		public override object GetSetting(string settingName)
		{
			return settings[settingName];
		}

		public override void RemoveSetting(string settingName)
		{
			settings.Remove( settingName );
		}

		public override void SaveSetting(string settingName, object settingValue)
		{
			settings[settingName] = settingValue;
		}

		#endregion

		#region IDisposable Members

		public override void Dispose()
		{
			// TODO:  Add MemorySettingsStorage.Dispose implementation
		}

		#endregion
	}
}
