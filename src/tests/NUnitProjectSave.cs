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

		[Test]
		public void SaveEmptyProject()
		{
			project.Save( writer );

			Assert.Equals( NUnitProjectXml.EmptyProject, sb.ToString() );
		}

		[Test]
		public void SaveEmptyConfigs()
		{
			project.Configs.Add( "Debug" );
			project.Configs.Add( "Release" );

			project.Save( writer );

			Assert.Equals( NUnitProjectXml.EmptyConfigs, sb.ToString() );			
		}

		[Test]
		public void SaveNormalProject()
		{
			ProjectConfig config1 = new ProjectConfig( "Debug" );
			config1.Assemblies.Add( @"h:\bin\debug\assembly1.dll" );
			config1.Assemblies.Add( @"h:\bin\debug\assembly2.dll" );

			ProjectConfig config2 = new ProjectConfig( "Release" );
			config2.Assemblies.Add( @"h:\bin\release\assembly1.dll" );
			config2.Assemblies.Add( @"h:\bin\release\assembly2.dll" );

			project.Configs.Add( config1 );
			project.Configs.Add( config2 );

			project.Save( writer );

			Assert.Equals( NUnitProjectXml.NormalProject, sb.ToString() );
		}
	}
}
