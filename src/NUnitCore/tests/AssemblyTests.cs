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

namespace NUnit.Core.Tests
{
	using System;
	using System.Configuration;
	using System.IO;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Threading;
	using NUnit.Framework;

	[TestFixture]
	public class AssemblyTests 
	{
		private string thisDll;

		[SetUp]
		public void InitStrings()
		{
			thisDll = this.GetType().Module.Name;
		}

		// TODO: Review and remove unnecessary tests

		[Test]
		public void RunSetsCurrentDirectory()
		{
			Assert.IsTrue( File.Exists( thisDll ), "Run does not set current directory" );
		}

		[Test]
		public void LoadAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build( thisDll );
			Assert.IsNotNull( suite );
			//Assert.IsNotNull(testAssembly, "should be able to load assembly");
			Assert.IsTrue( File.Exists( thisDll ), "Load does not set current Directory" );
		}

		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void LoadAssemblyNotFound()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			builder.Build("XXXX");
		}

		[Test]
		public void LoadAssemblyWithoutTestFixtures()
		{
			string fileName = "notestfixtures-assembly.dll";
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build(fileName);
			Assert.IsNotNull(suite,"Should not be null");
			Assert.IsFalse(suite.ShouldRun, "Should not run");
			Assert.AreEqual(suite.IgnoreReason, "Has no TestFixtures");
			Assert.AreEqual(0, suite.Tests.Count);
		}

		[Test]
		public void LoadTestFixtureFromAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestSuite suite = builder.Build( thisDll, this.GetType().FullName );
			Assert.IsNotNull(suite, "Should not be Null");
		}

		[Test]
		public void AppSettingsLoaded()
		{
			string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			Assert.IsTrue( File.Exists( configFile ), string.Format( "{0} does not exist", configFile ) );
			Assert.IsNull(ConfigurationSettings.AppSettings["tooltip.ShowAlways"]);
			Assert.AreEqual("54321",ConfigurationSettings.AppSettings["test.setting"]);
		}
	}
}
