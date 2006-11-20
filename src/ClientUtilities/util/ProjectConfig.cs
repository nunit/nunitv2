#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Text;
using System.Collections;
using System.IO;
using NUnit.Core;

namespace NUnit.Util
{
	public enum BinPathType
	{
		Auto,
		Manual,
		None
	}

	public class ProjectConfig
	{
		#region Instance Variables

		/// <summary>
		/// The name of this config
		/// </summary>
		private string name;

		/// <summary>
		/// IProject interface of containing project
		/// </summary>
		protected NUnitProject project = null;

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
		private BinPathType binPathType = BinPathType.Auto;

		#endregion

		#region Constructor
		public ProjectConfig( string name )
		{
			this.name = name;
			this.assemblies = new AssemblyList();
			assemblies.Changed += new EventHandler( assemblies_Changed );
		}
		#endregion

		#region Properties and Events

		public event EventHandler Changed;

		public NUnitProject Project
		{
//			get { return project; }
			set { project = value; }
		}

		public string Name
		{
			get { return name; }
			set 
			{
				if ( name != value )
				{
					name = value; 
					FireChangedEvent();
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
				if ( project == null || project.BasePath == null )
					return basePath;

				if ( basePath == null )
					return project.BasePath;

				return Path.Combine( project.BasePath, basePath );
			}
			set 
			{
				if ( BasePath != value )
				{
					basePath = value;
					FireChangedEvent();
				}
			}
		}

		/// <summary>
		/// The base path relative to the project base
		/// </summary>
		public string RelativeBasePath
		{
			get
			{
				if ( project == null || basePath == null || !Path.IsPathRooted( basePath ) )
					return basePath;

				return PathUtils.RelativePath( project.BasePath, basePath );
			}
		}

		public string ConfigurationFile
		{
			get 
			{ 
				return configFile == null && project != null
					? project.ConfigurationFile 
					: configFile;
			}
			set
			{
				if ( ConfigurationFile != value )
				{
					configFile = value;
					FireChangedEvent();
				}
			}
		}

		public string ConfigurationFilePath
		{
			get
			{		
				return BasePath != null && ConfigurationFile != null
					? Path.Combine( BasePath, ConfigurationFile )
					: ConfigurationFile;
			}
		}

		/// <summary>
		/// The Path.PathSeparator-separated path containing all the
		/// assemblies in the list.
		/// </summary>
		public string PrivateBinPath
		{
			get
			{
				switch( binPathType )
				{
					case BinPathType.Manual:
						return binPath;

					case BinPathType.Auto:
						StringBuilder sb = new StringBuilder(200);
						ArrayList dirList = new ArrayList();

						foreach( string assembly in Assemblies )
						{
							string dir = PathUtils.RelativePath( BasePath, Path.GetDirectoryName( assembly ) );
							if ( dir != null && dir != "." && !dirList.Contains( dir ) )
							{
								dirList.Add( dir );
								if ( sb.Length > 0 )
									sb.Append( Path.PathSeparator );
								sb.Append( dir );
							}
						}

						return sb.Length == 0 ? null : sb.ToString();

					default:
						return null;
				}
			}

			set
			{
				if ( binPath != value )
				{
					binPath = value;
					binPathType = binPath == null ? BinPathType.Auto : BinPathType.Manual;
					FireChangedEvent();
				}
			}
		}

		/// <summary>
		/// How our PrivateBinPath is generated
		/// </summary>
		public BinPathType BinPathType
		{
			get { return binPathType; }
			set 
			{
				if ( binPathType != value )
				{
					binPathType = value;
					FireChangedEvent();
				}
			}
		}

		/// <summary>
		/// Return our AssemblyList
		/// </summary>
		public AssemblyList Assemblies
		{
			get { return assemblies; }
		}
		#endregion

		private void assemblies_Changed( object sender, EventArgs e )
		{
			FireChangedEvent();
		}

		private void FireChangedEvent()
		{
			if ( Changed != null )
				Changed( this, EventArgs.Empty );
		}
	}
}
