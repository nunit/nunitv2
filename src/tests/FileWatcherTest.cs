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
using System.IO;
using NUnit.Framework;
using System.Text;
using System.Timers;
using NUnit.Util;

namespace NUnit.Tests
{
	[TestFixture]
	public class FileWatcherTest
	{
		private FileInfo file;
		private AssemblyWatcher watcher;
		private CounterEventHandler handler;
		private static int watcherDelayMs = 1000;
		private static readonly String fileName = "temp.txt";
		private static readonly String tempFileName = "newTempFile.txt";

		[SetUp]
		public void CreateFile()
		{
			file = new FileInfo(fileName);
			FileStream stream = file.Create();
			stream.Close();

			handler = new CounterEventHandler();
			watcher = new AssemblyWatcher(watcherDelayMs, file);
			watcher.AssemblyChangedEvent += new AssemblyWatcher.AssemblyChangedHandler( handler.OnChanged );
			watcher.Start();
		}

		[TearDown]
		public void DeleteFile()
		{
			watcher.Stop();
			FileInfo fileInfo = new FileInfo(fileName);
			fileInfo.Delete();

			FileInfo temp = new FileInfo(tempFileName);
			if(temp.Exists) temp.Delete();
		}

		[Test]
		public void TestManyFrequentEvents()
		{
			for(int i=0; i<3; i++)
			{
				StreamWriter writer =  file.AppendText();
				writer.WriteLine("Data");
				writer.Flush();
				writer.Close();
				System.Threading.Thread.Sleep(250);
			}
			WaitForTimerExpiration();
			Assertion.AssertEquals(1,handler.Counter);
			Assertion.AssertEquals(file.FullName,handler.FileName);			
		}

		[Test]
		public void ChangeAttributes()
		{
			FileInfo fi = new FileInfo(fileName);
			FileAttributes attr = fi.Attributes;
			fi.Attributes = FileAttributes.Hidden | attr;

			WaitForTimerExpiration();
			Assertion.AssertEquals(0, handler.Counter);
		}

		[Test]
		public void CopyFile()
		{
			FileInfo fi = new FileInfo(fileName);
			fi.CopyTo(tempFileName);
			fi.Delete();

			WaitForTimerExpiration();
			Assertion.AssertEquals(0, handler.Counter);
		}

		private static void WaitForTimerExpiration()
		{
			System.Threading.Thread.Sleep(watcherDelayMs + 1000);
		}

	}

	class CounterEventHandler
	{
		int counter;
		String fileName;
		public int Counter
		{
			get{ return counter;}
		}
		public String FileName
		{
			get{ return fileName;}
		}

		public void OnChanged(String fullPath)
		{
			fileName = fullPath;
			counter++;
		}
	}
}
