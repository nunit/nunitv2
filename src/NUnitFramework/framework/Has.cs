// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework.Constraints;

namespace NUnit.Framework
{
    /// <summary>
    /// With is essentially a synonym for has
    /// </summary>
    public class With : Has { }

	/// <summary>
	/// Summary description for Has.
	/// </summary>
	public class Has
	{
        /// <summary>
        /// Nested class that allows us to restrict the number
        /// of key words that may appear after Has.No.
        /// </summary>
		public class HasNoPrefixBuilder
		{
            /// <summary>
            /// Return a Constraint that succeeds if the actual object
            /// does not have the specified property.
            /// </summary>
            /// <param name="name">The property name</param>
            /// <returns>A NotConstraint wrapping a PropertyExistsConstraint</returns>
			public NotConstraint Property(string name)
			{
				return new NotConstraint( new PropertyExistsConstraint (name) );
			}

            /// <summary>
            /// Return a Constraint that succeeds if the expected object is
            /// not contained in a collection.
            /// </summary>
            /// <param name="expected">The expected object</param>
            /// <returns>A NotConstraint wrapping a CollectionContainsConstraint</returns>
            public NotConstraint Member(object expected)
			{
				return new NotConstraint( new CollectionContainsConstraint(expected) ) ;
			}
		}

		#region Prefix Operators
		/// <summary>
		/// Has.No returns a ConstraintBuilder that negates
		/// the constraint that follows it.
		/// </summary>
		public static HasNoPrefixBuilder No
		{
			get { return new HasNoPrefixBuilder(); }
		}

		/// <summary>
		/// Has.AllItems returns a ConstraintBuilder, which will apply
		/// the following constraint to all members of a collection,
		/// succeeding if all of them succeed.
		/// </summary>
		public static ConstraintExpression All
		{
			get { return new ConstraintExpression().All; }
		}

		/// <summary>
		/// Has.Some returns a ConstraintBuilder, which will apply
		/// the following constraint to all members of a collection,
		/// succeeding if any of them succeed. It is a synonym
		/// for Has.Item.
		/// </summary>
		public static ConstraintExpression Some
		{
			get { return new ConstraintExpression().Some; }
		}

		/// <summary>
		/// Has.None returns a ConstraintBuilder, which will apply
		/// the following constraint to all members of a collection,
		/// succeeding only if none of them succeed.
		/// </summary>
		public static ConstraintExpression None
		{
			get { return new ConstraintExpression().None; }
		}

        /// <summary>
        /// Returns a new ConstraintBuilder, which will apply the
        /// following constraint to a named property of the object
        /// being tested.
        /// </summary>
        /// <param name="name">The name of the property</param>
		public static PropertyConstraintExpression Property( string name )
		{
            return new PropertyConstraintExpression(name);
		}

        /// <summary>
        /// Returns a new ConstraintBuilder, which will apply the
        /// following constraint to the Length property of the object
        /// being tested.
        /// </summary>
        public static PropertyConstraintExpression Length
        {
            get { return Property("Length"); }
        }

        /// <summary>
        /// Returns a new ConstraintBuilder, which will apply the
        /// following constraint to the Count property of the object
        /// being tested.
        /// </summary>
        public static PropertyConstraintExpression Count
        {
            get { return Property("Count");  }
        }

        /// <summary>
        /// Returns a new ConstraintBuilder, which will apply the
        /// following constraint to the Message property of the object
        /// being tested.
        /// </summary>
        public static PropertyConstraintExpression Message
        {
            get { return Property("Message"); }
        }
        #endregion

		#region Member Constraint
		/// <summary>
		/// Returns a new CollectionContainsConstraint checking for the
		/// presence of a particular object in the collection.
		/// </summary>
		/// <param name="expected">The expected object</param>
		public static CollectionContainsConstraint Member( object expected )
		{
			return new CollectionContainsConstraint( expected );
		}
		#endregion

        #region Attribute Constraint
        /// <summary>
        /// Returns a new AttributeConstraint checking for the
        /// presence of a particular attribute on an object.
        /// </summary>
        /// <param name="type">The expected attribute type</param>
        public static AttributeConstraint Attribute(Type type)
        {
            return new AttributeConstraint(type);
        }

#if NET_2_0
        /// <summary>
        /// Returns a new AttributeConstraint checking for the
        /// presence of a particular attribute on an object.
        /// </summary>
        /// <param name="type">The expected object</param>
        /// <typeparam name="T">The expected attribute type</typeparam>
        public static AttributeConstraint Attribute<T>()
        {
            return Attribute( typeof(T) );
        }
#endif
        #endregion
    }
}
