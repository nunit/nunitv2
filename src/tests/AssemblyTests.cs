#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
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

	/// <summary>
	/// Summary description for AssemblyTests.
	/// </summary>
	/// 
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
			Assertion.Assert("should be able to load assembly", testAssembly != null);
		}

		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void LoadAssemblyNotFound()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			Assembly assembly = builder.Load("XXXX");
		}

		[Test]
		[ExpectedException(typeof(NoTestFixturesException))]
		public void LoadAssemblyWithoutTestFixtures()
		{
			string fileName = "nunit.extensions.dll";
			TestSuiteBuilder builder = new TestSuiteBuilder();
			builder.Build(fileName);
		}

		[Test]
		public void LoadTestFixtureFromAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build("NUnit.Tests.Assemblies.AssemblyTests", testsDll);
			Assertion.Assert(suite != null);
			Assertion.AssertEquals(suite.CountTestCases,TestCaseBuilder.CountTestCases(this));
		}

		[Test]
		public void GetNamespace()
		{
			string typeNamespace = this.GetType().Namespace;
			Assertion.AssertEquals("NUnit.Tests.Assemblies", typeNamespace);
		}


		[Test]
		public void AppSettingsLoaded()
		{
			Assertion.AssertNull(ConfigurationSettings.AppSettings["tooltip.ShowAlways"]);
			Assertion.AssertNotNull("test.setting should not be null", ConfigurationSettings.AppSettings["test.setting"]);
		}
	}
}
