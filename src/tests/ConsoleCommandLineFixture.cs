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
		public void AssemblyName()
		{
			string assemblyName = "nunit.tests.dll";
			ConsoleOptions options = new ConsoleOptions(new string[] 
              { assemblyName });
			Assertion.AssertEquals(assemblyName, options.Assembly);
		}

		[Test]
		public void FixtureName()
		{
			string assemblyName = "nunit.tests.dll";
			string fixtureName = "NUnit.Tests.AllTests";
			ConsoleOptions options = new ConsoleOptions(new string[] 
			  { "/fixture:" + fixtureName, 
				 assemblyName });
			Assertion.AssertEquals(assemblyName, options.Assembly);
			Assertion.AssertEquals(fixtureName, options.fixture);
			Assertion.Assert(options.Validate());
		}

		[Test]
		public void ValidateSuccessful()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { "nunit.tests.dll" });
			Assertion.Assert("command line should be valid", options.Validate());
		}

		[Test]
		public void InvalidArgs()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { "/asembly:nunit.tests.dll" });
			Assertion.Assert(!options.Validate());
		}


		[Test]
		public void NoFixtureName()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { 
				"/fixture:", "nunit.tests.dll",  });
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
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/xml:results.xml"});
			Assertion.Assert("assembly should be set", parser.ParameterCount == 1);
			Assertion.AssertEquals("tests.dll", parser.Parameters[0]);

			Assertion.Assert("xml file name should be set", parser.IsXml);
			Assertion.AssertEquals("results.xml", parser.xml);
		}

		[Test]
		public void XmlParameterWithFullPath()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/xml:C:\\nunit\\tests\\bin\\Debug\\console-test.xml"});
			Assertion.Assert("assembly should be set", parser.ParameterCount == 1);
			Assertion.AssertEquals("tests.dll", parser.Parameters[0]);

			Assertion.Assert("xml file name should be set", parser.IsXml);
			Assertion.AssertEquals("C:\\nunit\\tests\\bin\\Debug\\console-test.xml", parser.xml);
		}

		[Test]
		public void TransformParameter()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/transform:Summary.xslt"});
			Assertion.Assert("assembly should be set", parser.ParameterCount == 1);
			Assertion.AssertEquals("tests.dll", parser.Parameters[0]);

			Assertion.Assert("transform file name should be set", parser.IsTransform);
			Assertion.AssertEquals("Summary.xslt", parser.transform);
		}


		[Test]
		public void FileNameWithoutXmlParameter()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", ":result.xml"});
			Assertion.Assert(!parser.IsXml);
		}

		[Test]
		public void XmlParameterWithoutFileName()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/xml:"});
			Assertion.Assert(!parser.IsXml);			
		}
	}
}
