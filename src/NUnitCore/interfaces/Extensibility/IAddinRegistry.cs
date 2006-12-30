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

        void Register(System.Reflection.Assembly assembly);

		void SetStatus( string name, AddinStatus status );
	}
}
