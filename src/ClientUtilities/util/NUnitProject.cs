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
using System.Collections;
using System.Xml;
using System.IO;
using System.Threading;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// Class that represents an NUnit test project
	/// </summary>
	public class NUnitProject : IProjectContainer
	{
		#region Static and instance variables

		/// <summary>
		/// The extension used for test projects
		/// </summary>
		private static readonly string nunitExtension = ".nunit";

		/// <summary>
		/// The path to the project as saved.
		/// </summary>
		private string projectPath;

		/// <summary>
		/// The path from which the project was loaded
		/// </summary>
		private string loadPath;

		/// <summary>
		/// Dirty bit indicates we need to save
		/// </summary>
		private bool isDirty = false;

		/// <summary>
		/// Wrapper bit indicates an anonymous project
		/// that is wrapping a single assembly.
		/// </summary>
		private bool isWrapper = false;

		/// <summary>
		/// Our collection of configurations
		/// </summary>
		private ProjectConfigCollection configs;

		/// <summary>
		/// Name of the current active configuration
		/// </summary>
		private string activeConfigName;

		/// <summary>
		/// Default base path for the project
		/// </summary>
		private string basePath;

		#endregion

		#region Constructors

		public NUnitProject() 
		{	
			configs = new ProjectConfigCollection( this );		
		}

		public NUnitProject( string filePath )
		{
			configs = new ProjectConfigCollection( this );		
			Load( filePath );
		}

		#endregion

		#region Static Methods

		public static bool IsProjectFile( string path )
		{
			return Path.GetExtension( path ) == nunitExtension;
		}

		/// <summary>
		/// Return a test project by either loading it from
		/// the supplied path, creating one from a VS file
		/// or wrapping an assembly.
		/// </summary>
		public static NUnitProject MakeProject( string path )
		{
			NUnitProject project = null;

			if ( NUnitProject.IsProjectFile( path ) )
				project = new NUnitProject( path );
			else
			{
				string projectPath = NUnitProject.ProjectPathFromFile( path );
				
				if ( File.Exists( projectPath ) )
					project = new NUnitProject( projectPath );
				else if ( VSProject.IsProjectFile( path ) )
					project = NUnitProject.FromVSProject( path );
				else if ( VSProject.IsSolutionFile( path ) )
					project = NUnitProject.FromVSSolution( path );
				else
					project = NUnitProject.FromAssembly( path );
					// No automatic save for assemblies
			}

			if ( project.IsDirty && ! project.isWrapper )
				project.Save();

			return project;
		}

		/// <summary>
		/// Creates a project to wrap an assembly
		/// </summary>
		public static NUnitProject FromAssembly( string assemblyPath )
		{
			NUnitProject project = new NUnitProject();
			ProjectConfig config = new ProjectConfig( "Default" );
			config.Assemblies.Add( assemblyPath );
			project.Configs.Add( config );
			project.ActiveConfigName = "Default";

			project.loadPath = Path.GetFullPath( assemblyPath );
			project.projectPath = ProjectPathFromFile( project.loadPath );
			project.isWrapper = true;

			return project;
		}

		public static NUnitProject FromVSProject( string vsProjectPath )
		{
			NUnitProject project = new NUnitProject();
			VSProject vsProject = new VSProject( vsProjectPath );

			project.Add( vsProject );

			project.loadPath = Path.GetFullPath( vsProjectPath );
			project.projectPath = ProjectPathFromFile( project.loadPath );
			if ( project.Configs.Count > 0 )
				project.ActiveConfigName = project.Configs[0].Name;

			return project;
		}

		public static NUnitProject FromVSSolution( string solutionPath )
		{
			NUnitProject project = new NUnitProject();

			string solutionDirectory = Path.GetDirectoryName( solutionPath );
			StreamReader reader = new StreamReader( solutionPath );

			char[] delims = { '=', ',' };
			char[] trimchars = { ' ', '"' };

			string line = reader.ReadLine();
			while ( line != null )
			{
				if ( line.StartsWith( "Project" ) )
				{
					string[] parts = line.Split( delims );
					string vsProjectPath = Path.Combine( solutionDirectory, parts[2].Trim(trimchars) );
					
					if ( VSProject.IsProjectFile( vsProjectPath ) )
						project.Add( new VSProject( vsProjectPath ) );
				}

				line = reader.ReadLine();
			}

			project.loadPath = Path.GetFullPath( solutionPath );
			project.projectPath = ProjectPathFromFile( project.loadPath );

			if ( project.Configs.Count > 0 )
				project.activeConfigName = project.Configs[0].Name;

			return project;
		}

		/// <summary>
		/// Figure out the proper name to be used when saving a file.
		/// </summary>
		public static string ProjectPathFromFile( string path )
		{
			string fileName = Path.GetFileNameWithoutExtension( path ) + nunitExtension;
			return Path.Combine( Path.GetDirectoryName( path ), fileName );
		}

		#endregion

		#region Properties

		public string ProjectPath
		{
			get { return projectPath; }
		}

		public string LoadPath
		{
			get { return loadPath; }
		}

		public bool IsDirty
		{
			get { return isDirty; }
			set { isDirty = value; }
		}

		public bool IsWrapper
		{
			get { return isWrapper; }
		}

		public string TestFileName
		{
			get { return isWrapper ? LoadPath : ProjectPath; }
		}

		public string ActiveConfigName
		{
			get	{ return activeConfigName; }
			set	{ isDirty = true; activeConfigName = value; }
		}

		public ProjectConfig ActiveConfig
		{
			get { return configs[activeConfigName]; }
		}

		public ProjectConfigCollection Configs
		{
			get { return configs; }
		}

		public bool IsLoadable
		{
			get
			{
				return	ActiveConfig != null &&
						ActiveConfig.Assemblies.Count > 0;
			}
		}

		public string BasePath
		{
			get 
			{ 
				if ( basePath != null )
					return basePath;

				return Path.GetDirectoryName( projectPath );
			}
			set { isDirty = true; basePath = value; }
		}

		#endregion

		#region Instance Methods

		public void Add( VSProject vsProject )
		{
			foreach( ProjectConfig vsConfig in vsProject.Configs )
			{
				string name = vsConfig.Name;

				if ( !this.Configs.Contains( name ) )
					this.Configs.Add( name );

				ProjectConfig config = this.Configs[name];

				foreach ( string path in vsConfig.Assemblies )
					config.Assemblies.Add( path );
			}

			this.IsDirty = true;
		}

		public void Load( string projectPath )
		{
			string fullPath = Path.GetFullPath( projectPath );

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load( fullPath );

				string projectDir = Path.GetDirectoryName( fullPath );

				XmlNode settingsNode = doc.SelectSingleNode( "/NUnitProject/Settings" );
				string activeConfigName = GetAttribute( settingsNode, "activeconfig" );
				string applicationBase = GetAttribute( settingsNode, "appbase" );

				bool foundActiveConfig = false;
		
				foreach( XmlNode configNode in doc.SelectNodes( "/NUnitProject/Config" ) )
				{
					ProjectConfig config = new ProjectConfig( GetAttribute( configNode, "name" ) );

					foreach( XmlNode assemblyNode in configNode.SelectNodes(  "assembly" ) )
					{
						string path = GetAttribute( assemblyNode, "path" );
				
						config.Assemblies.Add( path );
					}

					Configs.Add( config );

					if ( config.Name == activeConfigName )
						foundActiveConfig = true;
				}

				if ( foundActiveConfig && activeConfigName != null && activeConfigName != "" )
					this.activeConfigName = activeConfigName;
				else if ( configs.Count > 0 )
					this.activeConfigName = configs[0].Name;
				else
					this.activeConfigName = null;

				this.basePath = applicationBase;
				
				this.loadPath = fullPath;
				this.projectPath = fullPath;			
				this.IsDirty = false;
			}
			catch( FileNotFoundException )
			{
				throw;
			}
			catch( Exception e )
			{
				throw new ArgumentException( 
					string.Format( "Invalid project file format: {0}", 
					Path.GetFileName( projectPath ) ), e );
			}
		}

		public void Save()
		{			
			StreamWriter writer = new StreamWriter( this.projectPath );
			writer.WriteLine( "<NUnitProject>" );

			if ( Configs.Count > 0 )
				writer.WriteLine( "  <Settings activeconfig=\"{0}\"/>", ActiveConfigName );
			
			foreach( ProjectConfig config in Configs )
			{
				writer.WriteLine( "  <Config name=\"{0}\">", config.Name );

				foreach( string assembly in config.Assemblies )
				{
					writer.WriteLine( "    <assembly path=\"{0}\"/>", assembly );
				}

				writer.WriteLine( "  </Config>" );
			}

			writer.WriteLine( "</NUnitProject>" );
			writer.Close();
			this.IsDirty = false;
		}

		public void Save( string projectPath )
		{
			this.projectPath = projectPath;
			Save();
		}

		/// <summary>
		/// Load tests for this project into a test domain
		/// </summary>
		public Test LoadTest( TestDomain testDomain )
		{
			if ( IsWrapper )
			{
				return testDomain.LoadAssembly( LoadPath );
			}
			else
			{
				AppDomainSetup setup = new AppDomainSetup();

				setup.ApplicationName = "Tests";
				setup.ShadowCopyFiles = "true";
				
				setup.ApplicationBase = ActiveConfig.BasePath;
				setup.ConfigurationFile =  Path.ChangeExtension( ProjectPath, ".config" );

				string binPath = ActiveConfig.Assemblies.PrivateBinPath;
				setup.ShadowCopyDirectories = binPath;
				setup.PrivateBinPath = binPath;

				return testDomain.LoadAssemblies( setup, ProjectPath, ActiveConfig.Assemblies.Files );
			}
		}

		private string GetAttribute( XmlNode node, string name )
		{
			if ( node == null ) return null;

			XmlNode attrNode = node.Attributes[name];
			if ( attrNode == null ) return null;

			return attrNode.Value;
		}

		#endregion
	}
}
