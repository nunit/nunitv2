#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

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
