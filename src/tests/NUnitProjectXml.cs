using System;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for NUnitProjectXml.
	/// </summary>
	public class NUnitProjectXml
	{
		public static readonly string EmptyProject = "<NUnitProject />";
		
		public static readonly string EmptyConfigs = 
			"<NUnitProject>\r\n" +
			"  <Settings activeconfig=\"Debug\" />\r\n" +
			"  <Config name=\"Debug\" binpathtype=\"Auto\" />\r\n" +
			"  <Config name=\"Release\" binpathtype=\"Auto\" />\r\n" +
			"</NUnitProject>";
		
		public static readonly string NormalProject =
			"<NUnitProject>\r\n" +
			"  <Settings activeconfig=\"Debug\" />\r\n" +
			"  <Config name=\"Debug\" appbase=\"bin\\debug\" binpathtype=\"Auto\">\r\n" +
			"    <assembly path=\"assembly1.dll\" />\r\n" +
			"    <assembly path=\"assembly2.dll\" />\r\n" +
			"  </Config>\r\n" +
			"  <Config name=\"Release\" appbase=\"bin\\release\" binpathtype=\"Auto\">\r\n" +
			"    <assembly path=\"assembly1.dll\" />\r\n" +
			"    <assembly path=\"assembly2.dll\" />\r\n" +
			"  </Config>\r\n" +
			"</NUnitProject>";
	}
}
