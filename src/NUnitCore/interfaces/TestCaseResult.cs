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

namespace NUnit.Core
{
	using System;
	using System.Text;

	/// <summary>
	/// TestCaseResult represents the result of a test case execution
	/// </summary>
	[Serializable]
	public class TestCaseResult : TestResult
	{
        /// <summary>
        /// Construct a result for a test case
        /// </summary>
        /// <param name="testCase">The test case for which this is a result</param>
		public TestCaseResult(TestInfo testCase)
			: base(testCase, testCase.TestName.FullName) { }

		/// <summary>
		/// Construct a result from a string - used for tests
		/// </summary>
		/// <param name="testCaseString"></param>
		public TestCaseResult(string testCaseString) 
			: base(null, testCaseString) { }

        /// <summary>
        /// Accept a ResultVisitor
        /// </summary>
        /// <param name="visitor">The visitor to accept</param>
		public override void Accept(ResultVisitor visitor) 
		{
			visitor.Visit(this);
		}
	}
}
