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
			Assert.True(assemblyOptions.Validate());
		}

		[Test]
		public void IsAssemblyTest()
		{
			Assert.True(assemblyOptions.IsAssembly && 
				        !assemblyOptions.IsFixture);
		}

		[Test]
		public void ParameterCount()
		{
			Assert.Equals(2, assemblyOptions.Parameters.Count);
		}

		[Test]
		public void CheckParameters()
		{
			ArrayList parms = assemblyOptions.Parameters;
			Assert.True(parms.Contains(firstAssembly));
			Assert.True(parms.Contains(secondAssembly));
		}

		[Test]
		public void FixtureValidate()
		{
			Assert.True(fixtureOptions.Validate());
		}

		[Test]
		public void IsFixture()
		{
			Assert.True(fixtureOptions.IsFixture && 
				        !fixtureOptions.IsAssembly);
		}

		[Test]
		public void FixtureParameters()
		{
			Assert.Equals(fixture, fixtureOptions.fixture);
			ArrayList parms = fixtureOptions.Parameters;
			Assert.True(parms.Contains(firstAssembly));
			Assert.True(parms.Contains(secondAssembly));
		}
	}
}
