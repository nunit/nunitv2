#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Core
{
	using System;
	using System.Reflection;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Configuration;
	using NUnit.Framework;

	/// <summary>
	/// Summary description for TestCaseBuilder.
	/// </summary>
	public class TestCaseBuilder
	{
		private static readonly Type TestType = typeof( NUnit.Framework.TestAttribute );
		private static readonly Type IgnoreType = typeof( NUnit.Framework.IgnoreAttribute );
		private static readonly Type ExplicitType = typeof( NUnit.Framework.ExplicitAttribute );
		private static readonly Type CategoryType = typeof( NUnit.Framework.CategoryAttribute );
		private static readonly Type PlatformType = typeof( NUnit.Framework.PlatformAttribute );

		private static Hashtable builders;
		private static NormalBuilder normalBuilder = new NormalBuilder();

		private static bool allowOldStyleTests;

		private static void InitBuilders() 
		{
			builders = new Hashtable();
			builders[typeof(NUnit.Framework.ExpectedExceptionAttribute)] = new ExpectedExceptionBuilder();
		}

		static TestCaseBuilder()
		{
			NameValueCollection settings = (NameValueCollection)
				ConfigurationSettings.GetConfig( "NUnit/TestCaseBuilder" );

			try
			{
				string oldStyle = settings["OldStyleTestCases"];
				if ( oldStyle != null )
					allowOldStyleTests = Boolean.Parse( oldStyle );
			}
			catch
			{
				// Use default values
			}
		}

		private static TestCase MakeTestCase(Type fixtureType, MethodInfo method) 
		{
			if (builders == null)
				InitBuilders();

			object[] attributes = method.GetCustomAttributes(false);
			
			foreach (object attribute in attributes) 
			{
				ITestBuilder builder = GetBuilder(attribute);
				if (builder != null)
					return builder.Make (fixtureType, method, attribute);
			}

			return normalBuilder.Make (fixtureType, method);
		}

		private static ITestBuilder GetBuilder(object attribute)
		{
			Type attributeType = attribute.GetType();
			ITestBuilder builder = (ITestBuilder) builders[attribute.GetType()];
			if (builder == null)
			{
				object[] attributes = attributeType.GetCustomAttributes(typeof(TestBuilderAttribute), false);
				if (attributes != null && attributes.Length > 0)
				{
					TestBuilderAttribute builderAttribute = (TestBuilderAttribute) attributes[0];
					Type builderType = builderAttribute.BuilderType;
					builder = (ITestBuilder) Activator.CreateInstance(builderType);
					builders[attributeType] = builder;
				}
			}

			return builder;
		}

		/// <summary>
		/// Make a test case from a given fixture type and method
		/// </summary>
		/// <param name="fixtureType">The fixture type</param>
		/// <param name="method">MethodInfo for the particular method</param>
		/// <returns>A test case or null</returns>
		public static TestCase Make(Type fixtureType, MethodInfo method)
		{
			TestCase testCase = null;

			TestAttribute testAttribute = (TestAttribute)Reflect.GetAttribute( method, TestType, false );
			if( testAttribute != null || allowOldStyleTests && IsObsoleteTestMethod( method ) )
			{
				if (!method.IsStatic &&
					!method.IsAbstract &&
					 method.IsPublic &&
					 method.GetParameters().Length == 0 &&
					 method.ReturnType.Equals(typeof(void) ) )
				{
					testCase = MakeTestCase(fixtureType, method);

					object[] platformAttributes = method.GetCustomAttributes( PlatformType, false );
					if ( platformAttributes.Length > 0 )
					{
						PlatformHelper helper = new PlatformHelper();
						if ( !helper.IsPlatformSupported( (PlatformAttribute[])platformAttributes ) )
						{
							testCase.ShouldRun = false;
							testCase.IgnoreReason = "Not running on correct platform";
						}
					}

					IgnoreAttribute ignoreAttribute = (IgnoreAttribute)
						Reflect.GetAttribute( method, typeof( IgnoreAttribute ), false );
					if ( ignoreAttribute != null )
					{
						testCase.ShouldRun = false;
						testCase.IgnoreReason = ignoreAttribute.Reason;
					}

					object[] categoryAttributes = method.GetCustomAttributes( CategoryType, false );
					if ( categoryAttributes.Length > 0 )
					{
						ArrayList categories = new ArrayList();
						foreach(CategoryAttribute categoryAttribute in categoryAttributes) 
						{
							CategoryManager.Add( categoryAttribute.Name );
							categories.Add( categoryAttribute.Name );
						}
						testCase.Categories = categories;
					}

					testCase.IsExplicit = Reflect.HasAttribute( method, ExplicitType, false );
					
					if ( testAttribute != null )
						testCase.Description = testAttribute.Description;
				}
				else
				{
					testCase = new NotRunnableTestCase(method);
				}
			}

			return testCase;
		}

		private static bool IsObsoleteTestMethod(MethodInfo methodToCheck)
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
	}
}

