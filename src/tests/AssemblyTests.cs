/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
namespace NUnit.Tests.Assemblies
{
	using System;
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
		public void LoadAsssemblyNotFound()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			Assembly assembly = builder.Load("XXXX");
		}

		[Test]
		[ExpectedException(typeof(NoTestFixturesException))]
		public void LoadAsssemblyWithoutTestFixtures()
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
	}
}
