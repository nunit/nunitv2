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
	using System.IO;
	using System.Collections;
	using System.Reflection;
	using System.Runtime.Remoting;

	/// <summary>
	/// Summary description for RemoteTestRunner.
	/// </summary>
	/// 
	[Serializable]
	public class RemoteTestRunner : LongLivingMarshalByRefObject, TestRunner
	{
		#region Instance variables

		/// <summary>
		/// The loaded test suite
		/// </summary>
		private TestSuite suite;

		#endregion

		#region Properties
		
		#endregion

		#region Loading Tests

		public Test Load( string assemblyName )
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build( assemblyName );
			return suite;
		}

		public Test Load( string assemblyName, string testName )
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build( assemblyName, testName );
			return suite;
		}

		public Test Load( string projectName, IList assemblies )
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build( projectName, assemblies );
			return suite;
		}

		public Test Load( string projectName, IList assemblies, string testName )
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build( assemblies, testName );
			return suite;
		}

		public void Unload()
		{
			suite = null; // All for now
		}

		#endregion

		#region Running Tests

		public int CountTestCases(IList testNames) 
		{
			IFilter filter = BuildNameFilter(testNames);
			return suite.CountTestCases(filter);
		}

		public TestResult Run(NUnit.Core.EventListener listener, TextWriter outText, TextWriter errorText, IFilter filter) 
		{
			BufferedStringTextWriter outBuffer = new BufferedStringTextWriter( outText );
			BufferedStringTextWriter errorBuffer = new BufferedStringTextWriter( errorText );

			Console.SetOut( outBuffer );
			Console.SetError( errorBuffer );

//			string currentDirectory = Environment.CurrentDirectory;
//
//			string assemblyName = assemblies == null ? testFileName : (string)assemblies[test.AssemblyKey];
//			string assemblyDirectory = Path.GetDirectoryName( assemblyName );
//
//			if ( assemblyDirectory != null && assemblyDirectory != string.Empty )
//				Environment.CurrentDirectory = assemblyDirectory;


			TestResult result = suite.Run(listener, filter);

		//	Environment.CurrentDirectory = currentDirectory;

			outBuffer.Close();
			errorBuffer.Close();

			return result;
		}

		public TestResult Run(NUnit.Core.EventListener listener, TextWriter outText, TextWriter errorText )
		{
			NameFilter filter = new NameFilter(suite);

			return Run(listener, outText, errorText, filter);
		}

		public TestResult Run(NUnit.Core.EventListener listener, TextWriter outText, TextWriter errorText, string testName )
		{
			Test test = FindByName(suite, testName);

			NameFilter filter = new NameFilter(test);

			return Run(listener, outText, errorText, filter);
		}

		public TestResult Run(NUnit.Core.EventListener listener, TextWriter outText, TextWriter errorText, IList testNames)
		{
			IFilter filter = BuildNameFilter(testNames);
		
			return Run(listener, outText, errorText, filter);
		}

		private IFilter BuildNameFilter(IList testNames) 
		{
			ArrayList testNodes = new ArrayList();
			foreach (string name in testNames) 
			{
				Test test = FindByName(suite, name);
				testNodes.Add(test);
			}

			return new NameFilter(testNodes);
		}

		#endregion

		#region FindByName Helper

		private Test FindByName(Test test, string fullName)
		{
			if(test.UniqueName.Equals(fullName)) return test;
			if(test.FullName.Equals(fullName)) return test;
			
			Test result = null;
			if(test is TestSuite)
			{
				TestSuite suite = (TestSuite)test;
				foreach(Test testCase in suite.Tests)
				{
					result = FindByName(testCase, fullName);
					if(result != null) break;
				}
			}

			return result;
		}
	}

	#endregion
}

