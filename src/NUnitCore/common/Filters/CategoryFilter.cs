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

namespace NUnit.Core.Filters
{
	/// <summary>
	/// Summary description for CategoryFilter.
	/// </summary>
	/// 
	[Serializable]
	public class CategoryFilter : ITestFilter
	{
		ArrayList categories;

		public CategoryFilter()
		{
			categories = new ArrayList();
		}

		public CategoryFilter( string name )
		{
			categories = new ArrayList();
			categories.Add( name );
		}

		public CategoryFilter( string[] names )
		{
			categories = new ArrayList();
			categories.AddRange( names );
		}

		public void AddCategory(string name) 
		{
			categories.Add( name );
		}

		#region IFilter Members

		public bool Pass( ITest test )
		{
            if ( categories.Count == 0 ) return true;

            if ( CheckCategories( test ) ) return true;

            bool pass = false;

            if (test.IsSuite)
                foreach (ITest child in test.Tests)
                {
                    if ( Pass( child ) )
                    {
                        pass = true;
                        break;
                    }
                }

            return pass;
        }

		#endregion

		/// <summary>
		/// Method returns true if the test has a particular
		/// category or if an ancestor test does. We don't
		/// worry about whether this is an include or an
		/// exclude filter at this point because only positive
		/// categories are inherited, not their absence.
		/// </summary>
		private bool CheckCategories(ITest test) 
		{
			return test.HasCategory( categories )
				|| test.Parent != null 
				&& test.Parent.HasCategory( categories );
		}
	}
}
