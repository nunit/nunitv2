// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

namespace NUnit.ConsoleRunner
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Resources;
	using System.Text;
	using NUnit.Core;
	using NUnit.Core.Filters;
	using NUnit.Util;
	
	/// <summary>
	/// Summary description for ConsoleUi.
	/// </summary>
	public class ConsoleUi
	{
		public static readonly int OK = 0;
		public static readonly int INVALID_ARG = -1;
		public static readonly int FILE_NOT_FOUND = -2;
		public static readonly int FIXTURE_NOT_FOUND = -3;
		public static readonly int UNEXPECTED_ERROR = -100;

		public ConsoleUi()
		{
		}

		public int Execute( ConsoleOptions options )
		{
			TextWriter outWriter = Console.Out;
			bool redirectOutput = options.output != null && options.output != string.Empty;
			if ( redirectOutput )
			{
				StreamWriter outStreamWriter = new StreamWriter( options.output );
				outStreamWriter.AutoFlush = true;
				outWriter = outStreamWriter;
			}

			TextWriter errorWriter = Console.Error;
			bool redirectError = options.err != null && options.err != string.Empty;
			if ( redirectError )
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
					return FIXTURE_NOT_FOUND;
				}

				EventCollector collector = new EventCollector( options, outWriter, errorWriter );

				TestFilter testFilter = TestFilter.Empty;
				if ( options.run != null && options.run != string.Empty )
				{
					Console.WriteLine( "Selected test: " + options.run );
					testFilter = new SimpleNameFilter( options.run );
				}

				if ( options.include != null && options.include != string.Empty )
				{
					Console.WriteLine( "Included categories: " + options.include );
					TestFilter includeFilter = new CategoryExpression( options.include ).Filter;
					if ( testFilter.IsEmpty )
						testFilter = includeFilter;
					else
						testFilter = new AndFilter( testFilter, includeFilter );
				}

				if ( options.exclude != null && options.exclude != string.Empty )
				{
					Console.WriteLine( "Excluded categories: " + options.exclude );
					TestFilter excludeFilter = new NotFilter( new CategoryExpression( options.exclude ).Filter );
					if ( testFilter.IsEmpty )
						testFilter = excludeFilter;
					else if ( testFilter is AndFilter )
						((AndFilter)testFilter).Add( excludeFilter );
					else
						testFilter = new AndFilter( testFilter, excludeFilter );
				}

				TestResult result = null;
				string savedDirectory = Environment.CurrentDirectory;
				TextWriter savedOut = Console.Out;
				TextWriter savedError = Console.Error;

				try
				{
					result = testRunner.Run( collector, testFilter );
				}
				finally
				{
					outWriter.Flush();
					errorWriter.Flush();

					if ( redirectOutput )
						outWriter.Close();
					if ( redirectError )
						errorWriter.Close();

					Environment.CurrentDirectory = savedDirectory;
					Console.SetOut( savedOut );
					Console.SetError( savedError );
				}

				Console.WriteLine();

				string xmlOutput = CreateXmlOutput( result );
				ResultSummarizer summary = new ResultSummarizer( result );

                if (options.xmlConsole)
				{
					Console.WriteLine(xmlOutput);
				}
				else
				{
                    WriteSummaryReport(summary);
                    if (summary.ErrorsAndFailures > 0)
                        WriteErrorsAndFailuresReport(result);
                    if (summary.TestsNotRun > 0)
                        WriteNotRunReport(result);
				}

				// Write xml output here
				string xmlResultFile = options.xml == null || options.xml == string.Empty
					? "TestResult.xml" : options.xml;

				using ( StreamWriter writer = new StreamWriter( xmlResultFile ) ) 
				{
					writer.Write(xmlOutput);
				}

				if ( collector.HasExceptions )
				{
					collector.WriteExceptions();
					return UNEXPECTED_ERROR;
				}
            
				return summary.ErrorsAndFailures;
			}
			finally
			{
				testRunner.Unload();
			}
		}

		#region Helper Methods
		private static TestRunner MakeRunnerFromCommandLine( ConsoleOptions options )
		{
			TestPackage package;
			DomainUsage domainUsage = DomainUsage.Default;

			if (options.IsTestProject)
			{
				NUnitProject project = 
					Services.ProjectService.LoadProject((string)options.Parameters[0]);

				string configName = options.config;
				if (configName != null)
					project.SetActiveConfig(configName);

				package = project.ActiveConfig.MakeTestPackage();
				package.TestName = options.fixture;

				domainUsage = DomainUsage.Single;
			}
			else if (options.Parameters.Count == 1)
			{
				package = new TestPackage((string)options.Parameters[0]);
				domainUsage = DomainUsage.Single;
			}
			else
			{
				package = new TestPackage("UNNAMED", options.Parameters);
				domainUsage = DomainUsage.Multiple;
			}

			if (options.domain != DomainUsage.Default)
				domainUsage = options.domain;

			package.TestName = options.fixture;
            
            package.Settings["ProcessModel"] = options.process;
            package.Settings["DomainUsage"] = domainUsage;

            if (domainUsage == DomainUsage.None)
            {
                // Make sure that addins are available
                CoreExtensions.Host.AddinRegistry = Services.AddinRegistry;
            }

            package.Settings["ShadowCopyFiles"] = !options.noshadow;
			package.Settings["UseThreadedRunner"] = !options.nothread;
            package.Settings["DefaultTimeout"] = options.timeout;

            TestRunner testRunner = TestRunnerFactory.MakeTestRunner(package);
			testRunner.Load( package );
			return testRunner;
		}

		private static string CreateXmlOutput( TestResult result )
		{
			StringBuilder builder = new StringBuilder();
			new XmlResultWriter(new StringWriter( builder )).SaveTestResult(result);

			return builder.ToString();
		}

		private static void WriteSummaryReport( ResultSummarizer summary )
		{
            Console.WriteLine(
                "Tests run: {0}, Errors: {1}, Failures: {2}, Inconclusive: {3} Time: {4} seconds",
                summary.TestsRun, summary.Errors, summary.Failures, summary.Inconclusive, summary.Time);
            Console.WriteLine(
                "  Not run: {0}, Invalid: {1}, Ignored: {2}, Skipped: {3}",
                summary.TestsNotRun, summary.NotRunnable, summary.Ignored, summary.Skipped);
            Console.WriteLine();
        }

        private void WriteErrorsAndFailuresReport(TestResult result)
        {
            reportIndex = 0;
            Console.WriteLine("Errors and Failures:");
            WriteErrorsAndFailures(result);
            Console.WriteLine();
        }

        private void WriteErrorsAndFailures(TestResult result)
        {
            if (result.Executed)
            {
                if (result.HasResults)
                {
                    if ( (result.IsFailure || result.IsError) && result.FailureSite == FailureSite.SetUp)
                        WriteSingleResult(result);

                    foreach (TestResult childResult in result.Results)
                        WriteErrorsAndFailures(childResult);
                }
                else if (result.IsFailure || result.IsError)
                {
                    WriteSingleResult(result);
                }
            }
        }

        private void WriteNotRunReport(TestResult result)
        {
	        reportIndex = 0;
            Console.WriteLine("Tests Not Run:");
	        WriteNotRunResults(result);
            Console.WriteLine();
        }

	    private int reportIndex = 0;
        private void WriteNotRunResults(TestResult result)
        {
            if (result.HasResults)
                foreach (TestResult childResult in result.Results)
                    WriteNotRunResults(childResult);
            else if (!result.Executed)
                WriteSingleResult( result );
        }

        private void WriteSingleResult( TestResult result )
        {
            string status = result.IsFailure || result.IsError
                ? string.Format("{0} {1}", result.FailureSite, result.ResultState)
                : result.ResultState.ToString();

            Console.WriteLine("{0}) {1} : {2}", ++reportIndex, status, result.FullName);

            if ( result.Message != null && result.Message != string.Empty )
                 Console.WriteLine("   {0}", result.Message);

            if (result.StackTrace != null && result.StackTrace != string.Empty)
                Console.WriteLine( result.IsFailure
                    ? StackTraceFilter.Filter(result.StackTrace)
                    : result.StackTrace + Environment.NewLine );
        }
	    #endregion
	}
}

