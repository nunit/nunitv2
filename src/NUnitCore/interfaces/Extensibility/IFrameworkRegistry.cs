using System;

namespace NUnit.Core.Extensibility
{
	/// <summary>
	/// Summary description for IFrameworkRegistry.
	/// </summary>
	public interface IFrameworkRegistry
	{
		void Register( string frameworkName, string assemblyName );
	}
}
