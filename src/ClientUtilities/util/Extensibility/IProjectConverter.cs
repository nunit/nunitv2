using System;

namespace NUnit.Util.Extensibility
{
	/// <summary>
	/// The IProjectConverter interface is implemented by any class
	/// that knows how to convert a foreign project format to an
	/// NUnitProject.
	/// </summary>
	public interface IProjectConverter
	{
		/// <summary>
		/// Returns true if the file indicated is one that this
		/// converter knows how to convert.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		bool CanConvertFrom( string path );

		/// <summary>
		/// Converts an external project returning an NUnitProject
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		NUnitProject ConvertFrom( string path );
	}
}
