using System;
using System.IO;
using System.Collections;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for MultipleAssembliesDomain.
	/// </summary>
	[TestFixture]
	public class MultipleAssembliesTestDomain
	{
		private Test suite;

		private TestDomain domain; 
		private TextWriter outStream;
		private TextWriter errorStream;

		[SetUp]
		public void LoadSuite()
		{
			outStream = new ConsoleWriter(Console.Out);
			errorStream = new ConsoleWriter(Console.Error);
			domain = new TestDomain(outStream, errorStream);

			AssemblyList assemblies = new AssemblyList();
			assemblies.Add( Path.GetFullPath( "nonamespace-assembly.dll" ) );
			assemblies.Add( Path.GetFullPath( "mock-assembly.dll" ) );

			suite = domain.Load( Path.GetFullPath( "TestSuite.nunit" ), assemblies );
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
			Assert.NotNull(suite);
		}

		[Test]
		public void RootNode()
		{
			Assert.True( suite is RootTestSuite );
			Assert.Equals( Path.GetFullPath( "TestSuite.nunit" ), suite.Name );
		}

		[Test]
		public void AssemblyNodes()
		{
			Assert.True( suite.Tests[0] is AssemblyTestSuite );
			Assert.True( suite.Tests[1] is AssemblyTestSuite );
		}

		[Test]
		public void TestCaseCount()
		{
			Assert.Equals(10, suite.CountTestCases);
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			TestResult result = suite.Run(NullListener.NULL);
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.Equals(8, summary.ResultCount);
		}
	}
}
