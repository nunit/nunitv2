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
			"  <Config name=\"Debug\" />\r\n" +
			"  <Config name=\"Release\" />\r\n" +
			"</NUnitProject>";
		
		public static readonly string NormalProject =
			"<NUnitProject>\r\n" +
			"  <Settings activeconfig=\"Debug\" />\r\n" +
			"  <Config name=\"Debug\">\r\n" +
			"    <assembly path=\"h:\\bin\\debug\\assembly1.dll\" />\r\n" +
			"    <assembly path=\"h:\\bin\\debug\\assembly2.dll\" />\r\n" +
			"  </Config>\r\n" +
			"  <Config name=\"Release\">\r\n" +
			"    <assembly path=\"h:\\bin\\release\\assembly1.dll\" />\r\n" +
			"    <assembly path=\"h:\\bin\\release\\assembly2.dll\" />\r\n" +
			"  </Config>\r\n" +
			"</NUnitProject>";
	}
}
