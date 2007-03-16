// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Collections;
using System.Threading;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for EventQueueTests.
	/// </summary>
	[TestFixture]
	public class EventQueueTests
	{
		static readonly Event[] events = {
				new RunStartedEvent( string.Empty, 0 ),
				new SuiteStartedEvent( null ),
				new TestStartedEvent( null ),
				new TestFinishedEvent( null ),
				new SuiteFinishedEvent( null ),
				new RunFinishedEvent( (TestResult)null )
			};

		private void EnqueueEvents( EventQueue q )
		{
			foreach( Event e in events )
				q.Enqueue( e );
		}

		private void SendEvents( EventListener el )
		{
			foreach( Event e in events )
				e.Send( el );
		}

		private void VerifyQueue( EventQueue q )
		{
			for( int index = 0; index < events.Length; index++ )
			{
				Event e = q.Dequeue();
				Assert.AreEqual( events[index].GetType(), e.GetType(), 
					string.Format("Event {0}",index) );
			}
		}

		private void StartPump( EventPump pump, int waitTime )
		{
			pump.Start();
			
			while( waitTime > 0 && !pump.Pumping )
			{
				Thread.Sleep( 10 );
				waitTime -= 10;
			}
		}

		private void StopPump( EventPump pump, int waitTime )
		{
			pump.Stop();
			WaitForPumpToStop( pump, waitTime );
		}

		private void WaitForPumpToStop( EventPump pump, int waitTime )
		{
			while( waitTime > 0 && pump.Pumping )
			{
				Thread.Sleep( 10 );
				waitTime -= 10;
			}
		}

		[Test]
		public void QueueEvents()
		{
			EventQueue q = new EventQueue();
			EnqueueEvents( q );
			VerifyQueue( q );
		}

		[Test]
		public void SendEvents()
		{
			QueuingEventListener el = new QueuingEventListener();
			SendEvents( el );
			VerifyQueue( el.Events );
		}

		[Test]
		public void StartAndStopPumpOnEmptyQueue()
		{
			EventPump pump = new EventPump( NullListener.NULL, new EventQueue(), false );
			StartPump( pump, 1000 ); 
			Assert.IsTrue( pump.Pumping, "Pump failed to start" );
			StopPump( pump, 1000 );
			Assert.IsFalse( pump.Pumping, "Pump failed to stop" );
		}

		[Test]
		public void PumpAutoStopsOnRunFinished()
		{
			EventQueue q = new EventQueue();
			EventPump pump = new EventPump( NullListener.NULL, q, true );
			Assert.IsFalse( pump.Pumping, "Should not be pumping initially" );
			StartPump( pump, 1000 );
			Assert.IsTrue( pump.Pumping, "Pump failed to start" );
			q.Enqueue( new RunFinishedEvent( new Exception() ) );
			WaitForPumpToStop( pump, 1000 );
			Assert.IsFalse( pump.Pumping, "Pump failed to stop" );
		}

		[Test]
		public void PumpEvents()
		{
			EventQueue q = new EventQueue();
			EnqueueEvents( q );
			QueuingEventListener el = new QueuingEventListener();
			EventPump pump = new EventPump( el, q, false );
			Assert.IsFalse( pump.Pumping, "Should not be pumping initially" );
			StartPump( pump, 1000 );
			Assert.IsTrue( pump.Pumping, "Pump should still be running" );
			StopPump( pump, 1000 );
			Assert.IsFalse( pump.Pumping, "Pump should have stopped" );
			VerifyQueue( el.Events );
		}

		[Test]
		public void PumpEventsWithAutoStop()
		{
			EventQueue q = new EventQueue();
			EnqueueEvents( q );
			QueuingEventListener el = new QueuingEventListener();
			EventPump pump = new EventPump( el, q, true );
			pump.Start();
			int tries = 10;
			while( --tries > 0 && q.Count > 0 )
			{
				Thread.Sleep(100);
			}
			VerifyQueue( el.Events );
			Assert.IsFalse( pump.Pumping, "Pump failed to stop" );
		}
	}
}
