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
using System.Diagnostics;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Collections;

using NUnit.Framework;
using NUnit.Tests.Core;

namespace NUnit.Tests.ConsoleRunner
{
	[TestFixture]
	public class ConsoleRunnerTest : FixtureBase
	{
		private String nunitExe;
		private static readonly string xmlFile = "console-test.xml";
		private AppDomain domain = null;
		private Evidence evidence = null;

		[SetUp]
		public void Setup()
		{
			domain = AppDomain.CreateDomain( "test domain" );
			Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
			evidence = new Evidence( baseEvidence );

			FileInfo file = new FileInfo("nunit-console.exe");
			if(file.Exists)
				nunitExe = file.FullName;
			else
#if DEBUG
				nunitExe = SourcePath + @"\nunit-console\bin\Debug\nunit-console.exe";
#else
				nunitExe = SourcePath + @"\nunit-console\bin\Release\nunit-console.exe";
#endif
		}

		[TearDown]
		public void CleanUp()
		{
			FileInfo file = new FileInfo(xmlFile);
			if(file.Exists) file.Delete();

			file = new FileInfo( "TestResult.xml" );
			if(file.Exists) file.Delete();

			if ( domain != null )
				AppDomain.Unload( domain );
		}

		[TestFixture] internal class FailureTest
		{
			[Test]
			public void Fail()
			{
				Assert.Fail();
			}
		}

		[Test]
		public void FailureFixture() 
		{
			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(NUnit.Tests.ConsoleRunner.ConsoleRunnerTest.FailureTest).FullName,
				null);

			Process p = this.createProcess(arguments);
			int resultCode = executeProcess(p);
			Assert.AreEqual(1, resultCode);
		}

		[Test]
		public void SuccessFixture() 
		{
			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(NUnit.Tests.Core.SuccessTest).FullName, 
				null);
			Process p = this.createProcess(arguments);
			int resultCode = executeProcess(p);
			Assert.AreEqual(0, resultCode);
		}

		[Test]
		public void XmlResult() 
		{
			FileInfo info = new FileInfo(xmlFile);
			info.Delete();

			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(NUnit.Tests.Core.SuccessTest).FullName,
				info.FullName);
			Process p = this.createProcess(arguments);
			int resultCode = executeProcess(p);
			Assert.AreEqual(0, resultCode);
			Assert.AreEqual(true, info.Exists);
		}

		[Test]
		public void InvalidFixture()
		{
			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				"NUnit.Tests.BogusTest", 
				null);
			Process p = this.createProcess(arguments);
			int resultCode = executeProcess(p);
			Assert.AreEqual(2, resultCode);
		}

		[Test]
		public void InvalidAssembly()
		{
			string[] arguments = MakeCommandLine("badassembly.dll", 
				null, 
				null);
			Process p = this.createProcess(arguments);
			int resultCode = executeProcess(p);
			Assert.AreEqual(2, resultCode);
		}

		[Test]
		public void XmlToConsole() 
		{
			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(NUnit.Tests.Core.SuccessTest).FullName, 
				null);
			ArrayList args = new ArrayList(arguments.Length + 2);
			foreach( string arg in arguments) 
			{
				args.Add(arg);
			}
			args.Add("/xmlconsole");
			args.Add("/nologo");
			Process p = this.createProcess((string[])args.ToArray(typeof(string)));
			StringBuilder builder = new StringBuilder();
			int resultCode = executeProcess(p, builder);
			Assert.AreEqual(0, resultCode);
			Assert.IsTrue(builder.ToString().Trim().IndexOf( @"<?xml version=""1.0""" ) >= 0,
				"Only XML should be displayed in xmlconsole mode");
		}

		private string[] MakeCommandLine(string assembly, string fixture, string xmlFile)
		{
			ArrayList list = new ArrayList();
			list.Add(String.Format("\"{0}\"", assembly));
			if(fixture != null)
				list.Add(String.Format("/fixture:{0}", fixture));
			if(xmlFile != null)
				list.Add(String.Format("\"/xml:{0}\"", xmlFile));

			int index = 0;
			string[] result = new string[list.Count];
			foreach(string arg in list)
			{
				result[index] = arg;
				index = index + 1;
			}
			return result;
		}

		private Process createProcess(string[] arguments) 
		{
			Process p = new Process();
			p.StartInfo.Arguments = String.Join(" ", arguments);
			p.StartInfo.FileName = nunitExe;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
			return p;
		}

		private int executeProcess(Process p) 
		{
			return executeProcess(p, new StringBuilder());
		}

		private int executeProcess(Process p, StringBuilder builder) 
		{
			p.Start();
			StreamReader stdOut = p.StandardOutput;
			string output = stdOut.ReadToEnd();
			builder.Append(output);
			p.WaitForExit();
			return p.ExitCode;
		}
	}
}
