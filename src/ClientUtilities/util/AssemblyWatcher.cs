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

using System;
using System.IO;
using System.Text;
using System.Timers;

namespace NUnit.Util
{
	/// <summary>
	/// AssemblyWatcher keeps track of a single assembly to see if
	/// it has changed. It incorporates a delayed notification
	/// and uses a standard event to notify any interested parties
	/// about the change. The path to the assembly is provided as
	/// an argument to the event handler so that one routine can
	/// be used to handle events from multiple watchers.
	/// </summary>
	public class AssemblyWatcher
	{
		FileSystemWatcher fileWatcher;
		protected System.Timers.Timer timer;
		FileInfo fileInfo;

		public delegate void AssemblyChangedHandler(String fullPath);
		public event AssemblyChangedHandler AssemblyChangedEvent;

		public AssemblyWatcher(int delay, FileInfo file)
		{
			fileWatcher = new FileSystemWatcher();
			fileWatcher.Path = file.DirectoryName;
			fileWatcher.Filter = file.Name;
			fileWatcher.NotifyFilter = NotifyFilters.Size;
			fileWatcher.Changed+=new FileSystemEventHandler(OnChanged);
			fileWatcher.EnableRaisingEvents = false;

			fileInfo = file;
			
			timer = new System.Timers.Timer( delay );
			timer.AutoReset=false;
			timer.Enabled=false;
			timer.Elapsed+=new ElapsedEventHandler(OnTimer);
		}

		public string Name
		{
			get { return fileInfo.Name; }
		}

		public string DirectoryName
		{
			get { return fileInfo.DirectoryName; }
		}

		public string FullName
		{
			get { return fileInfo.FullName; }
		}


		public void Start()
		{
			fileWatcher.EnableRaisingEvents=true;
		}

		public void Stop()
		{
			fileWatcher.EnableRaisingEvents=false;
		}

		protected void OnTimer(Object source, ElapsedEventArgs e)
		{
			lock(this)
			{
				PublishEvent();
				timer.Enabled=false;
			}
		}
		
		protected void OnChanged(object source, FileSystemEventArgs e)
		{
			if ( timer != null )
			{
				lock(this)
				{
					if(!timer.Enabled)
					{
						timer.Enabled=true;
					}
					timer.Start();
				}
			}
			else
			{
				PublishEvent();
			}
		}
	
		protected void PublishEvent()
		{
			if ( AssemblyChangedEvent != null )
				AssemblyChangedEvent( fileInfo.FullName );
		}
	}
}