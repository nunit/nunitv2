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
using System.IO;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests.Core
{
	[TestFixture]
	public class TestDomainFixture
	{
		private TestDomain domain; 
		private TextWriter outStream;
		private TextWriter errorStream;
		private static readonly string tempFile = "x.dll";
		private ArrayList assemblies; 


		[SetUp]
		public void MakeAppDomain()
		{
			outStream = new ConsoleWriter(Console.Out);
			errorStream = new ConsoleWriter(Console.Error);
			domain = new TestDomain();

			assemblies = new ArrayList();
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
			Test test = domain.LoadAssembly("mock-assembly.dll");
			Assert.IsNotNull(test, "Test should not be null");
		}

		[Test]
		public void CountTestCases()
		{
			Test test = domain.LoadAssembly("mock-assembly.dll");
			Assert.AreEqual(7, test.CountTestCases);
		}

		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void FileNotFound()
		{
			Test test = domain.LoadAssembly("xxxx");
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

			Test test = domain.LoadAssembly(tempFile);
		}

		[Test]
		public void RunMockAssembly()
		{
			Test test = domain.LoadAssembly("mock-assembly.dll");

			TestResult result = domain.Run(NullListener.NULL,outStream,errorStream);
			Assert.IsNotNull(result);
		}

		[Test]
		public void MockAssemblyResults()
		{
			Test test = domain.LoadAssembly("mock-assembly.dll");

			TestResult result = domain.Run(NullListener.NULL, outStream, errorStream);
			Assert.AreEqual(true, result.IsSuccess);
			
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assert.AreEqual(5, summarizer.ResultCount);
			Assert.AreEqual(2, summarizer.TestsNotRun);
		}

		[Test]
		public void SpecificTestFixture()
		{
			Test test = domain.LoadAssembly( "mock-assembly.dll", "NUnit.Tests.Assemblies.MockTestFixture" );

			TestResult result = domain.Run(NullListener.NULL, outStream, errorStream);
			Assert.AreEqual(true, result.IsSuccess);
			
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assert.AreEqual(3, summarizer.ResultCount);
			Assert.AreEqual(2, summarizer.TestsNotRun);
		}

		[Test]
		public void InvalidTestFixture()
		{
			Test test = domain.LoadAssembly( "mock-assembly.dll", "NUnit.Tests.Assemblies.Bogus" );
			Assert.IsNull(test, "test should be null");
		}

		[Test]
		public void MultipleAssemblies()
		{
			ArrayList assemblies = new ArrayList();
			assemblies.Add("mock-assembly.dll");
			assemblies.Add("nonamespace-assembly.dll");

			Test test = domain.LoadAssemblies( "Multiple", assemblies );
			Assert.IsNotNull(test, "test should not be null");
			Assert.AreEqual(10, test.CountTestCases);
		}

		[Test]
		public void BinPath()
		{
			ArrayList assemblies = new ArrayList();
			assemblies.Add( @"h:\app1\bin\debug\test1.dll" );
			assemblies.Add( @"h:\app2\bin\debug\test2.dll" );
			assemblies.Add( @"h:\app1\bin\debug\test3.dll" );

			Assert.AreEqual( @"h:\app1\bin\debug;h:\app2\bin\debug", 
				TestDomain.GetBinPath( assemblies ) );
		}
	}
}
