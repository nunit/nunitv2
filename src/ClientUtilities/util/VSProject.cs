using System;
using System.IO;
using System.Xml;

namespace NUnit.Util
{
	/// <summary>
	/// This class allows loading information about
	/// configurations and assemblies in a Visual
	/// Studio project file and inspecting them.
	/// Only the most common project types are
	/// supported and an exception is thrown if
	/// an attempt is made to load an invalid
	/// file or one of an unknown type.
	/// </summary>
	public class VSProject : IProject
	{
		private static readonly string[] validExtensions = { ".csproj", ".vbproj", ".vcproj" };
		private static readonly string solutionExtension = ".sln";

		private string projectPath;
		private bool isDirty = false;
		private ProjectConfigCollection configs;

		public VSProject()
		{ 
			configs = new ProjectConfigCollection( this );		
		}

		public VSProject( string projectPath )
		{
			configs = new ProjectConfigCollection( this );		
			Load( projectPath );
		}

		public static bool IsProjectFile( string path )
		{
			string extension = Path.GetExtension( path );

			foreach( string validExtension in validExtensions )
				if ( extension == validExtension )
					return true;

			return false;
		}

		public static bool IsSolutionFile( string path )
		{
			return Path.GetExtension( path ) == solutionExtension;
		}

		public bool IsDirty
		{
			get { return isDirty; }
			set { isDirty = value; }
		}

		public string ProjectPath
		{
			get { return projectPath; }
		}

		public string Name
		{
			get 
			{ 
				if ( projectPath == null )
					return null;
				else
					return Path.GetFileNameWithoutExtension( projectPath );
			}
		}

		public ProjectConfigCollection Configs
		{
			get { return configs; }
		}

		public void Load( string projectPath )
		{
			if ( !IsProjectFile( projectPath ) )
				ThrowInvalidFileType( projectPath );

			string projectDirectory = Path.GetFullPath( Path.GetDirectoryName( projectPath ) );
			string currentDirectory = Environment.CurrentDirectory;

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load( projectPath );

				string extension = Path.GetExtension( projectPath );
				string assemblyName;

				switch ( extension )
				{
					case ".vcproj":
						foreach ( XmlNode configNode in doc.SelectNodes( "/VisualStudioProject/Configurations/Configuration" ) )
						{
							string name = configNode.Attributes["Name"].Value;
							string outputPath = configNode.Attributes["OutputDirectory"].Value;
							string outputDirectory = Path.Combine( projectDirectory, outputPath );
							XmlNode toolNode = configNode.SelectSingleNode( "Tool[@Name='VCLinkerTool']" );
							assemblyName = Path.GetFileName( toolNode.Attributes["OutputFile"].Value );
							string assemblyPath = Path.Combine( outputDirectory, assemblyName );

							ProjectConfig config = new ProjectConfig ( name );
							config.Assemblies.Add( assemblyPath );

							this.configs.Add( config );
						}
					
						break;

					case ".csproj":
					case ".vbproj":
						XmlNode settingsNode = doc.SelectSingleNode( "/VisualStudioProject/*/Build/Settings" );
			
						assemblyName = settingsNode.Attributes["AssemblyName"].Value;
						string outputType = settingsNode.Attributes["OutputType"].Value;

						if ( outputType == "Console" || outputType == "WinExe" )
							assemblyName = assemblyName + ".exe";
						else
							assemblyName = assemblyName + ".dll";

						XmlNodeList nodes = settingsNode.SelectNodes("Config");
						if ( nodes != null ) 
							foreach ( XmlNode configNode in nodes )
							{
								string name = configNode.Attributes["Name"].Value;
								string outputPath = configNode.Attributes["OutputPath"].Value;
								string outputDirectory = Path.Combine( projectDirectory, outputPath );
								string assemblyPath = Path.Combine( outputDirectory, assemblyName );
				
								ProjectConfig config = new ProjectConfig ( name );
								config.Assemblies.Add( assemblyPath );

								this.configs.Add( config );
							}

						break;

					default:
						break;
				}

				this.projectPath = projectPath;
			}
			catch( FileNotFoundException )
			{
				throw;
			}
			catch( Exception e )
			{
				ThrowInvalidFormat( projectPath, e );
			}
		}

		private void ThrowInvalidFileType( string projectPath )
		{
			throw new ArgumentException( 
				string.Format( "Invalid project file type: {0}", 
								Path.GetFileName( projectPath ) ) );
		}

		private void ThrowInvalidFormat( string projectPath, Exception e )
		{
			throw new ArgumentException( 
				string.Format( "Invalid project file format: {0}", 
								Path.GetFileName( projectPath ) ), e );
		}
	}
}
