using System;
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.Tests
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
			Assert.True( ProjectPath.IsAssemblyFileType( @"c:\bin\test.dll" ) );
			Assert.True( ProjectPath.IsAssemblyFileType( @"test.exe" ) );
			Assert.False( ProjectPath.IsAssemblyFileType( @"c:\bin\test.nunit" ) );
		}

		[Test]
		public void Canonicalize()
		{
			Assert.Equals( @"C:\folder1\file.tmp",
				ProjectPath.Canonicalize( @"C:\folder1\.\folder2\..\file.tmp" ) );
			Assert.Equals( @"folder1\file.tmp",
				ProjectPath.Canonicalize( @"folder1\.\folder2\..\file.tmp" ) );
		}

		[Test]
		public void RelativePath()
		{
			Assert.Equals( @"folder2\folder3", ProjectPath.RelativePath( 
				@"c:\folder1", @"c:\folder1\folder2\folder3" ) );
			Assert.Equals( @"..\folder2\folder3", ProjectPath.RelativePath(
				@"c:\folder1", @"c:\folder2\folder3" ) );
			Assert.Equals( @"bin\debug", ProjectPath.RelativePath(
				@"c:\folder1", @"bin\debug" ) );
			Assert.Null( "Unrelated paths should return null",
				ProjectPath.RelativePath( @"C:\folder", @"D:\folder" ) );
		}

		[Test]
		public void SamePath()
		{
			Assert.True( ProjectPath.SamePath( @"C:\folder1\file.tmp", @"c:\folder1\File.TMP" ) );
			Assert.True( ProjectPath.SamePath( @"C:\folder1\file.tmp", @"C:\folder1\.\folder2\..\file.tmp" ) );
			Assert.False( ProjectPath.SamePath( @"C:\folder1\file.tmp", @"C:\folder1\.\folder2\..\file.temp" ) );
		}

		[Test]
		public void SamePathOrUnder()
		{
			Assert.True( ProjectPath.SamePathOrUnder( @"C:\folder1\folder2\folder3", @"c:\folder1\.\folder2\junk\..\folder3" ) );
			Assert.True( ProjectPath.SamePathOrUnder( @"C:\folder1\folder2\", @"c:\folder1\.\folder2\junk\..\folder3" ) );
			Assert.True( ProjectPath.SamePathOrUnder( @"C:\folder1\folder2", @"c:\folder1\.\folder2\junk\..\folder3" ) );
			Assert.False( ProjectPath.SamePathOrUnder( @"C:\folder1\folder2", @"c:\folder1\.\folder22\junk\..\folder3" ) );
			Assert.False( ProjectPath.SamePathOrUnder( @"C:\folder1\folder2ile.tmp", @"D:\folder1\.\folder2\folder3\file.tmp" ) );
			Assert.False( ProjectPath.SamePathOrUnder( @"C:\", @"D:\" ) );
			Assert.True( ProjectPath.SamePathOrUnder( @"C:\", @"c:\" ) );
			Assert.True( ProjectPath.SamePathOrUnder( @"C:\", @"c:\bin\debug" ) );

		}
	}
}
