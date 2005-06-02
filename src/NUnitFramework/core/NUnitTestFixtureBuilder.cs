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

namespace NUnit.Core.Builders
{
	/// <summary>
	/// Built-in SuiteBuilder for NUnit TestFixture
	/// </summary>
	public class NUnitTestFixtureBuilder : GenericTestFixtureBuilder
	{ 
		#region Constructor
		public NUnitTestFixtureBuilder() : base( NUnitTestFixture.Parameters ) { }
		#endregion

		#region GenericTestFixtureBuilder Overrides
		/// <summary>
		/// Makes an NUnitTestFixture instance
		/// </summary>
		/// <param name="type">The type to be used</param>
		/// <param name="assemblyKey">The assembly key</param>
		/// <returns>A TestSuite or null</returns>
		protected override TestSuite MakeSuite( Type type, int assemblyKey )
		{
			return new NUnitTestFixture( type, assemblyKey );
		}

		/// <summary>
		/// Overrides GenericTestFixtureBuilder.BuildFrom to allow
		/// use of the Category, Explicit and Platform attributes
		/// on NUnitTestFixtures:
		/// </summary>
		/// <param name="type">The type to use in building the fixture</param>
		/// <param name="assemblyKey">The assembly key</param>
		/// <returns>A TestSuite or null</returns>
		public override TestSuite BuildFrom(Type type, int assemblyKey)
		{
			TestSuite suite = base.BuildFrom (type, assemblyKey);

			if ( suite != null )
			{
				PlatformHelper helper = new PlatformHelper();
				if ( !helper.IsPlatformSupported( type ) )
				{
					suite.ShouldRun = false;
					suite.IgnoreReason = helper.Reason;
				}

				suite.Categories = CategoryManager.GetCategories( type );	
				suite.IsExplicit = Reflect.HasAttribute( type, "NUnit.Framework.ExplicitAttribute", false );
			}

			return suite;
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
		#endregion
	}
}