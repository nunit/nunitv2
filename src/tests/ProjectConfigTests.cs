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
		[Test]
		public void EmptyConfig()
		{
			ProjectConfig config = new ProjectConfig( "Debug" );
			Assertion.AssertEquals( "Debug", config.Name );
			Assertion.AssertEquals( 0, config.Assemblies.Count );
		}

		[Test]
		public void AddAssemblies()
		{
			ProjectConfig config = new ProjectConfig( "Debug" );
			config.Assemblies.Add( @"C:\assembly1.dll" );
			config.Assemblies.Add( "assembly2.dll" );
			Assertion.AssertEquals( 2, config.Assemblies.Count );
			Assertion.AssertEquals( @"c:\assembly1.dll", config.Assemblies[0] );
			Assertion.AssertEquals( Path.GetFullPath( "assembly2.dll" ).ToLower(), config.Assemblies[1] );
		}

		[Test]
		public void DuplicateAssemblies()
		{
			ProjectConfig config = new ProjectConfig( "Debug" );
			config.Assemblies.Add( @"C:\junk\assembly1.dll" );
			config.Assemblies.Add( @"C:\junk\assembly1.dll" );			
			config.Assemblies.Add( @"C:\junk\Assembly1.dll" );

			Assertion.AssertEquals( 1, config.Assemblies.Count );
		}

	}
}
