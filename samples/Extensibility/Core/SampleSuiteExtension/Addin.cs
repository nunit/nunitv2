using System;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Extensions
{
	/// <summary>
	/// Summary description for Addin.
	/// </summary>
	[NUnitAddin(Name="SampleSuiteExtension", Description = "Recognizes Tests starting with SampleTest...")]
	public class Addin : IAddin
	{
		#region IAddin Members
		public bool Install(IExtensionHost host)
		{
			IExtensionPoint builders = host.GetExtensionPoint( "SuiteBuilders" );
			if ( builders == null )
				return false;

			builders.Install( new SampleSuiteExtensionBuilder() );
			return true;
		}
		#endregion
	}
}
