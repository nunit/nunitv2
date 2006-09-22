using System;

namespace NUnit.Framework
{
	/// <summary>
	/// Summary description for DescriptionAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method|AttributeTargets.Assembly, AllowMultiple=false)]
	public sealed class DescriptionAttribute : Attribute
	{
		string description;

		public DescriptionAttribute(string description)
		{
			this.description=description;
		}

		public string Description
		{
			get { return description; }
		}
	}
}
