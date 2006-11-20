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

namespace NUnit.Util.Tests
{
	/// <summary>
	/// This fixture tests both AssemblyList and AssemblyListItem
	/// </summary>
	[TestFixture]
	public class AssemblyListTests
	{
		private AssemblyList assemblies;

        private string path1;
        private string path2;
        private string path3;

		private int events = 0;

		[SetUp]
		public void CreateAssemblyList()
		{
			assemblies = new AssemblyList();

            path1 = CleanPath("/tests/bin/debug/assembly1.dll");
            path2 = CleanPath("/tests/bin/debug/assembly2.dll");
            path3 = CleanPath("/tests/bin/debug/assembly3.dll");

			events = 0;

			assemblies.Changed += new EventHandler( assemblies_Changed );
        }

		private void assemblies_Changed( object sender, EventArgs e )
		{
			++events;
		}

		[Test]
		public void EmptyList()
		{
			Assert.AreEqual( 0, assemblies.Count );
		}

		[Test]
		public void CanAddAssemblies()
		{
			assemblies.Add( path1 );
			assemblies.Add( path2 );

			Assert.AreEqual( 2, assemblies.Count );
			Assert.AreEqual( path1, assemblies[0] );
			Assert.AreEqual( path2, assemblies[1] );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void MustAddAbsolutePath()
		{
			assemblies.Add( CleanPath( "bin/debug/assembly1.dll" ) );
		}

		[Test]
		public void AddFiresChangedEvent()
		{
			assemblies.Add( path1 );
			Assert.AreEqual( 1, events );
		}

		[Test]
		public void CanRemoveAssemblies()
		{
            assemblies.Add(path1);
            assemblies.Add(path2);
            assemblies.Add(path3);
			assemblies.Remove( path2 );

			Assert.AreEqual( 2, assemblies.Count );
			Assert.AreEqual( path1, assemblies[0] );
			Assert.AreEqual( path3, assemblies[1] );
		}

		[Test]
		public void RemoveAtFiresChangedEvent()
		{
			assemblies.Add( path1 );
			assemblies.RemoveAt(0);
			Assert.AreEqual( 2, events );
		}

		[Test]
		public void RemoveFiresChangedEvent()
		{
			assemblies.Add( path1 );
			assemblies.Remove( path1 );
			Assert.AreEqual( 2, events );
		}

		[Test]
		public void SettingFullPathFiresChangedEvent()
		{
			assemblies.Add( path1 );
			assemblies[0] = path2;
			Assert.AreEqual( 2, events );
		}
		
        private string CleanPath( string path )
        {
            return path.Replace( '/', Path.DirectorySeparatorChar );
        }
	}
}
