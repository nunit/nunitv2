// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Core;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class XmlResultVisitorTest
	{
		private XmlDocument resultDoc;

		[TestFixtureSetUp]
		public void RunMockTests()
		{
			string testsDll = "mock-assembly.dll";
			TestSuiteBuilder suiteBuilder = new TestSuiteBuilder();
			Test suite = suiteBuilder.Build( new TestPackage( testsDll ) );

			TestResult result = suite.Run(NullListener.NULL);
			StringBuilder builder = new StringBuilder();
			StringWriter writer = new StringWriter(builder);
			XmlResultVisitor visitor = new XmlResultVisitor(writer, result);
			result.Accept(visitor);
			visitor.Write();

			string resultXml = builder.ToString();
			Console.WriteLine(resultXml);

			resultDoc = new XmlDocument();
			resultDoc.LoadXml(resultXml);
		}

		[Test]
		public void SuiteResultHasCategories()
		{
			XmlNodeList categories = resultDoc.SelectNodes("//test-suite[@name=\"MockTestFixture\"]/categories/category");
			Assert.IsNotNull(categories);
			Assert.AreEqual(1, categories.Count);
			Assert.AreEqual("FixtureCategory", categories[0].Attributes["name"].Value);
		}

		[Test]
		public void HasSingleCategory()
		{
			XmlNodeList categories = resultDoc.SelectNodes("//test-case[@name=\"NUnit.Tests.Assemblies.MockTestFixture.MockTest2\"]/categories/category");
			Assert.IsNotNull(categories);
			Assert.AreEqual(1, categories.Count);
			Assert.AreEqual("MockCategory", categories[0].Attributes["name"].Value);
		}

		[Test]
		public void HasSingleProperty()
		{
			XmlNodeList properties = resultDoc.SelectNodes("//test-case[@name=\"NUnit.Tests.Assemblies.MockTestFixture.MockTest2\"]/properties/property");
			Assert.IsNotNull(properties);
			Assert.AreEqual(1, properties.Count);
			Assert.AreEqual("Severity",properties[0].Attributes["name"].Value);
			Assert.AreEqual("Critical",properties[0].Attributes["value"].Value);
		}

		[Test]
		public void HasMultipleCategories()
		{
			XmlNodeList categories = resultDoc.SelectNodes("//test-case[@name=\"NUnit.Tests.Assemblies.MockTestFixture.MockTest3\"]/categories/category");
			Assert.IsNotNull(categories);
			Assert.AreEqual(2, categories.Count);
			ArrayList names = new ArrayList();
			names.Add( categories[0].Attributes["name"].Value );
			names.Add( categories [1].Attributes["name"].Value);
			Assert.IsTrue( names.Contains( "AnotherCategory" ), "AnotherCategory" );
			Assert.IsTrue( names.Contains( "MockCategory" ), "MockCategory" );
		}

		[Test]
		public void HasMultipleProperties()
		{
			XmlNodeList properties = resultDoc.SelectNodes("//test-case[@name=\"NUnit.Tests.Assemblies.MockTestFixture.TestWithManyProperties\"]/properties/property");
			Assert.IsNotNull(properties);
			Assert.AreEqual(3, properties.Count);
			Hashtable hash = new Hashtable();
			foreach( XmlNode property in properties )
				hash.Add( property.Attributes["name"].Value, property.Attributes["value"].Value );

			Assert.AreEqual( "SomeClassName", hash["TargetMethod"] );
			Assert.AreEqual( "5", hash["Size"] );
			Assert.AreEqual( "System.Threading.Thread", hash["TargetType"] );
		}

		[Test]
		public void TestHasEnvironmentInfo() 
		{
			XmlNode sysinfo = resultDoc.SelectSingleNode("//environment");
			Assert.IsNotNull(sysinfo);
			// In theory, we could do some validity checking on the values
			// of the attributes, but that seems redundant.
			Assert.IsNotNull(sysinfo.Attributes["nunit-version"]);
			Assert.IsNotNull(sysinfo.Attributes["clr-version"]);
			Assert.IsNotNull(sysinfo.Attributes["os-version"]);
			Assert.IsNotNull(sysinfo.Attributes["platform"]);
			Assert.IsNotNull(sysinfo.Attributes["cwd"]);
			Assert.IsNotNull(sysinfo.Attributes["machine-name"]);
			Assert.IsNotNull(sysinfo.Attributes["user"]);
			Assert.IsNotNull(sysinfo.Attributes["user-domain"]);
			
		}

		[Test]
		public void TestHasCultureInfo() 
		{
			XmlNode cultureInfo = resultDoc.SelectSingleNode("//culture-info");
			Assert.IsNotNull(cultureInfo);
			Assert.IsNotNull(cultureInfo.Attributes["current-culture"]);
			Assert.IsNotNull(cultureInfo.Attributes["current-uiculture"]);

			String currentCulture = cultureInfo.Attributes["current-culture"].Value;
			String currentUiCulture = cultureInfo.Attributes["current-uiculture"].Value;

			String currentCultureOnMachine = CultureInfo.CurrentCulture.ToString();
			String currentUiCultureOnMachine = CultureInfo.CurrentUICulture.ToString();
			Assert.AreEqual(currentCultureOnMachine, currentCulture,
				"Current Culture node did not contain the same culture name as the machine");
			Assert.AreEqual(currentUiCultureOnMachine, currentUiCulture,
				"Current UI Culture node did not contain the same Culture UI name as the machine");
		}
	}
}
