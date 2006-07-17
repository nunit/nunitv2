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

using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// Represents a test suite constructed from a type that has a static Suite property
	/// </summary>
	public class LegacySuite : TestSuite
	{
		#region Private Fields

		private PropertyInfo suiteProperty;

		#endregion

		#region Static Methods

		public static PropertyInfo GetSuiteProperty( Type testClass )
		{
			if( testClass == null )
				return null;

			PropertyInfo property = Reflect.GetPropertyWithAttribute( 
				testClass, 
				NUnitFramework.SuiteAttribute,
				BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly );

			if ( property == null )
				return null;

			MethodInfo method = property.GetGetMethod(true);

			if( method.ReturnType.FullName != "NUnit.Core.TestSuite" )
				return null;
				//throw new InvalidSuiteException("Invalid suite property method signature");
			if( method.GetParameters().Length > 0 )
				return null;
				//throw new InvalidSuiteException("Invalid suite property method signature");

			return property;
		}

		#endregion

		#region Constructors

		public LegacySuite( Type fixtureType ) : base( fixtureType )
		{
			suiteProperty = GetSuiteProperty( fixtureType );

			this.fixtureSetUp = NUnitFramework.GetFixtureSetUpMethod( fixtureType );
			this.fixtureTearDown = NUnitFramework.GetFixtureTearDownMethod( fixtureType );
			
			MethodInfo method = suiteProperty.GetGetMethod(true);
			
			if( method.ReturnType.FullName != "NUnit.Core.TestSuite" || method.GetParameters().Length > 0 )
			{
				this.RunState = RunState.NotRunnable;
				this.IgnoreReason = "Invalid suite property method signature";
			}
			else
			{
				TestSuite suite = (TestSuite)suiteProperty.GetValue(null, new Object[0]);		
				foreach( Test test in suite.Tests )
					this.Add( test );
			}
		}

		#endregion

		#region Properties

		public override bool IsFixture
		{
			get { return false; }
		}


		#endregion
	}
}
