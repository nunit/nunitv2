#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;

namespace NUnit.Tests.Util
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
