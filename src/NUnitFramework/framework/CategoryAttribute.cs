#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
//************************************************************************************
//'
//' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
//' Copyright © 2000-2003 Philip A. Craig
//'
//' This software is provided 'as-is', without any express or implied warranty. In no 
//' event will the authors be held liable for any damages arising from the use of this 
//' software.
//' 
//' Permission is granted to anyone to use this software for any purpose, including 
//' commercial applications, and to alter it and redistribute it freely, subject to the 
//' following restrictions:
//'
//' 1. The origin of this software must not be misrepresented; you must not claim that 
//' you wrote the original software. If you use this software in a product, an 
//' acknowledgment (see the following) in the product documentation is required.
//'
//' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
//' or Copyright © 2000-2003 Philip A. Craig
//'
//' 2. Altered source versions must be plainly marked as such, and must not be 
//' misrepresented as being the original software.
//'
//' 3. This notice may not be removed or altered from any source distribution.
//'
//'***********************************************************************************/
#endregion

using System;

namespace NUnit.Framework
{
	/// <summary>
	/// Attribute used to apply a category to a test
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method|AttributeTargets.Assembly, AllowMultiple=true)]
	public class CategoryAttribute : Attribute
	{
		/// <summary>
		/// The name of the category
		/// </summary>
		protected string categoryName;

		/// <summary>
		/// Construct attribute for a given category
		/// </summary>
		/// <param name="name">The name of the category</param>
		public CategoryAttribute(string name)
		{
			this.categoryName = name;
		}

		/// <summary>
		/// Protected constructor uses the Type name as the name
		/// of the category.
		/// </summary>
		protected CategoryAttribute()
		{
			this.categoryName = this.GetType().Name;
			if ( categoryName.EndsWith( "Attribute" ) )
				categoryName = categoryName.Substring( 0, categoryName.Length - 9 );
		}

		/// <summary>
		/// The name of the category
		/// </summary>
		public string Name 
		{
			get { return categoryName; }
		}
	}
}
