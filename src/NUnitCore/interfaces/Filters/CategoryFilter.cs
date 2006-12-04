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
	/// CategoryFilter is able to select or exclude tests
	/// based on their categories.
	/// </summary>
	/// 
	[Serializable]
	public class CategoryFilter : RecursiveTestFilter
	{
		ArrayList categories;

		/// <summary>
		/// Construct an empty CategoryFilter
		/// </summary>
		public CategoryFilter()
		{
			categories = new ArrayList();
		}

		/// <summary>
		/// Construct a CategoryFilter using a single category name
		/// </summary>
		/// <param name="name">A category name</param>
		public CategoryFilter( string name )
		{
			categories = new ArrayList();
			if ( name != null )
				categories.Add( name );
		}

		/// <summary>
		/// Construct a CategoryFilter using an array of category names
		/// </summary>
		/// <param name="names">An array of category names</param>
		public CategoryFilter( string[] names )
		{
			categories = new ArrayList();
			if ( names != null )
				categories.AddRange( names );
		}

		/// <summary>
		/// Add a category name to the filter
		/// </summary>
		/// <param name="name">A category name</param>
		public void AddCategory(string name) 
		{
			categories.Add( name );
		}

		/// <summary>
		/// Check whether the filter matches a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns></returns>
        public override bool Match(ITest test)
        {
			if ( test.Categories == null )
				return false;

			foreach( string cat in categories )
				if ( test.Categories.Contains( cat ) )
					return true;

			return false;
        }
	}
}
