using System;
using System.Runtime.Serialization;

namespace NUnit.Core
{
    /// <summary>
    /// Exception raised when loading a test assembly using
    /// an unsupported version of the nunit framework
    /// </summary>
    [Serializable]
    public class UnsupportedFrameworkException : ApplicationException
    {
        #region Constructors

        public UnsupportedFrameworkException(string message)
            : base(message) { }

        protected UnsupportedFrameworkException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        #endregion
    }
}