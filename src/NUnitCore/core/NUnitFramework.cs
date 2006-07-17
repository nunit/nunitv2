using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace NUnit.Core
{
	/// <summary>
	/// Static methods that implement aspects of the NUnit framework that cut 
	/// across individual test types, extensions, etc. Some of these use the 
	/// methods of the Reflect class to implement operations specific to the 
	/// NUnit Framework.
	/// </summary>
	public class NUnitFramework
	{
		private static Type assertType;
		//private static Hashtable frameworkByAssembly = new Hashtable();

		#region TestFixtureAttribute
		public static readonly string TestFixtureAttribute = "NUnit.Framework.TestFixtureAttribute";
		
		public static bool HasTestFixtureAttribute( Type type )
		{
			return Reflect.HasAttribute(type, TestFixtureAttribute, true);           
		}

		public static Attribute GetTestFixtureAttribute( Type type )
		{
			return Reflect.GetAttribute(type, TestFixtureAttribute, true);
		}
		
		/// <summary>
		/// Method to return the description for a fixture
		/// </summary>
		/// <param name="fixtureType">The fixture to check</param>
		/// <returns>The description, if any, or null</returns>
		public static string GetTestFixtureDescription(Type type)
		{
			Attribute fixtureAttribute = NUnitFramework.GetTestFixtureAttribute( type );

			if (fixtureAttribute != null)
				return NUnitFramework.GetDescription( fixtureAttribute );

			return null;
		}
		#endregion

		#region TestAttribute
		public static readonly string TestAttribute = "NUnit.Framework.TestAttribute";

		public static bool HasTestAttribute( MethodInfo method )
		{
			return Reflect.HasAttribute(method, TestAttribute, false);           
		}

		public static Attribute GetTestAttribute( MethodInfo method )
		{
			return Reflect.GetAttribute(method, TestAttribute, false);
		}

		public static string GetTestCaseDescription( MethodInfo method )
		{
			Attribute testAttribute = GetTestAttribute( method );
			if (testAttribute != null)
				return GetDescription( testAttribute );

			return null;
		}
		#endregion

		#region SetUpFixture
		public static readonly string SetUpFixtureAttribute = "NUnit.Framework.SetUpFixtureAttribute";

		public static bool HasSetUpFixtureAttribute(Type type)
		{
			return Reflect.HasAttribute( type, SetUpFixtureAttribute, false );
		}
		#endregion

		#region Suite
		public static readonly string SuiteAttribute = "NUnit.Framework.SuiteAttribute";
		#endregion

		#region SetUp
		public static readonly string SetUpAttribute = "NUnit.Framework.SetUpAttribute";

		public static bool IsSetUpMethod(MethodInfo method)
		{
			return Reflect.HasAttribute(method, NUnitFramework.SetUpAttribute, false);
		}

		public static MethodInfo GetSetUpMethod(Type fixtureType)
		{
			return Reflect.GetMethodWithAttribute(fixtureType, SetUpAttribute,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				true);
		}
		#endregion

		#region TearDown
		public static readonly string TearDownAttribute = "NUnit.Framework.TearDownAttribute";

		public static bool IsTearDownMethod(MethodInfo method)
		{
			return Reflect.HasAttribute(method, NUnitFramework.TearDownAttribute, false);
		}

		public static MethodInfo GetTearDownMethod(Type fixtureType)
		{
			return Reflect.GetMethodWithAttribute(fixtureType, TearDownAttribute,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				true);
		}
		#endregion

		#region TestFixtureSetUp
		public static readonly string FixtureSetUpAttribute = "NUnit.Framework.TestFixtureSetUpAttribute";

		public static bool IsFixtureSetUpMethod(MethodInfo method)
		{
			return Reflect.HasAttribute(method, NUnitFramework.FixtureSetUpAttribute, false);
		}

		public static MethodInfo GetFixtureSetUpMethod(Type fixtureType)
		{
			return Reflect.GetMethodWithAttribute(fixtureType, FixtureSetUpAttribute,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				true);
		}
		#endregion

		#region TestFixtureTearDown
		public static readonly string FixtureTearDownAttribute = "NUnit.Framework.TestFixtureTearDownAttribute";
		
		public static bool IsFixtureTearDownMethod(MethodInfo method)
		{
			return Reflect.HasAttribute(method, NUnitFramework.FixtureTearDownAttribute, false);
		}

		public static MethodInfo GetFixtureTearDownMethod(Type fixtureType)
		{
			return Reflect.GetMethodWithAttribute(fixtureType, FixtureTearDownAttribute,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				true);
		}
		#endregion

		#region Ignore
		public static readonly string IgnoreAttribute = "NUnit.Framework.IgnoreAttribute";

		public static Attribute GetIgnoreAttribute( MemberInfo member )
		{
			return Reflect.GetAttribute( member, NUnitFramework.IgnoreAttribute, false );
		}
		
		public static void ApplyIgnoreAttribute( MemberInfo member, Test test )
		{
			Attribute ignoreAttribute = GetIgnoreAttribute( member );
			if (ignoreAttribute != null)
			{
				test.RunState = RunState.Ignored;
				test.IgnoreReason = GetIgnoreReason( ignoreAttribute );
			}
		}
		#endregion

		#region Categories
		public static readonly string CategoryAttribute = "NUnit.Framework.CategoryAttribute";

		public static Attribute[] GetCategoryAttributes( MemberInfo member )
		{
			return Reflect.GetAttributes( member, CategoryAttribute, false );
		}
		
		public static IList GetCategories( MemberInfo member )
		{
			System.Attribute[] attributes = NUnitFramework.GetCategoryAttributes( member );
			IList categories = new ArrayList();

			foreach( Attribute categoryAttribute in attributes ) 
				categories.Add( 
					Reflect.GetPropertyValue( 
					categoryAttribute, 
					"Name", 
					BindingFlags.Public | BindingFlags.Instance ) );

			return categories;
		}
		#endregion

		#region Platform
		public static readonly string PlatformAttribute = "NUnit.Framework.PlatformAttribute";

		public static Attribute GetPlatformAttribute( MemberInfo member )
		{
			return Reflect.GetAttribute( member, NUnitFramework.PlatformAttribute, false );
		}

		public static void ApplyPlatformAttribute( MemberInfo member, Test test )
		{
			PlatformHelper helper = new PlatformHelper();
			if ( !helper.IsPlatformSupported( member ) )
			{
				test.RunState = RunState.Skipped;
				test.IgnoreReason = helper.Reason;
			}
		}
		#endregion

		#region Explicit
		public static readonly string ExplicitAttribute = "NUnit.Framework.ExplicitAttribute";
		
		public static Attribute GetExplicitAttribute( MemberInfo member )
		{
			return Reflect.GetAttribute( member, ExplicitAttribute, false );
		}

		public static void ApplyExplicitAttribute( MemberInfo member, Test test )
		{
			Attribute explicitAttribute = NUnitFramework.GetExplicitAttribute( member );
			if (explicitAttribute != null)
			{
				test.IsExplicit = true;
				test.RunState = RunState.Explicit;
				test.IgnoreReason = NUnitFramework.GetIgnoreReason( explicitAttribute );
			}
		}
		#endregion

		#region Properties
		public static readonly string PropertyAttribute = "NUnit.Framework.PropertyAttribute";
		
		public static Attribute[] GetPropertyAttributes( MemberInfo member )
		{
			return Reflect.GetAttributes( member, PropertyAttribute, false );
		}

		public static IDictionary GetProperties( MemberInfo member )
		{
			ListDictionary properties = new ListDictionary();

			foreach( Attribute propertyAttribute in NUnitFramework.GetPropertyAttributes( member ) ) 
			{
				string name = (string)Reflect.GetPropertyValue( propertyAttribute, "Name", BindingFlags.Public | BindingFlags.Instance );
				if ( name != null && name != string.Empty )
				{
					object val = Reflect.GetPropertyValue( propertyAttribute, "Value", BindingFlags.Public | BindingFlags.Instance );
					properties[name] = val;
				}
			}

			return properties;
		}
		#endregion

		#region ExpectedException
		public static readonly string ExpectedExceptionAttribute = "NUnit.Framework.ExpectedExceptionAttribute";
		
		public static Attribute GetExpectedExceptionAttribute( MethodInfo method )
		{
			return Reflect.GetAttribute( method, ExpectedExceptionAttribute, false );
		}

		// TODO: Handle this with a separate ExceptionProcessor object
		public static void ApplyExpectedExceptionAttribute(MethodInfo method, TestMethod testMethod)
		{
			Type expectedException = null;
			string expectedExceptionName = null;
			string expectedMessage = null;
			string matchType = null;

			Attribute attribute = NUnitFramework.GetExpectedExceptionAttribute( method );
			if (attribute != null)
			{
				expectedException = Reflect.GetPropertyValue(
					attribute, "ExceptionType",
					BindingFlags.Public | BindingFlags.Instance) as Type;
				expectedExceptionName = (string)Reflect.GetPropertyValue(
					attribute, "ExceptionName",
					BindingFlags.Public | BindingFlags.Instance) as String;
				expectedMessage = (string)Reflect.GetPropertyValue(
					attribute, "ExpectedMessage",
					BindingFlags.Public | BindingFlags.Instance) as String;
				object matchEnum = Reflect.GetPropertyValue(
					attribute, "MatchType",
					BindingFlags.Public | BindingFlags.Instance);
				if (matchEnum != null)
					matchType = matchEnum.ToString();
			}

			if ( expectedException != null )
				testMethod.ExpectedException = expectedException;
			else if ( expectedExceptionName != null )
				testMethod.ExpectedExceptionName = expectedExceptionName;

			testMethod.ExpectedMessage = expectedMessage;
			testMethod.MatchType = matchType;
		}
		#endregion

		#region AssertException
		public static readonly string AssertException = "NUnit.Framework.AssertionException";

		public static bool IsAssertException(Exception ex)
		{
			return ex.GetType().FullName == AssertException;
		}
		#endregion

		#region IgnoreException
		public static readonly string IgnoreException = "NUnit.Framework.IgnoreException";

		public static bool IsIgnoreException(Exception ex)
		{
			return ex.GetType().FullName == IgnoreException;
		}
		#endregion

		#region Assertion Count
		public static readonly string AssertType = "NUnit.Framework.Assert";

		public static int GetAssertCount()
		{
			if ( assertType == null )
				foreach( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies() )
					if ( assembly.GetName().Name == "nunit.framework" )
					{
						assertType = assembly.GetType( AssertType );
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
		#endregion

		#region SuiteBuilder
		public static readonly string SuiteBuilderAttribute = typeof( SuiteBuilderAttribute ).FullName;
		public static readonly string SuiteBuilderInterface = typeof( ISuiteBuilder ).FullName;
		
		public static bool IsSuiteBuilder( Type type )
		{
			return Reflect.HasAttribute( type, SuiteBuilderAttribute, false )
				&& Reflect.HasInterface( type, SuiteBuilderInterface );
		}
		#endregion

		#region TestCaseBuilder
		public static readonly string TestCaseBuilderAttributeName = typeof( TestCaseBuilderAttribute ).FullName;
		public static readonly string TestCaseBuilderInterfaceName = typeof( ITestCaseBuilder ).FullName;
		
		public static bool IsTestCaseBuilder( Type type )
		{
			return Reflect.HasAttribute( type, TestCaseBuilderAttributeName, false )
				&& Reflect.HasInterface( type, TestCaseBuilderInterfaceName );
		}
		#endregion

		#region TestDecorator
		public static readonly string TestDecoratorAttributeName = typeof( TestDecoratorAttribute ).FullName;
		public static readonly string TestDecoratorInterfaceName = typeof( ITestDecorator ).FullName;

		public static bool IsTestDecorator( Type type )
		{
			return Reflect.HasAttribute( type, TestDecoratorAttributeName, false )
				&& Reflect.HasInterface( type, TestDecoratorInterfaceName );
		}
		#endregion

		#region Helpers for Attribute Properties
		public static string GetIgnoreReason( System.Attribute attribute )
		{
			return (string)Reflect.GetPropertyValue(
				attribute,
				"Reason",
				BindingFlags.Public | BindingFlags.Instance);
		}
		public static string GetDescription( System.Attribute attribute )
		{
			return (string)Reflect.GetPropertyValue(
				attribute,
				"Description",
				BindingFlags.Public | BindingFlags.Instance);
		}
		#endregion

		#region NUnit Configuration Settings
		public static bool AllowOldStyleTests
		{
			get
			{
				try
				{
					NameValueCollection settings = (NameValueCollection)
						ConfigurationSettings.GetConfig("NUnit/TestCaseBuilder");
					if (settings != null)
					{
						string oldStyle = settings["OldStyleTestCases"];
						if (oldStyle != null)
							return Boolean.Parse(oldStyle);
					}
				}
				catch( Exception e )
				{
					Debug.WriteLine( e );
				}

				return false;
			}
		}
		#endregion
	}
}
