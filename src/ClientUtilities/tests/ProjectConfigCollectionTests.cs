// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.IO;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
	/// <summary>
	/// Summary description for ProjectConfigCollectionTests.
	/// </summary>
	[TestFixture]
	public class ProjectConfigCollectionTests
	{
		private ProjectConfigCollection configs;
		private NUnitProject project = new NUnitProject( Path.DirectorySeparatorChar + "tests" + Path.DirectorySeparatorChar + "myproject.nunit" );

		[SetUp]
		public void SetUp()
		{
			configs = new ProjectConfigCollection( project );
		}

		[Test]
		public void EmptyCollection()
		{
			Assert.AreEqual( 0, configs.Count );
		}

		[Test]
		public void AddConfig()
		{
			configs.Add("Debug");
			configs["Debug"].Assemblies.Add( Path.DirectorySeparatorChar + "tests" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly1.dll" );
			configs["Debug"].Assemblies.Add( Path.DirectorySeparatorChar + "tests" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly2.dll" );

			Assert.AreEqual( 2, configs["Debug"].Assemblies.Count );
		}

		[Test]
		public void AddMakesProjectDirty()
		{
			configs.Add("Debug");
			Assert.IsTrue( project.IsDirty );
		}

		[Test]
		public void BuildConfigAndAdd()
		{
			ProjectConfig config = new ProjectConfig("Debug");
			config.Assemblies.Add( Path.DirectorySeparatorChar + "tests" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly1.dll" );
			config.Assemblies.Add( Path.DirectorySeparatorChar + "tests" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly2.dll" );

			configs.Add( config );

			Assert.AreEqual( 2, configs["Debug"].Assemblies.Count );
		}

		[Test]
		public void AddTwoConfigs()
		{
			configs.Add("Debug");
			configs.Add("Release");
			configs["Debug"].Assemblies.Add( Path.DirectorySeparatorChar + "tests" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly1.dll" );
			configs["Debug"].Assemblies.Add( Path.DirectorySeparatorChar + "tests" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly2.dll" );
			configs["Release"].Assemblies.Add( Path.DirectorySeparatorChar + "tests" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "release" + Path.DirectorySeparatorChar + "assembly3.dll" );

			Assert.AreEqual( 2, configs.Count );
			Assert.AreEqual( 2, configs["Debug"].Assemblies.Count );
			Assert.AreEqual( 1, configs["Release"].Assemblies.Count );
		}
	}
}
