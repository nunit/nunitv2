using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;

namespace NUnit.Core.Builders
{
	public class NUnitTestCaseBuilder : GenericTestCaseBuilder
	{
		public NUnitTestCaseBuilder() 
			: base( NUnitTestFixture.Parameters ) 
		{
			if ( !allowOldStyleTests )
				parms.TestCasePattern = "";
		}

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

		public override TestCase BuildFrom( MethodInfo method )
		{
			TestCase testCase = base.BuildFrom( method );
		
			if ( testCase != null )
			{
				PlatformHelper helper = new PlatformHelper();
				if ( !helper.IsPlatformSupported( method ) )
				{
					testCase.ShouldRun = false;
					testCase.IgnoreReason = "Not running on correct platform";
				}

				testCase.Categories = CategoryManager.GetCategories( method );
				testCase.IsExplicit = Reflect.HasAttribute( method, "NUnit.Framework.ExplicitAttribute", false );
			}			

			return testCase;
		}

		protected virtual IList GetCategories( MethodInfo method )
		{
			System.Attribute[] attributes = 
				Reflect.GetAttributes( method, "NUnit.Framework.CategoryAttribute", false );
			IList categories = new ArrayList();

			foreach( Attribute categoryAttribute in attributes ) 
				categories.Add( 
					Reflect.GetPropertyValue( 
					categoryAttribute, 
					"Name", 
					BindingFlags.Public | BindingFlags.Instance ) );

			return categories;
		}
	}
}
