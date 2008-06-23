using System;
using System.Text;

namespace NUnit.Framework
{
    /// <summary>
    /// The Catch class is used to capture an exception.
    /// </summary>
    public class Catch
    {
        /// <summary>
        /// Capture any exception that is thrown by the delegate
        /// </summary>
        /// <param name="code">A TestDelegate</param>
        /// <returns>The exception thrown, or null</returns>
        public static Exception Exception(TestDelegate code)
        {
            try
            {
                code();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
