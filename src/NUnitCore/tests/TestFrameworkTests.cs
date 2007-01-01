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
		public void NUnitFrameworkIsKnownAndReferenced()
		{
			foreach( AssemblyName assemblyName in CoreExtensions.Host.TestFrameworks.GetReferencedFrameworks( Assembly.GetExecutingAssembly() ) )
				if ( assemblyName.Name == "nunit.framework" ) return;
			Assert.Fail("Cannot find nunit.framework");
		}
	}
}
