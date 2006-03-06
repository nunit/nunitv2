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

namespace NUnit.Core
{
	/// <summary>
	/// Interface to be implemented by filters applied to tests.
	/// The filter applies when running the test, after it has been
	/// loaded, since this is the only time an ITest exists.
	/// </summary>
	[Serializable]
	public abstract class TestFilter
	{
		/// <summary>
		/// Unique Empty filter.
		/// </summary>
		public static TestFilter Empty = new EmptyFilter();

		public bool IsEmpty
		{
			get { return this is TestFilter.EmptyFilter; }
		}

		/// <summary>
		/// Determine if a particular test passes the filter criteria.
		/// </summary>
		/// <param name="test">The test to which the filter is applied</param>
		/// <returns>True if the test passes the filter, otherwise false</returns>
		public abstract bool Pass( ITest test );

		/// <summary>
		/// Nested class provides an empty filter - one that always
		/// returns true when called, unless the test is marked explicit.
		/// </summary>
		[Serializable]
		private class EmptyFilter : TestFilter
		{
			public override bool Pass( ITest test )
			{
				return !test.IsExplicit;
			}
		}
	}
}
