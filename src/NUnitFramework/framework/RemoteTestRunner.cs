#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole 
' or Copyright © 2000-2002 Philip A. Craig
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
	public class RemoteTestRunner : LongLivingMarshalByRefObject
	{
		#region Instance variables

		/// <summary>
		/// The loaded test suite
		/// </summary>
		private TestSuite suite;

		/// <summary>
		/// The test Fixture name to load
		/// </summary>
		private string testName;

		/// <summary>
		/// The test file to load. If assemblies is null,
		/// this must be an assembly file, otherwise it
		/// is only used to provide a name for the root
		/// test node having the assemblies as children.
		/// </summary>
		private string testFileName;

		/// <summary>
		/// The list of assemblies to load
		/// </summary>
		private IList assemblies;

		#endregion

		#region Properties

		public string TestName 
		{
			get { return testName; }
			set { testName = value; }
		}
			
		public Test Test
		{
			get { return suite; }
		}

		public string TestFileName
		{
			get { return testFileName; }
			set { testFileName = value; }
		}

		public IList Assemblies
		{
			get { return assemblies; }
			set { assemblies = value; }
		}

		#endregion

		#region Public Methods

		public void BuildSuite() 
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();

			if(testName == null )
				if ( assemblies == null )
					suite = builder.Build( testFileName );
				else
					suite = builder.Build( testFileName, assemblies );
			else
				suite = builder.Build( testFileName, testName );

			if(suite != null) TestName = suite.FullName;
		}

		public TestResult Run(NUnit.Core.EventListener listener, TextWriter outText, TextWriter errorText)
		{
			Console.SetOut(new StringTextWriter(outText));
			Console.SetError(new StringTextWriter(errorText));

			Test test = FindByName(suite, testName);

			TestResult result = test.Run(listener);

			return result;
		}

		#endregion

		#region StringTextWriter Class

		/// <summary>
		/// Use this wrapper to ensure that only strings get passed accross the AppDomain
		/// boundry.  Otherwise tests will break when non-remotable objecs are passed to
		/// Console.Write/WriteLine.
		/// </summary>
		private class StringTextWriter : TextWriter
		{
			public StringTextWriter(TextWriter aTextWriter)
			{
				theTextWriter = aTextWriter;
			}
			private TextWriter theTextWriter;

			override public void Write(char aChar)
			{
				theTextWriter.Write(aChar);
			}

			override public void Write(string aString)
			{
				theTextWriter.Write(aString);
			}

			override public void WriteLine(string aString)
			{
				theTextWriter.WriteLine(aString);
			}

			override public System.Text.Encoding Encoding
			{
				get { return theTextWriter.Encoding; }
			}
		}

		#endregion

		#region FindByName Helper

		private Test FindByName(Test test, string fullName)
		{
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

