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
		private string name;
		private bool active = false;
		private AssemblyList assemblies = new AssemblyList();

		public ProjectConfig( string name )
		{
			this.name = name;
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		public AssemblyList Assemblies
		{
			get { return assemblies; }
		}
	}
}
