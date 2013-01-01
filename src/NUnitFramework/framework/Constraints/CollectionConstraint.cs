// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.Collections;

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// CollectionConstraint is the abstract base class for
    /// constraints that operate on collections.
    /// </summary>
    public abstract class CollectionConstraint : Constraint
    {
        /// <summary>
        /// Construct an empty CollectionConstraint
        /// </summary>
        protected CollectionConstraint() { }

        /// <summary>
        /// Construct a CollectionConstraint
        /// </summary>
        /// <param name="arg"></param>
        protected CollectionConstraint(object arg) : base(arg) { }

        /// <summary>
        /// Determines whether the specified enumerable is empty.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>
        /// 	<c>true</c> if the specified enumerable is empty; otherwise, <c>false</c>.
        /// </returns>
		protected static bool IsEmpty( IEnumerable enumerable )
		{
			ICollection collection = enumerable as ICollection;
			if ( collection != null )
				return collection.Count == 0;

            // NOTE: Ignore unsuppressed warning about o in .NET 1.1 build
            foreach (object o in enumerable)
                return false;

            return true;
        }

		/// <summary>
		/// Test whether the constraint is satisfied by a given value
		/// </summary>
		/// <param name="actual">The value to be tested</param>
		/// <returns>True for success, false for failure</returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;

			IEnumerable enumerable = actual as IEnumerable;
			if ( enumerable == null )
				throw new ArgumentException( "The actual value must be an IEnumerable", "actual" );
		
			return doMatch( enumerable );
		}

		/// <summary>
		/// Protected method to be implemented by derived classes
		/// </summary>
		/// <param name="collection"></param>
		/// <returns></returns>
		protected abstract bool doMatch(IEnumerable collection);
    }
}
