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
	/// checking for a given attribute. This class contains no knowledge
	/// of what different attributes represent.
	/// </summary>
	public class Reflect
	{
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
			object[] attributes = member.GetCustomAttributes( inherit );
			foreach( Attribute attribute in attributes )
				if ( attribute.GetType().FullName == type.FullName )
					return true;
			return false;
			//return member.GetCustomAttributes( type, inherit ).Length > 0;
		}

		/// <summary>
		/// Get attribute of a given type on a member. If multiple attributes
		/// of a type are present, the first one found is returned.
		/// </summary>
		/// <param name="member">The member to examine</param>
		/// <param name="type">The attribute type to look for</param>
		/// <param name="inherit">True to include inherited attributes</param>
		/// <returns>The attribute or null</returns>
		public static System.Attribute GetAttribute( MemberInfo member, Type type, bool inherit )
		{
			object[] attributes = member.GetCustomAttributes( inherit );
			foreach( Attribute attribute in attributes )
				if ( attribute.GetType().FullName == type.FullName )
					return attribute;
			return null;
			//return attributes.Length > 0 ? (System.Attribute) attributes[0] : null;
		}

		/// <summary>
		/// Get all attributes of a given type on a member.
		/// </summary>
		/// <param name="member">The member to examine</param>
		/// <param name="type">The attribute type to look for</param>
		/// <param name="inherit">True to include inherited attributes</param>
		/// <returns>The attribute or null</returns>
		public static System.Attribute[] GetAttributes( MemberInfo member, Type type, bool inherit )
		{
			object[] attributes = member.GetCustomAttributes( inherit );
			ArrayList result = new ArrayList();
			foreach( Attribute attribute in attributes )
				if ( attribute.GetType().FullName == type.FullName )
					result.Add( attribute );
			return (System.Attribute[])result.ToArray( typeof( System.Attribute ) );
		}

		/// <summary>
		/// Get all attributes on a member.
		/// </summary>
		/// <param name="member">The member to examine</param>
		/// <param name="inherit">True to include inherited attributes</param>
		/// <returns>The attribute or null</returns>
		public static System.Attribute[] GetAttributes( MemberInfo member, bool inherit )
		{
			return (System.Attribute[])member.GetCustomAttributes( inherit );
		}

		#endregion

		#region Interfaces

		public static bool HasInterface( Type fixtureType, Type interfaceType )
		{
			foreach( Type type in fixtureType.GetInterfaces() )
				if ( type.FullName == interfaceType.FullName )
						return true;
			return false;
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
		/// <param name="bindingFlags">BindingFlags to use in looking for method</param>
		/// <returns>A MethodInfo or null</returns>
		public static MethodInfo GetMethod( Type fixtureType, Type attributeType, BindingFlags bindingFlags )
		{
			foreach(MethodInfo method in fixtureType.GetMethods( bindingFlags ) )
			{
				if( HasAttribute( method, attributeType, true ) ) 
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
		/// <param name="bindingFlags">BindingFlags to use in the search</param>
		/// <returns>A MethodInfo or null</returns>
		public static MethodInfo GetMethod( Type fixtureType, string methodName, BindingFlags bindingFlags )
		{
			foreach(MethodInfo method in fixtureType.GetMethods( bindingFlags ) )
			{
				if( method.Name == methodName ) 
					return method;
			}

			return null;
		}

		/// <summary>
		/// Examine a fixture type and return a method having a particular attribute.
		/// In the case of multiple methods, an InvalidTestFixtureException is thrown.
		/// </summary>
		/// <param name="fixtureType">The type to examine</param>
		/// <param name="attributeType">The attribute to look for</param>
		/// <returns>A MethodInfo or null</returns>
		public static MethodInfo GetUniqueMethod( Type fixtureType, Type attributeType )
		{
			MethodInfo result = null;
			int count = 0;

			foreach(MethodInfo method in fixtureType.GetMethods( 
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly ) )
			{
				if( Reflect.HasAttribute( method, attributeType, true ) ) 
				{
					result = method;
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

			return result;
		}

		#endregion

		#region Get Properties of a type

		/// <summary>
		/// Examine a type and return a property having a particular attribute.
		/// In the case of multiple methods, the first one found is returned.
		/// </summary>
		/// <param name="fixtureType">The type to examine</param>
		/// <param name="attributeType">The attribute to look for</param>
		/// <param name="bindingFlags">Binding flags to use in searching</param>
		/// <returns>A PropertyInfo or null</returns>
		public static PropertyInfo GetProperty( Type fixtureType, Type attributeType, BindingFlags bindingFlags )
		{
			foreach(PropertyInfo property in fixtureType.GetProperties( bindingFlags ) )
			{
				if( HasAttribute( property, attributeType, true ) ) 
					return property;
			}

			return null;
		}

		/// <summary>
		/// Examine a type and get a property with a particular name.
		/// In the case of overloads, the first one found is returned.
		/// </summary>
		/// <param name="type">The type to examine</param>
		/// <param name="propertyName">The name of the method</param>
		/// <returns>A MethodInfo or null</returns>
		public static PropertyInfo GetProperty( Type type, string name, BindingFlags binding )
		{
			return type.GetProperty( name, binding );
		}

		public static object GetPropertyValue( object obj, string name, BindingFlags binding )
		{
			PropertyInfo property = GetProperty( obj.GetType(), name, binding );
			if ( property != null )
				return property.GetValue( obj, null );
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
		public static void InvokeMethod( Type attributeType, object fixture, BindingFlags bindingFlags )
		{
			MethodInfo method = GetMethod( fixture.GetType(), attributeType, bindingFlags );
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
