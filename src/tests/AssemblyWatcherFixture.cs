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
