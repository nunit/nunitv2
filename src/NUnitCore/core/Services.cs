using System;
using NUnit.Core.Extensibility;

namespace NUnit.Core
{
    public class Services
    {
		#region AddinRegistry
        private static IAddinRegistry addinRegistry;
        public static IAddinRegistry AddinRegistry
        {
            get 
            {
                if (addinRegistry == null)
                {
                    addinRegistry = AppDomain.CurrentDomain.GetData("AddinRegistry") as IAddinRegistry;
                    if (addinRegistry == null)
                        addinRegistry = new AddinRegistry();
                }
                
                return addinRegistry;
            }
        }
		#endregion

		#region ExtensionHost
        private static CoreExtensions coreExtensions;
        public static CoreExtensions CoreExtensions
        {
            get
            {
                if (coreExtensions == null)
                {
                    coreExtensions = new CoreExtensions();
                    coreExtensions.InstallBuiltins();
                    coreExtensions.InstallAddins();
                }

                return coreExtensions;
            }
        }
		#endregion
    }
}
