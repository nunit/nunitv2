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

namespace NUnit.Core.Builders
{
	/// <summary>
	/// Builder for csunit test fixtures
	/// </summary>
	public class CSUnitTestFixtureBuilder : AbstractFixtureBuilder
	{
		#region AbstractFixtureBuilder Overrides
		/// <summary>
		/// Returns a CSUnitTestFixture
		/// </summary>
		/// <param name="type">The type to use in making the fixture</param>
		/// <returns>A TestSuite or null</returns>
		protected override TestSuite MakeSuite( Type type )
		{
			return new CSUnitTestFixture( type );
		}
		
		/// <summary>
		/// Adds test cases to the fixture. Overrides the base class 
		/// to install a CSUnitTestCaseBuilder while the tests are
		/// being added.
		/// </summary>
		/// <param name="fixtureType">The type of the fixture</param>
		protected override void AddTestCases(Type fixtureType)
		{
			using( new AddinState() )
			{
				Addins.Register( new CSUnitTestCaseBuilder() );
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
            if (Reflect.HasAttribute(type, "csUnit.TestFixtureAttribute", true))
                return true;

            return type.Name.StartsWith("Test");
        }

        /// <summary>
        /// Checks to see if the fixture is runnable by looking for the ignore 
        /// attribute specified in the parameters.
        /// </summary>
        /// <param name="fixtureType">The type to be checked</param>
        /// <param name="reason">A message indicating why the fixture is not runnable</param>
        /// <returns>True if runnable, false if not</returns>
        protected override bool IsRunnable(Type fixtureType, ref string reason)
        {
            Attribute ignoreAttribute =
                Reflect.GetAttribute(fixtureType, "csUnit.IgnoreAttribute", false);
            if (ignoreAttribute != null)
            {
                reason = (string)Reflect.GetPropertyValue(
                    ignoreAttribute,
                    "Reason",
                    BindingFlags.Public | BindingFlags.Instance);
                return false;
            }

            return true;
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

            if (!CheckSetUpTearDownMethod(fixtureType, "csUnit.SetUpAttribute"))
            {
                reason = "Invalid SetUp method signature";
                return false;
            }

            if (!CheckSetUpTearDownMethod(fixtureType, "csUnit.TearDownAttribute"))
            {
                reason = "Invalid TearDown method signature";
                return false;
            }

            if (!CheckSetUpTearDownMethod(fixtureType, "csUnit.FixtureSetUpAttribute"))
            {
                reason = "Invalid TestFixtureSetUp method signature";
                return false;
            }

            if (!CheckSetUpTearDownMethod(fixtureType, "csUnit.FixtureTearDownAttribute"))
            {
                reason = "Invalid TestFixtureTearDown method signature";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Internal helper to check a single setup or teardown method
        /// </summary>
        /// <param name="fixtureType">The type to be checked</param>
        /// <param name="attributeName">The attribute name to be checked</param>
        /// <returns>True if the method is present no more than once and has a valid signature</returns>
        private bool CheckSetUpTearDownMethod(Type fixtureType, string attributeName)
        {
            int count = Reflect.CountMethodsWithAttribute(
                fixtureType, attributeName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly,
                true);

            if (count == 0) return true;

            if (count > 1) return false;

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
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Method to return the description for a fixture
        /// </summary>
        /// <param name="fixtureType">The fixture to check</param>
        /// <returns>The description, if any, or null</returns>
        protected override string GetFixtureDescription(Type fixtureType)
        {
            Attribute fixtureAttribute =
                Reflect.GetAttribute(fixtureType, "csUnit.TestFixtureAttribute", true);

            // Some of our tests create a fixture without the attribute
            if (fixtureAttribute != null)
                return (string)Reflect.GetPropertyValue(
                    fixtureAttribute,
                    "Description",
                    BindingFlags.Public | BindingFlags.Instance);

            return null;
        }
        #endregion
    }
}
