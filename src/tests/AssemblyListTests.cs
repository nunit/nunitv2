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
using System.IO;
using System.Collections;
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.Tests.Util
{
	/// <summary>
	/// This fixture tests both AssemblyList and AssemblyListItem
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
			config.BasePath = @"C:\tests";
			assemblies = new AssemblyList( config );
		}

		[Test]
		public void EmptyList()
		{
			Assert.AreEqual( 0, assemblies.Count );
		}

		[Test]
		public void CanAddAssemblies()
		{
			assemblies.Add( @"C:\tests\bin\debug\assembly1.dll" );
			assemblies.Add( @"C:\tests\bin\debug\assembly2.dll" );

			Assert.AreEqual( 2, assemblies.Count );
			Assert.AreEqual( @"C:\tests\bin\debug\assembly1.dll", assemblies[0].FullPath );
			Assert.AreEqual( @"C:\tests\bin\debug\assembly2.dll", assemblies[1].FullPath );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void MustAddAbsolutePath()
		{
			assemblies.Add( @"bin\debug\assembly1.dll" );
		}

		[Test]
		public void AddMarksConfigurationDirty()
		{
			assemblies.Add( @"C:\tests\bin\debug\assembly1.dll" );
			Assert.IsTrue( config.IsDirty );
		}

		[Test]
		public void CanRemoveAssemblies()
		{
			assemblies.Add( @"C:\tests\bin\debug\assembly1.dll" );
			assemblies.Add( @"C:\tests\bin\debug\assembly2.dll" );
			assemblies.Add( @"C:\tests\bin\debug\assembly3.dll" );
			assemblies.Remove( @"C:\tests\bin\debug\assembly2.dll" );

			Assert.AreEqual( 2, assemblies.Count );
			Assert.AreEqual( @"C:\tests\bin\debug\assembly1.dll", assemblies[0].FullPath );
			Assert.AreEqual( @"C:\tests\bin\debug\assembly3.dll", assemblies[1].FullPath );
		}

		[Test]
		public void RemoveAtMarksConfigurationDirty()
		{
			assemblies.Add( @"C:\tests\bin\debug\assembly1.dll" );
			config.IsDirty = false;
			assemblies.RemoveAt(0);
			Assert.IsTrue( config.IsDirty );
		}

		[Test]
		public void RemoveMarksConfigurationDirty()
		{
			assemblies.Add( @"C:\tests\bin\debug\assembly1.dll" );
			config.IsDirty = false;
			assemblies.Remove( @"C:\tests\bin\debug\assembly1.dll" );
			Assert.IsTrue( config.IsDirty );
		}

		[Test]
		public void SettingFullPathMarksConfigurationDirty()
		{
			assemblies.Add( @"C:\tests\bin\debug\assembly1.dll" );
			config.IsDirty = false;
			assemblies[0].FullPath = @"C:\tests\bin\debug\assembly2.dll";
			Assert.IsTrue( config.IsDirty );
		}
		
		[Test]
		public void SettingHasTestsMarksConfigurationDirty()
		{
			assemblies.Add( @"C:\tests\bin\debug\assembly1.dll" );
			config.IsDirty = false;
			assemblies[0].HasTests = false;
			Assert.IsTrue( config.IsDirty );
		}
	}
}
