using System;

namespace NUnit.Core.Extensions
{
	/// <summary>
	/// SampleSuiteExtensionBuilder knows how to build a SampleSuiteExtension
	/// </summary>
	[NUnitAddin]
	public class SampleSuiteExtensionBuilder : ISuiteBuilder, IAddin
	{	
		#region ISuiteBuilder Members

		// This builder delegates all the work to the constructor of the  
		// extension suite. Many builders will need to do more work, 
		// looking for other attributes, setting properties on the 
		// suite and locating methods for tests, setup and teardown.
		public Test BuildFrom(Type type)
		{
			if ( CanBuildFrom( type ) )
				return new SampleSuiteExtension( type );
			return null;
		}
		
		// The builder recognizes the types that it can use by the presense
		// of SampleSuiteExtensionAttribute. Note that an attribute does not
		// have to be used. You can use any arbitrary set of rules that can be 
		// implemented using reflection on the type.
		public bool CanBuildFrom(Type type)
		{
			return Reflect.HasAttribute( type, "NUnit.Core.Extensions.SampleSuiteExtensionAttribute", false );
		}

		#endregion

		#region IAddin Members
		public void Install(object host)
		{
			IAddinHost addinHost = host as IAddinHost;
			if ( addinHost != null )
				addinHost.Install( this );
		}
		#endregion
	}
}
