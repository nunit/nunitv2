namespace Nunit.Tests 
{
	using System;
	using System.IO;
	using System.Diagnostics;
	using System.Reflection;
	using System.Text;

	using Nunit.Framework;
/// <summary>
/// 
/// </summary>
/// 
	[TestFixture]
	public class ConsoleRunnerTest 
	{
		private String nunitExe;

		public ConsoleRunnerTest() 
		{
			// need a better way to make this location independant
			FileInfo file = new FileInfo("nunit-console.exe");
			if(file.Exists)
				nunitExe = file.FullName;
			else
				nunitExe = "..\\..\\..\\nunit-console\\bin\\Debug\\nunit-console.exe";
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
			string arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(Nunit.Tests.ConsoleRunnerTest.FailureTest).FullName,
				null);
			RunConsoleTest(arguments, 1);
		}

		[Test]
		public void SuccessFixture() 
		{
			string arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(Nunit.Tests.SuccessTest).FullName, 
				null);
			RunConsoleTest(arguments, 0);
		}

		[Test]
		public void XmlResult() 
		{
			FileInfo info = new FileInfo("console-test.xml");
			info.Delete();

			string arguments = MakeCommandLine(GetType().Module.Name, 
				typeof(Nunit.Tests.SuccessTest).FullName,
				info.FullName);
			RunConsoleTest(arguments, 0);
			Assertion.AssertEquals(true, info.Exists);
		}

		[Test]
		public void InvalidFixture()
		{
			string arguments = MakeCommandLine(GetType().Module.Name, 
				"Nunit.Tests.BogusTest", 
				null);
			RunConsoleTest(arguments, 2);
		}

		private void RunConsoleTest(string arguments, int expected)
		{
			Process p = new Process();
			p.StartInfo.FileName = nunitExe;
			p.StartInfo.Arguments = arguments;
			p.Start();

			p.WaitForExit();
			Assertion.AssertEquals(expected, p.ExitCode);
		}

		private string MakeCommandLine(string assembly, string fixture, string xmlFile)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("/assembly:{0}", assembly);
			if(fixture != null)
				builder.AppendFormat(" /fixture:{0}", fixture);

			if(xmlFile != null)
				builder.AppendFormat(" /xml:\"{0}\"", xmlFile);

			return builder.ToString();
		}
	}
}
