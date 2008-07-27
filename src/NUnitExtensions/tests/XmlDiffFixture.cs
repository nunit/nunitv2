using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Extensions.Xml;

namespace NUnit.Framework.Extensions.Xml.Tests
{
#if NET_2_0
	[TestFixture,Category("Net-2.0")]
	public class XmlDiffFixture
	{
		string document = "<Document/>";

		[Test]
		public void TestSameXml()
		{
			XmlConstraint xmlConstraint = new XmlConstraint(document);
			Assert.That(document, xmlConstraint);
		}

		[Test]
		public void TestDifferentRootElement()
		{
			string otherDocument = "<OtherDocument/>";
			XmlConstraint xmlConstraint = new XmlConstraint(otherDocument);
			Assert.That(document, new NotConstraint(xmlConstraint));
		}

		[Test]
		public void TestAdditionalAttribute()
		{
			string otherDocument = "<Document Id='1'/>";
			Assert.That(otherDocument, new XmlConstraint(otherDocument));
			XmlConstraint xmlConstraint = new XmlConstraint(otherDocument);
			Assert.That(document, new NotConstraint(xmlConstraint));
		}

		[Test]
		public void TestAdditionalChild()
		{
			string otherDocument = "<Document><Element /></Document>";
			Assert.That(otherDocument, new XmlConstraint(otherDocument));
			XmlConstraint xmlConstraint = new XmlConstraint(otherDocument);
			Assert.That(document, new NotConstraint(xmlConstraint));
		}

		[Test]
		public void TestDifferentNamespace()
		{
			string otherDocument = "<Document xmlns='urn:other' />";
			Assert.That(otherDocument, new XmlConstraint(otherDocument));
			XmlConstraint xmlConstraint = new XmlConstraint(otherDocument);
			Assert.That(document, new NotConstraint(xmlConstraint));
		}

		[Test]
		public void TestMissingAttribute()
		{
			string expected = "<Document><Child Id='1' Age='6' Name='Carlo' /><Child Id='2' Age='1' Name='Leo' /></Document>";
			Assert.That(expected, new XmlConstraint(expected));
			string actual = "<Document><Child Id='1' Age='6' Name='Carlo' /><Child Id='2' Name='Leo' /></Document>";
			Assert.That(actual, new XmlConstraint(actual));
			XmlConstraint xmlConstraint = new XmlConstraint(actual);
			Assert.That(expected, new NotConstraint(xmlConstraint));
		}

		[Test]
		public void TestMissingElement()
		{
			string expected = "<Document><Child Id='1' Age='6' Name='Carlo' /><Child Id='2' Age='1' Name='Leo' /></Document>";
			Assert.That(expected, new XmlConstraint(expected));
			string actual = "<Document><Child Id='1' Age='6' Name='Carlo' /></Document>";
			Assert.That(actual, new XmlConstraint(actual));
			XmlConstraint xmlConstraint = new XmlConstraint(actual);
			Assert.That(expected, new NotConstraint(xmlConstraint));
		}

		[Test]
		public void TestMismatchedElements()
		{
			string expected = "<Document><Child Id='2' Age='1' Name='Leo' /></Document>";
			string actual = "<Document><Child Id='1' Age='6' Name='Carlo' /></Document>";
			XmlConstraint xmlConstraint = new XmlConstraint(actual);
			Assert.That(expected, new NotConstraint(xmlConstraint));
		}

		[Test]
		public void TestMismatchedText()
		{
			string expected = "<Document>Hello!</Document>";
			Assert.That(expected, new XmlConstraint(expected));
			string actual = "<Document>Goodbye!</Document>";
			Assert.That(actual, new XmlConstraint(actual));

			XmlConstraint xmlConstraint = new XmlConstraint(actual);
			Assert.That(expected, new NotConstraint(xmlConstraint));
		}

		[Test]
		public void TestDeeplyNestedMismatch()
		{
			string expected = "<Document>Hello!<Child><Child><Child></Child></Child></Child></Document>";
			Assert.That(expected, new XmlConstraint(expected));
			string actual = "<Document>Hello!<Child><Child><Child></Child></Child><Mismatch/></Child></Document>";
			Assert.That(actual, new XmlConstraint(actual));
			XmlConstraint xmlConstraint = new XmlConstraint(actual);
			Assert.That(expected, new NotConstraint(xmlConstraint));

		}

		[Test]
		public void TestMultipleMismatches()
		{
			string expected = "<Document><Child Id='1' Age='6' Name='Carlo' /><Child Id='1' Age='1' Name='Leo' /></Document>";
			string actual = "<Document><Child Id='1' Age='6' Name='Carlito' /><Child Id='1' Age='1' Name='Leokins' /></Document>";
			XmlConstraint xmlConstraint = new XmlConstraint(actual);
			Assert.That(expected, new NotConstraint(xmlConstraint));
		}
	}
#endif
}