using System;
using System.Reflection;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Static methods that use the methods of the Reflect class to
	/// implement operations specific to the NUnit Framework.
	/// </summary>
	public class NUnitFramework
	{
		private static Type assertType;
		private static Hashtable frameworkByAssembly = new Hashtable();

		public static bool IsAssertException(Exception ex)
		{
			return ex.GetType().FullName == "NUnit.Framework.AssertionException";
		}

		public static bool IsIgnoreException(Exception ex)
		{
			return ex.GetType().FullName == "NUnit.Framework.IgnoreException";
		}

		public static int GetAssertCount()
		{
			if ( assertType == null )
				foreach( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies() )
					if ( assembly.GetName().Name == "nunit.framework" )
					{
						assertType = assembly.GetType( "NUnit.Framework.Assert" );
						break;
					}

			if ( assertType == null )
				return 0;

			PropertyInfo property = Reflect.GetNamedProperty( 
				assertType,
				"Counter", 
				BindingFlags.Public | BindingFlags.Static );

			if ( property == null )
				return 0;
		
			return (int)property.GetValue( null, new object[0] );
		}
	}
}
