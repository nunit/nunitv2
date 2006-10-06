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

using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Class that can build a tree of automatic namespace
	/// suites from a group of fixtures.
	/// </summary>
	public class NamespaceTreeBuilder
	{
		#region Instance Variables

		/// <summary>
		/// Hashtable of all test suites we have created to represent namespaces.
		/// Used to locate namespace parent suites for fixtures.
		/// </summary>
		Hashtable namespaceSuites  = new Hashtable();

		/// <summary>
		/// The root of the test suite being created by this builder.
		/// </summary>
		TestSuite rootSuite;

		#endregion

		#region Constructor

		public NamespaceTreeBuilder( TestSuite rootSuite )
		{
			this.rootSuite = rootSuite;
		}

		#endregion

		#region Properties

		public TestSuite RootSuite
		{
			get { return rootSuite; }
		}

		#endregion

		#region Public Methods

		public void Add( IList fixtures )
		{
            foreach (TestSuite fixture in fixtures)
                //if (fixture is SetUpFixture)
                //    Add(fixture as SetUpFixture);
                //else
                    Add( fixture );
		}

		public void Add( TestSuite fixture )
		{
            
			string ns = fixture.TestName.FullName;
            int index = ns.LastIndexOf( '.' );
            ns = index > 0 ? ns.Substring( 0, index ) : string.Empty;
			TestSuite containingSuite = BuildFromNameSpace( ns );

            if (fixture is SetUpFixture)
            {
                // The SetUpFixture must replace the namespace suite
                // in which it is "contained". 
                //
                // First, add the old suite's children
                foreach (TestSuite child in containingSuite.Tests)
                    fixture.Add(child);

                // Make the parent of the containing suite point to this
                // fixture instead
                // TODO: Get rid of this somehow?
                TestSuite parent = containingSuite.Parent;
                if (parent == null)
                {
                    fixture.TestName.Name = rootSuite.TestName.Name;
                    rootSuite = fixture;
                }
                else
                {
                    parent.Tests.Remove(containingSuite);
                    parent.Add(fixture);
                }

                // Update the hashtable
                namespaceSuites[ns] = fixture;
            }
            else
			    containingSuite.Add( fixture );
		}

        //public void Add( SetUpFixture fixture )
        //{
        //    string ns = fixture.FullName;
        //    int index = ns.LastIndexOf( '.' );
        //    ns = index > 0 ? ns.Substring( 0, index ) : string.Empty;
        //    TestSuite suite = BuildFromNameSpace( ns );

        //    // Make the parent point to this instead
        //    // TODO: Get rid of this somehow?
        //    TestSuite parent = suite.Parent;
        //    if ( parent != null )
        //    {
        //        parent.Tests.Remove( suite );
        //        parent.Add( fixture );
        //    }

        //    // Add the old suite's children
        //    foreach( TestSuite child in suite.Tests )
        //        fixture.Add( child );

        //    if (parent == null && fixture is SetUpFixture)
        //    {
        //        suite.Tests.Clear();
        //        suite.Add(fixture);
        //    }
        //    // Update the hashtable
        //    namespaceSuites[ns] = fixture;
        //}

		#endregion

		#region Helper Method

		private TestSuite BuildFromNameSpace( string nameSpace )
		{
			if( nameSpace == null || nameSpace  == "" ) return rootSuite;
			TestSuite suite = (TestSuite)namespaceSuites[nameSpace];
			if(suite!=null) return suite;
            
			int index = nameSpace.LastIndexOf(".");
			//string prefix = string.Format( "[{0}]" );
			if( index == -1 )
			{
				suite = new TestSuite( nameSpace );
				if ( rootSuite == null )
					rootSuite = suite;
				else
					rootSuite.Add(suite);
				namespaceSuites[nameSpace]=suite;
			}
			else
			{
				string parentNameSpace = nameSpace.Substring( 0,index );
				TestSuite parent = BuildFromNameSpace( parentNameSpace );
				string suiteName = nameSpace.Substring( index+1 );
				suite = new TestSuite( parentNameSpace, suiteName );
				parent.Add( suite );
				namespaceSuites[nameSpace] = suite;
			}

			return suite;
		}

		#endregion
	}
}
