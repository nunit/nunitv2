using System;
using System.Collections;

namespace NUnit.Util
{
	/// <summary>
	/// RecentAssemblySettings holds settings for recent assemblies
	/// </summary>
	public class RecentAssemblySettings : SettingsGroup
	{
		private static readonly string NAME = "Recent-Assemblies";
		
		private static string[] valueNames = {	"RecentAssembly1", 
												"RecentAssembly2", 
												"RecentAssembly3", 
												"RecentAssembly4", 
												"RecentAssembly5" };

		private IList assemblyEntries;

		public RecentAssemblySettings( ) : base ( NAME, UserSettings.GetStorageImpl( NAME ) )
		{
			LoadAssemblies();
		}

		public RecentAssemblySettings( SettingsStorage storage ) : base( NAME, storage ) 
		{
			LoadAssemblies();
		}

		public RecentAssemblySettings( SettingsGroup parent ) : base( NAME, parent ) 
		{ 
			LoadAssemblies();
		}

		private void LoadAssemblies()
		{
			assemblyEntries = new ArrayList();
			foreach( string valueName in valueNames )
			{
				string assemblyName = LoadStringSetting(valueName);
				if(assemblyName != null)
					assemblyEntries.Add(assemblyName);
			}
		}

		public override void Clear()
		{
			base.Clear();
			assemblyEntries = new ArrayList();
		}

		public IList GetAssemblies()
		{
			return assemblyEntries;
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

				SaveSettings();			
			}
		}

		public void Remove(string assemblyName)
		{
			assemblyEntries.Remove(assemblyName);
			SaveSettings();
		}

		private void SaveSettings()
		{
			for ( int index = 0; 
				  index < valueNames.Length;
				  index++)
			{
				if ( index < assemblyEntries.Count )
					SaveSetting( valueNames[index], assemblyEntries[index] );
				else
					RemoveSetting( valueNames[index] );
			}
		}
	}
}
