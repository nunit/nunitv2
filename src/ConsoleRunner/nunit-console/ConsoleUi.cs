//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Console
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Xsl;
	using System.Xml.XPath;
	using System.Resources;
	using System.Text;

	using NUnit.Core;
	using NUnit.Util;
	

	/// <summary>
	/// Summary description for ConsoleUi.
	/// </summary>
	public class ConsoleUi
	{
		private TestSuite suite;
		private string outputFile;
		private XmlTextReader transformReader;

		public static void Main(string[] args)
		{
			WriteCopyright();

			try
			{
				CommandLineParser parser = new CommandLineParser(args);
				if(parser.NoArgs) 
					Console.Error.WriteLine("fatal error: no inputs specified");
				else
				{
					TestSuite suite = MakeSuiteFromCommandLine(parser);
					if(suite == null) Environment.Exit(2);
				
					Directory.SetCurrentDirectory(new FileInfo(parser.AssemblyName).DirectoryName);
					string xmlResult = "TestResult.xml";
					if(parser.IsXml)
						xmlResult = parser.XmlFileName;

					XmlTextReader reader = GetTransformReader(parser);

					ConsoleUi consoleUi = new ConsoleUi(suite, xmlResult, reader);
					Environment.Exit(consoleUi.Execute());
				}
			}
			catch(CommandLineException cle)
			{
				Console.Error.WriteLine(cle.Message);
				Environment.Exit(2);
			}
		}

		private static XmlTextReader GetTransformReader(CommandLineParser parser)
		{
			XmlTextReader reader = null;
			if(!parser.IsTransform)
			{
				Assembly assembly = Assembly.GetEntryAssembly();
				ResourceManager resourceManager = new ResourceManager("nunit_console.Transform",assembly);
				string xmlData = (string)resourceManager.GetObject("Summary.xslt");

				reader = new XmlTextReader(new StringReader(xmlData));
			}
			else
			{
				FileInfo xsltInfo = new FileInfo(parser.TransformFileName);
				if(!xsltInfo.Exists)
				{
					Console.Error.WriteLine("\nTransform file: {0} does not exist", xsltInfo.FullName);
					Environment.Exit(3);
				}

				reader = new XmlTextReader(xsltInfo.FullName);
			}

			return reader;
		}

		private static void WriteCopyright()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			System.Version version = executingAssembly.GetName().Version;

			object[] objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
			AssemblyProductAttribute productAttr = (AssemblyProductAttribute)objectAttrs[0];

			objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
			AssemblyCopyrightAttribute copyrightAttr = (AssemblyCopyrightAttribute)objectAttrs[0];

			Console.WriteLine(String.Format("{0} version {1}", productAttr.Product, version.ToString(3)));
			Console.WriteLine(copyrightAttr.Copyright);
		}

		private static TestSuite MakeSuiteFromCommandLine(CommandLineParser parser)
		{
			TestSuite suite = null;
			if(parser.IsAssembly)
			{
				suite = TestSuiteBuilder.Build(parser.AssemblyName);
				if(suite == null) Console.WriteLine("\nfatal error: assembly ({0}) is invalid", parser.AssemblyName);
			}
			else if(parser.IsFixture)
			{
				suite = TestSuiteBuilder.Build(parser.TestName, parser.AssemblyName);
				if(suite == null) Console.WriteLine("\nfatal error: fixture ({0}) in assembly ({1}) is invalid", parser.TestName, parser.AssemblyName);
			}
			return suite;
		}

		private static void WriteHelp(TextWriter writer)
		{
			writer.WriteLine("\n\n         NUnit console options\n");
			writer.WriteLine("/assembly:<assembly name>                            Assembly to test");
			writer.WriteLine("/fixture:<class name> /assembly:<assembly name>      Fixture or Suite to run");
			writer.WriteLine("\n\n         XML formatting options");
			writer.WriteLine("/xml:<file>                 XML result file to generate");
			writer.WriteLine("/transform:<file>           XSL transform file");
		}

		public ConsoleUi(TestSuite testSuite, string xmlFile, XmlTextReader reader)
		{
			suite = testSuite;
			outputFile = xmlFile;
			transformReader = reader;
		}

		public int Execute()
		{
			EventCollector collector = new EventCollector();
			TestResult result = suite.Run(collector);

			Console.WriteLine("\n");
			XmlResultVisitor resultVisitor = new XmlResultVisitor(outputFile, result);
			result.Accept(resultVisitor);
			resultVisitor.Write();
			CreateSummaryDocument();

			int resultCode = 0;
			if(result.IsFailure)
				resultCode = 1;
			return resultCode;
		}

		private void CreateSummaryDocument()
		{
			XPathDocument originalXPathDocument = new XPathDocument (outputFile);
			XslTransform summaryXslTransform = new XslTransform();
			summaryXslTransform.Load(transformReader);
			
			summaryXslTransform.Transform(originalXPathDocument,null,Console.Out);
		}

		private class EventCollector : EventListener
		{
			public void TestFinished(TestCaseResult testResult)
			{
				if(testResult.Executed)
				{
					if(testResult.IsFailure)
					{	
						Console.Write("F");
					}
				}
				else
					Console.Write("N");
			}

			public void TestStarted(TestCase testCase)
			{
				Console.Write(".");
			}

			public void SuiteStarted(TestSuite suite) {}
			public void SuiteFinished(TestSuiteResult result) {}
		}

	}
}
