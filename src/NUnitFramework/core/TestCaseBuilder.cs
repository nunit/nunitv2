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

	/// <summary>
	/// Summary description for TestCaseBuilder.
	/// </summary>
	public class TestCaseBuilder
	{
		private static Hashtable builders;
		private static ITestBuilder normalBuilder = new NormalBuilder();

		private static void InitBuilders() 
		{
			builders = new Hashtable();
			builders[typeof(NUnit.Framework.ExpectedExceptionAttribute)] = new ExpectedExceptionBuilder();
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

			if( Reflect.HasTestAttribute(method) || Reflect.IsObsoleteTestMethod( method ) )
			{
				if( Reflect.IsTestMethodSignatureCorrect( method ) )
				{
					testCase = MakeTestCase(fixtureType, method);

					if(Reflect.HasIgnoreAttribute(method))
					{
						testCase.ShouldRun = false;
						testCase.IgnoreReason = Reflect.GetIgnoreReason(method);
					}

					if (Reflect.HasCategoryAttribute(method)) 
					{
						IList categories = Reflect.GetCategories(method);
						CategoryManager.Add(categories);
						testCase.Categories = categories;
					}

					testCase.IsExplicit = Reflect.HasExplicitAttribute(method);

					testCase.Description = Reflect.GetDescription(method);
				}
				else
				{
					testCase = new NotRunnableTestCase(method);
				}
			}

			return testCase;
		}

		#region Make Test Cases with pre-created fixtures

		// TODO: These methods are only used by our tests, since we no longer
		// create the fixture in advance. They should be phased out.

		public static TestCase Make(object fixture, MethodInfo method)
		{
			TestCase testCase = Make( fixture.GetType(), method );
			testCase.Fixture = fixture;

			return testCase;
		}

		public static TestCase Make(object fixture, string methodName)
		{
			MethodInfo method = Reflect.GetMethod( fixture.GetType(), methodName );
			if ( method != null )
				return Make(fixture, method);

			return null;
		}

		#endregion
	}
}

