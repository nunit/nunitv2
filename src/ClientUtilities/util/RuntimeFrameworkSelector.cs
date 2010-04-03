// ****************************************************************
// Copyright 2010, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************
using System;
using NUnit.Core;

namespace NUnit.Util
{
    public class RuntimeFrameworkSelector : IRuntimeFrameworkSelector
    {
        static Logger log = InternalTrace.GetLogger(typeof(TestRunnerFactory));

        /// <summary>
        /// Selects a target runtime framework for a TestPackage based on
        /// the settings in the package and the assemblies themselves.
        /// The package RuntimeFramework setting may be updated as a 
        /// result and the selected runtime is returned.
        /// </summary>
        /// <param name="package">A TestPackage</param>
        /// <returns>The selected RuntimeFramework</returns>
        public RuntimeFramework SelectRuntimeFramework(TestPackage package)
        {
            RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;
            RuntimeFramework requestedFramework = package.Settings["RuntimeFramework"] as RuntimeFramework;

            log.Debug("Current framework is {0}", currentFramework);
            if (requestedFramework == null)
                log.Debug("No specific framework requested");
            else
                log.Debug("Requested framework is {0}", requestedFramework);

            RuntimeType targetRuntime = requestedFramework == null
                ? RuntimeType.Any 
                : requestedFramework.Runtime;
            Version targetVersion = requestedFramework == null
                ? RuntimeFramework.AnyVersion
                : requestedFramework.FrameworkVersion;

            if (targetRuntime == RuntimeType.Any)
                targetRuntime = currentFramework.Runtime;

            if (targetVersion == RuntimeFramework.AnyVersion)
            {
                foreach (string assembly in package.Assemblies)
                {
                    AssemblyReader reader = new AssemblyReader(assembly);
                    Version v = new Version(reader.ImageRuntimeVersion.Substring(1));
                    if (v > targetVersion) targetVersion = v;
                }
                
                RuntimeFramework checkFramework = new RuntimeFramework(targetRuntime, targetVersion);
                if (!RuntimeFramework.IsAvailable(checkFramework) || NUnitConfiguration.GetTestAgentExePath(targetVersion) == null)
                    if (targetVersion < currentFramework.FrameworkVersion)
                        targetVersion = currentFramework.FrameworkVersion;
            }

            RuntimeFramework targetFramework = new RuntimeFramework(targetRuntime, targetVersion);
            package.Settings["RuntimeFramework"] = targetFramework;

            log.Debug("Test requires {0} framework", targetFramework);

            return targetFramework;
        }
    }
}
