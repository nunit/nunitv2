#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Extensions
{
	using System;
	using System.Collections;
	using NUnit.Core;

	/// <summary>
	/// Summary description for RepeatedTest.
	/// </summary>
	public class RepeatedTest : Test
	{
		private int repeatCount;
		private TestCase testCase;

		public RepeatedTest(TestCase testCase, int repeatCount) : base(testCase.Name)
		{
			this.testCase = testCase;
			this.repeatCount = repeatCount;
		}

		public override int CountTestCases 
		{
			get { return repeatCount; }
		}

		public override TestResult Run(EventListener listener)
		{
			TestSuiteResult suiteResult = new TestSuiteResult(this, testCase.Name);

			for(int i = 0; i < repeatCount; i++)
			{
				TestResult testResult = testCase.Run(NullListener.NULL);
				suiteResult.AddResult(testResult);
		
				if(!testResult.IsSuccess)
					break;
			}
		
			return suiteResult;
		}

		public override bool IsSuite 
		{ 
			get { return false; } 
		}

		public override ArrayList Tests 
		{ 
			get { return null; }
		}
	}
}
