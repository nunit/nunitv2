// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;

namespace NUnit.Framework.Constraints
{
    enum Op
    {
        Not,
        All
    }

    /// <summary>
    /// ConstraintBuilder is used to resolve the Not and All properties,
    /// which serve as prefix operators for constraints. With the addition
    /// of an operand stack, And and Or could be supported, but we have
    /// left them out in favor of a simpler, more type-safe implementation.
    /// Use the &amp; and | operator overloads to combine constraints.
    /// </summary>
    public class ConstraintBuilder
    {
        Stack ops = new Stack();

        #region Constraints Without Arguments
        public Constraint Null
        {
            get { return Resolve(Is.Null); }
        }

        public Constraint True
        {
            get { return Resolve(Is.True); }
        }

        public Constraint False
        {
            get { return Resolve(Is.False); }
        }

        public Constraint NaN
        {
            get { return Resolve(Is.NaN); }
        }

        public Constraint Empty
        {
            get { return Resolve(Is.Empty); }
        }
        #endregion

        #region Constraints with an expected value

        #region Equality and Identity
        public Constraint EqualTo(object expected)
        {
            return Resolve(Is.EqualTo(expected));
        }

        public Constraint SameAs(object expected)
        {
            return Resolve(Is.SameAs(expected));
        }
        #endregion

        #region Comparison Constraints
        public Constraint LessThan(IComparable expected)
        {
            return Resolve(Is.LessThan(expected));
        }

        public Constraint GreaterThan(IComparable expected)
        {
            return Resolve(Is.GreaterThan(expected));
        }

        public Constraint AtMost(IComparable expected)
        {
            return Resolve( Is.AtMost(expected));
        }

        public Constraint AtLeast(IComparable expected)
        {
            return Resolve(Is.AtLeast(expected));
        }
        #endregion

        #region Type Constraints
        public Constraint Type(Type expectedType)
        {
            return Resolve(Is.Type(expectedType));
        }

        public Constraint InstanceOfType(Type expectedType)
        {
            return Resolve(Is.InstanceOfType(expectedType));
        }

        public Constraint AssignableFrom(Type expectedType)
        {
            return Resolve(Is.AssignableFrom(expectedType));
        }
        #endregion

        #region String Constraints
        public Constraint StringContaining(string substring)
        {
            return Resolve( new SubstringConstraint(substring) );
        }

        public Constraint StringStarting(string substring)
        {
            return Resolve( new StartsWithConstraint(substring) );
        }

        public Constraint StringEnding(string substring)
        {
            return Resolve( new EndsWithConstraint(substring) );
        }

        public Constraint StringMatching(string pattern)
        {
            return Resolve(new RegexConstraint(pattern));
        }
        #endregion

        #region Collection Constraints
        public Constraint EquivalentTo(ICollection expected)
        {
            return Resolve( new CollectionEquivalentConstraint(expected) );
        }

		public Constraint CollectionContaining(object expected)
		{
			return Resolve( new CollectionContainsConstraint(expected) );
		}

        public Constraint SubsetOf(ICollection expected)
        {
            return Resolve(new CollectionSubsetConstraint(expected));
        }
        #endregion

        #endregion

        #region Prefix Operators
        public ConstraintBuilder Not
        {
            get
            {
                ops.Push(Op.Not);
                return this;
            }
        }

        public ConstraintBuilder All
        {
            get
            {
                ops.Push(Op.All);
                return this;
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Resolve a constraint that has been recognized by applying
        /// any pending operators and returning the resulting Constraint.
        /// </summary>
        /// <param name="constraint">The root constraint</param>
        /// <returns>A constraint that incorporates all pending operators</returns>
        private Constraint Resolve(Constraint constraint)
        {
            while (ops.Count > 0)
                switch ((Op)ops.Pop())
                {
                    case Op.Not:
                        constraint = new NotConstraint(constraint);
                        break;
                    case Op.All:
                        constraint = new AllItemsConstraint(constraint);
                        break;
                }

            return constraint;
        }
        #endregion
    }
}
