using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// SetUpFixture extends NamespaceSuite and allows a namespace to have
	/// a TestFixtureSetup and TestFixtureTearDown.
	/// </summary>
	public class SetUpFixture : NamespaceSuite
	{
		public SetUpFixture( Type type, int assemblyKey ) : base( type, assemblyKey )
		{
			testName = type.Namespace;
			int index = testName.LastIndexOf( '.' );
			if ( index > 0 )
				testName = testName.Substring( index + 1 );
			// NOTE: Once again, since we are not inheriting from TestFixture,
			// no automatic construction is performed for us, so we do it here.
			this.Fixture = Reflect.Construct( type );

			this.fixtureSetUp = Reflect.GetMethodWithAttribute( 
				type, "NUnit.Framework.TestFixtureSetUpAttribute",
				BindingFlags.Public | BindingFlags.Instance );
			this.fixtureTearDown = Reflect.GetMethodWithAttribute( 
				type, "NUnit.Framework.TestFixtureTearDownAttribute",
				BindingFlags.Public | BindingFlags.Instance );
		}

		public override void DoOneTimeSetUp(TestResult suiteResult)
		{
			base.DoOneTimeSetUp (suiteResult);

			if ( fixtureSetUp != null && suiteResult.IsSuccess )
				Reflect.InvokeMethod( fixtureSetUp, this.Fixture );
		}

		public override void DoOneTimeTearDown(TestResult suiteResult)
		{
			if (fixtureTearDown != null )
				Reflect.InvokeMethod( fixtureTearDown, this.Fixture );

			base.DoOneTimeTearDown (suiteResult);
		}
	}
}
