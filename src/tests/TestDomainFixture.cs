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
