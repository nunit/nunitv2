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
			watcher = new AssemblyWatcher(watcherDelayMs,handler,file);
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
			Assertion.AssertEquals(fileName,handler.FileName);			
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

	class CounterEventHandler : FileChangedEventHandler
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
