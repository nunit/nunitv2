/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
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
namespace NUnit.Tests
{
	using System;
	using System.IO;
	using System.Threading;
	using NUnit.Framework;

	/// <summary>
	/// Summary description for AssemblyWatcherFixture.
	/// </summary>
	/// 
	[TestFixture]
	public class AssemblyWatcherFixture
	{
		private string fileName = "temp.txt";
		private FileSystemWatcher watcher;
		private static int changeCount;

		[SetUp]
		public void SetUp()
		{
			ChangeTempFile();

			changeCount = 0;

			watcher = new FileSystemWatcher();
			watcher.Path = ".";
			watcher.Filter = fileName;
			
			watcher.NotifyFilter = NotifyFilters.LastWrite;

			watcher.Changed += new FileSystemEventHandler(OnChanged);

			watcher.EnableRaisingEvents = true;
		}

		[TearDown]
		public void TearDown()
		{
			watcher.EnableRaisingEvents = false;

			FileInfo info = new FileInfo(fileName);
			info.Delete();

			FileInfo tmpInfo = new FileInfo("newTemp.txt");
			tmpInfo.Delete();
		}

		public static void OnChanged(object source, FileSystemEventArgs e) 
		{
			changeCount++;
		}

		[Test]
		public void Create()
		{
			Assertion.AssertEquals(0, changeCount);
		}

		private void ChangeTempFile()
		{
			FileInfo fi = new FileInfo(fileName);

			// create a writer, ready to add entries to the file
			StreamWriter sw = fi.AppendText();

			sw.WriteLine("Add as many lines as you like...");
			sw.WriteLine("Add another line to the output...");
			sw.Flush();
			sw.Close();
		}

		[Test]
		public void ChangeSize()
		{
			ChangeTempFile();

			Yield();

			Assertion.AssertEquals(1, changeCount);
		}

		[Test]
		public void ChangeAttributes()
		{
			FileInfo fi = new FileInfo(fileName);
			FileAttributes attr = fi.Attributes;
			fi.Attributes = FileAttributes.Hidden | attr;

			Yield();

			Assertion.AssertEquals(0, changeCount);
		}

		[Test]
		public void ChangeName()
		{
			FileInfo fi = new FileInfo(fileName);
			fi.CopyTo("newTemp.txt");
			fi.Delete();

			Yield();

			Assertion.AssertEquals(0, changeCount);
		}

		// need a more definitive way to make sure that the background thread has executed
		private void Yield()
		{
			Thread currentThread = Thread.CurrentThread;
			currentThread.Join(100);
		}
	}
}
