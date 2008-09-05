// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System.Collections;
using System.Collections.Specialized;

namespace NUnit.Framework.Constraints
{
	/// <summary>
	/// The Constraint class is the base of all built-in constraints
    /// within NUnit. It provides the operator overloads used to combine 
    /// constraints.
	/// </summary>
    public abstract class Constraint : IConstraint
    {
        #region UnsetObject Class
        /// <summary>
        /// Class used to detect any derived constraints
        /// that fail to set the actual value in their
        /// Matches override.
        /// </summary>
        private class UnsetObject
        {
            public override string ToString()
            {
                return "UNSET";
            }
        }
        #endregion

		#region Static and Instance Fields
        /// <summary>
        /// Static UnsetObject used to detect derived constraints
        /// failing to set the actual value.
        /// </summary>
        protected static object UNSET = new UnsetObject();

		/// <summary>
		/// Dictionary containing property values set by individual constraints
		/// and used by them in evaluating the match and/or reporting results.
		/// 
		/// The following properties are currently defined:
		///   PathConstraint_IsWindows
		/// </summary>
		protected ListDictionary properties = new System.Collections.Specialized.ListDictionary();

		/// <summary>
        /// The actual value being tested against a constraint
        /// </summary>
        protected object actual = UNSET;

        /// <summary>
        /// The display name of this Constraint for use by ToString()
        /// </summary>
        private string displayName;

        /// <summary>
        /// Argument fields used by ToString();
        /// </summary>
        private readonly int argcnt;
        private readonly object arg1;
        private readonly object arg2;
        #endregion

        #region Constructors
        public Constraint()
        {
            argcnt = 0;
        }

        public Constraint(object arg)
        {
            argcnt = 1;
            this.arg1 = arg;
        }

        public Constraint(object arg1, object arg2)
        {
            argcnt = 2;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The display name of this Constraint for use by ToString()
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (displayName == null)
                {
                    displayName = this.GetType().Name.ToLower();
                    if (displayName.EndsWith("constraint"))
                        displayName = displayName.Substring(0, displayName.Length - 10);
                }

                return displayName;
            }

            set { displayName = value; }
        }
		#endregion

		#region IConstraint Members
        /// <summary>
        /// Write the failure message to the MessageWriter provided
        /// as an argument. The default implementation simply passes
        /// the constraint and the actual value to the writer, which
        /// then displays the constraint description and the value.
        /// 
        /// Constraints that need to provide additional details,
        /// such as where the error occured can override this.
        /// </summary>
        /// <param name="writer">The MessageWriter on which to display the message</param>
        public virtual void WriteMessageTo(MessageWriter writer)
        {
            writer.DisplayDifferences(this);
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public abstract bool Matches(object actual);

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public abstract void WriteDescriptionTo(MessageWriter writer);

        /// <summary>
		/// Write the actual value for a failing constraint test to a
		/// MessageWriter. The default implementation simply writes
		/// the raw value of actual, leaving it to the writer to
		/// perform any formatting.
		/// </summary>
		/// <param name="writer">The writer on which the actual value is displayed</param>
		public virtual void WriteActualValueTo(MessageWriter writer)
		{
			writer.WriteActualValue( actual );
		}
		#endregion

        #region ToString()
        public override string ToString()
        {
            switch (argcnt)
            {
                default:
                case 0:
                    return string.Format("<{0}>", DisplayName);
                case 1:
                    return string.Format("<{0} {1}>", DisplayName, _displayable(arg1));
                case 2:
                    return string.Format("<{0} {1} {2}>", DisplayName, _displayable(arg1), _displayable(arg2));
            }
        }

        private string _displayable(object o)
        {
            if (o == null)
                return "null";
            else if (o is string)
                return string.Format("\"{0}\"", o);
            else
                return o.ToString();

        }
        #endregion

        #region Operator Overloads
        /// <summary>
        /// This operator creates a constraint that is satisfied only if both 
        /// argument constraints are satisfied.
        /// </summary>
        public static Constraint operator &(Constraint left, Constraint right)
        {
            return new AndConstraint(left, right);
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied if either 
        /// of the argument constraints is satisfied.
        /// </summary>
        public static Constraint operator |(Constraint left, Constraint right)
        {
            return new OrConstraint(left, right);
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied if the 
        /// argument constraint is not satisfied.
        /// </summary>
        public static Constraint operator !(Constraint m)
        {
            return new NotConstraint(m == null ? new EqualConstraint(null) : m);
        }
        #endregion

        #region Binary Operators
        public PartialConstraintExpression And
        {
            get { return new PartialConstraintExpression().Append(this).And; }
        }

        public PartialConstraintExpression Or
        {
            get { return new PartialConstraintExpression().Append(this).Or; }
        }
        #endregion
    }
}
