// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Framework.Constraints
{
	/// <summary>
	/// Abstract base class used for prefixes
	/// </summary>
    public abstract class PrefixConstraint : Constraint
    {
        /// <summary>
        /// The base constraint
        /// </summary>
        protected Constraint baseConstraint;

        /// <summary>
        /// Construct given a base constraint
        /// </summary>
        /// <param name="resolvable"></param>
        protected PrefixConstraint(IResolveConstraint resolvable) : base(resolvable)
        {
            if ( resolvable != null )
                this.baseConstraint = resolvable.Resolve();
        }
    }
}