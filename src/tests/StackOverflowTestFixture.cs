using System;
using NUnit.Framework;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for StackOverflowTestFixture.
	/// </summary>
	[TestFixture]
	public class StackOverflowTestFixture
	{
		private void FunctionCallsSelf()
		{
			FunctionCallsSelf();
		}

		[Test, ExpectedException( typeof( StackOverflowException ) )]
		public void SimpleOverflow()
		{
			FunctionCallsSelf();
		}
	}
}
