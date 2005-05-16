using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core.Builders;

namespace NUnit.Core.Extensions.Tests
{
	/// <summary>
	/// Test class that demonstrates MockSuiteExtension
	/// </summary>
	[SampleSuiteExtension]
	public class SampleSuiteExtensionTests
	{
		public void SampleTest1()
		{
			Console.WriteLine( "Hello from sample test 1" );
		}

		public void SampleTest2()
		{
			Console.WriteLine( "Hello from sample test 2" );
		}

		public void NotATest()
		{
			Console.WriteLine( "I shouldn't be called!" );
		}
	}
}
