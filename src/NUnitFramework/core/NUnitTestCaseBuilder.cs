using System;
using System.Reflection;
using System.Collections.Specialized;
using System.Configuration;

namespace NUnit.Core
{
	public class NUnitTestCaseBuilder : GenericTestCaseBuilder
	{
		public NUnitTestCaseBuilder() 
			: base( NUnitTestFixture.Parameters ) { }

		static bool allowOldStyleTests;

		static NUnitTestCaseBuilder()
		{
			NameValueCollection settings = (NameValueCollection)
				ConfigurationSettings.GetConfig( "NUnit/TestCaseBuilder" );

			try
			{
				string oldStyle = settings["OldStyleTestCases"];
				if ( oldStyle != null )
					allowOldStyleTests = Boolean.Parse( oldStyle );
			}
			catch
			{
				// Use default values
			}
		}

		protected override bool IsOldStyleTestMethod(MethodInfo methodToCheck)
		{
			if ( allowOldStyleTests )
				if ( methodToCheck.Name.ToLower().StartsWith("test") )
				{
					object[] attributes = methodToCheck.GetCustomAttributes( false );

					foreach( Attribute attribute in attributes )
					{
						string typeName = attribute.GetType().FullName;
						if( typeName == "NUnit.Framework.SetUpAttribute" ||
							typeName == "NUnit.Framework.TestFixtureSetUpAttribute" ||
							typeName == "NUnit.Framework.TearDownAttribute" || 
							typeName == "NUnit.Framework.TestFixtureTearDownAttribute" )
						{
							return false;
						}
					}

					return true;	
				}

			return false;
		}
	}
}
