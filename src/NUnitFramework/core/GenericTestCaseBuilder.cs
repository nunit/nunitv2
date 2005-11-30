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
	using System.Text.RegularExpressions;

	/// <summary>
	/// GenericTestCaseBuilder is a parameterized builder of test cases
	/// for use by test fixtures. It is the base for NUnitTestCaseBuilder, 
	/// CSUnitTestCaseBuilder and VstsTestCaseBuilder.
	/// </summary>
	public class GenericTestCaseBuilder : AbstractTestCaseBuilder
	{
		#region Fields
		protected TestFixtureParameters parms;
		#endregion

		#region Constructor
		public GenericTestCaseBuilder( TestFixtureParameters parms )
		{
			this.parms = parms;
		}
		#endregion

		#region AbstractTestCaseBuilder Overrides
		public override bool CanBuildFrom(MethodInfo method)
		{
			if( parms.HasTestCaseType )
			{
				if( Reflect.HasAttribute( method, parms.TestCaseType, parms.InheritTestCaseType ) )
					return true;
			}

			if( parms.HasTestCasePattern )
			{
				Regex regex = new Regex( parms.TestCasePattern );
				if ( regex.Match( method.Name ).Success && !IsSpecialMethod( method ) )
					return true;
			}
		
			return false;
		}

		protected override TestCase MakeTestCase( MethodInfo method )
		{
			Type expectedException = null;
			string expectedExceptionName = null;
			string expectedMessage = null;

			if( parms.HasExpectedExceptionType )
			{
				Attribute attribute = Reflect.GetAttribute( method, parms.ExpectedExceptionType, false );
				if ( attribute != null )
				{
					expectedException = (System.Type)Reflect.GetPropertyValue( 
						attribute, "ExceptionType",
						BindingFlags.Public | BindingFlags.Instance );
					expectedExceptionName = (string)Reflect.GetPropertyValue(
						attribute, "ExceptionName",
						BindingFlags.Public | BindingFlags.Instance );
					expectedMessage = (string)Reflect.GetPropertyValue(
						attribute, "ExpectedMessage",
						BindingFlags.Public | BindingFlags.Instance );
				}
			}

			return expectedException != null
				? new TestMethod( method, expectedException, expectedMessage )
				: new TestMethod( method, expectedExceptionName, expectedMessage );
		}

		/// <summary>
		/// Checks to see if the test is runnable by looking for the ignore 
		/// attribute specified in the parameters.
		/// </summary>
		/// <param name="method">The method to be checked</param>
		/// <param name="reason">A message indicating why the fixture is not runnable</param>
		/// <returns>True if runnable, false if not</returns>
		protected override bool IsRunnable( MethodInfo method, ref string reason )
		{
			if ( parms.HasIgnoreType )
			{
				Attribute ignoreAttribute =
					Reflect.GetAttribute( method, parms.IgnoreType, false );
				if ( ignoreAttribute != null )
				{
					reason = (string)Reflect.GetPropertyValue(
						ignoreAttribute, 
						"Reason",
						BindingFlags.Public | BindingFlags.Instance );
					return false;
				}
			}

			return true;
		}

		protected override string GetTestCaseDescription( MethodInfo method )
		{
			if ( parms.HasTestCaseType )
			{
				Attribute testAttribute = Reflect.GetAttribute( method, parms.TestCaseType, false );
				if ( testAttribute != null )
					return (string)Reflect.GetPropertyValue( 
						testAttribute, 
						"Description", 
						BindingFlags.Public | BindingFlags.Instance );
			}

			return null;
		}
		#endregion

		#region Virtual Methods
		protected virtual bool IsSpecialMethod( MethodInfo method )
		{
			return IsSetUpMethod( method )
				|| IsTearDownMethod( method )
				|| IsFixtureSetUpMethod( method )
				|| IsFixtureTearDownMethod( method );
		}

		protected virtual bool IsSetUpMethod( MethodInfo method )
		{
			return parms.HasSetUpType
				&& Reflect.HasAttribute( method, parms.SetUpType, false );
		}
		protected virtual bool IsTearDownMethod( MethodInfo method )
		{
			return parms.HasTearDownType
				&& Reflect.HasAttribute( method, parms.TearDownType, false );
		}
		protected virtual bool IsFixtureSetUpMethod( MethodInfo method )
		{
			return parms.HasFixtureSetUpType
				&& Reflect.HasAttribute( method, parms.FixtureSetUpType, false );
		}
		protected virtual bool IsFixtureTearDownMethod( MethodInfo method )
		{
			return parms.HasFixtureTearDownType
				&& Reflect.HasAttribute( method, parms.FixtureTearDownType, false );
		}
		#endregion
	}
}
