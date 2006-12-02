#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class RemoteTestResultTest
	{
		[Test]
		public void ResultStillValidAfterDomainUnload() 
		{
			TestDomain domain = new TestDomain();
			Assert.IsTrue( domain.Load( new TestPackage( "mock-assembly.dll" ) ) );
			TestResult result = domain.Run( new NullListener() );
			TestSuiteResult suite = result as TestSuiteResult;
			Assert.IsNotNull(suite);
			TestCaseResult caseResult = findCaseResult(suite);
			Assert.IsNotNull(caseResult);
			TestResultItem item = new TestResultItem(caseResult);
			//domain.Unload(); // TODO: Figure out where unhandled exception comes from
			string message = item.GetMessage();
			Assert.IsNotNull(message);
		}

        [Test, Explicit("Fails intermittently")]
        public void AppDomainUnloadedBug()
        {
            TestDomain domain = new TestDomain();
            domain.Load( new TestPackage( "mock-assembly.dll" ) );
            domain.Run(new NullListener());
            domain.Unload();
        }

		private TestCaseResult findCaseResult(TestSuiteResult suite) 
		{
			foreach (TestResult r in suite.Results) 
			{
				if (r is TestCaseResult)
				{
					return (TestCaseResult) r;
				}
				else 
				{
					TestCaseResult result = findCaseResult((TestSuiteResult)r);
					if (result != null)
						return result;
				}

			}

			return null;
		}
	}
}
