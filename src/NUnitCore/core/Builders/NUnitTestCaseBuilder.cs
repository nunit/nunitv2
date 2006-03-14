#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

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
			try
			{
				NameValueCollection settings = (NameValueCollection)
					ConfigurationSettings.GetConfig("NUnit/TestCaseBuilder");
				if (settings != null)
				{
					string oldStyle = settings["OldStyleTestCases"];
					if (oldStyle != null)
						allowOldStyleTests = Boolean.Parse(oldStyle);
				}
			}
			catch(Exception e)
			{
				Debug.WriteLine(e);
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
					testCase.RunState = RunState.Skipped;
					testCase.IgnoreReason = helper.Reason;
				}

				testCase.Categories = CategoryManager.GetCategories( method );
				testCase.IsExplicit = Reflect.HasAttribute( method, "NUnit.Framework.ExplicitAttribute", false );
				
				System.Attribute[] attributes = 
					Reflect.GetAttributes( method, "NUnit.Framework.PropertyAttribute", false );

				foreach( Attribute propertyAttribute in attributes ) 
				{
					string name = (string)Reflect.GetPropertyValue( propertyAttribute, "Name", BindingFlags.Public | BindingFlags.Instance );
					if ( name != null && name != string.Empty )
					{
						object value = Reflect.GetPropertyValue( propertyAttribute, "Value", BindingFlags.Public | BindingFlags.Instance );
						testCase.Properties[name] = value;
					}
				}
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
