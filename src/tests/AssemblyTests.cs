//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Tests.Assemblies
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Threading;
	using Nunit.Framework;
	using Nunit.Core;

	/// <summary>
	/// Summary description for AssemblyTests.
	/// </summary>
	/// 
	[TestFixture]
	public class AssemblyTests 
	{
		private string testsDll = "Nunit.Tests.dll";
		private Assembly testAssembly;
		private Type assemblyTestType;

		[SetUp]
		public void SetUp() 
		{
			testAssembly = TestSuiteBuilder.Load(testsDll);
			assemblyTestType = testAssembly.GetType("Nunit.Tests.assemblies.AssemblyTests");
		}

		[Test]
		public void LoadAssembly()
		{
			Assertion.Assert("should be able to load assembly", testAssembly != null);
		}

		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void LoadAsssemblyNotFound()
		{
			Assembly assembly = TestSuiteBuilder.Load("XXXX");
		}

		[Test]
		[ExpectedException(typeof(NoTestFixturesException))]
		public void LoadAsssemblyWithoutTestFixtures()
		{
			string fileName = "nunit.core.dll";
			TestSuiteBuilder.Build(fileName);
		}

		[Test]
		public void LoadTestFixtureFromAssembly()
		{
			TestSuite suite = TestSuiteBuilder.Build("Nunit.Tests.Assemblies.AssemblyTests", testsDll);
			Assertion.Assert(suite != null);
			Assertion.AssertEquals(suite.CountTestCases,TestCaseBuilder.CountTestCases(this));
		}

		[Test]
		public void GetNamespace()
		{
			string typeNamespace = this.GetType().Namespace;
			Assertion.AssertEquals("Nunit.Tests.Assemblies", typeNamespace);
		}
	}
}
