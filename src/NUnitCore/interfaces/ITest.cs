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
	/// Common interface supported by all representations
	/// of a test. Only includes informational fields.
	/// The Run method is specifically excluded to allow
	/// for data-only representations of a test.
	/// </summary>
	public interface ITest
    {
        #region Properties
        /// <summary>
		/// Gets the completely specified name of the test
		/// encapsulated in a TestName object.
		/// </summary>
		TestName TestName { get; }

		/// <summary>
		/// Gets a string representing the type of test, e.g.: "Test Case"
		/// </summary>
		string TestType { get; }

        /// <summary>
        /// Indicates whether the test can be run using
        /// the RunState enum.
        /// </summary>
		RunState RunState { get; set; }

		/// <summary>
		/// Reason for not running the test, if applicable
		/// </summary>
		string IgnoreReason { get; set; }
		
		/// <summary>
		/// Count of the test cases ( 1 if this is a test case )
		/// </summary>
		int TestCount { get; }

		/// <summary>
		/// Categories available for this test
		/// </summary>
		IList Categories { get; }

		/// <summary>
		/// Return the description field. 
		/// </summary>
		string Description { get; set; }

		/// <summary>
		/// Return additional properties of the test
		/// </summary>
		IDictionary Properties { get; }

		/// <summary>
		/// True if this is a suite
		/// </summary>
		bool IsSuite { get; }

		/// <summary>
		///  Gets the parent test of this test
		/// </summary>
		ITest Parent { get; }

		/// <summary>
		/// For a test suite, the child tests or suites
		/// Null if this is not a test suite
		/// </summary>
		IList Tests { get; }
        #endregion

        #region Methods
        int CountTestCases(ITestFilter filter);
        #endregion
    }
}

