using System;

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// ThrowsConstraint is used to test the exception thrown by 
    /// a delegate. It checks the type of the exception and may
    /// optionally apply additional constraints to it.
    /// </summary>
    public class ThrowsConstraint : Constraint
    {
        private Type expectedExceptionType;
        private Exception caughtException;
        private Constraint furtherConstraint;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ThrowsConstraint"/> class,
        /// using the Type of exception that is expected.
        /// </summary>
        /// <param name="exceptionType">The Type of the expected exception.</param>
        public ThrowsConstraint(Type exceptionType)
        {
            this.expectedExceptionType = exceptionType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ThrowsConstraint"/> class,
        /// using the Type of the expected exception and an additional constraint
        /// to be applied to the exception.
        /// </summary>
        /// <param name="exceptionType">The Type of the expected exception.</param>
        /// <param name="constraint">An additional constraint to be applied to the exception.</param>
        public ThrowsConstraint(Type exceptionType, Constraint constraint)
        {
            this.expectedExceptionType = exceptionType;
            this.furtherConstraint = constraint;
        }

        /// <summary>
        /// Executes the code of the delegate and captures any exception.
        /// Tests the type of the exception and whether any added constraint 
        /// is satisfied by it.
        /// </summary>
        /// <param name="actual">A delegate representing the code to be tested</param>
        /// <returns>True if an exception of the specified Type is thrown and the constraint succeeds, otherwise false</returns>
		public override bool Matches(object actual)
		{
			TestDelegate code = actual as TestDelegate;
			if (code == null)
				throw new ArgumentException("The actual value must be a TestDelegate", "actual");

			this.caughtException = Catch.Exception(code);

			if( caughtException == null ||
                expectedExceptionType != caughtException.GetType())
                    return false;

            return furtherConstraint == null  || furtherConstraint.Matches(caughtException);
		}

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
            writer.WriteExpectedValue( expectedExceptionType );
            if ( furtherConstraint != null )
            {
                writer.WriteConnector( "with");
                furtherConstraint.WriteDescriptionTo(writer);
            }
		}

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. The default implementation simply writes
        /// the raw value of actual, leaving it to the writer to
        /// perform any formatting.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
		public override void WriteActualValueTo(MessageWriter writer)
		{
            writer.WriteActualValue(caughtException == null ? null : caughtException.GetType());

            if( caughtException != null  && 
                caughtException.GetType() == expectedExceptionType &&
                furtherConstraint != null )
            {
                writer.WriteConnector("with");
                furtherConstraint.WriteActualValueTo(writer);
            }
		}

	}

#if NET_2_0
    /// <summary>
    /// ThrowsConstraint&lt;T&gt; provides a convenient notation creating
    /// a ThrowsConstraint under .NET 2.0. It provides no added function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThrowsConstraint<T> : ThrowsConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ThrowsConstraint&lt;T&gt;"/> class
        /// using the Type of the expected exception.
        /// </summary>
        public ThrowsConstraint()
            : base(typeof(T))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ThrowsConstraint&lt;T&gt;"/> class,
        /// using the Type of the expected exception and an additional constraint
        /// to be applied to the exception.
        /// </summary>
        /// <param name="constraint">An additional constraint to be applied to the exception.</param>
        public ThrowsConstraint(Constraint constraint)
            : base(typeof(T), constraint)
        {
        }
    }
#endif

    /// <summary>
    /// ThrowsNothingConstraint tests that a delegate does not
    /// throw an exception.
    /// </summary>
	public class ThrowsNothingConstraint : Constraint
	{
		private Exception caughtException;

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True if no exception is thrown, otherwise false</returns>
		public override bool Matches(object actual)
		{
			TestDelegate code = actual as TestDelegate;
			if (code == null)
				throw new ArgumentException("The actual value must be a TestDelegate", "actual");

			this.caughtException = Catch.Exception(code);

			return caughtException == null;
		}

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.Write(string.Format("No Exception to be thrown"));
		}

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. The default implementation simply writes
        /// the raw value of actual, leaving it to the writer to
        /// perform any formatting.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
		public override void WriteActualValueTo(MessageWriter writer)
		{
			writer.WriteActualValue( this.caughtException.GetType() );
		}
	}
}
