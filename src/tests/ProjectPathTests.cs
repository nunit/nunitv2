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
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.Tests.Util
{
	/// <summary>
	/// Summary description for ProjectPathTests.
	/// </summary>
	[TestFixture]
	public class ProjectPathTests
	{
		[Test]
		public void IsAssemblyFileType()
		{
			Assert.IsTrue( ProjectPath.IsAssemblyFileType( @"c:\bin\test.dll" ) );
			Assert.IsTrue( ProjectPath.IsAssemblyFileType( @"test.exe" ) );
			Assert.IsFalse( ProjectPath.IsAssemblyFileType( @"c:\bin\test.nunit" ) );
		}

		[Test]
		public void Canonicalize()
		{
			Assert.AreEqual( @"C:\folder1\file.tmp",
				ProjectPath.Canonicalize( @"C:\folder1\.\folder2\..\file.tmp" ) );
			Assert.AreEqual( @"folder1\file.tmp",
				ProjectPath.Canonicalize( @"folder1\.\folder2\..\file.tmp" ) );
		}

		[Test]
		public void RelativePath()
		{
			Assert.AreEqual( @"folder2\folder3", ProjectPath.RelativePath( 
				@"c:\folder1", @"c:\folder1\folder2\folder3" ) );
			Assert.AreEqual( @"..\folder2\folder3", ProjectPath.RelativePath(
				@"c:\folder1", @"c:\folder2\folder3" ) );
			Assert.AreEqual( @"bin\debug", ProjectPath.RelativePath(
				@"c:\folder1", @"bin\debug" ) );
			Assert.IsNull( ProjectPath.RelativePath( @"C:\folder", @"D:\folder" ),
				"Unrelated paths should return null" );
		}

		[Test]
		public void SamePath()
		{
			Assert.IsTrue( ProjectPath.SamePath( @"C:\folder1\file.tmp", @"c:\folder1\File.TMP" ) );
			Assert.IsTrue( ProjectPath.SamePath( @"C:\folder1\file.tmp", @"C:\folder1\.\folder2\..\file.tmp" ) );
			Assert.IsFalse( ProjectPath.SamePath( @"C:\folder1\file.tmp", @"C:\folder1\.\folder2\..\file.temp" ) );
		}

		[Test]
		public void SamePathOrUnder()
		{
			Assert.IsTrue( ProjectPath.SamePathOrUnder( @"C:\folder1\folder2\folder3", @"c:\folder1\.\folder2\junk\..\folder3" ) );
			Assert.IsTrue( ProjectPath.SamePathOrUnder( @"C:\folder1\folder2\", @"c:\folder1\.\folder2\junk\..\folder3" ) );
			Assert.IsTrue( ProjectPath.SamePathOrUnder( @"C:\folder1\folder2", @"c:\folder1\.\folder2\junk\..\folder3" ) );
			Assert.IsFalse( ProjectPath.SamePathOrUnder( @"C:\folder1\folder2", @"c:\folder1\.\folder22\junk\..\folder3" ) );
			Assert.IsFalse( ProjectPath.SamePathOrUnder( @"C:\folder1\folder2ile.tmp", @"D:\folder1\.\folder2\folder3\file.tmp" ) );
			Assert.IsFalse( ProjectPath.SamePathOrUnder( @"C:\", @"D:\" ) );
			Assert.IsTrue( ProjectPath.SamePathOrUnder( @"C:\", @"c:\" ) );
			Assert.IsTrue( ProjectPath.SamePathOrUnder( @"C:\", @"c:\bin\debug" ) );

		}
	}
}
