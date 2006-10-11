using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for TestDecoratorCollection.
	/// </summary>
	public class TestDecoratorCollection : CollectionBase, ITestDecorator
	{
		public TestDecoratorCollection() { }

		public TestDecoratorCollection( TestDecoratorCollection other )
		{
			this.InnerList.AddRange( other );
		}

		#region ITestDecorator Members
		public Test Decorate(Test test, MemberInfo member)
		{
			Test decoratedTest = test;

			foreach( ITestDecorator decorator in List )
				decoratedTest = decorator.Decorate( decoratedTest, member );

			return decoratedTest;
		}

//		public Test Decorate(Test test, Type fixtureType)
//		{
//			Test decoratedTest = test;
//
//			foreach( ITestDecorator decorator in List )
//				decoratedTest = decorator.Decorate( decoratedTest, fixtureType );
//
//			return decoratedTest;
//		}
		#endregion

		public void Add( ITestDecorator decorator )
		{
			List.Add( decorator );
		}
	}
}
