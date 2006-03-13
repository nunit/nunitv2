using System;

namespace NUnit.Framework.Extensions
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
	public class RepeatAttribute : Attribute
	{
		private int count;

		public RepeatAttribute(int count)
		{
			this.count = count;
		}

		public int Count
		{
			get { return count; }
		}
	}
}
