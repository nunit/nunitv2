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

namespace NUnit.Tests.Assemblies
{
	using System;
	using System.Configuration;
	using System.IO;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Threading;
	using NUnit.Framework;
	using NUnit.Core;

	[TestFixture]
	public class AssemblyTests 
	{
		private string testsDll = "NUnit.Tests.dll";
		private Assembly testAssembly;
		private Type assemblyTestType;

		[SetUp]
		public void SetUp() 
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			testAssembly = builder.Load(testsDll);
			assemblyTestType = testAssembly.GetType("NUnit.Tests.assemblies.AssemblyTests");
		}

		[Test]
		public void LoadAssembly()
		{
			Assert.IsNotNull(testAssembly, "should be able to load assembly");
		}

		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void LoadAssemblyNotFound()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			Assembly assembly = builder.Load("XXXX");
		}

		[Test]
		public void LoadAssemblyWithoutTestFixtures()
		{
			string fileName = "nunit.extensions.dll";
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build(fileName);
			Assert.IsNotNull(suite,"Should not be null");
			Assert.IsFalse(suite.ShouldRun, "Should not run");
			Assert.AreEqual(suite.IgnoreReason, "Has no TestFixtures");
			Assert.AreEqual(0, suite.Tests.Count);
		}

		[Test]
		public void LoadTestFixtureFromAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build( testsDll, "NUnit.Tests.Assemblies.AssemblyTests" );
			Assert.IsNotNull(suite, "Should not be Null");
			Assert.AreEqual(suite.CountTestCases,TestCaseBuilder.CountTestCases(this));
		}

		[Test]
		public void GetNamespace()
		{
			string typeNamespace = this.GetType().Namespace;
			Assert.AreEqual(typeNamespace, "NUnit.Tests.Assemblies");
		}


		[Test]
		public void AppSettingsLoaded()
		{
			Assert.IsNull(ConfigurationSettings.AppSettings["tooltip.ShowAlways"]);
			Assert.IsNotNull(ConfigurationSettings.AppSettings["test.setting"], 
				"test.setting should not be null");
		}
	}
}
