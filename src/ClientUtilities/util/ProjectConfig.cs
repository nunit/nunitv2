#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
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
using System.Collections;
using System.IO;
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for ProjectConfig.
	/// </summary>
	public class ProjectConfig : IProjectContainer
	{
		#region Instance Variables

		/// <summary>
		/// The name of this config
		/// </summary>
		private string name;

		/// <summary>
		/// IProject interface of containing project
		/// </summary>
		private IProjectContainer container = null;

		/// <summary>
		/// List of the names of the assemblies
		/// </summary>
		private AssemblyList assemblies;

		/// <summary>
		/// Base path for this configuration
		/// </summary>
		private string basePath;

		/// <summary>
		/// Mark this config as changed
		/// </summary>
		private bool isDirty;

		#endregion

		#region Constructors

		public ProjectConfig()
		{
			this.assemblies = new AssemblyList( this );
		}

		public ProjectConfig( string name )
		{
			this.name = name;
			this.assemblies = new AssemblyList( this );
		}

		#endregion

		#region Properties

		public IProjectContainer Container
		{
			get { return container; }
			set { container = value; }
		}

		public string Name
		{
			get { return name; }
			set 
			{ 
				name = value; 
				if ( container != null )
					container.IsDirty = true;
			}
		}

		public bool IsDirty
		{
			get { return isDirty; }
			set 
			{ 
				isDirty = value;

				if ( isDirty && container != null )
					container.IsDirty = true;
			}
		}

		public string BasePath
		{
			get
			{ 
				if ( basePath == null )
					return container.BasePath;

				if ( container.BasePath == null )
					return basePath; 

				return Path.Combine( container.BasePath, basePath );
			}
			set { basePath = value; }
		}

		public AssemblyList Assemblies
		{
			get { return assemblies; }
		}

		public IList FullNames
		{
			get 
			{
				ArrayList fullNames = new ArrayList();
				string fullBasePath = BasePath;

				foreach( string assembly in assemblies )
				{
					if ( !Path.IsPathRooted( assembly ) && fullBasePath != null )
						fullNames.Add( Path.Combine( fullBasePath, assembly ) );
					else
						fullNames.Add( assembly );
				}

				return fullNames;
			}
		}

		#endregion
	}
}
