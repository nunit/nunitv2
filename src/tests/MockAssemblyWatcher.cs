using System;
using System.IO;
using NUnit.Util;

namespace NUnit.Tests
{
	/// <summary>
	/// MockAssemblyWatcher provides way of triggering change
	/// events for test purposes.
	/// </summary>
	public class MockAssemblyWatcher : AssemblyWatcher
	{
		private bool eventPublished = false;

		private DateTime triggerTime;
		private DateTime publishTime;

		public MockAssemblyWatcher(int delay, FileInfo file) : base( delay, file ) { }

		public bool EventPublished
		{
			get { return eventPublished; }
		}

		public int ElapsedTime
		{
			get 
			{ 
				TimeSpan elapsed = publishTime - triggerTime;
				return (int)elapsed.TotalMilliseconds;
			}
		}

		public void TriggerEvent( int delay )
		{
			Delay = delay;
			TriggerEvent( );
		}

		public void TriggerEvent( )
		{
			eventPublished = false;
			triggerTime = DateTime.Now;

			OnChanged( this, 
				new FileSystemEventArgs( WatcherChangeTypes.Changed, 
				DirectoryName, 
				Name ) );
		}

		public int Delay
		{
			get { return (int)timer.Interval; }
			set { timer.Interval = value; }
		}

		protected new void PublishEvent()
		{
			publishTime = DateTime.Now;

			base.PublishEvent();
			eventPublished = true;
		}
	}
}
