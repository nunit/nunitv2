using System;

namespace NUnit.Framework
{
	/// <summary>
	/// Summary description for PropertyAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method, AllowMultiple=true)]
	public class PropertyAttribute : Attribute
	{
		private string propertyName;
		private object propertyValue;

		/// <summary>
		/// Construct a PropertyAttribute with a name and value
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public PropertyAttribute( string propertyName, object propertyValue )
		{
			this.propertyName = propertyName;
			this.propertyValue = propertyValue;
		}

		public string Name
		{
			get { return propertyName; }
		}

		public object Value
		{
			get { return propertyValue; }
		}
	}
}
