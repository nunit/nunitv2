using System;
using System.IO;
using System.Collections;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for AssemblyListTests.
	/// </summary>
	[TestFixture]
	public class AssemblyListTests
	{
		private AssemblyList assemblies;

		[SetUp]
		public void CreateAssemblyList()
		{
			assemblies = new AssemblyList();
		}

		[Test]
		public void EmptyList()
		{
			Assert.Equals( 0, assemblies.Count );
		}

		[Test]
		public void AddAssemblies()
		{
			assemblies.Add( @"bin\debug\assembly1.dll" );
			assemblies.Add( @"bin\debug\assembly2.dll" );

			string path1 = AssemblyList.GetFullPath( @"bin\debug\assembly1.dll" );
			string path2 = AssemblyList.GetFullPath( @"bin\debug\assembly2.dll" );

			Assert.Equals( 2, assemblies.Count );
			Assert.Equals( path1, assemblies[0] );
			Assert.Equals( path2, assemblies[1] );
		}

		[Test]
		public void TryToAddDuplicates()
		{
			assemblies.Add( @"C:\junk\assembly1.dll" );
			assemblies.Add( @"C:\junk\assembly1.dll" );			
			assemblies.Add( @"C:\junk\Assembly1.dll" );

			Assert.Equals( 1, assemblies.Count );
		}

		[Test]
		public void RemoveAssemblies()
		{
			assemblies.Add( @"bin\debug\assembly1.dll" );
			assemblies.Add( @"bin\debug\assembly2.dll" );
			assemblies.Add( @"bin\debug\assembly3.dll" );
			assemblies.Remove( @"bin\debug\assembly2.dll" );

			string path1 = AssemblyList.GetFullPath( @"bin\debug\assembly1.dll" );
			string path3 = AssemblyList.GetFullPath( @"bin\debug\assembly3.dll" );

			Assert.Equals( 2, assemblies.Count );
			Assert.Equals( path1, assemblies[0] );
			Assert.Equals( path3, assemblies[1] );
		}

		[Test]
		public void Directories()
		{
			assemblies.Add( @"h:\app1\bin\debug\test1.dll" );
			assemblies.Add( @"h:\app2\bin\debug\test2.dll" );
			assemblies.Add( @"h:\app1\bin\debug\test3.dll" );

			Assert.Equals( 2, assemblies.Directories.Count ); 
		}

		[Test]
		public void AssemblyPath()
		{
			assemblies.Add( @"h:\app1\bin\debug\test1.dll" );
			assemblies.Add( @"h:\app2\bin\debug\test2.dll" );
			assemblies.Add( @"h:\app1\bin\debug\test3.dll" );

			Assert.Equals( @"h:\app1\bin\debug;h:\app2\bin\debug", 
				assemblies.AssemblyPath );
		}
	}
}
