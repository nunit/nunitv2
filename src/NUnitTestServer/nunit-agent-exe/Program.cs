using System;
using System.Runtime.Remoting.Services;
using NUnit.Util;

[assembly: log4net.Config.XmlConfigurator(Watch=true)]

namespace NUnit.Agent
{
	/// <summary>
	/// Summary description for Program.
	/// </summary>
	public class NUnitTestAgent
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
			System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static int Main(string[] args)
		{
			log4net.GlobalContext.Properties["PID"] = System.Diagnostics.Process.GetCurrentProcess().Id;
			log.Info("Starting");

			// Add Standard Services to ServiceManager
			ServiceManager.Services.AddService( new SettingsService() );
			ServiceManager.Services.AddService( new DomainManager() );
			//ServiceManager.Services.AddService( new RecentFilesService() );
			//ServiceManager.Services.AddService( new TestLoader() );
			ServiceManager.Services.AddService( new AddinRegistry() );
			ServiceManager.Services.AddService( new AddinManager() );

			// Initialize Services
			ServiceManager.Services.InitializeServices();

			RemoteTestAgent agent = new RemoteTestAgent(args[0]);

			try
			{
				agent.Start();
				agent.WaitForStop();
			}
			finally
			{
				ServiceManager.Services.StopAllServices();
			}

			log.Info("Exiting");
            //Console.WriteLine("Press Enter to Terminate");
            //Console.ReadLine();
			return 0;
		}
	}
}
