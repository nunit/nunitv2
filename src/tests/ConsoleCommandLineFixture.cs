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
			Assert.True(options.NoArgs);
		}

		[Test]
		public void NoLogo()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/nologo"});
			Assert.True(options.nologo);
		}

		[Test]
		public void Help()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/help"});
			Assert.True(options.help);
		}

		[Test]
		public void ShortHelp()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/?"});
			Assert.True(options.help);
		}

		[Test]
		public void Wait()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/wait"});
			Assert.True(options.wait);
		}

		[Test]
		public void AssemblyName()
		{
			string assemblyName = "nunit.tests.dll";
			ConsoleOptions options = new ConsoleOptions(new string[] 
              { assemblyName });
			Assert.Equals(assemblyName, options.Assembly);
		}

		[Test]
		public void FixtureName()
		{
			string assemblyName = "nunit.tests.dll";
			string fixtureName = "NUnit.Tests.AllTests";
			ConsoleOptions options = new ConsoleOptions(new string[] 
			  { "/fixture:" + fixtureName, 
				 assemblyName });
			Assert.Equals(assemblyName, options.Assembly);
			Assert.Equals(fixtureName, options.fixture);
			Assert.True(options.Validate());
		}

		[Test]
		public void ValidateSuccessful()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { "nunit.tests.dll" });
			Assert.True("command line should be valid", options.Validate());
		}

		[Test]
		public void InvalidArgs()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { "/asembly:nunit.tests.dll" });
			Assert.False(options.Validate());
		}


		[Test]
		public void NoFixtureName()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { 
				"/fixture:", "nunit.tests.dll",  });
			Assert.False(options.Validate());
		}

		[Test] 
		public void InvalidCommandLineParms()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"/garbage:TestFixture", "/assembly:Tests.dll"});
			Assert.False(parser.Validate());
		}

		[Test] 
		public void NoNameValuePairs()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"TestFixture", "Tests.dll"});
			Assert.False(parser.Validate());
		}

		[Test]
		public void XmlParameter()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/xml:results.xml"});
			Assert.True("assembly should be set", parser.ParameterCount == 1);
			Assert.Equals("tests.dll", parser.Parameters[0]);

			Assert.True("xml file name should be set", parser.IsXml);
			Assert.Equals("results.xml", parser.xml);
		}

		[Test]
		public void XmlParameterWithFullPath()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/xml:C:\\nunit\\tests\\bin\\Debug\\console-test.xml"});
			Assert.True("assembly should be set", parser.ParameterCount == 1);
			Assert.Equals("tests.dll", parser.Parameters[0]);

			Assert.True("xml file name should be set", parser.IsXml);
			Assert.Equals("C:\\nunit\\tests\\bin\\Debug\\console-test.xml", parser.xml);
		}

		[Test]
		public void TransformParameter()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/transform:Summary.xslt"});
			Assert.True("assembly should be set", parser.ParameterCount == 1);
			Assert.Equals("tests.dll", parser.Parameters[0]);

			Assert.True("transform file name should be set", parser.IsTransform);
			Assert.Equals("Summary.xslt", parser.transform);
		}


		[Test]
		public void FileNameWithoutXmlParameter()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", ":result.xml"});
			Assert.False(parser.IsXml);
		}

		[Test]
		public void XmlParameterWithoutFileName()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/xml:"});
			Assert.False(parser.IsXml);			
		}
	}
}
