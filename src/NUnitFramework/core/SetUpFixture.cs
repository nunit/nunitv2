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
	/// SetUpFixture extends NamespaceSuite and allows a namespace to have
	/// a TestFixtureSetup and TestFixtureTearDown.
	/// </summary>
	public class SetUpFixture : NamespaceSuite
	{
		public SetUpFixture( Type type, int assemblyKey ) : base( type, assemblyKey )
		{
			testName = type.Namespace;
			int index = testName.LastIndexOf( '.' );
			if ( index > 0 )
				testName = testName.Substring( index + 1 );
			// NOTE: Once again, since we are not inheriting from TestFixture,
			// no automatic construction is performed for us, so we do it here.
			this.Fixture = Reflect.Construct( type );

			this.fixtureSetUp = Reflect.GetMethodWithAttribute( 
				type, "NUnit.Framework.TestFixtureSetUpAttribute",
				BindingFlags.Public | BindingFlags.Instance );
			this.fixtureTearDown = Reflect.GetMethodWithAttribute( 
				type, "NUnit.Framework.TestFixtureTearDownAttribute",
				BindingFlags.Public | BindingFlags.Instance );
		}

		public override void DoOneTimeSetUp(TestResult suiteResult)
		{
			base.DoOneTimeSetUp (suiteResult);

			if ( fixtureSetUp != null && suiteResult.IsSuccess )
				Reflect.InvokeMethod( fixtureSetUp, this.Fixture );
		}

		public override void DoOneTimeTearDown(TestResult suiteResult)
		{
			if (fixtureTearDown != null )
				Reflect.InvokeMethod( fixtureTearDown, this.Fixture );

			base.DoOneTimeTearDown (suiteResult);
		}
	}
}
