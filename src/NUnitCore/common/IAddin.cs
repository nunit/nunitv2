using System;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for IPlugin.
	/// </summary>
	public interface IAddin
	{
		string Name { get; }
		string Description { get; }
		void Initialize();
	}
}
