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

using System;
using System.IO;
using System.Collections;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for MultipleAssembliesDomain.
	/// </summary>
	[TestFixture]
	public class MultipleAssembliesTestDomain
	{
		private Test suite;

		private TestDomain domain; 
		private TextWriter outStream;
		private TextWriter errorStream;

		[SetUp]
		public void LoadSuite()
		{
			outStream = new ConsoleWriter(Console.Out);
			errorStream = new ConsoleWriter(Console.Error);
			domain = new TestDomain();

			ArrayList assemblies = new ArrayList();
			assemblies.Add( Path.GetFullPath( "nonamespace-assembly.dll" ) );
			assemblies.Add( Path.GetFullPath( "mock-assembly.dll" ) );

			suite = domain.LoadAssemblies( "Multiple Assemblies Test", assemblies );
		}

		[TearDown]
		public void UnloadTestDomain()
		{
			domain.Unload();
			domain = null;
		}
			
		[Test]
		public void BuildSuite()
		{
			Assert.NotNull(suite);
		}

		[Test]
		public void RootNode()
		{
			Assert.True( suite is RootTestSuite );
			Assert.Equals( "Multiple Assemblies Test", suite.Name );
		}

		[Test]
		public void AssemblyNodes()
		{
			Assert.True( suite.Tests[0] is AssemblyTestSuite );
			Assert.True( suite.Tests[1] is AssemblyTestSuite );
		}

		[Test]
		public void TestCaseCount()
		{
			Assert.Equals(10, suite.CountTestCases);
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			TestResult result = suite.Run(NullListener.NULL);
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.Equals(8, summary.ResultCount);
		}
	}
}
