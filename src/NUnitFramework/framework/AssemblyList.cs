using System;
using System.IO;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Represents a list of assemblies. It stores the 
	/// full path of whatever is added and fails silently
	/// if you try to add a duplicate entry.
	/// </summary>
	[Serializable]
	public class AssemblyList : CollectionBase
	{
		#region Construction and Conversion

		public AssemblyList() { }

		public AssemblyList( IList assemblies )
		{
			InnerList.AddRange( assemblies );
		}

		public static implicit operator AssemblyList( ArrayList assemblies )
		{
			return new AssemblyList( assemblies );
		}

		#endregion

		#region Properties

		/// <summary>
		/// Our indexer
		/// </summary>
		public string this[int index]
		{
			get { return (string)InnerList[index]; }
		}

		/// <summary>
		/// A list of directories containing all the assemblies
		/// </summary>
		public IList Directories
		{
			get
			{
				ArrayList dirList = new ArrayList();
			
				foreach( string assembly in InnerList )
				{
					string dir = Path.GetDirectoryName( assembly );
					if ( !dirList.Contains( dir ) )
						dirList.Add( dir );
				}

				return dirList;
			}
		}

		/// <summary>
		/// An IList of all the file names in the list.
		/// </summary>
		public IList Names
		{
			get
			{
				ArrayList names = new ArrayList();
				foreach( string assembly in InnerList )
					names.Add( Path.GetFileName( assembly ) );
				return names;
			}
		}

		/// <summary>
		/// The semicolon-separated path containing all the
		/// assemblies in the list.
		/// </summary>
		public string AssemblyPath
		{
			get
			{
				IList dirs = Directories;
				string assemblyPath = null;

				foreach( string dir in dirs )
					if ( assemblyPath == null )
						assemblyPath = dir;
					else
						assemblyPath = assemblyPath + ";" + dir;

				return assemblyPath;
			}
		}

		#endregion

		#region Methods

		public static string GetFullPath( string path )
		{
			return Path.GetFullPath( path ).ToLower();
		}

		public void Add( string path )
		{
			string fullPath = GetFullPath( path );
			
			if ( !InnerList.Contains( fullPath ) )
			{
				InnerList.Add( fullPath );
			}
		}

		public void Remove( string path )
		{
			InnerList.Remove( GetFullPath( path ) );
		}

		#endregion
	}
}
