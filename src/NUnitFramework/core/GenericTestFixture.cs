using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// GenericTestFixture is a test fixture based on a set
	/// of parameters stored in a TestFixtureParameters struct.
	/// </summary>
	public abstract class GenericTestFixture : TestFixture
	{
		#region Instance Fields 

		protected TestFixtureParameters parms;

		#endregion
	
		#region Constructors

		public GenericTestFixture( TestFixtureParameters parms, Type fixtureType ) 
			: this( parms, fixtureType, 0 ) { }

		public GenericTestFixture( TestFixtureParameters parms, Type fixtureType, int assemblyKey ) 
			: base( fixtureType, assemblyKey ) 
		{ 
			// This must be first, GetXxxxXxMethod calls depend on it
			this.parms = parms;

			this.testFramework = TestFramework.FromType( fixtureType );

			this.testSetUp = GetSetUpMethod();
			this.testTearDown = GetTearDownMethod();
			this.fixtureSetUp = GetFixtureSetUpMethod();
			this.fixtureTearDown = GetFixtureTearDownMethod();
		}

		#endregion 

		#region Protected Methods

		protected virtual MethodInfo GetSetUpMethod()
		{
			return Reflect.GetMethodWithAttribute( FixtureType, parms.SetUpType,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
		}
		protected virtual MethodInfo GetTearDownMethod()
		{
			return Reflect.GetMethodWithAttribute( FixtureType, parms.TearDownType,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
		}
		protected virtual MethodInfo GetFixtureSetUpMethod()
		{
			return Reflect.GetMethodWithAttribute( FixtureType, parms.FixtureSetUpType,
				BindingFlags.Public | BindingFlags.Instance );
		}
		protected virtual MethodInfo GetFixtureTearDownMethod()
		{
			return Reflect.GetMethodWithAttribute( FixtureType, parms.FixtureTearDownType,
				BindingFlags.Public | BindingFlags.Instance );
		}

//		protected virtual TestCase MakeTestCase( MethodInfo method )
//		{
//			return new NormalTestCase( method.ReflectedType, method );
//		}

		#endregion
	}
}
