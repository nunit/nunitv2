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
	[TestFixture]
	public class PathUtilTests : PathUtils
	{
		[Test]
		public void CheckDefaults()
		{
			Assert.AreEqual( Path.DirectorySeparatorChar, PathUtils.DirectorySeparatorChar );
			Assert.AreEqual( Path.AltDirectorySeparatorChar, PathUtils.AltDirectorySeparatorChar );
		}
	}

	// Local Assert extension
	internal class Assert : NUnit.Framework.Assert
	{
		public static void SamePath( string path1, string path2 )
		{
			string msg = "\r\n\texpected: Same path as <{0}>\r\n\t but was: <{1}>";
			Assert.IsTrue( PathUtils.SamePath( path1, path2 ), msg, path1, path2 );
		}

		public static void NotSamePath( string path1, string path2 )
		{
			string msg = "\r\n\texpected: Not same path as <{0}>\r\n\t but was: <{1}>";
			Assert.IsFalse( PathUtils.SamePath( path1, path2 ), msg, path1, path2 );
		}

		public static void SamePathOrUnder( string path1, string path2 )
		{
			string msg = "\r\n\texpected: Same path or under <{0}>\r\n\t but was: <{1}>";
			Assert.IsTrue( PathUtils.SamePathOrUnder( path1, path2 ), msg, path1, path2 );
		}

		public static void NotSamePathOrUnder( string path1, string path2 )
		{
			string msg = "\r\n\texpected: Not same path or under <{0}>\r\n\t but was: <{1}>";
			Assert.IsFalse( PathUtils.SamePathOrUnder( path1, path2 ), msg, path1, path2 );
		}
	}

	[TestFixture]
	[Platform(Exclude="Linux")]
	public class PathUtilTests_Windows : PathUtils
	{
		[TestFixtureSetUp]
		public void SetUpUnixSeparators()
		{
			PathUtils.DirectorySeparatorChar = '\\';
			PathUtils.AltDirectorySeparatorChar = '/';
		}

		[TestFixtureTearDown]
		public void RestoreDefaultSeparators()
		{
			PathUtils.DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
			PathUtils.AltDirectorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar;
		}

		[Test]
		public void IsAssemblyFileType()
		{
			Assert.IsTrue( PathUtils.IsAssemblyFileType( @"c:\bin\test.dll" ) );
			Assert.IsTrue( PathUtils.IsAssemblyFileType( @"test.exe" ) );
			Assert.IsFalse( PathUtils.IsAssemblyFileType( @"c:\bin\test.nunit" ) );
		}

		[Test]
		public void Canonicalize()
		{
			Assert.AreEqual( @"C:\folder1\file.tmp",
				PathUtils.Canonicalize( @"C:\folder1\.\folder2\..\file.tmp" ) );
			Assert.AreEqual( @"folder1\file.tmp",
				PathUtils.Canonicalize( @"folder1\.\folder2\..\file.tmp" ) );
			Assert.AreEqual( @"folder1\file.tmp", 
				PathUtils.Canonicalize( @"folder1\folder2\.\..\file.tmp" ) );
			Assert.AreEqual( @"file.tmp", 
				PathUtils.Canonicalize( @"folder1\folder2\..\.\..\file.tmp" ) );
			Assert.AreEqual( @"file.tmp", 
				PathUtils.Canonicalize( @"folder1\folder2\..\..\..\file.tmp" ) );
		}

		[Test]
		public void RelativePath()
		{
			Assert.AreEqual( @"folder2\folder3", PathUtils.RelativePath( 
				@"c:\folder1", @"c:\folder1\folder2\folder3" ) );
			Assert.AreEqual( @"..\folder2\folder3", PathUtils.RelativePath(
				@"c:\folder1", @"c:\folder2\folder3" ) );
			Assert.AreEqual( @"bin\debug", PathUtils.RelativePath(
				@"c:\folder1", @"bin\debug" ) );
			Assert.IsNull( PathUtils.RelativePath( @"C:\folder", @"D:\folder" ),
				"Unrelated paths should return null" );
		}

		[Test]
		public void SamePath()
		{
			Assert.SamePath( @"C:\folder1\file.tmp", @"c:\folder1\File.TMP" );
			Assert.SamePath( @"C:\folder1\file.tmp", @"C:\folder1\.\folder2\..\file.tmp" );
			Assert.NotSamePath( @"C:\folder1\file.tmp", @"C:\folder1\.\folder2\..\file.temp" );

			Assert.SamePath( "D:/folder1/folder2", @"d:\Folder1\Folder2" );
		}

		[Test]
		public void SamePathOrUnder()
		{
			Assert.SamePathOrUnder( @"C:\folder1\folder2\folder3", @"c:\folder1\.\folder2\junk\..\folder3" );
			Assert.SamePathOrUnder( @"C:\folder1\folder2\", @"c:\folder1\.\folder2\junk\..\folder3" );
			Assert.SamePathOrUnder( @"C:\folder1\folder2", @"c:\folder1\.\folder2\junk\..\folder3" );
			Assert.SamePathOrUnder( @"C:\folder1\folder2", @"c:\folder1\.\Folder2\junk\..\folder3" );
			Assert.NotSamePathOrUnder( @"C:\folder1\folder2", @"c:\folder1\.\folder22\junk\..\folder3" );
			Assert.NotSamePathOrUnder( @"C:\folder1\folder2ile.tmp", @"D:\folder1\.\folder2\folder3\file.tmp" );
			Assert.NotSamePathOrUnder( @"C:\", @"D:\" );
			Assert.SamePathOrUnder( @"C:\", @"c:\" );
			Assert.SamePathOrUnder( @"C:\", @"c:\bin\debug" );

		}
	}

	[TestFixture]
	public class PathUtilTests_Unix : PathUtils
	{
		[TestFixtureSetUp]
		public void SetUpUnixSeparators()
		{
			PathUtils.DirectorySeparatorChar = '/';
			PathUtils.AltDirectorySeparatorChar = '\\';
		}

		[TestFixtureTearDown]
		public void RestoreDefaultSeparators()
		{
			PathUtils.DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
			PathUtils.AltDirectorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar;
		}

		[Test]
		public void IsAssemblyFileType()
		{
			Assert.IsTrue( PathUtils.IsAssemblyFileType( "/bin/test.dll" ) );
			Assert.IsTrue( PathUtils.IsAssemblyFileType( "test.exe" ) );
			Assert.IsFalse( PathUtils.IsAssemblyFileType( "/bin/test.nunit" ) );
		}

		[Test]
		public void Canonicalize()
		{
			Assert.AreEqual( "/folder1/file.tmp",
				PathUtils.Canonicalize( "/folder1/./folder2/../file.tmp" ) );
			Assert.AreEqual( "folder1/file.tmp",
				PathUtils.Canonicalize( "folder1/./folder2/../file.tmp" ) );
			Assert.AreEqual( "folder1/file.tmp", 
				PathUtils.Canonicalize( "folder1/folder2/./../file.tmp" ) );
			Assert.AreEqual( "file.tmp", 
				PathUtils.Canonicalize( "folder1/folder2/.././../file.tmp" ) );
			Assert.AreEqual( "file.tmp", 
				PathUtils.Canonicalize( "folder1/folder2/../../../file.tmp" ) );
		}

		[Test]
		public void RelativePath()
		{
			Assert.AreEqual( "folder2/folder3", 
				PathUtils.RelativePath(	"/folder1", "/folder1/folder2/folder3" ) );
			Assert.AreEqual( "../folder2/folder3", 
				PathUtils.RelativePath( "/folder1", "/folder2/folder3" ) );
			Assert.AreEqual( "bin/debug", 
				PathUtils.RelativePath( "/folder1", "bin/debug" ) );
			Assert.AreEqual( "../other/folder", 
				PathUtils.RelativePath( "/folder", "/other/folder" ) );
			Assert.AreEqual( "../../d",
				PathUtils.RelativePath( "/a/b/c", "/a/d" ) );
		}

		[Test]
		public void SamePath()
		{
			Assert.SamePath( "/folder1/file.tmp", "/folder1/./folder2/../file.tmp" );
			Assert.NotSamePath( "/folder1/file.tmp", "/folder1/File.TMP" );
			Assert.NotSamePath( "/folder1/file.tmp", "/folder1/./folder2/../file.temp" );

			Assert.SamePath( "/folder1/folder2", @"\folder1\folder2" );
		}

		[Test]
		public void SamePathOrUnder()
		{
			Assert.SamePathOrUnder( "/folder1/folder2/folder3", "/folder1/./folder2/junk/../folder3" );
			Assert.SamePathOrUnder( "/folder1/folder2/", "/folder1/./folder2/junk/../folder3" );
			Assert.SamePathOrUnder( "/folder1/folder2", "/folder1/./folder2/junk/../folder3" );
			Assert.NotSamePathOrUnder( "/folder1/folder2", "/folder1/./Folder2/junk/../folder3" );
			Assert.NotSamePathOrUnder( "/folder1/folder2", "/folder1/./folder22/junk/../folder3" );
			Assert.SamePathOrUnder( "/", "/" );
			Assert.SamePathOrUnder( "/", "/bin/debug" );
		}
	}
}
