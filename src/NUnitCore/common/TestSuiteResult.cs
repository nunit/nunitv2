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
	using System.Collections;

	/// <summary>
	///		TestSuiteResult
	/// </summary>
	/// 
	[Serializable]
	public class TestSuiteResult : TestResult
	{
		private ArrayList results = new ArrayList();
		
		public TestSuiteResult(TestInfo test, string name) 
			: base(test, name) { }

		public TestSuiteResult(ITest suite, string name)
			: this(new TestInfo(suite), name) { }

		// For tests
		public TestSuiteResult(string testSuiteString) 
			: base(null, testSuiteString) { }

		public void AddResult(TestResult result) 
		{
			results.Add(result);

			if( this.ResultState == ResultState.Success &&
				result.ResultState != ResultState.Success )
			{
				this.Failure( "Child test failed", null, FailureSite.Child );
			}
		}

		public IList Results
		{
			get { return results; }
		}

		public override void Accept(ResultVisitor visitor) 
		{
			visitor.Visit(this);
		}
	}
}
