using System;

namespace NUnit.Framework
{
	/// <summary>
	/// Summary description for SetCultureAttribute.
	/// </summary>
	public class SetCultureAttribute : PropertyAttribute
	{
		/// <summary>
		/// Construct given the name of a culture
		/// </summary>
		/// <param name="culture"></param>
		public SetCultureAttribute( string culture ) : base( culture ) { }
	}
}
