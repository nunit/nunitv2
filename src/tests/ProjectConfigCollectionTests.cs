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
		private ProjectConfigCollection configs;
		private MockProject mockProject = new MockProject();

		[SetUp]
		public void SetUp()
		{
			configs = new ProjectConfigCollection( mockProject );
		}

		[Test]
		public void EmptyCollection()
		{
			Assertion.AssertEquals( 0, configs.Count );
		}

		[Test]
		public void AddConfig()
		{
			configs.Add("Debug");
			configs["Debug"].Assemblies.Add( @"bin\debug\assembly1.dll" );
			configs["Debug"].Assemblies.Add( @"bin\debug\assembly2.dll" );

			Assertion.AssertEquals( 2, configs["Debug"].Assemblies.Count );
		}

		[Test]
		public void AddMakesProjectDirty()
		{
			configs.Add("Debug");
			Assert.True( mockProject.IsDirty );
		}

		[Test]
		public void BuildConfigAndAdd()
		{
			ProjectConfig config = new ProjectConfig("Debug");
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			config.Assemblies.Add( @"bin\debug\assembly2.dll" );

			configs.Add( config );

			Assertion.AssertEquals( 2, configs["Debug"].Assemblies.Count );
		}

		[Test]
		public void AddTwoConfigs()
		{
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
