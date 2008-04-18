using System;

namespace NUnit.Framework.Constraints
{
    ///<summary>
    ///</summary>
    public class ThrowsConstraint : Constraint
    {
        private Type expectedExceptionType;
        private Exception caughtException;
        private Constraint furtherConstraint;

        public ThrowsConstraint(Type exceptionType)
        {
            this.expectedExceptionType = exceptionType;
        }

        public ThrowsConstraint(Type exceptionType, Constraint constraint)
        {
            this.expectedExceptionType = exceptionType;
            this.furtherConstraint = constraint;
        }

		public override bool Matches(object actual)
		{
			TestDelegate code = actual as TestDelegate;
			if (code == null)
				throw new ArgumentException("The actual value must be a TestDelegate", "actual");

			this.caughtException = Catch.Exception(code);

			return caughtException != null && expectedExceptionType == caughtException.GetType();
		}

		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.Write(string.Format("{0} to be thrown", expectedExceptionType));
		}

		public override void WriteActualValueTo(MessageWriter writer)
		{
			writer.WriteActualValue( this.caughtException.GetType() );
		}

	}

#if NET_2_0
    public class ThrowsConstraint<T> : ThrowsConstraint
    {
        public ThrowsConstraint() : base(typeof (T))
        {
        }

        public ThrowsConstraint( Constraint constraint ) : base( typeof(T), constraint )
        {
        }
    }
#endif

	public class ThrowsNothingConstraint : Constraint
	{
		private Exception caughtException;

		public override bool Matches(object actual)
		{
			TestDelegate code = actual as TestDelegate;
			if (code == null)
				throw new ArgumentException("The actual value must be a TestDelegate", "actual");

			this.caughtException = Catch.Exception(code);

			return caughtException == null;
		}

		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.Write(string.Format("No Exception to be thrown"));
		}

		public override void WriteActualValueTo(MessageWriter writer)
		{
			writer.WriteActualValue( this.caughtException.GetType() );
		}
	}
}
