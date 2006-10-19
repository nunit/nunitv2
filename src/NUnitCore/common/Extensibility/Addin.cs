using System;
using System.Reflection;

namespace NUnit.Core.Extensibility
{
	/// <summary>
	/// Summary description for Addin.
	/// </summary>
	[Serializable]
	public class Addin : IAddin
	{
		private Type type;
		private string name;
		private string description;
		private ExtensionType extensionType;

		[NonSerialized]
		private IAddin theAddin;

		public Addin( Type type )
		{
			this.type = type;

			object[] attrs = type.GetCustomAttributes( typeof(NUnitAddinAttribute), false );
			if ( attrs.Length == 1 )
			{
				NUnitAddinAttribute attr = (NUnitAddinAttribute)attrs[0];
				this.name = attr.Name;
				this.description = attr.Description;
				this.extensionType = attr.Type;
			}

			if ( this.name == null )
				this.name = type.Name;

			if ( this.extensionType == 0 )
				this.extensionType = ExtensionType.Core;
        }

		public string Name
		{
			get { return name; }
		}

		public string Description
		{
			get { return description; }
		}

		public ExtensionType ExtensionType
		{
			get { return extensionType; }
		}

		#region IAddin Members
		public bool Install(IExtensionHost host)
		{
			ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
			theAddin = (IAddin)ctor.Invoke(new object[0]);

			return theAddin.Install(host);
		}
		#endregion
	}
}
