using System;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.TestData.SuiteBuilderTests
{
	public class Suite
	{
		[Suite]
		public static TestSuite MockSuite
		{
			get 
			{
				TestSuite testSuite = new TestSuite("TestSuite");
				return testSuite;
			}
		}
	}

	class NonConformingSuite
	{
		[Suite]
		public static int Integer
		{
			get 
			{
				return 5;
			}
		}
	}

}
