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

namespace NUnit.Tests.CommandLine
{
	using System;
	using System.Collections;
	using NUnit.Framework;
	using NUnit.Util;

	[TestFixture]
	public class MultipleAssemblyFixture
	{
		private readonly string firstAssembly = "nunit.tests.dll";
		private readonly string secondAssembly = "mock-assembly.dll";
		private readonly string fixture = "NUnit.Tests.CommandLine";
		private ConsoleOptions assemblyOptions;
		private ConsoleOptions fixtureOptions;

		[SetUp]
		public void SetUp()
		{
			assemblyOptions = new ConsoleOptions(new string[]
				{ firstAssembly, secondAssembly });
			fixtureOptions = new ConsoleOptions(new string[]
				{ "/fixture:"+fixture, firstAssembly, secondAssembly });
		}

		[Test]
		public void MultipleAssemblyValidate()
		{
			Assert.IsTrue(assemblyOptions.Validate());
		}

		[Test]
		public void IsAssemblyTest()
		{
			Assert.IsTrue(assemblyOptions.IsAssembly && 
				        !assemblyOptions.IsFixture);
		}

		[Test]
		public void ParameterCount()
		{
			Assert.AreEqual(2, assemblyOptions.Parameters.Count);
		}

		[Test]
		public void CheckParameters()
		{
			ArrayList parms = assemblyOptions.Parameters;
			Assert.IsTrue(parms.Contains(firstAssembly));
			Assert.IsTrue(parms.Contains(secondAssembly));
		}

		[Test]
		public void FixtureValidate()
		{
			Assert.IsTrue(fixtureOptions.Validate());
		}

		[Test]
		public void IsFixture()
		{
			Assert.IsTrue(fixtureOptions.IsFixture && 
				        !fixtureOptions.IsAssembly);
		}

		[Test]
		public void FixtureParameters()
		{
			Assert.AreEqual(fixture, fixtureOptions.fixture);
			ArrayList parms = fixtureOptions.Parameters;
			Assert.IsTrue(parms.Contains(firstAssembly));
			Assert.IsTrue(parms.Contains(secondAssembly));
		}
	}
}
