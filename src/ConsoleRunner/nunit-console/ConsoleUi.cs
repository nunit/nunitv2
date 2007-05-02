// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

namespace NUnit.ConsoleRunner
{
	using System;
    using System.Collections;
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


			// Add Standard Services to ServiceManager
			ServiceManager.Services.AddService( new SettingsService() );
			ServiceManager.Services.AddService( new DomainManager() );
			//ServiceManager.Services.AddService( new RecentFilesService() );
			//ServiceManager.Services.AddService( new TestLoader() );
			ServiceManager.Services.AddService( new AddinRegistry() );
			ServiceManager.Services.AddService( new AddinManager() );

			// Initialize Services
			ServiceManager.Services.InitializeServices();

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

			Console.WriteLine( "Runtime Environment - " );
			RuntimeFramework framework = RuntimeFramework.CurrentFramework;
			Console.WriteLine( string.Format("   OS Version: {0}", Environment.OSVersion ) );
			Console.WriteLine( string.Format("  CLR Version: {0} ( {1} )",
				Environment.Version,  framework.GetDisplayName() ) );

			Console.WriteLine();
		}

		private static TestRunner MakeRunnerFromCommandLine( ConsoleOptions options )
		{
			ConsoleOptions.DomainUsage domainUsage = options.domain;
			if ( domainUsage == ConsoleOptions.DomainUsage.Default )
				domainUsage = options.ParameterCount == 1
					? ConsoleOptions.DomainUsage.Single
					: ConsoleOptions.DomainUsage.Multiple;

            TestRunner testRunner = null;
				
			switch( domainUsage )
			{
				case ConsoleOptions.DomainUsage.None:
					testRunner = new NUnit.Core.RemoteTestRunner();
					// Make sure that addins are available
					CoreExtensions.Host.AddinRegistry = Services.AddinRegistry;
					break;

				case ConsoleOptions.DomainUsage.Single:
					testRunner = new TestDomain();
					break;

				case ConsoleOptions.DomainUsage.Multiple:
					testRunner = new MultipleTestDomainRunner();
					break;
			}

			TestPackage package;
			if ( options.IsTestProject )
			{
				NUnitProject project = NUnitProject.LoadProject( (string)options.Parameters[0] );
				string configName = options.config;
				if ( configName != null )
					project.SetActiveConfig( configName );

				package = project.MakeTestPackage();
				if ( options.IsFixture )
					package.TestName = options.fixture;
			}
			else if ( options.Parameters.Count == 1 )
			{
				package = new TestPackage( (string)options.Parameters[0] );
			}
			else
			{
				package = new TestPackage( "UNNAMED", options.Parameters );
			}

			if ( options.IsFixture )
				package.TestName = options.fixture;
			package.Settings["ShadowCopyFiles"] = !options.noshadow;
			package.Settings["UseThreadedRunner"] = !options.nothread;
			testRunner.Load( package );

			return testRunner;
		}

		public ConsoleUi()
		{
		}

		public int Execute( ConsoleOptions options )
		{
			XmlTextReader transformReader = GetTransformReader(options);
			if(transformReader == null) return 3;

			TextWriter outWriter = Console.Out;
			if ( options.isOut )
			{
				StreamWriter outStreamWriter = new StreamWriter( options.output );
				outStreamWriter.AutoFlush = true;
				outWriter = outStreamWriter;
			}

			TextWriter errorWriter = Console.Error;
			if ( options.isErr )
			{
				StreamWriter errorStreamWriter = new StreamWriter( options.err );
				errorStreamWriter.AutoFlush = true;
				errorWriter = errorStreamWriter;
			}

			TestRunner testRunner = MakeRunnerFromCommandLine( options );

			try
			{
				if (testRunner.Test == null)
				{
					testRunner.Unload();
					Console.Error.WriteLine("Unable to locate fixture {0}", options.fixture);
					return 2;
				}

				EventCollector collector = new EventCollector( options, outWriter, errorWriter );

				TestFilter catFilter = TestFilter.Empty;

				if (options.HasInclude)
				{
					Console.WriteLine( "Included categories: " + options.include );
					catFilter = new CategoryFilter( options.IncludedCategories );
				}
			
				if ( options.HasExclude )
				{
					Console.WriteLine( "Excluded categories: " + options.exclude );
					TestFilter excludeFilter = new NotFilter( new CategoryFilter( options.ExcludedCategories ) );
					if ( catFilter.IsEmpty )
						catFilter = excludeFilter;
					else
						catFilter = new AndFilter( catFilter, excludeFilter );
				}

				TestResult result = null;
				string savedDirectory = Environment.CurrentDirectory;
				TextWriter savedOut = Console.Out;
				TextWriter savedError = Console.Error;

				try
				{
					result = testRunner.Run( collector, catFilter );
				}
				finally
				{
					outWriter.Flush();
					errorWriter.Flush();

					if ( options.isOut )
						outWriter.Close();
					if ( options.isErr )
						errorWriter.Close();

					Environment.CurrentDirectory = savedDirectory;
					Console.SetOut( savedOut );
					Console.SetError( savedError );
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

				//if ( testRunner != null )
				//    testRunner.Unload();

				if ( collector.HasExceptions )
				{
					collector.WriteExceptions();
					return 2;
				}
            
				return result.IsFailure ? 1 : 0;
			}
			finally
			{
				testRunner.Unload();
			}
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

		private class EventCollector : MarshalByRefObject, EventListener
		{
			private int testRunCount;
			private int testIgnoreCount;
			private int failureCount;
			private int level;

			private ConsoleOptions options;
			private TextWriter outWriter;
			private TextWriter errorWriter;

			StringCollection messages;
		
			private bool progress = false;
			private string currentTestName;

            private ArrayList unhandledExceptions = new ArrayList();

			public EventCollector( ConsoleOptions options, TextWriter outWriter, TextWriter errorWriter )
			{
				level = 0;
				this.options = options;
				this.outWriter = outWriter;
				this.errorWriter = errorWriter;
				this.currentTestName = string.Empty;
				this.progress = !options.xmlConsole && !options.labels && !options.nodots;

                AppDomain.CurrentDomain.UnhandledException += 
                    new UnhandledExceptionEventHandler(OnUnhandledException);
			}

            public bool HasExceptions
            {
                get { return unhandledExceptions.Count > 0; }
            }

            public void WriteExceptions()
            {
                Console.WriteLine();
                Console.WriteLine("Unhandled exceptions:");
                int index = 1;
                foreach( string msg in unhandledExceptions )
                    Console.WriteLine( "{0}) {1}", index++, msg );
            }

			public void RunStarted(string name, int testCount)
			{
			}

			public void RunFinished(TestResult result)
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
						
						messages.Add( string.Format( "{0}) {1} :", failureCount, testResult.Test.TestName.FullName ) );
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
				else
				{
					testIgnoreCount++;
					
					if ( progress )
						Console.Write("N");
				}

				currentTestName = string.Empty;
			}

			public void TestStarted(TestName testName)
			{
				currentTestName = testName.FullName;

				if ( options.labels )
					outWriter.WriteLine("***** {0}", currentTestName );
				
				if ( progress )
					Console.Write(".");
			}

			public void SuiteStarted(TestName testName)
			{
				if ( level++ == 0 )
				{
					messages = new StringCollection();
					testRunCount = 0;
					testIgnoreCount = 0;
					failureCount = 0;
					Trace.WriteLine( "################################ UNIT TESTS ################################" );
					Trace.WriteLine( "Running tests in '" + testName.FullName + "'..." );
				}
			}

			public void SuiteFinished(TestSuiteResult suiteResult) 
			{
				if ( --level == 0) 
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
					Trace.WriteLine( "Executed tests       : " + testRunCount );
					Trace.WriteLine( "Ignored tests        : " + testIgnoreCount );
					Trace.WriteLine( "Failed tests         : " + failureCount );
                    Trace.WriteLine( "Unhandled exceptions : " + unhandledExceptions.Count);
					Trace.WriteLine( "Total time           : " + suiteResult.Time + " seconds" );
					Trace.WriteLine( "############################################################################");
				}
			}

            private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
            {
                if (e.ExceptionObject.GetType() != typeof(System.Threading.ThreadAbortException))
                {
                    this.UnhandledException((Exception)e.ExceptionObject);
                }
            }


			public void UnhandledException( Exception exception )
			{
                // If we do labels, we already have a newline
                unhandledExceptions.Add(currentTestName + " : " + exception.ToString());
                //if (!options.labels) outWriter.WriteLine();
                string msg = string.Format("##### Unhandled Exception while running {0}", currentTestName);
                //outWriter.WriteLine(msg);
                //outWriter.WriteLine(exception.ToString());

                Trace.WriteLine(msg);
                Trace.WriteLine(exception.ToString());
			}

			public void TestOutput( TestOutput output)
			{
				switch ( output.Type )
				{
					case TestOutputType.Out:
						outWriter.Write( output.Text );
						break;
					case TestOutputType.Error:
						errorWriter.Write( output.Text );
						break;
				}
			}


            public override object InitializeLifetimeService()
            {
                return null;
            }
		}

		#endregion
	}
}

