using System;
using System.IO;
using System.Collections;

namespace NUnit.Util
{
	/// <summary>
	/// Represents a list of assemblies. It stores the 
	/// full path of whatever is added and fails silently
	/// if you try to add a duplicate entry.
	/// </summary>
	public class AssemblyList : CollectionBase
	{
		public AssemblyList() { }

		public static string GetFullPath( string path )
		{
			return Path.GetFullPath( path ).ToLower();
		}

		public void Add( string path )
		{
			string fullPath = GetFullPath( path );
			
			if ( !InnerList.Contains( fullPath ) )
				InnerList.Add( fullPath );	
		}

		public void Remove( string path )
		{
			InnerList.Remove( GetFullPath( path ) );
		}

		public string this[int index]
		{
			get { return (string)InnerList[index]; }
		}
	}
}
