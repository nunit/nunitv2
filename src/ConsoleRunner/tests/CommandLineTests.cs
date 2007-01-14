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

namespace NUnit.ConsoleRunner.Tests
{
	using System;
	using System.IO;
	using System.Reflection;
	using NUnit.Framework;

	[TestFixture]
	public class CommandLineTests
	{
		[Test]
		public void NoParametersCount()
		{
			ConsoleOptions options = new ConsoleOptions();
			Assert.IsTrue(options.NoArgs);
		}

		[Test]
		public void AllowForwardSlashDefaultsCorrectly()
		{
			ConsoleOptions options = new ConsoleOptions();
			Assert.AreEqual( Path.DirectorySeparatorChar != '/', options.AllowForwardSlash );
		}

		private void TestBooleanOption( string fieldName )
		{
			TestBooleanOption( fieldName, fieldName );
		}

		private void TestBooleanOption( string fieldName, string option )
		{
			FieldInfo field = typeof(ConsoleOptions).GetField( fieldName );
			Assert.IsNotNull( field, "Field '{0}' not found", fieldName );
			Assert.AreEqual( typeof(bool), field.FieldType, "Field '{0}' is wrong type", fieldName );

			ConsoleOptions options = new ConsoleOptions( "-" + option );
			Assert.AreEqual( true, (bool)field.GetValue( options ), "Didn't recognize -" + option );
			options = new ConsoleOptions( "--" + option );
			Assert.AreEqual( true, (bool)field.GetValue( options ), "Didn't recognize --" + option );
			options = new ConsoleOptions( false, "/" + option );
			Assert.AreEqual( false, (bool)field.GetValue( options ), "Incorrectly recognized /" + option );
			options = new ConsoleOptions( true, "/" + option );
			Assert.AreEqual( true, (bool)field.GetValue( options ), "Didn't recognize /" + option );
		}

		private void TestStringOption( string fieldName )
		{
			TestStringOption( fieldName, fieldName );
		}

		private void TestStringOption( string fieldName, string option )
		{
			FieldInfo field = typeof(ConsoleOptions).GetField( fieldName );
			Assert.IsNotNull( field, "Field {0} not found", fieldName );
			Assert.AreEqual( typeof(string), field.FieldType );

			ConsoleOptions options = new ConsoleOptions( "-" + option + ":text" );
			Assert.AreEqual( "text", (string)field.GetValue( options ), "Didn't recognize -" + option );
			options = new ConsoleOptions( "--" + option + ":text" );
			Assert.AreEqual( "text", (string)field.GetValue( options ), "Didn't recognize --" + option );
			options = new ConsoleOptions( false, "/" + option + ":text" );
			Assert.AreEqual( null, (string)field.GetValue( options ), "Incorrectly recognized /" + option );
			options = new ConsoleOptions( true, "/" + option + ":text" );
			Assert.AreEqual( "text", (string)field.GetValue( options ), "Didn't recognize /" + option );
		}

		private void TestEnumOption( string fieldName )
		{
			FieldInfo field = typeof(ConsoleOptions).GetField( fieldName );
			Assert.IsNotNull( field, "Field {0} not found", fieldName );
			Assert.IsTrue( field.FieldType.IsEnum, "Field {0} is not an enum", fieldName );
		}

		[Test]
		public void OptionsAreRecognized()
		{
			TestBooleanOption( "nologo" );
			TestBooleanOption( "help" );
			TestBooleanOption( "help", "?" );
			TestBooleanOption( "wait" );
			TestBooleanOption( "xmlConsole" );
			TestBooleanOption( "labels" );
			TestBooleanOption( "noshadow" );
			TestBooleanOption( "nothread" );
			TestStringOption( "fixture" );
			TestStringOption( "config" );
			TestStringOption( "xml" );
			TestStringOption( "transform" );
			TestStringOption( "output" );
			TestStringOption( "output", "out" );
			TestStringOption( "err" );
			TestStringOption( "include" );
			TestStringOption( "exclude" );
			TestEnumOption( "domain" );
		}

		[Test]
		public void AssemblyName()
		{
			ConsoleOptions options = new ConsoleOptions( "nunit.tests.dll" );
			Assert.AreEqual( "nunit.tests.dll", options.Parameters[0] );
		}

		[Test]
		public void IncludeCategories() 
		{
			ConsoleOptions options = new ConsoleOptions( "tests.dll", "-include:Database;Slow" );
			Assert.IsTrue( options.Validate() );
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
			ConsoleOptions options = new ConsoleOptions( "tests.dll", "-exclude:Database;Slow" );
			Assert.IsTrue( options.Validate() );
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
			ConsoleOptions options = new ConsoleOptions( "tests.dll", "-include:Database;Slow", "-exclude:Fast" );
			Assert.IsFalse( options.Validate() );
		}

		[Test]
		public void FixtureNamePlusAssemblyIsValid()
		{
			ConsoleOptions options = new ConsoleOptions( "-fixture:NUnit.Tests.AllTests", "nunit.tests.dll" );
			Assert.AreEqual("nunit.tests.dll", options.Parameters[0]);
			Assert.AreEqual("NUnit.Tests.AllTests", options.fixture);
			Assert.IsTrue(options.Validate());
		}

		[Test]
		public void AssemblyAloneIsValid()
		{
			ConsoleOptions options = new ConsoleOptions( "nunit.tests.dll" );
			Assert.IsTrue(options.Validate(), "command line should be valid");
		}

		[Test]
		public void InvalidOption()
		{
			ConsoleOptions options = new ConsoleOptions( "-asembly:nunit.tests.dll" );
			Assert.IsFalse(options.Validate());
		}


		[Test]
		public void NoFixtureNameProvided()
		{
			ConsoleOptions options = new ConsoleOptions( "-fixture:", "nunit.tests.dll" );
			Assert.IsFalse(options.Validate());
		}

		[Test] 
		public void InvalidCommandLineParms()
		{
			ConsoleOptions options = new ConsoleOptions( "-garbage:TestFixture", "-assembly:Tests.dll" );
			Assert.IsFalse(options.Validate());
		}

		[Test]
		public void XmlParameter()
		{
			ConsoleOptions options = new ConsoleOptions( "tests.dll", "-xml:results.xml" );
			Assert.IsTrue(options.ParameterCount == 1, "assembly should be set");
			Assert.AreEqual("tests.dll", options.Parameters[0]);

			Assert.IsTrue(options.IsXml, "XML file name should be set");
			Assert.AreEqual("results.xml", options.xml);
		}

		[Test]
		public void XmlParameterWithFullPath()
		{
			ConsoleOptions options = new ConsoleOptions( "tests.dll", "-xml:C:/nunit/tests/bin/Debug/console-test.xml" );
			Assert.IsTrue(options.ParameterCount == 1, "assembly should be set");
			Assert.AreEqual("tests.dll", options.Parameters[0]);

			Assert.IsTrue(options.IsXml, "XML file name should be set");
			Assert.AreEqual("C:/nunit/tests/bin/Debug/console-test.xml", options.xml);
		}

		[Test]
		public void XmlParameterWithFullPathUsingEqualSign()
		{
			ConsoleOptions options = new ConsoleOptions( "tests.dll", "-xml=C:/nunit/tests/bin/Debug/console-test.xml" );
			Assert.IsTrue(options.ParameterCount == 1, "assembly should be set");
			Assert.AreEqual("tests.dll", options.Parameters[0]);

			Assert.IsTrue(options.IsXml, "XML file name should be set");
			Assert.AreEqual("C:/nunit/tests/bin/Debug/console-test.xml", options.xml);
		}

		[Test]
		public void TransformParameter()
		{
			ConsoleOptions options = new ConsoleOptions( "tests.dll", "-transform:Summary.xslt" );
			Assert.IsTrue(options.ParameterCount == 1, "assembly should be set");
			Assert.AreEqual("tests.dll", options.Parameters[0]);

			Assert.IsTrue(options.IsTransform, "transform file name should be set");
			Assert.AreEqual("Summary.xslt", options.transform);
		}


		[Test]
		public void FileNameWithoutXmlParameterIsInvalid()
		{
			ConsoleOptions options = new ConsoleOptions( "tests.dll", ":result.xml" );
			Assert.IsFalse(options.IsXml);
		}

		[Test]
		public void XmlParameterWithoutFileNameIsInvalid()
		{
			ConsoleOptions options = new ConsoleOptions( "tests.dll", "-xml:" );
			Assert.IsFalse(options.IsXml);			
		}

		[Test]
		public void HelpTextUsesCorrectDelimiterForPlatform()
		{
			string helpText = new ConsoleOptions().GetHelpText();
			char delim = System.IO.Path.DirectorySeparatorChar == '/' ? '-' : '/';

			string expected = string.Format( "{0}output=", delim );
			StringAssert.Contains( expected, helpText );
			
			expected = string.Format( "{0}out=", delim );
			StringAssert.Contains( expected, helpText );
		}
	}
}
