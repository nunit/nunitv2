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
	/// A TestSuite that wraps a Visual Studio Team System test class
	/// </summary>
	public class VstsTestFixture : TestFixture
	{
		public VstsTestFixture( Type fixtureType ) : base( fixtureType )
		{
            this.testFramework = TestFramework.FromType(fixtureType);

            this.testSetUp = GetSetUpMethod();
            this.testTearDown = GetTearDownMethod();
            this.fixtureSetUp = GetFixtureSetUpMethod();
            this.fixtureTearDown = GetFixtureTearDownMethod();
        }

        #region Protected Methods

        protected virtual MethodInfo GetSetUpMethod()
        {
            return Reflect.GetMethodWithAttribute(FixtureType, "Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                true);
        }
        protected virtual MethodInfo GetTearDownMethod()
        {
            return Reflect.GetMethodWithAttribute(FixtureType, "Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                true);
        }
        protected virtual MethodInfo GetFixtureSetUpMethod()
        {
            return Reflect.GetMethodWithAttribute(FixtureType, "Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                true);
        }
        protected virtual MethodInfo GetFixtureTearDownMethod()
        {
            return Reflect.GetMethodWithAttribute(FixtureType, "Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                true);
        }
        #endregion
    }

    public class VstsTestMethod : TestMethod
    {
        #region Constructors
        public VstsTestMethod(MethodInfo method) : base(method) { }

        public VstsTestMethod(MethodInfo method,
            Type expectedException, string expectedMessage, string matchType)
            : base(method, expectedException, expectedMessage, matchType) { }

        public VstsTestMethod(MethodInfo method,
            string expectedExceptionName, string expectedMessage, string matchType)
            : base(method, expectedExceptionName, expectedMessage, matchType) { }
        #endregion
    }
}
