using System;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for SetUpFixtureTests.
	/// </summary>
	public class SetUpFixtureTests
	{
		public SetUpFixtureTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}

namespace NUnit
{
	/// <summary>
	/// Test class for SetUpFixture
	/// </summary>
	//[SetUpFixture]
	public class NUnitNamespaceSetUpFixture
	{
		[TestFixtureSetUp]
		public void DoNamespaceSetUp()
		{
			Console.WriteLine( "NUnit Namespace SetUp called" );
		}

		[TestFixtureTearDown]
		public void DoNamespaceTearDown()
		{
			Console.WriteLine( "NUnit Namespace TearDown called" );
		}
	}
}
