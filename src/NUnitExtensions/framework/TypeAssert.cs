using System;

namespace NUnit.Framework.Extensions
{
	/// <summary>
	/// Summary description for TypeAssert.
	/// </summary>
	public class TypeAssert
	{
		#region IsType
		static public void IsType( System.Type expected, object actual)
		{
			IsType( expected, actual, "" );
		}

		static public void IsType( System.Type expected, object actual, string message)
		{
			IsType( expected, actual, message, null );
		}

		static public void IsType( System.Type expected, object actual, string message, params object[] args )
		{
			Assert.DoAssert( new IsTypeAsserter( expected, actual, message, args ) );
		}
		#endregion

		#region IsAssignableFrom
		static public void IsAssignableFrom( System.Type expected, object actual )
		{
			IsAssignableFrom(expected, actual, "");
		}
		static public void IsAssignableFrom( System.Type expected, object actual, string message )
		{
			IsAssignableFrom(expected, actual, message, null);
		}
		static public void IsAssignableFrom( System.Type expected, object actual, string message, params object[] args )
		{
			Assert.DoAssert( new IsAssignableFromAsserter( expected, actual, message, args ) );
		}
		#endregion

		#region IsSubclassOf
		static public void IsSubclassOf( System.Type expected, object actual )
		{
			IsSubclassOf(expected, actual, "");
		}
		static public void IsSubclassOf( System.Type expected, object actual, string message )
		{
			IsSubclassOf(expected, actual, message, null);
		}
		static public void IsSubclassOf( System.Type expected, object actual, string message, params object[] args )
		{
			Assert.DoAssert( new IsSubclassOfAsserter( expected, actual, message, args ) );
		}
		#endregion

		#region Implements
		static public void Implements( System.Type expected, object actual )
		{
			Implements(expected, actual, "");
		}
		static public void Implements( System.Type expected, object actual, string message )
		{
			Implements(expected, actual, message, null);
		}
		static public void Implements( System.Type expected, object actual, string message, params object[] args )
		{
			Assert.DoAssert( new ImplementsAsserter( expected, actual, message, args ) );
		}
		#endregion
	}
	
	#region Asserters
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
				CreateFailureMessage();
				failureMessage.AddExpectedLine( Expectation );
				failureMessage.AddActualLine( actual.GetType().ToString() );
				return failureMessage.ToString();
			}
		}

		protected virtual string Expectation
		{
			get { return expected.ToString(); }
		}
	}

	public class ImplementsAsserter : TypeAsserter
	{
		public ImplementsAsserter( System.Type expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		// TODO: Find another approach to this
		public override bool Test()
		{
			try
			{
				System.Reflection.InterfaceMapping x = actual.GetType().GetInterfaceMap(expected);
				return true;
			}
			catch( ArgumentException )	
			{
				return false;
			}
		}

		protected override string Expectation
		{
			get	{ return string.Format( "Type implementing {0}", expected ); }
		}

	}

	public class IsSubclassOfAsserter : TypeAsserter
	{
		public IsSubclassOfAsserter( System.Type expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return actual.GetType().IsSubclassOf(expected);
		}

		protected override string Expectation
		{
			get { return string.Format( "Subclass of {0}", expected ); }
		}

	}

	public class IsAssignableFromAsserter : TypeAsserter
	{
		public IsAssignableFromAsserter( System.Type expected, object actual, string message, params object[] args )
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

	public class IsTypeAsserter : TypeAsserter
	{
		public IsTypeAsserter( System.Type expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return actual.GetType().Equals(expected);
		}
	}
	#endregion
}
