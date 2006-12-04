using System;

namespace NUnit.Core.Extensibility
{
	/// <summary>
	/// The IExtensionHost interface is implemented by each
	/// of NUnit's Extension hosts. Currently, there is
	/// only one host, which resides in the test domain.
	/// </summary>
	public interface IExtensionHost
	{
        /// <summary>
        /// Get a list of the ExtensionPoints provided by this host.
        /// </summary>
        IExtensionPoint[] ExtensionPoints
        {
            get;
        }

		/// <summary>
		/// Get an interface to the framework registry
		/// </summary>
		IFrameworkRegistry FrameworkRegistry
		{
			get;
		}
		
		/// <summary>
		/// Return an extension point by name, if present
		/// </summary>
		/// <param name="name">The name of the extension point</param>
		/// <returns>The extension point, if found, otherwise null</returns>
		IExtensionPoint GetExtensionPoint( string name );

	}
}
