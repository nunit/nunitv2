/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig, Ben Lowery
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/

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
