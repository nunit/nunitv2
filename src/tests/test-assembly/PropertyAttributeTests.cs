using System;
using NUnit.Framework;

namespace NUnit.TestData.PropertyAttributeTests
{
	[TestFixture, Property("ClassUnderTest","SomeClass" )]
	public class FixtureWithProperties
	{
		[Test, Property("user","Charlie")]
		public void Test1() { }

		[Test, Property("X",10.0), Property("Y",17.0)]
		public void Test2() { }
	}
}
