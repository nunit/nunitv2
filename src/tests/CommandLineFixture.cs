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
	using System.Collections;
	using NUnit.Framework;
	using NUnit.Util;

	[TestFixture]
	public class CommandLineFixture
	{
		[Test] 
		public void NoArgs()
		{
			CommandLineParser parser = new CommandLineParser(new string[]{});
			Assertion.Assert("should have no arguments", parser.NoArgs);
		}

		[Test] public void AssemblyTest()
		{
			CommandLineParser parser = new CommandLineParser(new String[]{"/assembly:tests.dll"});
			Assertion.Assert("assembly should be set", parser.IsAssembly);
			Assertion.AssertEquals("tests.dll", parser.AssemblyName);
		}

		[Test] public void TestAndAssemblyTest()
		{
			CommandLineParser parser = new CommandLineParser(new String[]{"/fixture:TestFixture","/assembly:tests.dll"});
			Assertion.Assert("not just an assembly", !parser.IsAssembly);
			Assertion.AssertEquals("tests.dll", parser.AssemblyName);

			Assertion.Assert("should have fixture and assembly", parser.IsFixture);
			Assertion.AssertEquals("TestFixture", parser.TestName);
		}

		[Test] 
		[ExpectedException(typeof(CommandLineException))]
		public void TestOnlyError()
		{
			CommandLineParser parser = new CommandLineParser(new String[]{"/fixture:TestFixture"});
		}

		[Test] 
		[ExpectedException(typeof(CommandLineException))]
		public void InvalidCommandLineParms()
		{
			CommandLineParser parser = new CommandLineParser(new String[]{"/garbage:TestFixture", "/assembly:Tests.dll"});
		}

		[Test] 
		[ExpectedException(typeof(CommandLineException))]
		public void NoNameValuePairs()
		{
			CommandLineParser parser = new CommandLineParser(new String[]{"TestFixture", "Tests.dll"});
		}

		[Test]
		public void XmlParameter()
		{
			CommandLineParser parser = new CommandLineParser(new String[]{"/assembly:tests.dll", "/xml:results.xml"});
			Assertion.Assert("assembly should be set", parser.IsAssembly);
			Assertion.AssertEquals("tests.dll", parser.AssemblyName);

			Assertion.Assert("xml file name should be set", parser.IsXml);
			Assertion.AssertEquals("results.xml", parser.XmlFileName);
		}

		[Test]
		public void XmlParameterWithFullPath()
		{
			CommandLineParser parser = new CommandLineParser(new String[]{"/assembly:tests.dll", "/xml:C:\\nunit\\tests\\bin\\Debug\\console-test.xml"});
			Assertion.Assert("assembly should be set", parser.IsAssembly);
			Assertion.AssertEquals("tests.dll", parser.AssemblyName);

			Assertion.Assert("xml file name should be set", parser.IsXml);
			Assertion.AssertEquals("C:\\nunit\\tests\\bin\\Debug\\console-test.xml", parser.XmlFileName);
		}

		[Test]
		public void TransformParameter()
		{
			CommandLineParser parser = new CommandLineParser(new String[]{"/assembly:tests.dll", "/transform:Summary.xslt"});
			Assertion.Assert("assembly should be set", parser.IsAssembly);
			Assertion.AssertEquals("tests.dll", parser.AssemblyName);

			Assertion.Assert("transform file name should be set", parser.IsTransform);
			Assertion.AssertEquals("Summary.xslt", parser.TransformFileName);
		}


		[Test]
		[ExpectedException(typeof(CommandLineException))]
		public void FileNameWithoutXmlParameter()
		{
			CommandLineParser parser = new CommandLineParser(new String[]{"/assembly:tests.dll", ":result.xml"});
		}

		[Test]
		[ExpectedException(typeof(CommandLineException))]
		public void XmlParameterWithoutFileName()
		{
			CommandLineParser parser = new CommandLineParser(new String[]{"/assembly:tests.dll", "/xml:"});
		}

		[Test]
		public void AllowedParemeters()
		{
			ArrayList allowedParameters = new ArrayList();
			allowedParameters.Add(CommandLineParser.ASSEMBLY_PARM);

			CommandLineParser parser = new CommandLineParser(allowedParameters, new String[]{"/assembly:tests.dll"});
			Assertion.Assert("assembly should be set", parser.IsAssembly);
			Assertion.AssertEquals("tests.dll", parser.AssemblyName);
		}
	}
}