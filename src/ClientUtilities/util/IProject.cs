using System;

namespace NUnit.Util
{
	/// <summary>
	/// Common Interface for NUnit and VS Projects
	/// </summary>
	public interface IProject
	{
		bool IsDirty { get; set; }
	}
}
