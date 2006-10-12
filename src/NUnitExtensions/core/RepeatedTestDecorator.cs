using System;
using System.Reflection;

namespace NUnit.Core.Extensions
{
	/// <summary>
	/// Summary description for RepeatedTestDecorator.
	/// </summary>
	[TestDecorator, NUnitAddin]
	public class RepeatedTestDecorator : ITestDecorator, IAddin
	{
		private static readonly string RepeatAttributeType = "NUnit.Framework.Extensions.RepeatAttribute";

		#region IAddin Members
		public string Name
		{
			get { return "RepeatedTestDecorator"; }
		}

		public string Description
		{
			get { return "Allows running a test multiple times"; }
		}

		public void Initialize()
		{
			AddinManager.CurrentManager.Install( this as ITestDecorator );
		}
		#endregion

		#region ITestDecorator Members
		public Test Decorate(Test test, MemberInfo member)
		{
			if ( member == null )
				return test;

			TestCase testCase = test as TestCase;
			if ( testCase == null )
				return test;

			Attribute repeatAttr = Reflect.GetAttribute( member, RepeatAttributeType, true );
			if ( repeatAttr == null )
				return test;		

			object propVal = Reflect.GetPropertyValue( repeatAttr, "Count", 
				BindingFlags.Public | BindingFlags.Instance );

			if ( propVal == null )
				return test;

			int count = (int)propVal;

			return new RepeatedTestCase( testCase, count );
		}

//		public Test Decorate( Test test, Type fixtureType )
//		{
//			return test;
//		}
		#endregion
	}
}
