using System;
using NUnit.Framework;
using System.IO;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class AssemblyReaderTests
	{
		private AssemblyReader rdr;

		[SetUp]
		public void CreateReader()
		{
			rdr = new AssemblyReader( this.GetType().Assembly );
		}

		[TearDown]
		public void DisposeReader()
		{
			if ( rdr != null )
				rdr.Dispose();

			rdr = null;
		}

		[Test]
		public void CreateFromPath()
		{
			Assert.AreEqual( "nunit.core.tests.dll", new AssemblyReader( "nunit.core.tests.dll" ).AssemblyPath );
		}

		[Test]
		public void CreateFromAssembly()
		{
			StringAssert.AreEqualIgnoringCase( Path.GetFullPath("nunit.core.tests.dll"), rdr.AssemblyPath );
		}

		[Test]
		public void IsValidPeFile()
		{
			Assert.IsTrue( rdr.IsValidPeFile );
		}

		[Test]
		public void IsValidPeFile_Fails()
		{
			Assert.IsFalse( new AssemblyReader( "nunit.core.tests.dll.config" ).IsValidPeFile );
		}

		[Test]
		public void IsDotNetFile()
		{
			Assert.IsTrue( rdr.IsDotNetFile );
		}

		[Test]
		public void ImageRuntimeVersion()
		{
			string runtimeVersion = rdr.ImageRuntimeVersion;

			StringAssert.StartsWith( "v", runtimeVersion );
			Version version = new Version( runtimeVersion.Substring( 1 ) );
			Assert.LessOrEqual( version, Environment.Version );
		}

	}
}
