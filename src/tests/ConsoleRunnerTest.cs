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
			RunConsoleTest(arguments, 1);
		}

		[Test]
		public void SuccessFixture() 
		{
			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(NUnit.Tests.SuccessTest).FullName, 
				null);
			RunConsoleTest(arguments, 0);
		}

		[Test]
		public void XmlResult() 
		{
			FileInfo info = new FileInfo(xmlFile);
			info.Delete();

			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(NUnit.Tests.SuccessTest).FullName,
				info.FullName);
			RunConsoleTest(arguments, 0);
			Assertion.AssertEquals(true, info.Exists);
		}

		[Test]
		public void InvalidFixture()
		{
			string[] arguments = MakeCommandLine(GetType().Module.Name, 
				"NUnit.Tests.BogusTest", 
				null);
			RunConsoleTest(arguments, 2);
		}

		private void RunConsoleTest(string[] arguments, int expected)
		{
			AppDomain domain = AppDomain.CreateDomain("test domain");

			Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
			Evidence evidence = new Evidence(baseEvidence);
		
			int resultCode = domain.ExecuteAssembly(nunitExe, evidence, arguments);

			AppDomain.Unload( domain );

			Assertion.AssertEquals(expected, resultCode);
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
