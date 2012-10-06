using System;
using System.Collections.Generic;
using System.Threading;

namespace NUnit.Core
{
	public class AsyncSynchronizationContext : SynchronizationContext
    {
        private readonly AutoResetEvent _operationCompleted = new AutoResetEvent(false);
        private readonly IList<Exception> _exceptions = new List<Exception>();

		public IList<Exception> Exceptions
        {
            get { return _exceptions; }
        }

		public override void Send(SendOrPostCallback d, object state)
		{
			throw new InvalidOperationException("Sending to this synchronization context is not supported");
		}

		public override void Post(SendOrPostCallback d, object state)
        {
			try
			{
				d(state);
			}
			catch (Exception e)
			{
				_exceptions.Add(e);
			}
        }

        public override void OperationCompleted()
        {
            base.OperationCompleted();
            _operationCompleted.Set();
        }

        public void WaitForOperationCompleted()
        {
            _operationCompleted.WaitOne();
        }
    }
}