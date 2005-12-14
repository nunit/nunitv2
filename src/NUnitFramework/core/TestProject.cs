#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
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
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
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

namespace NUnit.Core
{
	/// <summary>
	/// Simple representation of a test project used in the NUnit
	/// core. Unlike the NUnit.Util.NUnitProject class, it only holds 
	/// the single configuration that we are currently working with.
	/// 
	/// TODO: This is a temporary arrangement to keep things working
	/// while we determine the role of the two classes. It should be
	/// a lot simpler than it is, but NUnitProject has grown to have
	/// multiple roles and can't be used on the test side.
	/// </summary>
	[Serializable]
	public class TestProject
	{
		private string projectPath;
		private string basePath;
		private string configFile;
		private string[] assemblies;

		public TestProject( string projectPath, string[] assemblies )
			:this( projectPath, assemblies, null, null ) { }

		public TestProject( string projectPath, string[] assemblies, string basePath, string configFile )
		{
			this.projectPath = projectPath;
			this.assemblies = assemblies;
			this.basePath = basePath;
			this.configFile = configFile;
		}

		public string Name
		{
			get { return Path.GetFileNameWithoutExtension( projectPath ); }
		}

		public string ProjectPath
		{
			get { return projectPath; }
		}

		public string BasePath
		{
			get 
			{
				if ( basePath != null )
					return basePath;
				else
					return Path.GetDirectoryName( Path.GetFullPath( projectPath ) ); 
			}
		}

		public string ConfigurationFile
		{
			get 
			{ 
				if ( configFile != null )
					return configFile; 
				else if ( Path.GetExtension(projectPath).ToLower() == ".nunit" )
					return Name + ".config";
				else
					return projectPath + ".config";
			}
		}

		public string ConfigurationFilePath
		{
			get { return Path.Combine( BasePath, ConfigurationFile ); }
		}

		public string[] Assemblies
		{
			get { return assemblies; }
		}
	}
}
