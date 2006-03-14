using System;

namespace NUnit.Framework.Extensions
{
	/// <summary>
	/// RepeatAttribute may be applied to test case in order
	/// to run it multiple times.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
	public class RepeatAttribute : Attribute
	{
		private int count;

		/// <summary>
		/// Construct a RepeatAttribute
		/// </summary>
		/// <param name="count">The number of times to run the test</param>
		public RepeatAttribute(int count)
		{
			this.count = count;
		}

		/// <summary>
		/// Gets the number of times to run the test.
		/// </summary>
		public int Count
		{
			get { return count; }
		}
	}
}
