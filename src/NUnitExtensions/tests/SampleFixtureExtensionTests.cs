using System;
using NUnit.Framework;
using NUnit.Core.Extensions;

namespace NUnit.Extensions.Tests
{
	/// <summary>
	/// Test class that demonstrates SampleFixtureExtension
	/// </summary>
	[SampleFixtureExtension]
	public class SampleFixtureExtensionTests
	{
		[TestFixtureSetUp]
		public void SetUpTests()
		{
			Console.WriteLine( "TestFixtureSetUp called" );
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			Console.WriteLine( "TestFixtureTearDown called" );
		}

		[Test]
		public void SomeTest()
		{
			Console.WriteLine( "Hello from some test" );
		}

		[Test]
		public void AnotherTest()
		{
			Console.WriteLine( "Hello from another test" );
		}

		public void NotATest()
		{
			Console.WriteLine( "I shouldn't be called!" );
		}
	}
}
