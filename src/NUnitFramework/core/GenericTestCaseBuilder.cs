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

	/// <summary>
	/// GenericTestCaseBuilder is a parameterized builder of test cases
	/// for use by test fixtures.
	/// 
	/// REFACTORING NOTE: 
	/// 
	/// This class started out with static members and has been
	/// refactored to have instance members and variables so that it
	/// can be used with CSUnit and VSTS test fixtures.
	/// 
	/// In it's current state, the class has a dual role. It is
	/// both an ITestCaseBuilder and a dispatcher that uses
	/// other ITestCaseBuilder objects if it can't build the
	/// test case itself. This is an intermediate refactoring
	/// stage and the two functions will ultimately be separated.
	/// </summary>
	public class GenericTestCaseBuilder : ITestCaseBuilder
	{
		protected TestFixtureParameters parms;

		public GenericTestCaseBuilder( TestFixtureParameters parms )
		{
			this.parms = parms;
		}

		//Backward Compatiblility
//		public TestCase Make( Type fixtureType, MethodInfo method )
//		{
//			return BuildFrom( method );
//		}

//		/// <summary>
//		/// Make a test case from a given fixture type and method
//		/// </summary>
//		/// <param name="fixtureType">The fixture type</param>
//		/// <param name="method">MethodInfo for the particular method</param>
//		/// <returns>A test case or null</returns>
//		public TestCase Make(Type fixtureType, MethodInfo method)
//		{
//			TestCase testCase = null;
//
//			Attribute testAttribute = Reflect.GetAttribute( method, parms.TestType, false );
//			if( testAttribute != null || IsOldStyleTestMethod( method ) )
//			{
//				if ( HasValidTestCaseSignature( method ) )
//				{
//					testCase =	AddinManager.Addins.BuildFrom( method );
//					if ( testCase == null )
//						testCase = AddinManager.Builtins.BuildFrom( method );
//					if ( testCase == null )
//						testCase = this.BuildFrom( method );
//
//					Attribute platformAttribute = 
//						Reflect.GetAttribute( method, parms.PlatformType, false );
//					if ( platformAttribute != null )
//					{
//						PlatformHelper helper = new PlatformHelper();
//						if ( !helper.IsPlatformSupported( platformAttribute ) )
//						{
//							testCase.ShouldRun = false;
//							testCase.IgnoreReason = "Not running on correct platform";
//						}
//					}
//
//					Attribute ignoreAttribute =
//						Reflect.GetAttribute( method, parms.IgnoreType, false );
//					if ( ignoreAttribute != null )
//					{
//						testCase.ShouldRun = false;
//						testCase.IgnoreReason = (string)
//							Reflect.GetPropertyValue( 
//								ignoreAttribute, 
//								"Reason",
//								BindingFlags.Public | BindingFlags.Instance );
//					}
//
//					Attribute[] categoryAttributes = 
//						Reflect.GetAttributes( method, parms.CategoryType, false );
//					if ( categoryAttributes.Length > 0 )
//					{
//						ArrayList categories = new ArrayList();
//						foreach( Attribute categoryAttribute in categoryAttributes) 
//						{
//							string category = (string)
//								Reflect.GetPropertyValue( 
//									categoryAttribute, 
//									"Name",
//									BindingFlags.Public | BindingFlags.Instance );
//							CategoryManager.Add( category );
//							categories.Add( category );
//						}
//						testCase.Categories = categories;
//					}
//
//					testCase.IsExplicit = Reflect.HasAttribute( method, parms.ExplicitType, false );
//				
//					if ( testAttribute != null )
//						testCase.Description = (string)Reflect.GetPropertyValue( 
//							testAttribute, 
//							"Description", 
//							BindingFlags.Public | BindingFlags.Instance );
//				}
//				else
//				{
//					testCase = new NotRunnableTestCase(method);
//				}
//			}
//
//			return testCase;
//		}

		#region ITestCaseBuilder Members

		public bool CanBuildFrom(MethodInfo method)
		{
			return Reflect.HasAttribute( method, parms.TestType, false )
				|| IsOldStyleTestMethod( method );
		}

		public TestCase BuildFrom(MethodInfo method)
		{
			TestCase testCase = null;

			Attribute testAttribute = Reflect.GetAttribute( method, parms.TestType, false );
			if( testAttribute != null || IsOldStyleTestMethod( method ) )
			{
				if ( HasValidTestCaseSignature( method ) )
				{
					Attribute attribute = Reflect.GetAttribute( method, parms.ExpectedExceptionType, false );
					if ( attribute != null )
					{
						Type expectedException = (System.Type)Reflect.GetPropertyValue( 
							attribute, "ExceptionType",
							BindingFlags.Public | BindingFlags.Instance );
						string expectedMessage = (string)Reflect.GetPropertyValue(
							attribute, "ExpectedMessage",
							BindingFlags.Public | BindingFlags.Instance );
						testCase = new ExpectedExceptionTestCase( 
							method, expectedException, expectedMessage );
					}
					else
						testCase = new NormalTestCase( method );

					Attribute platformAttribute = 
						Reflect.GetAttribute( method, parms.PlatformType, false );
					if ( platformAttribute != null )
					{
						PlatformHelper helper = new PlatformHelper();
						if ( !helper.IsPlatformSupported( platformAttribute ) )
						{
							testCase.ShouldRun = false;
							testCase.IgnoreReason = "Not running on correct platform";
						}
					}

					Attribute ignoreAttribute =
						Reflect.GetAttribute( method, parms.IgnoreType, false );
					if ( ignoreAttribute != null )
					{
						testCase.ShouldRun = false;
						testCase.IgnoreReason = (string)
							Reflect.GetPropertyValue( 
							ignoreAttribute, 
							"Reason",
							BindingFlags.Public | BindingFlags.Instance );
					}

					Attribute[] categoryAttributes = 
						Reflect.GetAttributes( method, parms.CategoryType, false );
					if ( categoryAttributes.Length > 0 )
					{
						ArrayList categories = new ArrayList();
						foreach( Attribute categoryAttribute in categoryAttributes) 
						{
							string category = (string)
								Reflect.GetPropertyValue( 
								categoryAttribute, 
								"Name",
								BindingFlags.Public | BindingFlags.Instance );
							CategoryManager.Add( category );
							categories.Add( category );
						}
						testCase.Categories = categories;
					}

					testCase.IsExplicit = Reflect.HasAttribute( method, parms.ExplicitType, false );
				
					if ( testAttribute != null )
						testCase.Description = (string)Reflect.GetPropertyValue( 
							testAttribute, 
							"Description", 
							BindingFlags.Public | BindingFlags.Instance );
				}
				else
				{
					testCase = new NotRunnableTestCase( method );
				}
			}

			return testCase;
		}

		#endregion

		protected bool HasValidTestCaseSignature( MethodInfo method )
		{
			return !method.IsStatic
				&& !method.IsAbstract
				&& method.IsPublic
				&& method.GetParameters().Length == 0
				&& method.ReturnType.Equals(typeof(void) );
		}

		protected virtual bool IsOldStyleTestMethod( MethodInfo method )
		{
			return false;
		}
	}
}
