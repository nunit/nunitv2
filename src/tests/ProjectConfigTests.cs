#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

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
		private MockProjectContainer container;

		[SetUp]
		public void SetUp()
		{
			config = new ProjectConfig( "Debug" );
			container = new MockProjectContainer();
			container.BasePath = @"c:\test";
			config.Container = container;
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
			Assertion.AssertEquals( @"C:\assembly1.dll", config.Assemblies[0] );
			Assertion.AssertEquals( "assembly2.dll", config.Assemblies[1] );
		}

		[Test]
		public void AddMarksContainerDirty()
		{
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			Assert.True( container.IsDirty );
		}

		[Test]
		public void RenameMarksContainerDirty()
		{
			config.Name = "Renamed";
			Assert.True( container.IsDirty );
		}

		[Test]
		public void RemoveMarksContainerDirty()
		{
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			container.IsDirty = false;
			config.Assemblies.Remove( @"bin\debug\assembly1.dll" );
			Assert.True( container.IsDirty );			
		}

		[Test]
		public void AbsoluteBasePath()
		{
			config.BasePath = @"c:\junk";
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			Assert.Equals( @"c:\junk\bin\debug\assembly1.dll", config.FullNames[0] );
			Assert.Equals( @"c:\junk\bin\debug\assembly1.dll", config.Assemblies.FullNames[0] );
		}

		[Test]
		public void RelativeBasePath()
		{
			config.BasePath = @"junk";
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			Assert.Equals( @"c:\test\junk\bin\debug\assembly1.dll", config.FullNames[0] );
			Assert.Equals( @"c:\test\junk\bin\debug\assembly1.dll", config.Assemblies.FullNames[0] );
		}

		[Test]
		public void NoBasePathSet()
		{
			config.Assemblies.Add( @"bin\debug\assembly1.dll" );
			Assert.Equals( @"c:\test\bin\debug\assembly1.dll", config.FullNames[0] );
			Assert.Equals( @"c:\test\bin\debug\assembly1.dll", config.Assemblies.FullNames[0] );
		}
	}
}
