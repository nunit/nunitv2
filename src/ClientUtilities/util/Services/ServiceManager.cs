// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using NUnit.Core;

namespace NUnit.Util
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

		public void InitializeServices()
		{
			foreach( IService service in services )
				service.InitializeService();
		}

		public void StopAllServices()
		{
			foreach( IService service in services )
				service.UnloadService();
		}

		private ServiceManager() { }
	}
}
