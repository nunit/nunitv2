using System;
using NUnit.Framework;

namespace NUnit.Tests
{
	[TestFixture, Ignore]
	public class IgnoredFixture
	{
		[Test]
		public void Test1() { }

		[Test]
		public void Test2() { }
		
		[Test]
		public void Test3() { }
	}
}
