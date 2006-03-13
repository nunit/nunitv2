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
		public TestCase Decorate(TestCase testCase, MethodInfo method)
		{
			TestCase decoratedTestCase = testCase;

			foreach( ITestDecorator decorator in List )
				decoratedTestCase = decorator.Decorate( decoratedTestCase, method );

			return decoratedTestCase;
		}

		public TestSuite Decorate(TestSuite suite, Type fixtureType)
		{
			TestSuite decoratedTestSuite = suite;

			foreach( ITestDecorator decorator in List )
				decoratedTestSuite = decorator.Decorate( decoratedTestSuite, fixtureType );

			return decoratedTestSuite;
		}
		#endregion

		public void Add( ITestDecorator decorator )
		{
			List.Add( decorator );
		}
	}
}
