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
using NUnit.Core;
using NUnit.Util;
using NUnit.Tests.Assemblies;

namespace NUnit.Tests.Core
{
	/// <summary>
	/// Summary description for MultipleAssembliesDomain.
	/// </summary>
	[TestFixture]
	public class TestDomainTests_Multiple
	{
		private string[] assemblies;
		private TestDomain domain; 
		private TextWriter outStream;
		private TextWriter errorStream;

		private string name = "Multiple Assemblies Test";

		[SetUp]
		public void Init()
		{
			outStream = new ConsoleWriter(Console.Out);
			errorStream = new ConsoleWriter(Console.Error);
			
			domain = new TestDomain();
			assemblies = new string[]
				{ Path.GetFullPath( "nonamespace-assembly.dll" ), Path.GetFullPath( "mock-assembly.dll" ) };
		}

		[TearDown]
		public void UnloadTestDomain()
		{
			domain.Unload();
			domain = null;
		}
			
		[Test]
		public void BuildSuite()
		{
			Test suite = domain.Load( name, assemblies );
			Assert.IsNotNull(suite);
		}

		[Test]
		public void RootNode()
		{
			Test suite = domain.Load( name, assemblies );
			Assert.IsTrue( suite is RootTestSuite );
			Assert.AreEqual( name, suite.Name );
		}

		[Test]
		public void AssemblyNodes()
		{
			Test suite = domain.Load( name, assemblies );
			Assert.IsTrue( suite.Tests[0] is AssemblyTestSuite );
			Assert.IsTrue( suite.Tests[1] is AssemblyTestSuite );
		}

		[Test]
		public void TestCaseCount()
		{
			Test suite = domain.Load( name, assemblies );
			Assert.AreEqual(NoNamespaceTestFixture.Tests + MockAssembly.Tests, 
				suite.CountTestCases());
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			Test suite = domain.Load( name, assemblies );
			TestResult result = suite.Run(NullListener.NULL);
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.AreEqual(
				NoNamespaceTestFixture.Tests + MockAssembly.Tests - MockAssembly.NotRun, 
				summary.ResultCount);
		}

		[Test]
		public void LoadFixture()
		{
			Test suite = domain.Load( name, assemblies, "NUnit.Tests.Assemblies.MockTestFixture" );
			Assert.IsNotNull( suite );
			Assert.AreEqual( MockTestFixture.Tests, suite.CountTestCases() );
		}
	}
}
