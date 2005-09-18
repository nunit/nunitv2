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
				new RunStartedEvent( new Test[0] ),
				new SuiteStartedEvent( null ),
				new TestStartedEvent( null ),
				new TestFinishedEvent( null ),
				new SuiteFinishedEvent( null ),
				new RunFinishedEvent( new TestResult[0] )
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
			pump.Start(); Thread.Sleep( 100 );
			Assert.IsTrue( pump.Pumping, "Pump failed to start" );
			pump.Stop(); Thread.Sleep( 100 );
			Assert.IsFalse( pump.Pumping, "Pump failed to stop" );
		}

		[Test]
		public void PumpAutoStopsOnRunFinished()
		{
			EventQueue q = new EventQueue();
			EventPump pump = new EventPump( NullListener.NULL, q, true );
			Assert.IsFalse( pump.Pumping, "Should not be pumping initially" );
			pump.Start(); Thread.Sleep( 100 );
			Assert.IsTrue( pump.Pumping, "Pump failed to start" );
			q.Enqueue( new RunFinishedEvent( new Exception() ) ); Thread.Sleep(100);
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
			pump.Start(); Thread.Sleep(100);
			Assert.IsTrue( pump.Pumping, "Pump should still be running" );
			pump.Stop(); Thread.Sleep(100);
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
