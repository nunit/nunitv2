using System;

namespace NUnit.Framework
{
	#region TypeAsserter
	/// <summary>
	/// The abstract asserter from which all specific type asserters
	/// will inherit from in order to limit code-reproduction.
	/// </summary>
	public abstract class TypeAsserter : AbstractAsserter
	{
		protected System.Type   expected;
		protected object        actual;

		/// <summary>
		/// Constructor
		/// </summary>
		public TypeAsserter( System.Type expected, object actual, string message, params object[] args )
			: base( message, args ) 
		{
			this.expected = expected;
			this.actual = actual;
		}

		public override string Message
		{
			get
			{
				FailureMessage.AddExpectedLine( Expectation );
				FailureMessage.AddActualLine( actual.GetType().ToString() );
				return FailureMessage.ToString();
			}
		}

		protected virtual string Expectation
		{
			get { return expected.ToString(); }
		}
	}
	#endregion

	#region AssignableFromAsserter
	public class AssignableFromAsserter : TypeAsserter
	{
		public AssignableFromAsserter( System.Type expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return actual.GetType().IsAssignableFrom(expected);
		}

		protected override string Expectation
		{
			get { return string.Format( "Type assignable from {0}", expected ); }
		}

	}
	#endregion

	#region NotAssignableFromAsserter
	public class NotAssignableFromAsserter : TypeAsserter
	{
		public NotAssignableFromAsserter( System.Type expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return !actual.GetType().IsAssignableFrom(expected);
		}

		protected override string Expectation
		{
			get { return string.Format( "Type not assignable from {0}", expected ); }
		}

	}
	#endregion

	#region InstanceOfTypeAsserter
	public class InstanceOfTypeAsserter : TypeAsserter
	{
		public InstanceOfTypeAsserter( System.Type expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return expected.IsInstanceOfType( actual );
		}

		protected override string Expectation
		{
			get
			{
				return string.Format( "Object to be instance of {0}", expected );
			}
		}

	}
	#endregion

	#region NotInstanceOfTypeAsserter
	public class NotInstanceOfTypeAsserter : TypeAsserter
	{
		public NotInstanceOfTypeAsserter( System.Type expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return !expected.IsInstanceOfType( actual );
		}

		protected override string Expectation
		{
			get
			{
				return string.Format( "Object not an instance of {0}", expected );
			}
		}

	}
	#endregion
}
