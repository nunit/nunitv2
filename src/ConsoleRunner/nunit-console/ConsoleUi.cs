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

namespace NUnit.ConsoleRunner
{
	using System;
	using System.Collections.Specialized;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Resources;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Diagnostics;
	using System.Runtime.InteropServices;
	using NUnit.Core;
	using NUnit.Core.Filters;
	using NUnit.Util;
	
	/// <summary>
	/// Summary description for ConsoleUi.
	/// </summary>
	public class ConsoleUi
	{
		[STAThread]
		public static int Main(string[] args)
		{
			ConsoleOptions options = new ConsoleOptions(args);
			
			if(!options.nologo)
				WriteCopyright();

			if(options.help)
			{
				options.Help();
				return 0;
			}
			
			if(options.NoArgs) 
			{
				Console.Error.WriteLine("fatal error: no inputs specified");
				options.Help();
				return 0;
			}
			
			if(!options.Validate())
			{
				Console.Error.WriteLine("fatal error: invalid arguments");
				options.Help();
				return 2;
			}

			try
			{
				ConsoleUi consoleUi = new ConsoleUi();
				return consoleUi.Execute( options );
			}
			catch( FileNotFoundException ex )
			{
				Console.WriteLine( ex.Message );
				return 2;
			}
			catch( BadImageFormatException ex )
			{
				Console.WriteLine( ex.Message );
				return 2;
			}
			catch( Exception ex )
			{
				Console.WriteLine( "Unhandled Exception:\n{0}", ex.ToString() );
				return 2;
			}
			finally
			{
				if(options.wait)
				{
					Console.Out.WriteLine("\nHit <enter> key to continue");
					Console.ReadLine();
				}
			}
		}

		private static XmlTextReader GetTransformReader(ConsoleOptions parser)
		{
			XmlTextReader reader = null;
			if(!parser.IsTransform)
			{
				Assembly assembly = Assembly.GetAssembly(typeof(XmlResultVisitor));
				ResourceManager resourceManager = new ResourceManager("NUnit.Util.Transform",assembly);
				string xmlData = (string)resourceManager.GetObject("Summary.xslt");

				reader = new XmlTextReader(new StringReader(xmlData));
			}
			else
			{
				FileInfo xsltInfo = new FileInfo(parser.transform);
				if(!xsltInfo.Exists)
				{
					Console.Error.WriteLine("Transform file: {0} does not exist", xsltInfo.FullName);
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
			Console.WriteLine();

			string clrPlatform = Type.GetType("Mono.Runtime", false) == null ? ".NET" : "Mono";
			Console.WriteLine( string.Format("OS Version: {0}    {1} Version: {2}",
				Environment.OSVersion, clrPlatform, Environment.Version ) );
			Console.WriteLine();
		}

		private static bool MakeTestFromCommandLine(TestRunnerEx testRunner, ConsoleOptions parser)
		{
			NUnitProject project;

			if ( parser.IsTestProject )
			{
				project = NUnitProject.LoadProject( (string)parser.Parameters[0] );
				string configName = parser.config;
				if ( configName != null )
					project.SetActiveConfig( configName );
			}
			else
				project = NUnitProject.FromAssemblies( (string[])parser.Parameters.ToArray( typeof( string ) ) );

			return testRunner.Load( project, parser.fixture );
		}

		public ConsoleUi()
		{
		}

		public int Execute( ConsoleOptions options )
		{
			XmlTextReader transformReader = GetTransformReader(options);
			if(transformReader == null) return 3;

			TextWriter outWriter = options.isOut
				? new StreamWriter( options.output )
				: Console.Out;

			TextWriter errorWriter = options.isErr
				? new StreamWriter( options.err )
				: Console.Error;

			TestRunnerEx testRunner = options.ParameterCount == 1 
				? (TestRunnerEx)new TestDomain()
				: (TestRunnerEx)new MultipleTestDomainRunner();

			if ( options.noshadow  ) testRunner.Settings["ShadowCopyFiles"] = false;
			
			if ( !MakeTestFromCommandLine(testRunner, options) )
			{
				Console.Error.WriteLine("Unable to locate fixture {0}", options.fixture);
				return 2;
			}

			EventListener collector = new EventCollector( options, outWriter, errorWriter );

			ITestFilter catFilter = null;

			if (options.HasInclude)
			{
				Console.WriteLine( "Included categories: " + options.include );
				catFilter = new CategoryFilter( options.IncludedCategories );
			}
			
			if ( options.HasExclude )
			{
				Console.WriteLine( "Excluded categories: " + options.exclude );
				ITestFilter excludeFilter = new NotFilter( new CategoryFilter( options.ExcludedCategories ) );
				if ( catFilter == null )
					catFilter = excludeFilter;
				else
					catFilter = new AndFilter( catFilter, excludeFilter );
			}

			TestResult result = null;
			using( new DirectorySwapper() )
			{
				try
				{
					result = testRunner.Run( collector );
				}
				finally
				{
					if ( options.isOut )
						outWriter.Close();
					if ( options.isErr )
						errorWriter.Close();
				}
			}

			Console.WriteLine();

			string xmlOutput = CreateXmlOutput( result );
			
			if (options.xmlConsole)
			{
				Console.WriteLine(xmlOutput);
			}
			else
			{
				try
				{
					//CreateSummaryDocument(xmlOutput, transformReader );
					XmlResultTransform xform = new XmlResultTransform( transformReader );
					xform.Transform( new StringReader( xmlOutput ), Console.Out );
				}
				catch( Exception ex )
				{
					Console.WriteLine( "Error: {0}", ex.Message );
					return 3;
				}
			}

			// Write xml output here
			string xmlResultFile = options.IsXml ? options.xml : "TestResult.xml";

			using ( StreamWriter writer = new StreamWriter( xmlResultFile ) ) 
			{
				writer.Write(xmlOutput);
			}

			if ( testRunner != null )
				testRunner.Unload();


			return result.IsFailure ? 1 : 0;
		}

		private string CreateXmlOutput( TestResult result )
		{
			StringBuilder builder = new StringBuilder();
			XmlResultVisitor resultVisitor = new XmlResultVisitor(new StringWriter( builder ), result);
			result.Accept(resultVisitor);
			resultVisitor.Write();

			return builder.ToString();
		}

		#region Nested Class to Handle Events

		private class EventCollector : LongLivingMarshalByRefObject, EventListener
		{
			private int testRunCount;
			private int testIgnoreCount;
			private int failureCount;
			private int level;

			private ConsoleOptions options;
			private TextWriter outWriter;
			private TextWriter errorWriter;

			StringCollection messages;
		
			private bool debugger = false;
			private bool progress = false;
			private string currentTestName;

			public EventCollector( ConsoleOptions options, TextWriter outWriter, TextWriter errorWriter )
			{
				debugger = Debugger.IsAttached;
				level = 0;
				this.options = options;
				this.outWriter = outWriter;
				this.errorWriter = errorWriter;
				this.currentTestName = string.Empty;
				this.progress = !options.xmlConsole && !options.labels;
			}

			public void RunStarted(string name, int testCount)
			{
			}

			public void RunFinished(TestResult[] results)
			{
			}

			public void RunFinished(Exception exception)
			{
			}

			public void TestFinished(TestCaseResult testResult)
			{
				if(testResult.Executed)
				{
					testRunCount++;
					
					if(testResult.IsFailure)
					{	
						failureCount++;
						
						if ( progress )
							Console.Write("F");
						
						if ( debugger )
						{
							messages.Add( string.Format( "{0}) {1} :", failureCount, testResult.Test.FullName ) );
							messages.Add( testResult.Message.Trim( Environment.NewLine.ToCharArray() ) );

							string stackTrace = StackTraceFilter.Filter( testResult.StackTrace );
							if ( stackTrace != null && stackTrace != string.Empty )
							{
								string[] trace = stackTrace.Split( System.Environment.NewLine.ToCharArray() );
								foreach( string s in trace )
								{
									if ( s != string.Empty )
									{
										string link = Regex.Replace( s.Trim(), @".* in (.*):line (.*)", "$1($2)");
										messages.Add( string.Format( "at\n{0}", link ) );
									}
								}
							}
						}
					}
				}
				else
				{
					testIgnoreCount++;
					
					if ( progress )
						Console.Write("N");
				}

				currentTestName = string.Empty;
			}

			public void TestStarted(TestInfo testCase)
			{
				currentTestName = testCase.FullName;

				if ( options.labels )
					outWriter.WriteLine("***** {0}", testCase.FullName );
				
				if ( progress )
					Console.Write(".");
			}

			public void SuiteStarted(TestInfo suite) 
			{
				if ( debugger && level++ == 0 )
				{
					messages = new StringCollection();
					testRunCount = 0;
					testIgnoreCount = 0;
					failureCount = 0;
					Trace.WriteLine( "################################ UNIT TESTS ################################" );
					Trace.WriteLine( "Running tests in '" + suite.FullName + "'..." );
				}
			}

			public void SuiteFinished(TestSuiteResult suiteResult) 
			{
				if ( debugger && --level == 0) 
				{
					Trace.WriteLine( "############################################################################" );

					if (messages.Count == 0) 
					{
						Trace.WriteLine( "##############                 S U C C E S S               #################" );
					}
					else 
					{
						Trace.WriteLine( "##############                F A I L U R E S              #################" );
						
						foreach ( string s in messages ) 
						{
							Trace.WriteLine(s);
						}
					}

					Trace.WriteLine( "############################################################################" );
					Trace.WriteLine( "Executed tests : " + testRunCount );
					Trace.WriteLine( "Ignored tests  : " + testIgnoreCount );
					Trace.WriteLine( "Failed tests   : " + failureCount );
					Trace.WriteLine( "Total time     : " + suiteResult.Time + " seconds" );
					Trace.WriteLine( "############################################################################");
				}
			}

			public void UnhandledException( Exception exception )
			{
				string msg = string.Format( "##### Unhandled Exception while running {0}", currentTestName );

				// If we do labels, we already have a newline
				if ( !options.labels ) outWriter.WriteLine();
				outWriter.WriteLine( msg );
				outWriter.WriteLine( exception.ToString() );

				if ( debugger )
				{
					Trace.WriteLine( msg );
					Trace.WriteLine( exception.ToString() );
				}
			}

			public void TestOutput( TestOutput output)
			{
				switch ( output.Type )
				{
					case TestOutputType.Out:
						outWriter.WriteLine( output.Text );
						break;
					case TestOutputType.Error:
						errorWriter.WriteLine( output.Text );
						break;
				}
			}
		}

		#endregion
	}
}

