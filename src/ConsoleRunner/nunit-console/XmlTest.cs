//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Console
{
	using System;
	using System.IO;
	using System.Xml;
	using System.Xml.Schema;
	using Nunit.Core;
	using Nunit.Tests;
	using Nunit.Framework;

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

		[Test]
		public void TestSchemaValidator()
		{
			string testsDll = "nunit.tests.dll";
			TestSuite suite = TestSuiteBuilder.Build(testsDll);
		
			TestResult result = suite.Run(NullListener.NULL);
			XmlResultVisitor visitor = new XmlResultVisitor("temp.xml", result);
			result.Accept(visitor);
			visitor.Write();

			string schemaFile = null;
			FileInfo file = new FileInfo("Results.xsd");
			if(file.Exists)
				schemaFile = file.FullName;
			else
				schemaFile = "..\\..\\..\\nunit-console\\Results.xsd";

			SchemaValidator validator = new SchemaValidator("temp.xml", schemaFile);
			Assertion.Assert("validate failed", validator.Validate());
		}
	}
}
