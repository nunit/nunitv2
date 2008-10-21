using System;
using System.Runtime.Remoting.Services;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.Agent
{
	/// <summary>
	/// Summary description for Program.
	/// </summary>
	public class NUnitTestAgent
	{
        static Guid AgentId;
        static string AgencyUrl;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static int Main(string[] args)
		{
            AgentId = new Guid(args[0]);
            AgencyUrl = args[1];

            //System.Diagnostics.Debug.Assert(false);
			// Add Standard Services to ServiceManager
			ServiceManager.Services.AddService( new SettingsService(false) );
            ServiceManager.Services.AddService(new ProjectService());
			ServiceManager.Services.AddService( new DomainManager() );
			//ServiceManager.Services.AddService( new RecentFilesService() );
			//ServiceManager.Services.AddService( new TestLoader() );
			ServiceManager.Services.AddService( new AddinRegistry() );
			ServiceManager.Services.AddService( new AddinManager() );

			// Initialize Services
			ServiceManager.Services.InitializeServices();

			RemoteTestAgent agent = new RemoteTestAgent(AgentId, AgencyUrl);

			try
			{
				if ( agent.Start() )
					agent.WaitForStop();
			}
			finally
			{
				ServiceManager.Services.StopAllServices();
			}

//            Console.WriteLine("Press Enter to Terminate");
//            Console.ReadLine();
			return 0;
		}
	}
}
