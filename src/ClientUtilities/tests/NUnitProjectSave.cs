#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Text;
using System.Xml;
using System.IO;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class NUnitProjectSave
	{
		static readonly string xmlfile = "test.nunit";

		private NUnitProject project;

		[SetUp]
		public void SetUp()
		{
			project = NUnitProject.EmptyProject();
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
			Assert.AreEqual( expected, contents );
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
			config1.BasePath = "bin" + Path.DirectorySeparatorChar + "debug";
			config1.Assemblies.Add( Path.GetFullPath( "bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly1.dll" ) );
			config1.Assemblies.Add( Path.GetFullPath( "bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly2.dll" ) );

			ProjectConfig config2 = new ProjectConfig( "Release" );
			config2.BasePath = "bin" + Path.DirectorySeparatorChar + "release";
			config2.Assemblies.Add( Path.GetFullPath( "bin" + Path.DirectorySeparatorChar + "release" + Path.DirectorySeparatorChar + "assembly1.dll" ) );
			config2.Assemblies.Add( Path.GetFullPath( "bin" + Path.DirectorySeparatorChar + "release" + Path.DirectorySeparatorChar + "assembly2.dll" ) );

			project.Configs.Add( config1 );
			project.Configs.Add( config2 );

			project.Save( xmlfile );

			CheckContents( NUnitProjectXml.NormalProject );
		}
	}
}
