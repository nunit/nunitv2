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
	public class NUnitProject
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
		private ProjectConfigCollection configs = new ProjectConfigCollection();

		#endregion

		#region Constructors

		public NUnitProject() {	}

		public NUnitProject( string filePath )
		{
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
				project.Save( path );

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
			project.SetActiveConfig( "Default" );

			project.loadPath = assemblyPath;
			project.projectPath = ProjectPathFromFile( assemblyPath );
			project.isWrapper = true;

			return project;
		}

		public static NUnitProject FromVSProject( string vsProjectPath )
		{
			NUnitProject project = new NUnitProject();
			VSProject vsProject = new VSProject( vsProjectPath );

			project.Add( vsProject );

			project.loadPath = vsProjectPath;
			project.projectPath = ProjectPathFromFile( vsProjectPath );

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

			project.loadPath = solutionPath;
			project.projectPath = ProjectPathFromFile( solutionPath );

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

		public bool IsDirty
		{
			get { return isDirty; }
			set { isDirty = value; }
		}

		public bool IsWrapper
		{
			get { return isWrapper; }
		}

		public ProjectConfig ActiveConfig
		{
			get
			{
				foreach( ProjectConfig config in Configs )
					if ( config.Active ) 
						return config;

				if ( Configs.Count > 0 )
				{
					Configs[0].Active = true;
					return Configs[0];
				}
					
				return null; 
			}
		}

		public void SetActiveConfig( string name )
		{
			if ( Configs[name] == null )
				throw new ArgumentException( "Specified configuration does not exist in the collection" );

			foreach( ProjectConfig config in Configs )
				config.Active = ( name == config.Name );		
		}

		public ProjectConfigCollection Configs
		{
			get { return configs; }
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
			XmlDocument doc = new XmlDocument();
			doc.Load( projectPath );

			string projectDir = Path.GetDirectoryName( projectPath );
			string activeConfigName = "Default";

			XmlNode settingsNode = doc.SelectSingleNode( "/NUnitProject/Settings" );

			if ( settingsNode != null )
				activeConfigName = settingsNode.Attributes["activeconfig"].Value;
		
			foreach( XmlNode configNode in doc.SelectNodes( "/NUnitProject/Config" ) )
			{
				ProjectConfig config = new ProjectConfig( configNode.Attributes["name"].Value );

				foreach( XmlNode assemblyNode in configNode.SelectNodes(  "assembly" ) )
				{
					string path = assemblyNode.Attributes["path"].Value;
					if ( !Path.IsPathRooted( path ) )
						path = Path.Combine( projectDir, path );
				
					config.Assemblies.Add( path );
				}

				if ( config.Name == activeConfigName )
					config.Active = true;

				Configs.Add( config );
			}

			this.loadPath = projectPath;
			this.projectPath = projectPath;
			this.IsDirty = false;
		}

		public void Save()
		{			
			StreamWriter writer = new StreamWriter( this.projectPath );
			writer.WriteLine( "<NUnitProject>" );

			if ( Configs.Count > 0 )
				writer.WriteLine( "  <Settings activeconfig=\"{0}\"/>", ActiveConfig.Name );
			
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

		#endregion
	}
}
