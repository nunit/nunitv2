namespace NUnit.Tests
{
	using System;
	using NUnit.Framework;
	using NUnit.Util;
	using Codeblast;

	[TestFixture]
	public class ConsoleCommandLineFixture
	{
		[Test]
		public void NoParametersCount()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {});
			Assertion.Assert(options.NoArgs);
		}

		[Test]
		public void NoLogo()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/nologo"});
			Assertion.Assert(options.nologo);
		}

		[Test]
		public void Help()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/help"});
			Assertion.Assert(options.help);
		}

		[Test]
		public void ShortHelp()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/?"});
			Assertion.Assert(options.help);
		}

		[Test]
		public void Wait()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/wait"});
			Assertion.Assert(options.wait);
		}

		[Test]
		public void NoParametersNullCheck()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {});
			Assertion.AssertNull(options.assembly);
		}

		[Test]
		public void AssemblyName()
		{
			string assemblyName = "nunit.tests.dll";
			ConsoleOptions options = new ConsoleOptions(new string[] 
              { "/assembly:" + assemblyName });
			Assertion.AssertEquals(assemblyName, options.assembly);
		}

		[Test]
		public void FixtureName()
		{
			string assemblyName = "nunit.tests.dll";
			string fixtureName = "NUnit.Tests.AllTests";
			ConsoleOptions options = new ConsoleOptions(new string[] 
			  { "/assembly:" + assemblyName,
			    "/fixture:" + fixtureName });
			Assertion.AssertEquals(assemblyName, options.assembly);
			Assertion.AssertEquals(fixtureName, options.fixture);
			Assertion.Assert(options.Validate());
		}

		[Test]
		public void ValidateSuccessful()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { "/assembly:nunit.tests.dll" });
			Assertion.Assert("command line should be valid", options.Validate());
		}

		[Test]
		public void InvalidArgs()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { "/asembly:nunit.tests.dll" });
			Assertion.Assert(!options.Validate());
		}

		[Test]
		public void NoAssemblyName()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { "/assembly:" });
			Assertion.Assert(!options.Validate());
		}

		[Test]
		public void NoFixtureName()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { 
				"/assembly:nunit.tests.dll", "/fixture:" });
			Assertion.Assert(!options.Validate());
		}

		[Test] 
		public void InvalidCommandLineParms()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"/garbage:TestFixture", "/assembly:Tests.dll"});
			Assertion.AssertEquals(false, parser.Validate());
		}

		[Test] 
		public void NoNameValuePairs()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"TestFixture", "Tests.dll"});
			Assertion.Assert(!parser.Validate());
		}

		[Test]
		public void XmlParameter()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"/assembly:tests.dll", "/xml:results.xml"});
			Assertion.Assert("assembly should be set", parser.IsAssembly);
			Assertion.AssertEquals("tests.dll", parser.assembly);

			Assertion.Assert("xml file name should be set", parser.IsXml);
			Assertion.AssertEquals("results.xml", parser.xml);
		}

		[Test]
		public void XmlParameterWithFullPath()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"/assembly:tests.dll", "/xml:C:\\nunit\\tests\\bin\\Debug\\console-test.xml"});
			Assertion.Assert("assembly should be set", parser.IsAssembly);
			Assertion.AssertEquals("tests.dll", parser.assembly);

			Assertion.Assert("xml file name should be set", parser.IsXml);
			Assertion.AssertEquals("C:\\nunit\\tests\\bin\\Debug\\console-test.xml", parser.xml);
		}

		[Test]
		public void TransformParameter()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"/assembly:tests.dll", "/transform:Summary.xslt"});
			Assertion.Assert("assembly should be set", parser.IsAssembly);
			Assertion.AssertEquals("tests.dll", parser.assembly);

			Assertion.Assert("transform file name should be set", parser.IsTransform);
			Assertion.AssertEquals("Summary.xslt", parser.transform);
		}


		[Test]
		public void FileNameWithoutXmlParameter()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"/assembly:tests.dll", ":result.xml"});
			Assertion.Assert(!parser.IsXml);
		}

		[Test]
		public void XmlParameterWithoutFileName()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"/assembly:tests.dll", "/xml:"});
			Assertion.Assert(!parser.IsXml);			
		}
	}
}
