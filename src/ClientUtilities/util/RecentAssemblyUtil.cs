namespace NUnit.Util
{
	using System;
	using System.Collections;
	using Microsoft.Win32;

	/// <summary>
	/// Summary description for RecentAssembly.
	/// </summary>
	public class RecentAssemblyUtil
	{
		private RegistryKey key;
		private static string KEY = "Software\\Nascent Software\\Nunit\\";
		private static string[] valueNames = { "RecentAssembly1", 
											   "RecentAssembly2", 
											   "RecentAssembly3", 
											   "RecentAssembly4", 
											   "RecentAssembly5" };
		private string subKey;

		private IList assemblyEntries;

		public RecentAssemblyUtil(string actualKey)
		{
			this.subKey = String.Format("{0}{1}", KEY, actualKey);
			key = Registry.CurrentUser.CreateSubKey(subKey);
			assemblyEntries = new ArrayList();
			for(int index = 0; index < valueNames.Length; index++)
			{
				string valueName = (string)key.GetValue(valueNames[index]);
				if(valueName != null)
					assemblyEntries.Add(valueName);
			}
		}

		public void Clear()
		{
			Registry.CurrentUser.DeleteSubKeyTree(subKey);
			assemblyEntries = new ArrayList();
		}

		public string RecentAssembly
		{
			get 
			{ 
				if(assemblyEntries.Count > 0)
					return (string)assemblyEntries[0];

				return null;
			}
			set
			{
				int index = assemblyEntries.IndexOf(value);

				if(index == 0) return;

				if(index != -1)
				{
					assemblyEntries.RemoveAt(index);
				}

				assemblyEntries.Insert(0, value);
				if(assemblyEntries.Count > valueNames.Length)
					assemblyEntries.RemoveAt(valueNames.Length);

				SaveToRegistry();			
			}
		}

		public IList GetAssemblies()
		{
			return assemblyEntries;
		}

		private void SaveToRegistry()
		{
			for(int index = 0; 
				index < assemblyEntries.Count; 
				index++)
			{
				key.SetValue(valueNames[index], assemblyEntries[index]);
			}
		}
	}
}
