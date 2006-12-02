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

using System.IO;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.Assemblies;

namespace NUnit.Util.Tests
{
	/// <summary>
	/// Summary description for MultipleAssembliesDomain.
	/// </summary>
	[TestFixture]
	public class TestDomainTests_Multiple
	{
		private TestDomain domain; 
		private ITest loadedSuite;

		private static string path1 = Path.GetFullPath( "nonamespace-assembly.dll" );
		private static string path2 = Path.GetFullPath( "mock-assembly.dll" );

		private string name = "Multiple Assemblies Test";

		[TestFixtureSetUp]
		public void Init()
		{
			domain = new TestDomain();
			TestPackage package = new TestPackage( name );
			package.Assemblies.Add( path1 );
			package.Assemblies.Add( path2 );
			domain.Load( package );
			loadedSuite = domain.Test;
		}

		[TestFixtureTearDown]
		public void UnloadTestDomain()
		{
			domain.Unload();
			domain = null;
		}
			
		[Test]
		public void BuildSuite()
		{
			Assert.IsNotNull(loadedSuite);
		}

		[Test]
		public void RootNode()
		{
			Assert.AreEqual( name, loadedSuite.TestName.Name );
		}

		[Test]
		public void AssemblyNodes()
		{
			TestNode test0 = (TestNode)loadedSuite.Tests[0];
			TestNode test1 = (TestNode)loadedSuite.Tests[1];
			Assert.AreEqual( path1, test0.TestName.Name );
			Assert.AreEqual( path2, test1.TestName.Name );
		}

		[Test]
		public void TestCaseCount()
		{
			Assert.AreEqual(NoNamespaceTestFixture.Tests + MockAssembly.Tests, 
				loadedSuite.TestCount );
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			TestResult result = domain.Run(NullListener.NULL);
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.AreEqual(
				NoNamespaceTestFixture.Tests + MockAssembly.Tests - MockAssembly.NotRun, 
				summary.ResultCount);
		}
	}

	[TestFixture]
	public class TestDomainTests_MultipleFixture
	{
		[Test]
		public void LoadFixture()
		{
			TestDomain domain = new TestDomain();
			TestPackage package = new TestPackage( "Multiple Assemblies Test" );
			package.Assemblies.Add( Path.GetFullPath( "nonamespace-assembly.dll" ) );
			package.Assemblies.Add( Path.GetFullPath( "mock-assembly.dll" ) );
			package.TestName = "NUnit.Tests.Assemblies.MockTestFixture";
			domain.Load( package );
			Assert.AreEqual( MockTestFixture.Tests, domain.Test.TestCount );
		}
	}
}
