using System;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for RepeatedTestAttribute.
	/// </summary>
	/// 
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
