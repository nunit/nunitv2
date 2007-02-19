// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// Represents a test suite constructed from a type that has a static Suite property
	/// </summary>
	public class LegacySuite : TestSuite
	{
		#region Private Fields

		private PropertyInfo suiteProperty;

		#endregion

		#region Static Methods

		public static PropertyInfo GetSuiteProperty( Type testClass )
		{
			if( testClass == null )
				return null;

			PropertyInfo property = Reflect.GetPropertyWithAttribute( 
				testClass, 
				NUnitFramework.SuiteAttribute,
				BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly );

			if ( property == null )
				return null;

			MethodInfo method = property.GetGetMethod(true);

			if( method.ReturnType.FullName != "NUnit.Core.TestSuite" )
				return null;
				//throw new InvalidSuiteException("Invalid suite property method signature");
			if( method.GetParameters().Length > 0 )
				return null;
				//throw new InvalidSuiteException("Invalid suite property method signature");

			return property;
		}

		#endregion

		#region Constructors

		public LegacySuite( Type fixtureType ) : base( fixtureType )
		{
			suiteProperty = GetSuiteProperty( fixtureType );

			this.fixtureSetUp = NUnitFramework.GetFixtureSetUpMethod( fixtureType );
			this.fixtureTearDown = NUnitFramework.GetFixtureTearDownMethod( fixtureType );
			
			MethodInfo method = suiteProperty.GetGetMethod(true);
			
			if( method.ReturnType.FullName != "NUnit.Core.TestSuite" || method.GetParameters().Length > 0 )
			{
				this.RunState = RunState.NotRunnable;
				this.IgnoreReason = "Invalid suite property method signature";
			}
			else
			{
				TestSuite suite = (TestSuite)suiteProperty.GetValue(null, new Object[0]);		
				foreach( Test test in suite.Tests )
					this.Add( test );
			}
		}

		#endregion
	}
}
