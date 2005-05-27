namespace NUnit.Framework.Extensions
{
	#region StringAssert
	/// <summary>
	/// Summary description for StringAssert.
	/// </summary>
	public class StringAssert
	{
		static public void Contains( string expected, string actual, string message, params object[] args )
		{
			Assert.DoAssert( new ContainsAsserter( expected, actual, message, args ) );
		}

		static public void Contains( string expected, string actual )
		{
			Contains( expected, actual, string.Empty );
		}

		static public void ContainsAny( string expected, string actual, string message, params object[] args )
		{
			Assert.DoAssert( new ContainsAnyAsserter( expected, actual, message, args ) );
		}

		static public void ContainsAny( string expected, string actual )
		{
			ContainsAny( expected, actual, string.Empty );
		}

		static public void StartsWith( string expected, string actual, string message, params object[] args )
		{
			Assert.DoAssert( new StartsWithAsserter( expected, actual, message, args ) );
		}

		static public void StartsWith( string expected, string actual )
		{
			StartsWith( expected, actual, string.Empty );
		}

		static public void EndsWith( string expected, string actual, string message, params object[] args )
		{
			Assert.DoAssert( new EndsWithAsserter( expected, actual, message, args ) );
		}

		static public void EndsWith( string expected, string actual )
		{
			EndsWith( expected, actual, string.Empty );
		}

		static public void AreEqualIgnoringCase( string expected, string actual, string message, params object[] args )
		{
			Assert.DoAssert( new EqualIgnoringCaseAsserter( expected, actual, message, args ) );
		}

		static public void AreEqualIgnoringCase( string expected, string actual )
		{
			AreEqualIgnoringCase( expected, actual, string.Empty );
		}
	}
	#endregion

	#region StringAsserter
	/// <summary>
	/// Summary description for StringAsserter.
	/// </summary>
	public abstract class StringAsserter : AbstractAsserter
	{
		protected string expected;
		protected string actual;

		public StringAsserter( string expected, string actual, string message, params object[] args )
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
				failureMessage.DisplayActualValue( actual );
				return failureMessage.ToString();
			}
		}

		protected virtual string Expectation
		{
			get { return string.Format( "<\"{0}\">", expected ); }
		}
	}
	#endregion

	#region ContainsAsserter
	/// <summary>
	/// Summary description for ContainsAsserter.
	/// </summary>
	public class ContainsAsserter : StringAsserter
	{
		public ContainsAsserter( string expected, string actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return actual.IndexOf( expected ) >= 0;
		}

		protected override string Expectation
		{
			get { return string.Format( "String containing \"{0}\"", expected ); }
		}
	}
	#endregion

	#region ContainsAnyAsserter
	/// <summary>
	/// Summary description for ContainsAnyAsserter.
	/// </summary>
	public class ContainsAnyAsserter : StringAsserter
	{
		public ContainsAnyAsserter( string expected, string actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return actual.IndexOfAny( expected.ToCharArray() ) >= 0;
		}

		protected override string Expectation
		{
			get { return string.Format( "String containing any of \"{0}\"", expected ); }
		}
	}
	#endregion

	#region StartsWithAsserter
	/// <summary>
	/// Summary description for StartsWithAsserter.
	/// </summary>
	public class StartsWithAsserter : StringAsserter
	{
		public StartsWithAsserter( string expected, string actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return actual.StartsWith( expected );
		}

		protected override string Expectation
		{
			get { return string.Format( "String starting with \"{0}\"", expected ); }
		}
	}
	#endregion

	#region EndsWithAsserter
	/// <summary>
	/// Summary description for EndsWithAsserter.
	/// </summary>
	public class EndsWithAsserter : StringAsserter
	{
		public EndsWithAsserter( string expected, string actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return actual.EndsWith( expected );
		}

		protected override string Expectation
		{
			get { return string.Format( "String ending with \"{0}\"", expected ); }
		}
	}
	#endregion

	#region EqualIgnoringCaseAsserter
	public class EqualIgnoringCaseAsserter : StringAsserter
	{
		public EqualIgnoringCaseAsserter( string expected, string actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return string.Compare( expected, actual, true ) == 0;
		}

		public override string Message
		{
			get
			{
				CreateFailureMessage().DisplayDifferences( expected, actual, true );
				return failureMessage.ToString();
			}
		}
	}
	#endregion
}
