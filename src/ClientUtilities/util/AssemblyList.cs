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
		private IProject project;

		#region Properties

		public IProject Project
		{
			get { return project; }
			set { project = value; }
		}

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
//		private IList Directories
//		{
//			get
//			{
//				ArrayList dirList = new ArrayList();
//			
//				foreach( string assembly in InnerList )
//				{
//					string dir = Path.GetDirectoryName( assembly );
//					if ( !dirList.Contains( dir ) )
//						dirList.Add( dir );
//				}
//
//				return dirList;
//			}
//		}

		/// <summary>
		/// An IList of all the file names in the list.
		/// </summary>
//		public IList Names
//		{
//			get
//			{
//				ArrayList names = new ArrayList();
//				foreach( string assembly in InnerList )
//					names.Add( Path.GetFileName( assembly ) );
//				return names;
//			}
//		}

		/// <summary>
		/// The semicolon-separated path containing all the
		/// assemblies in the list.
		/// </summary>
//		public string AssemblyPath
//		{
//			get
//			{
//				IList dirs = Directories;
//				string assemblyPath = null;
//
//				foreach( string dir in dirs )
//					if ( assemblyPath == null )
//						assemblyPath = dir;
//					else
//						assemblyPath = assemblyPath + ";" + dir;
//
//				return assemblyPath;
//			}
//		}

		#endregion

		#region Methods

		public static string GetFullPath( string path )
		{
			return Path.GetFullPath( path ).ToLower();
		}

		public IList GetFiles()
		{
			ArrayList files = new ArrayList();
			foreach( string assembly in InnerList )
				files.Add( assembly );
			return files;
		}

		public void Add( string path )
		{
			string fullPath = GetFullPath( path );
			
			if ( !List.Contains( fullPath ) )
			{
				List.Add( fullPath );
			}
		}

		public void Remove( string path )
		{
			List.Remove( GetFullPath( path ) );
		}

		protected override void OnRemoveComplete(int index, object value)
		{
			if ( project != null )
				project.IsDirty = true;
		}

		protected override void OnInsertComplete(int index, object value)
		{
			if ( project != null )
				project.IsDirty = true;
		}
		
		protected override void OnSetComplete(int index, object oldValue, object newValue )
		{
			if ( project != null )
				project.IsDirty = true;
		}

		#endregion

	}
}
