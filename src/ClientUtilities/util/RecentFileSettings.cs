using System;
using System.Collections;

namespace NUnit.Util
{
	/// <summary>
	/// Base class for settings that hold lists of recent files
	/// </summary>
	public abstract class RecentFileSettings : SettingsGroup
	{
		private IList fileEntries;

		private int maxValues = 5;

		public RecentFileSettings( string name ) : base ( name, UserSettings.GetStorageImpl( name ) )
		{
			LoadFiles();
		}

		public RecentFileSettings( string name, SettingsStorage storage ) : base( name, storage ) 
		{
			LoadFiles();
		}

		public RecentFileSettings( string name, SettingsGroup parent ) : base( name, parent ) 
		{ 
			LoadFiles();
		}

		public int MaxValues
		{
			get { return maxValues; }
		}

		protected void LoadFiles()
		{
			fileEntries = new ArrayList();
			for ( int index = 1; index <= MaxValues; index++ )
			{
				string fileName = LoadStringSetting( ValueName( index ) );
				if ( fileName != null )
					fileEntries.Add( fileName );
			}
		}

		public override void Clear()
		{
			base.Clear();
			fileEntries = new ArrayList();
		}

		public IList GetFiles()
		{
			return fileEntries;
		}
		
		public string RecentFile
		{
			get 
			{ 
				if( fileEntries.Count > 0 )
					return (string)fileEntries[0];

				return null;
			}
			set
			{
				int index = fileEntries.IndexOf(value);

				if(index == 0) return;

				if(index != -1)
				{
					fileEntries.RemoveAt(index);
				}

				fileEntries.Insert( 0, value );
				if( fileEntries.Count > MaxValues )
					fileEntries.RemoveAt( MaxValues );

				SaveSettings();			
			}
		}

		public void Remove( string fileName )
		{
			fileEntries.Remove( fileName );
			SaveSettings();
		}

		private void SaveSettings()
		{
			for ( int index = 0; 
				index < MaxValues;
				index++)
			{
				string valueName = ValueName( index + 1 );
				if ( index < fileEntries.Count )
					SaveSetting( valueName, fileEntries[index] );
				else
					RemoveSetting( valueName );
			}
		}

		private string ValueName( int index )
		{
			return string.Format( "File{0}", index );
		}
	}
}
