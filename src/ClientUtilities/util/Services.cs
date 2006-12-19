using System;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for Services.
	/// </summary>
	public class Services : NUnit.Core.Services
	{
		#region DomainManager
		private static DomainManager domainManager;
		public static DomainManager DomainManager
		{
			get
			{
				if ( domainManager == null )
					domainManager = new DomainManager();

				return domainManager;
			}
		}
		#endregion
	}
}
