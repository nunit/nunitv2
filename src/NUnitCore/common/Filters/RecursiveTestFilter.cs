using System;
using System.Collections;
using System.Text;

namespace NUnit.Core.Filters
{
    /// <summary>
    /// RecursiveTestFilter is an abstract base class for filters
    /// which pass if either the test itself, a parent test or a
    /// descendendant satisfies the criterion for passing.
    /// </summary>
    [Serializable]
    public abstract class RecursiveTestFilter : TestFilter
    {
        /// <summary>
        /// Test the filter on a given test node
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public override bool Pass(ITest test)
        {
            if (Match(test))
                return true;

            if (MatchParent(test))
                return true;

            if (MatchDescendant(test))
                return true;

            return false;
        }

        public abstract bool Match(ITest test);

        private bool MatchParent(ITest test)
        {
            if (test.IsExplicit)
                return false;

            for (ITest parent = test.Parent; parent != null; parent = parent.Parent)
            {
                if (Match(parent))
                    return true;

                // Don't proceed past a parent marked Explicit
                if (parent.IsExplicit)
                    return false;
            }

            return false;
        }

        private bool MatchDescendant(ITest test)
        {
            if (!test.IsSuite || test.Tests == null)
                return false;

            foreach (ITest child in test.Tests)
            {
                if (Match(child))
                    return true;

                if (MatchDescendant(child))
                    return true;
            }

            return false;
        }
    }
}
