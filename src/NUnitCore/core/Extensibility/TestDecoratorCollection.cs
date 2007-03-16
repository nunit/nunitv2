// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core.Extensibility
{
	/// <summary>
	/// TestDecoratorCollection is an ExtensionPoint for TestDecorators and
	/// implements the ITestDecorator interface itself, passing calls 
	/// on to the individual decorators.
	/// </summary>
	public class TestDecoratorCollection : ITestDecorator, IExtensionPoint
	{
		private ArrayList decorators = new ArrayList();

		#region Constructors
		public TestDecoratorCollection() { }

		public TestDecoratorCollection( TestDecoratorCollection other )
		{
			decorators.AddRange( other.decorators );
		}
		#endregion

		#region ITestDecorator Members
		public Test Decorate(Test test, MemberInfo member)
		{
			Test decoratedTest = test;

			foreach( ITestDecorator decorator in decorators )
				decoratedTest = decorator.Decorate( decoratedTest, member );

			return decoratedTest;
		}
		#endregion

		#region IExtensionPoint Members
		public string Name
		{
			get { return "TestDecorators"; }
		}

        public IExtensionHost Host
        {
            get { return CoreExtensions.Host; }
        }

		public void Install(object extension)
		{
			ITestDecorator decorator = extension as ITestDecorator;
			if ( decorator == null )
				throw new ArgumentException( 
					extension.GetType().FullName + " is not an ITestDecorator", "exception" );

			decorators.Add( extension );
		}

		public void Remove( object extension )
		{
			decorators.Remove( extension );
		}
		#endregion
	}
}
