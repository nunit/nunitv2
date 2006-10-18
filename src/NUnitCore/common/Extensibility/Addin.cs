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
			}

			if ( this.name == null )
				this.name = type.Name;

//            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
//            theAddin = (IAddin)ctor.Invoke(new object[0]);
        }

		public string Name
		{
			get { return name; }
		}

		public string Description
		{
			get { return description; }
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
