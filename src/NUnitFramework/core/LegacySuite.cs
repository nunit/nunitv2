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

		private static readonly Type SuiteType = typeof( NUnit.Framework.SuiteAttribute );
		private static readonly Type FixtureSetUpType = typeof( NUnit.Framework.TestFixtureSetUpAttribute );
		private static readonly Type FixtureTearDownType = typeof( NUnit.Framework.TestFixtureTearDownAttribute );

		#endregion

		#region Private Fields

		private PropertyInfo suiteProperty;

		#endregion

		#region Static Methods

		public static PropertyInfo GetSuiteProperty( Type testClass )
		{
			if( testClass != null )
			{
				PropertyInfo[] properties = testClass.GetProperties( 
					BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly );
				foreach( PropertyInfo property in properties )
				{
					if( property.IsDefined( SuiteType, false ) )
					{
						try 
						{
							CheckSuiteProperty(property);
						}
						catch( InvalidSuiteException )
						{
							return null;
						}
						return property;
					}
				}
			}
			return null;
		}

		private static void CheckSuiteProperty(PropertyInfo property)
		{
			MethodInfo method = property.GetGetMethod(true);
			if(method.ReturnType!=typeof(NUnit.Core.TestSuite))
				throw new InvalidSuiteException("Invalid suite property method signature");
			if(method.GetParameters().Length>0)
				throw new InvalidSuiteException("Invalid suite property method signature");
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

			this.fixtureSetUp = Reflect.GetMethod( fixtureType, FixtureSetUpType );
			this.fixtureTearDown = Reflect.GetMethod( fixtureType, FixtureTearDownType );			
			
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
	}
}
