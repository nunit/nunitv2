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

namespace NUnit.Tests.CommandLine
{
	using System;
	using NUnit.Framework;
	using NUnit.Util;
	using Codeblast;

	[TestFixture]
	public class ConsoleFixture
	{
		[Test]
		public void NoParametersCount()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {});
			Assert.IsTrue(options.NoArgs);
		}

		[Test]
		public void NoLogo()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/nologo"});
			Assert.IsTrue(options.nologo);
		}

		[Test]
		public void Help()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/help"});
			Assert.IsTrue(options.help);
		}

		[Test]
		public void ShortHelp()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/?"});
			Assert.IsTrue(options.help);
		}

		[Test]
		public void Wait()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/wait"});
			Assert.IsTrue(options.wait);
		}

		[Test]
		public void AssemblyName()
		{
			string assemblyName = "nunit.tests.dll";
			ConsoleOptions options = new ConsoleOptions(new string[] 
              { assemblyName });
			Assert.AreEqual(assemblyName, options.Parameters[0]);
		}

		[Test]
		public void IncludeCategories() 
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/include:Database;Slow"});
//			Assert.IsTrue( options.Validate() );
			Assert.IsNotNull(options.include);
			Assert.AreEqual(options.include, "Database;Slow");
			Assert.IsTrue(options.HasInclude);
			string[] categories = options.IncludedCategories;
			Assert.IsNotNull(categories);
			Assert.AreEqual(2, categories.Length);
			Assert.AreEqual("Database", categories[0]);
			Assert.AreEqual("Slow", categories[1]);
		}

		[Test]
		public void ExcludeCategories() 
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/exclude:Database;Slow"});
//			Assert.IsTrue( options.Validate() );
			Assert.IsNotNull(options.exclude);
			Assert.AreEqual(options.exclude, "Database;Slow");
			Assert.IsTrue(options.HasExclude);
			string[] categories = options.ExcludedCategories;
			Assert.IsNotNull(categories);
			Assert.AreEqual(2, categories.Length);
			Assert.AreEqual("Database", categories[0]);
			Assert.AreEqual("Slow", categories[1]);
		}

		[Test]
		public void IncludeAndExcludeAreInvalidTogether()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] {"/include:Database;Slow /exclude:Fast"});
			Assert.IsFalse( options.Validate() );
		}

		[Test]
		public void FixtureName()
		{
			string assemblyName = "nunit.tests.dll";
			string fixtureName = "NUnit.Tests.AllTests";
			ConsoleOptions options = new ConsoleOptions(new string[] 
			  { "/fixture:" + fixtureName, 
				 assemblyName });
			Assert.AreEqual(assemblyName, options.Parameters[0]);
			Assert.AreEqual(fixtureName, options.fixture);
			Assert.IsTrue(options.Validate());
		}

		[Test]
		public void ValidateSuccessful()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { "nunit.tests.dll" });
			Assert.IsTrue(options.Validate(), "command line should be valid");
		}

		[Test]
		public void InvalidArgs()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { "/asembly:nunit.tests.dll" });
			Assert.IsFalse(options.Validate());
		}


		[Test]
		public void NoFixtureName()
		{
			ConsoleOptions options = new ConsoleOptions(new string[] { 
				"/fixture:", "nunit.tests.dll",  });
			Assert.IsFalse(options.Validate());
		}

		[Test] 
		public void InvalidCommandLineParms()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"/garbage:TestFixture", "/assembly:Tests.dll"});
			Assert.IsFalse(parser.Validate());
		}

		[Test]
		public void XmlParameter()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/xml:results.xml"});
			Assert.IsTrue(parser.ParameterCount == 1, "assembly should be set");
			Assert.AreEqual("tests.dll", parser.Parameters[0]);

			Assert.IsTrue(parser.IsXml, "XML file name should be set");
			Assert.AreEqual("results.xml", parser.xml);
		}

		[Test]
		public void XmlParameterWithFullPath()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/xml:C:\\nunit\\tests\\bin\\Debug\\console-test.xml"});
			Assert.IsTrue(parser.ParameterCount == 1, "assembly should be set");
			Assert.AreEqual("tests.dll", parser.Parameters[0]);

			Assert.IsTrue(parser.IsXml, "XML file name should be set");
			Assert.AreEqual("C:\\nunit\\tests\\bin\\Debug\\console-test.xml", parser.xml);
		}

		[Test]
		public void XmlParameterWithFullPathUsingEqualSign()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/xml=C:\\nunit\\tests\\bin\\Debug\\console-test.xml"});
			Assert.IsTrue(parser.ParameterCount == 1, "assembly should be set");
			Assert.AreEqual("tests.dll", parser.Parameters[0]);

			Assert.IsTrue(parser.IsXml, "XML file name should be set");
			Assert.AreEqual("C:\\nunit\\tests\\bin\\Debug\\console-test.xml", parser.xml);
		}

		[Test]
		public void TransformParameter()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/transform:Summary.xslt"});
			Assert.IsTrue(parser.ParameterCount == 1, "assembly should be set");
			Assert.AreEqual("tests.dll", parser.Parameters[0]);

			Assert.IsTrue(parser.IsTransform, "transform file name should be set");
			Assert.AreEqual("Summary.xslt", parser.transform);
		}


		[Test]
		public void FileNameWithoutXmlParameter()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", ":result.xml"});
			Assert.IsFalse(parser.IsXml);
		}

		[Test]
		public void XmlParameterWithoutFileName()
		{
			ConsoleOptions parser = new ConsoleOptions(new String[]{"tests.dll", "/xml:"});
			Assert.IsFalse(parser.IsXml);			
		}
	}
}
