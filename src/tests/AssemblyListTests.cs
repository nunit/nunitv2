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
using System.IO;
using System.Collections;
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for AssemblyListTests.
	/// </summary>
	[TestFixture]
	public class AssemblyListTests
	{
		ProjectConfig config;
		private AssemblyList assemblies;

		[SetUp]
		public void CreateAssemblyList()
		{
			config = new ProjectConfig();
			config.BasePath = @"c:\tests";
			assemblies = new AssemblyList( config );
		}

		[Test]
		public void EmptyList()
		{
			Assert.Equals( 0, assemblies.Count );
		}

		[Test]
		public void CanAddAssemblies()
		{
			assemblies.Add( @"bin\debug\assembly1.dll" );
			assemblies.Add( @"bin\debug\assembly2.dll" );

			Assert.Equals( 2, assemblies.Count );
			Assert.Equals( @"bin\debug\assembly1.dll", assemblies[0] );
			Assert.Equals( @"bin\debug\assembly2.dll", assemblies[1] );
		}

		[Test]
		public void GetListOfFiles()
		{
			assemblies.Add( @"bin\debug\assembly1.dll" );
			assemblies.Add( @"bin\debug\assembly2.dll" );

			IList files = assemblies.Files;
			Assert.Equals( @"bin\debug\assembly2.dll", files[1] );
		}

		[Test]
		public void GetFullNames()
		{
			assemblies.Add( @"bin\debug\assembly1.dll" );
			assemblies.Add( @"bin\debug\assembly2.dll" );

			IList fullNames = assemblies.FullNames;
			Assert.Equals( @"c:\tests\bin\debug\assembly2.dll", fullNames[1] );
		}

		[Test]
		public void GetListOfDirectories()
		{
			assemblies.Add( @"h:\app1\bin\debug\test1.dll" );
			assemblies.Add( @"h:\app2\bin\debug\test2.dll" );
			assemblies.Add( @"h:\app1\bin\debug\test3.dll" );

			Assert.Equals( 2, assemblies.Directories.Count ); 
		}

		[Test]
		public void GetListOfNames()
		{
			assemblies.Add( @"h:\app1\bin\debug\test1.dll" );
			assemblies.Add( @"h:\app2\bin\debug\test2.dll" );
			assemblies.Add( @"h:\app1\bin\debug\test3.dll" );

			Assert.Equals( "test3.dll", assemblies.Names[2] ); 
		}

		[Test]
		public void AddMarksConfigurationDirty()
		{
			assemblies.Add( @"bin\debug\assembly1.dll" );
			Assert.True( config.IsDirty );
		}

		[Test]
		public void CanRemoveAssemblies()
		{
			assemblies.Add( @"bin\debug\assembly1.dll" );
			assemblies.Add( @"bin\debug\assembly2.dll" );
			assemblies.Add( @"bin\debug\assembly3.dll" );
			assemblies.Remove( @"bin\debug\assembly2.dll" );

			Assert.Equals( 2, assemblies.Count );
			Assert.Equals( @"bin\debug\assembly1.dll", assemblies[0] );
			Assert.Equals( @"bin\debug\assembly3.dll", assemblies[1] );
		}

		[Test]
		public void RemoveAtMarksConfigurationDirty()
		{
			assemblies.Add( @"C:\bin\debug\assembly1.dll" );
			config.IsDirty = false;
			assemblies.RemoveAt(0);
			Assert.True( config.IsDirty );
		}

		[Test]
		public void RemoveMarksConfigurationDirty()
		{
			assemblies.Add( @"C:\bin\debug\assembly1.dll" );
			config.IsDirty = false;
			assemblies.Remove( @"C:\bin\debug\assembly1.dll" );
			Assert.True( config.IsDirty );
		}

		[Test]
		public void GetPrivateBinPath()
		{
			assemblies.Add( @"h:\app1\bin\debug\test1.dll" );
			assemblies.Add( @"h:\app2\bin\debug\test2.dll" );
			assemblies.Add( @"h:\app1\bin\debug\test3.dll" );

			Assert.Equals( @"h:\app1\bin\debug;h:\app2\bin\debug", assemblies.PrivateBinPath ); 
		}
	}
}
