using System;
using System.Reflection;
using System.Collections;
using NUnit.Framework;

namespace NUnit.Core
{
	/// <summary>
	/// Helper methods for inspecting a type by reflection.
	/// 
	/// Many of these methods take a MemberInfo as an argument to avoid
	/// duplication, even though certain attributes can only appear on
	/// specific types of members, like MethodInfo or Type.
	/// 
	/// Generally, these methods perform simple utility functions like
	/// checking for a given attribute. However, some of the methdods
	/// actually implement policies. That is, the methods "know" what
	/// each attribute represents.
	/// 
	/// TODO: Move knowledge of policies to classes that should have
	/// that knowledge and reserve this class for general-purpose
	/// helpers that perform reflection.
	/// </summary>
	public class Reflect
	{
		#region Attribute types used by reflect
 
		// TODO: Move these definitions to the fixtures and test cases that
		// understand them.

		private static readonly Type TestFixtureType = typeof( TestFixtureAttribute );
		private static readonly Type TestType = typeof( TestAttribute );
		private static readonly Type SetUpType = typeof( SetUpAttribute );
		private static readonly Type TearDownType = typeof( TearDownAttribute );
		private static readonly Type FixtureSetUpType = typeof( TestFixtureSetUpAttribute );
		private static readonly Type FixtureTearDownType = typeof( TestFixtureTearDownAttribute );
		
		#endregion

		#region Binding flags used by Reflect

		private static readonly BindingFlags InstanceMethods =
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

		private static readonly BindingFlags AllMethods = 
			BindingFlags.Public |  BindingFlags.NonPublic |
			BindingFlags.Instance | BindingFlags.Static;

		private static readonly BindingFlags AllDeclaredMethods = 
			AllMethods | BindingFlags.DeclaredOnly; 

		#endregion

		#region Legacy Checks on Names

		// TODO: This belongs in a TestCaseBuilder
		public static bool IsObsoleteTestMethod(MethodInfo methodToCheck)
		{
			if ( methodToCheck.Name.ToLower().StartsWith("test") )
			{
				object[] attributes = methodToCheck.GetCustomAttributes( false );

				foreach( Attribute attribute in attributes )
					if( attribute is SetUpAttribute ||
						attribute is TestFixtureSetUpAttribute ||
						attribute is TearDownAttribute || 
						attribute is TestFixtureTearDownAttribute )
					{
						return false;
					}

				return true;	
			}

			return false;
		}

		#endregion

		#region Attributes 

		/// <summary>
		/// Check presence of attribute of a given type on a member.
		/// </summary>
		/// <param name="member">The member to examine</param>
		/// <param name="type">The attribute typeto look for</param>
		/// <param name="inherit">True to include inherited attributes</param>
		/// <returns>True if the attribute is present</returns>
		public static bool HasAttribute( MemberInfo member, Type type, bool inherit )
		{
			return member.GetCustomAttributes( type, inherit ).Length > 0;
		}

		/// <summary>
		/// Get attribute of a given type on a member. If multiple attributes
		/// of a type are present, the first one found is returned.
		/// </summary>
		/// <param name="member">The member to examine</param>
		/// <param name="type">The attribute typeto look for</param>
		/// <param name="inherit">True to include inherited attributes</param>
		/// <returns>The attribute or null</returns>
		public static System.Attribute GetAttribute( MemberInfo member, Type type, bool inherit )
		{
			object[] attributes = member.GetCustomAttributes( type, inherit );
			return attributes.Length > 0 ? (System.Attribute) attributes[0] : null;
		}

		#endregion

		#region Methods to check validity of a type and its members

		// TODO: Move policy stuff to a builder. Boilerplate can stay here

		/// <summary>
		/// Method to validate that a type is a valid test fixture
		/// </summary>
		/// <param name="fixtureType">The type to be checked</param>
		public static void CheckFixtureType( Type fixtureType )
		{
			if ( fixtureType.GetConstructor( Type.EmptyTypes ) == null )
				throw new InvalidTestFixtureException(fixtureType.FullName + " does not have a valid constructor");
			
			CheckSetUpTearDownMethod( fixtureType, SetUpType );
			CheckSetUpTearDownMethod( fixtureType, TearDownType );
			CheckSetUpTearDownMethod( fixtureType, FixtureSetUpType );
			CheckSetUpTearDownMethod( fixtureType, FixtureTearDownType );
		}

		/// <summary>
		/// This method verifies that a type has no more than one method of a particular
		/// SetUp or TearDown type and that the method has a correct signature.
		/// </summary>
		/// <param name="fixtureType">The type to be checked</param>
		/// <param name="attributeType">The attribute to check for</param>
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

			if ( theMethod != null )
			{
				if ( theMethod.IsStatic ||
					 theMethod.IsAbstract ||
					!theMethod.IsPublic && !theMethod.IsFamily ||
					 theMethod.GetParameters().Length != 0 ||
					!theMethod.ReturnType.Equals( typeof(void) ) )
				{
					throw new InvalidTestFixtureException("Invalid SetUp or TearDown method signature");
				}
			}
		} 

		#endregion

		#region Get Methods of a type

		/// <summary>
		/// Find the default constructor on a type
		/// </summary>
		/// <param name="fixtureType"></param>
		/// <returns></returns>
		public static ConstructorInfo GetConstructor( Type fixtureType )
		{
			return fixtureType.GetConstructor( Type.EmptyTypes );
		}

		/// <summary>
		/// Examine a fixture type and return a method having a particular attribute.
		/// In the case of multiple methods, the first one found is returned.
		/// </summary>
		/// <param name="fixtureType">The type to examine</param>
		/// <param name="attributeType">The attribute to look for</param>
		/// <returns>A MethodInfo or null</returns>
		public static MethodInfo GetMethod( Type fixtureType, Type attributeType )
		{
			foreach(MethodInfo method in fixtureType.GetMethods( InstanceMethods ) )
			{
				if( method.IsDefined( attributeType, true ) ) 
					return method;
			}

			return null;
		}

		/// <summary>
		/// Examine a fixture type and get a method with a particular name.
		/// In the case of overloads, the first one found is returned.
		/// </summary>
		/// <param name="fixtureType">The type to examine</param>
		/// <param name="methodName">The name of the method</param>
		/// <returns>A MethodInfo or null</returns>
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

		#region Invoke Methods

		/// <summary>
		/// Invoke the default constructor on a type
		/// </summary>
		/// <param name="type">The type to be constructed</param>
		/// <returns>An instance of the type</returns>
		public static object Construct( Type type )
		{
			ConstructorInfo ctor = GetConstructor( type );
			if ( ctor == null )
				throw new InvalidTestFixtureException(type.FullName + " does not have a valid constructor");
			
			return ctor.Invoke( Type.EmptyTypes );
		}

		/// <summary>
		/// Invoke a parameterless method on an object.
		/// </summary>
		/// <param name="method">A MethodInfo for the method to be invoked</param>
		/// <param name="fixture">The object on which to invoke the mentod</param>
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

		/// <summary>
		/// Invoke the method having a particular attribute on an object.
		/// The method should be unique, but no check is made and the first 
		/// one found is invoked.
		/// </summary>
		/// <param name="attributeType">The attribute to look for</param>
		/// <param name="fixture">The object on which to invoke the method</param>
		public static void InvokeMethod( Type attributeType, object fixture )
		{
			MethodInfo method = GetMethod( fixture.GetType(), attributeType );
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
