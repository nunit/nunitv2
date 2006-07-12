using System;
using NUnit.Framework;
using System.Reflection;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for TestFrameworkTests.
	/// </summary>
	[TestFixture]
	public class TestFrameworkTests
	{
		[Test]
		public void NUnitFrameworkIsReportedAsLoaded()
		{
			foreach( AssemblyName assemblyName in TestFramework.GetLoadedFrameworks() )
				if ( assemblyName.Name == "nunit.framework" ) return;
			Assert.Fail("Cannot find nunit.framework");
		}
	}
}
