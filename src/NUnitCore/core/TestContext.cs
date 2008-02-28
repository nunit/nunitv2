// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace NUnit.Core
{
	/// <summary>
	/// Helper class used to save and restore certain static or
	/// singleton settings in the environment that affect tests 
	/// or which might be changed by the user tests.
	/// 
	/// An internal class is used to hold settings and a stack
	/// of these objects is pushed and popped as Save and Restore
	/// are called.
	/// 
	/// Static methods for each setting forward to the internal 
	/// object on the top of the stack.
	/// 
	/// When TestContext itself is instantiated, it is used to
	/// save and restore settings for a block. It should be 
	/// used with using() or Disposed in a finally block.
	/// </summary>
	public class TestContext : IDisposable
	{
		#region Instance Variables
		/// <summary>
		/// The current context, head of the list of saved contexts.
		/// </summary>
		private static ContextHolder current = new ContextHolder();
		#endregion

		#region Static Methods
		public static bool Tracing
		{
			get { return current.Tracing; }
			set { current.Tracing = value; }
		}

		public static bool Logging
		{
			get { return current.Logging; }
			set { current.Logging = value; }
		}

		/// <summary>
		/// Controls where Console.Out is directed
		/// </summary>
		public static TextWriter Out
		{
			get { return current.Out; }
			set { current.Out = value; }
		}
		
		/// <summary>
		/// Controls where Console.Error is directed
		/// </summary>
		public static TextWriter Error
		{
			get { return current.Error; }
			set { current.Error = value; }
		}

		/// <summary>
		/// Controls where Trace output is directed
		/// </summary>
		public static TextWriter TraceWriter
		{
			get { return current.TraceWriter; }
			set { current.TraceWriter = value; }
		}

		public static TextWriter LogWriter
		{
			get { return current.LogWriter; }
			set { current.LogWriter = value; }
		}

		/// <summary>
		/// The current directory setting
		/// </summary>
		public static string CurrentDirectory
		{
			get { return current.CurrentDirectory; }
			set { current.CurrentDirectory = value; }
		}

		public static CultureInfo CurrentCulture
		{
			get { return current.CurrentCulture; }
			set { current.CurrentCulture = value; }
		}
		
		/// <summary>
		/// Saves the old context and makes a fresh one 
		/// current without changing any settings.
		/// </summary>
		public static void Save()
		{
			TestContext.current = new ContextHolder( current );
		}

		/// <summary>
		/// Restores the last saved context and puts
		/// any saved settings back into effect.
		/// </summary>
		public static void Restore()
		{
			current.ReverseChanges();
			current = current.prior;
		}
		#endregion

		#region Construct and Dispose
		/// <summary>
		/// The constructor saves the current context.
		/// </summary>
		public TestContext() 
		{ 
			TestContext.Save();
		}

		/// <summary>
		/// Dispose restores the old context
		/// </summary>
		public void Dispose()
		{
			TestContext.Restore();
		}
		#endregion

		#region ContextHolder internal class
		private class ContextHolder
		{
			/// <summary>
			/// Indicates whether trace is enabled
			/// </summary>
			private bool tracing;

			/// <summary>
			/// Controls whether log events are captured
			/// </summary>
			private bool logging;

			/// <summary>
			/// Destination for standard output
			/// </summary>
			private TextWriter outWriter;

			/// <summary>
			/// Destination for standard error
			/// </summary>
			private TextWriter errorWriter;

			/// <summary>
			/// Destination for Trace output
			/// </summary>
			private TextWriter traceWriter;

			/// <summary>
			/// Destination for Log output
			/// </summary>
			private TextWriter logWriter;

			/// <summary>
			/// Log4net Appender for Log output from the tests
			/// </summary>
			private log4net.Appender.TextWriterAppender log4netAppender;

			/// <summary>
			/// The current working directory
			/// </summary>
			private string currentDirectory;

			/// <summary>
			/// The current culture
			/// </summary>
			private CultureInfo currentCulture;

			/// <summary>
			/// Link to a prior saved context
			/// </summary>
			public ContextHolder prior;

			public ContextHolder()
			{
				this.prior = null;
				this.tracing = false;
				this.outWriter = Console.Out;
				this.errorWriter = Console.Error;
				this.traceWriter = null;
				this.logWriter = null;
				this.log4netAppender = null;

				this.currentDirectory = Environment.CurrentDirectory;
				this.currentCulture = CultureInfo.CurrentCulture;
			}

			public ContextHolder( ContextHolder other )
			{
				this.prior = other;
				this.tracing = other.tracing;
				this.outWriter = other.outWriter;
				this.errorWriter = other.errorWriter;
				this.traceWriter = other.traceWriter;
				this.logWriter = other.logWriter;
				this.log4netAppender = other.log4netAppender;

				this.currentDirectory = Environment.CurrentDirectory;
				this.currentCulture = CultureInfo.CurrentCulture;
			}

			/// <summary>
			/// Used to restore settings to their prior
			/// values before reverting to a prior context.
			/// </summary>
			public void ReverseChanges()
			{ 
				if ( prior == null )
					throw new InvalidOperationException( "TestContext: too many Restores" );

				this.Tracing = prior.Tracing;
				this.Out = prior.Out;
				this.Error = prior.Error;
				this.CurrentDirectory = prior.CurrentDirectory;
				this.CurrentCulture = prior.CurrentCulture;
			}

			/// <summary>
			/// Controls whether trace and debug output are written
			/// to the standard output.
			/// </summary>
			public bool Tracing
			{
				get { return tracing; }
				set 
				{
					if ( tracing != value )
					{
						if ( traceWriter != null && tracing )
							StopTracing();

						tracing = value; 

						if ( traceWriter != null && tracing )
							StartTracing();
					}
				}
			}

			/// <summary>
			/// Controls whether log output is captured
			/// </summary>
			public bool Logging
			{
				get { return logging; }
				set 
				{
					if ( logging != value )
					{
						if ( logWriter != null && logging )
							StopLogging();

						logging = value; 

						if ( logWriter != null && logging )
							StartLogging();
					}
				}
			}

			/// <summary>
			/// Controls where Console.Out is directed
			/// </summary>
			public TextWriter Out
			{
				get { return outWriter; }
				set 
				{
					if ( outWriter != value )
					{
						outWriter = value; 
						Console.Out.Flush();
						Console.SetOut( outWriter );
					}
				}
			}
		
			/// <summary>
			/// Controls where Console.Error is directed
			/// </summary>
			public TextWriter Error
			{
				get { return errorWriter; }
				set 
				{
					if ( errorWriter != value )
					{
						errorWriter = value; 
						Console.Error.Flush();
						Console.SetError( errorWriter );
					}
				}
			}

			public TextWriter TraceWriter
			{
				get { return traceWriter; }
				set
				{
					if ( traceWriter != value )
					{
						if ( traceWriter != null  && tracing )
							StopTracing();

						traceWriter = value;

						if ( traceWriter != null && tracing )
							StartTracing();
					}
				}
			}

			/// <summary>
			///  Gets or sets the Log writer, which is actually held by a log4net 
			///  TextWriterAppender. When first set, the appender will be created
			///  and will thereafter send any log events to the writer.
			///  
			///  In normal operation, LogWriter is set to an EventListenerTextWriter
			///  connected to the EventQueue in the test domain. The events are
			///  subsequently captured in the Gui an the output displayed in
			///  the Log tab. The application under test does not need to define
			///  any additional appenders.
			/// </summary>
			public TextWriter LogWriter
			{
				get { return logWriter; }
				set 
				{ 
					logWriter = value;

					if ( logWriter != null && logging )
						StartLogging();
				}
			}

			private void StopTracing()
			{
				traceWriter.Close();
				System.Diagnostics.Trace.Listeners.Remove( "NUnit" );
			}

			private void StartTracing()
			{
				System.Diagnostics.Trace.Listeners.Add( new TextWriterTraceListener( traceWriter, "NUnit" ) );
			}

			private void StartLogging()
			{
				if ( log4netAppender == null )
				{
					// Add process id to the global context for display in log entries
					log4net.GlobalContext.Properties["PID"] = System.Diagnostics.Process.GetCurrentProcess().Id;
					// Create and set up the appender
					log4netAppender = new log4net.Appender.TextWriterAppender();
					log4netAppender.Layout = new log4net.Layout.PatternLayout(
						"%date{ABSOLUTE} %-5level [%4thread] %logger{1}: PID=%property{PID} %message%newline" );
				}

				log4netAppender.Threshold = GetLoggingLevel();
				log4net.Config.BasicConfigurator.Configure(log4netAppender);
				log4netAppender.Writer = logWriter; 
			}

			private log4net.Core.Level GetLoggingLevel()
			{
				NameValueCollection settings = (NameValueCollection)
					ConfigurationSettings.GetConfig( "NUnit/TestRunner" );

				log4net.Core.Level defaultLevel = log4net.Core.Level.Error;

				if ( settings == null )
					return defaultLevel;

				string logLevel = settings["DefaultLogThreshold"];
				if ( logLevel == null )
					return defaultLevel;

				switch( logLevel.ToLower() )
				{
					case "alert":
						return log4net.Core.Level.Alert;
					case "all":
						return log4net.Core.Level.All;
					case "critical":
						return log4net.Core.Level.Critical;
					case "debug":
						return log4net.Core.Level.Debug;
					case "emergency":
						return log4net.Core.Level.Emergency;
					case "error":
						return log4net.Core.Level.Error;
					case "fatal":
						return log4net.Core.Level.Fatal;
					case "fine":
						return log4net.Core.Level.Fine;
					case "finer":
						return log4net.Core.Level.Finer;
					case "finest":
						return log4net.Core.Level.Finest;
					case "info":
						return log4net.Core.Level.Info;
					case "notice":
						return log4net.Core.Level.Notice;
					case "off":
						return log4net.Core.Level.Off;
					case "severe":
						return log4net.Core.Level.Severe;
					case "trace":
						return log4net.Core.Level.Trace;
					case "verbose":
						return log4net.Core.Level.Verbose;
					case "warn":
						return log4net.Core.Level.Warn;
					default:
						return defaultLevel;
				}
			}

			private void StopLogging()
			{
				if ( log4netAppender != null )
				{
					log4netAppender.Threshold = log4net.Core.Level.Off;
					log4netAppender.Writer = null;
				}
			}

			public string CurrentDirectory
			{
				get { return currentDirectory; }
				set
				{
					currentDirectory = value;
					Environment.CurrentDirectory = currentDirectory;
				}
			}

			public CultureInfo CurrentCulture
			{
				get { return currentCulture; }
				set
				{
					currentCulture = value;
					Thread.CurrentThread.CurrentCulture = currentCulture;
				}
			}
		}
		#endregion
	}
}
