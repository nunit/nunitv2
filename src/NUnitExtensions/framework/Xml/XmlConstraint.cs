using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using NUnit.Framework.Constraints;

namespace NUnit.Framework.Extensions.Xml
{
#if NET_2_0
	/// <summary>
	/// Compares XML against the expected XML that was passed to its constructor.
	/// </summary>
	public class XmlConstraint : Constraint
	{
		// Created from the expected XML fragment or document. 
		private XPathNavigator expected;
		// The resolver that lets us inject an XML fragment into the XSLT's call to xsl:document().
		private XmlResolver resolver;
		private XslCompiledTransform transform = CompileTransform();
		// The first mismatch caught by the XSLT.
		private XPathNodeIterator mismatch;

		/// <summary>
		/// Compares XML against the expected XML that was passed to its constructor.
		/// </summary>
		/// <param name="xml">the expected XML, as document or fragment</param>
		/// <remarks>The xml will not be validated until the constraint is applied.</remarks>
		public XmlConstraint(string xml)
		{
			resolver = new StringResolver(xml);
		}

		/// <summary>
		/// Compares XML against the expected XML that was passed to its constructor.
		/// </summary>
		/// <param name="reader">the expected XML as an appropriately configured reader</param>
		/// <remarks>The xml will not be validated until the constraint is applied.</remarks>
		public XmlConstraint(XmlReader reader)
		{
			resolver = new XmlReaderResolver(reader);
		}

		private static XslCompiledTransform CompileTransform()
		{
            Type type = typeof(XmlConstraint);
		    Stream stream = type.Assembly.GetManifestResourceStream(type, "xmlDiff.xslt");
		    XmlReader xmlReader = XmlReader.Create(stream);
            //XmlReader xmlReader = XmlReader.Create(new StringReader(Resources.XmlDiff));
			XslCompiledTransform transform = new XslCompiledTransform();
			XsltSettings settings = new XsltSettings();
			settings.EnableDocumentFunction = true;
			transform.Load(xmlReader, settings, new XmlUrlResolver());
			return transform;
		}

		/// <summary>
		/// See base class documentation. This implementation compares expected against
		/// actual XML, by means of an XSL transform. 
		/// </summary>
		/// <param name="actual">The actual XML, as string</param>
		/// <returns>the results of an XML diff, as Boolean</returns>
		public override bool Matches(object actual)
		{
			StringBuilder output = new StringBuilder();
			XmlWriter xmlWriter = XmlWriter.Create(output);
			XsltArgumentList argumentList = new XsltArgumentList();
			XmlReader actualReader = XmlReader.Create(new StringReader((string) actual));
			// Transform the actual XML. The XSLT will attempt to load the expected XML
			// from a document() call, by calling our resolver.
			transform.Transform(actualReader, argumentList, xmlWriter, this.resolver);
			// Construct a read-only representation of the product.
			XPathDocument outputDocument = new XPathDocument(new StringReader(output.ToString()));
			XPathNavigator navigator = outputDocument.CreateNavigator();
			XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
			// Now find *all* mismatches.
			manager.AddNamespace("d", "urn:nunit.org:diff");
			this.mismatch = navigator.Select("//d:mismatch", manager);
			if (this.mismatch.Count == 0)
				return true;
			// Move the iterator ahead. At the moment, we're using only the first mismatch. 
			this.mismatch.MoveNext();
			this.expected = mismatch.Current.SelectSingleNode("d:expected", manager);
			Debug.Assert(expected != null);
			this.actual = mismatch.Current.SelectSingleNode("d:actual", manager);
			Debug.Assert(this.actual != null);
			return false;
		}

		public override void WriteActualValueTo(MessageWriter writer)
		{
			writer.WriteActualValue(((XPathNavigator) actual).InnerXml);
		}

		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WriteExpectedValue(expected.InnerXml);
		}
	}

	/// <summary>
	/// An implementation of XmlResolver that resolves any URI as the XmlReader with which
	/// it was initialized. This allows an arbitrary fragment of XML to be injected into
	/// an XSLT that calls document(). In the present implementation, it is used to inject
	/// XML from an embedded resource.
	/// </summary>
	internal class XmlReaderResolver : XmlResolver
	{
		private XmlReader reader;

		public XmlReaderResolver(XmlReader reader)
		{
			this.reader = reader;
		}

		public override ICredentials Credentials
		{
			set { throw new NotImplementedException(); }
		}

		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			return reader;
		}
	}

	/// <summary>
	/// An implementation of XmlResolver that resolves any URI as the string with which
	/// it was initialized. This allows an arbitrary fragment of XML to be injected into
	/// an XSLT that calls document().
	/// </summary>
	internal class StringResolver : XmlResolver
	{
		private string xml;

		public StringResolver(string xml)
		{
			Debug.Assert(xml != null);
			this.xml = xml;
		}

		public override ICredentials Credentials
		{
			set { throw new NotImplementedException(); }
		}

		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			return XmlReader.Create(new StringReader(xml));
		}
	}
#endif
}