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
    /// Class to implement an NUnit test fixture
    /// </summary>
    public class NUnitTestFixture : TestFixture
    {
        public NUnitTestFixture(Type fixtureType) : base( fixtureType)
        {
            this.testSetUp = NUnitFramework.GetSetUpMethod( fixtureType );
            this.testTearDown = NUnitFramework.GetTearDownMethod( fixtureType );
            this.fixtureSetUp = NUnitFramework.GetFixtureSetUpMethod( fixtureType );
            this.fixtureTearDown = NUnitFramework.GetFixtureTearDownMethod( fixtureType );
        }

        protected override void DoOneTimeSetUp(TestResult suiteResult)
        {
            base.DoOneTimeSetUp(suiteResult);

			suiteResult.AssertCount = NUnitFramework.GetAssertCount(); ;
        }

        protected override void DoOneTimeTearDown(TestResult suiteResult)
        {
            base.DoOneTimeTearDown(suiteResult);

			suiteResult.AssertCount += NUnitFramework.GetAssertCount();
        }
    }
}
