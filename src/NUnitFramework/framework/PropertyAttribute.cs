using System;

namespace NUnit.Framework
{
	/// <summary>
	/// PropertyAttribute is used to attach information to a test as a name/value pair..
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method|AttributeTargets.Assembly, AllowMultiple=true)]
	public class PropertyAttribute : Attribute
	{
		private string propertyName;
		private object propertyValue;

		/// <summary>
		/// Construct a PropertyAttribute with a name and value
		/// </summary>
		/// <param name="propertyName">The name of the property</param>
		/// <param name="propertyValue">The property value</param>
		public PropertyAttribute( string propertyName, object propertyValue )
		{
			this.propertyName = propertyName;
			this.propertyValue = propertyValue;
		}

		/// <summary>
		/// Gets the property name
		/// </summary>
		public string Name
		{
			get { return propertyName; }
		}

		/// <summary>
		/// Gets the property value
		/// </summary>
		public object Value
		{
			get { return propertyValue; }
		}
	}
}
