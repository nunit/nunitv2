using System;
using System.ComponentModel;
using NUnit.Framework.Constraints;

namespace NUnit.Framework
{
    /// <summary>
    /// Delegate used by tests that execute code and
    /// capture any thrown exception.
    /// </summary>
    public delegate void TestDelegate();

    /// <summary>
    /// Provides a base for Assert, Assume and any other
    /// additional classes that have static methods for
    /// applying constraints and reporting the result.
    /// 
    /// Unfortunately, since the derived class are
    /// all static, we can't put much in this base.
    /// </summary>
    public class AssertBase
    {
        #region Assert Counting

        private static int counter = 0;

        /// <summary>
        /// Gets the number of assertions executed so far and 
        /// resets the counter to zero.
        /// </summary>
        public static int Counter
        {
            get
            {
                int cnt = counter;
                counter = 0;
                return cnt;
            }
        }

        protected static void IncrementAssertCount()
        {
            ++counter;
        }

        #endregion

        #region Equals and ReferenceEquals

        /// <summary>
        /// The Equals method throws an AssertionException. This is done 
        /// to make sure there is no mistake by calling this function.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object a, object b)
        {
            // TODO: This should probably be InvalidOperationException
            throw new AssertionException("Assert.Equals should not be used for Assertions");
        }

        /// <summary>
        /// override the default ReferenceEquals to throw an AssertionException. This 
        /// implementation makes sure there is no mistake in calling this function 
        /// as part of Assert. 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static new void ReferenceEquals(object a, object b)
        {
            throw new AssertionException("Assert.ReferenceEquals should not be used for Assertions");
        }

        #endregion
    }
}
