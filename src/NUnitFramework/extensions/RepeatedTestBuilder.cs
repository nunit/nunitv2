using System;
using System.Reflection;
using NUnit.Core;

namespace NUnit.Extensions
{
	[TestCaseBuilder]
	public class RepeatedTestBuilder : ITestBuilder, ITestCaseBuilder
	{
		private static readonly string RepeatedTestType =
			"NUnit.Extensions.RepeatedTestAttribute";

		public TestCase Make ( MethodInfo method )
		{
			return new RepeatedTestCase( method, GetRepeatCount( method ) );
		}

		#region ITestCaseBuilder Members

		public TestCase BuildFrom(MethodInfo method)
		{
			return new RepeatedTestCase( method, GetRepeatCount( method ) );
		}

		public bool CanBuildFrom(MethodInfo method)
		{
			return Reflect.HasAttribute( method, RepeatedTestType, false );
		}

		#endregion

		/// <summary>
		/// Get the repeat count using reflection. This allows the extension
		/// to be used across multiple versions of NUnit.
		/// </summary>
		/// <param name="method">MethodInfo to examine for the attribute</param>
		/// <returns>The number of repetitions to perform</returns>
		private int GetRepeatCount( MethodInfo method )
		{
			Attribute repeatedAttribute =
				Reflect.GetAttribute( method, "NUnit.Extensions.RepeatedTestAttribute", false );
			if ( repeatedAttribute == null )
				return 1;
			
			return (int)Reflect.GetPropertyValue( repeatedAttribute, "Count", 
				BindingFlags.Public | BindingFlags.Instance );
		}
	}
}
