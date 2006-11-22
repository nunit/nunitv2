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
using System.Collections;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Util;
using NUnit.Tests.Assemblies;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class SuiteBuilderTests_Multiple
	{
		private static readonly int totalTests = NoNamespaceTestFixture.Tests + MockAssembly.Tests;

		private TestSuiteBuilder builder;
		private static string[] assemblies = new string[]
			{ "nonamespace-assembly.dll", "mock-assembly.dll" };
		private Test loadedSuite;

		[SetUp]
		public void LoadSuite()
		{
			builder = new TestSuiteBuilder();
			loadedSuite = builder.Build( new TestPackage( "TestSuite", assemblies ) );
		}

		[Test]
		public void BuildSuite()
		{
			Assert.IsNotNull( loadedSuite );
		}

		[Test]
		public void RootNode()
		{
			Assert.AreEqual( "TestSuite", loadedSuite.TestName.Name );
		}

        //[Test]
        //public void AssemblyNodes()
        //{
        //    Assert.IsTrue( loadedSuite.Tests[0] is TestAssembly );
        //    Assert.IsTrue( loadedSuite.Tests[1] is TestAssembly );
        //}

		[Test]
		public void TestCaseCount()
		{
			Assert.AreEqual( totalTests , loadedSuite.TestCount);
		}

		[Test]
		public void LoadFixture()
		{
			Test suite = builder.Build(
				new TestPackage( "MultipleAssemblies", assemblies ), 
				"NUnit.Tests.Assemblies.MockTestFixture" );
			Assert.IsNotNull( suite );
			Assert.AreEqual( MockTestFixture.Tests, suite.TestCount );
		}
	}
}
