namespace NUnit.Tests.CommandLine
{
	using System;
	using NUnit.Framework;
	using NUnit.Util;
	using Codeblast;

	[TestFixture]
	public class GuiFixture
	{
		[Test]
		public void NoParametersCount()
		{
			GuiOptions options = new GuiOptions(new string[] {});
			Assert.True(options.NoArgs);
		}

		[Test]
		public void Help()
		{
			GuiOptions options = new GuiOptions(new string[] {"/help"});
			Assert.True(options.help);
		}

		[Test]
		public void ShortHelp()
		{
			GuiOptions options = new GuiOptions(new string[] {"/?"});
			Assert.True(options.help);
		}

		[Test]
		public void AssemblyName()
		{
			string assemblyName = "nunit.tests.dll";
			GuiOptions options = new GuiOptions(new string[]{ assemblyName });
			Assert.Equals(assemblyName, options.Assembly);
		}

		[Test]
		public void ValidateSuccessful()
		{
			GuiOptions options = new GuiOptions(new string[] { "nunit.tests.dll" });
			Assert.True("command line should be valid", options.Validate());
		}

		[Test]
		public void InvalidArgs()
		{
			GuiOptions options = new GuiOptions(new string[] { "/asembly:nunit.tests.dll" });
			Assert.False(options.Validate());
		}


		[Test] 
		public void InvalidCommandLineParms()
		{
			GuiOptions parser = new GuiOptions(new String[]{"/garbage:TestFixture", "/assembly:Tests.dll"});
			Assert.False(parser.Validate());
		}

		[Test] 
		public void NoNameValuePairs()
		{
			GuiOptions parser = new GuiOptions(new String[]{"TestFixture", "Tests.dll"});
			Assert.False(parser.Validate());
		}
	}
}

