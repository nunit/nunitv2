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

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class SameFixture : MessageChecker
	{
		[Test]
		public void Same()
		{
			string s1 = "S1";
			Assert.AreSame(s1, s1);
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void SameFails()
		{
			Exception ex1 = new Exception( "one" );
			Exception ex2 = new Exception( "two" );
			expectedMessage =
				"  Expected: same as <System.Exception: one>" + Environment.NewLine +
				"  But was:  <System.Exception: two>" + Environment.NewLine;
			Assert.AreSame(ex1, ex2);
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void SameValueTypes()
		{
			int index = 2;
			expectedMessage =
				"  Expected: same as 2" + Environment.NewLine +
				"  But was:  2" + Environment.NewLine;
			Assert.AreSame(index, index);
		}
	}
}
