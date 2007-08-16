using System;
using System.IO;
using System.Reflection;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.ConsoleRunner
{
	/// <summary>
	/// Summary description for Runner.
	/// </summary>
	public class Runner
	{
		[STAThread]
		public static int Main(string[] args)
		{
			ConsoleOptions options = new ConsoleOptions(args);
			
			if(!options.nologo)
				WriteCopyright();

			if(options.help)
			{
				options.Help();
				return ConsoleUi.OK;
			}
			
			if(options.NoArgs) 
			{
				Console.Error.WriteLine("fatal error: no inputs specified");
				options.Help();
				return ConsoleUi.OK;
			}
			
			if(!options.Validate())
			{
				Console.Error.WriteLine("fatal error: invalid arguments");
				options.Help();
				return ConsoleUi.INVALID_ARG;
			}


			// Add Standard Services to ServiceManager
			ServiceManager.Services.AddService( new SettingsService() );
			ServiceManager.Services.AddService( new DomainManager() );
			//ServiceManager.Services.AddService( new RecentFilesService() );
			//ServiceManager.Services.AddService( new TestLoader() );
			ServiceManager.Services.AddService( new AddinRegistry() );
			ServiceManager.Services.AddService( new AddinManager() );

			// Initialize Services
			ServiceManager.Services.InitializeServices();

			try
			{
				ConsoleUi consoleUi = new ConsoleUi();
				return consoleUi.Execute( options );
			}
			catch( FileNotFoundException ex )
			{
				Console.WriteLine( ex.Message );
				return ConsoleUi.FILE_NOT_FOUND;
			}
			catch( Exception ex )
			{
				Console.WriteLine( "Unhandled Exception:\n{0}", ex.ToString() );
				return ConsoleUi.UNEXPECTED_ERROR;
			}
			finally
			{
				if(options.wait)
				{
					Console.Out.WriteLine("\nHit <enter> key to continue");
					Console.ReadLine();
				}
			}
		}

		private static void WriteCopyright()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			System.Version version = executingAssembly.GetName().Version;

			object[] objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
			AssemblyProductAttribute productAttr = (AssemblyProductAttribute)objectAttrs[0];

			objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
			AssemblyCopyrightAttribute copyrightAttr = (AssemblyCopyrightAttribute)objectAttrs[0];

			Console.WriteLine(String.Format("{0} version {1}", productAttr.Product, version.ToString(3)));
			Console.WriteLine(copyrightAttr.Copyright);
			Console.WriteLine();

			Console.WriteLine( "Runtime Environment - " );
			RuntimeFramework framework = RuntimeFramework.CurrentFramework;
			Console.WriteLine( string.Format("   OS Version: {0}", Environment.OSVersion ) );
			Console.WriteLine( string.Format("  CLR Version: {0} ( {1} )",
				Environment.Version,  framework.GetDisplayName() ) );

			Console.WriteLine();
		}
	}
}
