using System;

namespace NUnit.Framework
{
	/// <summary>
	/// Summary description for CategoryAttribute.
	/// </summary>
	/// 
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method, AllowMultiple=true)]
	public sealed class CategoryAttribute : Attribute
	{
		private string name;

		public CategoryAttribute(string name)
		{
			this.name = name;
		}

		public string Name 
		{
			get { return name; }
		}
	}
}
