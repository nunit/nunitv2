#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
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
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Core
{
	using System;
	using System.Threading;

	/// <summary>
	/// EventPump pulls events out of an EventQUeue and sends
	/// them to a listener. It is used to send events back to
	/// the client without using the CallContext of the test
	/// runner thread.
	/// </summary>
	public class EventPump : IDisposable
	{
		#region Instance Variables
		/// <summary>
		/// The downstream listener to which we send events
		/// </summary>
		EventListener eventListener;
		
		/// <summary>
		/// The queue that holds our events
		/// </summary>
		EventQueue events;
		
		/// <summary>
		/// Thread to do the pumping
		/// </summary>
		Thread pumpThread;

		/// <summary>
		/// Indicator that we are pumping events
		/// </summary>
		private bool pumping;
		
		/// <summary>
		/// Indicator that we are stopping
		/// </summary>
		private bool stopping;

		/// <summary>
		/// If true, stop after sending RunFinished
		/// </summary>
		private bool autostop;
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="eventListener">The EventListener to receive events</param>
		/// <param name="events">The event queue to pull events from</param>
		/// <param name="autostop">Set to true to stop pump after RunFinished</param>
		public EventPump( EventListener eventListener, EventQueue events, bool autostop)
		{
			this.eventListener = eventListener;
			this.events = events;
			this.autostop = autostop;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Returns true if we are pumping events
		/// </summary>
		public bool Pumping
		{
			get { return pumping; }
		}

		/// <summary>
		/// Returns true if a stop is in progress
		/// </summary>
		public bool Stopping
		{
			get { return stopping; }
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Dispose stops the pump
		/// </summary>
		public void Dispose()
		{
			Stop();
		}

		/// <summary>
		/// Start the pump
		/// </summary>
		public void Start()
		{
			if ( !this.Pumping )  // Ignore if already started
			{
				this.pumpThread = new Thread( new ThreadStart( PumpThreadProc ) );
				this.pumpThread.Name = "EventPumpThread";
				this.pumpThread.Start();
			}
		}

		/// <summary>
		/// Tell the pump to stop after emptying the queue.
		/// </summary>
		public void Stop()
		{
			if ( this.Pumping && !this.Stopping ) // Ignore extra calls
			{
				lock( events )
				{
					stopping = true;
					Monitor.Pulse( events ); // In case thread is waiting
				}
				this.pumpThread.Join();
			}
		}
		#endregion

		#region PumpThreadProc
		/// <summary>
		/// Our thread proc for removing items from the event
		/// queue and sending them on. Note that this would
		/// need to do more locking if any other thread were
		/// removing events from the queue.
		/// </summary>
		private void PumpThreadProc()
		{
			pumping = true;
			Monitor.Enter( events );
            try
            {
                while (this.events.Count > 0 || !stopping)
                {
                    while (this.events.Count > 0)
                    {
                        Event e = this.events.Dequeue();
                        e.Send(this.eventListener);
                        if (autostop && e is RunFinishedEvent)
                            stopping = true;
                    }
                    // Will be pulsed if there are any events added
                    // or if it's time to stop the pump.
                    if (!stopping)
                        Monitor.Wait(events);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception in pump thread", ex);
            }
			finally
			{
				Monitor.Exit( events );
				pumping = false;
				stopping = false;
				//pumpThread = null;
			}
		}
		#endregion
	}
}
