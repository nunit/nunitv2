using System;
using System.Reflection;
using System.Diagnostics;
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for DomainAgent.
	/// </summary>
	public class DomainAgent : TestAgent
	{
        static Logger log = InternalTrace.GetLogger(typeof(DomainAgent));

		static public DomainAgent CreateInstance(AppDomain targetDomain, TraceLevel traceLevel)
		{
#if NET_2_0
            System.Runtime.Remoting.ObjectHandle oh = Activator.CreateInstance(
                targetDomain,
#else
			System.Runtime.Remoting.ObjectHandle oh = targetDomain.CreateInstance(
#endif
				Assembly.GetExecutingAssembly().FullName,
				typeof(DomainAgent).FullName,
				false, BindingFlags.Default, null, new object[] { traceLevel }, null, null, null);

			object obj = oh.Unwrap();
			Type type = obj.GetType();
			return (DomainAgent)obj;
		}

		public DomainAgent( TraceLevel traceLevel ) : base( Guid.NewGuid() )
		{
            InternalTrace.Initialize("%a_%p.log", traceLevel);
            log.Info("Initializing domain " + AppDomain.CurrentDomain.FriendlyName);

            AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
		}

        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            log.Info("Unloading domain " + AppDomain.CurrentDomain.FriendlyName);
            InternalTrace.Close();
        }

		#region Public Methods - For Client Use
		public override TestRunner CreateRunner(int runnerID)
		{
            log.Info("Creating RemoteTestRunner");
			return new RemoteTestRunner(runnerID);
		}

        public override void Stop()
        {
            InternalTrace.Flush();
        }
		#endregion
	}
}
