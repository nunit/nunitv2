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
using NUnit.Framework;
using NUnit.Core;
using NUnit.TestUtilities;
using NUnit.TestData.SuiteBuilderTests;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class SuiteBuilderTests
	{
		private string testsDll = "nunit.core.tests.dll";
		private string testData = "test-assembly.dll";
		private string tempFile = "x.dll";
		private TestSuiteBuilder builder;

		[SetUp]
		public void CreateBuilder()
		{
			builder = new TestSuiteBuilder();
		}
		[TearDown]
		public void TearDown()
		{
			FileInfo info = new FileInfo(tempFile);
			if(info.Exists) info.Delete();
		}

		[Test]
		public void LoadAssembly() 
		{
			TestSuite suite = builder.Build(testsDll);
			Assert.IsNotNull(suite, "Unable to build suite" );
			Assert.AreEqual( 1, suite.Tests.Count );
			Assert.AreEqual( "NUnit", ((ITest)suite.Tests[0]).Name );
		}

		[Test]
		public void LoadAssemblyWithoutNamespaces()
		{
			builder.AutoNamespaceSuites = false;
			TestSuite suite = builder.Build(testsDll);
			Assert.IsNotNull(suite, "Unable to build suite" );
			Assert.Greater( suite.Tests.Count, 1 );
			Assert.IsTrue(((ITest)suite.Tests[0]).IsFixture, "Should be a fixture" );
		}

		[Test]
		public void LoadFixture()
		{
			TestSuite suite = builder.Build(testsDll, "NUnit.Core.Tests.SuiteBuilderTests");
			Assert.IsNotNull(suite, "Unable to build suite");
		}

		[Test]
		public void LoadSuite()
		{
			TestSuite suite = builder.Build( testsDll, "NUnit.Core.Tests.AllTests" );
			Assert.IsNotNull(suite, "Unable to build suite");
			Assert.AreEqual( 3, suite.Tests.Count );
		}

		[Test]
		public void LoadNamespaceAsSuite()
		{
			TestSuite suite= builder.Build( testsDll, "NUnit.Core.Tests" );
			Assert.IsNotNull( suite );
			Assert.AreEqual( testsDll, suite.Name );
			Assert.AreEqual( "NUnit", ((Test)suite.Tests[0]).Name );
		}

		[Test]
		public void DiscoverSuite()
		{
			TestSuite suite = builder.Build( testData, "NUnit.TestData.SuiteBuilderTests.Suite" );
			Assert.IsNotNull(suite, "Could not discover suite attribute");
		}

		[Test]
		public void WrongReturnTypeSuite()
		{
			TestSuite suite = builder.Build( testData, "NUnit.TestData.SuiteBuilderTests.NonConformingSuite" );
			Assert.IsNull(suite, "Suite property returns wrong type");
		}

		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void FileNotFound()
		{
			builder.Build( "xxxx" );
		}

		// Gives FileNotFoundException on Mono
		[Test, Platform(Exclude="Mono")]
		[ExpectedException(typeof(BadImageFormatException))]
		public void InvalidAssembly()
		{
			FileInfo file = new FileInfo( tempFile );

			StreamWriter sw = file.AppendText();

			sw.WriteLine("This is a new entry to add to the file");
			sw.WriteLine("This is yet another line to add...");
			sw.Flush();
			sw.Close();

			builder.Build( tempFile );
		}

		[Test]
		public void FixtureNotFound()
		{
			TestSuite suite = builder.Build(testsDll, "NUnit.Tests.Junk" );
			Assert.IsNull( suite );
		}
	}
}
