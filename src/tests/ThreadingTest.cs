#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion


using System;
using System.Threading;
using NUnit.Framework;

namespace NUnit.Tests
{
	/// <summary>
	/// This test attempts to expose the bug shown in bug #591350 on source forge.
	/// The bug states that creating and running a thread causes the runner to crash.
	/// </summary>
	[TestFixture]
	public class ThreadingTest
	{
		/// <summary>
		/// Attempt to create and run a thread.  
		/// Wait for 200 millis for the thread to complete
		/// </summary>
		[Test]
		public void CreateAndRunThread() 
		{
			Runner r = new Runner();
			Thread thread = new Thread(new ThreadStart(r.Run));
			thread.Start();
			thread.Join(100);
			Assertion.Assert("Thread did not run.", r.WasRun);
		}

		/// <summary>
		/// Little private class to instantiate and run on a thread
		/// </summary>
		private class Runner
		{
			public bool WasRun = false;
			public void Run() 
			{
				WasRun = true;
			}
		}
	}
}
