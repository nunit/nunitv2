using System;
//using NUnit.Core;

namespace NUnit.Extensions
{
	//[TestBuilder(typeof(RepeatedTestBuilder))]
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
	public class RepeatedTestAttribute : Attribute
	{
		private int count;

		public RepeatedTestAttribute(int count)
		{
			this.count = count;
		}

		public int Count
		{
			get { return count; }
		}
	}
}
