// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for RecentFilesService.
	/// </summary>
	public class RecentFilesService : RecentFiles, NUnit.Core.IService
	{
		private RecentFilesCollection fileEntries = new RecentFilesCollection();

		private ISettings settings;

		public static readonly int MinSize = 1;

		public static readonly int MaxSize = 24;

		public static readonly int DefaultSize = 5;

		#region Constructor
		public RecentFilesService()
			: this( Services.UserSettings ) { }

		public RecentFilesService( ISettings settings ) 
		{
			this.settings = settings;
			LoadEntries();
		}
		#endregion

		#region Properties
		public int Count
		{
			get { return fileEntries.Count; }
		}

		public int MaxFiles
		{
			get 
			{ 
				int size = settings.GetSetting( "RecentProjects.MaxFiles", DefaultSize );
				
				if ( size < MinSize ) size = MinSize;
				if ( size > MaxSize ) size = MaxSize;
				
				return size;
			}
			set 
			{ 
				int oldSize = MaxFiles;
				int newSize = value;
				
				if ( newSize < MinSize ) newSize = MinSize;
				if ( newSize > MaxSize ) newSize = MaxSize;

				settings.SaveSetting( "RecentProjects.MaxFiles", newSize );
				if ( newSize < oldSize ) SaveEntries();
			}
		}
		#endregion

		#region Public Methods
		public RecentFilesCollection Entries
		{
			get
			{
				LoadEntries();
				return fileEntries;
			}
		}
		
		public void Remove( string fileName )
		{
			LoadEntries();
			fileEntries.Remove( fileName );
			SaveEntries();
		}

		public void SetMostRecent( string fileName )
		{
			SetMostRecent( new RecentFileEntry( fileName ) );
		}

		public void SetMostRecent( RecentFileEntry entry )
		{
			LoadEntries();

			int index = fileEntries.IndexOf(entry.Path);

			if(index != -1)
				fileEntries.RemoveAt(index);

			fileEntries.Insert( 0, entry );
			if( fileEntries.Count > MaxFiles )
				fileEntries.RemoveAt( MaxFiles );

			SaveEntries();			
		}
		#endregion

		#region Helper Methods for saving and restoring the settings
		private void LoadEntries()
		{
			fileEntries.Clear();
			for ( int index = 1; index <= MaxFiles; index++ )
			{
				string fileSpec = settings.GetSetting( ValueName( index ) ) as string;
				if ( fileSpec != null )
					fileEntries.Add( NUnit.Util.RecentFileEntry.Parse( fileSpec ) );
			}
		}

		private void SaveEntries()
		{
			while( fileEntries.Count > MaxFiles )
				fileEntries.RemoveAt( fileEntries.Count - 1 );

			for( int index = 0; index < MaxSize; index++ ) 
			{
				string valueName = ValueName( index + 1 );
				if ( index < fileEntries.Count )
					settings.SaveSetting( valueName, fileEntries[index].ToString() );
				else
					settings.RemoveSetting( valueName );
			}
		}

		private string ValueName( int index )
		{
			return string.Format( "RecentProjects.File{0}", index );
		}
		#endregion

		#region IService Members

		public void UnloadService()
		{
			// TODO:  Add RecentFilesService.UnloadService implementation
		}

		public void InitializeService()
		{
			// TODO:  Add RecentFilesService.InitializeService implementation
		}

		#endregion
	}
}
