using System;
using System.IO;
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
		[Test]
		public void EmptyList()
		{
			AssemblyList assemblies = new AssemblyList();
			Assertion.AssertEquals( 0, assemblies.Count );
		}

		[Test]
		public void AddAssemblies()
		{
			AssemblyList assemblies = new AssemblyList();
			assemblies.Add( @"bin\debug\assembly1.dll" );
			assemblies.Add( @"bin\debug\assembly2.dll" );

			string path1 = AssemblyList.GetFullPath( @"bin\debug\assembly1.dll" );
			string path2 = AssemblyList.GetFullPath( @"bin\debug\assembly2.dll" );

			Assertion.AssertEquals( 2, assemblies.Count );
			Assertion.AssertEquals( path1, assemblies[0] );
			Assertion.AssertEquals( path2, assemblies[1] );
		}

		[Test]
		public void TryToAddDuplicates()
		{
			AssemblyList assemblies = new AssemblyList();
			assemblies.Add( @"C:\junk\assembly1.dll" );
			assemblies.Add( @"C:\junk\assembly1.dll" );			
			assemblies.Add( @"C:\junk\Assembly1.dll" );

			Assertion.AssertEquals( 1, assemblies.Count );
		}

		[Test]
		public void RemoveAssemblies()
		{
			AssemblyList assemblies = new AssemblyList();
			assemblies.Add( @"bin\debug\assembly1.dll" );
			assemblies.Add( @"bin\debug\assembly2.dll" );
			assemblies.Add( @"bin\debug\assembly3.dll" );
			assemblies.Remove( @"bin\debug\assembly2.dll" );

			string path1 = AssemblyList.GetFullPath( @"bin\debug\assembly1.dll" );
			string path3 = AssemblyList.GetFullPath( @"bin\debug\assembly3.dll" );

			Assertion.AssertEquals( 2, assemblies.Count );
			Assertion.AssertEquals( path1, assemblies[0] );
			Assertion.AssertEquals( path3, assemblies[1] );
		}
	}
}
