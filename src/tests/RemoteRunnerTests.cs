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
	[TestFixture]
	public class RemoteRunnerTests
	{
		private readonly string testsDll = "nonamespace-assembly.dll";
		private readonly string mockDll = "mock-assembly.dll";
		private RemoteTestRunner runner;
		private string[] assemblies;

		[SetUp]
		public void CreateRunner()
		{
			runner = new RemoteTestRunner();
			assemblies = new string[] { testsDll, mockDll };
		}

		[Test]
		public void LoadAssembly() 
		{
			Test test = runner.Load(testsDll);
			Assert.IsNotNull(test, "Unable to load assembly" );
		}

		[Test]
		public void LoadAssemblyWithFixture()
		{
			Test test = runner.Load( mockDll, "NUnit.Tests.Assemblies.MockTestFixture" );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void LoadAssemblyWithSuite()
		{
			Test test = runner.Load( mockDll, "NUnit.Tests.Assemblies.MockSuite" );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void CountTestCases()
		{
			Test test = runner.Load( mockDll );
			Assert.AreEqual( MockAssembly.Tests, test.CountTestCases() );
		}

		[Test]
		public void LoadMultipleAssemblies()
		{
			Test test = runner.Load( "TestSuite", assemblies );
			Assert.IsNotNull( test, "Unable to load assemblies" );
		}

		[Test]
		public void LoadMultipleAssembliesWithFixture()
		{
			Test test = runner.Load( "TestSuite", assemblies, "NUnit.Tests.Assemblies.MockTestFixture"  );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void LoadMultipleAssembliesWithSuite()
		{
			Test test = runner.Load( "TestSuite", assemblies, "NUnit.Tests.Assemblies.MockSuite" );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void CountTestCasesAcrossMultipleAssemblies()
		{
			assemblies[0] = "nonamespace-assembly.dll";
			Test test = runner.Load( "TestSuite", assemblies );
			Assert.AreEqual( NoNamespaceTestFixture.Tests + MockAssembly.Tests, 
				test.CountTestCases() );			
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			assemblies[0] = "nonamespace-assembly.dll";
			Test test = runner.Load( "TestSuite", assemblies);
			TestResult result = runner.Run( NullListener.NULL );
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.AreEqual( 
				NoNamespaceTestFixture.Tests + MockAssembly.Tests - MockAssembly.NotRun, 
				summary.ResultCount);
		}
	}
}
