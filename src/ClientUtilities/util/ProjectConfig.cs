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
	public class ProjectConfig
	{
		#region Instance Variables

		/// <summary>
		/// The name of this config
		/// </summary>
		protected string name;

		/// <summary>
		/// IProject interface of containing project
		/// </summary>
		protected Project project = null;

		/// <summary>
		/// Mark this config as changed
		/// </summary>
		protected bool isDirty = false;

		/// <summary>
		/// List of the names of the assemblies
		/// </summary>
		private AssemblyList assemblies;

		/// <summary>
		/// Base path specific to this configuration
		/// </summary>
		private string basePath;

		/// <summary>
		/// Our configuration file, if specified
		/// </summary>
		private string configFile;

		/// <summary>
		/// Private bin path, if specified
		/// </summary>
		private string binPath;

		/// <summary>
		/// True if assembly paths should be added to bin path
		/// </summary>
		private bool autoBinPath = true;

		#endregion

		#region Construction

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

		public Project Project
		{
			get { return project; }
			set { project = value; }
		}

		public bool IsDirty
		{
			get { return isDirty; }
			set 
			{ 
				isDirty = value;

				if ( isDirty && project != null )
					project.IsDirty = true;
			}
		}

		public string Name
		{
			get { return name; }
			set 
			{
				if ( name != value )
				{
					name = value; 
					IsDirty = true;
				}
			}
		}
		
		/// <summary>
		/// The base directory for this config - used
		/// as the application base for loading tests.
		/// </summary>
		public string BasePath
		{
			get
			{ 
				return basePath;
			}
			set 
			{
				if ( basePath != value )
				{
					basePath = value;
					IsDirty = true;
				}
			}
		}

		/// <summary>
		/// Same as BasePath if the path is absolute,
		/// otherwise, combined with project BasePath.
		/// </summary>
		public string FullBasePath
		{
			get
			{
				if ( project == null || project.BasePath == null )
					return basePath;

				if ( basePath == null )
					return project.BasePath;

				return Path.Combine( project.BasePath, basePath );
			}
		}

		public string ConfigurationFile
		{
			get 
			{ 
				return configFile;
			}
			set
			{
				if ( configFile != value )
				{
					configFile = value;
					IsDirty = true;
				}
			}
		}

		public string ConfigurationFilePath
		{
			get
			{		
				string file = configFile == null && project != null 
					? project.ConfigurationFile
					: configFile;
					
				if ( FullBasePath == null || file == null )
					return file;

				return Path.Combine( FullBasePath, file );
			}
		}

		public string BinPath
		{
			get 
			{ 
				return binPath;
			}
			set 
			{
				if ( binPath != value )
				{
					binPath = value; 
					IsDirty = true;
				}
			}
		}

		public string FullBinPath
		{
			get
			{
				string assemblyPath = AutoBinPath
					? Assemblies.PrivateBinPath
					: null;

				if ( assemblyPath == null )
					return binPath;

				if ( binPath == null )
					return assemblyPath;

				return assemblyPath + ";" + binPath;
			}
		}

		public bool AutoBinPath
		{
			get { return autoBinPath; }
			set 
			{
				if ( autoBinPath != value )
				{
					autoBinPath = value; 
					IsDirty = true;
				}
			}
		}

		public AssemblyList Assemblies
		{
			get { return assemblies; }
		}

		#endregion
	}
}
