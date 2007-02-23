// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;	
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using NUnit.Util;
using NUnit.TestUtilities;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for XmlTest.
	/// </summary>
	/// 
	[TestFixture]
	public class XmlTest
	{
		public class SchemaValidator
		{
			private XmlValidatingReader myXmlValidatingReader;
			private bool success;

			public SchemaValidator(string xmlFile, string schemaFile)
			{
				XmlSchemaCollection myXmlSchemaCollection = new XmlSchemaCollection();
				XmlTextReader xmlTextReader = new XmlTextReader(schemaFile);
				try
				{
					myXmlSchemaCollection.Add(XmlSchema.Read(xmlTextReader, null));
				}
				finally
				{
					xmlTextReader.Close();
				}

				// Validate the XML file with the schema
				XmlTextReader myXmlTextReader = new XmlTextReader (xmlFile);
				myXmlValidatingReader = new XmlValidatingReader(myXmlTextReader);
				myXmlValidatingReader.Schemas.Add(myXmlSchemaCollection);
				myXmlValidatingReader.ValidationType = ValidationType.Schema;
			}

			public bool Validate()
			{
				success = true;

				try
				{
					// Set the validation event handler
					myXmlValidatingReader.ValidationEventHandler += new ValidationEventHandler (this.ValidationEventHandle);

					// Read XML data
					while (myXmlValidatingReader.Read()){}
				}
				catch (Exception e)
				{
					throw new NUnitException(e.Message, e);
				}
				finally
				{
					myXmlValidatingReader.Close();
				}

				return success;
			}

			public void ValidationEventHandle (object sender, ValidationEventArgs args)
			{
				success = false;
				Console.WriteLine("\tValidation error: " + args.Message);

				if (args.Severity == XmlSeverityType.Warning)
				{
					Console.WriteLine("No schema found to enforce validation.");
				} 
				else
					if (args.Severity == XmlSeverityType.Error)
				{
					Console.WriteLine("validation error occurred when validating the instance document.");
				} 

				if (args.Exception != null) // XSD schema validation error
				{
					Console.WriteLine(args.Exception.SourceUri + "," +  args.Exception.LinePosition + "," +  args.Exception.LineNumber);
				}
			}
		}

		private void runSchemaValidatorTest(string reportFileName, CultureInfo testCulture)
		{
			// Preserve current culture
			CultureInfo previousCulture = Thread.CurrentThread.CurrentCulture;

			// Enable test culture
			Thread.CurrentThread.CurrentCulture = testCulture;

			try
			{
				string testsDll = "mock-assembly.dll";
				TestSuiteBuilder builder = new TestSuiteBuilder();
				Test suite = builder.Build( new TestPackage( testsDll ) );

				TestResult result = suite.Run(NullListener.NULL);

			  XmlResultVisitor visitor = new XmlResultVisitor(reportFileName, result);
				result.Accept(visitor);
				visitor.Write();

				SchemaValidator validator = new SchemaValidator(reportFileName, schemaFile.Path);
				Assert.IsTrue(validator.Validate(), "validate failed");
			}
			finally
			{
				// Restore previous culture
				Thread.CurrentThread.CurrentCulture = previousCulture;
			}
		}

		private void runSchemaValidatorTest(TextWriter writer, CultureInfo testCulture)
		{
			// Preserve current culture
			CultureInfo previousCulture = Thread.CurrentThread.CurrentCulture;

			// Enable test culture
			Thread.CurrentThread.CurrentCulture = testCulture;

			try
			{
				string testsDll = "mock-assembly.dll";
				TestSuiteBuilder builder = new TestSuiteBuilder();
				Test suite = builder.Build( new TestPackage( testsDll ) );

				TestResult result = suite.Run(NullListener.NULL);

			  XmlResultVisitor visitor = new XmlResultVisitor(writer, result);
				result.Accept(visitor);
				visitor.Write();

			}
			finally
			{
				// Restore previous culture
				Thread.CurrentThread.CurrentCulture = previousCulture;
			}
		}

		private string tempFile;
		private TempResourceFile schemaFile;

		[SetUp]
		public void CreateTempFileName()
		{
			tempFile = "temp" + Guid.NewGuid().ToString() + ".xml";
			schemaFile = new TempResourceFile( GetType(), "Results.xsd");
		}

		[TearDown]
		public void RemoveTempFiles()
		{
			schemaFile.Dispose();

			FileInfo info = new FileInfo(tempFile);
			if(info.Exists) info.Delete();
		}

		[Test]
		public void TestSchemaValidatorInvariantCulture()
		{
			runSchemaValidatorTest(tempFile, CultureInfo.InvariantCulture);
		}

		[Test]
		public void TestSchemaValidatorUnitedStatesCulture()
		{
			CultureInfo unitedStatesCulture = new CultureInfo("en-US", false);
			runSchemaValidatorTest(tempFile,unitedStatesCulture);
		}

		[Test]
		public void TestStream()
		{
			CultureInfo unitedStatesCulture = new CultureInfo("en-US", false);
			runSchemaValidatorTest(tempFile,unitedStatesCulture);
			StringBuilder builder = new StringBuilder();
			StringWriter writer = new StringWriter(builder);
			runSchemaValidatorTest(writer,unitedStatesCulture);
			string second = builder.ToString();
			StreamReader reader = new StreamReader(tempFile);
			string first = reader.ReadToEnd();
			reader.Close();
			Assert.AreEqual(removeTimeAndAssertAttributes(first), removeTimeAndAssertAttributes(second));
		}

		[Test]
		public void TestSchemaValidatorFrenchCulture()
		{
			CultureInfo frenchCulture = new CultureInfo("fr-FR", false);
			runSchemaValidatorTest(tempFile, frenchCulture);
		}

		[Test]
		public void removeTime() 
		{
			string input = "foo time=\"123.745774xxx\" bar asserts=\"5\" time=\"0\"";
			string output = removeTimeAndAssertAttributes(input);
			Assert.AreEqual("foo  bar  ", output);
		}

		private string removeTimeAndAssertAttributes(string text) 
		{
			int index = 0;
			while ((index = text.IndexOf("time=\"")) != -1) 
			{
				int endQuote = text.IndexOf("\"", index + 7);
				text = text.Remove(index, endQuote - index + 1);
			}

			while ((index = text.IndexOf("asserts=\"")) != -1) 
			{
				int endQuote = text.IndexOf("\"", index + 10);
				text = text.Remove(index, endQuote - index + 1);
			}

			return text;
		}
	}
}
