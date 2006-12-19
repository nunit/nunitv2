using System;
using System.Reflection;

namespace NUnit.Core.Extensibility
{
	/// <summary>
	/// Summary description for Addin.
	/// </summary>
	[Serializable]
	public class Addin
	{
		private string typeName;
		private string name;
		private string description;
		private ExtensionType extensionType;
		private AddinStatus status;

		public Addin( Type type )
		{
			this.typeName = type.AssemblyQualifiedName;

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

			this.status = AddinStatus.Enabled;
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

		public string TypeName
		{
			get { return typeName; }
		}

		public AddinStatus Status
		{
			get { return status; }
			set { status = value; }
		}
	}
}
