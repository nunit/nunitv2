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
		private IProject project = null;
		private AssemblyList assemblies = new AssemblyList();

		public ProjectConfig( string name )
		{
			this.name = name;
		}

		public IProject Project
		{
			get { return project; }
			set 
			{ 
				project = value; 
				assemblies.Project = project;
			}
		}

		public string Name
		{
			get { return name; }
			set 
			{ 
				name = value; 
				if ( project != null )
					project.IsDirty = true;
			}
		}

		public AssemblyList Assemblies
		{
			get { return assemblies; }
		}
	}
}
