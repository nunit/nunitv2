// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using NUnit.Framework;
using System.Runtime.InteropServices;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for StackOverflowTestFixture.
	/// </summary>
	[TestFixture, Platform(Exclude="Net-2.0,Mono",Reason="Cannot handle StackOverflowException in managed code")]
	public class StackOverflowTestFixture
	{
		private void FunctionCallsSelf()
		{
			FunctionCallsSelf();
		}

		[Test, ExpectedException( typeof( StackOverflowException ) )]
		[Platform(Exclude="Net-2.0,Mono",Reason="Cannot handle StackOverflowException in managed code")]
		public void SimpleOverflow()
		{
            FunctionCallsSelf();
        }
	}
}
