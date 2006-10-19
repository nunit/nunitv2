using System;

namespace NUnit.Core.Extensibility
{
	/// <summary>
	/// NUnitAddinAttribute is used to mark all add-ins. The marked class
	/// must implement the IAddin interface.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
	public sealed class NUnitAddinAttribute : Attribute
	{
		public string Name;
		public string Description;
		public ExtensionType Type;

		public NUnitAddinAttribute()
		{
			this.Type = ExtensionType.Core;
		}
	}
}
