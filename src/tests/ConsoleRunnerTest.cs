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
	using System.Diagnostics;
	using System.Reflection;
	using System.Security.Policy;
	using System.Text;
	using System.Collections;

	using NUnit.Framework;

	[TestFixture]
	public class ConsoleRunnerTest 
	{
		private String nunitExe;
		private static readonly string xmlFile = "console-test.xml";
		private AppDomain domain = null;
		private Evidence evidence = null;

		public ConsoleRunnerTest() 
		{
			// need a better way to make this location independant
			FileInfo file = new FileInfo("nunit-console.exe");
			if(file.Exists)
				nunitExe = file.FullName;
			else
#if DEBUG
				nunitExe = "..\\..\\..\\nunit-console\\bin\\Debug\\nunit-console.exe";
#else
				nunitExe = "..\\..\\..\\nunit-console\\bin\\Release\\nunit-console.exe";
#endif
		}

		[SetUp]
		public void Setup()
		{
			domain = AppDomain.CreateDomain( "test domain" );
			Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
			evidence = new Evidence( baseEvidence );
		}

		[TearDown]
		public void CleanUp()
		{
			FileInfo file = new FileInfo(xmlFile);
			if(file.Exists)
				file.Delete();

			file = new FileInfo("TestResult.xml");
			if(file.Exists) file.Delete();
			else
			{
				file = new FileInfo(@"..\..\..\nunit-console\bin\Debug\TestResult.xml");
				if(file.Exists) file.Delete();
			}

			if ( domain != null )
				AppDomain.Unload( domain );
		}

		[TestFixture] internal class FailureTest
		{
			[Test]
			public void Fail()
			{
				Assertion.Fail();
			}
		}

		[Test]
		public void FailureFixture() 
		{
			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(NUnit.Tests.ConsoleRunnerTest.FailureTest).FullName,
				null);
			int resultCode = domain.ExecuteAssembly(nunitExe, evidence, arguments);
			Assertion.AssertEquals(1, resultCode);
		}

		[Test]
		public void SuccessFixture() 
		{
			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(NUnit.Tests.SuccessTest).FullName, 
				null);
			int resultCode = domain.ExecuteAssembly(nunitExe, evidence, arguments);
			Assertion.AssertEquals(0, resultCode);
		}

		[Test]
		public void XmlResult() 
		{
			FileInfo info = new FileInfo(xmlFile);
			info.Delete();

			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(NUnit.Tests.SuccessTest).FullName,
				info.FullName);
			int resultCode = domain.ExecuteAssembly(nunitExe, evidence, arguments);
			Assertion.AssertEquals(0, resultCode);
			Assertion.AssertEquals(true, info.Exists);
		}

		[Test]
		public void InvalidFixture()
		{
			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				"NUnit.Tests.BogusTest", 
				null);
			int resultCode = domain.ExecuteAssembly(nunitExe, evidence, arguments);
			Assertion.AssertEquals(2, resultCode);
		}

		private string[] MakeCommandLine(string assembly, string fixture, string xmlFile)
		{
			ArrayList list = new ArrayList();
			list.Add(String.Format("/assembly:{0}", assembly));
			if(fixture != null)
				list.Add(String.Format("/fixture:{0}", fixture));
			if(xmlFile != null)
				list.Add(String.Format("/xml:{0}", xmlFile));

			int index = 0;
			string[] result = new string[list.Count];
			foreach(string arg in list)
			{
				result[index] = arg;
				index = index + 1;
			}
			return result;
		}
	}
}
