using System;
using System.IO;
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for ProjectConfigTests.
	/// </summary>
	[TestFixture]
	public class ProjectConfigTests
	{
		private ProjectConfig config;

		[SetUp]
		public void SetUp()
		{
			config = new ProjectConfig( "Debug" );
		}

		[Test]
		public void EmptyConfig()
		{
			Assert.Equals( "Debug", config.Name );
			Assert.Equals( 0, config.Assemblies.Count );
		}

		[Test]
		public void CanAddAssemblies()
		{
			config.Assemblies.Add( @"C:\assembly1.dll" );
			config.Assemblies.Add( "assembly2.dll" );
			Assertion.AssertEquals( 2, config.Assemblies.Count );
			Assertion.AssertEquals( @"c:\assembly1.dll", config.Assemblies[0] );
			Assertion.AssertEquals( Path.GetFullPath( "assembly2.dll" ).ToLower(), config.Assemblies[1] );
		}

		[Test]
		public void AddMarksProjectDirty()
		{
			MockProject project = new MockProject();
			config.Project = project;
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			Assert.True( project.IsDirty );
		}

		[Test]
		public void DuplicatesAreIgnored()
		{
			config.Assemblies.Add( @"C:\junk\assembly1.dll" );
			config.Assemblies.Add( @"C:\junk\assembly1.dll" );			
			config.Assemblies.Add( @"C:\junk\Assembly1.dll" );

			Assertion.AssertEquals( 1, config.Assemblies.Count );
		}

		[Test]
		public void NameChangeMarksProjectDirty()
		{
			MockProject project = new MockProject();
			config.Project = project;
			config.Name = "Renamed";
			Assert.True( project.IsDirty );
		}
	}
}
