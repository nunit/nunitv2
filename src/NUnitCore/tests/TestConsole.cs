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
using NUnit.Framework;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// This test is designed to check that console output is being passed
	/// correctly accross the AppDomain boundry.  Non-remotable objects should
	/// be converted to a string before being passed accross.
	/// </summary>
	[TestFixture]
	public class TestConsole
	{
		[Test]
		public void ConsoleWrite()
		{
			Console.Write("I am a 'String' object.");
			Console.WriteLine();
			Console.Write(new TestSerialisable());
			Console.WriteLine();
			Console.Write(new TestMarshalByRefObject());
			Console.WriteLine();
			System.Diagnostics.Trace.WriteLine( "Output from Trace", "NUnit" );
			Console.Write(new TestNonRemotableObject());
			Console.WriteLine();
			Console.Error.WriteLine( "This is from Console.Error" );
		}

		[Test]
		public void ConsoleWriteLine()
		{
			Console.WriteLine("I am a 'String' object.");
			Console.WriteLine(new TestSerialisable());
			Console.WriteLine(new TestMarshalByRefObject());
			Console.WriteLine(new TestNonRemotableObject());
		}

		[Serializable] 
			public class TestSerialisable
		{
			override public string ToString()
			{
				return "I am a 'Serializable' object.";
			}
		}

		public class TestMarshalByRefObject : MarshalByRefObject
		{
			override public string ToString()
			{
				return "I am a 'MarshalByRefObject' object.";
			}
		}

		public class TestNonRemotableObject
		{
			override public string ToString()
			{
				return "I am a non-remotable object.";
			}
		}
	}
}
