using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// Ignore Decorator is an alternative method of marking tests to
	/// be ignored. It is currently not used, since the test builders
	/// take care of the ignore attribute.
	/// </summary>
	public class IgnoreDecorator : ITestDecorator
	{
		private string ignoreAttributeType;

		public IgnoreDecorator( string ignoreAttributeType )
		{
			this.ignoreAttributeType = ignoreAttributeType;
		}

		#region ITestDecorator Members

		public TestCase Decorate(TestCase testCase, MethodInfo method)
		{
			return (TestCase)DecorateTest( testCase, method );
		}

		public TestSuite Decorate(TestSuite suite, Type fixtureType)
		{
			return (TestSuite)DecorateTest( suite, fixtureType );
		}

		private Test DecorateTest( Test test, MemberInfo member )
		{
			Attribute ignoreAttribute = NUnitFramework.GetIgnoreAttribute( member );

			if ( ignoreAttribute != null )
			{
				test.RunState = RunState.Ignored;
				test.IgnoreReason = NUnitFramework.GetIgnoreReason( ignoreAttribute );
			}

			return test;
		}

		#endregion
	}
}
