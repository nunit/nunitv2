// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Diagnostics;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for Logger.
	/// </summary>
	public class InternalTrace
	{
		private readonly static string NL = Environment.NewLine;
        private static InternalTraceWriter writer;
        public static InternalTraceWriter Writer
        {
            get { return writer; }
        }

		public static TraceLevel Level = new TraceSwitch( "NTrace", "NUnit internal trace" ).Level;

        public static void Initialize(string logName, TraceLevel level)
        {
            Level = level;
            Initialize(logName);
        }

        public static void Initialize(string logName)
        {
            if (writer == null && Level > TraceLevel.Off)
            {
                writer = new InternalTraceWriter(logName);
                writer.WriteLine("InternalTrace: Initializing at level " + Level.ToString());
            }
        }

        public static void Flush()
        {
            if (writer != null)
                writer.Flush();
        }

        public static void Close()
        {
            if (writer != null)
                writer.Close();

            writer = null;
        }

        public static Logger GetLogger(string name)
		{
			return new Logger( name );
		}

		public static Logger GetLogger( Type type )
		{
			return new Logger( type.FullName );
		}

        public static void WriteLine(string message, string category)
        {
            Writer.WriteLine("{0} {1}: {2}", DateTime.Now.ToString("HH:mm:ss.fff"), category, message);
        }
    }
}
