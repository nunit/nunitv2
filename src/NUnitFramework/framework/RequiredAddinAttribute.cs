using System;

namespace NUnit.Framework
{
    /// <summary>
    /// RequiredAddinAttribute may be used to indicate the names of any addins
    /// that must be present in order to run a given test. It may be applied
    /// at the method, class or assembly level. If the addin is not loaded,
    /// the test in question is marked as NotRunnable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method,AllowMultiple=true)]
    public class RequiredAddinAttribute : Attribute
    {
        private string requiredAddin;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RequiredAddinAttribute"/> class.
        /// </summary>
        /// <param name="requiredAddin">The required addin.</param>
        public RequiredAddinAttribute(string requiredAddin)
        {
            this.requiredAddin = requiredAddin;
        }

        /// <summary>
        /// Gets the name of required addin.
        /// </summary>
        /// <value>The required addin name.</value>
        public string RequiredAddin
        {
            get { return requiredAddin; }
        }
    }
}
