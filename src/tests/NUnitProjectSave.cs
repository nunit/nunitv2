using System;
using System.Text;
using System.Xml;
using System.IO;
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.Tests
{
	[TestFixture]
	public class NUnitProjectSave
	{
		static readonly string xmlfile = "test.nunit";

		private NUnitProject project;
		private StringBuilder sb;
		private XmlTextWriter writer;

		[SetUp]
		public void SetUp()
		{
			project = NUnitProject.EmptyProject();
			sb = new StringBuilder( 1024 );
			writer = new XmlTextWriter( new StringWriter( sb ) );
		}

		[TearDown]
		public void TearDown()
		{
			if ( File.Exists( xmlfile ) )
				File.Delete( xmlfile );
		}

		private void CheckContents( string expected )
		{
			StreamReader reader = new StreamReader( xmlfile );
			string contents = reader.ReadToEnd();
			reader.Close();
			Assert.Equals( expected, contents );
		}

		[Test]
		public void SaveEmptyProject()
		{
			project.Save( xmlfile );

			CheckContents( NUnitProjectXml.EmptyProject );
		}

		[Test]
		public void SaveEmptyConfigs()
		{
			project.Configs.Add( "Debug" );
			project.Configs.Add( "Release" );

			project.Save( xmlfile );

			CheckContents( NUnitProjectXml.EmptyConfigs );			
		}

		[Test]
		public void SaveNormalProject()
		{
			ProjectConfig config1 = new ProjectConfig( "Debug" );
			config1.BasePath = @"bin\debug";
			config1.Assemblies.Add( Path.GetFullPath( @"bin\debug\assembly1.dll" ) );
			config1.Assemblies.Add( Path.GetFullPath( @"bin\debug\assembly2.dll" ) );

			ProjectConfig config2 = new ProjectConfig( "Release" );
			config2.BasePath = @"bin\release";
			config2.Assemblies.Add( Path.GetFullPath( @"bin\release\assembly1.dll" ) );
			config2.Assemblies.Add( Path.GetFullPath( @"bin\release\assembly2.dll" ) );

			project.Configs.Add( config1 );
			project.Configs.Add( config2 );

			project.Save( xmlfile );

			CheckContents( NUnitProjectXml.NormalProject );
		}
	}
}
