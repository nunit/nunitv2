using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// Represents a test suite constructed from a type that has a static Suite property
	/// </summary>
	public class LegacySuite : TestSuite
	{
		#region Attributes recognized by LegacySuite

		private static readonly string SuiteType = "NUnit.Framework.SuiteAttribute";
		private static readonly string FixtureSetUpType = "NUnit.Framework.TestFixtureSetUpAttribute";
		private static readonly string FixtureTearDownType = "NUnit.Framework.TestFixtureTearDownAttribute";

		#endregion

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
				SuiteType,
				BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly );

			if ( property == null )
				return null;

			MethodInfo method = property.GetGetMethod(true);

			if(method.ReturnType!=typeof(NUnit.Core.TestSuite))
				return null;
				//throw new InvalidSuiteException("Invalid suite property method signature");
			if(method.GetParameters().Length>0)
				return null;
				//throw new InvalidSuiteException("Invalid suite property method signature");

			return property;
		}

		#endregion

		#region Constructors

		public LegacySuite( Type fixtureType ) : base( fixtureType, 0 )
		{
			Initialize( fixtureType );
		}

		public LegacySuite( Type fixtureType, int assemblyKey ) : base( fixtureType, assemblyKey ) 
		{
			Initialize( fixtureType );
		}

		public LegacySuite( object fixture ) : base( fixture, 0 ) 
		{
			Initialize( fixture.GetType() );
		}

		public LegacySuite( object fixture, int assemblyKey ) : base( fixture, assemblyKey ) 
		{
			Initialize( fixture.GetType() );
		}

		private void Initialize( Type fixtureType )
		{
			suiteProperty = GetSuiteProperty( fixtureType );

			this.fixtureSetUp = Reflect.GetMethodWithAttribute( fixtureType, FixtureSetUpType,
				BindingFlags.Public | BindingFlags.Instance );
			this.fixtureTearDown = Reflect.GetMethodWithAttribute( fixtureType, FixtureTearDownType,
				BindingFlags.Public | BindingFlags.Instance );			
			
			MethodInfo method = suiteProperty.GetGetMethod(true);
			
			if(method.ReturnType!=typeof(NUnit.Core.TestSuite) || method.GetParameters().Length>0)
			{
				this.ShouldRun = false;
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

		#region Properties

		public override bool IsFixture
		{
			get { return false; }
		}


		#endregion
	}
}
