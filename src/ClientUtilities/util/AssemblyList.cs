#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.IO;
using System.Collections;

namespace NUnit.Util
{
	/// <summary>
	/// Represents a list of assemblies. It stores paths 
	/// that are added and marks it's ProjectContainer
	/// as dirty whenever it changes. 
	/// </summary>
	public class AssemblyList : CollectionBase
	{
		private IProjectContainer container;

		public AssemblyList( IProjectContainer container )
		{
			this.container = container;
		}

		#region Properties

		public IProjectContainer Container
		{
			get { return container; }
		}

		/// <summary>
		/// Our indexer
		/// </summary>
		public string this[int index]
		{
			get { return (string)List[index]; }
			set { List[index] = value; }
		}

		public string ApplicationBase
		{
			get { return container.BasePath; }
		}

		// Use this property to pass to the TestDomain
		// rather than using the AssemblyList directly.
		// AssemblyList is not serializable.
		public IList Files
		{
			get
			{
				ArrayList files = new ArrayList();
				foreach( string assembly in InnerList )
					files.Add( assembly );
				return files;
			}
		}

		public IList FullNames
		{
			get
			{
				ArrayList files = new ArrayList();
				foreach( string assembly in InnerList )
					files.Add( Path.Combine( ApplicationBase, assembly ) );
				return files;
			}
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
		public string PrivateBinPath
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

		public void Add( string path )
		{
			List.Add( path );
		}

		public void Remove( string path )
		{
			List.Remove( path );
		}

		protected override void OnRemoveComplete(int index, object value)
		{
			container.IsDirty = true;
		}

		protected override void OnInsertComplete(int index, object value)
		{
			container.IsDirty = true;
		}
		
		protected override void OnSetComplete(int index, object oldValue, object newValue )
		{
			container.IsDirty = true;
		}

		#endregion
	}
}
