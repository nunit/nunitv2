using System;
using NUnit.Framework;

namespace NUnit.Extensions
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

		#region Asserters
		private class ImplementsAsserter : TypeAsserter
		{
			public ImplementsAsserter( System.Type expected, object actual, string message, params object[] args )
				: base( expected, actual, message, args ) { }

			public override void Assert()
			{
				try
				{
					System.Reflection.InterfaceMapping x = actual.GetType().GetInterfaceMap(expected);
				}
				catch( ArgumentException )	
				{
					Fail();
				}
			}
		}

		private class IsSubclassOfAsserter : TypeAsserter
		{
			public IsSubclassOfAsserter( System.Type expected, object actual, string message, params object[] args )
				: base( expected, actual, message, args ) { }

			public override void Assert()
			{
				if ( !actual.GetType().IsSubclassOf(expected) )
					Fail();
			}
		}

		private class IsAssignableFromAsserter : TypeAsserter
		{
			public IsAssignableFromAsserter( System.Type expected, object actual, string message, params object[] args )
				: base( expected, actual, message, args ) { }

			public override void Assert()
			{
				if ( !actual.GetType().IsAssignableFrom(expected) )
					Fail();
			}
		}

		private class IsTypeAsserter : TypeAsserter
		{
			public IsTypeAsserter( System.Type expected, object actual, string message, params object[] args )
				: base( expected, actual, message, args ) { }

			public override void Assert()
			{
				if ( !actual.GetType().Equals(expected) )
					Fail();
			}
		}

		/// <summary>
		/// The abstract asserter from which all specific type asserters
		/// will inherit from in order to limit code-reproduction.
		/// </summary>
		private abstract class TypeAsserter : AbstractAsserter
		{
			protected System.Type   expected;
			protected object        actual;

			public TypeAsserter( System.Type expected, object actual, string message, params object[] args )
				: base( message, args ) 
			{
				this.expected = expected;
				this.actual = actual;
			}

			protected virtual void Fail()
			{
				AssertionFailureMessage msg = new AssertionFailureMessage( message, args );
				msg.DisplayExpectedAndActual( expected.ToString(), actual.GetType().ToString() );
				throw new AssertionException( msg.ToString() );
			}
		}
		#endregion
	}
}
