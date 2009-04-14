using System;
using System.IO;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Reflection;
using System.Threading;

using log4net;
using log4net.Config;

using PNUnit.Framework;

using NUnit.Util;

namespace PNUnit.Agent
{
    class Agent
    {
        [STAThread]
        static void Main(string[] args)
        {
            AgentConfig config = new AgentConfig();

            // read --daemon
            bool bDaemonMode = ReadFlag(args, "--daemon");

            string configfile = ReadArg(args);

            int port = -1;

            string pathtoassemblies = ReadArg(args);

            if (pathtoassemblies != null)
            {
                port = int.Parse(configfile);
                configfile = null;
            }

            // Load the test configuration file
            if (pathtoassemblies == null && configfile == null)
            {
                Console.WriteLine("Usage: agent [configfile | port path_to_assemblies] [--daemon]");
                return;
            }

            if (configfile != null)
            {
                config = AgentConfigLoader.LoadFromFile(configfile);

                if (config == null)
                {
                    Console.WriteLine("No agent.conf file found");
                }
            }
            else
            {
                config.Port = port;
                config.PathToAssemblies = pathtoassemblies;
            }

            ConfigureLogging();

            // initialize NUnit services
            // Add Standard Services to ServiceManager
            ServiceManager.Services.AddService(new SettingsService());
            ServiceManager.Services.AddService(new DomainManager());
            ServiceManager.Services.AddService(new ProjectService());

            // Initialize Services
            ServiceManager.Services.InitializeServices();


            PNUnitAgent agent = new PNUnitAgent();
            agent.Run(config, bDaemonMode);
        }

        private static bool ReadFlag(string[] args, string flag)
        {
            for (int i = args.Length - 1; i >= 0; --i)
                if (args[i] == flag)
                {
                    args[i] = null;
                    return true;
                }

            return false;
        }

        private static string ReadArg(string[] args)
        {
            for (int i = 0; i < args.Length; ++i)
                if (args[i] != null)
                {
                    string result = args[i];
                    args[i] = null;
                    return result;
                }
            return null;
        }

        private static void ConfigureLogging()
        {
            string log4netpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "agent.log.conf");
            XmlConfigurator.Configure(new FileInfo(log4netpath));
        }
    }

    public class PNUnitAgent : MarshalByRefObject, IPNUnitAgent
    {


        private AgentConfig mConfig;
        private static readonly ILog log = LogManager.GetLogger(typeof(PNUnitAgent));

        #region IPNUnitAgent

        public void RunTest(PNUnitTestInfo info)
        {
            log.InfoFormat("RunTest called for Test {0}, AssemblyName {1}, TestToRun {2}",
                info.TestName, info.AssemblyName, info.TestToRun);

            new PNUnitTestRunner(info, mConfig).Run();
        }

        #endregion

        #region MarshallByRefObject
        // Lives forever
        public override object InitializeLifetimeService()
        {
            return null;
        }
        #endregion

        public void Run(AgentConfig config, bool bDaemonMode)
        {
            mConfig = config;
            // init remoting
            BinaryClientFormatterSinkProvider clientProvider =
                new BinaryClientFormatterSinkProvider();
            BinaryServerFormatterSinkProvider serverProvider =
                new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel =
                System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

            IDictionary props = new Hashtable();
            props["port"] = mConfig.Port;
            string s = System.Guid.NewGuid().ToString();
            props["name"] = s;
            props["typeFilterLevel"] = TypeFilterLevel.Full;
            try
            {
                TcpChannel chan = new TcpChannel(
                    props, clientProvider, serverProvider);

                log.InfoFormat("Registering channel on port {0}", mConfig.Port);
#if NET_2_0
                ChannelServices.RegisterChannel(chan, false);
#else
                ChannelServices.RegisterChannel(chan);
#endif
            }
            catch (Exception e)
            {
                log.InfoFormat("Can't register channel.\n{0}", e.Message);
                return;
            }

            // publish
            RemotingServices.Marshal(this, PNUnit.Framework.Names.PNUnitAgentServiceName);

            // otherwise in .NET 2.0 memory grows continuosly
            FreeMemory();

            if (bDaemonMode)
            {
                // wait continously
                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            else
            {
                Console.ReadLine();
            }

            //RemotingServices.Disconnect(this);
        }

        private void FreeMemory()
        {
            GC.GetTotalMemory(true);
        }

    }

}
