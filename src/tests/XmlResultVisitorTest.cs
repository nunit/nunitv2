using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Util;
using System.IO;
using System.Text;
using System.Xml;

namespace NUnit.Tests.Util
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
			TestSuite suite = suiteBuilder.Build(testsDll);

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

		public void TestHasSingleCategory()
		{
			XmlNodeList categories = resultDoc.SelectNodes("//test-case[@name=\"NUnit.Tests.Assemblies.MockTestFixture.MockTest2\"]/categories/category");
			Assert.IsNotNull(categories);
			Assert.AreEqual(1, categories.Count);
			Assert.AreEqual("MockCategory", categories[0].Attributes["name"].Value);
		}

		public void TestHasMultipleCategories()
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
	}
}
