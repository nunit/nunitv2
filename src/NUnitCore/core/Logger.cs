using System;
using System.Diagnostics;

namespace NUnit.Core
{
    public class Logger
    {
        private string name;
        private string fullname;

        public Logger(string name)
        {
            this.fullname = this.name = name;
            int index = fullname.LastIndexOf('.');
            if (index >= 0)
                this.name = fullname.Substring(index + 1);
        }

        #region Error
        public void Error(string message)
        {
            if (InternalTrace.Level >= TraceLevel.Error)
                InternalTrace.WriteLine(message, name);
        }

        public void Error(string message, params object[] args)
        {
            if (InternalTrace.Level >= TraceLevel.Error)
                WriteFormat(message, args);
        }

        public void Error(string message, Exception ex)
        {
            if (InternalTrace.Level >= TraceLevel.Error)
            {
                InternalTrace.WriteLine(message, name);
                if ( ex != null )
				    InternalTrace.WriteLine(ex.ToString(), name);
            }
        }
        #endregion

        #region Warning
        public void Warning(string message)
        {
            if (InternalTrace.Level >= TraceLevel.Warning)
                InternalTrace.WriteLine(message, name);
        }

        public void Warning(string message, params object[] args)
        {
            if (InternalTrace.Level >= TraceLevel.Warning)
                WriteFormat(message, args);
        }
        #endregion

        #region Info
        public void Info(string message)
        {
            if (InternalTrace.Level >= TraceLevel.Info)
                InternalTrace.WriteLine(message, name);
        }

        public void Info(string message, params object[] args)
        {
            if (InternalTrace.Level >= TraceLevel.Info)
                WriteFormat(message, args);
        }
        #endregion

        #region Debug
        public void Debug(string message)
        {
            if (InternalTrace.Level >= TraceLevel.Verbose)
                InternalTrace.WriteLine(message, name);
        }

        public void Debug(string message, params object[] args)
        {
            if (InternalTrace.Level >= TraceLevel.Verbose)
                WriteFormat(message, args);
        }
        #endregion

        #region Helper Methods
        private void WriteFormat(string format, params object[] args)
        {
            string message = string.Format(format, args);
            InternalTrace.WriteLine(message, name);
        }
        #endregion
    }
}
