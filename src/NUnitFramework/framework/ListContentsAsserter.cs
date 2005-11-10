using System;
using System.Collections;

namespace NUnit.Framework
{
	/// <summary>
	/// ListContentsAsserter implements an assertion that a given
	/// item is found in an array or other List.
	/// </summary>
	public class ListContentsAsserter : AbstractAsserter
	{
		private object expected;
		private IList list;

		public ListContentsAsserter( object expected, IList list, string message, params object[] args )
			: base( message, args )
		{
			this.expected = expected;
			this.list = list;
		}

		public override bool Test()
		{
			return list.Contains( expected );
		}

		public override string Message
		{
			get
			{
				FailureMessage.DisplayExpectedValue( expected );
				FailureMessage.DisplayListElements( "\t but was: ", list, 0, 5 );
				return FailureMessage.ToString();
			}
		}

	}
}
