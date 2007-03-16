// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
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
    }
}
