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

		[Test, Priority(5)]
		public void Test3() { }
	}

	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method|AttributeTargets.Assembly, AllowMultiple=true)]
	public class PriorityAttribute : PropertyAttribute
	{
		public PriorityAttribute( int level ) : base( level ) { }
	}
}
