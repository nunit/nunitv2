using System;
using NUnit.Util;
using NUnit.Framework;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for ProjectConfigCollectionTests.
	/// </summary>
	[TestFixture]
	public class ProjectConfigCollectionTests
	{
		[Test]
		public void EmptyCollection()
		{
			ProjectConfigCollection configs = new ProjectConfigCollection();
			Assertion.AssertEquals( 0, configs.Count );
		}

		[Test]
		public void AddConfig()
		{
			ProjectConfigCollection configs = new ProjectConfigCollection();
			configs.Add("Debug");
			configs["Debug"].Assemblies.Add( @"bin\debug\assembly1.dll" );
			configs["Debug"].Assemblies.Add( @"bin\debug\assembly2.dll" );

			Assertion.AssertEquals( 2, configs["Debug"].Assemblies.Count );
		}

		[Test]
		public void BuildConfigAndAdd()
		{
			ProjectConfig config = new ProjectConfig("Debug");
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			config.Assemblies.Add( @"bin\debug\assembly2.dll" );

			ProjectConfigCollection configs = new ProjectConfigCollection();
			configs.Add( config );

			Assertion.AssertEquals( 2, configs["Debug"].Assemblies.Count );
		}

		[Test]
		public void AddTwoConfigs()
		{
			ProjectConfigCollection configs = new ProjectConfigCollection();
			configs.Add("Debug");
			configs.Add("Release");
			configs["Debug"].Assemblies.Add( @"bin\debug\assembly1.dll" );
			configs["Debug"].Assemblies.Add( @"bin\debug\assembly2.dll" );
			configs["Release"].Assemblies.Add( @"bin\debug\assembly3.dll" );

			Assertion.AssertEquals( 2, configs.Count );
			Assertion.AssertEquals( 2, configs["Debug"].Assemblies.Count );
			Assertion.AssertEquals( 1, configs["Release"].Assemblies.Count );
		}
	}
}
