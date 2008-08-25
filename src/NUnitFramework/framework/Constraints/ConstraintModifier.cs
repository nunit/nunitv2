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
        private IConstraint wrapperConstraint;
        private Constraint baseConstraint;

        /// <summary>
        /// Construct a modifier for Constraint
        /// </summary>
        /// <param name="constraint">The constraint to be wrapped</param>
        public ConstraintModifier(Constraint constraint)
        {
            this.baseConstraint = constraint;
        }

        /// <summary>
        /// Internal property used to get the outermost
        /// wrapping constraint. Delayed evaluation is 
        /// used so that the value is not resolved until
        /// Match is called.
        /// </summary>
        private IConstraint WrapperConstraint
        {
            get
            {
                if (wrapperConstraint == null)
                {
                    wrapperConstraint = baseConstraint;

                    ResolvableConstraintBuilder builder = baseConstraint.Builder as ResolvableConstraintBuilder;
                    if (builder == null && baseConstraint.Builder != null)
                        builder = new ResolvableConstraintBuilder(baseConstraint.Builder);
                    
                    if (builder != null)
                        wrapperConstraint = builder.Constraint;
                }

                return wrapperConstraint;
            }
        }

        #region IConstraint Members
        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public bool Matches(object actual)
        {
            return WrapperConstraint.Matches(actual);
        }

        /// <summary>
        /// Write the failure message to a MessageWriter.
        /// </summary>
        /// <param name="writer">The MessageWriter on which to display the message</param>
        public void WriteMessageTo(MessageWriter writer)
        {
            WrapperConstraint.WriteMessageTo(writer);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public void WriteDescriptionTo(MessageWriter writer)
        {
            WrapperConstraint.WriteDescriptionTo(writer);
        }

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
        public void WriteActualValueTo(MessageWriter writer)
        {
            WrapperConstraint.WriteActualValueTo(writer);
        }
        #endregion

        #region Binary Operators
        public ConstraintBuilder And
        {
            get 
            {
                ConstraintBuilder builder = baseConstraint.Builder;
                if (builder == null)
                    builder = new ConstraintBuilder(baseConstraint);

                return new ResolvableConstraintBuilder( builder ).And;
            }
        }

        public ConstraintBuilder Or
        {
            get 
            {
                ConstraintBuilder builder = baseConstraint.Builder;
                if (builder == null)
                    builder = new ConstraintBuilder(baseConstraint);

                return new ResolvableConstraintBuilder( builder ).Or;
            }
        }
        #endregion

        public override string ToString()
        {
            return WrapperConstraint.ToString();
        }
    }

}
