using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for IgnoreDecorator.
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
			Attribute ignoreAttribute = Reflect.GetAttribute( member, ignoreAttributeType, false );

			if ( ignoreAttribute != null )
			{
				test.ShouldRun = false;
				test.IgnoreReason = (string)Reflect.GetPropertyValue(
					ignoreAttribute, 
					"Reason",
					BindingFlags.Public | BindingFlags.Instance );
			}

			return test;
		}

		#endregion
	}
}
