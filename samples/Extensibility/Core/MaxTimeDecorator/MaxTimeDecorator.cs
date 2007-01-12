using System;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Extensions
{
	/// <summary>
	/// Summary description for MaxTimeDecorator.
	/// </summary>
	[NUnitAddin(Description="Fails a test if its elapsed time is longer than a given maximum")]
	public class MaxTimeDecorator : IAddin, ITestDecorator
	{
		#region IAddin Members

		public bool Install(IExtensionHost host)
		{
			System.Diagnostics.Trace.WriteLine( "MaxTimeDecorator: Install called" );
			IExtensionPoint decorators = host.GetExtensionPoint( "TestDecorators" );
			if ( decorators == null ) return false;

			decorators.Install( this );
			return true;
		}

		#endregion

		#region ITestDecorator Members

		public Test Decorate(Test test, System.Reflection.MemberInfo member)
		{
			if ( test is TestCase )
			{
				Attribute attr = Reflect.GetAttribute( 
					member, "NUnit.Framework.Extensions.MaxTimeAttribute", false );
				if ( attr != null )
				{
					int maxTime = (int)Reflect.GetPropertyValue( attr, "MaxTime", BindingFlags.Public | BindingFlags.Instance );
					test = new MaxTimeTestCase( (TestCase)test, maxTime );
				}
			}

			return test;
		}

		#endregion
	}
}
