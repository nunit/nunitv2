// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;

namespace NUnit.Framework.Constraints
{
    #region CollectionConstraint
    /// <summary>
    /// CollectionConstraint is the abstract base class for
    /// constraints that operate on collections.
    /// </summary>
    public abstract class CollectionConstraint : Constraint
    {
        /// <summary>
        /// Determine whether an expected object is contained in a collection
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        protected bool IsItemInCollection(object expected, ICollection collection)
        {
            foreach (object obj in collection)
                if ( Object.Equals( obj, expected ) )
                    return true;

            return false;
        }

        /// <summary>
        /// Determine whether one collection is a subset of another
        /// </summary>
        /// <param name="subset"></param>
        /// <param name="superset"></param>
        /// <returns></returns>
        protected bool IsSubsetOf(ICollection subset, ICollection superset)
        {
            foreach (object obj in subset)
                if (!IsItemInCollection(obj, superset))
                    return false;

            return true;
        }
    }
    #endregion

	#region AllItemsConstraint
	/// <summary>
	/// AllItemsConstraint applies another constraint to each
	/// item in a collection, succeeding if they all succeed.
	/// </summary>
	public class AllItemsConstraint : Constraint
	{
		private Constraint itemConstraint;

		/// <summary>
		/// Construct an AllItemsConstraint on top of an existing constraint
		/// </summary>
		/// <param name="itemConstraint"></param>
		public AllItemsConstraint(Constraint itemConstraint)
		{
			this.itemConstraint = itemConstraint;
		}

		/// <summary>
		/// Apply the item constraint to each item in the collection,
		/// failing if any item fails.
		/// </summary>
		/// <param name="actual"></param>
		/// <returns></returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;
			if ( actual == null || !(actual is ICollection) )
				return false;

			if ( this.caseInsensitive )
				itemConstraint = itemConstraint.IgnoreCase;
			foreach(object item in (ICollection)actual)
				if (!itemConstraint.Matches(item))
					return false;

			return true;
		}

		/// <summary>
		/// Write a description of this constraint to a MessageWriter
		/// </summary>
		/// <param name="writer"></param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WritePredicate("all items");
			itemConstraint.WriteDescriptionTo(writer);
		}
	}
	#endregion

	#region SomeItemsConstraint
	/// <summary>
	/// SomeItemsConstraint applies another constraint to each
	/// item in a collection, succeeding if any of them succeeds.
	/// </summary>
	public class SomeItemsConstraint : Constraint
	{
		private Constraint itemConstraint;

		/// <summary>
		/// Construct a SomeItemsConstraint on top of an existing constraint
		/// </summary>
		/// <param name="itemConstraint"></param>
		public SomeItemsConstraint(Constraint itemConstraint)
		{
			this.itemConstraint = itemConstraint;
		}

		/// <summary>
		/// Apply the item constraint to each item in the collection,
		/// failing if any item fails.
		/// </summary>
		/// <param name="actual"></param>
		/// <returns></returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;
			if ( actual == null || !(actual is ICollection) )
				return false;

			if ( this.caseInsensitive )
				itemConstraint = itemConstraint.IgnoreCase;
			foreach(object item in (ICollection)actual)
				if (itemConstraint.Matches(item))
					return true;

			return false;
		}

		/// <summary>
		/// Write a description of this constraint to a MessageWriter
		/// </summary>
		/// <param name="writer"></param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WritePredicate("some item");
			itemConstraint.WriteDescriptionTo(writer);
		}
	}
	#endregion

	#region UniqueItemsConstraint
    /// <summary>
    /// UniqueItemsConstraint tests whether all the items in a 
    /// collection are unique.
    /// </summary>
    public class UniqueItemsConstraint : CollectionConstraint
    {
        /// <summary>
        /// Apply the item constraint to each item in the collection,
        /// failing if any item fails.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;
            ICollection collection = actual as ICollection;
            if ( collection == null )
                return false;

            foreach (object loopObj in collection)
            {
                bool foundOnce = false;
                foreach (object innerObj in collection)
                {
                    if ( Object.Equals(loopObj,innerObj))
                    {
                        if (foundOnce)
                            return false;
                        else
                            foundOnce = true;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Write a description of this constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write("all items unique");
        }
    }
    #endregion

    #region CollectionContainsConstraint
    /// <summary>
    /// CollectionContainsConstraint is used to test whether a collection
    /// contains an expected object as a member.
    /// </summary>
    public class CollectionContainsConstraint : CollectionConstraint
    {
        private object expected;

        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public CollectionContainsConstraint(object expected)
        {
            this.expected = expected;
        }

        /// <summary>
        /// Test whether the expected item is contained in the collection
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public override bool Matches(object actual)
        {
			this.actual = actual;
            return actual != null && IsItemInCollection(expected, (ICollection)actual);
        }

        /// <summary>
        /// Write a descripton of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate( "collection containing" );
            writer.WriteExpectedValue(expected);
        }
    }
    #endregion

    #region CollecionEquivalentConstraint
    /// <summary>
    /// CollectionEquivalentCOnstraint is used to determine whether two
    /// collections are equivalent.
    /// </summary>
    public class CollectionEquivalentConstraint : CollectionConstraint
    {
        private ICollection expected;

        /// <summary>
        /// Construct a CollectionEquivalentConstraint
        /// </summary>
        /// <param name="expected"></param>
        public CollectionEquivalentConstraint(ICollection expected)
        {
            this.expected = expected;
        }

        /// <summary>
        /// Test whether two collections are equivalent
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public override bool Matches(object actual)
        {
			this.actual = actual;
            return actual is ICollection &&
                IsSubsetOf((ICollection)actual, expected) &&
                IsSubsetOf(expected, (ICollection)actual);
        }

        /// <summary>
        /// Write a description of this constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("equivalent to");
            writer.WriteExpectedValue(expected);
        }
    }
    #endregion

    #region CollectionSubsetConstraint
    /// <summary>
    /// CollectionSubsetConstraint is used to determine whether
    /// one collection is a subset of another
    /// </summary>
    public class CollectionSubsetConstraint : CollectionConstraint
    {
        private ICollection expected;

        /// <summary>
        /// Construct a CollectionSubsetConstraint
        /// </summary>
        /// <param name="superset">The collection that the actual value is expected to be a subset of</param>
        public CollectionSubsetConstraint(ICollection superset)
        {
            this.expected = superset;
        }

        /// <summary>
        /// Test whether the actual collection is a subset of 
        /// the expected collection provided.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;
            return IsSubsetOf( (ICollection)actual, expected );
        }
        
        /// <summary>
        /// Write a desicription of this constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate( "subset of" );
            writer.WriteExpectedValue(expected);
        }
    }
    #endregion
}
