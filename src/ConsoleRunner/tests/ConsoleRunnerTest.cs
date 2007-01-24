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
using System.Text;
using System.Collections;

using NUnit.Framework;
using NUnit.TestData.ConsoleRunnerTest;

namespace NUnit.ConsoleRunner.Tests
{
	[TestFixture]
	public class ConsoleRunnerTest
	{
		private static readonly string xmlFile = "console-test.xml";
		private StringBuilder output;
		TextWriter saveOut;

		[SetUp]
		public void Init()
		{
			output = new StringBuilder();

			Console.Out.Flush();
			saveOut = Console.Out;
			Console.SetOut( new StringWriter( output ) );
		}

		[TearDown]
		public void CleanUp()
		{
			Console.SetOut( saveOut );

			FileInfo file = new FileInfo(xmlFile);
			if(file.Exists) file.Delete();

			file = new FileInfo( "TestResult.xml" );
			if(file.Exists) file.Delete();
		}

		[Test]
		public void FailureFixture() 
		{
			int resultCode = runFixture( typeof ( FailureTest ) ); 
			Assert.AreEqual(1, resultCode);
		}

		[Test]
		public void SuccessFixture() 
		{
			int resultCode = runFixture( 
				typeof(SuccessTest) );
			Assert.AreEqual(0, resultCode);
		}

		[Test]
		public void XmlResult() 
		{
			FileInfo info = new FileInfo(xmlFile);
			info.Delete();

			int resultCode = runFixture( 
				typeof(SuccessTest),
				"-xml:" + info.FullName );

			Assert.AreEqual(0, resultCode);
			Assert.AreEqual(true, info.Exists);
		}

		[Test]
		public void InvalidFixture()
		{
			int resultCode = executeConsole( new string[] 
				{ GetType().Module.Name, "-fixture:NUnit.Tests.BogusTest" } );
			Assert.AreEqual(2, resultCode);
		}

		[Test]
		public void InvalidAssembly()
		{
			int resultCode = executeConsole( new string[] { "badassembly.dll" } );
			Assert.AreEqual(2, resultCode);
		}

		[Test]
		public void XmlToConsole() 
		{
			int resultCode = runFixture( 
				typeof(SuccessTest),
				"-xmlconsole", 
				"-nologo" );

			Assert.AreEqual(0, resultCode);
			StringAssert.Contains( @"<?xml version=""1.0""", output.ToString(),
				"Only XML should be displayed in xmlconsole mode");
		}

		[Test]
		public void Bug1073539Test()
		{
			int resultCode = runFixture( typeof( Bug1073539Fixture ) );
			Assert.AreEqual( 1, resultCode );
		}

		[Test]
		public void Bug1311644Test()
		{
			int resultCode = runFixture( typeof( Bug1311644Fixture ) );
			Assert.AreEqual( 1, resultCode );
		}

		[Test, Platform(Exclude="Mono", Reason="Hangs on Mono")]
		public void CanRunWithoutTestDomain()
		{
			Assert.AreEqual( 0, executeConsole( "mock-assembly.dll", "/domain:None" ) );
			StringAssert.Contains( "Failures: 0", output.ToString() );
		}

		[Test]
		public void CanRunWithSingleTestDomain()
		{
			Assert.AreEqual( 0, executeConsole( "mock-assembly.dll", "/domain:Single" ) );
			StringAssert.Contains( "Failures: 0", output.ToString() );
		}

		[Test]
		public void CanRunWithMultipleTestDomains()
		{
			Assert.AreEqual( 0, executeConsole( "mock-assembly.dll", "nonamespace-assembly.dll", "/domain:Multiple" ) );
			StringAssert.Contains( "Failures: 0", output.ToString() );
		}

		[Test, Platform(Exclude="Mono", Reason="Hangs on Mono")]
		public void CanRunWithoutTestDomain_NoThread()
		{
			Assert.AreEqual( 0, executeConsole( "mock-assembly.dll", "/domain:None", "/nothread" ) );
			StringAssert.Contains( "Failures: 0", output.ToString() );
		}

		[Test]
		public void CanRunWithSingleTestDomain_NoThread()
		{
			Assert.AreEqual( 0, executeConsole( "mock-assembly.dll", "/domain:Single", "/nothread" ) );
			StringAssert.Contains( "Failures: 0", output.ToString() );
		}

		[Test]
		public void CanRunWithMultipleTestDomains_NoThread()
		{
			Assert.AreEqual( 0, executeConsole( "mock-assembly.dll", "nonamespace-assembly.dll", "/domain:Multiple", "/nothread" ) );
			StringAssert.Contains( "Failures: 0", output.ToString() );
		}

		private int runFixture( Type type )
		{
			return executeConsole( new string[] 
				{ type.Module.Name, "-fixture:" + type.FullName } );
		}

		private int runFixture( Type type, params string[] arguments )
		{
			string[] args = new string[arguments.Length+2];
			int n = 0;
			args[n++] = type.Module.Name;
			args[n++] = "-fixture:" + type.FullName;
			foreach( string arg in arguments )
				args[n++] = arg;
			return executeConsole( args ); 
		}

		private int executeConsole( params string[] arguments )
		{
			return NUnit.ConsoleRunner.ConsoleUi.Main( arguments );
		}
	}
}
