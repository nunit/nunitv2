using System;
using System.Reflection;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Helper methods for inspecting a type by reflection
	/// </summary>
	public class Reflect
	{
		#region Attribute types used by reflect
			
		private static readonly Type SetUpType = typeof( NUnit.Framework.SetUpAttribute );
		private static readonly Type TearDownType = typeof( NUnit.Framework.TearDownAttribute );
		private static readonly Type FixtureSetUpType = typeof( NUnit.Framework.TestFixtureSetUpAttribute );
		private static readonly Type FixtureTearDownType = typeof( NUnit.Framework.TestFixtureTearDownAttribute );

		#endregion

		#region Binding flags used by reflect

		private static readonly BindingFlags InstanceMethods =
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

		private static readonly BindingFlags AllMethods = 
			BindingFlags.Public |  BindingFlags.NonPublic |
			BindingFlags.Instance | BindingFlags.Static;

		private static readonly BindingFlags AllDeclaredMethods = 
			AllMethods | BindingFlags.DeclaredOnly; 

		#endregion

		#region Methods to check validity of a type and its members

		public static void CheckFixtureType( Type fixtureType )
		{
			if ( fixtureType.GetConstructor( Type.EmptyTypes ) == null )
				throw new InvalidTestFixtureException(fixtureType.FullName + " does not have a valid constructor");
			
			CheckSetUpTearDownMethod( fixtureType, SetUpType );
			CheckSetUpTearDownMethod( fixtureType, TearDownType );
			CheckSetUpTearDownMethod( fixtureType, FixtureSetUpType );
			CheckSetUpTearDownMethod( fixtureType, FixtureTearDownType );
		}

		private static void CheckSetUpTearDownMethod( Type fixtureType, Type attributeType )
		{
			int count = 0;
			MethodInfo theMethod = null;

			foreach(MethodInfo method in fixtureType.GetMethods( AllDeclaredMethods ))
			{
				if( method.IsDefined( attributeType, false ) )
				{
					theMethod = method;
					count++;
				}
			}

			if ( count > 1 )
			{
				string attributeName = attributeType.Name;
				if ( attributeName.EndsWith( "Attribute" ) )
					attributeName = attributeName.Substring( 
						0, attributeName.Length - 9 );

				throw new InvalidTestFixtureException( 
					string.Format( "{0} has multiple {1} methods",
					fixtureType.Name, attributeName ) );
			}

			CheckSetUpTearDownSignature( theMethod );
		} 

		private static void CheckSetUpTearDownSignature( MethodInfo method )
		{
			if ( method != null )
			{
				if ( !method.IsPublic && !method.IsFamily || method.IsStatic || method.ReturnType != typeof(void) || method.GetParameters().Length > 0 )
					throw new InvalidTestFixtureException("Invalid SetUp or TearDown method signature");
			}
		}

 		#endregion

		#region Get Methods of a type

		// These methods all take an object and assume that the type of the
		// object was pre-checked so that there are no duplicate methods,
		// statics, private methods, etc.

		public static ConstructorInfo GetConstructor( Type fixtureType )
		{
			return fixtureType.GetConstructor( Type.EmptyTypes );
		}

		public static MethodInfo GetSetUpMethod( Type fixtureType )
		{
			return GetMethod( fixtureType, SetUpType );
		}

		public static MethodInfo GetTearDownMethod(Type fixtureType)
		{			
			return GetMethod(fixtureType, TearDownType );
		}

		public static MethodInfo GetFixtureSetUpMethod( Type fixtureType )
		{
			return GetMethod( fixtureType, FixtureSetUpType );
		}

		public static MethodInfo GetFixtureTearDownMethod( Type fixtureType )
		{
			return GetMethod( fixtureType, FixtureTearDownType );
		}

		public static MethodInfo GetMethod( Type fixtureType, Type attributeType )
		{
			foreach(MethodInfo method in fixtureType.GetMethods( InstanceMethods ) )
			{
				if( method.IsDefined( attributeType, true ) ) 
					return method;
			}

			return null;
		}

		public static MethodInfo GetMethod( Type fixtureType, string methodName )
		{
			foreach(MethodInfo method in fixtureType.GetMethods( InstanceMethods ) )
			{
				if( method.Name == methodName ) 
					return method;
			}

			return null;
		}

		#endregion

		public static IList GetCategories( Type type ) 
		{
			IList names = new ArrayList();
			object[] attributes = type.GetCustomAttributes(typeof(NUnit.Framework.CategoryAttribute), true);
			foreach(NUnit.Framework.CategoryAttribute attribute in attributes) 
			{
				names.Add(attribute.Name);
			}
			return names;
		}


		#region Invoke Methods

		public static object Construct( Type type )
		{
			ConstructorInfo ctor = GetConstructor( type );
			if ( ctor == null )
				throw new InvalidTestFixtureException(type.FullName + " does not have a valid constructor");
			
			return ctor.Invoke( Type.EmptyTypes );
		}

		public static void InvokeMethod( MethodInfo method, object fixture ) 
		{
			if(method != null)
			{
				try
				{
					method.Invoke( fixture, null );
				}
				catch(TargetInvocationException e)
				{
					Exception inner = e.InnerException;
					throw new NunitException("Rethrown",inner);
				}
			}
		}

		public static void InvokeSetUp( object fixture )
		{
			MethodInfo method = GetSetUpMethod( fixture.GetType() );
			if(method != null)
			{
				InvokeMethod(method, fixture);
			}
		}

		public static void InvokeTearDown( object fixture )
		{
			MethodInfo method = GetTearDownMethod( fixture.GetType() );
			if(method != null)
			{
				InvokeMethod(method, fixture);
			}
		}

		#endregion

		#region Private Constructor for static-only class

		private Reflect() { }

		#endregion
	}
}
