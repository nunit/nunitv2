using System;
using System.Reflection;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Helper methods for inspecting a type by reflection. 
	/// 
	/// Many of these methods take a MemberInfo as an argument to avoid
	/// duplication, even though certain attributes can only appear on
	/// specific types of members, like MethodInfo or Type.
	/// 
	/// In the case where a type is being examined for the presence of
	/// an attribute, interface or named member, the Reflect methods
	/// operate with the full name of the member being sought. This
	/// removes the necessity of the caller having a reference to the
	/// assembly that defines the item being sought and allows the
	/// NUnit core to inspect assemblies that reference an older
	/// version of the NUnit framework.
	/// </summary>
	public class Reflect
	{
		#region Attributes 

		/// <summary>
		/// Check presence of attribute of a given type on a member.
		/// </summary>
		/// <param name="member">The member to examine</param>
		/// <param name="attrName">The FullName of the attribute type to look for</param>
		/// <param name="inherit">True to include inherited attributes</param>
		/// <returns>True if the attribute is present</returns>
		public static bool HasAttribute( MemberInfo member, string attrName, bool inherit )
		{
			object[] attributes = member.GetCustomAttributes( inherit );
			foreach( Attribute attribute in attributes )
				if ( attribute.GetType().FullName == attrName )
					return true;
			return false;
		}

		/// <summary>
		/// Get attribute of a given type on a member. If multiple attributes
		/// of a type are present, the first one found is returned.
		/// </summary>
		/// <param name="member">The member to examine</param>
		/// <param name="attrName">The FullName of the attribute type to look for</param>
		/// <param name="inherit">True to include inherited attributes</param>
		/// <returns>The attribute or null</returns>
		public static System.Attribute GetAttribute( MemberInfo member, string attrName, bool inherit )
		{
			object[] attributes = member.GetCustomAttributes( inherit );
			foreach( Attribute attribute in attributes )
				if ( attribute.GetType().FullName == attrName )
					return attribute;
			return null;
		}

		/// <summary>
		/// Get all attributes of a given type on a member.
		/// </summary>
		/// <param name="member">The member to examine</param>
		/// <param name="attrName">The FullName of the attribute type to look for</param>
		/// <param name="inherit">True to include inherited attributes</param>
		/// <returns>The attribute or null</returns>
		public static System.Attribute[] GetAttributes( MemberInfo member, string attrName, bool inherit )
		{
			object[] attributes = member.GetCustomAttributes( inherit );
			ArrayList result = new ArrayList();
			foreach( Attribute attribute in attributes )
				if ( attribute.GetType().FullName == attrName )
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

		/// <summary>
		/// Check to see if a type implements a named interface.
		/// </summary>
		/// <param name="fixtureType">The type to examine</param>
		/// <param name="interfaceName">The FullName of the interface to check for</param>
		/// <returns>True if the interface is implemented by the type</returns>
		public static bool HasInterface( Type fixtureType, string interfaceName )
		{
			foreach( Type type in fixtureType.GetInterfaces() )
				if ( type.FullName == interfaceName )
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
		/// <param name="attributeName">The FullName of the attribute to look for</param>
		/// <param name="bindingFlags">BindingFlags to use in looking for method</param>
		/// <returns>A MethodInfo or null</returns>
		public static MethodInfo GetMethodWithAttribute( Type fixtureType, string attributeName, BindingFlags bindingFlags )
		{
			foreach(MethodInfo method in fixtureType.GetMethods( bindingFlags ) )
			{
				if( HasAttribute( method, attributeName, true ) ) 
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
		public static MethodInfo GetNamedMethod( Type fixtureType, string methodName, BindingFlags bindingFlags )
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
		/// <param name="attributeName">The FullName of the attribute to look for</param>
		/// <returns>A MethodInfo or null</returns>
		public static MethodInfo GetUniqueMethod( Type fixtureType, string attributeName )
		{
			MethodInfo result = null;
			int count = 0;

			foreach(MethodInfo method in fixtureType.GetMethods( 
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly ) )
			{
				if( Reflect.HasAttribute( method, attributeName, true ) ) 
				{
					result = method;
					count++;
				}
			}

			if ( count > 1 )
			{
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
		/// <param name="attributeName">The FullName of the attribute to look for</param>
		/// <param name="bindingFlags">Binding flags to use in searching</param>
		/// <returns>A PropertyInfo or null</returns>
		public static PropertyInfo GetPropertyWithAttribute( Type fixtureType, string attributeName, BindingFlags bindingFlags )
		{
			foreach(PropertyInfo property in fixtureType.GetProperties( bindingFlags ) )
			{
				if( HasAttribute( property, attributeName, true ) ) 
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
		/// <param name="bindingFlags">BindingFlags to use</param>
		/// <returns>A PropertyInfo or null</returns>
		public static PropertyInfo GetNamedProperty( Type type, string name, BindingFlags bindingFlags )
		{
			return type.GetProperty( name, bindingFlags );
		}

		/// <summary>
		/// Get the value of a named property on an object
		/// </summary>
		/// <param name="obj">The object for which the property value is needed</param>
		/// <param name="name">The name of a non-indexed property of the object</param>
		/// <param name="bindingFlags">BindingFlags for use in determining which properties are needed</param>param>
		/// <returns></returns>
		public static object GetPropertyValue( object obj, string name, BindingFlags bindingFlags )
		{
			PropertyInfo property = GetNamedProperty( obj.GetType(), name, bindingFlags );
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

		#endregion

		#region Private Constructor for static-only class

		private Reflect() { }

		#endregion
	}
}
