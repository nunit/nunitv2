#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
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
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Console
{
	using System;
	using System.Collections;
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
		private NUnit.Core.TestDomain testDomain;
		private StringBuilder builder;
		private XmlTextReader transformReader;
		private bool silent;

		public static int Main(string[] args)
		{
			int returnCode = 0;

			NUnitRegistry.InitializeAddReferenceDialog();

			ConsoleOptions parser = new ConsoleOptions(args);
			if(!parser.nologo)
				WriteCopyright();

			if(parser.help)
			{
				parser.Help();
			}
			else if(parser.NoArgs) 
			{
				Console.Error.WriteLine("\nfatal error: no inputs specified");
				parser.Help();
			}
			else if(!parser.Validate())
			{
				Console.Error.WriteLine("\nfatal error: invalid arguments");
				parser.Help();
				returnCode = 2;
			}
			else
			{
				NUnit.Core.TestDomain domain = new NUnit.Core.TestDomain();

				Test test = MakeTestFromCommandLine(domain, parser);
				try
				{
					if(test == null)
					{
						Console.Error.WriteLine("\nfatal error: invalid assembly {0}", parser.Parameters[0]);
						returnCode = 2;
					}
					else
					{
						Directory.SetCurrentDirectory(new FileInfo((string)parser.Parameters[0]).DirectoryName);
						string xmlResult = "TestResult.xml";
						if(parser.IsXml)
							xmlResult = parser.xml;

						StringBuilder b = new StringBuilder();
						
						XmlTextReader reader = GetTransformReader(parser);
						if(reader != null)
						{
							ConsoleUi consoleUi = new ConsoleUi(domain, b, reader, parser.xmlConsole);
							returnCode = consoleUi.Execute();
							if (parser.xmlConsole)
								Console.WriteLine(b.ToString());
							using (StreamWriter writer = new StreamWriter(xmlResult)) 
							{
								writer.Write(b.ToString());
							}
							if(parser.wait)
							{
								Console.Out.WriteLine("Hit <enter> key to continue");
								Console.ReadLine();
							}
						}
						else
							returnCode = 3;
					}
				}
				finally
				{
					domain.Unload();
				}
			}

			return returnCode;
		}

		private static XmlTextReader GetTransformReader(ConsoleOptions parser)
		{
			XmlTextReader reader = null;
			if(!parser.IsTransform)
			{
				Assembly assembly = Assembly.GetAssembly(typeof(XmlResultVisitor));
				ResourceManager resourceManager = new ResourceManager("NUnit.Framework.Transform",assembly);
				string xmlData = (string)resourceManager.GetObject("Summary.xslt");

				reader = new XmlTextReader(new StringReader(xmlData));
			}
			else
			{
				FileInfo xsltInfo = new FileInfo(parser.transform);
				if(!xsltInfo.Exists)
				{
					Console.Error.WriteLine("\nTransform file: {0} does not exist", xsltInfo.FullName);
					reader = null;
				}
				else
				{
					reader = new XmlTextReader(xsltInfo.FullName);
				}
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

		private static Test MakeTestFromCommandLine(NUnit.Core.TestDomain testDomain, 
			ConsoleOptions parser)
		{
			Test test = null;

			if(!DoAssembliesExist(parser.Parameters)) return null; 
			
			if(parser.IsTestProject)
			{
				NUnitProject project = NUnitProject.LoadProject( (string)parser.Parameters[0] );
				test = project.LoadTest( testDomain );
				if (test == null) Console.WriteLine("\nfatal error: project ({0}) is invalid", parser.Parameters[0]);
			}
			else if(parser.IsAssembly)
			{
				test = testDomain.LoadAssemblies( parser.Parameters );
				if(test == null) Console.WriteLine("\nfatal error: assembly ({0}) is invalid", parser.Parameters[0]);
			}
			else if(parser.IsFixture)
			{
				test = testDomain.LoadAssembly( (string)parser.Parameters[0], parser.fixture );
				if(test == null) Console.WriteLine("\nfatal error: fixture ({0}) in assembly ({1}) is invalid", parser.fixture, parser.Parameters[0]);
			}
			return test;
		}

		private static bool DoAssembliesExist(IList files)
		{
			bool exist = true; 
			foreach(string fileName in files)
				exist &= DoesFileExist(fileName);
			return exist;
		}

		private static bool DoesFileExist(string fileName)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			return fileInfo.Exists;
		}

		public ConsoleUi(NUnit.Core.TestDomain testDomain, StringBuilder builder, XmlTextReader reader, bool silent)
		{
			this.testDomain = testDomain;
			this.builder = builder;
			transformReader = reader;
			this.silent = silent;
		}

		public int Execute()
		{
			EventListener collector = null;
			if (silent)
				collector = new NullListener();
			else
				collector = new EventCollector();
			ConsoleWriter outStream = new ConsoleWriter(Console.Out);
			ConsoleWriter errorStream = new ConsoleWriter(Console.Error);
			
			string savedDirectory = Environment.CurrentDirectory;
			TestResult result = testDomain.Run(collector, outStream, errorStream);
			Directory.SetCurrentDirectory( savedDirectory );
			
			Console.WriteLine("\n");
			XmlResultVisitor resultVisitor = new XmlResultVisitor(new StringWriter(builder), result);
			result.Accept(resultVisitor);
			resultVisitor.Write();
			if (!silent)
				CreateSummaryDocument();

			int resultCode = 0;
			if(result.IsFailure)
				resultCode = 1;
			return resultCode;
		}

		private void CreateSummaryDocument()
		{
			XPathDocument originalXPathDocument = new XPathDocument(new StringReader(builder.ToString()));
			XslTransform summaryXslTransform = new XslTransform();
			summaryXslTransform.Load(transformReader);
			
			summaryXslTransform.Transform(originalXPathDocument,null,Console.Out);
		}

		private class EventCollector : LongLivingMarshalByRefObject, EventListener
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
