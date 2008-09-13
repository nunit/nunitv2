// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// ConstraintModifier is the abstract base class for
    /// all constraint modifiers - objects that represent
    /// the current constraint in the syntax, allowing
    /// modifier suffixes to be applied, even when the
    /// constraint has been wrapped by another constraint.
    /// </summary>
    public abstract class ConstraintModifier : IConstraint
    {
        private Constraint baseConstraint;
        private ConstraintExpression builder;

        /// <summary>
        /// Construct a modifier for Constraint
        /// </summary>
        /// <param name="constraint">The constraint to be wrapped</param>
        public ConstraintModifier(Constraint constraint, ConstraintExpression builder)
        {
            this.baseConstraint = constraint;
            this.builder = builder;
        }

        #region IConstraint Members
        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public bool Matches(object actual)
        {
            return ((IConstraint)builder).Matches(actual);
        }

        /// <summary>
        /// Write the failure message to a MessageWriter.
        /// </summary>
        /// <param name="writer">The MessageWriter on which to display the message</param>
        public void WriteMessageTo(MessageWriter writer)
        {
            ((IConstraint)builder).WriteMessageTo(writer);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public void WriteDescriptionTo(MessageWriter writer)
        {
            ((IConstraint)builder).WriteDescriptionTo(writer);
        }

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
        public void WriteActualValueTo(MessageWriter writer)
        {
            ((IConstraint)builder).WriteActualValueTo(writer);
        }
        #endregion

        #region Binary Operators
        public PartialConstraintExpression And
        {
            get { return builder.And; }
        }

        public PartialConstraintExpression Or
        {
            get { return builder.Or; }
        }
        #endregion

        #region ToString()
        public override string ToString()
        {
            return builder.ToString();
        }
        #endregion
    }

}
