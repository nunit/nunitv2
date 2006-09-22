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
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// Built-in SuiteBuilder for NUnit TestFixture
	/// </summary>
	public class NUnitTestFixtureBuilder : AbstractFixtureBuilder
	{
		#region AbstractFixtureBuilder Overrides
		/// <summary>
		/// Makes an NUnitTestFixture instance
		/// </summary>
		/// <param name="type">The type to be used</param>
		/// <returns>An NUnitTestFixture as a TestSuite</returns>
		protected override TestSuite MakeSuite( Type type )
		{
			return new NUnitTestFixture( type );
		}

		/// <summary>
		/// Method that sets properties of the test suite based on the
		/// information in the provided Type.
		/// </summary>
		/// <param name="type">The type to examine</param>
		/// <param name="suite">The test suite being constructed</param>
		protected override void SetTestSuiteProperties( Type type, TestSuite suite )
		{
			base.SetTestSuiteProperties( type, suite );

            NUnitFramework.ApplyCommonAttributes( type, suite );
		}

		/// <summary>
		/// Adds test cases to the fixture. Overrides the base class 
		/// to install an NUnitTestCaseBuilder while the tests are
		/// being added.
		/// </summary>
		/// <param name="fixtureType">The type of the fixture</param>
		protected override void AddTestCases(Type fixtureType)
		{
			using( new AddinState() )
			{
				Addins.Register( new NUnitTestCaseBuilder() );
				base.AddTestCases (fixtureType);
			}
		}

        /// <summary>
        /// Checks to see if the fixture type has the test fixture
        /// attribute type specified in the parameters. Override
        /// to allow additional types - based on name, for example.
        /// </summary>
        /// <param name="type">The fixture type to check</param>
        /// <returns>True if the fixture can be built, false if not</returns>
        public override bool CanBuildFrom(Type type)
        {
            return Reflect.HasAttribute( type, NUnitFramework.TestFixtureAttribute, true );
        }

        /// <summary>
        /// Check that the fixture is valid. In addition to the base class
        /// check for a valid constructor, this method ensures that there 
        /// is no more than one of each setup or teardown method and that
        /// their signatures are correct.
        /// </summary>
        /// <param name="fixtureType">The type of the fixture to check</param>
        /// <param name="reason">A message indicating why the fixture is invalid</param>
        /// <returns>True if the fixture is valid, false if not</returns>
        protected override bool IsValidFixtureType(Type fixtureType, ref string reason)
        {
            if (!base.IsValidFixtureType(fixtureType, ref reason))
                return false;

            if (!fixtureType.IsPublic && !fixtureType.IsNestedPublic)
            {
                reason = "Fixture class is not public";
                return false;
            }

            return CheckSetUpTearDownMethod(fixtureType, "SetUp", NUnitFramework.SetUpAttribute, ref reason)
                && CheckSetUpTearDownMethod(fixtureType, "TearDown", NUnitFramework.TearDownAttribute, ref reason)
                && CheckSetUpTearDownMethod(fixtureType, "TestFixtureSetUp", NUnitFramework.FixtureSetUpAttribute, ref reason)
                && CheckSetUpTearDownMethod(fixtureType, "TestFixtureTearDown", NUnitFramework.FixtureTearDownAttribute, ref reason);
        }

        /// <summary>
        /// Internal helper to check a single setup or teardown method
        /// </summary>
        /// <param name="fixtureType">The type to be checked</param>
        /// <param name="attributeName">The short name of the attribute to be checked</param>
        /// <returns>True if the method is present no more than once and has a valid signature</returns>
        private bool CheckSetUpTearDownMethod(Type fixtureType, string name, string attributeName, ref string reason)
        {
            int count = Reflect.CountMethodsWithAttribute(
                fixtureType, attributeName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly,
                true);

            if (count == 0) return true;

            if (count > 1)
            {
                reason = string.Format("More than one {0} method", name);
                return false;
            }

            MethodInfo theMethod = Reflect.GetMethodWithAttribute(
                fixtureType, attributeName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly,
                true);

            if (theMethod != null)
            {
                if (theMethod.IsStatic ||
                    theMethod.IsAbstract ||
                    !theMethod.IsPublic && !theMethod.IsFamily ||
                    theMethod.GetParameters().Length != 0 ||
                    !theMethod.ReturnType.Equals(typeof(void)))
                {
                    reason = string.Format("Invalid {0} method signature", name);
                    return false;
                }
            }

            return true;
        }
		#endregion
    }
}