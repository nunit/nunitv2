using System;
using System.Collections;
using System.Xml;
using System.IO;

namespace NUnit.Util
{
	/// <summary>
	/// Class that represents an NUnit test project
	/// </summary>
	public class NUnitProject
	{
		private string projectPath;
		private ProjectConfigCollection configs = new ProjectConfigCollection();

		public NUnitProject() {	}

		public string ProjectPath
		{
			get { return projectPath; }
		}

		public ProjectConfigCollection Configs
		{
			get { return configs; }
		}

		public void Load( string projectPath )
		{
			XmlDocument doc = new XmlDocument();
			doc.Load( projectPath );
			
			foreach( XmlNode configNode in doc.SelectNodes( "/NUnitProject/Config" ) )
			{
				ProjectConfig config = new ProjectConfig( configNode.Attributes["name"].Value );

				foreach( XmlNode assemblyNode in configNode.SelectNodes(  "assembly" ) )
				{
					config.Assemblies.Add( assemblyNode.Attributes["path"].Value );
				}

				Configs.Add( config );
			}

			this.projectPath = projectPath;
		}

		public void Save()
		{			
			StreamWriter writer = new StreamWriter( this.projectPath );
			writer.WriteLine( "<NUnitProject>" );
			
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
		}

		public void Save( string projectPath )
		{
			this.projectPath = projectPath;
			Save();
		}
	}
}
