using System;
using NUnit.Core.Extensibility;

namespace NUnit.Core
{
    public class Services
    {
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
