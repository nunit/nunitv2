#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
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
' Portions Copyright  2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
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
		private Project project = new Project( @"C:\tests\myproject.nunit" );

		[SetUp]
		public void SetUp()
		{
			configs = new ProjectConfigCollection( project );
		}

		[Test]
		public void EmptyCollection()
		{
			Assert.Equals( 0, configs.Count );
		}

		[Test]
		public void AddConfig()
		{
			configs.Add("Debug");
			configs["Debug"].Assemblies.Add( @"C:\tests\bin\debug\assembly1.dll" );
			configs["Debug"].Assemblies.Add( @"C:\tests\bin\debug\assembly2.dll" );

			Assert.Equals( 2, configs["Debug"].Assemblies.Count );
		}

		[Test]
		public void AddMakesProjectDirty()
		{
			configs.Add("Debug");
			Assert.True( project.IsDirty );
		}

		[Test]
		public void BuildConfigAndAdd()
		{
			ProjectConfig config = new ProjectConfig("Debug");
			config.Assemblies.Add( @"C:\tests\bin\debug\assembly1.dll" );
			config.Assemblies.Add( @"C:\tests\bin\debug\assembly2.dll" );

			configs.Add( config );

			Assert.Equals( 2, configs["Debug"].Assemblies.Count );
		}

		[Test]
		public void AddTwoConfigs()
		{
			configs.Add("Debug");
			configs.Add("Release");
			configs["Debug"].Assemblies.Add( @"C:\tests\bin\debug\assembly1.dll" );
			configs["Debug"].Assemblies.Add( @"C:\tests\bin\debug\assembly2.dll" );
			configs["Release"].Assemblies.Add( @"C:\tests\bin\release\assembly3.dll" );

			Assert.Equals( 2, configs.Count );
			Assert.Equals( 2, configs["Debug"].Assemblies.Count );
			Assert.Equals( 1, configs["Release"].Assemblies.Count );
		}
	}
}
