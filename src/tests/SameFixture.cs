#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
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
' Portions Copyright  2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, 
' Charlie Poole or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Text;
using NUnit.Framework;

namespace NUnit.Tests.Assertions
{
	[TestFixture]
	public class SameFixture
	{
		[Test]
		public void Same()
		{
			string s1 = "S1";
			Assert.AreSame(s1, s1);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void SameFails()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("S1");
			string s1 = builder.ToString();
			string s2 = "S1";

			Assert.AreEqual(s1, s2);
			Assert.AreSame(s1, s2);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void SameValueTypes()
		{
			int index = 2;
			Assert.AreSame(index, index);
		}
	}
}
