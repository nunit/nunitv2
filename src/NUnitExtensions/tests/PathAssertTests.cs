using System;

namespace NUnit.Framework.Extensions.Tests
{
	[TestFixture]
	public class PathAssertTests
	{
		[Test]
		public void IdenticalPathsAreSame()
		{
			PathAssert.SamePath( @"C:\folder1\file.tmp", @"C:\folder1\file.tmp" );
			PathAssert.SamePath( "/folder1/file.tmp", "/folder1/file.tmp" );
		}

		[Test, Platform("Win")]
		public void CaseIsIgnoredOnWindows()
		{
			PathAssert.SamePath( @"C:\folder1\file.tmp", @"c:\folder1\File.TMP" );
			PathAssert.SamePath( "/folder1/file.tmp", "/folder1/File.TMP" );
		}

		[Test, Platform("Linux")]
		public void CaseMattersOnLinux()
		{
			PathAssert.NotSamePath( @"C:\folder1\file.tmp", @"c:\folder1\File.TMP" );
			PathAssert.NotSamePath( "/folder1/file.tmp", "/folder1/File.TMP" );
		}

		[Test]
		public void PathsAreCanonicalizedBeforeComparing()
		{
			PathAssert.SamePath( @"C:\folder1\file.tmp", @"C:\folder1\.\folder2\..\file.tmp" );
			PathAssert.SamePath( "/folder1/file.tmp", "/folder1/./folder2/../file.tmp" );
		}

		[Test]
		public void SlashAndBackslashAreEquivalent()
		{
			PathAssert.SamePath( "D:/folder1/folder2", @"D:\folder1\folder2" );
			PathAssert.SamePath( "/folder1/folder2", @"\folder1\folder2" );
		}

		[Test]
		public void NotSamePath()
		{
			PathAssert.NotSamePath( @"C:\folder1\file.tmp", @"C:\folder1\.\folder2\..\file.temp" );
			PathAssert.NotSamePath( "/folder1/file.tmp", "/folder1/./folder1/../folder2/file.temp" );
		}
	}
}
