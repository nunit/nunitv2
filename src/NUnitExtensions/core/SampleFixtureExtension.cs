using System;

namespace NUnit.Core.Extensions
{
	/// <summary>
	/// SampleFixtureExtension extends NUnitTestFixture and adds a custom setup
	/// before running TestFixtureSetUp and after running TestFixtureTearDown.
	/// Because it inherits from NUnitTestFixture, a lot of work is done for it.
	/// </summary>
	class SampleFixtureExtension : NUnitTestFixture
	{
		public SampleFixtureExtension( Type fixtureType, int assemblyKey ) 
			: base( fixtureType, assemblyKey )
		{
			// NOTE: Since we are inheriting from TestFixture we don't 
			// have to do anything if we don't want to. Our tests will 
			// be recognized in the normal way by TestFixture, based
			// on the presence of the Test attribute.
			//
			// Just to have something to do, we override DoSetUp and DoTearDown
			// below to do some special processing before and after the normal
			// TestFixtureSetUp and TestFixtureTearDown
		}

		public override void DoOneTimeSetUp(TestResult suiteResult)
		{
			Console.WriteLine( "Extended Fixture SetUp called" );
			base.DoOneTimeSetUp (suiteResult);
		}

		public override void DoOneTimeTearDown(TestResult suiteResult)
		{
			base.DoOneTimeTearDown (suiteResult);
			Console.WriteLine( "Extended Fixture TearDown called" );
		}
	}
}
