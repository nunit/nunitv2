using System;
using System.Collections;
using System.IO;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for ProjectConfig.
	/// </summary>
	public class ProjectConfig
	{
		private string name;
		private AssemblyList assemblies = new AssemblyList();

		public ProjectConfig( string name )
		{
			this.name = name;
		}

		public string Name
		{
			get { return name; }
		}

		public AssemblyList Assemblies
		{
			get { return assemblies; }
		}
	}
}
