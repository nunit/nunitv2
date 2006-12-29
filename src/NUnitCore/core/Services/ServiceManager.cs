using System;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for ServiceManger.
	/// </summary>
	public class ServiceManager
	{
		private ArrayList services = new ArrayList();
		private Hashtable serviceIndex = new Hashtable();

		private static ServiceManager defaultServiceManager = new ServiceManager();

		public static ServiceManager Services
		{
			get { return defaultServiceManager; }
		}

		public void AddService( IService service )
		{
			services.Add( service );
		}

		public IService GetService( Type serviceType )
		{
			IService theService = (IService)serviceIndex[serviceType];
			if ( theService != null )
				return theService;

			foreach( IService service in services )
			{
				// TODO: Does this work on Mono?
				if( serviceType.IsInstanceOfType( service ) )
				{
					serviceIndex[serviceType] = service;
					return service;
				}
			}

			return null;
		}

		public void StopAllServices()
		{
			foreach( IService service in services )
				service.UnloadService();
		}

		private ServiceManager() { }
	}
}
