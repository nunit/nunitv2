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
using System.IO;

namespace NUnit.Util
{
	/// <summary>
	/// Base class for both NUnit and VS projects
	/// </summary>
	public class Project
	{
		#region Static and Instance Variables

		/// <summary>
		/// Path to the file storing this project
		/// </summary>
		protected string projectPath;

		/// <summary>
		///  Whether the project is dirty
		/// </summary>
		protected bool isDirty = false;
		
		/// <summary>
		/// Collection of configs for the project
		/// </summary>
		protected ProjectConfigCollection configs;

		#endregion

		#region Constructor

		public Project( string projectPath )
		{
			this.projectPath = Path.GetFullPath( projectPath );
			configs = new ProjectConfigCollection( this );		
		}

		#endregion

		#region Properties

		public bool IsDirty
		{
			get { return isDirty; }
			set { isDirty = value; }
		}

		/// <summary>
		/// The path to which a project will be saved.
		/// </summary>
		public string ProjectPath
		{
			get { return projectPath; }
			set 
			{
				projectPath = Path.GetFullPath( value );
				isDirty = true;
			}
		}

		/// <summary>
		/// The base path for the project is the
		/// directory part of the project path.
		/// </summary>
		public string BasePath
		{
			get { return Path.GetDirectoryName( projectPath ); }
		}

		/// <summary>
		/// The name of the project.
		/// </summary>
		public string Name
		{
			get { return Path.GetFileNameWithoutExtension( projectPath ); }
		}

		public string ConfigurationFile
		{
			get { return Path.GetFileNameWithoutExtension( projectPath ) + ".config"; }
		}

		public ProjectConfigCollection Configs
		{
			get { return configs; }
		}

		#endregion
	}
}