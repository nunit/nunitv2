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
namespace NUnit.Console
{
	using System;
	using System.IO;
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
					myXmlValidatingReader.Close();
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

		private static readonly string fileName = "temp.xml";

		[TearDown]
		public void DeleteTempFiles()
		{
			FileInfo file = new FileInfo(fileName);
			if(file.Exists) file.Delete();
		}

		[Test]
		public void TestSchemaValidator()
		{
			string testsDll = "nunit.tests.dll";
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build(testsDll);
		
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

			SchemaValidator validator = new SchemaValidator(fileName, schemaFile);
			Assertion.Assert("validate failed", validator.Validate());
		}
	}
}
