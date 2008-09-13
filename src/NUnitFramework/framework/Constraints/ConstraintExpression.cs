// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// ConstraintExpression is one of the classes used to represent
    /// a constraint in the process of being constructed from a series 
    /// of syntactic elements. 
    /// 
    /// Specifically, ConstraintExpression represents an expression
    /// that is complete in itself. It can be resolved to an actual
    /// constraint and, in fact, implements the IConstraint interface.
    /// A ConstraintExpression may still be extended by appending a 
    /// binary operator, which creates a PartialConstraintExpression.
    /// </summary>
    public class ConstraintExpression : IConstraint
    {
        private ConstraintBuilder builder;

        public ConstraintExpression(ConstraintBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Resolves the builder to a constraint by applying
        /// all pending operators and operands from the stack.
        /// The result is cached for further calls, and cleared
        /// when any further pushes are done.
        /// </summary>
        /// <returns>A constraint that incorporates all pending operators</returns>
        private Constraint resolvedConstraint = null;
        public Constraint Resolve()
        {
            if (resolvedConstraint == null)
                resolvedConstraint = builder.Resolve();

            return resolvedConstraint;
        }

        #region And
        public PartialConstraintExpression And
        {
            get
            {
                builder.Append(new AndOperator());
                return new PartialConstraintExpression(builder);
            }
        }
        #endregion

        #region With
        /// <summary>
        /// With is equivalent to And after a completed expression
        /// </summary>
        public PartialConstraintExpression With
        {
            get
            {
                builder.Append(new AndOperator());
                return new PartialConstraintExpression(builder);
            }
        }
        #endregion

        #region Or
        public PartialConstraintExpression Or
        {
            get
            {
                builder.Append(new OrOperator());
                return new PartialConstraintExpression(builder);
            }
        }
        #endregion

        #region Operator Overloads
        /// <summary>
        /// This operator creates a constraint that is satisfied only if both 
        /// argument constraints are satisfied.
        /// </summary>
        public static Constraint operator &(ConstraintExpression left, ConstraintExpression right)
        {
            return new AndConstraint(left.Resolve(), right.Resolve());
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied only if both 
        /// argument constraints are satisfied.
        /// </summary>
        public static Constraint operator &(ConstraintExpression left, Constraint right)
        {
            return new AndConstraint(left.Resolve(), right);
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied only if both 
        /// argument constraints are satisfied.
        /// </summary>
        public static Constraint operator &(Constraint left, ConstraintExpression right)
        {
            return new AndConstraint(left, right.Resolve());
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied if either 
        /// of the argument constraints is satisfied.
        /// </summary>
        public static Constraint operator |(ConstraintExpression left, ConstraintExpression right)
        {
            return new OrConstraint(left.Resolve(), right.Resolve());
            //left.builder.Push(new OrOperator());
            //left.builder.Push(right.Resolve());
            //return left;
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied if the 
        /// argument constraint is not satisfied.
        /// </summary>
        public static Constraint operator !(ConstraintExpression m)
        {
            return new NotConstraint(m == null ? new EqualConstraint(null) : m.Resolve());
        }
        #endregion

        public override string ToString()
        {
            return Resolve().ToString();
        }

        #region IConstraint Members
        bool IConstraint.Matches(object actual)
        {
            return Resolve().Matches(actual);
        }

        void IConstraint.WriteMessageTo(MessageWriter writer)
        {
            Resolve().WriteMessageTo(writer);
        }

        void IConstraint.WriteDescriptionTo(MessageWriter writer)
        {
            Resolve().WriteDescriptionTo(writer);
        }

        void IConstraint.WriteActualValueTo(MessageWriter writer)
        {
            Resolve().WriteActualValueTo(writer);
        }
        #endregion
    }
}
