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

using System;

namespace NUnit.Gui
{
	using NUnit.Core;

	/// <summary>
	/// Summary description for TestResultItem.
	/// </summary>
	public class TestResultItem
	{
		private TestResult testResult;

		public TestResultItem(TestResult result)
		{
			testResult = result;
		}

		public override string ToString()
		{
			return String.Format("{0} : {1}", testResult.Test.Name, testResult.Message);
		}

		public string GetMessage()
		{
			return String.Format("{0} : {1}", testResult.Test.Name, testResult.Message);
		}

		public string StackTrace
		{
			get 
			{
				string stackTrace = "No stack trace is available";
				if(testResult.StackTrace != null)
					stackTrace = StackTraceFilter.Filter(testResult.StackTrace);

				return stackTrace;
			}
		}
	}
}
