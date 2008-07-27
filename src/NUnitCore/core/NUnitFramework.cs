// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using NUnit.Core.Extensibility;

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
        #region Constants

		#region Attribute Names
		// NOTE: Attributes used in switch statements must be const

        // Attributes that apply to Assemblies, Classes and Methods
        public const string IgnoreAttribute = "NUnit.Framework.IgnoreAttribute";
		public const string PlatformAttribute = "NUnit.Framework.PlatformAttribute";
		public const string CultureAttribute = "NUnit.Framework.CultureAttribute";
		public const string ExplicitAttribute = "NUnit.Framework.ExplicitAttribute";
        public const string CategoryAttribute = "NUnit.Framework.CategoryAttribute";
        public const string PropertyAttribute = "NUnit.Framework.PropertyAttribute";
		public const string DescriptionAttribute = "NUnit.Framework.DescriptionAttribute";
        public const string RequiredAddinAttribute = "NUnit.Framework.RequiredAddinAttribute";

        // Attributes that apply only to Classes
        public const string TestFixtureAttribute = "NUnit.Framework.TestFixtureAttribute";
        public const string SetUpFixtureAttribute = "NUnit.Framework.SetUpFixtureAttribute";

        // Attributes that apply only to Methods
        public const string TestAttribute = "NUnit.Framework.TestAttribute";
        public const string TestCaseAttribute = "NUnit.Framework.TestCaseAttribute";
        public static readonly string SetUpAttribute = "NUnit.Framework.SetUpAttribute";
        public static readonly string TearDownAttribute = "NUnit.Framework.TearDownAttribute";
        public static readonly string FixtureSetUpAttribute = "NUnit.Framework.TestFixtureSetUpAttribute";
        public static readonly string FixtureTearDownAttribute = "NUnit.Framework.TestFixtureTearDownAttribute";
        public static readonly string ExpectedExceptionAttribute = "NUnit.Framework.ExpectedExceptionAttribute";

        // Attributes that apply only to Properties
        public static readonly string SuiteAttribute = "NUnit.Framework.SuiteAttribute";
        #endregion

        #region Other Framework Types
        public static readonly string AssertException = "NUnit.Framework.AssertionException";
        public static readonly string IgnoreException = "NUnit.Framework.IgnoreException";
        public static readonly string InconclusiveException = "NUnit.Framework.InconclusiveException";
        public static readonly string SuccessException = "NUnit.Framework.SuccessException";
        public static readonly string AssertType = "NUnit.Framework.Assert";
		public static readonly string ExpectExceptionInterface = "NUnit.Framework.IExpectException";
        #endregion

        #region Core Types
        public static readonly string SuiteBuilderAttribute = typeof(SuiteBuilderAttribute).FullName;
        public static readonly string SuiteBuilderInterface = typeof(ISuiteBuilder).FullName;

        public static readonly string TestCaseBuilderAttributeName = typeof(TestCaseBuilderAttribute).FullName;
        public static readonly string TestCaseBuilderInterfaceName = typeof(ITestCaseBuilder).FullName;

        public static readonly string TestDecoratorAttributeName = typeof(TestDecoratorAttribute).FullName;
        public static readonly string TestDecoratorInterfaceName = typeof(ITestDecorator).FullName;
        #endregion

        #endregion

        #region Properties
        private static Assembly frameworkAssembly;
        private static Assembly FrameworkAssembly
        {
            get
            {
                if (frameworkAssembly == null)
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                        if (assembly.GetName().Name == "nunit.framework")
                        {
                            frameworkAssembly = assembly;
                            break;
                        }

                return frameworkAssembly;
            }
        }

        private static Version Version
        {
            get { return FrameworkAssembly.GetName().Version; }
        }
        #endregion

        #region Identify SetUp and TearDown Methods
        public static bool IsSetUpMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, NUnitFramework.SetUpAttribute, false);
        }

        public static bool IsTearDownMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, NUnitFramework.TearDownAttribute, false);
        }

        public static bool IsFixtureSetUpMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, NUnitFramework.FixtureSetUpAttribute, false);
        }

        public static bool IsFixtureTearDownMethod(MethodInfo method)
        {
            return Reflect.HasAttribute(method, NUnitFramework.FixtureTearDownAttribute, false);
        }

        #endregion

        #region Locate SetUp and TearDown Methods
        public static MethodInfo GetSetUpMethod(Type fixtureType)
        {
            return Reflect.GetMethodWithAttribute(fixtureType, SetUpAttribute, true);
        }

        public static MethodInfo GetTearDownMethod(Type fixtureType)
		{
			return Reflect.GetMethodWithAttribute(fixtureType, TearDownAttribute, true);
		}

		public static MethodInfo GetFixtureSetUpMethod(Type fixtureType)
		{
			return Reflect.GetMethodWithAttribute(fixtureType, FixtureSetUpAttribute, true);
		}

        public static MethodInfo GetFixtureTearDownMethod(Type fixtureType)
		{
			return Reflect.GetMethodWithAttribute(fixtureType, FixtureTearDownAttribute, true);
		}
		#endregion

        #region Check SetUp and TearDown Signatures
        /// <summary>
        /// Check any SetUp method on fixture for validity
        /// </summary>
        /// <param name="fixtureType">The type to be checked</param>
        /// <param name="reason">The reason for any failure</param>
        /// <returns>True if the method is either not present or is present only once with a valid signature</returns>
        public static bool IsSetUpMethodValid(Type fixtureType, ref string reason)
        {
            if (Reflect.CountMethodsWithAttribute(fixtureType, SetUpAttribute, true) > 1)
            {
                reason = "More than one SetUp method";
                return false;
            }

            MethodInfo theMethod = Reflect.GetMethodWithAttribute(fixtureType, SetUpAttribute, true);
            if (theMethod != null && !CheckSetUpTearDownSignature(theMethod))
            {
                reason = "Invalid SetUp method signature";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check any TearDown method on fixture for validity
        /// </summary>
        /// <param name="fixtureType">The type to be checked</param>
        /// <param name="reason">The reason for any failure</param>
        /// <returns>True if the method is either not present or is present only once with a valid signature</returns>
        public static bool IsTearDownMethodValid(Type fixtureType, ref string reason)
        {
            if (Reflect.CountMethodsWithAttribute(fixtureType, TearDownAttribute, true) > 1)
            {
                reason = "More than one TearDown method";
                return false;
            }

            MethodInfo theMethod = Reflect.GetMethodWithAttribute(fixtureType, TearDownAttribute, true);
            if (theMethod != null && !CheckSetUpTearDownSignature(theMethod))
            {
                reason = "Invalid TearDown method signature";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check any TestFixtureSetUp method on fixture for validity
        /// </summary>
        /// <param name="fixtureType">The type to be checked</param>
        /// <param name="reason">The reason for any failure</param>
        /// <returns>True if the method is either not present or is present only once with a valid signature</returns>
        public static bool IsFixtureSetUpMethodValid(Type fixtureType, ref string reason)
        {
            if (Reflect.CountMethodsWithAttribute(fixtureType, FixtureSetUpAttribute, true) > 1)
            {
                reason = "More than one TestFixtureSetUp method";
                return false;
            }

            MethodInfo theMethod = Reflect.GetMethodWithAttribute(fixtureType, FixtureSetUpAttribute, true);
            if (theMethod != null && !CheckSetUpTearDownSignature(theMethod))
            {
                reason = "Invalid TestFixtureSetUp method signature";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check any TestFixtureTearDown method on fixture for validity
        /// </summary>
        /// <param name="fixtureType">The type to be checked</param>
        /// <param name="reason">The reason for any failure</param>
        /// <returns>True if the method is either not present or is present only once with a valid signature</returns>
        public static bool IsFixtureTearDownMethodValid(Type fixtureType, ref string reason)
        {
            if (Reflect.CountMethodsWithAttribute(fixtureType, FixtureTearDownAttribute, true) > 1)
            {
                reason = "More than one TestFixtureTearDown method";
                return false;
            }

            MethodInfo theMethod = Reflect.GetMethodWithAttribute(fixtureType, FixtureTearDownAttribute, true);
            if (theMethod != null && !CheckSetUpTearDownSignature(theMethod))
            {
                reason = "Invalid TestFixtureTearDown method signature";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Internal helper to check a single setup or teardown method
        /// </summary>
        /// <param name="theMethod">The method to be checked</param>
        /// <returns>True if the method has a valid signature</returns>
        private static bool CheckSetUpTearDownSignature(MethodInfo theMethod)
        {
            return !theMethod.IsAbstract &&
                   (theMethod.IsPublic || theMethod.IsFamily) &&
                    theMethod.GetParameters().Length == 0 &&
                    theMethod.ReturnType.Equals(typeof(void));
        }
        #endregion

        #region Get Special Properties of Attributes

        #region IgnoreReason
        public static string GetIgnoreReason( System.Attribute attribute )
		{
			return Reflect.GetPropertyValue( attribute, "Reason" ) as string;
		}
		#endregion

		#region Description
		/// <summary>
		/// Method to return the description from an source
		/// </summary>
		/// <param name="source">The source to check</param>
		/// <returns>The description, if any, or null</returns>
		public static string GetDescription(System.Attribute attribute)
		{
			return Reflect.GetPropertyValue( attribute, "Description" ) as string;
		}
		#endregion

		#endregion

		#region ApplyCommonAttributes
        /// <summary>
        /// Modify a newly constructed test based on a type or method by 
        /// applying any of NUnit's common attributes.
        /// </summary>
        /// <param name="member">The type or method from which the test was constructed</param>
        /// <param name="test">The test to which the attributes apply</param>
        public static void ApplyCommonAttributes(MemberInfo member, Test test)
        {
            ApplyCommonAttributes( Reflect.GetAttributes( member, false ), test );
        }

        /// <summary>
        /// Modify a newly constructed test based on an assembly by applying 
        /// any of NUnit's common attributes.
        /// </summary>
        /// <param name="assembly">The assembly from which the test was constructed</param>
        /// <param name="test">The test to which the attributes apply</param>
        public static void ApplyCommonAttributes(Assembly assembly, Test test)
        {
            ApplyCommonAttributes( Reflect.GetAttributes( assembly, false ), test );
        }

        /// <summary>
        /// Modify a newly constructed test by applying any of NUnit's common
        /// attributes, based on an input array of attributes. This method checks
        /// for all attributes, relying on the fact that specific attributes can only
        /// occur on those constructs on which they are allowed.
        /// </summary>
        /// <param name="attributes">An array of attributes possibly including NUnit attributes
        /// <param name="test">The test to which the attributes apply</param>
        public static void ApplyCommonAttributes(Attribute[] attributes, Test test)
        {
            foreach (Attribute attribute in attributes)
            {
				Type attributeType = attribute.GetType();
				string attributeName = attributeType.FullName;
                bool isValid = test.RunState != RunState.NotRunnable;

                switch (attributeName)
                {
					case TestFixtureAttribute:
					case TestAttribute:
						if ( test.Description == null )
							test.Description = GetDescription( attribute );
						break;
					case DescriptionAttribute:
						test.Description = GetDescription( attribute );
						break;
					case ExplicitAttribute:
                        if (isValid)
                        {
                            test.RunState = RunState.Explicit;
                            test.IgnoreReason = GetIgnoreReason(attribute);
                        }
                        break;
                    case IgnoreAttribute:
                        if (isValid)
                        {
                            test.RunState = RunState.Ignored;
                            test.IgnoreReason = GetIgnoreReason(attribute);
                        }
                        break;
                    case PlatformAttribute:
                        PlatformHelper pHelper = new PlatformHelper();
                        if (isValid && !pHelper.IsPlatformSupported(attribute))
                        {
                            test.RunState = RunState.Skipped;
                            test.IgnoreReason = GetIgnoreReason(attribute);
							if ( test.IgnoreReason == null )
								test.IgnoreReason = pHelper.Reason;
                        }
                        break;
					case CultureAttribute:
						CultureDetector cultureDetector = new CultureDetector();
						if (isValid && !cultureDetector.IsCultureSupported(attribute))
						{
							test.RunState = RunState.Skipped;
							test.IgnoreReason = cultureDetector.Reason;
						}
						break;
                    case RequiredAddinAttribute:
                        string required = (string)Reflect.GetPropertyValue(attribute,"RequiredAddin");
                        if (!IsAddinAvailable(required))
                        {
                            test.RunState = RunState.NotRunnable;
                            test.IgnoreReason = string.Format("Required addin {0} not available", required);
                        }
                        break;
					default:
						if ( Reflect.InheritsFrom( attributeType, CategoryAttribute ) )
						{	
							test.Categories.Add( Reflect.GetPropertyValue( attribute, "Name" ) );
						}
						else if ( Reflect.InheritsFrom( attributeType, PropertyAttribute ) )
						{
							string name = (string)Reflect.GetPropertyValue( attribute, "Name" );
							if ( name != null && name != string.Empty )
							{
								object val = Reflect.GetPropertyValue( attribute, "Value" );
								test.Properties[name] = val;
							}
						}
						break;
                }
            }
        }
		#endregion

        #region ApplyExpectedExceptionAttribute
        /// <summary>
        /// Modify a newly constructed test by checking for ExpectedExceptionAttribute
        /// and setting properties on the test accordingly.
        /// </summary>
        /// <param name="attributes">An array of attributes possibly including NUnit attributes
        /// <param name="test">The test to which the attributes apply</param>
        public static void ApplyExpectedExceptionAttribute(MethodInfo method, TestMethod testMethod)
        {
            Attribute attribute = Reflect.GetAttribute(
                method, NUnitFramework.ExpectedExceptionAttribute, false);

            if (attribute != null)
                testMethod.ExceptionProcessor = new ExpectedExceptionProcessor(testMethod, attribute);
        }

        #endregion

        #region IsSuiteBuilder
        public static bool IsSuiteBuilder( Type type )
		{
			return Reflect.HasAttribute( type, SuiteBuilderAttribute, false )
				&& Reflect.HasInterface( type, SuiteBuilderInterface );
		}
		#endregion

		#region IsTestCaseBuilder
		public static bool IsTestCaseBuilder( Type type )
		{
			return Reflect.HasAttribute( type, TestCaseBuilderAttributeName, false )
				&& Reflect.HasInterface( type, TestCaseBuilderInterfaceName );
		}
		#endregion

		#region IsTestDecorator
		public static bool IsTestDecorator( Type type )
		{
			return Reflect.HasAttribute( type, TestDecoratorAttributeName, false )
				&& Reflect.HasInterface( type, TestDecoratorInterfaceName );
		}
		#endregion

        #region IsAddinAvailable
        public static bool IsAddinAvailable(string name)
        {
            foreach (Addin addin in CoreExtensions.Host.AddinRegistry.Addins)
                if (addin.Name == name && addin.Status == AddinStatus.Loaded)
                    return true;

            return false;
        }
        #endregion

        #region Framework Assert Access

        /// <summary>
        /// NUnitFramework.Assert is a nested class that implements
        /// a few of the framework operations by reflection, 
        /// using whatever framework version is available.
        /// </summary>
        public class Assert
        {
            #region Properties
            private static Type assertType;
            private static Type AssertType
            {
                get
                {
                    if (assertType == null)
                        assertType = FrameworkAssembly.GetType(NUnitFramework.AssertType);

                    return assertType;
                }
            }

            private static MethodInfo areEqualMethod;
            private static MethodInfo AreEqualMethod
            {
                get
                {
                    if (areEqualMethod == null)
                        areEqualMethod = AssertType.GetMethod(
                            "AreEqual", 
                            BindingFlags.Static | BindingFlags.Public, 
                            null, 
                            new Type[] { typeof(object), typeof(object) },
                            null );

                    return areEqualMethod;
                }
            }

            private static PropertyInfo counterProperty;
            private static PropertyInfo CounterProperty
            {
                get
                {
                    if (counterProperty == null)
                        counterProperty = Reflect.GetNamedProperty(
                            AssertType,
                            "Counter",
                            BindingFlags.Public | BindingFlags.Static);

                    return counterProperty;
                }
            }
            #endregion

            /// <summary>
            /// Invoke Assert.AreEqual by reflection
            /// </summary>
            /// <param name="expected">The expected value</param>
            /// <param name="actual">The actual value</param>
            public static void AreEqual(object expected, object actual)
            {
                if (AreEqualMethod != null)
                    AreEqualMethod.Invoke( null, new object[] { expected, actual });
            }

            /// <summary>
            /// Get the assertion counter. It clears itself automatically
            /// on each call.
            /// </summary>
            /// <returns>Count of number of asserts since last call</returns>
            public static int GetAssertCount()
            {
                return CounterProperty == null
                    ? 0
                    : (int)CounterProperty.GetValue(null, new object[0]);
            }
        }

        #endregion

        #region GetResultState
        /// <summary>
        /// Returns a result state for a special exception.
        /// If the exception is not handled specially, returns
        /// ResultState.Error.
        /// </summary>
        /// <param name="ex">The exception to be examined</param>
        /// <returns>A ResultState</returns>
        public static ResultState GetResultState(Exception ex)
        {
            string name = ex.GetType().FullName;

            if (name == NUnitFramework.AssertException)
                return ResultState.Failure;
            else
                if (name == NUnitFramework.IgnoreException)
                    return ResultState.Ignored;
                else
                    if (name == NUnitFramework.InconclusiveException)
                        return ResultState.Inconclusive;
                    else
                        if (name == NUnitFramework.SuccessException)
                            return ResultState.Success;
                        else
                            return ResultState.Error;
        }
        #endregion
    }
}
