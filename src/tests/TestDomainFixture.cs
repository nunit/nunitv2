
namespace NUnit.Tests
{
	using System;
	using System.IO;
	using NUnit.Framework;
	using NUnit.Core;

	/// <summary>
	/// Summary description for TestDomainFixture.
	/// </summary>
	/// 
	[TestFixture]
	public class TestDomainFixture
	{
		private TestDomain domain; 
		private TextWriter outStream;
		private TextWriter errorStream;
		private static readonly string tempFile = "x.dll";


		[SetUp]
		public void MakeAppDomain()
		{
			outStream = new ConsoleWriter(Console.Out);
			errorStream = new ConsoleWriter(Console.Error);
			domain = new TestDomain(outStream, errorStream);
		}

		[TearDown]
		public void UnloadTestDomain()
		{
			domain.Unload();
			domain = null;

			FileInfo info = new FileInfo(tempFile);
			if(info.Exists) info.Delete();
		}
			
		[Test]
		public void InitTest()
		{
			Test test = domain.Load("mock-assembly.dll");
			Assertion.AssertNotNull("Test should not be null", test);
		}

		[Test]
		public void CountTestCases()
		{
			Test test = domain.Load("mock-assembly.dll");
			Assertion.AssertEquals(7, test.CountTestCases);
		}

		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void FileNotFound()
		{
			Test test = domain.Load("xxxx");
		}

		[Test]
		[ExpectedException(typeof(BadImageFormatException))]
		public void FileFoundButNotValidAssembly()
		{
			FileInfo file = new FileInfo(tempFile);

			StreamWriter sw = file.AppendText();

			sw.WriteLine("This is a new entry to add to the file");
			sw.WriteLine("This is yet another line to add...");
			sw.Flush();
			sw.Close();

			Test test = domain.Load(tempFile);
		}

		[Test]
		public void RunMockAssembly()
		{
			Test test = domain.Load("mock-assembly.dll");

			TestResult result = domain.Run(NullListener.NULL);
			Assertion.AssertNotNull(result);
		}

		[Test]
		public void MockAssemblyResults()
		{
			Test test = domain.Load("mock-assembly.dll");

			TestResult result = domain.Run(NullListener.NULL);
			Assertion.AssertEquals(true, result.IsSuccess);
			
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assertion.AssertEquals(5, summarizer.ResultCount);
			Assertion.AssertEquals(2, summarizer.TestsNotRun);
		}

		[Test]
		public void SpecificTestFixture()
		{
			Test test = domain.Load("NUnit.Tests.Assemblies.MockTestFixture", "mock-assembly.dll");

			TestResult result = domain.Run(NullListener.NULL);
			Assertion.AssertEquals(true, result.IsSuccess);
			
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assertion.AssertEquals(3, summarizer.ResultCount);
			Assertion.AssertEquals(2, summarizer.TestsNotRun);
		}

		[Test]
		public void InvalidTestFixture()
		{
			Test test = domain.Load("NUnit.Tests.Assemblies.Bogus", "mock-assembly.dll");
			Assertion.AssertNull("test should be null", test);
		}
	}
}
