using System;
using System.IO;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// TestPackage holds information about a set of
	/// tests to be loaded by a TestRunner.
	/// </summary>
	[Serializable]
	public class TestPackage : ReadOnlyCollectionBase
	{
		private string projectPath;
		private string basePath;
		private string configFile;
		private string binPath;

		public TestPackage( IList assemblies )
			: this ( (string)assemblies[0], assemblies ) { }

		public TestPackage( string projectPath, IList assemblies )
		{
			this.projectPath = projectPath;
			this.InnerList.AddRange( assemblies );
		}

		/// <summary>
		/// The path to the NUnit project used to create this
		/// package
		/// </summary>
		public string ProjectPath
		{
			get { return projectPath; }
		}

		public string BasePath
		{
			get { return basePath; }
			set { basePath = value; }
		}

		public string ConfigurationFile
		{
			get { return configFile; }
			set { configFile = value; }
		}

		public string PrivateBinPath
		{
			get { return binPath; }
			set { binPath = value; }
		}
	}
}
