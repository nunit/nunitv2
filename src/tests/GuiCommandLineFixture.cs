namespace NUnit.Tests
{
	using System;
	using NUnit.Framework;
	using NUnit.Util;
	using Codeblast;

	[TestFixture]
	public class GuiCommandLineFixture
	{
		[Test]
		public void NoParametersCount()
		{
			GuiOptions options = new GuiOptions(new string[] {});
			Assertion.Assert(options.NoArgs);
		}

		[Test]
		public void Help()
		{
			GuiOptions options = new GuiOptions(new string[] {"/help"});
			Assertion.Assert(options.help);
		}

		[Test]
		public void ShortHelp()
		{
			GuiOptions options = new GuiOptions(new string[] {"/?"});
			Assertion.Assert(options.help);
		}

		[Test]
		public void AssemblyName()
		{
			string assemblyName = "nunit.tests.dll";
			GuiOptions options = new GuiOptions(new string[]{ assemblyName });
			Assertion.AssertEquals(assemblyName, options.Assembly);
		}

		[Test]
		public void ValidateSuccessful()
		{
			GuiOptions options = new GuiOptions(new string[] { "nunit.tests.dll" });
			Assertion.Assert("command line should be valid", options.Validate());
		}

		[Test]
		public void InvalidArgs()
		{
			GuiOptions options = new GuiOptions(new string[] { "/asembly:nunit.tests.dll" });
			Assertion.Assert(!options.Validate());
		}


		[Test] 
		public void InvalidCommandLineParms()
		{
			GuiOptions parser = new GuiOptions(new String[]{"/garbage:TestFixture", "/assembly:Tests.dll"});
			Assertion.AssertEquals(false, parser.Validate());
		}

		[Test] 
		public void NoNameValuePairs()
		{
			GuiOptions parser = new GuiOptions(new String[]{"TestFixture", "Tests.dll"});
			Assertion.Assert(!parser.Validate());
		}
	}
}

