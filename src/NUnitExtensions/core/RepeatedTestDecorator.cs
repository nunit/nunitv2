using System;
using System.Reflection;

namespace NUnit.Core.Extensions
{
	/// <summary>
	/// Summary description for RepeatedTestDecorator.
	/// </summary>
	[TestDecorator]
	public class RepeatedTestDecorator : ITestDecorator
	{
		private static readonly string RepeatAttributeType = "NUnit.Framework.Extensions.RepeatAttribute";

		#region ITestDecorator Members
		public TestCase Decorate(TestCase testCase, MethodInfo method)
		{
			if ( method == null )
				return testCase;

			Attribute repeatAttr = Reflect.GetAttribute( method, RepeatAttributeType, true );
			if ( repeatAttr == null )
				return testCase;		

			object propVal = Reflect.GetPropertyValue( repeatAttr, "Count", 
				BindingFlags.Public | BindingFlags.Instance );

			if ( propVal == null )
				return testCase;

			int count = (int)propVal;

			return new RepeatedTestCase( testCase, count );
		}

		public TestSuite Decorate( TestSuite suite, Type fixtureType )
		{
			return suite;
		}
		#endregion
	}
}
