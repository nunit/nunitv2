using System;

namespace NUnit.Core
{
	/// <summary>
	/// The IAddin interface must be implemented by all
	/// NUnit add-ins.
	/// </summary>
	public interface IAddin
	{
		void Initialize( IAddinHost host );
	}
}
