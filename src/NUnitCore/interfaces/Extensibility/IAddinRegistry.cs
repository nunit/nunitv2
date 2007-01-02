using System;

namespace NUnit.Core.Extensibility
{
	/// <summary>
	/// Summary description for IAddinRegistry.
	/// </summary>
	public interface IAddinRegistry
	{
		System.Collections.IList Addins { get; }

		void Register( Addin addin );

		void SetStatus( string name, AddinStatus status );
	}
}
