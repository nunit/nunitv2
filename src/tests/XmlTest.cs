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
	using System.Globalization;
	using System.IO;    
	using System.Threading;
	using System.Xml;
	using System.Xml.Schema;
	using NUnit.Core;
	using NUnit.Tests;
	using NUnit.Framework;

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
				myXmlSchemaCollection.Add(XmlSchema.Read(new XmlTextReader(schemaFile), null));

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
					throw new NunitException(e.Message, e);
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
				TestSuite suite = builder.Build(testsDll);

				TestResult result = suite.Run(NullListener.NULL);

                XmlResultVisitor visitor = new XmlResultVisitor(reportFileName, result);
				result.Accept(visitor);
				visitor.Write();

				string schemaFile = null;
				FileInfo file = new FileInfo("Results.xsd");
				if(file.Exists)
					schemaFile = file.FullName;
				else
					schemaFile = "..\\..\\..\\framework\\Results.xsd";

				SchemaValidator validator = new SchemaValidator(reportFileName, schemaFile);
				Assertion.Assert("validate failed", validator.Validate());
			}
			finally
			{
				// Restore previous culture
				Thread.CurrentThread.CurrentCulture = previousCulture;
			}
		}

		private string tempFile;

		[SetUp]
		public void CreateTempFileName()
		{
			tempFile = "temp" + Guid.NewGuid().ToString() + ".xml";
		}

		[TearDown]
		public void RemoveTempFile()
		{
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
		public void TestSchemaValidatorFrenchCulture()
		{
			CultureInfo frenchCulture = new CultureInfo("fr-FR", false);
			runSchemaValidatorTest(tempFile, frenchCulture);
		}
	}
}
